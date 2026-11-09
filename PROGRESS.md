# VetOs 3D Enhanced: İlerleme Durumu (PROGRESS.md)

## 1. Hazırlık ve Araç Durumu

Tüm temel araçlar kurulmuş ve doğrulanmıştır:

| Araç | Konum / Sürüm | Durum | Notlar |
|---|---|---|---|
| **Git** | `C:\Program Files\Git\cmd\git.exe` (v2.56.0.windows.4) | Doğrulandı | Git deposu başlatıldı (`master`), yerel kullanıcı ayarlandı. |
| **.NET SDK** | `C:\Program Files\dotnet\dotnet.exe` (v8.0.427) | Doğrulandı | .NET 8 SDK hazır. |
| **ilspycmd** | `C:\Users\user\.dotnet\tools\ilspycmd.exe` (v8.2.0.7535) | Doğrulandı | `Assembly-CSharp.dll` başarıyla C# projesine dönüştürüldü. |
| **VS Code** | `C:\Users\user\AppData\Local\Programs\Microsoft VS Code\Code.exe` | Doğrulandı | Kod ve proje dosyası incelemeleri için hazır. |
| **BepInEx** | `tools\BepInEx_x64_5.4.23.6` | Doğrulandı | 64-bit Unity Mono mod yükleyici hazır. |
| **AssetRipper** | `tools\AssetRipper_win_x64` (v2.0.0) | Doğrulandı | Asset analizi ve dışa aktarma aracı hazır. |
| **UnityExplorer** | `tools\UnityExplorer.BepInEx5.Mono` (v4.13.6 yukieiji fork) | Doğrulandı | Unity 2022 uyumlu BepInEx eklentisi hazır. |
| **dnSpyEx** | `tools\dnSpy-netcore-win64` (v6.6.0) | Doğrulandı | .NET görsel decompiler ve hata ayıklayıcı hazır. |
| **Unity Hub** | Microsoft Store sürümü (v3.22.6) | Doğrulandı | Kurulu ve çalışır durumda. |

### Eksik Kalan Kalem
- **Unity Editor 2022.3.11f1**: Unity Hub Microsoft Store sürümünde oturum açılması gerektiği için henüz kurulmamıştır.
  - *Etki:* `PLAN.md` gereğince Faz 0, Faz 1 ve Faz 2 için Editor kurulumu **zorunlu değildir**. İleride özel AssetBundle ve Android ARM64 derlemesi için gerekecektir.

---

## 2. Dizin Durumu ve Yedekleme

- `backup\`: ~618 MB (`VetOs_ORIGINAL` temiz salt okunur yedek).
- `work\`: ~618 MB (`VetOs_Game` modlama ve çalışma alanı).
- `tools\`: ~514 MB (Geliştirici ve tersine mühendislik araçları).
- `decompiled\`: Tamamlandı (`Assembly-CSharp.dll` tüm C# sınıfları decompile edildi).
- `extracted\`: Tamamlandı (`VetOs_Android\ExportedProject` Unity 2022.3.11f1 projesi olarak 0 MB internet ile AssetRipper üzerinden tam çıkartıldı).
- `.agents\rules\vetos-kurallar.md`: Oluşturuldu (Proje kuralları ve alt ajan ilkeleri).
- `ANALYSIS.md`: Oluşturuldu (Kapsamlı mimari analiz raporu).

---

## 3. Tamamlanan Aşamalar

### [x] Faz 0: Keşif ve Mimari Analiz (TAMAMLANDI)
1. **Kod Decompile İşlemi:** `ilspycmd` ile `work\VetOs_Game\VetOs_Data\Managed\Assembly-CSharp.dll` dosyası `decompiled\` klasörüne eksiksiz olarak aktarıldı.
2. **Proje Kuralları Tanımlandı:** `.agents/rules/vetos-kurallar.md` oluşturuldu. İletişim dili (Türkçe), kullanıcı profili (veteriner hekim/anatomi uzmanı), yönetici/alt ajan işbölümü, orijinal dosya koruma ve model devir kuralları netleştirildi.
3. **Mimari Analiz Raporu (`ANALYSIS.md`) Hazırlandı:**
   - **Genel Mimari ve Sınıflar:** Giriş sahnesi (`Entrance_Camera_Controller`, `Entrance_Object_Controller`), Ana sahne (`Camera_Controller`, `Object_Controller`, `Dir_Controller`), Arayüz ve Pin sistemi (`UI_TreeViewController`, `UI_PinTextController`, `UI_DrawLines` ile `GL.LINES`, `UI_PinPoint_Controller`, `UI_PinTextAnimation`) ve `General_Definitions` veri modelleri belgelendi.
   - **Gizli Yönetici Modu:** Giriş sahnesinde `Ctrl` + tık ile `Vars.AdminMode = true` yapılabildiği ve pin düzenleme arayüzünün bu modda açıldığı keşfedildi.
   - **Türkçe İsimlendirme Eksikliği:** `DBInfBones` ve `DBInfPins` sınıflarında Türkçe alan bulunmadığı, `Vars.Language == Turkish` durumunda doğrudan Latince metinlerin gösterildiği tespit edildi.
   - **Karşılaştırmalı Anatomi Kısıtı:** Hayvan eşleştirmelerinin `Vars.GroupNames` ve `Vars.CompGroupNames` içinde sabit olarak Koyun↔Köpek ve Sığır↔At olarak çiftlendiği, serbest karşılaştırmaya izin vermediği belgelendi.
   - **Uzak Sunucu (REST API) Bağımlılığı:** Verilerin her açılışta `https://apivetos.zcortex.com.tr/v1/Login` üzerinden JSON olarak çekildiği, çevrimdışı önbellek (offline JSON cache) ihtiyacı ortaya kondu.
   - **Android APK Analizi:** `ZCortex.VetOs.apk` dosyasında yalnızca 32-bit `armeabi-v7a` kütüphanelerinin bulunduğu, 64-bit `arm64-v8a` bulunmadığı için modern Honor ve Android 13/14+ cihazlarda `INSTALL_FAILED_CPU_ABI_INCOMPATIBLE` ("Cihazınızla uyumlu değil") hatası verdiği ve çözümün Unity 2022.3.11f1 IL2CPP ARM64 derlemesi olduğu tespit edildi.

