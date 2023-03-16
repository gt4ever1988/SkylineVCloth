using AltV.Net;

namespace SkylineVCloth.Classes
{
    public class DumpResult
    {
        #region General
        public string? DlcCollectionName { get; set; } = null;

        public string? PedName { get; set; } = null;
        #endregion

        #region Element
        public bool IsCloth { get; set; }

        public bool IsBlacklist { get; set; } = false;

        public string? NameHash { get; set; } = null;

        public string? ComponentType { get; set; } = null;

        public byte ComponentId { get; set; }

        public ushort DrawableId { get; set; }

        public byte TextureId { get; set; }

        public byte RelativeCollectionDrawableId { get; set; }
        #endregion

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dumpRoot"></param>
        /// <param name="dumpComponent"></param>
        public DumpResult(DumpRoot dumpRoot, DumpComponentVariant dumpComponent)
        {
            #region General
            DlcCollectionName = dumpRoot.DlcCollectionName;
            PedName = dumpRoot.PedName;
            #endregion

            #region Element
            IsCloth = true;
            NameHash = dumpComponent.NameHash;
            ComponentType = dumpComponent.ComponentType;
            ComponentId = dumpComponent.ComponentId;
            DrawableId = dumpComponent.DrawableId;
            TextureId = dumpComponent.TextureId;
            RelativeCollectionDrawableId = dumpComponent.RelativeCollectionDrawableId;
            #endregion
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dumpRoot"></param>
        /// <param name="dumpProp"></param>
        public DumpResult(DumpRoot dumpRoot, DumpPropVariant dumpProp)
        {
            #region General
            DlcCollectionName = dumpRoot.DlcCollectionName;
            PedName = dumpRoot.PedName;
            #endregion

            #region Element
            IsCloth = false;
            NameHash = dumpProp.NameHash;
            ComponentType = dumpProp.ComponentType;
            ComponentId = dumpProp.ComponentId;
            DrawableId = dumpProp.DrawableId;
            TextureId = dumpProp.TextureId;
            RelativeCollectionDrawableId = dumpProp.RelativeCollectionDrawableId;
            #endregion
        }

        /// <summary>
        /// Lade DLC-Collection-Name Hash
        /// HINWEIS: mp_m_freemode_01 + mp_f_freemode_01 = DLC: basegame
        /// </summary>
        /// <returns></returns>
        public uint DlcCollectionNameHash() =>
            (DlcCollectionName == Cloth.PedMale || DlcCollectionName == Cloth.PedFemale) ? 0 : Alt.Hash(DlcCollectionName);

        /// <summary>
        /// Lade Ped-Name Hash
        /// </summary>
        /// <returns></returns>
        public uint PedNameHash() =>
            Alt.Hash(PedName);
    }
}