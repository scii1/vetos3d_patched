---
name: unity-comparative-anatomy-matrix
description: >-
  VetOs uygulamasındaki katı hayvan eşleşmesini (Koyun-Köpek ve Sığır-At) kırarak
  kullanıcının dilediği 2 hayvan türünü çift kamera (Camera.rect) ile senkronize karşılaştırmasını sağlar.
---

# Unity Comparative Anatomy Matrix Skill

Bu yetenek, `Vars.cs` içerisindeki `GroupNames` ve `CompGroupNames` sabit dizi eşleşmelerini Harmony yama mantığıyla dinamik serbest matrise dönüştürür.

## Prosedür

1. **Sabit Eşleşme Sınırlaması:**
   - Orijinal kodda indeks 0 (Koyun) doğrudan 3 (Köpek), indeks 1 (Sığır) doğrudan 2 (At) ile eşleştirilmiştir.
   - Bu durum Köpek-At veya Sığır-Koyun karşılaştırmasını engeller.

2. **Harmony Prefix Yaması:**
   - Hayvan seçim panelinde kullanıcının seçtiği birincil ve ikincil hayvan indekslerini yakalar.
   - `Camera_Controller.TakeAndStoreBoneInf` çağrısına dinamik kod haritası (`CompGroupCodes`) sağlar.

3. **Çift Kamera Senkronizasyonu:**
   - Fare dönüş ve yakınlaşma girdilerini `Main Camera` ve `Second Camera` nesnelerine eş zamanlı uygular.
