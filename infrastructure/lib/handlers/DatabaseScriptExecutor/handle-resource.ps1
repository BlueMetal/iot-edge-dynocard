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

    ## get the database connection info from deployment output
    if(($resource.connection_info -is [hashtable]) -and ($resource.connection_info.type -eq "AzureRMDeploymentOutputDatabaseConnectionInfo")) {
        $connInfo = $resource.connection_info
        $deployment = Get-AzureRmResourceGroupDeployment -resourcegroup $connInfo.resource_group -name $connInfo.deployment_name
        
        if (($connInfo.hostname_key -ne "")) {
            $dbHost = $deployment.outputs[$connInfo.hostname_key].value
        }

        if (($connInfo.database_key -ne "")) {
            $dbName = $deployment.outputs[$connInfo.database_key].value
        }

        if (($connInfo.username_key -ne "")) {
            $dbUser = $deployment.outputs[$connInfo.username_key].value
        }

        if (($connInfo.password_key -ne "")) {
            $dbPass = $deployment.outputs[$connInfo.password_key].value
        }
    }
    
    if(($resource.hostname -is [hashtable]) -and ($resource.hostname.type -eq "AzureRMDeploymentOutput")) {
        $deployment = Get-AzureRmResourceGroupDeployment `
            -resourcegroup $resource.hostname.resource_group `
            -name $resource.hostname.deployment_name
        $dbHost = $deployment.outputs[$resource.hostname.key].value
    } elseif (($resource.hostname -is [string]) -and ($resource.hostname -ne "")) {
        $dbHost = $resource.hostname
    } else {
        write-error "Database hostname not found or null/empty."
    }

    ## get the database name
    if(($resource.database -is [hashtable]) -and ($resource.database.type -eq "AzureRMDeploymentOutput")) {
        $deployment = Get-AzureRmResourceGroupDeployment `
            -resourcegroup $resource.database.resource_group `
            -name $resource.database.deployment_name
        $dbName = $deployment.outputs[$resource.database.key].value
    } elseif (($resource.database -is [string]) -and ($resource.database -ne "")) {
        $dbName = $resource.database
    } else {
        write-error "Database instance name not found or null/empty."
    }

    ## get the database username
    if(($resource.username -is [hashtable]) -and ($resource.username.type -eq "AzureRMDeploymentOutput")) {
        $deployment = Get-AzureRmResourceGroupDeployment `
            -resourcegroup $resource.username.resource_group `
            -name $resource.username.deployment_name
        $dbUser = $deployment.outputs[$resource.username.key].value
    } elseif (($resource.username -is [string]) -and ($resource.username -ne "")) {
        $dbUser = $resource.username
    } else {
        write-error "Database username not found or null/empty."
    }

    ## get the database password
    if(($resource.password -is [hashtable]) -and ($resource.password.type -eq "AzureRMDeploymentOutput")) {
        $deployment = Get-AzureRmResourceGroupDeployment `
            -resourcegroup $resource.password.resource_group `
            -name $resource.password.deployment_name
        $dbPass = $deployment.outputs[$resource.password.key].value
    } elseif (($resource.password -is [string]) -and ($resource.password -ne "")) {
        $dbPass = $resource.password
    } else {
        write-error "Database password not found or null/empty."
    }

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
    foreach($script in $resource.scripts) {
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
