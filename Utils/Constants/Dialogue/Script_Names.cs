using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// All the proper names.
/// In game the names will start out as ??? until the player becomes aware of them in which case
/// we'll update and save the name to state.
/// 
/// Call one of the Updater functions on Next Node Action event to update the name permanently.
/// 
/// Localization Notes:
/// - State will always either be saved as <b>???</b> or EN name
/// </summary>
public class Script_Names: MonoBehaviour
{
    public static Script_Names Names;

    public const string TripleQuestionMark = "???";
    public static string UnknownName = $"<b>{TripleQuestionMark}</b>";
    
    // ------------------------------------------------------------------
    // Name fields and properties. Properties used when the name should begin as ???.
    public static string Player                                                                  // {0}
    {
        get { return $"<b>{Names?._Player}</b>"; }
        set { Names._Player = value; }
    }
    
    public static string Mask                           = GetLocalized(
        MaskEN,
        MaskCN
    );         // {1}
    public const string MaskEN                          = "<b>mask</b>";
    public const string MaskCN                          = "<b>面具</b>";

    public static string PlayerUnbold                                                           // {2}
    {
        get => Names?._Player;
    }

    public static string Ids                                                                    // {3}
    {
        get => GetLocalizedName(
            Names?._Ids,
            IdsEN,
            IdsCN
        );
        set => Names._Ids = value;
    }
    
    public static string Ero                                                                    // {4}
    {
        get => Names?._Ero;
        set => Names._Ero = value;
    }

    public static string PreppedMasks                   = GetLocalized(
        PreppedMasksEN,
        PreppedMasksCN
    );               // {5}
    public const string PreppedMasksEN                  = "<b>Prepped Masks</b>";
    public const string PreppedMasksCN                  = "<b>Prepped Masks</b>";

    public static string Urselk                         = GetLocalized(
        UrselkEN,
        UrselkCN
    );                      // {6}
    public const string UrselkEN                        = "<b>Urselk</b>";
    public const string UrselkCN                        = "<b>Urselk</b>";

    public static string Urselks                        = GetLocalized(
        UrselksEN,
        UrselksCN
    );                     // {7}
    public const string UrselksEN                       = "<b>Urselks</b>";
    public const string UrselksCN                       = "<b>Urselks</b>";

    public static string Owner                          = GetLocalized(
        OwnerEN,
        OwnerCN
    );                       // {8}
    public const string OwnerEN                         = "<b>owner</b>";
    public const string OwnerCN                         = "<b>owner</b>";

    public static string ImpermanentUpper               = GetLocalized(
        ImpermanentUpperEN,
        ImpermanentUpperCN
    );                          // {9}
    public const string ImpermanentUpperEN              = "<b>Impermanent</b>";
    public const string ImpermanentUpperCN              = "<b>Impermanent</b>";
    
    public static string Myne                                                                    // {10}
    {
        get => GetLocalizedName(
            Names?._Myne,
            MyneEN,
            MyneCN
        );
        set => Names._Myne = value;
    }
    
    public static string Eileen                                                                 // {11}
    {
        get => GetLocalizedName(
            Names?._Eileen,
            EileenEN,
            EileenCN
        );
        set => Names._Eileen = value;
    }                     
    
    public static string Ellenia                                                                // {12}
    {
        get => GetLocalizedName(
            Names?._Ellenia,
            ElleniaEN,
            ElleniaCN
        );
        set => Names._Ellenia = value;
    }
    
    public static string Specter                        = GetLocalized(
        SpecterEN,
        SpecterCN
    );                     // {13}
    public const string SpecterEN                       = "<b>Specter</b>";
    public const string SpecterCN                       = "<b>Specter</b>";
    
    public static string ElleniaPassword                                                        // {14} Updated in Eileen Room (L21)
    {
        get => Names?._ElleniaPassword;
        set => Names._ElleniaPassword = value;
    }  
    
    public static string SwitchActiveStickerKeycodesJoystick = GetLocalized(
        SwitchActiveStickerKeycodesJoystickEN,
        SwitchActiveStickerKeycodesJoystickCN
    );    // {15}
    public const string SwitchActiveStickerKeycodesJoystickEN = "the <b>Wear Mask 1-4</b> commands";
    public const string SwitchActiveStickerKeycodesJoystickCN = "the <b>Wear Mask 1-4</b> commands";

