using System.IO;
using System.Text.Json;
using GearOS.Models;

namespace GearOS.Utilities
{
    public static class ProfileManager
    {
        private static string FilePath = "profiles.json";

        public static void SaveProfile(DeviceProfile profile)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(profile, options);
            File.WriteAllText(FilePath, json);
        }

        public static DeviceProfile LoadProfile()
        {
            if (!File.Exists(FilePath)) return null;
            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<DeviceProfile>(json);
        }
    }
}