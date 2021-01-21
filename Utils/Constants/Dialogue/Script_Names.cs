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
    
    public static string Player
    {
        get { return $"<b>{Names?._Player}</b>"; }
        set { Names._Player = value; }
    } // {0}
    public static readonly string Melz                  = "<b>Melz</b>";                        // {1}
    public static readonly string MelzTheGreat          = "<b>Melz the Great</b>";              // {2}
    public static readonly string Ids                   = "<b>Ids</b>";                         // {3}
    public static readonly string Ero                   = "<b>Ero</b>";                         // {4}
    public static readonly string SBook                 = "<b>S-book</b>";                      // {5}
    public static readonly string Urselk                = "<b>Urselk</b>";                      // {6}
    public static readonly string Urselks               = "<b>Urselks</b>";                     // {7}
    public static readonly string Anima                 = "<b>Anima</b>";                       // {8}
    public static readonly string Aenimals              = "<b>ænimals</b>";                     // {9}
    public static readonly string Myne                  = "<b>Myne</b>";                        // {10}
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
    public static readonly string Specter               = "<b>Specter</b>";                     // {13}
    public static string ElleniaPassword                                                        // {14} Updated in Eileen Room (L21)
    {
        get => Names?._ElleniaPassword;
        set => Names._ElleniaPassword = value;
    }  
    public static readonly string Tedmunch              = "<b>Tedmunch</b>";                    // {15}
    public static string Tedwich                                                                // {16} Updated L11 SavePoint
    {
        get => Names?._Tedwich;
        set => Names._Tedwich = value;
    }
    public static readonly string Tedward               = "<b>Tedward</b>";                     // {17}
    public static readonly string Kelsingor             = "<b>Kelsingør</b>";                   // {18}
    public static readonly string Specters              = "<b>Specters</b>";                    // {19}
    public static readonly string Aenimal               = "<b>ænimal</b>";                      // {20}
    public static readonly string Sheepluff             = "<b>Sheepluff</b>";                   // {21}
    public static readonly string Sieving               = "<b>Sieving</b>";                     // {22}
    public static readonly string Action1               = "<b><i>SPACE or ENTER</i></b>";       // {23}
    public static readonly string Action2               = "<b><i>X or RIGHT-SHIFT-KEY</i></b>"; // {24}
    public static readonly string Action3               = "<b><i>LEFT-SHIFT-KEY</i></b>";       // {25}
    public static readonly string InventoryKeyCode      = "<b><i>1</i></b>";                    // {26}
    public static readonly string Escape                = "<b><i>ESC</i></b>";                  // {27}
    public static readonly string Skip                  = "<b><i>SPACE or ENTER</i></b>";       // {28}
    public static readonly string Tedmas                = "<b>Tedmas</b>";                      // {29}
    public static readonly string Sieve                 = "<b>Sieve</b>";                       // {30}
    public static readonly string Master                = "<b>Master</b>";                      // {31}
    public static readonly string Inventory             = "<b>Inventory</b>";                   // {32}
    public static string Ursie                                                                  // {33}
    {
        get => Names?._Ursie;
        set => Names._Ursie = value;
    }
    public static readonly string UrselkHouse           = "<b>Urselk House</b>";                // {34}
    public static readonly string UrselksSaloon         = "<b>Urselks Saloon</b>";              // {35}
    public static readonly string UrselksBallroom       = "<b>Urselks Ballroom</b>";            // {36}
    public static readonly string KelsingorMansion      = "<b>Kelsingør Mansion</b>";           // {37}
    public static readonly string Kaffe                 = "<b>Kaffe</b>";                       // {38}
    public static readonly string Latte                 = "<b>Latte</b>";                       // {39}
    public static readonly string MagicCircle           = "<b>Magic Circle</b>";                // {40}
    public static readonly string Menu                  = "<b>Menu</b>";                        // {41}
    public static readonly string BadSpecters           = "<b>Bad Specters</b>";                // {42}
    public static readonly string BadSpecter            = "<b>Bad Specter</b>";                 // {43}
    public static readonly string HouseMaster           = "<b>House Master</b>";                // {44}
    public static readonly string Thoughts              = "<b>Thoughts</b>";                    // {45}
    public static readonly string HeartsCapacity        = "<b>Hearts Capacity</b>";             // {46}
    public static readonly string Vx                    = "<b>Vx</b>";                          // {47}
    public static readonly string Dan                   = "<b>dan</b>";                         // {48}
    public static readonly string NauticalDawn          = "<b>6:11 a.m.</b>";                   // {49}
    public static readonly string Mon                   = "<b>Monday</b>";                      // {50}
    public static readonly string Tue                   = "<b>Tuesday</b>";                     // {51}
    public static readonly string Wed                   = "<b>Wednesday</b>";                   // {52}
    public static readonly string Thu                   = "<b>Thursday</b>";                    // {53}
    public static readonly string Fri                   = "<b>Friday</b>";                      // {54}
    public static readonly string Sat                   = "<b>Saturday</b>";                    // {55}
    public static readonly string Sun                   = "<b>Sunday</b>";                      // {56}
    public static readonly string KingEclaire           = "<b>King Eclaire</b>";                // {57}

    // Give these getters and setters so we can see Names while dev'ing
    [SerializeField] private string _Player             = "<b>???</b>";
    [SerializeField] private string _Eileen             = "<b>???</b>";
    [SerializeField] private string _Ellenia            = "<b>???</b>";
    [SerializeField] private string _ElleniaPassword    = null;
    [SerializeField] private string _Tedwich            = "<b>???</b>";
    [SerializeField] private string _Ursie              = "<b>???</b>";

    // Updater functions. Call from dialogue to update the updateable name.
    public static void UpdateEileen() { Eileen = "<b>Eileen</b>"; }
    public static void UpdateEllenia() { Ellenia = "<b>Ellenia</b>"; }
    public static void UpdateElleniaPassword(string s) { ElleniaPassword = s; }
    public static void UpdateTedwich() { Tedwich = "<b>Tedwich</b>"; }
    public static void UpdateUrsie() { Ursie = "<b>Ursie</b>"; }

    // For loading game.
    public static void LoadNames(Model_Names names)
    {
        Player                  = names.Player;
        Eileen                  = names.Eileen;
        Ellenia                 = names.Ellenia;
        ElleniaPassword         = names.ElleniaPassword;
        Tedwich                 = names.Tedwich;
        Ursie                   = names.Ursie;
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