    public static string Tedwich                                                                // {16} Updated L11 SavePoint
    {
        get => Names?._Tedwich;
        set => Names._Tedwich = value;
    }
    
    public static string FlanShort                       = GetLocalized(
        FlanShortEN,
        FlanShortCN
    ); // {17}
    public const string FlanShortEN                       = "<b>Flan</b>";
    public const string FlanShortCN                       = "<b>Flan</b>";
    
    public static string Kelsingor                       = GetLocalized(
        KelsingorEN,
        KelsingorCN
    ); // {18}
    public const string KelsingorEN                       = "<b>Kelsingør</b>";
    public const string KelsingorCN                       = "<b>Kelsingør</b>";
    
    public static string Specters                        = GetLocalized(
        SpectersEN,
        SpectersCN
    );                    // {19}
    public const string SpectersEN                        = "<b>Specters</b>";
    public const string SpectersCN                        = "<b>Specters</b>";

    public static string CatwalkInTheSky                 = GetLocalized(
        CatwalkInTheSkyEN,
        CatwalkInTheSkyCN
    );          // {20}
    public const string CatwalkInTheSkyEN                 = "<b>Catwalk in the Sky</b>";
    public const string CatwalkInTheSkyCN                 = "<b>Catwalk in the Sky</b>";

    public static string Sheepluff                       = GetLocalized(
        SheepluffEN,
        SheepluffCN
    );                   // {21}
    public const string SheepluffEN                       = "<b>Sheepluff</b>";
    public const string SheepluffCN                       = "<b>Sheepluff</b>";

    public static string Sealing                         = GetLocalized(
        SealingEN,
        SealingCN
    );                     // {22}
    public const string SealingEN                         = "<b>Sealing</b>";
    public const string SealingCN                         = "<b>Sealing</b>";

    public const string Action1                         = "<b><i>SPACE or ENTER</i></b>";       // {23}
    public const string Action2                         = "<b><i>X or RIGHT-SHIFT-KEY</i></b>"; // {24}
    public const string Action3                         = "<b><i>LEFT-SHIFT-KEY</i></b>";       // {25}
    public const string InventoryKeyCode                = "<b><i>I</i></b>";                    // {26}
    public const string Escape                          = "<b><i>ESC</i></b>";                  // {27}
    public const string Skip                            = "<b><i>SPACE or ENTER</i></b>";       // {28}
    
    public static string UrsaSaloonHallway               = GetLocalized(
        UrsaSaloonHallwayEN,
        UrsaSaloonHallwayCN
    );         // {29}
    public const string UrsaSaloonHallwayEN               = "<b>Ursa Saloon Hallway</b>";
    public const string UrsaSaloonHallwayCN               = "<b>Ursa Saloon Hallway</b>";
    
    public const string Sieve                           = "<b>Sieve</b>";                       // {30}
    
    public static string Master                          = GetLocalized(
        MasterEN,
        MasterCN
    );                      // {31}
    public const string MasterEN                          = "<b>Master</b>";
    public const string MasterCN                          = "<b>Master</b>";
    
    public static string Inventory                       = GetLocalized(
        InventoryEN,
        InventoryCN
    );                   // {32}
    public const string InventoryEN                       = "<b>Inventory</b>";
    public const string InventoryCN                       = "<b>Inventory</b>";
    
    public static string Ursie                                                                  // {33}
    {
        get => GetLocalizedName(
            Names?._Ursie,
            UrsieEN,
            UrsieCN
        );
        set => Names._Ursie = value;
    }
    
    public const string UrselkHouse                     = "<b>Urselk House</b>";                // {34}
    
    public static string UrsaSaloon                      = GetLocalized(
        UrsaSaloonEN,
        UrsaSaloonCN
    );                 // {35}
    public const string UrsaSaloonEN                      = "<b>Ursa Saloon</b>";
    public const string UrsaSaloonCN                      = "<b>Ursa Saloon</b>";

    public static string Ballroom                        = GetLocalized(
        BallroomEN,
        BallroomCN
    );                    // {36}
    public const string BallroomEN                        = "<b>Ballroom</b>";
    public const string BallroomCN                        = "<b>Ballroom</b>";

