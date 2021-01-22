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
/// </summary>
public class Script_Names: MonoBehaviour
{
    public static Script_Names Names;
    
    // ------------------------------------------------------------------
    // Name fields and properties. Properties used when the name should begin as ???.
    public static string Player
    {
        get { return $"<b>{Names?._Player}</b>"; }
        set { Names._Player = value; }
    } // {0}
    
    public const string Melz                            = "<b>Melz</b>";                        // {1}
    public const string MelzTheGreat                    = "<b>Melz the Great</b>";              // {2}
    public const string Ids                             = "<b>Ids</b>";                         // {3}
    
    public static string Ero                                                                    // {4}
    {
        get => Names?._Ero;
        set => Names._Ero = value;
    }

    public const string SBook                           = "<b>S-book</b>";                      // {5}
    public const string Urselk                          = "<b>Urselk</b>";                      // {6}
    public const string Urselks                         = "<b>Urselks</b>";                     // {7}
    public const string MaskedOne                       = "<b>Masked One</b>";                  // {8}
    public const string Aenimals                        = "<b>ænimals</b>";                     // {9}
    public const string Myne                            = "<b>Myne</b>";                        // {10}
    
    public static string Eileen                                                                 // {11}
    {
        get => Names?._Eileen;
        set => Names._Eileen = value;
    }                     
    
    public static string Ellenia                                                                // {12}
    {
        get => Names?._Ellenia;
        set => Names._Ellenia = value;
    }
    
    public const string Specter                         = "<b>Specter</b>";                     // {13}
    
    public static string ElleniaPassword                                                        // {14} Updated in Eileen Room (L21)
    {
        get => Names?._ElleniaPassword;
        set => Names._ElleniaPassword = value;
    }  
    
    public const string Tedmunch                        = "<b>Tedmunch</b>";                    // {15}
    
    public static string Tedwich                                                                // {16} Updated L11 SavePoint
    {
        get => Names?._Tedwich;
        set => Names._Tedwich = value;
    }
    
    public const string Tedward                         = "<b>Tedward</b>";                     // {17}
    public const string Kelsingor                       = "<b>Kelsingør</b>";                   // {18}
    public const string Specters                        = "<b>Specters</b>";                    // {19}
    public const string Aenimal                         = "<b>ænimal</b>";                      // {20}
    public const string Sheepluff                       = "<b>Sheepluff</b>";                   // {21}
    public const string Sealing                         = "<b>Sealing</b>";                     // {22}
    public const string Action1                         = "<b><i>SPACE or ENTER</i></b>";       // {23}
    public const string Action2                         = "<b><i>X or RIGHT-SHIFT-KEY</i></b>"; // {24}
    public const string Action3                         = "<b><i>LEFT-SHIFT-KEY</i></b>";       // {25}
    public const string InventoryKeyCode                = "<b><i>1</i></b>";                    // {26}
    public const string Escape                          = "<b><i>ESC</i></b>";                  // {27}
    public const string Skip                            = "<b><i>SPACE or ENTER</i></b>";       // {28}
    public const string Tedmas                          = "<b>Tedmas</b>";                      // {29}
    public const string Sieve                           = "<b>Sieve</b>";                       // {30}
    public const string Master                          = "<b>Master</b>";                      // {31}
    public const string Inventory                       = "<b>Inventory</b>";                   // {32}
    
    public static string Ursie                                                                  // {33}
    {
        get => Names?._Ursie;
        set => Names._Ursie = value;
    }
    
    public const string UrselkHouse                     = "<b>Urselk House</b>";                // {34}
    public const string UrselksSaloon                   = "<b>Urselks Saloon</b>";              // {35}
    public const string UrselksBallroom                 = "<b>Urselks Ballroom</b>";            // {36}
    public const string KelsingorMansion                = "<b>Kelsingør Mansion</b>";           // {37}
    
    public static string Kaffe                                                                  // {38}
    {
        get => Names?._Kaffe;
        set => Names._Kaffe = value;
    }
    
    public static string Latte                                                                  // {39}
    {
        get => Names?._Latte;
        set => Names._Latte = value;
    }
    