---

### [x] Faz 1: Altyapı ve Modlama Ortamının Kurulması (TAMAMLANDI)
1. **BepInEx 5 Kurulumu:** `tools\BepInEx` dosyaları (`winhttp.dll`, `doorstop_config.ini`, `.doorstop_version`, `BepInEx\`) doğrudan `work\VetOs_Game\` dizinine entegre edildi.
2. **UnityExplorer Kurulumu:** Unity 2022 uyumlu UnityExplorer ve UniverseLib eklentileri `work\VetOs_Game\BepInEx\plugins\UnityExplorer\` altına başarıyla yerleştirildi.
3. **C# Mod Projesi (`src\VetOsEnhanced\`):** `net472` yerine Unity 2022.3 Mono ile %100 uyumlu `netstandard2.1` hedefiyle SDK-style `.csproj` yapılandırıldı.
4. **İlk Eklenti Derlemesi:** `Plugin.cs` (Harmony başlatma, loglama ve F1 test kancası) `.NET 8` SDK ile sıfır hata ve sıfır uyarıyla derlenerek `work\VetOs_Game\BepInEx\plugins\VetOsEnhanced.dll` olarak dağıtıldı.
5. **Özel Proje Yetenekleri (Skills):** `.agents\skills\` altında 5 kritik proje yeteneği (`bepinex-harmony-modder`, `unity-android-arm64-rebuilder`, `unity-comparative-anatomy-matrix`, `unity-mono-headless-inspector`, `vet-anatomy-terminology-sync`) oluşturuldu.
6. **Türkçe Terminoloji Sözlüğü:** `assets\terms.json` (NAV 6. Baskı standartlarında köpek, at, sığır ve koyun temel kemik ve çıkıntıları) hazırlandı.
7. **AssetRipper ile Android APK'dan Unity 2022.3.11f1 Projesi Çıkarıldı (0 MB İnternet):** `work\apk_inspect\ZCortex.VetOs.apk` (219.33 MB), yerel Kestrel REST API otomasyonu (`run_export.ps1`) ile `extracted\VetOs_Android\ExportedProject` dizinine tam bir Unity projesi olarak aktarıldı. 7.695 dosya (489.1 MB), 2 sahne (`Entrance.unity`, `Animals.unity`), 2.488 3D model/mesh, 2.494 kaplama ve C# oyun scriptleri doğrulandı.

---

## 4. Sıradaki Adımlar ve Öncelikler

### [>] Güncel Öncelik: 64-bit Android ARM64 APK Bulut Derlemesi (GitHub Actions)
- Modern 64-bit Android cihazlar (Honor Pad, Android 13/14+) için `arm64-v8a` APK üretimi GitHub Actions (`game-ci/unity-builder@v4`) ile bulutta çalıştırılacaktır.
- **İnternet Kotası Güncellemesi:** APK bulut derleme işlemi için internet kotası **600 MB** olarak güncellenmiştir.
- `cloud_build/` altında `.github/workflows/build-android-arm64.yml` ve `VetOsBuildScript.cs` taslakları hazırlanmaktadır.

### [!] Beklemedeki Görev: Faz 2 (Türkçe Sözlük Enjeksiyonu ve Harmony Yaması)
- `UNUTMA`: Faz 2 için bekleyen kullanıcı sorusu ("Kemik/pin isimlerinin görünüm tercihi: A / B / C") APK görevi sonrasında cevaplanmak üzere beklemeye alınmıştır. APK tamamlanınca bu soruya dönülecektir.
1. `Plugin.cs` içerisine `assets\terms.json` okuyucusunun eklenmesi.
2. `UI_TreeViewController.ShowBoneInfo` ve `UI_PinPoint_Controller` için Harmony Postfix yamalarının yazılarak arayüzdeki Latince zorunluluğunun Türkçe ile değiştirilmesi.
3. Modlu oyunun çalıştırılarak ekranda test edilmesi.
