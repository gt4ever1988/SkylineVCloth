using AltV.Net;
using AltV.Net.Async.Elements.Entities;
using AltV.Net.Data;
using Newtonsoft.Json;
using SkylineVCloth.Classes;

namespace SkylineVCloth
{
    public class Cloth : IScript
    {
        /// <summary>
        /// Globale Speicherung
        /// </summary>
        public static List<DumpResult> Results = new();

        /// <summary>
        /// Skin Männlich
        /// </summary>
        public const string PedMale = "mp_m_freemode_01";

        /// <summary>
        /// Skin Weiblich
        /// </summary>
        public const string PedFemale = "mp_f_freemode_01";

        /// <summary>
        /// Original Dump User (Github)
        /// </summary>
        public const string OriginalGithubDumpUser = "DurtyFree";

        /// <summary>
        /// Original Dump Repository (Github)
        /// </summary>
        public const string OriginalGithubDumpRepository = "gta-v-data-dumps";

        /// <summary>
        /// Original Dump Datei (Github)
        /// </summary>
        public const string OriginalGithubDumpFile = "pedComponentVariations_free.json";

        /// <summary>
        /// Modding Dump-Verzeichnis
        /// </summary>
        public static string OriginalDumpDirectory = "data";

        /// <summary>
        /// Modding Dump-Datei
        /// </summary>
        public static string OriginalDumpFilename = "pedComponentVariations_free.json";

        /// <summary>
        /// Modding Dump-Verzeichnis
        /// </summary>
        public static string ModdingDumpDirectory = "resources";

        /// <summary>
        /// Modding Dump-Datei
        /// </summary>
        public static string ModdingDumpFilename = "clothes_dump.json";

        /// <summary>
        /// Nackt Männlich
        /// </summary>
        private static List<string> NakedMale = new()
        {
            "SP_M_BERD_0_0",
            "SP_M_HAIR_0_0",
            "SP_M_UPPR_15_0",
            "DLC_MP_VAL2_M_LEGS_1_0",
            "SP_M_HAND_0_0",
            "DLC_MP_APA_M_FEET_1_0",
            "SP_M_TEEF_0_0",
            "SP_M_ACCS_15_0",
            "SP_M_TASK_0_0",
            "SP_M_DECL_0_0",
            "SP_M_JBIB_15_0",
            "SP_M_HEAD_8_0_1",
            "SP_M_EYES_0_0",
            "DLC_MP_LUXE2_M_PEARS_12_0",
            "DLC_MP_VWD_M_PLEFT_WRIST_6_0",
            "DLC_MP_BIKER_M_PRIGHT_WRIST_0_0"
        };

        /// <summary>
        /// Nackt Weiblich
        /// </summary>
        private static List<string> NakedFemale = new()
        {
            "SP_F_BERD_0_0",
            "SP_F_HAIR_0_0",
            "SP_F_UPPR_15_0",
            "SP_F_LOWR_15_0",
            "SP_F_HAND_0_0",
            "DLC_MP_APA_F_FEET_1_0",
            "SP_F_TEEF_0_0",
            "SP_F_ACCS_15_0",
            "SP_F_TASK_0_0",
            "SP_F_DECL_0_0",
            "SP_F_JBIB_15_0",
            "DLC_MP_X17_F_PHEAD_0_0",
            "SP_F_EYES_5_0",
            "DLC_MP_LUXE_F_PEARS_9_0",
            "SP_F_LEFT_WRIST_1_0",
            "DLC_MP_BIKER_F_PRIGHT_WRIST_0_0"
        };

