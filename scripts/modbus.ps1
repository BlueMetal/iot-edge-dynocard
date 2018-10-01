
param(

    [string] $plcsimulator = "$1",
    
    [string] $vcredist_x86 = "$2",
    
    [string] $plcsimulator2 = "$3",
    
    [string] $DynoCard = "$4"
    
    )
    
    Set-ExecutionPolicy -ExecutionPolicy unrestricted  -Force
    
    $client = new-object System.Net.WebClient
    
    $client.DownloadFile($plcsimulator,"C:\SimSetup.msi") 
    
    Write-Host ----installation--starts --- -ForegroundColor Green
    
    C:\SimSetup.msi /qn
    
    sleep 50
    
    Write-Host ----installation--completed --- -ForegroundColor Green
    
    Write-Host ----installation--starts --- -ForegroundColor Green
    
    $client.DownloadFile($vcredist_x86,"C:\vcredist_x86.exe") 
    
    C:\vcredist_x86.exe /quiet
    
    sleep 50
    
    Write-Host ----installation--completed --- -ForegroundColor Green
    
    Write-Host ----installation--starts --- -ForegroundColor Green
    
    $client.DownloadFile($plcsimulator2,"C:\ModRSsim2.exe")
    
    Write-Host ----installation--completed --- -ForegroundColor Green
    
    $client.DownloadFile($DynoCard,"C:\DynoCard.vbs")
    
    Write-Host ----installation--completed --- -ForegroundColor Green
    
    
    