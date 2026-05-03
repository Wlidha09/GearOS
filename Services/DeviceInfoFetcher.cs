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
        private static readonly HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };

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

            // 2. Chercher sur OpenHardware API
            var spec = await FetchFromOpenHardwareAsync(device);

            // 3. Si pas trouvé, générer basé sur le nom
            if (spec == null)
                spec = GenerateDeviceSpec(device);

            // 4. Sauvegarder en cache
            _database.SaveDeviceSpec(spec);

            return spec;
        }

        private async Task<DeviceDatabase.DeviceSpec> FetchFromOpenHardwareAsync(DeviceInfo device)
        {
            try
            {
                // OpenHardware API endpoint
                string url = $"https://api.openhardwaremonitor.org/devices?vendorid={device.VendorId:X4}&productid={device.ProductId:X4}";
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var devices = JsonSerializer.Deserialize<List<OpenHardwareDevice>>(json, options);

                    if (devices?.Count > 0)
                    {
                        var hwDevice = devices[0];
                        return new DeviceDatabase.DeviceSpec
                        {
                            VendorID = device.VendorId.ToString("X4"),
                            ProductID = device.ProductId.ToString("X4"),
                            Name = hwDevice.Name ?? device.Name,
                            KeyCount = DetectKeyCount(hwDevice.Type),
                            HasRGB = hwDevice.Lighting == "RGB",
                            Type = hwDevice.Type ?? "Generic",
                            ImageUrl = hwDevice.Image,
                            LastUpdated = DateTime.Now
                        };
                    }
                }
            }
            catch { }

            return null;
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
                    ImageUrl = "/Assets/tartarus_v2.png",
                    LastUpdated = DateTime.Now
                };
            }
            else if (name.Contains("MOUSE"))
            {
                int keyCount = name.Contains("G502") ? 11 : name.Contains("RIVAL") ? 7 : 5;
                return new DeviceDatabase.DeviceSpec
                {
                    VendorID = device.VendorId.ToString("X4"),
                    ProductID = device.ProductId.ToString("X4"),
                    Name = device.Name,
                    KeyCount = keyCount,
                    HasRGB = name.Contains("RGB") || name.Contains("HERO") || name.Contains("PRO"),
                    Type = "Mouse",
                    ImageUrl = null,
                    LastUpdated = DateTime.Now
                };
            }
            else if (name.Contains("KEYBOARD") || name.Contains("MECHANICAL"))
            {
                return new DeviceDatabase.DeviceSpec
                {
                    VendorID = device.VendorId.ToString("X4"),
                    ProductID = device.ProductId.ToString("X4"),
                    Name = device.Name,
                    KeyCount = 20,
                    HasRGB = name.Contains("RGB") || name.Contains("CHROMA") || name.Contains("RAZER"),
                    Type = "Keyboard",
                    ImageUrl = null,
                    LastUpdated = DateTime.Now
                };
            }
            else if (name.Contains("GAMEPAD") || name.Contains("CONTROLLER") || name.Contains("XBOX") || name.Contains("PS4"))
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

        private int DetectKeyCount(string deviceType)
        {
            return deviceType?.ToUpper() switch
            {
                "MOUSE" => 8,
                "KEYBOARD" => 20,
                "GAMEPAD" => 14,
                "TARTARUS" => 22,
                _ => 8
            };
        }

        private class OpenHardwareDevice
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Lighting { get; set; }
            public string Image { get; set; }
        }
    }
}
