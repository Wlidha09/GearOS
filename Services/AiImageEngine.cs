using System;
using System.IO;

namespace GearOS.Services
{
    public static class AiImageEngine
    {
        // Returns the path to an image for a device, or null if not found
        public static string GetDeviceImage(string deviceName)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var candidate = Path.Combine(baseDir, "assets", deviceName + ".png");
            if (File.Exists(candidate)) return candidate;
            return null;
        }

        // Uncomment and implement when DALL-E 3 integration is desired
        // public static async Task<string> GenerateDeviceImageAsync(string deviceName, string description)
        // {
        //     // Placeholder for DALL-E 3 generation using OpenAI APIs
        //     throw new NotImplementedException();
        // }
    }
}