        /// <summary>
        /// Initialisierung
        /// </summary>
        /// <param name="loadingFromGithub">Gibt an ob die Originale GTA-Daten von Github live geladen werden sollen (true) oder nur lokale Daten (false)</param>
        /// <param name="showStatistic">Gibt an ob die Statistik ausgegeben werden soll</param>
        public static async Task InitAsync(bool loadingFromGithub = true, bool showStatistic = true)
        {
            // Laden von Github (NEU)
            if (loadingFromGithub)
            {
                // Erstelle Http-Verbindung
                using HttpClient httpClient = new();

                // Logging
                Log("OriginalGithubDump", $"Loading {OriginalGithubDumpFile} from {OriginalGithubDumpUser}-Github ...");

                try
                {
                    // Lade Dump
                    HttpResponseMessage responseDump = await httpClient.GetAsync($"https://raw.githubusercontent.com/{OriginalGithubDumpUser}/" +
                        $"{OriginalGithubDumpRepository}/master/{OriginalGithubDumpFile}");

                    // Erfolgreich
                    if (responseDump.IsSuccessStatusCode)
                    {
                        // Lade Dump & extrahiere Daten
                        ExtractFromDump(string.Empty, await responseDump.Content.ReadAsStringAsync());

                        // Logging
                        Log("OriginalGithubDump", $"Loading {OriginalGithubDumpFile} from {OriginalGithubDumpUser}-Github ... [OK]");
                    }
                    // Fehlerhaft (Dump)
                    else
                    {
                        // Logging
                        Log("OriginalGithubDump", $"Loading {OriginalGithubDumpFile} from {OriginalGithubDumpUser}-Github ... " +
                            $"[FAILED:{responseDump.StatusCode}]");
                    }
                }
                catch (Exception exception)
                {
                    // Logging
                    Log("OriginalGithubDump", $"Loading {OriginalGithubDumpFile} from {OriginalGithubDumpUser}-Github ... " +
                        $"[FAILED:{exception.Message}]");
                }
            }
            // Laden von Lokal (ALT)
            else
            {
                // Finde Original-Dumps
                FindFile(OriginalDumpDirectory, OriginalDumpFilename);
            }

            // Finde Custom-Dumps
            FindFile(ModdingDumpDirectory, ModdingDumpFilename);

            // Gib Statistik aus
            if (showStatistic) Statistic();
        }

        /// <summary>
        /// Gibt alle aktuellen Klamotten-Anzahl je Komponente/Geschlecht aus
        /// </summary>
        public static void Statistic()
        {
            // Kleidung-Komponenten (ausgenommen Head)
            for (byte componentId = 1; componentId <= 11; componentId++)
                Log("Statistic", $"Cloth {componentId}: (Male) {GetCloths(true, componentId).Count}, (Female) {GetCloths(false, componentId).Count}");

            // Zubehör-Komponenten
            foreach (byte componentId in new List<byte>() { 0, 1, 2, 6, 7 })
                Log("Statistic", $"Prop {componentId}: (Male) {GetProps(true, componentId).Count}, (Female) {GetProps(false, componentId).Count}");
        }

        /// <summary>
        /// Setzte Blacklist
        /// </summary>
        /// <param name="nameHashs">Gibt eine Liste von NameHashs an</param>
        public static void Blacklist(List<string> nameHashs)
        {
            // Gehe Hashs durch
            foreach (string nameHash in nameHashs)
            {
                // Blacklist
                Blacklist(nameHash);
            }
        }

        /// <summary>
        /// Setzte Blacklist
        /// </summary>
        /// <param name="nameHash">Gibt den NameHash oder ein JSON mit einer Liste von NameHashs an</param>
        public static void Blacklist(string nameHash)
        {
            // Entferne Leerzeichen (Beginn/Ende)
            nameHash = nameHash.Trim();

            // Validierung
            if (string.IsNullOrEmpty(nameHash)) return;

            // Wenn JSON übergeben
            if ((nameHash.StartsWith("{") && nameHash.EndsWith("}")) ||
                (nameHash.StartsWith("[") && nameHash.EndsWith("]")))
            {
                // Blacklist
                Blacklist(JsonConvert.DeserializeObject<List<string>>(nameHash));
            }
            // Wenn String übergeben
            else
            {
                // Lade Stammdaten
                DumpResult? dump = Results.FirstOrDefault(x => x.NameHash == nameHash);

                // Wenn Stammdaten gefunden
                if (dump != null)
                {
                    // Setzte Blacklist
                    dump.IsBlacklist = true;
                }
            }
        }

        /// <summary>
        /// Finde DUMP-Datei/Extrahiere Ergebnisse
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="searchPattern"></param>
        private static void FindFile(string folder, string searchPattern)
        {
            // Lade Dateien
            foreach (string file in Directory.GetFiles(folder, searchPattern))
            {
                // Extrahiere
                ExtractFromDump(file, File.ReadAllText(file));
            }

            // Rekursion
            foreach (string subfolder in Directory.GetDirectories(folder))
                FindFile(subfolder, searchPattern);
        }

