# VetOs 3D Enhanced - Derleme ve Dağıtım Betiği
param(
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$ProjectRoot = $PSScriptRoot
$DotnetExe = "C:\Program Files\dotnet\dotnet.exe"
$Csproj = Join-Path $ProjectRoot "src\VetOsEnhanced\VetOsEnhanced.csproj"
$BuiltDll = Join-Path $ProjectRoot "src\VetOsEnhanced\bin\$Configuration\netstandard2.1\VetOsEnhanced.dll"
$PluginTargetDir = Join-Path $ProjectRoot "work\VetOs_Game\BepInEx\plugins"
$PluginTarget = Join-Path $PluginTargetDir "VetOsEnhanced.dll"

Write-Host "=========================================" -ForegroundColor Cyan
Write-Host "VetOs 3D Enhanced Derleme Süreci Başladı" -ForegroundColor Cyan
Write-Host "=========================================" -ForegroundColor Cyan

if (-not (Test-Path $DotnetExe)) {
    throw "dotnet.exe bulunamadı: $DotnetExe"
}

Write-Host "Proje derleniyor ($Configuration)..." -ForegroundColor Yellow
& $DotnetExe build $Csproj -c $Configuration

if (-not (Test-Path $BuiltDll)) {
    throw "Derleme çıktısı bulunamadı: $BuiltDll"
}

if (-not (Test-Path $PluginTargetDir)) {
    New-Item -ItemType Directory -Path $PluginTargetDir -Force | Out-Null
}

Write-Host "Derlenen eklenti BepInEx eklentiler klasörüne kopyalanıyor..." -ForegroundColor Yellow
Copy-Item -Path $BuiltDll -Destination $PluginTarget -Force

if (Test-Path $PluginTarget) {
    $dllInfo = Get-Item $PluginTarget
    Write-Host "BAŞARILI: Mod eklentisi derlendi ve yerleştirildi!" -ForegroundColor Green
    Write-Host "Hedef: $($dllInfo.FullName) ($([math]::Round($dllInfo.Length / 1KB, 2)) KB)" -ForegroundColor Green
} else {
    throw "Derleme tamamlandı ancak hedef DLL bulunamadı: $PluginTarget"
}
