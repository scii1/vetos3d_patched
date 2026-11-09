---
name: unity-android-arm64-rebuilder
description: >-
  VetOs 3D uygulamasını AssetRipper ile Unity projesine dönüştürerek Unity Editor 2022.3.11f1 üzerinde
  64-bit ARM64 (arm64-v8a) IL2CPP APK olarak derleme sürecini yönetir.
---

# Unity Android ARM64 Rebuilder Skill

Bu yetenek, 32-bit `armeabi-v7a` mimarisiyle sınırlandırılmış olan VetOs 3D Android uygulamasını (`work\apk_inspect\ZCortex.VetOs.apk`), AssetRipper kullanarak eksiksiz bir Unity projesine dönüştürme ve Unity Editor 2022.3.11f1 ortamında 64-bit **ARM64 (arm64-v8a)** IL2CPP APK olarak yeniden derleme iş akışını yönetir.

---

## 1. Problemin Tanımı ve Kök Neden Analizi

- **Mevcut Durum:** Orijinal `ZCortex.VetOs.apk` paketi (`work\apk_inspect\ZCortex.VetOs.apk`) incelendiğinde, `lib/` dizini altında yalnızca 32-bit `armeabi-v7a` kütüphanelerinin (`libmono.so`, `libunity.so` vb.) bulunduğu görülmektedir.
- **Hata:** Modern Honor Pad tabletler, Google Pixel telefonlar ve Android 13/14+ yüklü yeni nesil mobil yongalar (Snapdragon 8 Gen 2/3, Dimensity 9200+ vb.) 32-bit komut kütüğünü tamamen terk etmiştir (64-bit-only mimari). Bu nedenle kurulum anında Android Paket Yöneticisi şu hatayı fırlatır:
  ```text
  INSTALL_FAILED_CPU_ABI_INCOMPATIBLE: Package couldn't be installed. The phone's CPU architecture does not support this app.
  ```
- **Çözüm:** Unity projesinin varlıklarını ve sahnelerini AssetRipper ile kurtarmak, `Scripting Backend` ayarını `IL2CPP` ve `Target Architectures` ayarını `ARM64` olarak yapılandırıp `BuildPipeline.BuildPlayer` ile modern cihazlara uyumlu 64-bit APK derlemektir.

---

## 2. İş Akışı ve Adımlar

### Adım 1: AssetRipper ile Unity Projesini Dışa Aktarma
AssetRipper GUI veya CLI modu kullanılarak APK arşivi decompile edilir ve Unity 2022.3 uyumlu proje dizinine aktarılır.

```powershell
# AssetRipper CLI üzerinden projeyi dışa aktarma
& "tools\AssetRipper\AssetRipper.GUI.Free.exe" `
    --export-unity-project `
    "extracted\VetOs_UnityProject" `
    "work\apk_inspect\ZCortex.VetOs.apk"
