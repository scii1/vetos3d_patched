// ============================================================================
// VetOs 3D Patched - Headless / Cloud Android Build Script
// Dosya: Assets/Editor/VetOsBuildScript.cs (veya cloud_build/VetOsBuildScript.cs)
// Amac: Unity 2022.3.11f1 üzerinde 64-bit ARM64 (arm64-v8a) ve 32-bit ARMv7
//       destekli IL2CPP Android APK derlemesini tam otomatik yürütmek.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Rendering;

public static class VetOsBuildScript
{
    [MenuItem("VetOs/Build Android ARM64 APK")]
    public static void ConfigureAndBuildAndroid()
    {
        Debug.Log("====================================================================");
        Debug.Log("[VetOsBuild] Android ARM64+ARMv7 IL2CPP derleme süreci başlatılıyor...");
        Debug.Log("====================================================================");

        // 1. Android Platformuna Geçiş Yap
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            Debug.Log("[VetOsBuild] Aktif platform Android olarak değiştiriliyor...");
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        }

        // 2. Paket ve Tanımlama Bilgilerini Yapılandır
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.ZCortex.VetOs");
        PlayerSettings.productName = "3D Veterinary Osteology";
        PlayerSettings.companyName = "ZCortex";
        PlayerSettings.bundleVersion = "1.0.1";
        PlayerSettings.Android.bundleVersionCode = 2;

        // 3. Scripting Backend: IL2CPP Zorunludur (ARM64 mimarisi Mono desteklemez)
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.Android, Il2CppCompilerConfiguration.Release);
        PlayerSettings.SetIl2CppCodeGeneration(Il2CppCodeGeneration.OptimizeSize);

        // 4. Android Graphics API: OpenGLES3 önceliği (GL.LINES / UI_DrawLines uyumluluğu için)
        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { GraphicsDeviceType.OpenGLES3 });
        Debug.Log("[VetOsBuild] Android Graphics API: OpenGLES3 olarak yapılandırıldı.");

        // 5. Hedef Mimariler: ARM64 + ARMv7 (Universal uyumluluk)
        // Modern Honor Pad / Android 13-14+ cihazlar için ARM64, eski cihazlar için ARMv7
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        Debug.Log($"[VetOsBuild] Hedef Mimariler: {PlayerSettings.Android.targetArchitectures}");

        // 6. SDK Seviyeleri (Modern Android 13/14 Uyumluluğu)
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24; // Android 7.0+
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel33; // Android 13+

        // 7. APK Çıktısı (AAB yerine doğrudan kurulabilir APK)
        EditorUserBuildSettings.buildAppBundle = false;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;

        // 8. Sahnelerin Tespiti ve Sıralanması (Entrance.unity = index 0, Animals.unity = index 1 zorunlu kılınır)
        string[] rawScenes = EditorBuildSettings.scenes
            .Where(s => s.enabled && !string.IsNullOrEmpty(s.path) && File.Exists(s.path))
            .Select(s => s.path)
            .ToArray();

        if (rawScenes.Length == 0)
        {
            Debug.LogWarning("[VetOsBuild] EditorBuildSettings içinde kayıtlı aktif sahne bulunamadı. Assets dizini taranıyor...");
            rawScenes = Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories)
                .OrderBy(s => s)
                .ToArray();
        }

        List<string> orderedScenes = new List<string>();

        // 0. indeks: Entrance.unity
        string entranceScene = rawScenes.FirstOrDefault(s => s.EndsWith("Entrance.unity", StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrEmpty(entranceScene))
        {
            orderedScenes.Add(entranceScene);
        }
        else if (File.Exists("Assets/Scenes/Entrance.unity"))
        {
            orderedScenes.Add("Assets/Scenes/Entrance.unity");
        }

        // 1. indeks: Animals.unity
        string animalsScene = rawScenes.FirstOrDefault(s => s.EndsWith("Animals.unity", StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrEmpty(animalsScene))
        {
            orderedScenes.Add(animalsScene);
        }
        else if (File.Exists("Assets/Scenes/Animals.unity"))
        {
            orderedScenes.Add("Assets/Scenes/Animals.unity");
        }

        // Diğer sahneleri ekle
        foreach (var s in rawScenes)
        {
            if (!orderedScenes.Contains(s, StringComparer.OrdinalIgnoreCase))
            {
                orderedScenes.Add(s);
            }
        }

        string[] scenes = orderedScenes.ToArray();

        Debug.Log($"[VetOsBuild] Derlemeye dahil edilen sahne sayısı: {scenes.Length}");
        for (int i = 0; i < scenes.Length; i++)
        {
            Debug.Log($"  - [{i}] Sahne: {scenes[i]}");
        }

        // 9. Derleme Hedef Yolunun Belirlenmesi
        string apkPath = GetOutputApkPath();
        string outputDir = Path.GetDirectoryName(apkPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        Debug.Log($"[VetOsBuild] Hedef APK yolu: {apkPath}");

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = apkPath,
            target = BuildTarget.Android,
            targetGroup = BuildTargetGroup.Android,
            options = BuildOptions.None
        };

        // 10. Derleme Çağrısı
        DateTime startTime = DateTime.Now;
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;
        TimeSpan duration = DateTime.Now - startTime;

        if (summary.result == BuildResult.Succeeded)
        {
            long sizeMb = (long)(summary.totalSize / (1024 * 1024));
            Debug.Log("====================================================================");
            Debug.Log($"[VetOsBuild] DERLEME BAŞARILI!");
            Debug.Log($"[VetOsBuild] APK Konumu : {apkPath}");
            Debug.Log($"[VetOsBuild] Dosya Boyutu: {sizeMb} MB");
            Debug.Log($"[VetOsBuild] Süre        : {duration.TotalMinutes:F1} dakika");
            Debug.Log("====================================================================");

            if (Application.isBatchMode)
            {
                EditorApplication.Exit(0);
            }
        }
        else
        {
            Debug.LogError("====================================================================");
            Debug.LogError($"[VetOsBuild] DERLEME BAŞARISIZ OLDU!");
            Debug.LogError($"[VetOsBuild] Sonuç      : {summary.result}");
            Debug.LogError($"[VetOsBuild] Hata Sayısı: {summary.totalErrors}");
            Debug.LogError($"[VetOsBuild] Uyarı Sayısı: {summary.totalWarnings}");
            Debug.LogError("====================================================================");

            if (Application.isBatchMode)
            {
                EditorApplication.Exit(1);
            }
        }
    }

    /// <summary>
    /// Komut satırı argümanlarından (-customBuildPath) veya varsayılan yoldan APK yolunu üretir.
    /// GameCI uyumluluğu için -customBuildPath parametresi önceliklidir.
    /// </summary>
    private static string GetOutputApkPath()
    {
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (string.Equals(args[i], "-customBuildPath", StringComparison.OrdinalIgnoreCase))
            {
                string custom = args[i + 1];
                if (!string.IsNullOrEmpty(custom))
                {
                    if (Directory.Exists(custom) || custom.EndsWith("/") || custom.EndsWith("\\"))
                    {
                        return Path.Combine(custom, "vetos3d_patched.apk");
                    }
                    if (!custom.EndsWith(".apk", StringComparison.OrdinalIgnoreCase))
                    {
                        return custom + ".apk";
                    }
                    return custom;
                }
            }
        }

        // Varsayılan yol: <proje_kökü>/builds/Android/vetos3d_patched.apk
        return Path.Combine(Directory.GetCurrentDirectory(), "builds", "Android", "vetos3d_patched.apk");
    }
}
