public static class Const_States_Player
{
    /// Game state: INTERACT
    public static readonly string Interact = "interact";
    public static readonly string Dialogue = "dialogue";
    public static readonly string Attack = "attack";
    public static readonly string Viewing = "viewing";
    public static readonly string PickingUp = "picking-up";
    /// <summary>
    /// Player is completely disabled; good when we don't want to do full on game cut scene
    /// </summary>
    public static readonly string Standby = "standby";

    /// Game state: INVENTORY
    public static readonly string Inventory = "inventory";
}
