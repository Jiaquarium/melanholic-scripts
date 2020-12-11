public static class Const_States_Player
{
    /// Game state: INTERACT
    public const string Interact = "interact";
    public const string Dialogue = "dialogue";
    public const string Attack = "attack";
    public const string Viewing = "viewing";
    public const string PickingUp = "picking-up";
    /// <summary>
    /// Player is completely disabled; good when we don't want to do full on game cut scene
    /// </summary>
    public const string Standby = "standby";

    /// Game state: INVENTORY
    public const string Inventory = "inventory";
}