    public static string KelsingorMansion                = GetLocalized(
        KelsingorMansionEN,
        KelsingorMansionCN
    );           // {37}
    public const string KelsingorMansionEN                = "<b>Kelsingør Mansion</b>";
    public const string KelsingorMansionCN                = "<b>Kelsingør Mansion</b>";
    
    public static string Kaffe                                                                  // {38}
    {
        get => GetLocalizedName(
            Names?._Kaffe,
            KaffeEN,
            KaffeCN
        );
        set => Names._Kaffe = value;
    }
    
    public static string Latte                                                                  // {39}
    {
        get => GetLocalizedName(
            Names?._Latte,
            LatteEN,
            LatteCN
        );
        set => Names._Latte = value;
    }
    
    public const string MagicCircle                     = "<b>Magic Circle</b>";                // {40}
    
    public static string Menu                            = GetLocalized(
        MenuEN,
        MenuCN
    );                        // {41}
    public const string MenuEN                            = "<b>Menu</b>";
    public const string MenuCN                            = "<b>Menu</b>";
    
    public static string CursedOnes                      = GetLocalized(
        CursedOnesEN,
        CursedOnesCN
    );             // {42}
    public const string CursedOnesEN                      = "<b>Cursed Specters</b>";
    public const string CursedOnesCN                      = "<b>Cursed Specters</b>";

    public static string CursedOne                       = GetLocalized(
        CursedOneEN,
        CursedOneCN
    );              // {43}
    public const string CursedOneEN                       = "<b>Cursed Specter</b>";
    public const string CursedOneCN                       = "<b>Cursed Specter</b>";

    public static string Notes                           = GetLocalized(
        NotesEN,
        NotesCN
    );                       // {44}
    public const string NotesEN                           = "<b>Notes</b>";
    public const string NotesCN                           = "<b>Notes</b>";

    public const string Thoughts                        = "<b>Thoughts</b>";                    // {45}
    public const string HeartsCapacity                  = "<b>Hearts Capacity</b>";             // {46}
    public const string Vx                              = "<b>Vx</b>";                          // {47}
    public const string Dan                             = "<b>dan</b>";                         // {48}
    public const string T600am                          = "<b>6:00 a.m.</b>";                   // {49}
    public const string Mon                             = "<b>Monday</b>";                      // {50}
    public const string Tue                             = "<b>Tuesday</b>";                     // {51}
    public const string Wed                             = "<b>Wednesday</b>";                   // {52}
    public const string Thu                             = "<b>Thursday</b>";                    // {53}
    public const string Fri                             = "<b>Friday</b>";                      // {54}
    public const string Sat                             = "<b>Saturday</b>";                    // {55}
    public const string Sun                             = "<b>Sunday</b>";                      // {56}
    
    public static string KingEclaire                                                            // {57}
    {
        get => GetLocalizedName(
            Names?._KingEclaire,
            KingEclaireEN,
            KingEclaireCN
        );
        set => Names._KingEclaire = value;
    }
    
    public static string Suzette                                                                // {58}
    {
        get => GetLocalizedName(
            Names?._Suzette,
            SuzetteEN,
            SuzetteCN
        );
        set => Names._Suzette = value;
    }
    
    public static string Shiteater                       = GetLocalized(
        ShiteaterEN,
        ShiteaterCN
    );                   // {59}
    public const string ShiteaterEN                       = "<b>shiteater</b>";
    public const string ShiteaterCN                       = "<b>shiteater</b>";

    public static string Shiteaters                      = GetLocalized(
        ShiteatersEN,
        ShiteatersCN
    );                  // {60}
    public const string ShiteatersEN                      = "<b>shiteaters</b>";
    public const string ShiteatersCN                      = "<b>shiteaters</b>";
    
    public static string Peche                                                                  // {61}
    {
        get => GetLocalizedName(
            Names?._Peche,
            PecheEN,
            PecheCN
        );
        set => Names._Peche = value;
    }
    
    public static string Melba                                                                  // {62}
    {
        get => GetLocalizedName(
            Names?._Melba,
            MelbaEN,
            MelbaCN
        );
        set => Names._Melba = value;
    }
    
    public static string Moose                                                                  // {63}
    {
        get => GetLocalizedName(
            Names?._Moose,
            MooseEN,
            MooseCN
        );
        set => Names._Moose = value;
    }
    
