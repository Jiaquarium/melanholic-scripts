using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Linq;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.Events;

public delegate void modifierCallback(Vector3Int tileLoc);

public static class Script_Utils
{
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

    public static void AnimatorSetDirection(Animator animator, Directions dir)
    {
        if (dir == Directions.Up)
        {
            animator.SetFloat("LastMoveX", 0f);
            animator.SetFloat("LastMoveZ", 1f);
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveZ", 1f);
        }
        else if (dir == Directions.Down)
        {
            animator.SetFloat("LastMoveX", 0f);
            animator.SetFloat("LastMoveZ", -1f);
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveZ", -1f);
        }
        else if (dir == Directions.Left)
        {
            animator.SetFloat("LastMoveX", -1f);
            animator.SetFloat("LastMoveZ", 0f);
            animator.SetFloat("MoveX", -1f);
            animator.SetFloat("MoveZ", 0f);
        }
        else if (dir == Directions.Right)
        {
            animator.SetFloat("LastMoveX", 1f);
            animator.SetFloat("LastMoveZ", 0f);
            animator.SetFloat("MoveX", 1f);
            animator.SetFloat("MoveZ", 0f);
        }
    }

    /// <summary>
    /// Params for item names defined in Script_ItemStringBuilder
    /// </summary>
    public static string FormatString(string unformattedString)
    {
        string itemFormattedStr = ReplaceParams(
            unformattedString,
            Script_ItemStringBuilder.Params
        );
        
        string itemAndDynamicFormattedStr = itemFormattedStr;
        try
        {
            Script_DynamicStringBuilder.BuildParams();
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
            Script_Names.Melz,                          // {1}
            Script_Names.MelzTheGreat,                  // {2}
            Script_Names.Ids,                           // {3}
            Script_Names.Ero,                           // {4}
            Script_Names.SBook,                         // {5}
            Script_Names.Urselk,                        // {6}
            Script_Names.Urselks,                       // {7}
            Script_Names.MaskedOne,                     // {8}
            Script_Names.Aenimals,                      // {9}
            Script_Names.Myne,                          // {10}
            Script_Names.Eileen,                        // {11}  
            Script_Names.Ellenia,                       // {12}
            Script_Names.Specter,                       // {13}
            Script_Names.ElleniaPassword,               // {14}
            Script_Names.Tedmunch,                      // {15}
            Script_Names.Tedwich,                       // {16}
            Script_Names.Tedward,                       // {17}
            Script_Names.Kelsingor,                     // {18}
            Script_Names.Specters,                      // {19}
            Script_Names.Aenimal,                       // {20}
            Script_Names.Sheepluff,                     // {21}
            Script_Names.Sealing,                       // {22}
            Script_Names.Action1, // SPACE              // {23}
            Script_Names.Action2, // X                  // {24}
            Script_Names.Action3, // LEFT SHIFT         // {25}
            Script_Names.InventoryKeyCode,              // {26}
            Script_Names.Escape,                        // {27}
            Script_Names.Skip,                          // {28}
            Script_Names.Tedmas,                        // {29}
            Script_Names.Sieve,                         // {30}
            Script_Names.Master,                        // {31}
            Script_Names.Inventory,                     // {32}
            Script_Names.Ursie,                         // {33}
            Script_Names.UrselkHouse,                   // {34}
            Script_Names.UrselksSaloon,                 // {35}
            Script_Names.UrselksBallroom,               // {36}
            Script_Names.KelsingorMansion,              // {37}
            Script_Names.Kaffe,                         // {38}
            Script_Names.Latte,                         // {39}
            Script_Names.MagicCircle,                   // {40}
            Script_Names.Menu,                          // {41}
            Script_Names.BadSpecters,                   // {42}
            Script_Names.BadSpecter,                    // {43}
            Script_Names.HouseMaster,                   // {44}
            Script_Names.Thoughts,                      // {45}
            Script_Names.HeartsCapacity,                // {46}
            Script_Names.Vx,                            // {47}
            Script_Names.Dan,                           // {48}
            Script_Names.NauticalDawn,                  // {49}
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
            Script_Names.ControllableSpecters           // {73}
        );
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
                Debug.Log("making font crispy: " + f);
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
    
        // handle if diagonal cases, if odd handle
        // set default directions (if perfectly diagonal, default to Right or Left)
        if (dir == CompassDir.NE)
        {
            if (direction.x * direction.x >= direction.z * direction.z) dir = CompassDir.E;
            else                                                        dir = CompassDir.N;
        }
        else if (dir == CompassDir.NW)
        {
            if (direction.x * direction.x >= direction.z * direction.z) dir = CompassDir.W;
            else                                                        dir = CompassDir.N;
        }
        else if (dir == CompassDir.SE)
        {
            if (direction.x * direction.x >= direction.z * direction.z) dir = CompassDir.E;
            else                                                        dir = CompassDir.S;
        }
        else if (dir == CompassDir.SW)
        {
            if (direction.x * direction.x >= direction.z * direction.z) dir = CompassDir.W;
            else                                                        dir = CompassDir.S;
        }
        
        return CompassToDirectionDict[dir];
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

        int sec = tRemainder;

        return $"{hours.ToString(fmt)}:{min.ToString(fmt)}:{sec.ToString(fmt)}";
    }

