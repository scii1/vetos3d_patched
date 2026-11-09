# VetOs 3D Enhanced: Plan (Türkçe)

Hedef uygulama: `C:\Program Files\ZCortex\3D Veterinary Osteology\VetOs.exe`
Unity 2022.3.11f1, x64, Mono (`Assembly-CSharp.dll` ≈ 78 KB). Kişisel kullanım. Modlu dosyalar paylaşılmaz.

## Kurallar (tüm modeller için)
- `C:\Program Files\...` altındaki orijinal dosyalara asla yazma. Önce `work\VetOs_Game` altına kopyala.
- Orijinalin ikinci bir kopyası `backup\VetOs_ORIGINAL` olarak kalır ve değiştirilmez.
- Her aşama sonunda `git commit` yap ve `PROGRESS.md` dosyasına ne yapıldığını, ne bulunduğunu, sıradaki adımı yaz.
- Kullanıcı kodlama bilmiyor. Açıklamalar Türkçe ve sade olmalı.
- Orijinal içerik (çıkarılan asset ve kod) git'e eklenmez (`.gitignore`).

## Kurulacak programlar
| # | Program | Kurulum | Zorunlu mu | Ne için |
|---|---|---|---|---|
| 1 | Git | `winget install Git.Git` | Evet | Sürüm kaydı, geri alma |
| 2 | .NET SDK 8 | `winget install Microsoft.DotNet.SDK.8` | Evet | Eklenti derleme (`net472` hedefi için `Microsoft.NETFramework.ReferenceAssemblies` NuGet paketi eklenir) |
| 3 | ilspycmd | `dotnet tool install -g ilspycmd` | Evet | `Assembly-CSharp.dll` dosyasını komut satırından C# koduna çevirir |
| 4 | BepInEx 5.4.x **x64 Mono** | GitHub Releases zip, `work\VetOs_Game` içine açılır | Evet | Mod yükleyici. İçinde Harmony (HarmonyX) var |
| 5 | AssetRipper | GitHub Releases | Evet | İsimlerin ve etiketlerin tutulduğu asset verisini çıkarır |
| 6 | UnityExplorer (BepInEx5 Mono, Unity 2022 ile çalışan güncel fork) | GitHub Releases, `BepInEx\plugins` içine | Önerilir | Çalışırken sahne ve arayüz hiyerarşisini inceler |
| 7 | dnSpyEx | GitHub Releases, zip | Opsiyonel | Kullanıcının kodu görsel olarak okuması için |
| 8 | VS Code (+ C# eklentisi) | `winget install Microsoft.VisualStudioCode` | Opsiyonel | Kullanıcının dosya görmesi için |
| 9 | UABEA | GitHub Releases | Opsiyonel | AssetRipper yetmezse tek asset inceleme |

Gerekmeyenler: Unity Editor (Faz 0–2), Blender (model değişmiyor), IL2CPP araçları (Mono), Ghidra.
Unity Editor sadece ileride özel arayüz paketi (AssetBundle) gerekirse eklenir, aynı sürüm 2022.3.11f1 ile.

### Kurulum notları
- `winget` ve ağ erişimi sandbox dışında çalışır, kullanıcı onayı gerekir.
- Önceki denemede `winget --version` sandbox içinde çalışmadı. İlk adım yetkili/sandbox dışı çalıştırmaktır.
- Kurulumdan sonra yeni terminal açılmalı (PATH yenilenir).
- BepInEx için `winhttp.dll` ve `doorstop_config.ini` oyunun `.exe` dosyasının yanında olmalı. İlk çalıştırmada `BepInEx\LogOutput.log` oluşur.

## Aşamalar
**Faz 0: Keşif (kod yazmadan)**
1. Kopyalar alınır (`backup`, `work`).
2. `ilspycmd` ile `Assembly-CSharp.dll` dışa aktarılır: `decompiled\`.
3. AssetRipper ile proje çıkarılır: `extracted\`.
4. Rapor (`ANALYSIS.md`): isimler nerede, etiketler nasıl çiziliyor, karşılaştırmalı anatomi nasıl çalışıyor, hata nedenleri.

**Faz 1: Altyapı**
BepInEx kurulumu, UnityExplorer, "merhaba" yazan test eklentisi, derleme ve dağıtım betiği (`build.ps1`).

**Faz 2: İsim gösterimi**
Latince (NAV) / Türkçe / İngilizce sözlük (`assets\terms.json`), etiket biçimi değişikliği, bilinen hatalar.

**Faz 3: Karşılaştırmalı anatomi**
Faz 0 raporuna göre tasarım, sonra uygulama.

## Model dağılımı
| Görev | Model | Neden |
|---|---|---|
| Faz 0 analiz raporu, Faz 3 tasarımı, zor hata ayıklama | Claude Opus 5.5 | Muhakeme gerektiren işler, az ama kritik |
| Faz 1–3 kod yazımı, Harmony yamaları | Claude Sonnet 5.5 | Ana geliştirici |
| Kurulum, indirme, betikler, log okuma, sözlük doldurma | Gemini 3.8 Flash (veya DeepSeek / GLM) | Ucuz, hızlı, mekanik işler |
| Bağımsız kod incelemesi (isteğe bağlı) | ChatGPT (Luna/Sol) | İkinci göz, farklı hata yakalama |

Planlama ve analiz için Flash sınıfı model kullanılmaz (önceki konuşmada plan hataları orada çıktı).