    public static string Flan                                                                   // {64}
    {
        get => GetLocalizedName(
            Names?._Flan,
            FlanEN,
            FlanCN
        );
        set => Names._Flan = value;
    }
    
    public static string ActionSticker                   = GetLocalized(
        ActionStickerEN,
        ActionStickerCN
    );                 // {65}
    public const string ActionStickerEN                   = "<b>Active Mask</b>";
    public const string ActionStickerCN                   = "<b>Active Mask</b>";

    public static string LastElevator                    = GetLocalized(
        LastElevatorEN,
        LastElevatorCN
    );               // {66}
    public const string LastElevatorEN                    = "<b>Last Elevator</b>";
    public const string LastElevatorCN                    = "<b>Last Elevator</b>";
    
    // These refer to Passive Effects.
    public static string StickerSkill                    = GetLocalized(
        StickerSkillEN,
        StickerSkillCN
    );                 // {67}
    public const string StickerSkillEN                    = "<b>Mask Effect</b>";
    public const string StickerSkillCN                    = "<b>Mask Effect</b>";
    
    public static string StickerSkills                   = GetLocalized(
        StickerSkillsEN,
        StickerSkillsCN
    );                // {68}
    public const string StickerSkillsEN                   = "<b>Mask Effects</b>";
    public const string StickerSkillsCN                   = "<b>Mask Effects</b>";

    public const string Coconut                         = "<b>Coconut</b>";                     // {69}
    public const string WeekdayShift                    = "<b>Weekday Shift</b>";               // {70}
    public const string WeekendShift                    = "<b>Weekend Shift</b>";               // {71}
    
    public static string ElevatorBay                     = GetLocalized(
        ElevatorBayEN,
        ElevatorBayCN
    );                // {72}
    public const string ElevatorBayEN                     = "<b>Elevator Bay</b>";
    public const string ElevatorBayCN                     = "<b>Elevator Bay</b>";
    
    public static string ControllableSpecters            = GetLocalized(
        ControllableSpectersEN,
        ControllableSpectersCN
    );       // {73}
    public const string ControllableSpectersEN            = "<b>Controllable Specters</b>";
    public const string ControllableSpectersCN            = "<b>Controllable Specters</b>";   
    
    public const string Unknown                         = "<b>???</b>";                         // {74}

    public static string XXXWorld                        = GetLocalized(
        XXXWorldEN,
        XXXWorldCN
    );                   // {75}
    public const string XXXWorldEN                        = "<b>XXX World</b>";
    public const string XXXWorldCN                        = "<b>XXX World</b>";

    public static string OtherSide                       = GetLocalized(
        OtherSideEN,
        OtherSideCN
    );                  // {76}
    public const string OtherSideEN                       = "<b>other side</b>";
    public const string OtherSideCN                       = "<b>other side</b>";

    public static string NauticalDawn                    = GetLocalized(
        NauticalDawnEN,
        NauticalDawnCN
    );               // {77}
    public const string NauticalDawnEN                    = "<b>Nautical Dawn</b>";
    public const string NauticalDawnCN                    = "<b>Nautical Dawn</b>";

    public static string DarkDarkHall                    = GetLocalized(
        DarkDarkHallEN,
        DarkDarkHallCN
    );              // {78}
    public const string DarkDarkHallEN                    = "<b>Dark Dark Hall</b>";
    public const string DarkDarkHallCN                    = "<b>Dark Dark Hall</b>";
    
    // The Active Mask Command.
    public static string ActiveStickerCommand            = GetLocalized(
        ActiveStickerCommandEN,
        ActiveStickerCommandCN
    );                // {79}
    public const string ActiveStickerCommandEN            = "<b>Mask Command</b>";
    public const string ActiveStickerCommandCN            = "<b>Mask Command</b>";

    public static string SwitchActiveSticker             = GetLocalized(
        SwitchActiveStickerEN,
        SwitchActiveStickerCN
    );                   // {80}
    public const string SwitchActiveStickerEN             = "<b>Wear Mask</b>";
    public const string SwitchActiveStickerCN             = "<b>Wear Mask</b>";

