﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Text;
using System.Linq;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Rewired;

public delegate void modifierCallback(Vector3Int tileLoc);

public static class Script_Utils
{
    private static readonly int LastMoveX = Animator.StringToHash("LastMoveX");
    private static readonly int LastMoveZ = Animator.StringToHash("LastMoveZ");
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveZ = Animator.StringToHash("MoveZ");

    public const string AppId = "1826060";
    public static readonly string SteamClientStoreURL = $"steam://store/{AppId}";
    
    // Replaced '×' with 'ẋ'; '×' leads to errors and doesn't read as Letter or Digit with Char.IsLetterOrDigit
    public static List<char> glitchLetters = new List<char>()
    {
        'Â','Ã','Ä','Å','Æ','Ç','È','É','Ê','Ë','Ì','Í','Î','Ï','Ð','Ñ','Ò','Ó','Ô','Õ','Ö','ẋ','Ø','Ù','Ú','Û','Ü',
        'Ý','Þ','ß','à','á','â','ã','ä','å','æ','ç','è','é','ê','ë','ì','í','î','ï','ð','ñ','ò','ó','ô','õ','ö','ø',
        'ù','ú','û','ü','ý','þ','ÿ','Ā','ā','Ă','ă','Ą','ą','Ć','ć','Ĉ','ĉ','Ċ','ċ','Č','č','Ď','ď','Đ','đ','Ē','ē',
        'Ĕ','ĕ','Ė','ė','Ę','ę','Ě','ě','Ĝ','ĝ','Ğ','ğ','Ġ','ġ','Ģ','ģ','Ĥ','ĥ','Ħ','ħ','Ĩ','ĩ','Ī','ī','Ĭ','ĭ','Į',
        'į','İ','ı','Ĳ','ĳ','Ĵ','ĵ','Ķ','ķ','ĸ','Ĺ','ĺ','Ļ','ļ','Ľ','ľ','Ŀ','ŀ','Ł','ł','Ń','ń','Ņ','ņ','Ň','ň','ŉ',
        'Ŋ','ŋ','Ō','ō','Ŏ','ŏ','Ő','ő','Œ','œ','Ŕ','ŕ','Ŗ','ŗ','Ř','ř','Ś','ś','Ŝ','ŝ','Ş','ş','Š','š','Ţ','ţ','Ť',
        'ť','Ŧ','ŧ','Ũ','ũ','Ū','ū','Ŭ','ŭ','Ů','ů','Ű','ű','Ų','ų','Ŵ','ŵ','Ŷ','ŷ','Ÿ','Ź','ź','Ż','ż','Ž','ž',
        'A','a','E','e','I','i','O','o','U','u','R','r','S','s','T','T'
    };

    public static Dictionary<KeyCode, string> HumanReadableKeyCodes = new Dictionary<KeyCode, string>()
    {
        { KeyCode.Escape, "Esc" },
        { KeyCode.KeypadPeriod, "Keypad ." },
        { KeyCode.KeypadDivide, "Keypad /" },
        { KeyCode.KeypadMultiply, "Keypad *" },
        { KeyCode.KeypadMinus, "Keypad -" },
        { KeyCode.KeypadPlus, "Keypad +" },
        { KeyCode.KeypadEnter, "Keypad Enter" },
        { KeyCode.KeypadEquals, "Keypad =" },
        { KeyCode.UpArrow, "Up" },
        { KeyCode.DownArrow, "Down" },
        { KeyCode.RightArrow, "Right" },
        { KeyCode.LeftArrow, "Left" },
        { KeyCode.Alpha0, "0" },
        { KeyCode.Alpha1, "1" },
        { KeyCode.Alpha2, "2" },
        { KeyCode.Alpha3, "3" },
        { KeyCode.Alpha4, "4" },
        { KeyCode.Alpha5, "5" },
        { KeyCode.Alpha6, "6" },
        { KeyCode.Alpha7, "7" },
        { KeyCode.Alpha8, "8" },
        { KeyCode.Alpha9, "9" },
        { KeyCode.Exclaim, "!" },
        { KeyCode.DoubleQuote, "”" },
        { KeyCode.Hash, "#" },
        { KeyCode.Dollar, "$" },
        { KeyCode.Percent, "%" },
        { KeyCode.Ampersand, "&" },
        { KeyCode.Quote, "’" },
        { KeyCode.LeftParen, "(" },
        { KeyCode.RightParen, ")" },
        { KeyCode.Asterisk, "*" },
        { KeyCode.Plus, "+" },
        { KeyCode.Comma, "," },
        { KeyCode.Minus, "-" },
        { KeyCode.Period, "." },
        { KeyCode.Slash, "/" },
        { KeyCode.Colon, ":" },
        { KeyCode.Semicolon, ";" },
        { KeyCode.Less, "<" },
        { KeyCode.Equals, "=" },
        { KeyCode.Greater, ">" },
        { KeyCode.Question, "?" },
        { KeyCode.At, "@" },
        { KeyCode.LeftBracket, "[" },
        { KeyCode.Backslash, "\\" },
        { KeyCode.RightBracket, "]" },
        { KeyCode.Caret, "^" },
        { KeyCode.Underscore, "_" },
        { KeyCode.BackQuote, "`" },
        { KeyCode.LeftCurlyBracket, "{" },
        { KeyCode.Pipe, "|" },
        { KeyCode.RightCurlyBracket, "}" },
        { KeyCode.Tilde, "~" },
        { KeyCode.RightShift, "Right Shift" },
        { KeyCode.LeftShift, "Left Shift" },
        { KeyCode.RightControl, "Right Control" },
        { KeyCode.LeftControl, "Left Control" },
        { KeyCode.RightAlt, "Right Alt" },
        { KeyCode.LeftAlt, "Left Alt" },
        { KeyCode.LeftCommand, "Left Command" },
        { KeyCode.LeftWindows, "Left Windows" },
        { KeyCode.RightCommand, "Right Command" },
        { KeyCode.RightWindows, "Right Windows" },
        { KeyCode.AltGr, "Alt Gr" },
        { KeyCode.SysReq, "Sys Req" },
    };
    
    public static T FindComponentInChildWithTag<T>(
        this GameObject parent, string tag
    ) where T:Component
    {
        Transform t = parent.transform;
        foreach(Transform tr in t)
        {
            if(tr.tag == tag)
            {
                return tr.GetComponent<T>();
            }
        }
        return null;
    }