        /// <summary>
        /// Extrahiere
        /// </summary>
        /// <param name="file"></param>
        /// <param name="content"></param>
        private static void ExtractFromDump(string file, string content)
        {
            // Logging
            if (file != string.Empty) Log("Extract", $"Loading {file} ...");

            try
            {
                // Extrahiere JSON
                foreach (DumpRoot dumpRoot in JsonConvert.DeserializeObject<List<DumpRoot>>(content))
                {
                    // Extrahiere Component-Varianten
                    foreach (DumpComponentVariant dumpComponent in dumpRoot.ComponentVariations)
                        Results.Add(new DumpResult(dumpRoot, dumpComponent));

                    // Extrahiere Prop-Varianten
                    foreach (DumpPropVariant dumpProp in dumpRoot.Props)
                        Results.Add(new DumpResult(dumpRoot, dumpProp));
                }

                // Logging
                if (file != string.Empty) Log("Extract", $"Loading {file} ... [OK]");
            }
            catch (Exception exception)
            {
                // Logging
                if (file != string.Empty) Log("Extract", $"Loading {file} ... [FAILED:{exception.Message}]");
            }
        }

        /// <summary>
        /// Lade Kleidung-Stammdaten
        /// </summary>
        /// <param name="componentId"></param>
        /// <param name="cloth"></param>
        /// <returns></returns>
        private static DumpResult? Get(byte componentId, DlcCloth cloth) => Results.FirstOrDefault(x => x.IsCloth == true && x.ComponentId == componentId &&
            x.RelativeCollectionDrawableId == cloth.Drawable && x.TextureId == cloth.Texture && x.DlcCollectionNameHash() == cloth.Dlc);

        /// <summary>
        /// Lade Prop-Stammdaten
        /// </summary>
        /// <param name="componentId"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        private static DumpResult? Get(byte componentId, DlcProp prop) => Results.FirstOrDefault(x => x.IsCloth == false && x.ComponentId == componentId &&
            x.RelativeCollectionDrawableId == prop.Drawable && x.TextureId == prop.Texture && x.DlcCollectionNameHash() == prop.Dlc);

        /// <summary>
        /// Lade Kleidungs-Stammdaten
        /// </summary>
        /// <param name="male"></param>
        /// <param name="componentId"></param>
        /// <param name="includeBlacklist"></param>
        /// <returns></returns>
        public static List<DumpResult> GetCloths(bool male, byte componentId, bool includeBlacklist = false) =>
            Results.FindAll(x => x.IsCloth == true && (includeBlacklist || !x.IsBlacklist) &&
            x.ComponentId == componentId && x.PedName == (male ? PedMale : PedFemale));

        /// <summary>
        /// Lade Props-Stammdaten
        /// </summary>
        /// <param name="male"></param>
        /// <param name="componentId"></param>
        /// <param name="includeBlacklist"></param>
        /// <returns></returns>
        public static List<DumpResult> GetProps(bool male, byte componentId, bool includeBlacklist = false) =>
            Results.FindAll(x => x.IsCloth == false && (includeBlacklist || !x.IsBlacklist) &&
            x.ComponentId == componentId && x.PedName == (male ? PedMale : PedFemale));

        /// <summary>
        /// Ziehe Nackt an
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool EquipNaked(AsyncPlayer player)
        {
            // Männlich
            if (player.Model == Alt.Hash(PedMale))
            {
                Equip(player, NakedMale, true);
            }
            // Weiblich
            else if (player.Model == Alt.Hash(PedFemale))
            {
                Equip(player, NakedFemale, true);
            }

            // Default
            return false;
        }

        /// <summary>
        /// Ziehe Kleidungs/Props an
        /// </summary>
        /// <param name="player"></param>
        /// <param name="nameHashs">Gibt eine Liste von NameHashs an</param>
        /// <param name="includeBlacklist"></param>
        /// <returns></returns>
        public static bool Equip(AsyncPlayer player, List<string> nameHashs, bool includeBlacklist = false)
        {
            // Global
            bool result = true;

            // Gehe Hashs durch
            foreach (string nameHash in nameHashs)
            {
                // Ziehe Kleidung/Prop an
                result &= Equip(player, nameHash, includeBlacklist);
            }

            // Rückgabe
            return result;
        }

