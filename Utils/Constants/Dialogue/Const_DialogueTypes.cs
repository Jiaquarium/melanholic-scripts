public static class Const_DialogueTypes
{
    public class Type
    {
        public static readonly string Read = "read";
        public static readonly string Item = "item";
        // Item type that doesn't go through pick up flow, but still uses Item dialogue canvases.
        public static readonly string ItemNoPickUp = "item-no-pick-up";
        public static readonly string PaintingEntrance = "painting-entrance";
    }

    public class Location
    {
        public static readonly string Bottom = "bottom";
        public static readonly string Top = "top";
    }
}
