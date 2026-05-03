using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GearOS.Models;

namespace GearOS.Utilities
{
    public class RGBController
    {
        private DeviceInfo _device;
        private LightingProfile _globalLighting;

        public RGBController(DeviceInfo device)
        {
            _device = device;
            _globalLighting = new LightingProfile { Name = "Global Lighting" };
        }

        public void SetGlobalLighting(LightingProfile profile)
        {
            _globalLighting = profile;
            ApplyLightingAsync().Wait();
        }

        public LightingProfile GetGlobalLighting() => _globalLighting;

        public async Task ApplyLightingAsync()
        {
            if (!_globalLighting.Enabled)
                return;

            try
            {
                // TODO: Implémenter communication HID avec le périphérique
                // Cela dépend du protocole spécifique du périphérique
                // Exemple pour Razer Tartarus:
                // - Envoyer commande USB via HidSharp
                // - Format: [0x00, MODE, BRIGHTNESS, R, G, B, SPEED]

                await Task.Delay(100); // Simulation
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RGB Error: {ex.Message}");
            }
        }

        public void SetStaticColor(string hexColor)
        {
            _globalLighting.Mode = "Static";
            _globalLighting.Color = hexColor;
            ApplyLightingAsync().Wait();
        }

        public void SetBreathing(string hexColor, int speed)
        {
            _globalLighting.Mode = "Breathing";
            _globalLighting.Color = hexColor;
            _globalLighting.Speed = speed;
            ApplyLightingAsync().Wait();
        }

        public void SetRainbow(int speed)
        {
            _globalLighting.Mode = "Rainbow";
            _globalLighting.Speed = speed;
            ApplyLightingAsync().Wait();
        }

        public void SetBrightness(int brightness)
        {
            _globalLighting.Brightness = Math.Clamp(brightness, 0, 100);
            ApplyLightingAsync().Wait();
        }
    }
}
