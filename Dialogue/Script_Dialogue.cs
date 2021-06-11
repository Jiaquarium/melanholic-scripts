using System.Collections;
using System.Collections.Generic;

public class Model_Languages
{
    public string speaker { get; set; }
    public string[] EN { get; set; }
    public Metadata[] metadata { get; set; }
    public string choiceText { get; set; }
    
    public class Metadata
    {
        public bool? isUnskippable;
        public bool? noContinuationIcon;
        public bool? waitForTimeline;
    }
}

public class Model_LanguagesUI
{
    public string EN { get; set; }
}

/// <summary>
/// Helper to populate Dialogue Nodes. Keep the dynamic format here.
/// Id: area_speaker_description_XXXX
/// </summary>
public static class Script_Dialogue
{
    public static Dictionary<string, Model_LanguagesUI> Text = new Dictionary<string, Model_LanguagesUI>
    {
        {
            "intro_narrator_hotel",
            new Model_LanguagesUI
            {
                EN = "I work at the seaside hotel about two hours away from my hometown."
            }
        },
        {
            "intro_narrator_hotel1",
            new Model_LanguagesUI
            {
                EN = "Fortunately, they actually let me stay in a room here if one's not being used."
            }
        }
    };
    
    public static Dictionary<string, Model_Languages> Dialogue = new Dictionary<string, Model_Languages>
    {
        // ------------------------------------------------------------------
        // Hotel
        {
            "hotel-lobby_player_nautical-dawn",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "According to my calculations,| today {77} should be at {49}.",
                }
            }
        },
        {
            "hotel-lobby_narrator_contract",
            new Model_Languages
            {
                speaker = "???",
                EN = new string[]{
                    "If you accept, sign your name next to the X.",
                }
            }
        },
        {
            "hotel-lobby_player_new-game-thought",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "So this is it, huh?",
                },
                metadata = new Model_Languages.Metadata[]
                {
                    new Model_Languages.Metadata { isUnskippable = true }
                }
            }
        },

        // Front Door
        {
            "hotel-lobby_player_front-door",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "I really would like to leave, but no I can’t.",
                }
            }
        },
        {
            "hotel-lobby_player_front-door1",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "I’m being watched.",
                }
            }
        },
        {
            "hotel-lobby_player_front-door2",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "I hate this feeling.",
                }
            }
        },
        
        // Coffee Maker
        {
            "hotel-lobby_player_coffee-maker",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "The day shift guy always forgets to rebrew the coffee when he gets off, so I always do it.",
                }
            }
        },

        // Family Portrait
        {
            "hotel-lobby_player_family-portrait",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "It’s an old photo with Dad and my little sis.",
                    "She’s working some job in finance now – honestly I don’t even really know what she does though."
                }
            }
        },

        // Coffee Table
        {
            "hotel-lobby_player_monday-recipes",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "A recipe book for French pastries. Bon appetit!",
                    "I’ve read this a million times by now, sigh."
                }
            }
        },
        {
            "hotel-lobby_player_monday-poems",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "<b>Tangled Hair</b>, a book of poetry.",
                    "I like poems and all, but if I had a choice, I’d rather be a novelist."
                }
            }
        },
        {
            "hotel-lobby_player_monday-textbook",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "It’s a textbook, <b>The Art of Fractal Geometry</b>.",
                    "Absolutely no clue why we have this here...| anyways, it’s interesting too."
                }
            }
        },
        
        {
            "hotel-lobby_player_tuesday-books",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "Someone must’ve been leafing through these...",
                }
            }
        },

        {
            "hotel-lobby_player_wednesday-book",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "Hm I don’t ever remember us having a book like this.",
                    "Wait a second, there’s something inside."
                }
            }
        },

        {
            "hotel-bay-v2_player_portrait-comment",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "This isn't the same portrait as in the Lobby's Elevator Bay.| This man has a face.",
                }
            }
        },
        {
            "hotel-bay-v2_player_portrait-comment_1",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "It looks like he's half-smiling, half-crying.",
                }
            }
        },


        // ------------------------------------------------------------------
        // Parlor (Dining)
        {
            "parlor_myne_mirror_tues",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "...",
                    "You have come to visit us on a very special day. Yes, @@Run is a special day, indeed it is.",
                    "Why can {19} never get along?| Look, the sisters down the {78} may need your assistance.",
                    "Siblings know more about each other than they think... You’ll see what I mean I’m sure of it.",
                }
            }
        },

        // ------------------------------------------------------------------
        // Grand Mirror Room
        {
            "grand-mirror-room_grand-mirror_responsibility",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "......",
                    "Because you are trusted, you will be given responsibility.",
                    "Do you accept it?",
                }
            }
        },
        {
            "grand-mirror-room_grand-mirror_responsibility_choice0",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "Very well then, my dear, heh heh.",
                },
                choiceText = "Yes"
            }
        },
        {
            "grand-mirror-room_grand-mirror_responsibility_choice1",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "Well that’s just too bad, my dear, heh heh.",
                    "Because this responsibility is yours whether you like it or not."
                },
                choiceText = "No"
            }
        },
        {
            "grand-mirror-room_grand-mirror_responsibility1",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "But also...",
                }
            }
        },
        {
            "grand-mirror-room_grand-mirror_responsibility2",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "Like I said you are trusted so you are given responsibility.",
                }
            }
        },

        // ------------------------------------------------------------------
        // Player
        {
            "any_player_first-psychic",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "Strange, I can't understand what they're saying.",
                }
            }
        },
    };
}
