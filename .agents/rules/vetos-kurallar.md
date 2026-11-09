# VetOs 3D Enhanced Proje Kuralları

Bu kurallar VetOs 3D Enhanced projesinde çalışan tüm yapay zeka modelleri ve ajanlar için bağlayıcıdır.

---

### 1. Dil ve İletişim
- Tüm iletişim, durum raporları, açıklamalar ve dokümantasyon **Türkçe**, sade ve anlaşılır olmalıdır.
- Kullanıcı veteriner hekimlik ve veteriner anatomi alanı uzmanıdır; yazılım geliştirme / kodlama bilmemektedir.
- Açıklamalarda gereksiz teknik karmaşa yerine amaca yönelik, net ve sonuç odaklı ifadeler kullanılmalıdır.

---

### 2. Yönetici Rolü ve Alt Ajan (Subagent) Kullanımı
- Ana model projeyi uçtan uca yönetir, stratejik kararları alır ve genel akışı koordine eder.
- Dosya arama/tarama, kod yazma, derleme, paket kurulumu ve test gibi operasyonel/uygulama işlerini ana model doğrudan yapmak yerine `invoke_subagent` (tercihen `Model: "flash"`) aracılığıyla alt ajanlara devreder.

---

### 3. Orijinal Dosya Koruması
- `C:\Program Files\ZCortex\3D Veterinary Osteology` altındaki orijinal kurulu dosyalara ve `backup\VetOs_ORIGINAL` altındaki temiz yedek kopyaya **ASLA yazılmaz, değiştirilmez ve silinmez**.
- Tüm modlama, test, tersine mühendislik ve geliştirme çalışmaları istisnasız `work\` klasörü (`work\VetOs_Game`) üzerinde yürütülür.

---

### 4. Süreklilik ve Modeller Arası Devir
- Projeyi yeni devralan her model/ajan çalışmaya başlamadan önce mutlaka `PLAN.md`, `PROGRESS.md` ve `ANALYSIS.md` dosyalarını okuyarak güncel durumu ve mimariyi kavramalıdır.
- Her aşama ve kritik kilometre taşı tamamlandığında `PROGRESS.md` dosyası güncellenmeli ve Git commit (`git commit`) alınarak durum kalıcılaştırılmalıdır.
- Orijinal ikili dosyalar, çıkarılan assetler ve decompile edilen kaynak kodlar `.gitignore` kurallarına uygun olarak depoya eklenmez; sadece dokümantasyon, kurallar ve geliştirilen mod/eklenti kaynakları Git'e dahil edilir.

---

### 5. İnternet / Veri Kotası Kuralı
- `10.11.2026 saat 06:00`'a kadar yapılacak tüm işlemlerde toplam internet indirmesi **kesinlikle 100 MB'ı aşmamalıdır** (mümkünse 0 MB / tamamen çevrimdışı çalışılmalıdır).
- **İstisna / Güncelleme:** APK bulut derleme işlemi (GitHub Actions hazırlığı, repo push vb.) için internet kotası **600 MB** olarak güncellenmiştir.
- Büyük boyutlu indirmeler (yerel Android SDK/NDK vb.) yalnızca bu saatten sonra veya kullanıcı açıkça onay verdiğinde yapılabilir.

---

### 6. Bekleyen Kararlar ve Notlar
- `UNUTMA`: Faz 2 için bekleyen kullanıcı sorusu ("Kemik/pin isimlerinin görünüm tercihi: A / B / C") APK derleme görevi sonrasında cevaplanmak üzere beklemeye alınmıştır. APK süreci biter bitmez kullanıcıya sunulacaktır.
