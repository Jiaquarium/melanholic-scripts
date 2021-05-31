using System.Collections;
using System.Collections.Generic;

public class Model_Languages
{
    public string speaker { get; set;}
    public string[] EN { get; set;}
}

/// <summary>
/// Helper to populate Dialogue Nodes. Keep the dynamic format here.
/// Id: area_speaker_description_XXXX
/// </summary>
public static class Script_Dialogue
{
    
    public static Dictionary<string, Model_Languages> Dialogue = new Dictionary<string, Model_Languages>
    {
        {
            "hotel-lobby_player_nautical-dawn_0001", new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "According to my calculations,| today {77} should be at {49}.",
                }
            }
        }
    };
}