    public static string Prep                            = GetLocalized(
        PrepEN,
        PrepCN
    );                        // {81}
    public const string PrepEN                            = "<b>Prep</b>";
    public const string PrepCN                            = "<b>Prep</b>";

    public const string SwitchActiveStickerKeyCodes     = "<b>1, 2, 3, 4</b>";                  // {82}
    
    public static string Elder                           = GetLocalized(
        ElderEN,
        ElderCN
    );                       // {83}
    public const string ElderEN                           = "<b>Elder</b>";
    public const string ElderCN                           = "<b>Elder</b>";

    public static string WellsWorld                      = GetLocalized(
        WellsWorldEN,
        WellsWorldCN
    );                 // {84}
    public const string WellsWorldEN                      = "<b>Wells World</b>";
    public const string WellsWorldCN                      = "<b>Wells World</b>";

    public static string CelestialGardensWorld           = GetLocalized(
        CelestialGardensWorldEN,
        CelestialGardensWorldCN
    );     // {85}
    public const string CelestialGardensWorldEN           = "<b>Celestial Gardens World</b>";
    public const string CelestialGardensWorldCN           = "<b>Celestial Gardens World</b>";

    public static string OwnerPlain                      = GetLocalized(
        OwnerPlainEN,
        OwnerPlainCN
    );                              // {86}
    public const string OwnerPlainEN                      = "owner";
    public const string OwnerPlainCN                      = "owner";

    public static string RockGarden                      = GetLocalized(
        RockGardenEN,
        RockGardenCN
    );         // {87}
    public const string RockGardenEN                      = "<b>Hidden Field of Sin</b>";
    public const string RockGardenCN                      = "<b>Hidden Field of Sin</b>";
    
    public static string MirrorHalls                     = GetLocalized(
        MirrorHallsEN,
        MirrorHallsCN
    ); // {88}
    public const string MirrorHallsEN                     = "<b>Mirror Halls</b>";
    public const string MirrorHallsCN                     = "<b>Mirror Halls</b>";

    public static string HallwayToBasement               = GetLocalized(
        HallwayToBasementEN,
        HallwayToBasementCN
    ); // {89}
    public const string HallwayToBasementEN               = "<b>Passage to the Depths</b>";
    public const string HallwayToBasementCN               = "<b>Passage to the Depths</b>";
    
    // Ids will be introduced at this point (beginning of Day 2)    
    public static string Basement                        = GetLocalized(
        BasementEN,
        BasementCN
    ); // {90}
    public const string BasementEN                        = "<b>Ids’ Vault</b>";
    public const string BasementCN                        = "<b>Ids’ Vault</b>";

    public static string InsideAPainting                 = GetLocalized(
        InsideAPaintingEN,
        InsideAPaintingCN
    ); // {91}
    public const string InsideAPaintingEN                 = "<b>Inside a Painting</b>";
    public const string InsideAPaintingCN                 = "<b>Inside a Painting</b>";
    
    // Specify Room bc first room to encounter, sets stage
    public static string DiningRoom                      = GetLocalized(
        DiningRoomEN,
        DiningRoomCN
    ); // {92}
    public const string DiningRoomEN                      = "<b>Parlor Room</b>";
    public const string DiningRoomCN                      = "<b>Parlor Room</b>";

    public const string R2CursedTime                    = "<b>5:30 a.m.</b>"; // {93}
    public const string IdsDeadTime                     = "<b>5:45 a.m.</b>"; // {94}
    
    public static string HotelLobby                      = GetLocalized(
        HotelLobbyEN,
        HotelLobbyCN
    ); // {95}
    public const string HotelLobbyEN                      = "<b>Hotel Lobby</b>";
    public const string HotelLobbyCN                      = "<b>Hotel Lobby</b>";


