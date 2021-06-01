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
        // ================================================================================
        // Hotel
        {
            "hotel-lobby_player_nautical-dawn_0001", new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "According to my calculations,| today {77} should be at {49}.",
                }
            }
        },
        {
            "hotel-lobby_narrator_contract_0002", new Model_Languages
            {
                speaker = "???",
                EN = new string[]{
                    "If you accept, sign your name next to the X.",
                }
            }
        },
        {
            "hotel-bay-v2_player_portrait-comment_0001", new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "This isn't the same portrait as in the Lobby's Elevator Bay.| This man has a face.",
                }
            }
        },
        {
            "hotel-bay-v2_player_portrait-comment_0002", new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "It looks like he's half-smiling, half-crying.",
                }
            }
        }
    };
}
