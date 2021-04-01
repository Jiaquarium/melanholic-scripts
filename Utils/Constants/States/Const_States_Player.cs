public static class Const_States_Player
{
    // ------------------------------------------------------------------
    // Game state: Interact
    public const string Interact        = "interact";
    public const string Dialogue        = "dialogue";
    public const string Attack          = "attack";
    public const string Viewing         = "viewing";
    public const string PickingUp       = "picking-up";
    // Player is completely disabled; good when we don't want to do full on game cut scene
    public const string Standby         = "standby";
    public const string Puppeteer       = "puppeteer";
    public const string PuppeteerNull   = "puppeteer-null";

    // ------------------------------------------------------------------
    // Game state: Inventory
    public const string Inventory       = "inventory";
}