    // ------------------------------------------------------------------
    // Give these getters and setters so we can see Names while dev'ing
    [SerializeField] private string _Player             = "<b>???</b>";
    [SerializeField] private string _Ids                = "<b>???</b>";
    [SerializeField] private string _Ero                = "<b>???</b>";
    [SerializeField] private string _Myne               = "<b>???</b>";
    [SerializeField] private string _Eileen             = "<b>???</b>";
    [SerializeField] private string _Ellenia            = "<b>???</b>";
    [SerializeField] private string _ElleniaPassword    = null;
    [SerializeField] private string _Tedwich            = "<b>???</b>";
    [SerializeField] private string _Ursie              = "<b>???</b>";
    [SerializeField] private string _Kaffe              = "<b>???</b>";
    [SerializeField] private string _Latte              = "<b>???</b>";
    [SerializeField] private string _KingEclaire        = "<b>???</b>";
    [SerializeField] private string _Suzette            = "<b>???</b>";
    [SerializeField] private string _Peche              = "<b>???</b>";
    [SerializeField] private string _Melba              = "<b>???</b>";
    [SerializeField] private string _Moose              = "<b>???</b>";
    [SerializeField] private string _Flan               = "<b>???</b>";

    // ------------------------------------------------------------------
    // Updater functions. Call from dialogue to update the updateable name.
    // Refs:    (1) Full Art Note PreFullArtAction on door to Ids room (lvl 9)
    //          (2) In Ids' room introduction, when calling Nameplate Timeline (IntroNode_NRoom)
    public static void UpdateIds() { Ids                                        = IdsEN; }
    public const string IdsEN                                                   = "<b>Ids</b>";
    public const string IdsCN                                                   = "<b>第四</b>";
    public static string IdsNameStateEN() => GetStateNameEN(Names?._Ids, IdsEN);
    
    public static void UpdateEro() { Ero                                        = "<b>Ero</b>"; }
    
    // Refs: (1) Myne's Mirror Interaction0
    public static void UpdateMyne() { Myne                                      = MyneEN; }
    public const string MyneEN                                                  = "<b>Myne</b>";
    public const string MyneCN                                                  = "<b>我是</b>";
    public static string MyneNameStateEN() => GetStateNameEN(Names?._Myne, MyneEN);

    // Refs: (1) Eileen's room via UpdateSisters in Level Behavior
    public static void UpdateEileen() { Eileen                                  = EileenEN; }
    public const string EileenEN                                                = "<b>Eileen</b>";
    public const string EileenCN                                                = "<b>我是</b>";
    public static string EileenNameStateEN() => GetStateNameEN(Names?._Eileen, EileenEN);
    
    // Refs: (1) Eileen's room via UpdateSisters in Level Behavior
    public static void UpdateEllenia() { Ellenia                                = ElleniaEN; }
    public const string ElleniaEN                                               = "<b>Ellenia</b>";
    public const string ElleniaCN                                               = "<b>我是</b>";
    public static string ElleniaNameStateEN() => GetStateNameEN(Names?._Ellenia, ElleniaEN);
    
    // Refs: (1) Eileen's room
    public static void UpdateElleniaPassword(string s) { ElleniaPassword        = s; }
    
    // Refs: (1) Updated in SavePoint L11
    public static void UpdateTedwich() { Tedwich                                = "<b>Tedwich</b>"; }
    
    // Refs: (1) Updated in Saloon Hallway L29
    // (2) Ballroom
    // (3) Urselks Saloon
    public static void UpdateUrsie() { Ursie                                    = UrsieEN; }
    public const string UrsieEN                                                 = "<b>Ursie</b>";
    public const string UrsieCN                                                 = "<b>我是</b>";
    public static string UrsieNameStateEN() => GetStateNameEN(Names?._Ursie, UrsieEN);
    
    // Refs: (1) Kaffe & Latte's Dialogue in Ballroom
    public static void UpdateKaffe() { Kaffe                                    = KaffeEN; }
    public const string KaffeEN                                                 = "<b>Kaffe</b>";
    public const string KaffeCN                                                 = "<b>我是</b>";
    public static string KaffeNameStateEN() => GetStateNameEN(Names?._Kaffe, KaffeEN);
    
    // Refs: (1) Kaffe & Latte's Dialogue in Ballroom
    public static void UpdateLatte() { Latte                                    = LatteEN; }
    public const string LatteEN                                                 = "<b>Latte</b>";
    public const string LatteCN                                                 = "<b>我是</b>";
    public static string LatteNameStateEN() => GetStateNameEN(Names?._Latte, LatteEN);
    
    // Refs: (1) King's Dialogue in Ballroom
    public static void UpdateKingEclaire() { KingEclaire                        = KingEclaireEN; }
    public const string KingEclaireEN                                           = "<b>Elder Eclaire</b>";
    public const string KingEclaireCN                                           = "<b>我是</b>";
    public static string KingEclaireNameStateEN() => GetStateNameEN(Names?._KingEclaire, KingEclaireEN);
    
