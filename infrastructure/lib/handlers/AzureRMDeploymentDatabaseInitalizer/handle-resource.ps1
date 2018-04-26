Param(
    [parameter(Mandatory=$true)]
    [ValidateSet('deploy', 'destroy', 'info')]
    [string] $action,

    [parameter(Mandatory=$true)]
    [string] $name,

    [parameter(Mandatory=$true)]
    [hashtable] $resource
)

## build path to the base project folder
$BASE_PATH = resolve-path (join-path (split-path -parent $MyInvocation.MyCommand.path) "../../..")

## figure out some info about ourself
$SELF_PATH = split-path -parent $MyInvocation.MyCommand.Path
$SELF_NAME = split-path -leaf $MyInvocation.PSCommandPath

## add utilty_scripts to our path
$SCRIPTS_PATH = (join-path $BASE_PATH "lib/utility_scripts")
if($ENV:PATH -notcontains $SCRIPTS_PATH) {
    $ENV:PATH  = ("{0}{1}{2}" -f $ENV:PATH,[IO.Path]::PathSeparator,$SCRIPTS_PATH)
}

function deploy {
    Param(
        [parameter(Mandatory=$true)]
        [string] $name,

        [parameter(Mandatory=$true)]
        [hashtable] $resource
    )
    $deployment = Get-AzureRmResourceGroupDeployment -resourcegroup $resource.resource_group -name $resource.deployment_name
    $dbHost = $deployment.outputs[$resource.database_server_name_key].value
    $dbName = $deployment.outputs[$resource.database_name_key].value
    $dbUser = $deployment.outputs[$resource.database_admin_username_key].value
    $dbPass = $deployment.outputs[$resource.database_admin_password_key].value
    $script = (resolve-path (join-path $BASE_PATH $resource.initalization_script))

    ## add self to firewall (use AZ-CLI for cross platform)
    $myIp     = (invoke-restmethod https://api.ipify.org?format=json).ip
    $ruleName = ("dbinit_{0}" -f $(new-guid).guid.replace('-','').substring(0,5))
    write-info ("Creating database firewall rule '{0}' to allow IP {1} to database..." -f $ruleName,$myIp)
    az sql server firewall-rule create `
        --resource-group $resource.resource_group `
        --server $dbHost.split('.')[0] `
        --name $ruleName `
        --start-ip-address $myIp `
        --end-ip-address $myIp

    ## run db initialization script
    write-info ("Initializing database {0}/{1}..." -f $dbHost,$dbName)
    foreach($script in $resource.initalization_scripts) {
        $scriptPath = (resolve-path (join-path $BASE_PATH $script))
        write-info ("Executing script: {0}" -f $script)
        sqlcmd -S $dbHost -d $dbName -U $dbUser -P $dbPass -i $scriptPath
        if($LASTEXITCODE -eq 0) { 
            $dbInitSuccess = $true
            write-info ("Execution of script {0} was successful." -f $script)
        } else {
            $dbInitSuccess = $false
        }
    }
    ## remove self from firewall
    write-info ("Removing database firewall rule '{0}'..." -f $ruleName,$myIp)
    az sql server firewall-rule delete `
        --resource-group $resource.resource_group `
        --server $dbHost.split('.')[0] `
        --name $ruleName

    if(! $dbInitSuccess) { exit 1 }
}

function destroy {
    Param(
        [parameter(Mandatory=$true)]
        [string] $name,

        [parameter(Mandatory=$true)]
        [hashtable] $resource
    )
    write-warning "$SELF_PATH does not implement the 'destroy' action."
}

function info {
    Param(
        [parameter(Mandatory=$true)]
        [string] $name,

        [parameter(Mandatory=$true)]
        [hashtable] $resource
    )
    write-warning "$SELF_PATH does not implement the 'info' action." 
}

switch($action) {
    'deploy' {
        deploy -name $name -resource $resource
    }
    'destroy' {
        destroy -name $name -resource $resource
    }
    'info' {
        info -name $name -resource $resource
    }
}
