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
    $ENV:PATH  = ("{0}:{1}" -f $ENV:PATH,$SCRIPTS_PATH)
}

function deploy {
    Param(
        [parameter(Mandatory=$true)]
        [string] $name,

        [parameter(Mandatory=$true)]
        [hashtable] $resource
    )
    $deployment = deploy-template `
        -name $name `
        -resourceGroup $resource.resource_group `
        -location $resource.location `
        -template $resource.template `
        -parameters $resource.parameters
    write-host $deployment.ProvisioningState
}

function destroy {
    Param(
        [parameter(Mandatory=$true)]
        [string] $name,

        [parameter(Mandatory=$true)]
        [hashtable] $resource
    )
    
    write-info "Destroying deployment '$name'..."
    $result = Remove-AzureRmResourceGroupDeployment -name $name -resourceGroup $resource.resource_group
    if($result) { write-info "Destruction of deployment '$name' succeeded" }

    write-info ("Destroying resource group '{0}'..." -f $resource.resource_group)
    Remove-AzureRmResourceGroup -name $resource.resource_group -Force
    if($result) { write-info ("Destruction of resource group '{0}' succeeded." -f $resource.resource_group)}
}

function info {
    Param(
        [parameter(Mandatory=$true)]
        [string] $name,

        [parameter(Mandatory=$true)]
        [hashtable] $resource
    )
    Get-AzureRmResourceGroupDeployment -name $name -resourceGroup $resource.resource_group
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