    /// <summary>
    /// get the ONLY DIRECT children of this of specific type 
    /// to search recursively, use Unity func GetComponentsInChildren(Type type, bool includeInactive = false)
    /// </summary>
    /// <param name="parent">can use like function but also like [].GetChildren()</param>
    /// <param name="onlyActive">will only get active children</param>
    /// <typeparam name="T">Component Type</typeparam>
    /// <returns>Array of ONLY components of that type</returns>
    public static T[] GetChildren<T>(this Transform parent, bool onlyActive = false)
    {
        T[] children = new T[parent.childCount];
        for (int i = 0; i < children.Length; i++)
        {
            if (parent.GetChild(i).GetComponent<T>() != null)
            {
                if (onlyActive && !parent.GetChild(i).gameObject.activeSelf)    continue;
                
                children[i] = parent.GetChild(i).GetComponent<T>();
            }
        }
        
        T[] nonNullChildren = children.Where(x => x != null && !x.Equals(null)).ToArray();
        
        return nonNullChildren;
    }

    public static GameObject FindChildWithTag(
        this GameObject parent, string tag
    )
    {
        Transform t = parent.transform;
        foreach(Transform tr in t)
        {
            if(tr.tag == tag)
            {
                return tr.gameObject;
            }
        }
        return null;
    }

    /// <summary>
    /// GetComponentInParent will only return active objects. This will return all.
    /// </summary>
    public static T GetParentRecursive<T>(this Transform t) where T:Component
    {
        while (t.parent != null)
        {
            if (t.parent.GetComponent<T>() != null) return t.parent.GetComponent<T>();
            else                                    t = t.parent.transform;
        }
        return null; // Could not find a parent with given tag.
    }

    public static Dictionary<Directions, Vector3> GetDirectionToVectorDict()
    {
        return new Dictionary<Directions, Vector3>()
        {
            {Directions.Up       , new Vector3(0f, 0f, 1f)},
            {Directions.Down     , new Vector3(0f, 0f, -1f)},
            {Directions.Left     , new Vector3(-1f, 0f, 0f)},
            {Directions.Right    , new Vector3(1f, 0f, 0f)}
        };
    }

    public static Vector3 DirectionToVector(this Directions dir)
    {
        return GetDirectionToVectorDict()[dir];
    }

    public static bool GetButtonFromDirection(this Player rewiredInput, Directions dir)
    {
        switch (dir)
        {
            case Directions.Up:
                return rewiredInput.GetButton(Const_KeyCodes.RWVertical);
            case Directions.Down:
                return rewiredInput.GetNegativeButton(Const_KeyCodes.RWVertical);
            case Directions.Right:
                return rewiredInput.GetButton(Const_KeyCodes.RWHorizontal);
            case Directions.Left:
                return rewiredInput.GetNegativeButton(Const_KeyCodes.RWHorizontal);
            default:
                return false;
        }
    }

    public static Directions ReverseDirection(Directions dir) => dir switch
    {
        Directions.Up => Directions.Down,
        Directions.Down => Directions.Up,
        Directions.Left => Directions.Right,
        Directions.Right => Directions.Left,
        _ => Directions.None
    };

    public static Vector3 MovesToVector(Model_MoveSet moveSet)
    {
        Dictionary<Directions, Vector3> directions = GetDirectionToVectorDict();
        Vector3 v = Vector3.zero;
        foreach(Directions m in moveSet.moves)
        {
            v = v + directions[m];
        }
        return v;
    }

    public static Directions IntToDirection(this int i)
    {
        switch (i)
        {
            case 0:
                return Directions.Up;
            case 1:
                return Directions.Right;
            case 2:
                return Directions.Down;
            case 3:
                return Directions.Left;
            default:
                return Directions.None;
        }
    }

    /// <summary>
    /// Compare two Vector3s.
    /// </summary>
    /// <param name="vectorA"></param>
    /// <param name="vectorB"></param>
    /// <param name="decimalPlacesCompare">Specify number of decimal places to compare to</param>
    /// <returns>True if similar, False if not</returns>
    public static bool IsSame(this Vector3 vectorA, Vector3 vectorB, int decimalPlaces = 0)
    {
        return Math.Round(vectorA.x, decimalPlaces) == Math.Round(vectorB.x, decimalPlaces)
            && Math.Round(vectorA.y, decimalPlaces) == Math.Round(vectorB.y, decimalPlaces)
            && Math.Round(vectorA.z, decimalPlaces) == Math.Round(vectorB.z, decimalPlaces);
    }

    public static void AnimatorSetDirection(this Animator animator, Directions dir)
    {
        if (dir == Directions.Up)
        {
            animator.SetFloat(LastMoveX, 0f);
            animator.SetFloat(LastMoveZ, 1f);
            animator.SetFloat(MoveX, 0f);
            animator.SetFloat(MoveZ, 1f);
        }
        else if (dir == Directions.Down)
        {
            animator.SetFloat(LastMoveX, 0f);
            animator.SetFloat(LastMoveZ, -1f);
            animator.SetFloat(MoveX, 0f);
            animator.SetFloat(MoveZ, -1f);
        }
        else if (dir == Directions.Left)
        {
            animator.SetFloat(LastMoveX, -1f);
            animator.SetFloat(LastMoveZ, 0f);
            animator.SetFloat(MoveX, -1f);
            animator.SetFloat(MoveZ, 0f);
        }
        else if (dir == Directions.Right)
        {
            animator.SetFloat(LastMoveX, 1f);
            animator.SetFloat(LastMoveZ, 0f);
            animator.SetFloat(MoveX, 1f);
            animator.SetFloat(MoveZ, 0f);
        }
    }

    /// <summary>
    /// Useful mainly for end of timeline when trying to connect states seamlessly.
    /// </summary>
    /// <param name="animator">The animator to reset</param>
    /// <param name="layer">Animator layer, 0 is default</param>
    public static void ResetAnimator(this Animator animator, int layer)
    {
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(layer);
        animator.Play(animatorStateInfo.fullPathHash, layer, 0f);
    }

