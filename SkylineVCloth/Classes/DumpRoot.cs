namespace SkylineVCloth.Classes
{
    public class DumpRoot
    {
        public string? LastUpdateDlcName { get; set; } = null;

        public string? DlcCollectionName { get; set; } = null;

        public string? PedName { get; set; } = null;

        public List<DumpComponentVariant> ComponentVariations { get; set; } = new();

        public List<DumpPropVariant> Props { get; set; } = new();
    }
}