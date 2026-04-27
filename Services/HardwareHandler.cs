using HidSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using GearOS.Models;

namespace GearOS.Services
{

    public class HardwareHandler
    {
        public List<DeviceInfo> GetAllConnectedDevices()
        {
            var devices = new List<DeviceInfo>();
            var hidDevices = DeviceList.Local.GetHidDevices();
            string[] blacklist = { "ITE DEVICE", "UNKNOWN", "INCONNU", "CONTROL", "INTERFACE" };

            foreach (var dev in hidDevices)
            {
                try
                {
                    string pName = dev.GetProductName() ?? "Périphérique Inconnu";
                    string n = pName.ToUpper();

                    if (blacklist.Any(b => n.Contains(b))) continue;
                    if (n.Contains("WEBCAM") || n.Contains("MICROPHONE")) continue;

                    if (!string.IsNullOrEmpty(pName) && !devices.Any(d => d.ProductId == dev.ProductID))
                    {
                        devices.Add(new DeviceInfo
                        {
                            Name = pName,
                            Vendor = dev.Manufacturer ?? "Générique",
                            VendorId = dev.VendorID,
                            ProductId = (int)dev.ProductID,
                            Category = n.Contains("MOUSE") ? "Souris" : n.Contains("KEYBOARD") ? "Clavier" : "Périphérique"
                        });
                    }
                }
                catch { }
            }
            return devices;
        }
    }
}