    // Refs: (1) Suzette's Dialogue in Ballroom
    public static void UpdateSuzette() { Suzette                                = SuzetteEN; }
    public const string SuzetteEN                                               = "<b>Suzette</b>";
    public const string SuzetteCN                                               = "<b>我是</b>";
    public static string SuzetteNameStateEN() => GetStateNameEN(Names?._Suzette, SuzetteEN);
    
    // Refs: (1) Peche & Melba's Dialogue in Ballroom
    public static void UpdatePeche() { Peche                                    = PecheEN; }
    public const string PecheEN                                                 = "<b>Peche</b>";
    public const string PecheCN                                                 = "<b>我是</b>";
    public static string PecheNameStateEN() => GetStateNameEN(Names?._Peche, PecheEN);
    
    // Refs: (1) Peche & Melba's Dialogue in Ballroom
    public static void UpdateMelba() { Melba                                    = MelbaEN; }
    public const string MelbaEN                                                 = "<b>Melba</b>";
    public const string MelbaCN                                                 = "<b>我是</b>";
    public static string MelbaNameStateEN() => GetStateNameEN(Names?._Melba, MelbaEN);

    // Refs: (1) After completing Moose quest in Wells World
    public static void UpdateMoose() { Moose                                    = MooseEN; }
    public const string MooseEN                                                 = "<b>Moose</b>";
    public const string MooseCN                                                 = "<b>我是</b>";
    public static string MooseNameStateEN() => GetStateNameEN(Names?._Moose, MooseEN);

    // Refs: (1) After Flan unblocks Hallway
    public static void UpdateFlan() { Flan                                      = FlanEN; }
    public const string FlanEN                                                  = "<b>Flan the Guard</b>";
    public const string FlanCN                                                  = "<b>我是</b>";
    public static string FlanNameStateEN() => GetStateNameEN(Names?._Flan, FlanEN);

    // ------------------------------------------------------------------
    // For Game Load.
    public static void LoadNames(Model_Names names)
    {
        Player                  = names.Player;
        Ids                     = names.Ids;
        Ero                     = names.Ero;
        Myne                    = names.Myne;
        Eileen                  = names.Eileen;
        Ellenia                 = names.Ellenia;
        Tedwich                 = names.Tedwich;
        Ursie                   = names.Ursie;
        Kaffe                   = names.Kaffe;
        Latte                   = names.Latte;
        KingEclaire             = names.KingEclaire; 
        Suzette                 = names.Suzette; 
        Peche                   = names.Peche; 
        Melba                   = names.Melba;
        Moose                   = names.Moose;
        Flan                    = names.Flan;
    }

    // Dev only
    // Note: Moose is ??? until finish her quest in Wells World.
    public static void UpdateR1Names()
    {
        UpdateIds();
        UpdateMyne();
        UpdateEileen();
        UpdateEllenia();
        UpdateUrsie();
        UpdateKaffe();
        UpdateLatte();
        UpdateKingEclaire();
        UpdateSuzette();
        UpdatePeche();
        UpdateMelba();
        UpdateFlan();
    }

    private static string GetLocalizedName
    (
        string name,
        string EN,
        string CN
    )
    {
        return name == null || name.Contains(TripleQuestionMark)
            ? UnknownName
            : GetLocalized(
                EN,
                CN
            );
    }

    private static string GetLocalized
    (
        string EN,
        string CN
    ) => Script_LocalizationUtils.SwitchTextOnLang(
        EN,
        CN
    );

    private static string GetStateNameEN(string name, string EN)
    {
        return name == null || name.Contains(TripleQuestionMark)
            ? UnknownName
            : EN;
    }

    public void Setup()
    {
        if (Names == null)
        {
            Names = this;
        }
        else if (Names != this)
        {
            Destroy(this.gameObject);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Script_Names))]
public class Script_NamesTester : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Script_Names n = (Script_Names)target;
        if (GUILayout.Button("Script_Names.Player = 'STRAWBERRIEZ'"))
        {
            Script_Names.Player = "STRAWBERRIEZ";
        }
    }
}
#endif