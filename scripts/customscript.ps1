param(

    [string] $installdocker = "$1"
    
    )
    
Set-ExecutionPolicy -ExecutionPolicy unrestricted  -Force

mkdir C:\Downloads

    $client = new-object System.Net.WebClient
    
    $client.DownloadFile($installdocker,"C:\Downloads\installdocker.ps1") 

    sleep 20  
powershell -noprofile -command "&{ start-process powershell -ArgumentList '-noprofile -file C:\Downloads\installdocker.ps1' -verb RunAs}" >> "StartupLog.txt" 2>&1