    /// <summary>
    /// Params for item names defined in Script_ItemStringBuilder
    /// </summary>
    public static string FormatString(
        this string unformattedString,
        bool isFormatInventoryKey = false,
        bool isFormatSpeedKey = false,
        bool isFormatMaskCommandKey = false,
        bool isFormatInteractKey = false,
        bool isForceGamepadParamsWhenConnected = false
    )
    {
        string itemFormattedStr = ReplaceParams(
            unformattedString,
            Script_ItemStringBuilder.Params
        );
        
        string itemAndDynamicFormattedStr = itemFormattedStr;
        try
        {
            Script_DynamicStringBuilder.BuildParams();

            // Opt into these params because they are very slow to fetch
            // due to InputControlPath.ToHumanReadableString
            if (isFormatInventoryKey)
                Script_DynamicStringBuilder.BuildInventoryParam(isForceGamepadParamsWhenConnected);
            
            if (isFormatSpeedKey)
                Script_DynamicStringBuilder.BuildSpeedParam(isForceGamepadParamsWhenConnected);
            
            if (isFormatMaskCommandKey)
                Script_DynamicStringBuilder.BuildMaskCommandParam(isForceGamepadParamsWhenConnected);
            
            if (isFormatInteractKey)
                Script_DynamicStringBuilder.BuildInteractParam(isForceGamepadParamsWhenConnected);

            itemAndDynamicFormattedStr = ReplaceParams(
                itemFormattedStr,
                Script_DynamicStringBuilder.Params
            );
        }
        catch (SystemException e)
        {
            Debug.LogError(e);
        }

        return string.Format(
            itemAndDynamicFormattedStr,
            Script_Names.Player,                        // {0}
            Script_Names.Mask,                          // {1}
            Script_Names.PlayerUnbold,                  // {2}
            Script_Names.Ids,                           // {3}
            Script_Names.Ero,                           // {4}
            Script_Names.PreppedMasks,                  // {5}
            Script_Names.Urselk,                        // {6}
            Script_Names.Urselks,                       // {7}
            Script_Names.Owner,                         // {8}
            Script_Names.ImpermanentUpper,              // {9}
            Script_Names.Myne,                          // {10}
            Script_Names.Eileen,                        // {11}  
            Script_Names.Ellenia,                       // {12}
            Script_Names.Specter,                       // {13}
            Script_Names.ElleniaPassword,               // {14}
            Script_Names.SwitchActiveStickerKeycodesJoystick, // {15}
            Script_Names.Tedwich,                       // {16}
            Script_Names.FlanShort,                     // {17}
            Script_Names.Kelsingor,                     // {18}
            Script_Names.Specters,                      // {19}
            Script_Names.CatwalkInTheSky,               // {20}
            Script_Names.Sheepluff,                     // {21}
            Script_Names.Sealing,                       // {22}
            Script_Names.Action1, // SPACE              // {23}
            Script_Names.Action2, // X                  // {24}
            Script_Names.Action3, // LEFT SHIFT         // {25}
            Script_Names.InventoryKeyCode,              // {26}
            Script_Names.Escape,                        // {27}
            Script_Names.Skip,                          // {28}
            Script_Names.UrsaSaloonHallway,             // {29}
            Script_Names.Sieve,                         // {30}
            Script_Names.Master,                        // {31}
            Script_Names.Inventory,                     // {32}
            Script_Names.Ursie,                         // {33}
            Script_Names.UrselkHouse,                   // {34}
            Script_Names.UrsaSaloon,                    // {35}
            Script_Names.Ballroom,                      // {36}
            Script_Names.KelsingorMansion,              // {37}
            Script_Names.Kaffe,                         // {38}
            Script_Names.Latte,                         // {39}
            Script_Names.MagicCircle,                   // {40}
            Script_Names.Menu,                          // {41}
            Script_Names.CursedOnes,                    // {42}
            Script_Names.CursedOne,                     // {43}
            Script_Names.Notes,                         // {44}
            Script_Names.Thoughts,                      // {45}
            Script_Names.HeartsCapacity,                // {46}
            Script_Names.Vx,                            // {47}
            Script_Names.Dan,                           // {48}
            Script_Names.T600am,                        // {49}
            Script_Names.Mon,                           // {50}
            Script_Names.Tue,                           // {51}
            Script_Names.Wed,                           // {52}
            Script_Names.Thu,                           // {53}
            Script_Names.Fri,                           // {54}
            Script_Names.Sat,                           // {55}
            Script_Names.Sun,                           // {56}
            Script_Names.KingEclaire,                   // {57}
            Script_Names.Suzette,                       // {58}
            Script_Names.Shiteater,                     // {59}
            Script_Names.Shiteaters,                    // {60}
            Script_Names.Peche,                         // {61}
            Script_Names.Melba,                         // {62}
            Script_Names.Moose,                         // {63}
            Script_Names.Flan,                          // {64}
            Script_Names.ActionSticker,                 // {65}
            Script_Names.LastElevator,                  // {66}
            Script_Names.StickerSkill,                  // {67}
            Script_Names.StickerSkills,                 // {68}
            Script_Names.Coconut,                       // {69}
            Script_Names.WeekdayShift,                  // {70}
            Script_Names.WeekendShift,                  // {71}
            Script_Names.ElevatorBay,                   // {72}
            Script_Names.ControllableSpecters,          // {73}
            Script_Names.Unknown,                       // {74}
            Script_Names.XXXWorld,                      // {75}
            Script_Names.OtherSide,                     // {76}
            Script_Names.NauticalDawn,                  // {77}
            Script_Names.DarkDarkHall,                  // {78}
            Script_Names.ActiveStickerCommand,          // {79}
            Script_Names.SwitchActiveSticker,           // {80}
            Script_Names.Prep,                          // {81}
            Script_Names.SwitchActiveStickerKeyCodes,   // {82}
            Script_Names.Elder,                         // {83}
            Script_Names.WellsWorld,                    // {84}
            Script_Names.CelestialGardensWorld,         // {85}
            Script_Names.OwnerPlain,                    // {86}
            Script_Names.RockGarden,                    // {87}
            Script_Names.MirrorHalls,                   // {88}
            Script_Names.HallwayToBasement,             // {89}
            Script_Names.Basement,                      // {90}
            Script_Names.InsideAPainting,               // {91}
            Script_Names.DiningRoom,                    // {92}
            Script_Names.R2CursedTime,                  // {93}
            Script_Names.IdsDeadTime                    // {94}
        );
    }

    public static string FormatName(this string s)
    {
        return string.IsNullOrEmpty(s) ? "「 ? 」" : $"「 {s} 」";
    }

    public static char Zalgofy(this char c)
    {
        var random = new System.Random();
        int randomIndex = random.Next(glitchLetters.Count);
        return glitchLetters[randomIndex];
    }

