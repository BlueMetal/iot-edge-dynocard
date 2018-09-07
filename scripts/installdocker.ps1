param(

    [string] $mlworkbench = "https://projectiot.blob.core.windows.net/oilgas-iot/AzureML/AmlWorkbenchSetup.msi"
    
    )
    
    Set-ExecutionPolicy -ExecutionPolicy unrestricted  -Force

    $client = new-object System.Net.WebClient
    
    $client.DownloadFile($mlworkbench,"C:\Downloads\AmlWorkbenchSetup.msi") 

    sleep 20  

$DockerInstaller = Join-Path $Env:Temp InstallDocker.msi

Invoke-WebRequest https://download.docker.com/win/stable/InstallDocker.msi -OutFile $DockerInstaller

msiexec -i $DockerInstaller -quiet

Do
{
 $test=[ADSI]::Exists("WinNT://localhost/docker-users")

 echo "started sleeping"

 start-sleep 30

} While ($test -eq $False)

echo "Docker successfully installed"


Add-LocalGroupMember -Member $env:ComputerName\$env:username -Name docker-users

echo "user has been Added"
