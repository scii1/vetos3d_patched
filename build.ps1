# VetOs 3D Enhanced - Derleme ve Dağıtım Betiği
param(
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$ProjectRoot = $PSScriptRoot
$DotnetExe = "C:\Program Files\dotnet\dotnet.exe"
$Csproj = Join-Path $ProjectRoot "src\VetOsEnhanced\VetOsEnhanced.csproj"
$PluginTarget = Join-Path $ProjectRoot "work\VetOs_Game\BepInEx\plugins\VetOsEnhanced.dll"

Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "VetOs 3D Enhanced Derleme Süreci Başladı" -ForegroundColor Cyan
Write-Host "=========================================" -ForegroundColor Cyan

if (-not (Test-Path $DotnetExe)) {
    throw "dotnet.exe bulunamadı: $DotnetExe"
}

Write-Host "Proje derleniyor ($Configuration)..." -ForegroundColor Yellow
& $DotnetExe build $Csproj -c $Configuration

if (Test-Path $PluginTarget) {
    $dllInfo = Get-Item $PluginTarget
    Write-Host "BAŞARILI: Mod eklentisi derlendi ve yerleştirildi!" -ForegroundColor Green
    Write-Host "Hedef: $($dllInfo.FullName) ($([math]::Round($dllInfo.Length / 1KB, 2)) KB)" -ForegroundColor Green
} else {
    throw "Derleme tamamlandı ancak hedef DLL bulunamadı: $PluginTarget"
}
