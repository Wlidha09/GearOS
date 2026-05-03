using System.Collections.Generic;

namespace GearOS.Models
{
    public class LightingProfile
    {
        public string Name { get; set; } = "Default Lighting";
        public bool Enabled { get; set; } = false;
        public int Brightness { get; set; } = 100; // 0-100
        public string Mode { get; set; } = "Static"; // Static, Breathing, Rainbow, Wave, etc.
        public string Color { get; set; } = "#00FF00"; // Hex color
        public int Speed { get; set; } = 50; // 0-100
        public bool UseProfileLighting { get; set; } = true; // Si false, utilise GlobalLighting
        public Dictionary<int, string> PerKeyColors { get; set; } = new Dictionary<int, string>();
    }
}
