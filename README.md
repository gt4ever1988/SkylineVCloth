## Minimaler Testaufbau
Grunddaten laden, bei jedem Serverstart
```
SkylineVCloth.Cloth.InitAsync();
```
Zieht einen Spieler vollständig aus
```
SkylineVCloth.Cloth.EquipNaked(player);
```
Lädt die aktuell angezogene Kleidung von dem übergebenen Spieler inkl. Haare für eine Speicherung
```
SkylineVCloth.Cloth.CurrentsJSON(player);
```
Zieht den aktuell übergebenen männlichen Spieler ein Feuerwehr-Outfit an
```
SkylineVCloth.Cloth.Equip(player, '["DLC_MP_HEIST_M_TORSO_74_0","DLC_MP_H3_M_LEGS_0_0","SP_M_FEET_12_6","DLC_MP_X17_M_SPECIAL_7_0","DLC_MP_SUM_M_JBIB_16_3","DLC_MP_H3_M_PHEAD_1_0"]');
```

## Funktionsübersicht
Im Standard werden originale Klamotten/Props von Github (Danke an DurtyFree) immer live geladen wodurch diese automatisch bei GTA-Updates verwendet werden können. Wenn dies nicht gewünscht ist kann hier angegeben werden in welchem Verzeichniss die Initialisierung nach Stammdaten suchen soll.
```
SkylineVCloth.Cloth.OriginalDumpDirectory = "data";
SkylineVCloth.Cloth.OriginalDumpFilename = "pedComponentVariations_free.json";
```
Im Standard werden alle Ressourcen durchsucht um die Datei 'clothes_dump.json' zu finden um diese Modding-Pakete automatisch einzulesen. Bei Bedarf kann der Speicherort/Name hier geändert werden.
```
SkylineVCloth.Cloth.ModdingDumpDirectory = "resources";
SkylineVCloth.Cloth.ModdingDumpFilename = "clothes_dump.json";
```
Diese Funktion initialisiert die Kleidung-Stammdaten welche einmal zum Serverstart ausgeführt werden muss.
> bool loadingFromGithub = true ... Gibt an ob die originale GTA-Daten von Github live oder nur lokale geladen werden sollen / bool showStatistic = true ... Gibt an ob die Statistik ausgegeben werden soll
```
SkylineVCloth.Cloth.InitAsync();
```
Zeigt anhand Alt.Log-Ausgaben geladene Kleidung/Zubehör getrennt für Mann/Frau an - wird im Standard einmal mit der InitAsync() ausgelöst
```
SkylineVCloth.Cloth.Statistic();
```
Hier können einzelne ClothHash (bspw. DLC_MP_X17_M_SPECIAL_7_0) oder eine List<string> sowie JSON(List<string>) übergeben werden um gewisse Hashs als "Blacklist" zu markieren welche durch Anzieh-Funktionen ignoriert werden.
> List<string> nameHashs ... Gibt eine Liste von NameHashs an / string nameHash ... Gibt den NameHash oder ein JSON mit einer Liste von NameHashs an
```
SkylineVCloth.Cloth.Blacklist();
```
Lädt verfügbare Klamotten anhand Komponente welche optional die Blacklist ignoriert. Eine List<DumpResult> wird zurückgeliefert.
> bool male ... Gibt an ob für einen Mann (true) oder für eine Frau (false) Klamotten geladen werden sollen / byte componentId ... Gibt die Komponent-ID an welche Daten geladen werden sollen, bspw. 6 für Schuhe / bool includeBlacklist = false ... Gibt an ob aktivierte Blacklist-Hashs ausgeschlossen (true) oder eingeschlossen (false) werden sollen
```
SkylineVCloth.Cloth.GetCloths();
```
Lädt verfügbare Props anhand Komponente welche optional die Blacklist ignoriert. Eine List<DumpResult> wird zurückgeliefert.
> bool male ... Gibt an ob für einen Mann (true) oder für eine Frau (false) Props geladen werden sollen / byte componentId ... Gibt die Komponent-ID an welche Daten geladen werden sollen, bspw. 6 für Uhren / bool includeBlacklist = false ... Gibt an ob aktivierte Blacklist-Hashs ausgeschlossen (true) oder eingeschlossen (false) werden sollen
```
SkylineVCloth.Cloth.GetProps();
```
Hiermit wird der übergebene Spieler vollständig ausgezogen, Nackt
> AsyncPlayer player ... Ist der alt:V-Spieler
```
SkylineVCloth.Cloth.EquipNaked();
```
Zieht den Spieler Kleidung/Props an anhand eines NameHashs bzw. Liste/JSON
> AsyncPlayer player ... Ist der alt:V-Spieler / List<string> nameHashs ... Gibt eine Liste von NameHashs an / string nameHash ... Gibt den NameHash oder ein JSON mit einer Liste von NameHashs an / bool includeBlacklist = false ... Gibt an ob aktivierte Blacklist-Hashs ausgeschlossen (true) oder eingeschlossen (false) werden sollen
```
SkylineVCloth.Cloth.Equip();
```
Lädt die aktuell angezogene Kleidung/Props von einem Spieler als List<DumpResult> zurückgegeben.
> AsyncPlayer player ... Ist der alt:V-Spieler / bool withHairStyle = true ... Gibt an ob Haare ebenfalls zurückgegeben werden sollen (true) oder nicht (false) / bool includeNaked = false ... Gibt an ob nackte Kleidung/Props ebenfalls zurückgegeben (true) oder nicht zurückgegeben werden sollen (false)
```
SkylineVCloth.Cloth.Currents();
```
Lädt die aktuell angezogene Kleidung/Props von einem Spieler als JSON(List<string) zurückgegeben.
> AsyncPlayer player ... Ist der alt:V-Spieler / bool withHairStyle = true ... Gibt an ob Haare ebenfalls zurückgegeben werden sollen (true) oder nicht (false) / bool includeNaked = false ... Gibt an ob nackte Kleidung/Props ebenfalls zurückgegeben (true) oder nicht zurückgegeben werden sollen (false)
```
SkylineVCloth.Cloth.CurrentsJSON();
```
Zusätzlich gibt es eine Klasse "SkylineVCloth.Classes.TorsoGlove" welche für die automatische Bestimmung von Handschuhe (Torso) verwendet werden kann welche mit den Funktionen HasTorso(), GetGloveByType() und GetGloveByTorso() eine automatisch Berechnung durchführen kann.
