using System;
using System.Collections.Generic;

namespace GearOS.Models
{
    public class Game
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string ExecutablePath { get; set; }
        public string ExecutableName { get; set; }
        public string Platform { get; set; } // Steam, Epic, Battle.net, EA, Ubisoft, Rockstar, Custom
        public string PlatformID { get; set; } // AppID, etc.
        public string ImagePath { get; set; }
        public DeviceProfile AssignedProfile { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public bool IsInstalled { get; set; } = true;
    }
}
