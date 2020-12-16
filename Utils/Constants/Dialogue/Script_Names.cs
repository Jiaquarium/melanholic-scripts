using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Script_Names: MonoBehaviour
{
    public static Script_Names Names;
    
    public static string Player
    {
        get { return $"<b>{Names?._Player}</b>"; }
        set { Names._Player = value; }
    } // {0}
    public static readonly string Melz = "<b>Melz</b>";                     // {1}
    public static readonly string MelzTheGreat = "<b>Melz the Great</b>";   // {2}
    public static readonly string Ids = "<b>Ids</b>";                       // {3}
    public static readonly string Ero = "<b>Ero</b>";                    // {4}
    public static readonly string SBook = "<b>S-book</b>";                   // {5}
    public static readonly string Urselk = "<b>Urselk</b>";                   // {6}
    public static readonly string Urselks = "<b>Urselks</b>";                // {7}
    public static readonly string Anima = "<b>Anima</b>";        // {8}
    public static readonly string Aenimals = "<b>ænimals</b>";      // {9}
    public static readonly string Myne = "<b>Myne</b>";             // {10}
    public static string Eileen { get { return Names?._Eileen; } set { Names._Eileen = value; } }                     // {11}
    public static string Ellenia  { get { return Names?._Ellenia; } set { Names._Ellenia = value; } }                    // {12}
    public static readonly string Specter = "<b>Specter</b>";       // {13}
    public static string ElleniaPassword  { get { return Names?._ElleniaPassword; } set { Names._ElleniaPassword = value; } } // {14} Updated in Eileen Room (L21)
    public static readonly string Tedmunch      = "<b>Tedmunch</b>";    // {15}
    public static string Tedwich  { get { return Names?._Tedwich; } set { Names._Tedwich = value; } } // {16} Updated L11 SavePoint
    public static readonly string Tedward       = "<b>Tedward</b>";     // {17}
    public static readonly string Kelsingor     = "<b>Kelsingør</b>";   // {18}
    public static readonly string Specters      = "<b>Specters</b>";    // {19}
    public static readonly string Aenimal       = "<b>ænimal</b>";      // {20}
    public static readonly string Sheepluff     = "<b>Sheepluff</b>";   // {21}
    public static readonly string Sieving      = "<b>Sieving</b>";   // {22}
    public static readonly string Action1       = "<b><i>SPACE or ENTER</i></b>";   // {23}
    public static readonly string Action2       = "<b><i>X or RIGHT-SHIFT-KEY</i></b>";        // {24}
    public static readonly string Action3       = "<b><i>LEFT-SHIFT-KEY</i></b>"; // {25}
    public static readonly string InventoryKeyCode = "<b><i>1</i></b>";     // {26}
    public static readonly string Escape        = "<b><i>ESC</i></b>";     // {27}
    public static readonly string Skip          = "<b><i>SPACE or ENTER</i></b>"; // {28}
    public static readonly string Tedmas       = "<b>Tedmas</b>";       // {29}
    public static readonly string Sieve      = "<b>Sieve</b>";         // {30}
    public static readonly string Master      = "<b>Master</b>";         // {31}
    public static readonly string Inventory      = "<b>Inventory</b>";         // {32}
    public static string Ursie { get { return Names?._Ursie; } set { Names._Ursie = value; } } // {33}
    public static readonly string UrselkHouse      = "<b>Urselk House</b>";         // {34}
    public static readonly string UrselksSaloon     = "<b>Urselks Saloon</b>";       // {35}
    public static readonly string UrselksBallroom     = "<b>Urselks Ballroom</b>";       // {36}
    public static readonly string KelsingorMansion     = "<b>Kelsingør Mansion</b>";   // {37}
    public static readonly string Kaffe             = "<b>Kaffe</b>";   // {38}
    public static readonly string Latte             = "<b>Latte</b>";   // {39}
    public static readonly string MagicCircle        = "<b>Magic Circle</b>";   // {40}
    public static readonly string Menu              = "<b>Menu</b>";   // {41}
    public static readonly string BadSpecters      = "<b>Bad Specters</b>";    // {42}
    public static readonly string BadSpecter        = "<b>Bad Specter</b>";    // {43}
    public static readonly string HouseMaster       = "<b>House Master</b>";    // {44}
    public static readonly string Thoughts          = "<b>Thoughts</b>";    // {45}
    public static readonly string HeartsCapacity          = "<b>Hearts Capacity</b>";    // {46}
    public static readonly string Vx                = "<b>Vx</b>";    // {47}
    public static readonly string Dan                = "<b>dan</b>";    // {48}
    public static readonly string NauticalDawn          = "<b>6:11 a.m.</b>";    // {49}
    


    /// Give these getters and setters so we can see Names while dev'ing!!!
    [SerializeField] private string _Player             = "<b>???</b>";
    [SerializeField] private string _Eileen             = "<b>???</b>";
    [SerializeField] private string _Ellenia            = "<b>???</b>";
    [SerializeField] private string _ElleniaPassword    = null;
    [SerializeField] private string _Tedwich            = "<b>???</b>";
    [SerializeField] private string _Ursie              = "<b>???</b>";
    

    public static void UpdateEileen() { Eileen = "<b>Eileen</b>"; }
    public static void UpdateEllenia() { Ellenia = "<b>Ellenia</b>"; }
    public static void UpdateElleniaPassword(string s) { ElleniaPassword = s; }
    public static void UpdateTedwich() { Tedwich = "<b>Tedwich</b>"; }
    public static void UpdateUrsie() { Ursie = "<b>Ursie</b>"; }

    /// <summary>
    /// For loading game
    /// </summary>
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