    /// <summary>
    /// Note, this will even zalgofy tags.
    /// </summary>
    /// <param name="sentence">String without tags</param>
    /// <returns>Zalgofied string</returns>
    public static string ZalgofyUnrichString(this string sentence)
    {
        var stringBuilder = new StringBuilder(sentence);
        
        for (var i = 0; i < sentence.Length; i++)
        {
            char c = sentence[i];
            
            if (Char.IsLetterOrDigit(c) || c == Script_DialogueManager.DefaultDemonNPCChar)
            {
                var zalgoLetter = c.Zalgofy();
                stringBuilder.Remove(i, 1);
                stringBuilder.Insert(i, Char.ToString(zalgoLetter));
            }
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// copies elements from array 1 into array 2; depends on array2 size
    /// </summary>
    /// <param name="array1">array to copy from</param>
    /// <param name="array2">array to copy into</param>
    /// <typeparam name="T">type</typeparam>
    /// <returns>array2 with copied elements</returns>
    public static T[] CopyArrayElements<T>(
        T[] array1, T[] array2
    ) where T:Component
    {
        for (int i = 0; i < Mathf.Min(array2.Length, array1.Length); i++)
        {
            array2[i] = array1[i];
        }

        return array2;
    }

    /// <summary>
    /// Set all of list to desired active state up until the stated index
    /// If the index supplied is not in the range, then just set the whole range.
    /// </summary>
    /// <param name="items">List of components to modify</param>
    /// <param name="index">Up until this index change active state</param>
    /// <param name="isActive">Component's GameObject's active state</param>
    public static List<GameObject> SetAllUntilIndex(
        this List<GameObject> items, int index, bool isActive
    )
    {
        for (var i = 0; i <= Mathf.Min(items.Count - 1, index); i++)
            items[i].SetActive(isActive);
        
        return items;
    }

    /// <summary>
    /// Returns a new array w/o nulls. Does not mutate the original.
    /// </summary>
    /// <param name="arr">Any array</param>
    /// <typeparam name="T">Type</typeparam>
    /// <returns>A new array without nulls</returns>
    public static T[] FilterNulls<T>(this T[] arr)
    {
        return arr.Where(x => x != null && !x.Equals(null)).ToArray();
    }

    public static void MakeFontsCrispy(Font[] fonts)
    {
        foreach (Font f in fonts)
        {
            if (f)
            {
                Dev_Logger.Debug("making font crispy: " + f);
                f.material.mainTexture.filterMode = FilterMode.Point;
                f.material.mainTexture.anisoLevel = 0;
            }
        }
    }

    public static List<Vector3Int> AddTileLocs(
        List<Vector3Int> tileLocs,
        Tilemap tm,
        Action<Vector3Int> tileLocCallback
    )
    {
        foreach (var pos in tm.cellBounds.allPositionsWithin)
        {   
            Vector3Int tileLoc = new Vector3Int(pos.x, pos.y, pos.z);
            if (tm.HasTile(tileLoc))
            {
                tileLocs.Add(tileLoc);
                if (tileLocCallback != null) tileLocCallback(tileLoc);
            }
        }

        return tileLocs;
    }

    public static string Trim(this string s, int maxCharCount)
    {
        if (s.Length != 0 && s.Length > maxCharCount)
            return s.Remove(maxCharCount - 1);
        else                                            
            return s;   
    }

    /// <summary>
    /// replaces params defined in dictionary
    /// </summary>
    public static string ReplaceParams(string s, Dictionary<string, string> parameters)
    {
        return parameters.Aggregate(s, (current, parameter) =>
            current.Replace(parameter.Key, parameter.Value.ToString())
        );
    }

    /// <summary>
    /// Gives the Direction to target
    /// perfect diagonals will be prioritized as Left and Right
    /// </summary>
    public static Directions GetDirectionToTarget(this Vector3 myPos, Vector3 targetPos)
    {        
        Dictionary<CompassDir, Directions> CompassToDirectionDict = new Dictionary<CompassDir, Directions>
        {
            { CompassDir.E, Directions.Right },
            { CompassDir.N, Directions.Up },
            { CompassDir.W, Directions.Left },
            { CompassDir.S, Directions.Down },
        };
        
        Vector3 heading = targetPos - myPos;
        float distance = heading.magnitude;
        var direction = heading / distance;
        
        float angle = Mathf.Atan2(direction.z, direction.x);
        int octant = (int)(Mathf.Round(8 * angle / (2 * Mathf.PI) + 8) % 8);

        CompassDir dir = (CompassDir)octant;
        Directions dirToTarget;
    
        // Handle even the diagonal cases, if perfectly diagonal
        // default to Right (East) or Left (West)
        switch (dir)
        {
            case (CompassDir.E):
            case (CompassDir.N):
            case (CompassDir.W):
            case (CompassDir.S):
                dirToTarget = CompassToDirectionDict[dir];
                break;
            case (CompassDir.NE):
                if (direction.x * direction.x >= direction.z * direction.z)
                    dirToTarget = CompassToDirectionDict[CompassDir.E];
                else
                    dirToTarget = CompassToDirectionDict[CompassDir.N];
                break;
            case (CompassDir.NW):
                if (direction.x * direction.x >= direction.z * direction.z)
                    dirToTarget = CompassToDirectionDict[CompassDir.W];
                else
                    dirToTarget = CompassToDirectionDict[CompassDir.N];
                break;
            case (CompassDir.SE):
                if (direction.x * direction.x >= direction.z * direction.z)
                    dirToTarget = CompassToDirectionDict[CompassDir.E];
                else
                    dirToTarget = CompassToDirectionDict[CompassDir.S];
                break;
            case (CompassDir.SW):
                if (direction.x * direction.x >= direction.z * direction.z)
                    dirToTarget = CompassToDirectionDict[CompassDir.W];
                else
                    dirToTarget = CompassToDirectionDict[CompassDir.S];
                break;
            default:
                dirToTarget = Directions.None;
                break;
        }
            
        return dirToTarget;
    }

    public static Directions GetMyDirectionToTarget(this Transform obj, Transform target)
    {
        return GetDirectionToTarget(obj.position, target.position);
    }

    private enum CompassDir
    {
        E = 0, NE = 1,
        N = 2, NW = 3,
        W = 4, SW = 5,
        S = 6, SE = 7
    };

    public static string FormatSecondsHHMMSS(this float t)
    {
        string fmt = "00";

        int tRemainder = (int)t; 

        int hours = Mathf.FloorToInt(tRemainder / 3600);
        tRemainder -= hours * 3600;
        
        int min = Mathf.FloorToInt(tRemainder / 60);
        tRemainder -= min * 60;

        int sec = Mathf.RoundToInt(tRemainder);

        return $"{hours.ToString(fmt)}:{min.ToString(fmt)}:{sec.ToString(fmt)}";
    }

    public static string FormatSecondsHrMinSec(this float t)
    {
        string fmt = "0";

        int tRemainder = (int)t; 

        int hours = Mathf.FloorToInt(tRemainder / 3600);
        tRemainder -= hours * 3600;
        
        int min = Mathf.FloorToInt(tRemainder / 60);
        tRemainder -= min * 60;

        int sec = Mathf.RoundToInt(tRemainder);

        return $"{hours.ToString(fmt)} hr {min.ToString(fmt)} min {sec.ToString(fmt)} sec";
    }

    public static string FormatTotalPlayTime(this float t)
    {
        return $"Total time: {t.FormatSecondsHrMinSec()}";
    }

    public static string FormatSecondsClock(this float t, bool isClose, bool hideColons = false)
    {
        string fmt = "00";

        int tRemainder = (int)t; 

        int hours = Mathf.FloorToInt(tRemainder / 3600);
        tRemainder -= hours * 3600;
        
        int min = Mathf.FloorToInt(tRemainder / 60);
        tRemainder -= min * 60;

        int sec = tRemainder;

        string timeDisplay = isClose
            ? $"{hours.ToString(fmt)}:{min.ToString(fmt)}:{sec.ToString(fmt)}am".AddBrackets()
            : $"{hours.ToString(fmt)}:{min.ToString(fmt)}am".AddBrackets();
        
        return hideColons
            ? timeDisplay.Replace(":", " ")
            : timeDisplay;
    }

    public static float RoundSecondsDownToMinute(this float sec)
    {
        int roundedDownSec = (int)(sec / 60) * 60;

        return (float)roundedDownSec;
    }

    public static string FormatDateTime(this DateTime date)
    {
        var s = date.ToString("MMMM");
        var monthTitleCase = char.ToUpper(s[0]) + s.Substring(1);
        var d = date.ToString("dd, yyyy");
        
        return $"{monthTitleCase} {d}";
    }

    public static string FormatLastPlayedDateTime(this DateTime date)
    {
        return $"Last played: {date.FormatDateTime()}";
    }

    public static string FormatRun(this string run)
    {
        return $"{run.ToString()}";
    }

    public static string FormatCycleCount(this int count)
    {
        return $"no.{count + 1}";
    }

    public static string AddBrackets(this string text, bool withSpace = true)
    {
        return withSpace ? $"『 {text} 』" : $"『{text}』";
    }

    /// <summary>
    /// Sets an up and down UI elements list explicit navigation
    /// Ensure each element in the array has a Button component
    /// </summary>
    public static T[] SetExplicitListNav<T>(this T[] children) where T:Component
    { 
        if (children.Length <= 1)   return children;
        foreach (T child in children)
        {
            if (child.GetComponent<Button>() == null)
            {
                Debug.LogError("Each element needs a Button component to set explicit nav!");
                return children;
            }
        }
        
        for (int i = 0; i < children.Length; i++)
        {
            Transform thisChild     = children[i].transform;
            // if first child only set down nav
            if (i == 0)
            {
                Transform nextChild     = children[i + 1].transform;

                Navigation thisNav      = thisChild.GetComponent<Selectable>().navigation;
                thisNav.selectOnDown    = nextChild.GetComponent<Button>();
                thisChild.GetComponent<Selectable>().navigation = thisNav;
            }
            // if last child only set up nav
            else if (i == children.Length - 1)
            {
                Transform lastChild     = children[i - 1].transform;

                Navigation thisNav      = thisChild.GetComponent<Selectable>().navigation;
                thisNav.selectOnUp      = lastChild.GetComponent<Button>();
                thisChild.GetComponent<Selectable>().navigation = thisNav;
            }
            else // set this down and up nav
            {
                Transform lastChild     = children[i - 1].transform;
                Transform nextChild     = children[i + 1].transform;

                Navigation thisNav      = thisChild.GetComponent<Selectable>().navigation;
                thisNav.selectOnUp      = lastChild.GetComponent<Button>();
                thisNav.selectOnDown    = nextChild.GetComponent<Button>();
                thisChild.GetComponent<Selectable>().navigation = thisNav;
            }
        }

        return children;
    }

    public static void SetNavigation(
        this Button buttonToChange,
        Button newSelectOnUp = null,
        Button newSelectOnLeft = null,
        Button newSelectOnDown = null,
        Button newSelectOnRight = null
    )
    {
        Navigation btnNav = buttonToChange.GetComponent<Selectable>().navigation;
        
        if (newSelectOnUp != null)
            btnNav.selectOnUp = newSelectOnUp;
        
        if (newSelectOnLeft != null)
            btnNav.selectOnLeft = newSelectOnLeft;
        
        if (newSelectOnDown != null)
            btnNav.selectOnDown = newSelectOnDown;

        if (newSelectOnRight != null)
            btnNav.selectOnRight = newSelectOnRight;

        buttonToChange.GetComponent<Selectable>().navigation = btnNav;
    }

    public static void PrintArray<T>(this System.Collections.Generic.IEnumerable<T> list, string label = "array: ")
    {
        if (Debug.isDebugBuild)
            Dev_Logger.Debug($"{label}: {String.Join("", new List<T>(list).ConvertAll(i => i?.ToString() ?? "null").ToArray())}");
    }

    public static T[] AddItemToArray<T>(this T[] arr, T item) where T:Component
    {
        return arr.Concat(Enumerable.Repeat(item, 1)).ToArray();
    }

    /// <summary>
    /// ex: myClassInstance.GetProperty("isPuzzleComplete");
    /// Properties have getters and setters
    /// </summary>
    /// <param name="obj">instance type</param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static bool HasProp(this object obj, string propertyName) 
    {
        return obj.GetType().GetProperty(
            propertyName,
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance
        ) != null;
    }
    
    public static T GetProp<T>(this object obj, string propertyName) 
    {
        return (T)obj.GetType().GetProperty(
            propertyName,
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance
        ).GetValue(obj);
    }

    /// <summary>
    /// ex: myClassInstance.GetProperty("isPuzzleComplete");
    /// </summary>
    /// <param name="obj">instance type</param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static bool HasField(this object obj, string propertyName) 
    {
        return obj.GetType().GetField(
            propertyName,
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static
        ) != null;
    }
    
    public static T GetField<T>(this object obj, string propertyName) 
    {
        return (T)obj.GetType().GetField(
            propertyName,
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static
        ).GetValue(obj);
    }

    public static bool HasMethod(this object obj, string name)
    {
        Dev_Logger.Debug($"Checking for method: {name}; got {obj.GetType().GetMethod(name, BindingFlags.NonPublic)}");
        return obj.GetType().GetMethod(
            name,
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static
        ) != null;
    }

    public static void InvokeMethod(this object obj, string name)
    {
        MethodInfo method = obj.GetType().GetMethod(
            name,
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static
        );
        Dev_Logger.Debug($"invoking with MethodInfo: {method}");
        
        method.Invoke(obj, null);
    }

    public static Vector3Int ToVector3Int(this Vector3 location)
    {
        int x = (int)Mathf.Round(location.x);
        int y = (int)Mathf.Round(location.y);
        int z = (int)Mathf.Round(location.z);

        return new Vector3Int(x, y, z);
    }

    public static bool IsSorted(this int[] arr, bool isStrict = false)
    {
        for (int i = 1; i < arr.Length; i++)
        {
            if (isStrict)
            {
                if (arr[i - 1] >= arr[i])       return false;
            }
            else if (arr[i - 1] > arr[i])       return false;
        }
        return true;
    }

    public static bool IsSorted(this List<int> list, bool isStrict = false)
    {
        for (int i = 1; i < list.Count; i++)
        {
            if (isStrict)
            {
                if (list[i - 1] >= list[i])         return false;
            }
            else if (list[i - 1] > list[i])         return false;
        }
        return true;
    }

    public static float ToFadeTime(this FadeSpeeds fadeSpeed)
    {
        return Script_Utils.GetFadeTime(fadeSpeed);
    }

    /// <summary>
    /// Checks if there are any events on the event handler
    /// counts as not null if has attached an object target;
    /// doesn't need to have to specify a function.
    /// </summary>
    public static bool CheckUnityEventAction(this UnityEvent unityEvent)
    {
        for (int i = 0; i < unityEvent.GetPersistentEventCount(); i++)
        {
            if (unityEvent.GetPersistentTarget(i) != null)    return true;
        }

        return false;
    }

    public static bool SafeInvoke(this UnityEvent unityEvent)
    {
        if (unityEvent.CheckUnityEventAction())
        {
            unityEvent.Invoke();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Same as CheckUnityEventAction but allows to specify passing a param
    /// through the Event.
    /// </summary>
    public static bool CheckUnityEvent<T>(this UnityEvent<T> unityEvent)
    {
        for (int i = 0; i < unityEvent.GetPersistentEventCount(); i++)
        {
            if (unityEvent.GetPersistentTarget(i) != null)    return true;
        }

        return false;
    }

    public static void SafeInvokeDynamic<T>(this UnityEvent<T> unityEvent, T arg)
    {
        if (unityEvent.CheckUnityEvent<T>())
            unityEvent.Invoke(arg);
    }

    public static void DebugToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Dev_Logger.Debug(output);
    }

    public static bool CheckInTags(this List<Const_Tags.Tags> tags, string tag)
    {
        if (string.IsNullOrEmpty(tag))
            return false;

        // Check if Everything is included
        bool isDetectEverything = tags.FindIndex(tag => tag == Const_Tags.Tags.Everything) != -1;
        if (isDetectEverything)
            return true;
        
        Const_Tags.Tags result = tags.FirstOrDefault(_tag => Const_Tags.TagsMap[_tag] == tag);

        if (result.Equals(default(Const_Tags.Tags)))
            return false;
        else
            return true;
    }

    // Checks that value is not NaN or infinity.
    public static bool HasValue(this float value)
    {
        return !float.IsNaN(value) && !float.IsInfinity(value);
    }

    // Bind objects to tracks at runtime.
    // NOTE: do not use nested tracks, ordering can be unpredictable.
    public static void BindTimelineTracks(
        this PlayableDirector playableDirector,
        TimelineAsset timeline,
        List<GameObject> objectsToBind,
        bool isIgnoreNullBindings = false
    )
    {
        int i = 0;
        
        Dev_Logger.Debug($"track count: {timeline.outputTrackCount}; obj to bind count: {objectsToBind.Count}");
        Dev_Logger.Debug($"Director to bind: {playableDirector}");

        foreach (var track in timeline.outputs)
        {
            // Sometimes track.sourceObject will contain nulls, which will unsync tracks with objects to bind,
            // so filter nulls here.
            if (track.sourceObject == null)
                continue;
            
            if (i > objectsToBind.Count - 1)
            {
                Debug.LogError($"{playableDirector.name} There are more tracks than objects you are binding.");
                break;
            }
            
            Dev_Logger.Debug($"track: {track.sourceObject} to bind with: {objectsToBind[i]}, i={i}");
            
            // Option to leave track binding as it is if we pass in a null object
            if (isIgnoreNullBindings && objectsToBind[i] == null)
                Dev_Logger.Debug($"track: {track.sourceObject} leave as is.");
            else
                playableDirector.SetGenericBinding(track.sourceObject, objectsToBind[i]);
            i++;
        }
    }

    public static float GetFadeTime(this FadeSpeeds fadeSpeed)
    {
        const float fadeXFastTime = 0.10f;
        const float fadeFastTime = 0.25f;
        const float fadeMedFastTime = 0.50f;
        const float fadeMedTime = 0.75f;
        const float fadeSlowTime = 1.25f;
        const float fadeXSlowTime = 2.0f;
        const float fadeXXSlowTime = 5.0f;
        
        return fadeSpeed switch
        {
            FadeSpeeds.XFast => fadeXFastTime,
            FadeSpeeds.Fast => fadeFastTime,
            FadeSpeeds.MedFast => fadeMedFastTime,
            FadeSpeeds.Med => fadeMedTime,
            FadeSpeeds.Slow => fadeSlowTime,
            FadeSpeeds.XSlow => fadeXSlowTime,
            FadeSpeeds.XXSlow => fadeXXSlowTime,
            _ => 0f,
        };
    }

    /// <summary>
    /// Frame rate independent damping
    /// https://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp/
    /// </summary>
    /// <param name="source">from Vector</param>
    /// <param name="target">to Vector</param>
    /// <param name="smoothing">smoothing constant 0-1</param>
    /// <param name="time">delta time</param>
    /// <returns></returns>
    public static Vector3 FrameRateAwareDamp(this Vector3 source, Vector3 target, float smoothing, float time)
    {
        return Vector3.Lerp(source, target, 1 - Mathf.Pow(smoothing, time));
    }

    // -------------------------------------------------------------------------------------
    // Dialogue Utils
    public static bool IsChoicesNode(this Script_DialogueNode node)
    {
        return node.data.children.Length > 1 ||
            node.data.isChoices ||
            node.CheckChildrenForChoiceText();
    }

    static bool CheckChildrenForChoiceText(this Script_DialogueNode node)
    {
        foreach (var child in node.data.children)
        {
            if (!string.IsNullOrEmpty(child?.data?.choiceText))
                return true;
        }

        return false;
    }

    // -------------------------------------------------------------------------------------
    // Screen Helpers

    public static int GetCurrentDisplayIdx()
    {
        List<DisplayInfo> displayInfos = new List<DisplayInfo>();
        // Necessary call or Display Index will be -1
        Screen.GetDisplayLayout(displayInfos);
        
        return displayInfos.IndexOf(Screen.mainWindowDisplayInfo);
    }

    // Windows needs to first set max resolution before going to full screen; otherwise,
    // if currently on a forced windowed resolution, it will upscale from there.
    public static void SetFullScreenOnWindows(bool isAlreadyFullScreen = false)
    {
        // Set to max resolution of the display the app is on
        DisplayInfo displayInfo = Screen.mainWindowDisplayInfo;

        if (isAlreadyFullScreen)
        {
            // If already in full screen mode, don't override the fullscreen mode, only change res.
            Screen.SetResolution(displayInfo.width, displayInfo.height, Screen.fullScreenMode);
        }
        else
        {
            // "fullscreen: true" defaults to Fullscreen Window
            Screen.SetResolution(displayInfo.width, displayInfo.height, fullscreen: true);
        }
    }

    // Mac needs to first set the exact max pixel perfect rect resolution, or else it will letterbox
    // and scale the image, ruining the Pixel Perfectness. Setting to just DisplayInfo will result in this
    // unlike windows. The below algorithm works with all fullScreenMode and screen sizes.
    public static void SetFullScreenOnMac(double targetAspect, bool isAlreadyFullScreen = false)
    {
        DisplayInfo displayInfo = Screen.mainWindowDisplayInfo;
        Vector2Int pixelScreenSize = GetAspectEnforcedRect(displayInfo, targetAspect);

        if (isAlreadyFullScreen)
        {
            // If already in full screen mode, don't override the fullscreen mode, only change res.
            Screen.SetResolution(pixelScreenSize.x, pixelScreenSize.y, Screen.fullScreenMode);
        }
        else
        {
            // "fullscreen: true" defaults to Fullscreen Window
            Screen.SetResolution(pixelScreenSize.x, pixelScreenSize.y, fullscreen: true);
        }
    }

    /// <summary>
    /// Copied algorithm from custom implementation of Graphics/...PixelPerfectCamera
    /// </summary>
    public static Vector2Int GetAspectEnforcedRect(DisplayInfo displayInfo, double targetAspect)
    {
        double displayAspect = (double)displayInfo.width / (double)displayInfo.height;

        // Current display height should be scaled by this amount.
        double scaleHeight = displayAspect / targetAspect;

        int pixelHeight;
        int pixelWidth;

        // Letterbox
        if (scaleHeight < 1.0d)
        {
            double rawPixelHeight = scaleHeight * (double)displayInfo.height;
            pixelHeight = (int)Math.Round(rawPixelHeight, MidpointRounding.AwayFromZero);
            pixelWidth = displayInfo.width;
        }
        // Pillarbox
        else if (scaleHeight > 1.0d)
        {
            double scaleWidth = 1.0d / scaleHeight;
            pixelHeight = displayInfo.height;
            double rawPixelWidth = scaleWidth * (double)displayInfo.width;
            pixelWidth = (int)Math.Round(rawPixelWidth, MidpointRounding.AwayFromZero);
        }
        // No Cropping Necessary
        else
        {
            pixelWidth = displayInfo.width;
            pixelHeight = displayInfo.height;
        }

        return new Vector2Int(pixelWidth, pixelHeight);
    }

    // -------------------------------------------------------------------------------------
    // Conversions

    public static float ConvertFloatToDecibel(this float floatVol)
    {
        float clampedVol = Mathf.Clamp(floatVol, 0.0001f, 1f);

        return Mathf.Log10(clampedVol) * 20f;
    }

    public static float ConvertDecibelToFloat(this float dbVol) => Mathf.Pow(10f, (dbVol / 20f));

    // -------------------------------------------------------------------------------------
    // Hitbox Utils

    public static bool IsHitBoxBehaviorStateChanging(this Script_HitBoxBehavior hitBoxBehavior) =>
        hitBoxBehavior is Script_HitBoxRestartPlayerBehavior;

    // -------------------------------------------------------------------------------------
    // Physics

    public static bool CheckPlayerOverlap(this Script_PhysicsBox physicsBox)
    {
        // Check if Player is within the Physics Box
        List<Script_Player> players = new List<Script_Player>();
        physicsBox.ExposeBox();

        foreach (Collider col in physicsBox.Colliders)
        {
            if (col != null && col.transform.GetParentRecursive<Script_Player>() != null)
                players.Add(col.transform.GetParentRecursive<Script_Player>());
        }

        return players.Count > 0;
    }
    
    // -------------------------------------------------------------------------------------
    // Rewired Helpers
    
    public static bool IsJoystickSupported(this Controller controller)
    {
        IGamepadTemplate gamepad = controller.GetTemplate<IGamepadTemplate>();
        return gamepad != null;
    }
    
    /// <summary>
    /// Return the first bound keyboard human readable button name. Uses Unity Keycode string
    /// https://guavaman.com/projects/rewired/docs/HowTos.html#get-element-name-for-action
    /// This is a keyboard specific function as it fetches button maps
    /// </summary>
    public static string GetFirstMappingKeyboardByActionName(this Player rewiredInput, string actionName)
    {
        foreach (ActionElementMap aem in rewiredInput.controllers.maps.ButtonMapsWithAction(
            ControllerType.Keyboard, actionName, skipDisabledMaps: false
        ))
        {
            InputAction action = ReInput.mapping.GetAction(aem.actionId);
            
            if (action == null || aem.keyCode == KeyCode.None)
                continue;

            // Get the primary key code as a string
            KeyCode key = aem.keyCode;

            // Check custom defined human readable keycodes
            string customKeyCode;
            if (!Script_Utils.HumanReadableKeyCodes.TryGetValue(key, out customKeyCode))
                return key.ToString();

            return customKeyCode;
        }

        return null;
    }

    public static string GetFirstMappingByMap(
        this ControllerMap map,
        int actionId,
        Pole? axisContribution = null,
        AxisRange? axisRange = null
    )
    {
        ActionElementMap aem = map.GetFirstActionElementMapByMap(actionId, axisContribution, axisRange);

        return aem?.elementIdentifierName;
    }

    // Return the first bound human readable gamepad template button name
    // https://guavaman.com/projects/rewired/docs/HowTos.html#get-element-name-for-action
    public static ActionElementMap GetFirstActionElementMapByActionName(
        this Player rewiredInput,
        Controller controller,
        string actionName
    )
    {
        // Get the first button map with the Action
        ActionElementMap mapping = rewiredInput.controllers.maps.GetFirstElementMapWithAction(
            controller.type, actionName, skipDisabledMaps: false
        );
        
        return mapping;
    }

    public static string GetFirstMappingJoystickByActionName(
        this Player rewiredInput,
        Controller controller,
        string actionName
    )
    {
        ActionElementMap aem = GetFirstActionElementMapByActionName(rewiredInput, controller, actionName);

        if (aem == null)
            return null;
        
        return aem.elementIdentifierName;
    }


    public static ActionElementMap GetFirstActionElementMapByActionId(
        this Player rewiredInput,
        Controller controller,
        int actionId
    )
    {
        // Get the first button map with the Action
        ActionElementMap mapping = rewiredInput.controllers.maps.GetFirstElementMapWithAction(
            controller.type, actionId, skipDisabledMaps: false
        );
        
        return mapping;
    }

    public static ActionElementMap GetFirstActionElementMapByMap(
        this ControllerMap map,
        int actionId,
        Pole? axisContribution = null,
        AxisRange? axisRange = null
    )
    {
        foreach(var actionElementMap in map.ElementMapsWithAction(actionId))
        {
            if (actionElementMap != null)
            {
                // Check axisRange if given
                if (axisContribution != null && actionElementMap.axisContribution != axisContribution)
                    continue;
                
                if (axisRange != null && actionElementMap.axisRange != axisRange)
                    continue;
                
                return actionElementMap;
            }
        }

        return null;
    }

    public static ControllerType ControlsStateToControllerType(
        this Script_SettingsController.ControlsStates _controlsState
    ) => _controlsState switch
    {
        Script_SettingsController.ControlsStates.Joystick => ControllerType.Joystick,
        _ => ControllerType.Keyboard,
    };

    public static Script_SettingsController.ControlsStates ControllerTypeToControlsState(
        this ControllerType controllerType
    ) => controllerType switch
    {
        ControllerType.Joystick => Script_SettingsController.ControlsStates.Joystick,
        _ => Script_SettingsController.ControlsStates.Keyboard,
    };

    /// <summary>
    /// The Action Id found in the Rewired Editor Properties panel (NOT the index it is in the list
    /// </summary>
    public static int RWActionNamesToId(this string RWActionName) => RWActionName switch
    {
        Const_KeyCodes.RWInteract => 2,
        Const_KeyCodes.RWMaskCommand => 13,
        Const_KeyCodes.RWInventory => 3,
        Const_KeyCodes.RWSpeed => 4,
        Const_KeyCodes.RWMask1 => 5,
        Const_KeyCodes.RWMask2 => 6,
        Const_KeyCodes.RWMask3 => 7,
        Const_KeyCodes.RWMask4 => 8,
        Const_KeyCodes.RWUISubmit => 16,
        Const_KeyCodes.RWUICancel => 17,
        Const_KeyCodes.RWHorizontal => 0,
        Const_KeyCodes.RWVertical => 1,
        Const_KeyCodes.RWUnknownControllerSettings => 36,
        Const_KeyCodes.RWBackspace => 30,
        Const_KeyCodes.RWUIHorizontal => 14,
        Const_KeyCodes.RWUIVertical => 15,
        Const_KeyCodes.RWUnknownControllerMaskHz => 37,
        Const_KeyCodes.RWUnknownControllerMaskVert => 38,
        _ => 2,
    };

    public static string GetRWActionName(this RWActions RWAction) => RWAction switch
    {
        RWActions.MaskCommand => Const_KeyCodes.RWMaskCommand,
        RWActions.Inventory => Const_KeyCodes.RWInventory,
        RWActions.Speed => Const_KeyCodes.RWSpeed,
        RWActions.Mask1 => Const_KeyCodes.RWMask1,
        RWActions.Mask2 => Const_KeyCodes.RWMask2,
        RWActions.Mask3 => Const_KeyCodes.RWMask3,
        RWActions.Mask4 => Const_KeyCodes.RWMask4,
        RWActions.MoveHorizontalAxis => Const_KeyCodes.RWHorizontal,
        RWActions.MoveVerticalAxis => Const_KeyCodes.RWVertical,
        RWActions.Settings => Const_KeyCodes.RWUnknownControllerSettings,
        RWActions.MaskHorizontalAxis => Const_KeyCodes.RWUnknownControllerMaskHz,
        RWActions.MaskVerticalAxis => Const_KeyCodes.RWUnknownControllerMaskVert,
        _ => Const_KeyCodes.RWInteract,
    };

    public static bool IsLastControllerGamepad(Script_PlayerInputManager playerInputManager)
    {
        var lastController = playerInputManager.GetLastActiveController;
        return lastController != null && lastController.type == ControllerType.Joystick;
    }

    public static bool IsSteamDeck()
    {
        if (!SteamManager.Initialized)
            return false;
        
        return Steamworks.SteamUtils.IsSteamRunningOnSteamDeck();
    }
    
    // -------------------------------------------------------------------------------------
    // File Path Helpers
    
    /// <summary>
    /// Version number must be formatted like MAJOR.MINOR.PATCH
    /// </summary>
    
    public static string SaveFile(int slot) => $"{Prefix}_savedata_v{GetMajorVersion(Application.version)}_s{slot}.dat";
    public static string SaveTitleDataFile(int slot) => $"{Prefix}_titlesavedata_v{GetMajorVersion(Application.version)}_s{slot}.dat";
    public static string SettingsFile => $"{Prefix}_settings_v{GetMajorVersion(Application.version)}.dat";
    public static string CurseFile => $"{Prefix}_curse_v{GetMajorVersion(Application.version)}.dat";
    public static string LangPrefFile => $"{Prefix}_lang_v{GetMajorVersion(Application.version)}.dat";
    public static string Prefix => Const_Dev.IsDemo ? "nld" : "nl";

    // Will remove last "." and anything after it.
    public static string RemoveBuildNumber(string version)
    {
        int index = version.LastIndexOf(".");
        string removedLastNumberVersion = index >= 0 ? version.Substring(0, index) : version;

        return removedLastNumberVersion;
    }

    public static string GetMajorVersion(string version)
    {
        return version.Substring(0, 1);
    }
}
