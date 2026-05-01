using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GearOS.Models;
using System.Text.Json;

namespace GearOS.Services
{
    public class DeviceInfoFetcher
    {
        private readonly DeviceDatabase _database;
        private static readonly HttpClient client = new HttpClient();

        public DeviceInfoFetcher()
        {
            _database = new DeviceDatabase();
        }

        public async Task<DeviceDatabase.DeviceSpec> FetchDeviceInfoAsync(DeviceInfo device)
        {
            // 1. Chercher en cache local
            var cached = _database.GetDeviceSpec(device.VendorId, device.ProductId);
            if (cached != null)
                return cached;

            // 2. Créer une spec basée sur le nom du périphérique
            var spec = GenerateDeviceSpec(device);

            // 3. Essayer de chercher sur internet (fallback)
            try
            {
                var webSpec = await FetchFromWebAsync(device);
                if (webSpec != null)
                {
                    spec = webSpec;
                }
            }
            catch
            {
                // Fallback silencieux sur spec générée
            }

            // 4. Sauvegarder en cache
            _database.SaveDeviceSpec(spec);

            return spec;
        }

        private DeviceDatabase.DeviceSpec GenerateDeviceSpec(DeviceInfo device)
        {
            string name = device.Name.ToUpper();

            if (name.Contains("TARTARUS"))
            {
                return new DeviceDatabase.DeviceSpec
                {
                    VendorID = device.VendorId.ToString("X4"),
                    ProductID = device.ProductId.ToString("X4"),
                    Name = device.Name,
                    KeyCount = 22,
                    HasRGB = true,
                    Type = "Tartarus",
                    ImageUrl = "tartarus_v2.png",
                    LastUpdated = DateTime.Now
                };
            }
            else if (name.Contains("MOUSE"))
            {
                return new DeviceDatabase.DeviceSpec
                {
                    VendorID = device.VendorId.ToString("X4"),
                    ProductID = device.ProductId.ToString("X4"),
                    Name = device.Name,
                    KeyCount = 5,
                    HasRGB = name.Contains("RGB"),
                    Type = "Mouse",
                    ImageUrl = null,
                    LastUpdated = DateTime.Now
                };
            }
            else if (name.Contains("KEYBOARD"))
            {
                return new DeviceDatabase.DeviceSpec
                {
                    VendorID = device.VendorId.ToString("X4"),
                    ProductID = device.ProductId.ToString("X4"),
                    Name = device.Name,
                    KeyCount = 20,
                    HasRGB = name.Contains("RGB") || name.Contains("MECHANICAL"),
                    Type = "Keyboard",
                    ImageUrl = null,
                    LastUpdated = DateTime.Now
                };
            }
            else if (name.Contains("GAMEPAD") || name.Contains("CONTROLLER"))
            {
                return new DeviceDatabase.DeviceSpec
                {
                    VendorID = device.VendorId.ToString("X4"),
                    ProductID = device.ProductId.ToString("X4"),
                    Name = device.Name,
                    KeyCount = 14,
                    HasRGB = false,
                    Type = "Gamepad",
                    ImageUrl = null,
                    LastUpdated = DateTime.Now
                };
            }
            else
            {
                // Génération générique
                return new DeviceDatabase.DeviceSpec
                {
                    VendorID = device.VendorId.ToString("X4"),
                    ProductID = device.ProductId.ToString("X4"),
                    Name = device.Name,
                    KeyCount = 8,
                    HasRGB = false,
                    Type = "Generic",
                    ImageUrl = null,
                    LastUpdated = DateTime.Now
                };
            }
        }

        private async Task<DeviceDatabase.DeviceSpec> FetchFromWebAsync(DeviceInfo device)
        {
            try
            {
                // Exemple : Chercher sur OpenHardware ou Wikipedia
                // Pour l'instant, retourne null (à implémenter avec une véritable API)
                await Task.Delay(100); // Simule une requête réseau
                return null;
            }
            catch
            {
                return null;
            }
        }

        public void AddCustomDevice(DeviceInfo device, int keyCount, bool hasRGB, string imageUrl = null)
        {
            var spec = new DeviceDatabase.DeviceSpec
            {
                VendorID = device.VendorId.ToString("X4"),
                ProductID = device.ProductId.ToString("X4"),
                Name = device.Name,
                KeyCount = keyCount,
                HasRGB = hasRGB,
                Type = "Custom",
                ImageUrl = imageUrl,
                LastUpdated = DateTime.Now
            };

            _database.SaveDeviceSpec(spec);
        }
    }
}
