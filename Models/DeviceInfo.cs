using System.Collections.Generic;

namespace GearOS.Models
{
    public class DeviceInfo
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Vendor { get; set; }
        public ushort VendorId { get; set; }
        public int ProductId { get; set; } // Doit être un int ou ushort pour HidSharp
        public string Category { get; set; }
        public List<DeviceKey> Keys { get; set; } = new List<DeviceKey>();
        public DeviceProfile ActiveProfile { get; set; }
    }
}