        /// <summary>
        /// Ziehe Kleidung/Prop an
        /// </summary>
        /// <param name="player"></param>
        /// <param name="nameHash">Gibt den NameHash oder ein JSON mit einer Liste von NameHashs an</param>
        /// <param name="includeBlacklist"></param>
        /// <returns></returns>
        public static bool Equip(AsyncPlayer player, string nameHash, bool includeBlacklist = false)
        {
            // Entferne Leerzeichen (Beginn/Ende)
            nameHash = nameHash.Trim();

            // Validierung
            if (string.IsNullOrEmpty(nameHash)) return false;

            // Wenn JSON übergeben
            if ((nameHash.StartsWith("{") && nameHash.EndsWith("}")) ||
                (nameHash.StartsWith("[") && nameHash.EndsWith("]")))
            {
                // Ziehe Kleidungs/Props an
                return Equip(player, JsonConvert.DeserializeObject<List<string>>(nameHash));
            }
            // Wenn String übergeben
            else
            {
                // Lade Stammdaten
                DumpResult? dump = Results.FirstOrDefault(x => (includeBlacklist || !x.IsBlacklist) &&
                    x.NameHash == nameHash && x.PedNameHash() == player.Model);

                // Wenn Stammdaten gefunden
                if (dump != null)
                {
                    // Wenn Kleidung
                    if (dump.IsCloth)
                    {
                        return player.SetDlcClothes(dump.ComponentId, dump.RelativeCollectionDrawableId, dump.TextureId, 0, dump.DlcCollectionNameHash());
                    }
                    // Wenn Prop
                    else
                    {
                        return player.SetDlcProps(dump.ComponentId, dump.RelativeCollectionDrawableId, dump.TextureId, dump.DlcCollectionNameHash());
                    }
                }
            }

            // Default
            return false;
        }

        /// <summary>
        /// Lädt die aktuelle Kleidungs/Props
        /// </summary>
        /// <param name="player"></param>
        /// <param name="withHairStyle"></param>
        /// <param name="includeNaked"></param>
        /// <returns></returns>
        public static List<DumpResult> Currents(AsyncPlayer player, bool withHairStyle = true, bool includeNaked = false)
        {
            // Liste
            List<DumpResult> results = new();

            // Erstelle nur bei Freemode-Peds
            if (player.Model == Alt.Hash(PedMale) || player.Model == Alt.Hash(PedFemale))
            {
                // Kleidung-Komponenten (ausgenommen Head)
                for (byte componentId = 1; componentId <= 11; componentId++)
                {
                    if (componentId != 2 || withHairStyle)
                    {
                        DumpResult? dumpResult = Get(componentId, player.GetDlcClothes(componentId));
                        if (dumpResult == null) continue;
                        if (includeNaked || (!NakedMale.Contains(dumpResult.NameHash) && !NakedFemale.Contains(dumpResult.NameHash)))
                        {
                            results.Add(dumpResult);
                        }
                    }
                }

                // Zubehör-Komponenten
                foreach (byte componentId in new byte[] { 0, 1, 2, 6, 7 })
                {
                    DumpResult? dumpResult = Get(componentId, player.GetDlcProps(componentId));
                    if (dumpResult == null) continue;
                    if (includeNaked || (!NakedMale.Contains(dumpResult.NameHash) && !NakedFemale.Contains(dumpResult.NameHash)))
                    {
                        results.Add(dumpResult);
                    }
                }
            }

            // Rückgabe
            return results;
        }

        /// <summary>
        /// Lade die aktuelle Kleidungs/Props als JSON
        /// </summary>
        /// <param name="player"></param>
        /// <param name="withHairStyle"></param>
        /// <param name="includeNaked"></param>
        /// <returns></returns>
        public static string CurrentsJSON(AsyncPlayer player, bool withHairStyle = true, bool includeNaked = false) =>
            JsonConvert.SerializeObject(Currents(player, withHairStyle, includeNaked).Select(x => x.NameHash));

        /// <summary>
        /// Logging
        /// </summary>
        /// <param name="module"></param>
        /// <param name="content"></param>
        private static void Log(string module, string content) =>
            Alt.Log($"[SkylineVCloth:{module} {DateTime.Now}] {content}");
    }
}