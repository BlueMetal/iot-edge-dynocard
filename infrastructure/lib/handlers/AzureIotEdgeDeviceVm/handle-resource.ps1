## example resource:
# {
#     "type" : "AzureIotEdgeDeviceVm",
#     "resource_group":"iot-dynocard-dev-device2",
#     "location"   : "eastus",
#     "deviceName" : "iot-edge-dev-1",
#     "deviceHub"  : "iot-edge-dev-hub",
#     "deviceUsername" : "edgeuser",
#     "devicePassword" : "tzi61HcJjczlD6wBAvOM37PBPCUJkiTurwEV",
#     "vnetName"   :  "iot-edge-dev-vnet",
#     "subnetName" : "edge_devices"
# }
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

## if the device resource_group isn't set, set it equal to the device name
if([string]::IsNullOrEmpty($resource.resource_group)) {
    $resource.resource_group = $resource.deviceName
}

function deploy {
    Param(
        [parameter(Mandatory=$true)]
        [string] $name,

        [parameter(Mandatory=$true)]
        [hashtable] $resource
    )
    ## check that the az cli has the azure-cli-iot-ext extension
    $extension = (az extension show --name azure-cli-iot-ext --output json | convertfrom-json)
    if(-not $extension) {
        write-info "Adding Azure-CLI IoT Extension..."
        $extension = (az extension add --name azure-cli-iot-ext --output json | convertfrom-json)
        write-info ("Successfully added {0} v{1}" -f $extension.metadata.name,$extension.metadata.version)
    }

    ## check if device-identity exists; if not, create one
    $identity = (az iot hub device-identity show --hub-name $resource.deviceHub --device-id $resource.deviceName --output json | ConvertTo-Json)
    if($identity -eq $null) {
        ## create an identity for the IoT device
        write-info ("Creating device identity in hub {0} for device {1}" -f $resource.deviceHub,$resource.deviceName)
        $identity = (az iot hub device-identity create --hub-name $resource.deviceHub --device-id $resource.deviceName --edge-enabled --output json | convertfrom-json)
    }

    ## assign the device's configuration
    if(test-path $resource.deviceConfig) {
        $config = (az iot hub apply-configuration --hub-name $resource.deviceHub --device-id $resource.deviceName --content $resource.deviceConfig --output json | convertFrom-json)
    } else {
        write-error ("Unable to find config file '{0}' in '{1}'" -f $resource.deviceConfig,$PWD)
    }

    ## get the connection string for the device
    $connStr =  (az iot hub device-identity show-connection-string --hub-name $resource.deviceHub --device-id $resource.deviceName --output json | convertFrom-json).cs

    ## expand the cloud-init template and convert it to base64
    $template   = resolve-path (join-path $SELF_PATH 'edge_device_cloudinit.yaml')
    $customData = get-content $template | expand-template.ps1 -tokens @{connstr=$connStr}
    $customData =  $customData | convertto-base64.ps1

    ## create the resource via ARM
    $parameters = @{
        resourceName     = ($resource.deviceName -replace '[_.]','-')
        adminUsername    = $resource.deviceUsername
        adminPassword    = $resource.devicePassword
        dnsLabelPrefix   = ($resource.deviceName -replace '[-_.]','')
        vnetName         = $resource.vnetName
        vnetGroup        = $resource.vnetGroup
        subnetName       = $resource.subnetName
        base64CustomData = $customData
    }


    $template = resolve-path (join-path $SELF_PATH 'iot_edge_device.json')
    $deployment = deploy-template `
        -name $name `
        -resourceGroup $resource.resource_group `
        -location $resource.location `
        -template $template `
        -parameters $parameters
}

function destroy {
    Param(
        [parameter(Mandatory=$true)]
        [string] $name,

        [parameter(Mandatory=$true)]
        [hashtable] $resource
    )

    write-info "Destroying $name..."
    # delete the device's identity
    $identity = (az iot hub device-identity delete --hub-name $resource.deviceHub --device-id $resource.deviceName --output json | ConvertTo-Json)

    # delete the device's resource group 
    $result = Remove-AzureRmResourceGroup -Name $resource.resource_group -Force
    if($result) {
        write-info ("Destruction of '{0}' successful." -f $name)
    } else {
        write-error ("An error occrred while trying to destroy '{0}'." -f $name)
    }
}

function info {
    return (Get-AzureRmResourceGroupDeployment -ResourceGroupName $resource.resource_group -Name $name).outputs
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