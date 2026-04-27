using System.Collections.Generic;

namespace GearOS.Models
{
    public class DeviceProfile
    {
        // Nom du profil (ex: "Mode FPS", "Bureautique")
        public string Name { get; set; } = "Nouveau Profil";

        // C'est cette ligne qui manquait et causait l'erreur CS1061
        public string TargetProcessName { get; set; } = "";

        // Pour la compatibilité avec tes anciens fichiers
        public int MouseDPI { get; set; } = 800;

        public List<DeviceKey> Keys { get; set; } = new List<DeviceKey>();
        public List<string> AssociatedApps { get; set; } = new List<string>();
    }
}