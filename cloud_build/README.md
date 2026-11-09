# VetOs 3D Enhanced - GitHub Actions Bulut Derleme Kılavuzu

Bu klasör, VetOs 3D Android uygulamasının modern 64-bit (`arm64-v8a`) cihazlara (Honor Pad, modern tablet/telefonlar, Android 13/14+) uyumlu APK çıktısını **kullanıcının bilgisayarında GB'larca Android SDK/NDK ve Unity Editor indirmesine gerek kalmadan (0 MB yerel internet harcayarak)** GitHub Actions bulutu üzerinde derlemek için hazırlanmıştır.

---

## 1. Klasör İçeriği

- **`VetOsBuildScript.cs`**: Unity Editor içerisinde çalışan, `IL2CPP` derleyicisini, `ARM64` ve `ARMv7` (Universal) mimarilerini, API Seviyesi 33 (Android 13) hedefini ve doğrudan kurulabilir `.apk` çıktısını zorunlu kılan otomatik derleme betiğidir.
- **`.github/workflows/build-android-arm64.yml`**: GitHub Actions üzerinde Unity 2022.3.11f1 Docker konteynerini (`game-ci/unity-builder@v4`) çalıştırarak projeyi derleyen, `zipalign -p -f 4` ve `apksigner` ile V1+V2+V3 imzalayan, `arm64-v8a` (`libil2cpp.so` & `libunity.so`) 64-bit varlığını otomatik doğrulayan (CI QA) ve üretilen APK'yı hem Artifact hem doğrudan tek tıkla GitHub Release olarak sunan tam otomatik iş akışıdır.
- **`.github/workflows/acquire-activation-file.yml`**: Bilgisayarda Unity lisansı bulunmadığında, GitHub Actions üzerinde tek tıkla ücretsiz `.alf` aktivasyon dosyası üreten yardımcı iş akışıdır.

---

## 2. Kullanıcıdan Gereken 1-2 Bilgi / Hazırlık

Bulut derlemesinin çalışabilmesi için kullanıcının şu iki adımı tamamlaması (veya ajana bilgi vermesi) gerekmektedir:

### A. GitHub Deposu (Repository) Bağlantısı
- Bilgisayarda şu an GitHub CLI (`gh`) veya Git kimlik bilgisi tanımlı değildir.
- Kullanıcının GitHub üzerinde boş bir özel (private) repo açması ve linkini (`https://github.com/KULLANICI/VetOs-3D-Enhanced.git`) sağlaması yeterlidir.

### B. Unity Ücretsiz Lisansı (`UNITY_LICENSE` Secret'ı)
GitHub Actions GameCI derleyicisinin çalışabilmesi için GitHub Repo ayarlarında (`Settings > Secrets and variables > Actions`) şu secret tanımlanmalıdır:
- **`UNITY_LICENSE`**: Unity Kişisel (Personal / Free) lisans dosyasının (`.ulf`) metin içeriği.

> **Lisans Dosyası (.ulf) Nasıl Alınır? (0 MB İnternet ile Kolay Yöntem):**
> 1. Repo GitHub'a yüklendikten sonra `Acquire Unity Activation File` iş akışı "Run workflow" butonu ile çalıştırılır.
> 2. 30 saniye içinde oluşan `Unity_v2022.3.11f1.alf` dosyası GitHub'dan indirilir.
> 3. [https://license.unity3d.com/manual](https://license.unity3d.com/manual) adresine girilip bu `.alf` dosyası yüklenir, "Personal Edition" seçilir.
> 4. İndirilen `Unity_lic.ulf` dosyasının metni kopyalanarak GitHub repo Secrets içine `UNITY_LICENSE` adıyla yapıştırılır.
> *(Alternatif: Kullanıcı Unity hesabı e-posta ve şifresini `UNITY_EMAIL` ve `UNITY_PASSWORD` olarak da tanımlayabilir).*

---

## 3. Derleme Çıktısı

İş akışı tamamlandığında, GitHub Actions "Actions" sekmesinden **`VetOs-Android-ARM64-APK`** başlığı altında doğrudan Honor Pad ve tüm modern Android cihazlara yüklenebilir APK indirilebilecektir.
