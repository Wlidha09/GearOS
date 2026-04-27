# GearOS (Projet CartagOS) - WPF Prototype

GearOS est une application de gestion de périphériques gaming (Tartarus, Souris, Claviers) permettant le remapping de touches, la création de macros et la détection automatique de profils selon le jeu lancé.

## 🚀 État du Projet
- **Architecture :** C# / .NET 10 (WPF)
- **Gestion Hardware :** Utilisation de `HidSharp` pour la détection USB/HID.
- **Moteur d'Input :** `InputSimulatorPlus` pour l'exécution des macros et remaps.
- **Interface :** Design moderne sombre avec conversion dynamique SVG via `SkiaSharp`.

## 📂 Structure du Projet (Mise à jour)

### 1. Models (Données)
- **DeviceInfo.cs** : Définit les propriétés physiques du périphérique (VendorID, ProductID, Keys).
- **Profile.cs** : **Unique source** pour la classe `DeviceProfile`. Gère les noms de profils, les processus cibles (ex: `cs2.exe`) et les listes de touches.
- **KeyMapping.cs** : Définit les types d'actions (Remap, Macro, AppLauncher).

### 2. Services (Logique Métier)
- **HardwareHandler.cs** : Scan du bus USB et filtrage des périphériques via liste noire.
- **AiImageEngine.cs** : Gestionnaire d'assets pour les visuels des périphériques.
- **KeyMappingPanel.cs** : Assistant UI pour la configuration dynamique.

### 3. Utilities (Outils système)
- **GameDetector.cs** : Surveillance en temps réel du processus au premier plan pour l'auto-switch de profil.
- **ProfileManager.cs** : Persistance des données via `profiles.json` (Sérialisation System.Text.Json).
- **MappingEngine.cs** : Traduction des commandes logiques en entrées clavier/souris réelles.
- **AdminHelper.cs** : Vérification et demande des droits Administrateur (requis pour simuler des touches dans certains jeux).
- **SvgToImageConverter.cs** : Convertisseur XAML pour l'affichage des logos de marques.

## 🛠️ Installation & Compilation
1. S'assurer que le fichier `DeviceProfile.cs` est **supprimé** (doublon avec `Profile.cs`).
2. Nettoyer les dossiers `bin` et `obj` pour éviter les erreurs de cache de compilation.
3. Restaurer les packages NuGet (`HidSharp`, `InputSimulatorPlus`, `SkiaSharp`).
4. Lancer la solution via **GearOS.slnx**.

## 📝 À Faire
- [ ] Finaliser l'intégration OpenRGB pour le contrôle des LEDs.
- [ ] Étendre la base de données `DeviceDetector` pour plus de périphériques Razer/Logitech.
- [ ] Ajouter une interface de création de macros visuelle (Timeline).