    public const string MagicCircle                     = "<b>Magic Circle</b>";                // {40}
    public const string Menu                            = "<b>Menu</b>";                        // {41}
    public const string BadSpecters                     = "<b>Bad Specters</b>";                // {42}
    public const string BadSpecter                      = "<b>Bad Specter</b>";                 // {43}
    public const string HouseMaster                     = "<b>House Master</b>";                // {44}
    public const string Thoughts                        = "<b>Thoughts</b>";                    // {45}
    public const string HeartsCapacity                  = "<b>Hearts Capacity</b>";             // {46}
    public const string Vx                              = "<b>Vx</b>";                          // {47}
    public const string Dan                             = "<b>dan</b>";                         // {48}
    public const string NauticalDawn                    = "<b>6:11 a.m.</b>";                   // {49}
    public const string Mon                             = "<b>Monday</b>";                      // {50}
    public const string Tue                             = "<b>Tuesday</b>";                     // {51}
    public const string Wed                             = "<b>Wednesday</b>";                   // {52}
    public const string Thu                             = "<b>Thursday</b>";                    // {53}
    public const string Fri                             = "<b>Friday</b>";                      // {54}
    public const string Sat                             = "<b>Saturday</b>";                    // {55}
    public const string Sun                             = "<b>Sunday</b>";                      // {56}
    
    public static string KingEclaire                                                            // {57}
    {
        get => Names?._KingEclaire;
        set => Names._KingEclaire = value;
    }
    
    public static string Suzette                                                                // {58}
    {
        get => Names?._Suzette;
        set => Names._Suzette = value;
    }
    
    public const string Shiteater                       = "<b>shiteater</b>";                   // {59}
    public const string Shiteaters                      = "<b>shiteaters</b>";                  // {60}
    
    public static string Peche                                                                  // {61}
    {
        get => Names?._Peche;
        set => Names._Peche = value;
    }
    
    public static string Melba                                                                  // {62}
    {
        get => Names?._Melba;
        set => Names._Melba = value;
    }
    
    public static string Moose                                                                  // {63}
    {
        get => Names?._Moose;
        set => Names._Moose = value;
    }
    
    public const string Flan                            = "<b>Flan the Guard</b>";              // {64}

    // ------------------------------------------------------------------
    // Give these getters and setters so we can see Names while dev'ing
    [SerializeField] private string _Player             = "<b>???</b>";
    [SerializeField] private string _Ero                = "<b>???</b>";
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

    // ------------------------------------------------------------------
    // Updater functions. Call from dialogue to update the updateable name.
    public static void UpdateEro() { Ero                                        = "<b>Ero</b>"; }

    // Refs: (1) Eileen's room
    public static void UpdateEileen() { Eileen                                  = "<b>Eileen</b>"; }
    // Refs: (1) Eileen's room
    
    public static void UpdateEllenia() { Ellenia                                = "<b>Ellenia</b>"; }
    
    public static void UpdateElleniaPassword(string s) { ElleniaPassword        = s; }
    
    public static void UpdateTedwich() { Tedwich                                = "<b>Tedwich</b>"; }
    
    public static void UpdateUrsie() { Ursie                                    = "<b>Ursie</b>"; }
    
    // Refs: (1) Kaffe & Latte's Dialogue in Ballroom
    public static void UpdateKaffe() { Kaffe                                    = "<b>Kaffe</b>"; }
    
    // Refs: (1) Kaffe & Latte's Dialogue in Ballroom
    public static void UpdateLatte() { Latte                                    = "<b>Latte</b>"; }
    
    // Refs: (1) King's Dialogue in Ballroom
    public static void UpdateKingEclaire() { KingEclaire                        = "<b>King Eclaire</b>"; }
    
    // Refs: (1) Suzette's Dialogue in Ballroom
    public static void UpdateSuzette() { Suzette                                = "<b>Suzette</b>"; }
    
    // Refs: (1) Peche & Melba's Dialogue in Ballroom
    public static void UpdatePeche() { Peche                                    = "<b>Peche</b>"; }
    
    // Refs: (1) Peche & Melba's Dialogue in Ballroom
    public static void UpdateMelba() { Melba                                    = "<b>Melba</b>"; }
    
    public static void UpdateMoose() { Moose                                    = "<b>Moose</b>"; }

    // ------------------------------------------------------------------
    // For Game Load.
    public static void LoadNames(Model_Names names)
    {
        Player                  = names.Player;
        Ero                     = names.Ero;
        Eileen                  = names.Eileen;
        Ellenia                 = names.Ellenia;
        ElleniaPassword         = names.ElleniaPassword;
        Tedwich                 = names.Tedwich;
        Ursie                   = names.Ursie;
        Kaffe                   = names.Kaffe;
        Latte                   = names.Latte;
        KingEclaire             = names.KingEclaire; 
        Suzette                 = names.Suzette; 
        Peche                   = names.Peche; 
        Melba                   = names.Melba;
        Moose                   = names.Moose;
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