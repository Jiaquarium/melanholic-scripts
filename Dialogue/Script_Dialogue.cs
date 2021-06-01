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
            "hotel-lobby_player_nautical-dawn", new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "According to my calculations,| today {77} should be at {49}.",
                }
            }
        },
        {
            "hotel-lobby_narrator_contract", new Model_Languages
            {
                speaker = "???",
                EN = new string[]{
                    "If you accept, sign your name next to the X.",
                }
            }
        },
        {
            "hotel-bay-v2_player_portrait-comment", new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "This isn't the same portrait as in the Lobby's Elevator Bay.| This man has a face.",
                }
            }
        },
        {
            "hotel-bay-v2_player_portrait-comment_1", new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "It looks like he's half-smiling, half-crying.",
                }
            }
        },
        {
            "parlor_myne_mirror_tues", new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "...",
                    "You have come to visit us on a very special day. Yes, @@Run is a special day, indeed it is.",
                    "Why can {19} never get along?| Look, the sisters down the {78} may need your assistance.",
                    "Siblings know more about each other than they think... You’ll see what I mean I’m sure of it.",
                }
            }
        }
    };
}
