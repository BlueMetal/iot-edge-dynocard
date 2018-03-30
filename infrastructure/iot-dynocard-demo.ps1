Param(
    [parameter(Mandatory=$true)]
    [ValidateSet('deploy', 'destroy', 'info')]
    [string] $action,

    [parameter(Mandatory=$true)]
    [string] $resource_name
)

$CONFIG_FILE_NAME = "config.json"
$HANDLER_LIB      = "./lib/handlers"

## output/verbosity controls
$ErrorActionPreference = "Stop"
$InformationPreference = "Continue"
$DebugPreference = "SilentlyContinue"

## create aliases
New-Alias -Force -Name "Write-Info" Write-Information

## Functions
function ConvertTo-Hashtable {
    param (
        [Parameter(ValueFromPipeline)]
        $InputObject
    )

    process {
        if($null -eq $InputObject) {
            return $null
        }
        elseif($InputObject -is [hashtable]) {
            return $InputObject
        }
        elseif($InputObject -is [System.Collections.IEnumerable] -and $InputObject -isnot [string]) {
            $collection = @(
                $InputObject | %{ ConvertTo-Hashtable $_ }
            )
            Write-Output -NoEnumerate $collection
        }
        elseif($InputObject -is [psobject]) {
            $hash = @{}
            $InputObject.PSObject.Properties | %{
                $hash[$_.Name] = ConvertTo-Hashtable $_.Value
            }
            $hash
        }
        elseif($InputObject -is [System.Collections.IEnumerable] -and $InputObject -is [string]) {
            [regex] $psStringTemplate = "^PS\[(.*)\]$"
            $match = $psStringTemplate.match($InputObject)
            if($match.Success) {
                iex $match.groups[1].value
            } else {
                $InputObject
            }
        }
        else {
            $InputObject
        }
    }
}

function handle-resource {
    Param(
        [parameter(Mandatory=$true)]
        [ValidateSet('deploy', 'destroy', 'info')]
        [string] $action,

        [parameter(Mandatory=$true)]
        [string] $name,

        [parameter(Mandatory=$true)]
        [object] $resource
    )

    ## find the appropriate resource handler for the type
    $handlerPath   = (join-path $HANDLER_LIB ('{0}' -f $resource.type))
    $handlerScript = (join-path $handlerPath 'handle-resource.ps1')
    if(test-path $handlerScript) {
        [hashtable] $resource = $resource | ConvertTo-Hashtable
        & $handlerScript -action $action -name $name -resource $resource

    } else {
        write-error ("No handler of type '{0}' found for resource '{1}'." -f $resource.type,$name)
    }
}


## Script execution starts here...
Write-Info "Reading config..."
$config = get-content (join-path $PSScriptRoot $CONFIG_FILE_NAME) | ConvertFrom-Json

## check user input
$resource_names = $config.resources.PSObject.Properties.Name
if( ($resource_name -ne "all") -and ($resource_names -notcontains $resource_name)){
    Write-Error "Null or unknown resource name specified. Allowed values are '$($resource_names -join ''', ''')', and 'all'"
    exit
}

## log in with Service Principal
Write-Info "Logging into Azure...";
if($config.client_id -and $config.client_secret) {
    $pass = $config.client_secret | ConvertTo-SecureString -AsPlainText -Force
    $cred = new-object -typename System.Management.Automation.PSCredential -argumentlist $config.client_id, $pass
    $account = Add-AzureRmAccount -Credential $cred -Tenant $config.tenant_id -SubscriptionId $config.subscription_id-ServicePrincipal
} elseif (! (get-azurermcontext -ErrorAction SilentlyContinue).Account) {
    Login-AzureRmAccount
}

## select subscription
if((Get-AzureRmContext).subscription.id -ne $config.subscription_id) {
    Write-Host ("Selecting subscription '{0}'" -f $config.subscription_id)
    $subscription = Select-AzureRmSubscription -SubscriptionId $config.subscription_id
}

## register resource providers
$resourceProviders = @("microsoft.devices")
$resourceProviders |  %{
    if(-not (get-AzureRmResourceProvider -ProviderNamespace $_)) {
        Write-Host "Registering resource provider '$_'"
        $provider = Register-AzureRmResourceProvider -ProviderNamespace "$_"
    }
}


## perform action against the resource(s)
if($resource_name -eq 'all') {
    $config.resources.PSObject.Properties | %{
        $resource = $_
        handle-resource -action $action -resource $resource.value -name $resource.name
    }
} elseif ($resource_names -contains $resource_name) {
    $resource = ($config.resources.PSObject.Properties | ?{$_.name -eq $resource_name})
    handle-resource -action $action -resource $resource.value -name $resource.name
} else {
    write-error ("No resource named '{0}' found." -f $resource_name)
}