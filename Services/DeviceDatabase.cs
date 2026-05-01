using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using GearOS.Models;

namespace GearOS.Services
{
    public class DeviceDatabase
    {
        private readonly string _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "devices-database.json");
        private Dictionary<string, DeviceSpec> _cache;

        public class DeviceSpec
        {
            public string VendorID { get; set; }
            public string ProductID { get; set; }
            public string Name { get; set; }
            public string ImageUrl { get; set; }
            public int KeyCount { get; set; }
            public bool HasRGB { get; set; }
            public string Type { get; set; } // Tartarus, Mouse, Keyboard, Gamepad
            public List<int> KeyPositionsX { get; set; }
            public List<int> KeyPositionsY { get; set; }
            public DateTime LastUpdated { get; set; }
        }

        public DeviceDatabase()
        {
            LoadCache();
        }

        private void LoadCache()
        {
            _cache = new Dictionary<string, DeviceSpec>();

            try
            {
                if (File.Exists(_dbPath))
                {
                    var json = File.ReadAllText(_dbPath);
                    var devices = JsonSerializer.Deserialize<List<DeviceSpec>>(json);
                    if (devices != null)
                    {
                        foreach (var device in devices)
                        {
                            string key = $"{device.VendorID}:{device.ProductID}";
                            _cache[key] = device;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur chargement DB: {ex.Message}");
            }
        }

        public DeviceSpec GetDeviceSpec(ushort vendorId, int productId)
        {
            string key = $"{vendorId:X4}:{productId:X4}";
            
            if (_cache.ContainsKey(key))
                return _cache[key];

            return null;
        }

        public void SaveDeviceSpec(DeviceSpec spec)
        {
            string key = $"{spec.VendorID}:{spec.ProductID}";
            _cache[key] = spec;
            PersistToFile();
        }

        private void PersistToFile()
        {
            try
            {
                var dirPath = Path.GetDirectoryName(_dbPath);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                var devices = new List<DeviceSpec>(_cache.Values);
                var json = JsonSerializer.Serialize(devices, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_dbPath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur save DB: {ex.Message}");
            }
        }

        public void ClearCache()
        {
            _cache.Clear();
        }
    }
}
