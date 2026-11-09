---
name: unity-mono-headless-inspector
description: >-
  ilspycmd ve statik AST ayrıştırıcılar kullanarak Unity Mono CIL bytecode'larını (Assembly-CSharp.dll)
  komut satırından C# koduna döker, sınıf ilişkilerini ve UI/kamera sembollerini indeksler.
---

# Unity Mono Headless Inspector Skill

Bu yetenek, GUI decompiler araçlarına gerek kalmadan, doğrudan terminal üzerinden Unity Managed DLL'lerini tersine mühendislikle çözümler.

## Prosedür

1. **Ortam Değişkeni:**
   - .NET 8 üzerinde `ilspycmd` çalıştırırken `.NET 6` bağımlılık uyarısını aşmak için ortam değişkenini ayarla:
     `$env:DOTNET_ROLL_FORWARD="Major"`

2. **Decompile Komutu:**
   ```powershell
   ilspycmd -p -o "c:\Antigravity Projects Hub\VetOs 3D Enhanced\decompiled" "work\VetOs_Game\VetOs_Data\Managed\Assembly-CSharp.dll"
   ```

3. **Sembol Taraması:**
   - UI etkileşimleri, kamera kontrolleri, API istekleri ve gizli mod bayraklarını (`Vars.AdminMode`) tespit et.
