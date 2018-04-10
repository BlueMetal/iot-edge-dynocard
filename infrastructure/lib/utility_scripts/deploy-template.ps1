Param(
    [parameter(Mandatory=$true)]
    $name,

    [parameter(Mandatory=$true)]
    $resourceGroup,

    [parameter(Mandatory=$true)]
    $location,

    [parameter(Mandatory=$true)]
    $template,

    [parameter(Mandatory=$true)]
    $parameters
)

$rg = Get-AzureRmResourceGroup -Name $resourceGroup -ErrorAction SilentlyContinue
if(!$rg)
{
    Write-Info "Resource group '$resourceGroup' does not exist."
    Write-Info "Creating resource group '$resourceGroup' in location '$location'";
    New-AzureRmResourceGroup -Name $resourceGroup -Location $location
} elseif ($rg -and $rg.location -ne $location) {
    Write-Info "Resource group '$resourceGroup' does not exist in '$location' location."
    Write-Info "Creating resource group '$resourceGroup' in location '$location'";
    New-AzureRmResourceGroup -Name $resourceGroup -Location $location
}

Write-Host "Starting deployment of '$name'..."
$deployment = New-AzureRmResourceGroupDeployment `
    -Name $name `
    -ResourceGroupName $resourceGroup `
    -TemplateFile $template `
    -TemplateParameterObject $parameters
Write-Host ("Deployment of '$name' {0}." -f $deployment.ProvisioningState.toLower())
return $deployment