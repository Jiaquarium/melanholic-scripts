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
        // ------------------------------------------------------------------
        // Intro
        {
            "intro_narrator_hotel",
            new Model_LanguagesUI
            {
                EN = "I work at the front desk of a seaside hotel| about a two hour drive from my hometown."
            }
        },
        {
            "intro_narrator_hotel1",
            new Model_LanguagesUI
            {
                EN = "It's fine though.| I can use one of the empty rooms if I don't want to make the trip home."
            }
        },
        {
            "intro_narrator_hotel2",
            new Model_LanguagesUI
            {
                EN = "There's always a few empty rooms,| since it's pretty slow this winter season."
            }
        },

        {
            "intro_narrator_hotel3",
            new Model_LanguagesUI
            {
                EN = "Actually now that I think about it..."
            }
        },
        {
            "intro_narrator_hotel4",
            new Model_LanguagesUI
            {
                EN = "I'm not quite sure how long it's been since I've been home."
            }
        },

        {
            "intro_narrator_hotel5",
            new Model_LanguagesUI
            {
                EN = "And oh..."
            }
        },
        {
            "intro_narrator_hotel6",
            new Model_LanguagesUI
            {
                EN = "I work the night shift."
            }
        },
        
        // ------------------------------------------------------------------
        // Good Ending
        {
            "good-ending_narrator_monologue",
            new Model_LanguagesUI
            {
                EN = "After that day, I never saw {37} ever again."
            }
        },
        {
            "good-ending_narrator_monologue1",
            new Model_LanguagesUI
            {
                EN = "I often still think back about what happened there."
            }
        },
        
        {
            "good-ending_narrator_monologue2",
            new Model_LanguagesUI
            {
                EN = "But with each passing moment..."
            }
        },
        {
            "good-ending_narrator_monologue3",
            new Model_LanguagesUI
            {
                EN = "The memory fades a bit more."
            }
        },

        {
            "good-ending_narrator_monologue4",
            new Model_LanguagesUI
            {
                EN = "I'd like to think my time inside there was worth it."
            }
        },
        
        // ------------------------------------------------------------------
        // True Ending
        {
            "true-ending_narrator_monologue",
            new Model_LanguagesUI
            {
                EN = "After that day, I quit my job at the seaside hotel."
            }
        },
        {
            "true-ending_narrator_monologue1",
            new Model_LanguagesUI
            {
                EN = "Needless to say, I never saw {37} ever again."
            }
        },
        
        {
            "true-ending_narrator_monologue2",
            new Model_LanguagesUI
            {
                EN = "It's been a few years now..."
            }
        },
        {
            "true-ending_narrator_monologue3",
            new Model_LanguagesUI
            {
                EN = "But I know I'll never forget what I saw there."
            }
        },
        
        {
            "true-ending_narrator_monologue4",
            new Model_LanguagesUI
            {
                EN = "I'm confident that now I can say..."
            }
        },

        {
            "true-ending_narrator_monologue5",
            new Model_LanguagesUI
            {
                EN = "We saved {18}."
            }
        },
    };
    
    public static Dictionary<string, Model_Languages> Dialogue = new Dictionary<string, Model_Languages>
    {
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

        {
            "sticker-reaction_player_disabled-melancholy-piano",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "The Piano chords echoed...| but nothing happened...",
                }
            }
        },

        // ------------------------------------------------------------------
        // Items
        {
            "item_sticker_last-elevator",
            new Model_Languages
            {
                speaker = "",
                EN = new string[]{
                    "<b>If you ever get lost</b>, the @@LastElevator sticker can be used anywhere inside {18} to take the {66} back to the {72}.",
                }
            }
        },
        
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
                    "You've come to visit me all the way down here?| How very nice of you.",
                    "It seems you are proving your worth, my dear.",
                    "And as a result, I'm beginning to trust you.|.|.| So I'm here to give you some greater responsibilities.",
                    "Do you accept?",
                },
                metadata = new Model_Languages.Metadata[]
                {
                    new Model_Languages.Metadata { isUnskippable = true },
                    new Model_Languages.Metadata { isUnskippable = true },
                    new Model_Languages.Metadata { isUnskippable = true },
                    new Model_Languages.Metadata { isUnskippable = true }
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
                    "Allow me to demonstrate.",
                },
                choiceText = "Yes",
                metadata = new Model_Languages.Metadata[]
                {
                    new Model_Languages.Metadata { isUnskippable = true },
                    new Model_Languages.Metadata { isUnskippable = true },
                }
            }
        },
        {
            "grand-mirror-room_grand-mirror_responsibility_choice1",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "Well that’s just too bad, my dear, heh heh.",
                    "Because it's the only way you can get out from down here...",
                    "Now allow me to demonstrate.",
                },
                choiceText = "No",
                metadata = new Model_Languages.Metadata[]
                {
                    new Model_Languages.Metadata { isUnskippable = true },
                    new Model_Languages.Metadata { isUnskippable = true },
                    new Model_Languages.Metadata { isUnskippable = true },
                }
            }
        },
        {
            "grand-mirror-room_grand-mirror_responsibility1",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "Also...| You'll be needing this for the rest of your journey| in case you ever get| <b>l|o|s|t|</b>.",
                },
                metadata = new Model_Languages.Metadata[]
                {
                    new Model_Languages.Metadata { isUnskippable = true },
                }
            }
        },
        {
            "grand-mirror-room_grand-mirror_responsibility2",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "Why am I being so nice to you?| Well it's simple, my dear.| You might be of some use to me.",
                    "Bye now.",
                },
                metadata = new Model_Languages.Metadata[]
                {
                    new Model_Languages.Metadata { isUnskippable = true },
                    new Model_Languages.Metadata { isUnskippable = true },
                }
            }
        },

        // ------------------------------------------------------------------
        // Celestial Gardens
        {
            "celestial-gardens_garden-labyrinth_puppets",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "Hand puppets...| This room actually reminds me a bit of a theatre stage.",
                }
            }
        },       
    };
}
