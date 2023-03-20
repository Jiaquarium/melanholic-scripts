using System.Collections.Generic;

[System.Serializable]
public class Script_ListOfLists<T>
{
    public List<T> myList;

    public T this[int key]
    {
        get => myList[key];
        set => myList[key] = value;
    }

    public int Count => myList.Count;
}