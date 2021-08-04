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

/// <summary>
/// Helper to populate Dialogue Nodes. Keep the dynamic format here.
/// Id: area_speaker_description_XXXX
/// </summary>
public static class Script_Dialogue
{
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
        // Narrator
        {
            "pianos_narrator_piano_remembered",
            new Model_Languages
            {
                speaker = "",
                EN = new string[]{
                    "The chords of the piano complement the beating of your heart.",
                    "This location is remembered."
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

        // Elevator Bay
        {
            "hotel-bay-v1_player_portrait-comment",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "A painting of the mogul who originally funded construction of the hotel. Not too much is known about him.",
                    "It’s a strange painting. No matter how I look at it I can’t ever make out his face.",
                }
            }
        },
        {
            "hotel-bay-v1_player_portrait-comment_1",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "Rumor is this wasn’t supposed to be a hotel.",
                    "Eh, I don’t like thinking about this kind of stuff during my shift...",
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
        // Dining
        {
            "dining_mynes-mirror_prompt",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "It’s a mirror. Should I look into it?",
                }
            }
        },

        // Myne's Mirror Interaction Nodes
        {
            "dining_mynes-mirror_interaction-node",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "Welcome to...",
                    "『 {37} 』",
                    "I’ll be your host today.",
                    "And for the rest of eternity.",
                    "That was supposed to be a joke.",
                    "Anyways, you see,| you and I, we share something.",
                },
                metadata = new Model_Languages.Metadata[]
                {
                    new Model_Languages.Metadata { isUnskippable = true },
                    new Model_Languages.Metadata { isUnskippable = true },
                    null,
                    null,
                    null,
                    new Model_Languages.Metadata { isUnskippable = true },
                }
            }
        },
        {
            "dining_mynes-mirror_interaction-node_choice-prompt",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "......",
                },
                metadata = new Model_Languages.Metadata[]
                {
                    new Model_Languages.Metadata { isUnskippable = true },
                }
            }
        },
        {
            "dining_mynes-mirror_interaction-node_choice0",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "Well, you see, if I told you that, it would ruin everything.",
                    "Besides, it’s better this way.",
                    "He-he-he.",
                },
                choiceText = "Who are you?"
            }
        },
        {
            "dining_mynes-mirror_interaction-node_choice1",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "Well there are a few things you can do for me...| That, in fact, only you can do for me.",
                    "And the problem is that it has to be <b>you<b>.",
                    "He-he-he.",
                },
                choiceText = "What do you want from me?"
            }
        },
        {
            "dining_mynes-mirror_interaction-node_end",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "The fact is...| I’ve been watching you for a long time now...",
                    "And your fate is the same as mine.",
                    "No need to rush into things| – all your questions will be answered in due time, my dear.",
                },
            }
        },

        {
            "dining_mynes-mirror_interaction-node_1",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "Welcome to...",
                    "『 {37} 』",
                    "I’ve already said that, haven’t I?",
                    "He-he-he...",
                    "You know, there’s a reason you can understand me even without that little @@PsychicDuck of yours.",
                    "You see, dear,| you and I,| we understand each other.",
                },
                metadata = new Model_Languages.Metadata[]
                {
                    new Model_Languages.Metadata { isUnskippable = true },
                    new Model_Languages.Metadata { isUnskippable = true },
                    null,
                    null,
                    null,
                    new Model_Languages.Metadata { isUnskippable = true },
                }
            }
        },
        {
            "dining_mynes-mirror_interaction-node_1_choice-prompt",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "Are you familiar with the Ouroboros?",
                },
                metadata = new Model_Languages.Metadata[]
                {
                    new Model_Languages.Metadata { isUnskippable = true },
                }
            }
        },
        {
            "dining_mynes-mirror_interaction-node_1_choice0",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "Precisely, my dear.",
                },
                choiceText = "Yes, the snake eating its own tail?"
            }
        },
        {
            "dining_mynes-mirror_interaction-node_1_choice1",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "Well, this is no story, my dear.",
                },
                choiceText = "No, and I’m not interested in your stories."
            }
        },
        {
            "dining_mynes-mirror_interaction-node_1_end",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "What would make a snake begin to eat its own tail?",
                    "Anywho...",
                },
            }
        },
        
        {
            "dining_mynes-mirror_mon",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "...",
                    "You have come to visit us on a very special day. Yes, @@Run is a special day, indeed it is.",
                    "For today, dear, just find the {66}.",
                }
            }
        },
        {
            "dining_mynes-mirror_tues",
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
        {
            "dining_mynes-mirror_wed",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "...",
                    "You have come to visit us on a very special day. Yes, @@Run is a special day, indeed it is.",
                    "The sheep are at home today.",
                }
            }
        },
        {
            "dining_mynes-mirror_other",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "...",
                    "You have come to visit us on a very special day. Yes, @@Run is a special day, indeed it is.",
                }
            }
        },

        {
            "dining_mynes-mirror_default-end",
            new Model_Languages
            {
                speaker = "{10}",
                EN = new string[]{
                    "You see, you’ve fallen into a terrible fate.",
                    "Along with all of us here in {37}.",
                    "But I’ll help you along the way.| Just promise me one thing...",
                    "That you’ll never try to see...",
                    "What’s under...",
                    "This <b>mask</b>.",
                }
            }
        },

        {
            "dining_go-table_default",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    "Only thing I hate about games is losing.",
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

        // ------------------------------------------------------------------
        // Item Objects
        {
            "item-object_sticker_boar-needle",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    @"The @@BoarNeedle @@Sticker_NoBold allows you to enter paintings that have a doormat. Use the @@BoarNeedle's {67} when face-to-face with a painting with a doormat."
                }
            }
        },
        {
            "item-object_sticker_animal-within",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    @"If there are edible obstacles in your way, use the @@AnimalWithin @@Sticker_NoBold to eat through them."
                }
            }
        },
        {
            "item-object_sticker_ice-spike",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    @"The @@IceSpike is a spike so powerful, it'll crack open just about anything."
                }
            }
        },
        {
            "item-object_sticker_melancholy-piano",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    @"Use the @@MelancholyPiano @@Sticker_NoBold to follow the chords of your heart to the next piano."
                }
            }
        },
        {
            "item-object_sticker_last-elevator",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    @"<b>If you ever get lost</b>, the @@LastElevator @@Sticker_NoBold can be used anywhere inside {18} to take the {66} back to the {72}."
                }
            }
        },
        {
            "item-object_sticker_let-there-be-light",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    @"@@LetThereBeLight @@Sticker_NoBold will illuminate certain dark areas."
                }
            }
        },
        {
            "item-object_sticker_puppeteer",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{
                    @"Use the @@Puppeteer @@Sticker_NoBold to control {73}. Not too shabby."
                }
            }
        },
        
        // ------------------------------------------------------------------
        // Choices
        {
            "choices_default_yes",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{},
                choiceText = "Yes",
            }
        },
        {
            "choices_default_no",
            new Model_Languages
            {
                speaker = "{0}",
                EN = new string[]{},
                choiceText = "No",
            }
        },
    };
}
