---
name: vet-anatomy-terminology-sync
description: >-
  VetOs 3D uygulamasında Nomina Anatomica Veterinaria (NAV) uyumlu Latince, Türkçe ve İngilizce
  anatomik terminoloji eşlemesini, terms.json sözlüğünü ve UI metin kancalarını yönetir.
---

# Vet Anatomy Terminology Sync Skill

Bu yetenek, VetOs uygulamasındaki Türkçe dil eksikliğini (`DBInfBones` ve `DBInfPins` sınıflarında Türkçe alan bulunmaması) harici bir JSON sözlüğü ve Harmony yamasıyla çözer.

## Prosedür

1. **Sözlük Şeması (`assets\terms.json`):**
   - Her anatomik kemik ve pin yapısı için standart alanlar:
     ```json
     {
       "BoneKey": "Dog_Femur",
       "LatineName": "Os femoris",
       "TurkishName": "Uyluk Kemiği",
       "EnglishName": "Thigh bone / Femur",
       "Description_TR": "Uyluk iskeletini oluşturan en uzun ve en güçlü tubuler kemiktir."
     }
     ```

2. **Harmony Enjeksiyon Noktaları:**
   - `UI_TreeViewController.ShowBoneInfo`: `Vars.Language == Languages.Turkish` kontrolünde `CurBone.LatineName` yerine `terms.json` karşılığını arayüze basar.
   - `UI_PinPoint_Controller`: Pin üzerine gelindiğinde `CurrentPinToolTipText` alanına Türkçe açıklama enjekte eder.

3. **Doğrulama:**
   - JSON sözlüğündeki terimlerin NAV 6. Edisyon standartlarına uygunluğunu denetle.
