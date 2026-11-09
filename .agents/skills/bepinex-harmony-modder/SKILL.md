---
name: bepinex-harmony-modder
description: >-
  Unity 2022.3.11f1 Mono x64 tabanlı VetOs 3D Enhanced projesi için BepInEx 5 kurulumunu,
  Harmony çalışma zamanı kancalarını, .NET SDK ile net472 eklenti derleme ve test süreçlerini yönetir.
---

# BepInEx & Harmony Modder Skill

Bu yetenek, `work\VetOs_Game` dizinindeki Unity Mono x64 uygulamasını modlamak için BepInEx 5 ve Harmony entegrasyonunu yönetir.

## Prosedür

1. **BepInEx ve UnityExplorer Kurulumu:**
   - `tools\BepInEx_x64_5.4.23.6` içeriğini `work\VetOs_Game\` kök dizinine yerleştir (`winhttp.dll`, `doorstop_config.ini`, `BepInEx/`).
   - `tools\UnityExplorer.BepInEx5.Mono` dosyalarını `work\VetOs_Game\BepInEx\plugins\UnityExplorer` klasörüne kopyala.

2. **C# Mod Projesi Yapılandırması (`src\VetOsEnhanced\`):**
   - SDK-style `.csproj` dosyası `net472` hedefler ve `Microsoft.NETFramework.ReferenceAssemblies` NuGet paketini içerir.
   - Referanslar:
     - `work\VetOs_Game\VetOs_Data\Managed\UnityEngine.dll`
     - `work\VetOs_Game\VetOs_Data\Managed\UnityEngine.CoreModule.dll`
     - `work\VetOs_Game\VetOs_Data\Managed\UnityEngine.UI.dll`
     - `work\VetOs_Game\VetOs_Data\Managed\Assembly-CSharp.dll`
     - `work\VetOs_Game\BepInEx\core\BepInEx.dll`
     - `work\VetOs_Game\BepInEx\core\0Harmony.dll`

3. **Derleme ve Dağıtım:**
   - `dotnet build src\VetOsEnhanced\VetOsEnhanced.csproj -c Release`
   - Üretilen DLL'i `work\VetOs_Game\BepInEx\plugins\` dizinine kopyala.

4. **Doğrulama ve Hata Ayıklama:**
   - `work\VetOs_Game\BepInEx\LogOutput.log` dosyasını inceleyerek kanca ve yama durumlarını kontrol et.
