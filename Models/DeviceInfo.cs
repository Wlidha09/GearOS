using System.Collections.Generic;

namespace GearOS.Models
{
    public class DeviceKey
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public KeyMapping Mapping { get; set; } = new KeyMapping();
    }


    public class DeviceInfo
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Vendor { get; set; }
        public ushort VendorId { get; set; }
        public int ProductId { get; set; }
        public string Category { get; set; }
        public System.Collections.Generic.List<DeviceKey> Keys { get; set; } = new System.Collections.Generic.List<DeviceKey>();
        public DeviceProfile ActiveProfile { get; set; }
    }
}