```

*Not:* Dışa aktarma işlemi tamamlandığında `extracted\VetOs_UnityProject` dizininde `Assets/`, `ProjectSettings/` ve `Packages/` klasörleri oluşacaktır.

---

### Adım 2: Editor Derleme Betiğinin Eklenmesi (`VetOsBuildScript.cs`)
Headless/Batchmode derleme için `Assets/Editor/VetOsBuildScript.cs` dosyası projeye yerleştirilir. Bu script IL2CPP, ARM64 ve Android API seviyelerini otomatik yapılandırır ve `BuildPipeline.BuildPlayer` çağrısını yürütür.

```csharp
// Assets/Editor/VetOsBuildScript.cs
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class VetOsBuildScript
{
    [MenuItem("VetOs/Build Android ARM64 APK")]
    public static void ConfigureAndBuildAndroid()
    {
        Debug.Log("[VetOsBuild] Android ARM64 IL2CPP derleme süreci başlatılıyor...");

        // 1. Android Platformuna Geçiş
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        // 2. Paket ve Tanımlama Bilgileri
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.ZCortex.VetOs");
        PlayerSettings.productName = "3D Veterinary Osteology";
        PlayerSettings.companyName = "ZCortex";

        // 3. Scripting Backend: IL2CPP Zorunludur (ARM64 Mono desteklemez)
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.Android, Il2CppCompilerConfiguration.Release);

        // 4. Hedef Mimariler: ARM64 (arm64-v8a) Aktif
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

        // 5. SDK Seviyeleri (Modern Android 13/14 Uyumluluğu)
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24; // Android 7.0+
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel33; // Android 13+

        // 6. APK Çıktısı (AAB yerine doğrudan APK)
        EditorUserBuildSettings.buildAppBundle = false;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;

        // 7. Sahnelerin Tespiti
        string[] scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled && File.Exists(s.path))
            .Select(s => s.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            // Eğer BuildSettings içinde kayıtlı sahne yoksa projeyi tara
            scenes = Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories);
        }

        Debug.Log($"[VetOsBuild] Derlemeye dahil edilen sahneler: {string.Join(", ", scenes)}");

        // 8. Derleme Hedef Yolu
        string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "builds", "Android");
        Directory.CreateDirectory(outputDir);
        string apkPath = Path.Combine(outputDir, "VetOs_ARM64.apk");

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = apkPath,
            target = BuildTarget.Android,
            targetGroup = BuildTargetGroup.Android,
            options = BuildOptions.None
        };

        // 9. Derleme Çağrısı
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"[VetOsBuild] Başarılı! APK oluşturuldu: {apkPath} ({summary.totalSize / 1024 / 1024} MB)");
        }
        else
        {
            Debug.LogError($"[VetOsBuild] Derleme başarısız oldu! Hata sayısı: {summary.totalErrors}");
            EditorApplication.Exit(1);
        }
    }
}
```

---

### Adım 3: Unity Editor 2022.3.11f1 CLI Batchmode Komutu
Editor arayüzü açılmadan, komut satırından tam otomatik derleme gerçekleştirilir:

```powershell
$unityExe = "C:\Program Files\Unity\Hub\Editor\2022.3.11f1\Editor\Unity.exe"
$projectPath = "C:\Antigravity Projects Hub\VetOs 3D Enhanced\extracted\VetOs_UnityProject"
$logFile = "C:\Antigravity Projects Hub\VetOs 3D Enhanced\work\unity_build.log"

& $unityExe -quit -batchmode `
    -projectPath $projectPath `
    -executeMethod VetOsBuildScript.ConfigureAndBuildAndroid `
    -logFile $logFile

Write-Host "Derleme tamamlandı. Log kontrolü: $logFile"
```

---

### Adım 4: APK İçi Mimari Doğrulama (ABI Verification)
Üretilen APK dosyasının gerçekten 64-bit ARM64 ikili dosyalarını (`libunity.so`, `libil2cpp.so`) içerdiği komut satırından doğrulanır:

```powershell
# APK arşivi içindeki kütüphaneleri listele
tar -tf "builds\Android\VetOs_ARM64.apk" | Select-String "lib/"

# Beklenen Çıktı Örneği:
# lib/arm64-v8a/
# lib/arm64-v8a/libunity.so
# lib/arm64-v8a/libil2cpp.so
# lib/arm64-v8a/libmain.so
```

---

### Adım 5: Cihaza Kurulum ve Test
ADB (Android Debug Bridge) üzerinden hedef cihaza yükleme gerçekleştirilir:

```powershell
# Cihaz bağlantısını denetle
adb devices

# 64-bit APK'yı zorunlu/güncelleyerek yükle
adb install -r -d "builds\Android\VetOs_ARM64.apk"

# Unity çalışma zamanı loglarını izle
adb logcat -s Unity ActivityManager PackageManager
```

---

## 3. Karşılaşılabilecek Hatalar ve Çözümleri

| Hata / Belirti | Olası Neden | Çözüm |
|---|---|---|
| `NDK not configured` | Unity Editor Android Build Support içinde NDK eksik | Unity Hub üzerinden "Android Build Support (IL2CPP)" ve "Android NDK & SDK Tools" modüllerini kur. |
| `Gradle build failed` | Java JDK uyumsuzluğu veya bellek yetersizliği | Unity'nin dahili OpenJDK'sını kullan (`Preferences > External Tools`). `gradle.properties` içine `org.gradle.jvmargs=-Xmx4096m` ekle. |
| `Shader compilation errors` | AssetRipper dışa aktarımında gölgelendirici kayıpları | Varsayılan Standart/Universal RP shader referanslarını yenile veya pink material hatası durumunda shader cache'i temizle. |
| `Duplicate class in DEX` | Eski Android eklenti çakışması | `Assets/Plugins/Android` altındaki yinelenen `.aar` veya `.jar` bağımlılıklarını temizle. |