    public static string FormatTotalPlayTime(this float t)
    {
        return $"total play time: {t.FormatSecondsHHMMSS()}";
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
            ? $"『 {hours.ToString(fmt)}:{min.ToString(fmt)}:{sec.ToString(fmt)} a.m. 』"
            // : $"『 {hours.ToString(fmt)}:{min.ToString(fmt)}:{sec.ToString(fmt)} a.m. 』"; // testing
            : $"『 {hours.ToString(fmt)}:{min.ToString(fmt)} a.m. 』";
        
        return hideColons
            ? timeDisplay.Replace(":", " ")
            : timeDisplay;
    }

    public static string FormatDateTime(this DateTime date)
    {
        return date.ToString("MMMM dd, yyyy hh:mm tt").ToLower();
    }

    public static string FormatRun(this string run)
    {
        return $"{run.ToString()}";
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

    public static void PrintArray<T>(System.Collections.Generic.IEnumerable<T> list, string label = "array: ")
    {
        if (Debug.isDebugBuild)
            Debug.Log($"{label}: {String.Join("", new List<T>(list).ConvertAll(i => i.ToString()).ToArray())}");
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
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance
        ) != null;
    }
    
    public static T GetField<T>(this object obj, string propertyName) 
    {
        return (T)obj.GetType().GetField(
            propertyName,
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance
        ).GetValue(obj);
    }

    public static bool HasMethod(this object obj, string name)
    {
        Debug.Log($"Checking for method: {name}; got {obj.GetType().GetMethod(name, BindingFlags.NonPublic)}");
        return obj.GetType().GetMethod(
            name,
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance
        ) != null;
    }

    public static void InvokeMethod(this object obj, string name)
    {
        MethodInfo method = obj.GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        Debug.Log ($"invoking with MethodInfo: {method}");
        
        method.Invoke(obj, null);
    }

    public static Vector3Int ToVector3Int(this Vector3 vector3)
    {
        return new Vector3Int((int)vector3.x, (int)vector3.y, (int)vector3.z);
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
        return Script_GraphicsManager.GetFadeTime(fadeSpeed);
    }

    /// <summary>
    /// Checks if there are any events on the event handler
    /// counts as not null if has attached an object target;
    /// doesn't need to have to specify a function.
    /// </summary>
    public static bool CheckUnityEventAction(this UnityEvent onClickEvent)
    {
        for (int i = 0; i < onClickEvent.GetPersistentEventCount(); i++)
        {
            if (onClickEvent.GetPersistentTarget(i) != null)    return true;
        }

        return false;
    }

    /// <summary>
    /// Same as CheckUnityEventAction but allows to specify passing a param
    /// through the Event.
    /// </summary>
    public static bool CheckUnityEvent<T>(this UnityEvent<T> onClickEvent)
    {
        for (int i = 0; i < onClickEvent.GetPersistentEventCount(); i++)
        {
            if (onClickEvent.GetPersistentTarget(i) != null)    return true;
        }

        return false;
    }

    public static void DebugToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Debug.Log(output);
    }
}
