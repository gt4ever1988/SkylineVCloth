namespace SkylineVCloth.Classes
{
    public class DumpComponentVariant
    {
        public string? NameHash { get; set; }

        public string? ComponentType { get; set; }

        public byte ComponentId { get; set; }

        public ushort DrawableId { get; set; }

        public byte TextureId { get; set; }

        public byte RelativeCollectionDrawableId { get; set; }
    }
}
