namespace GearOS.Models
{
    public enum MappingType { Remap, Macro, AppLauncher }

    public class KeyMapping
    {
        public string PhysicalKey { get; set; } = "";
        public string TargetAction { get; set; } = "";
        public MappingType Type { get; set; } = MappingType.Remap;
    }
}