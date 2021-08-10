// Last created by Dialogue Exporter at 2021-08-09 22:27:09

using System.Collections;
using System.Collections.Generic;

// https://docs.google.com/spreadsheets/d/12PJr55wEMnZhO3n-c00xunSQxyDqnkutiAOx4BLh0tQ/edit#gid=0

public class Model_Languages
{
    public string speaker { get; set; }
    public string[] EN { get; set; }
    public Metadata[] metadata { get; set; }
    public string choiceText { get; set; }
    
    // If Metadata is not defined, it will default to what is in the Editor;
    // otherwise it will overwrite with what is present.
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
        EN = new string[]
        {
                "Strange, I can't understand what they're saying.",
        },
        
    }
},
{
    "sticker-reaction_player_disabled-melancholy-piano",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "The Piano chords echoed...| but nothing happened...",
        },
        
    }
},
// ------------------------------------------------------------------
// Narrator
{
    "pianos_narrator_piano_remembered",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "The chords of the piano complement the beating of your heart.",
                "This location is remembered.",
        },
        
    }
},
// ------------------------------------------------------------------
// Ice Block
{
    "iceblocks_default_description",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "Cold.",
                "It's ice...",
        },
        
    }
},
// ------------------------------------------------------------------
// Hotel
{
    "hotel-lobby_player_nautical-dawn",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "According to my calculations,| today {77} should be at {49}.",
        },
        
    }
},
{
    "hotel-lobby_narrator_contract",
    new Model_Languages
    {
        speaker = "???",
        EN = new string[]
        {
                "If you accept, sign your name next to the X.",
        },
        
    }
},
{
    "hotel-lobby_player_new-game-thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "So this is it, huh?",
        },
        
    }
},
{
    "hotel-lobby_player_front-door",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "I really would like to leave, but no I can’t.",
        },
        
    }
},
{
    "hotel-lobby_player_front-door1",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "I’m being watched.",
        },
        
    }
},
{
    "hotel-lobby_player_front-door2",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "I hate this feeling.",
        },
        
    }
},
{
    "hotel-lobby_player_coffee-maker",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "The day shift guy always forgets to rebrew the coffee when he gets off, so I always do it.",
        },
        
    }
},
{
    "hotel-lobby_player_family-portrait",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "It’s an old photo with Dad and my little sis.",
                "She’s working some job in finance now – honestly I don’t even really know what she does though.",
        },
        
    }
},
{
    "hotel-lobby_player_monday-recipes",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "A recipe book for French pastries. Bon appetit!",
                "I’ve read this a million times by now, sigh.",
        },
        
    }
},
{
    "hotel-lobby_player_monday-poems",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "<b>Tangled Hair</b>, a book of poetry.",
                "I like poems and all, but if I had a choice, I’d rather be a novelist.",
        },
        
    }
},
{
    "hotel-lobby_player_monday-textbook",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "It’s a textbook, <b>The Art of Fractal Geometry</b>.",
                "Absolutely no clue why we have this here...| anyways, it’s interesting too.",
        },
        
    }
},
{
    "hotel-lobby_player_tuesday-books",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "Someone must’ve been leafing through these...",
        },
        
    }
},
{
    "hotel-lobby_player_wednesday-book",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "Hm I don’t ever remember us having a book like this.",
                "Wait a second, there’s something inside.",
        },
        
    }
},
{
    "hotel-bay-v1_player_portrait-comment",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "A painting of the mogul who originally funded construction of the hotel. Not too much is known about him.",
                "It’s a strange painting. No matter how I look at it I can’t ever make out his face.",
        },
        
    }
},
{
    "hotel-bay-v1_player_portrait-comment_1",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "Rumor is this wasn’t supposed to be a hotel.",
                "Eh, I don’t like thinking about this kind of stuff during my shift...",
        },
        
    }
},
{
    "hotel-bay-v2_player_portrait-comment",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "This isn't the same portrait as in the Lobby's Elevator Bay.| This man has a face.",
        },
        
    }
},
{
    "hotel-bay-v2_player_portrait-comment_1",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "It looks like he's half-smiling, half-crying.",
        },
        
    }
},
// ------------------------------------------------------------------
// Dining
{
    "dining_mynes-mirror_prompt",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "It’s a mirror. Should I look into it?",
        },
        
    }
},
// ------------------------------------------------------------------
// Myne's Mirror Interaction Nodes
{
    "dining_mynes-mirror_interaction-node",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "Welcome to...",
                "『 {37} 』",
                "I’ll be your host today.",
                "And for the rest of eternity.",
                "That was supposed to be a joke.",
                "Anyways, you see,| you and I, we share something.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                null,
                null,
                null,
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "dining_mynes-mirror_interaction-node_choice-prompt",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "......",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "dining_mynes-mirror_interaction-node_choice0",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "Well, you see, if I told you that, it would ruin everything.",
                "Besides, it’s better this way.",
                "He-he-he.",
        },
        choiceText = "Who are you?",
        
    }
},
{
    "dining_mynes-mirror_interaction-node_choice1",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "Well there are a few things you can do for me...| That, in fact, only you can do for me.",
                "And the problem is that it has to be <b>you<b>.",
                "He-he-he.",
        },
        choiceText = "What do you want from me?",
        
    }
},
{
    "dining_mynes-mirror_interaction-node_end",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
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
        EN = new string[]
        {
                "Welcome to...",
                "『 {37} 』",
                "I’ve already said that, haven’t I?",
                "He-he-he...",
                "You know, there’s a reason you can understand me even without that little @@PsychicDuck of yours.",
                "You see, dear,| you and I,| we understand each other.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                null,
                null,
                null,
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "dining_mynes-mirror_interaction-node_1_choice-prompt",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "Are you familiar with the Ouroboros?",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "dining_mynes-mirror_interaction-node_1_choice0",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "Precisely, my dear.",
        },
        choiceText = "Yes, the snake eating its own tail?",
        
    }
},
{
    "dining_mynes-mirror_interaction-node_1_choice1",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "Well, this is no story, my dear.",
        },
        choiceText = "No, and I’m not interested in your stories.",
        
    }
},
{
    "dining_mynes-mirror_interaction-node_1_end",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "What would make a snake begin to eat its own tail?",
                "Anywho...",
        },
        
    }
},
// ------------------------------------------------------------------
// Myne's Mirror Hint Nodes
{
    "dining_mynes-mirror_mon",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "...",
                "You have come to visit us on a very special day. Yes, @@Run is a special day, indeed it is.",
                "For today, dear, just find the {66}.",
        },
        
    }
},
{
    "dining_mynes-mirror_tues",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "...",
                "You have come to visit us on a very special day. Yes, @@Run is a special day, indeed it is.",
                "Why can {19} never get along?| Look, the sisters down the {78} may need your assistance.",
                "Siblings know more about each other than they think... You’ll see what I mean I’m sure of it.",
        },
        
    }
},
{
    "dining_mynes-mirror_wed",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "...",
                "You have come to visit us on a very special day. Yes, @@Run is a special day, indeed it is.",
                "The sheep are at home today.",
        },
        
    }
},
{
    "dining_mynes-mirror_other",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "...",
                "You have come to visit us on a very special day. Yes, @@Run is a special day, indeed it is.",
        },
        
    }
},
// ------------------------------------------------------------------
// Myne's Mirror End Default
{
    "dining_mynes-mirror_default-end",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "You see, you’ve fallen into a terrible fate.",
                "Along with all of us here in {37}.",
                "But I’ll help you along the way.| Just promise me one thing...",
                "That you’ll never try to see...",
                "What’s under...",
                "This <b>mask</b>.",
        },
        
    }
},
{
    "dining_go-table_default",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "Only thing I hate about games is losing.",
        },
        
    }
},
// ------------------------------------------------------------------
// Piano Room
{
    "piano-room_player_giant-painting",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "It’s a painting... close up it's a lot less flat than I thought. Whoever painted it used a lot of paint.",
        },
        
    }
},
{
    "piano-room_player_wall-text",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "Anonymous.| <i>Found My Other Half</i>.| 19XX.| Oil on linen.",
        },
        
    }
},
// ------------------------------------------------------------------
// Ero
{
    "piano-room_ero_default",
    new Model_Languages
    {
        speaker = "{4}",
        EN = new string[]
        {
                "����.",
        },
        
    }
},
{
    "piano-room_ero_psychic",
    new Model_Languages
    {
        speaker = "{4}",
        EN = new string[]
        {
                "Grrrrrr......",
        },
        
    }
},
{
    "piano-room_ero_psychic_a",
    new Model_Languages
    {
        speaker = "{4}",
        EN = new string[]
        {
                "My duty is to guard, so guard is what I shall do.",
                "If you know your role then play it with honor!",
                "Woof!",
        },
        
    }
},
// ------------------------------------------------------------------
// Ero Weekend
{
    "piano-room_ero-weekend_psychic_ids-lost",
    new Model_Languages
    {
        speaker = "{4}",
        EN = new string[]
        {
                "*wimper*",
                "I’ve failed my duty.",
                "It truly is fate.",
        },
        
    }
},
{
    "piano-room_ero-weekend_psychic1_ids-lost",
    new Model_Languages
    {
        speaker = "{4}",
        EN = new string[]
        {
                "I’ve lost {3}.",
                "It’s my job, my role, but I’ve failed. I am terribly sorry.",
        },
        
    }
},
{
    "piano-room_ero_eat-reaction",
    new Model_Languages
    {
        speaker = "{4}",
        EN = new string[]
        {
                "(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// Nightclub Line
{
    "nightclub-line_sign_read",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "Estimated wait from this point: 0 minutes.",
        },
        
    }
},
// ------------------------------------------------------------------
// Ids' Room - Ids
{
    "basement_ids_default",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                "�  �  �  �.",
        },
        
    }
},
{
    "basement_ids_n-room_talked-with-myne",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                "{0}?",
                "We’ve already met actually. Back when you couldn’t even speak to us.",
                "Well I’m...",
        },
        
    }
},
{
    "basement_ids_n-room_on-nameplate-done_talked-with-myne",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                "Pretty cool entrance, am I right? Spent a while coming up with that.",
        },
        
    }
},
{
    "basement_ids_n-room_on-nameplate-done_choices_talked-with-myne",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                "Wait sorry got a little caught up with myself, did you have something to say?",
        },
        
    }
},
{
    "basement_ids_n-room_on-nameplate-done_choices_talked-with-myne_a",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                "You’re not serious, are you?",
                "No one’s met the {8}.",
                "It’s not real,| it’s just a story,| something you tell little kids.",
                "Please, you’re losing touch with reality.",
                "Here, let’s do something to take your mind off things.",
        },
        choiceText = "The {8}, I met him.",
        
    }
},
{
    "basement_ids_n-room_on-nameplate-done_choices_talked-with-myne_b",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                "Oh it looked like you wanted to say something... nevermind.",
                "Here, let’s do something fun.",
        },
        choiceText = "(Don’t mention anything.)",
        
    }
},
{
    "basement_ids_n-room_not-talked-with-myne",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                "{0}?",
                "We’ve already met actually. Back when you couldn’t even speak to us.",
                "Well I’m...",
        },
        
    }
},
{
    "basement_ids_n-room_on-nameplate-done_not-talked-with-myne",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                "Pretty cool entrance, am I right? Spent a while coming up with that.",
        },
        
    }
},
{
    "basement_ids_e-room_dance-intro",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                "Oh heyyy, {0}. Nice of you to join me, I was getting pretty lonely.",
                "What are we about to do you ask!?| Well how about I show you {0}!",
        },
        
    }
},
{
    "basement_ids_e-room_player-dance-intro",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                "You know why us {21} love dancing in the dark?",
                "There’s just something so romantic about staring into complete blackness, while feeling our warm wool rubbing against one another...",
                "Sorry I get a little mushy sometimes...| Okay your turn now!",
        },
        
    }
},
{
    "basement_ids_e-room_ddr-fail",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                "Wow...| you’re kinda terrible.| It’s okay though, you’re new here I can tell.",
                "Anyways, {0}, that was quite fun. We should do it again another time.",
        },
        
    }
},
{
    "basement_ids_e-room_ddr-pass",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                "Wow you can really <b><i>move</i></b>.",
                "It reminds me of...",
                "*black beady eyes begin welling up with what appears to be tears*",
                "I gotta admit, I can trust whoever got moves. So here.",
        },
        
    }
},
{
    "basement_ids_try-again-prompt",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                "Wanna try again?",
        },
        
    }
},
{
    "basement_ids_try-again-prompt_a",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                "Yay. Let’s go go go, move that body!",
        },
        choiceText = "Yes",
        
    }
},
{
    "basement_ids_try-again-prompt_b",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                "Why must you tease me, hmph.",
        },
        choiceText = "No",
        
    }
},
// ------------------------------------------------------------------
// Ids' Room - Decor
{
    "basement_w-room_certificates_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "Two certificates of some sort... I wonder why one is hung higher than the other...",
        },
        
    }
},
{
    "basement_e-room_painting",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "The figures in the painting are dancing. Looks fun.",
        },
        
    }
},
{
    "basement_n-room_wall-text_read",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "Anonymous. <i>You and I Forever</i>. 19XX. Oil on Linen.",
        },
        
    }
},
{
    "basement_n-room_desk_read",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "...the answer to everything...",
                "...a fresh way to approach a dull thing...",
                "...a way of doing things...",
                "...S&M style...",
        },
        
    }
},
// ------------------------------------------------------------------
// Ursie
{
    "ballroom_ursie_default",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {
                "� � �...",
        },
        
    }
},
{
    "ballroom_ursie_psychic_intro",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {
                "The name’s {33}.",
                "You look like you need a drink.| I can tell from all these years running the {35}.",
        },
        
    }
},
{
    "ballroom_ursie_psychic",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {
                "To be frank,| some disturbing things have been happening at my <b>saloon</b>, so I’m here to speak to the <b>King</b> about it.",
        },
        
    }
},
{
    "ballroom_ursie_psychic1",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {
                "Hopefully the <b>King</b> takes action...| and fast.",
                "I got a business to run.| I’m <b>bleeding</b> sales as we speak, don’t you understand?",
        },
        
    }
},
{
    "ballroom_ursie_eat-reaction",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {
                "(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// Moose
{
    "ballroom_moose_default",
    new Model_Languages
    {
        speaker = "{63}",
        EN = new string[]
        {
                "�.",
        },
        
    }
},
{
    "ballroom_moose_psychic",
    new Model_Languages
    {
        speaker = "{63}",
        EN = new string[]
        {
                "There’s no use. The {22}. It’s been decided.",
                "I specialize in dealing with spells. No outsiders means no more new spells for me.",
                "I’ll have to focus on the spells I know then.",
        },
        
    }
},
{
    "ballroom_moose_eat-reaction",
    new Model_Languages
    {
        speaker = "{63}",
        EN = new string[]
        {
                "(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// Suzette
{
    "ballroom_suzette_default",
    new Model_Languages
    {
        speaker = "{58}",
        EN = new string[]
        {
                "���� ���...",
        },
        
    }
},
{
    "ballroom_suzette_psychic_intro",
    new Model_Languages
    {
        speaker = "{58}",
        EN = new string[]
        {
                "There all just a bunch of {60}.",
        },
        
    }
},
{
    "ballroom_suzette_psychic_intro1",
    new Model_Languages
    {
        speaker = "{58}",
        EN = new string[]
        {
                "Oh I’m {58}.",
        },
        
    }
},
{
    "ballroom_suzette_psychic",
    new Model_Languages
    {
        speaker = "{58}",
        EN = new string[]
        {
                "Well the only thing anyone ever talks about these days is the {22}.| Me?| What do I think about it?",
                "Why would I care if anyone else is able to come into {18}?",
        },
        
    }
},
{
    "ballroom_suzette_psychic1",
    new Model_Languages
    {
        speaker = "{58}",
        EN = new string[]
        {
                "Everyone in this world is just a {59} anyways.",
                "Why would I want more {60} in here?",
        },
        
    }
},
{
    "ballroom_suzette_eat-reaction",
    new Model_Languages
    {
        speaker = "{58}",
        EN = new string[]
        {
                "(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// Peche & Melba
{
    "ballroom_peche-melba_default",
    new Model_Languages
    {
        speaker = "{61}",
        EN = new string[]
        {
                "����!!",
        },
        
    }
},
{
    "ballroom_peche-melba_default_a",
    new Model_Languages
    {
        speaker = "{62}",
        EN = new string[]
        {
                "���� ��!",
        },
        
    }
},
{
    "ballroom_peche-melba_psychic_intro",
    new Model_Languages
    {
        speaker = "{61}",
        EN = new string[]
        {
                "Ha! Good riddance,| to tell you the truth I’m glad we’re going through with the {22}.| Isn’t that right, {62}?",
        },
        
    }
},
{
    "ballroom_peche-melba_psychic_intro_a",
    new Model_Languages
    {
        speaker = "{62}",
        EN = new string[]
        {
                "Yeah, you tell’em, {61}!",
        },
        
    }
},
{
    "ballroom_peche-melba_psychic",
    new Model_Languages
    {
        speaker = "{61}",
        EN = new string[]
        {
                "We opened our doors and now look at us. You give a wolf your foot and it’ll come back in the winter for your hand!",
        },
        
    }
},
{
    "ballroom_peche-melba_psychic_a",
    new Model_Languages
    {
        speaker = "{62}",
        EN = new string[]
        {
                "We’ll do just fine by ourselves in here. It’s how our ancestors lived, ain’t it?",
        },
        
    }
},
{
    "ballroom_peche-melba_psychic1",
    new Model_Languages
    {
        speaker = "{61}",
        EN = new string[]
        {
                "We here are {37} natives. Born and raised!| How can you call yourself an {6} when you can’t even toughen out through the {22}?!",
        },
        
    }
},
{
    "ballroom_peche-melba_psychic1_a",
    new Model_Languages
    {
        speaker = "{62}",
        EN = new string[]
        {
                "Right! A true {6} never complains.| Head down and never frown!",
        },
        
    }
},
{
    "ballroom_peche-melba_eat-reaction",
    new Model_Languages
    {
        speaker = "{61} & {62}",
        EN = new string[]
        {
                "(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// King Eclaire
{
    "ballroom_king-eclaire_default",
    new Model_Languages
    {
        speaker = "{57}",
        EN = new string[]
        {
                "���� ��� �������...",
        },
        
    }
},
{
    "ballroom_king-eclaire_psychic_intro",
    new Model_Languages
    {
        speaker = "{57}",
        EN = new string[]
        {
                "As you may have already heard, I am {57}.",
        },
        
    }
},
{
    "ballroom_king-eclaire_psychic",
    new Model_Languages
    {
        speaker = "{57}",
        EN = new string[]
        {
                "It’s no secret we’re in a dire situation right now. I must take action.",
                "Intruders have invaded my {37}.",
        },
        
    }
},
{
    "ballroom_king-eclaire_psychic1",
    new Model_Languages
    {
        speaker = "{57}",
        EN = new string[]
        {
                "As the King of {18}, it is my sworn duty to serve and protect its inhabitants.| So after careful thought I’ve come to the difficult decision –",
        },
        
    }
},
{
    "ballroom_king-eclaire_psychic2",
    new Model_Languages
    {
        speaker = "{57}",
        EN = new string[]
        {
                "<b>To commence the</b> {22} <b>at</b> {49}",
        },
        
    }
},
{
    "ballroom_king-eclaire_psychic3",
    new Model_Languages
    {
        speaker = "{57}",
        EN = new string[]
        {
                "It’s been a tough decision and comes with its pitfalls, but after weighing all the viable options, it is the logical path we must take. to protect my fellow {19}.",
        },
        
    }
},
{
    "ballroom_king-eclaire_eat-reaction",
    new Model_Languages
    {
        speaker = "{57}",
        EN = new string[]
        {
                "(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// King Eclaire - King's Intro Cut Scene
{
    "ballroom_cut-scene_kings-intro_cursed-effects",
    new Model_Languages
    {
        speaker = "{57}",
        EN = new string[]
        {
                "I am {57}, master and servant of {37}.",
                "You have come to us while we are in a dire situation. <b>I am prepared to take action.</b>",
                "Intruders have invaded my {37}. If you could only see what I have seen...",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "ballroom_cut-scene_kings-intro_sealing-explanation",
    new Model_Languages
    {
        speaker = "{57}",
        EN = new string[]
        {
                "As the King of {18}, it is my sworn duty to serve and protect its inhabitants. so after careful thought I’ve come to the difficult decision –",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "ballroom_cut-scene_kings-intro_lock",
    new Model_Languages
    {
        speaker = "{57}",
        EN = new string[]
        {
                "<b>To commence the</b> {22} <b>at</b> {49}",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "ballroom_cut-scene_kings-intro_dawn",
    new Model_Languages
    {
        speaker = "{57}",
        EN = new string[]
        {
                "At exactly {49} the line between sea and sky is drawn in the night.| Astronomers call it <b>Nautical Dawn</b>.",
                "But at that time we will draw another line!| One that marks the changing of our fate...",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "ballroom_cut-scene_kings-intro_sealed",
    new Model_Languages
    {
        speaker = "{57}",
        EN = new string[]
        {
                "<b>We will lock our doors for eternity and drive out these wicked forces from my dear</b> {18}.",
                "Mark my word.| I will return {18} back to a time of peace with my plan.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
// ------------------------------------------------------------------
// Kaffe
{
    "ballroom_kaffe_default",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                "����.",
        },
        
    }
},
{
    "ballroom_kaffe_psychic",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                "This is not the answer, for God’s sake! We need to get to the root of the problem.",
                "Find those damn intruders. Figure out where they’re coming from and attack them first! Not lock ourselves away!",
                "My whole family is outside of {18}. What do they expect me to do?!",
                "Of course I want to stay here with {39}, but the closer it gets to {49}..",
        },
        
    }
},
{
    "ballroom_kaffe_psychic1",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                "I was born outside the {37} walls, I don’t think I can stay here forever like the true {7} will.",
                "But I know I won’t be able to find anyone else like {39}.",
        },
        
    }
},
{
    "ballroom_kaffe_eat-reaction",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                "(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// Latte
{
    "ballroom_latte_default",
    new Model_Languages
    {
        speaker = "{39}",
        EN = new string[]
        {
                "����.",
        },
        
    }
},
{
    "ballroom_latte_psychic",
    new Model_Languages
    {
        speaker = "{39}",
        EN = new string[]
        {
                "Yeah {38} gets really worked up sometimes. Oh of course, I don’t agree with the King’s decision either, but it’s reality, it’s going to happen. {49} on the dot.",
                "If he ends up leaving before the {22} to return to his family, I’d understand. It might be a sign rather. The way I look at it, it’s fate.",
        },
        
    }
},
{
    "ballroom_latte_psychic1",
    new Model_Languages
    {
        speaker = "{39}",
        EN = new string[]
        {
                "Oh but please don’t tell him I said any of this. Like I said, he gets really worked up over nothing.",
        },
        
    }
},
{
    "ballroom_latte_eat-reaction",
    new Model_Languages
    {
        speaker = "{39}",
        EN = new string[]
        {
                "(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// Ero
{
    "ballroom_ero_default",
    new Model_Languages
    {
        speaker = "{4}",
        EN = new string[]
        {
                "����...",
        },
        
    }
},
{
    "ballroom_ero_psychic",
    new Model_Languages
    {
        speaker = "{4}",
        EN = new string[]
        {
                "......",
                "It's useless to fight against fate.",
        },
        
    }
},
{
    "ballroom_ero_psychic1",
    new Model_Languages
    {
        speaker = "{4}",
        EN = new string[]
        {
                "What could I have done?",
        },
        
    }
},
{
    "ballroom_ero_eat-reaction",
    new Model_Languages
    {
        speaker = "{4}",
        EN = new string[]
        {
                "(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// World Paintings - Blank
{
    "ballroom_world-paintings_blank",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "Blank canvases...| I took a portraiture painting class once. Bad memories.",
                "The hardest part is getting started, really.| That first mark.",
        },
        
    }
},
{
    "ballroom_world-paintings_blank1",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "But after a few strokes, things got pretty <b>exciting</b>.",
                "From there you can really go anywhere.",
                "There was a certain kind of thrill building up this thing and knowing I could destroy it all at once.",
        },
        
    }
},
{
    "ballroom_world-paintings_blank2",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "And even if I royally mess up, I can just paint over it.",
                "And over and over and over and over.",
                "Then the hard part becomes knowing when to stop!",
        },
        
    }
},
// ------------------------------------------------------------------
// Misc
{
    "ballroom_paintings_cat-jumping",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "A day in the life of a cat. I’ve always wondered what life would be like as a cat.",
        },
        
    }
},
// ------------------------------------------------------------------
// Urselks Hall
{
    "urselks-hall_flan_default",
    new Model_Languages
    {
        speaker = "{64}",
        EN = new string[]
        {
                "����.",
        },
        
    }
},
{
    "urselks-hall_flan_psychic_other",
    new Model_Languages
    {
        speaker = "{64}",
        EN = new string[]
        {
                "{64} here at your humble service.",
                "Oh, you are able to understand me?",
                "My mistake. go right along ahead, young one, I am here to serve.",
        },
        
    }
},
{
    "urselks-hall_flan_psychic_wed",
    new Model_Languages
    {
        speaker = "{64}",
        EN = new string[]
        {
                "I’m terribly sorry to inform you that {52} is the sisters’ rest day.",
                "Come back another time, I am here to serve.",
        },
        
    }
},
{
    "urselks-hall_flan_eat-reaction",
    new Model_Languages
    {
        speaker = "{64}",
        EN = new string[]
        {
                "(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// Eileen's Room
{
    "eileens-room_painting_cat-jumping",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "A cat jumping onto a bed!",
                "Wait why am I getting so excited?",
                "It’s too high for me to get a really good look at it...",
        },
        
    }
},
{
    "eileens-room_eileen_default",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                "�.",
        },
        
    }
},
{
    "eileens-room_eileen_psychic",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                "Not in the mood.",
                "Don’t you dare talk to me again...",
        },
        
    }
},
{
    "eileens-room_eileen_psychic1",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                "Why do you keep trying to talk?",
                "Told you I’m really not in the mood.",
        },
        
    }
},
{
    "eileens-room_eileen_psychic2",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                "Sorry about the spikes.",
                "I really really hate this place, I really do.",
        },
        
    }
},
{
    "eileens-room_eileen_psychic2_a",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                "Hey no one asked for your opinion!",
        },
        choiceText = "Why don’t you get moving and just leave already?",
        
    }
},
{
    "eileens-room_eileen_psychic2_b",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                "Can’t help it, they’ve always been there.",
                "Up, down, up, and down they go. All day and all night long.",
                "I really really hate it.",
        },
        choiceText = "Where did these spikes come from?",
        
    }
},
{
    "eileens-room_eileen_psychic2_b_a",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                "Anyways, you aren’t a {13}...",
                "But still you can understand what I’m saying.",
                "Been a long time since a non-{13} could understand us.",
        },
        
    }
},
{
    "eileens-room_eileen_psychic2_b_a_a",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                "I’m {11}.",
                "You’ll probably meet my little sister {12} soon.",
        },
        
    }
},
{
    "eileens-room_eileen_psychic2_b_a_b",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                "I’m {11}.",
                "Overheard you already talking to my little sister {12}.",
        },
        
    }
},
{
    "eileens-room_eileen_psychic2_b_a_ab_a",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                "She’s always in a mood.",
                "Yells at everyone.",
        },
        
    }
},
{
    "eileens-room_eileen_psychic2_b_a_ab_a_a",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                "Won’t ever be able to paint the {8} like that.",
                "I always tell her,| it needs more|| <b><i>{14}</b></i>.",
                "Won’t listen to me though.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                null,
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                null,
        }
    }
},
{
    "eileens-room_eileen_psychic_talked",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                "She needs to hear it from someone else.",
                "That it needs more <b><i>{14}</b></i>.",
                "She’ll never listen to me, nope.",
                "Now please leave already.",
        },
        
    }
},
{
    "eileens-room_eileen_psychic1_talked",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                "Why’re you still here?",
                "Even if she wanted to paint the {8}...",
                "...she’ll be missing a vital part if she can't visualize...",
                "...<b><i>{14}</b></i>.",
        },
        
    }
},
{
    "eileens-room_eileen_psychic2_talked",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                "I always suggest she needs to render <b><i>{14}</b></i>.",
                "But I guess she’s at that age now...",
                "Where she wants to be the exact opposite of me.",
        },
        
    }
},
{
    "eileens-room_eileen_psychic_ellenia-hurt",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                "It’s fate.",
                "There's really no use.",
                "I guess no one can really escape it.",
        },
        
    }
},
{
    "eileens-room_eileen_eat-reaction",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                "(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// Ellenia's Room
{
    "ellenias-room_bookshelf_lore-book",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "{22}: On Love and Death Vol. 1... hmm should I skim through it?",
        },
        
    }
},
{
    "ellenias-room_bookshelf_lore-book_a_a",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "<i>“As a</i> {13}... <i>thou must at all costs seize thy chance to find Selfhood – and put an end to thee eternal cycle.”</i>",
                "<i>“So thou must remember, once ye find the moment, seize it by its tail because...”</i>",
                "<i>“The line between love and death is a thin one, my dear.”</i>",
        },
        
    }
},
{
    "ellenias-room_bookshelf_hentai-zine_prompt",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "Should I skim through this magazine purposefully shoved into the dark corner of this bookshelf?",
        },
        
    }
},
{
    "ellenias-room_bookshelf_hentai-zine",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "I'm guessing I'm not supposed to see that...",
        },
        
    }
},
{
    "ellenias-room_painting_snake-head",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "A snake. Or to be more exact, a cobra.",
        },
        
    }
},
{
    "ellenias-room_painting_bear-claw",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "I can’t imagine what it must feel like to get slashed by these.",
        },
        
    }
},
// ------------------------------------------------------------------
// Ellenia's Room - Ellenia Weekday
{
    "ellenias-room_ellenia_default",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "���� ���!",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "Whoa you can talk to me?| You’re obviously not a {13} and definitely not an {6}.",
                "Wait,| have you not heard of me?! Jeez... outsiders can be pretty uncultured these days...",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_a",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "I’m part of a long line of <b>famous</b> writers and illustrators.",
                "To tell you the truth, it’s my dream to be <b>famous</b> one day too!",
                "And I’ll do it by painting the portrait of...",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_masked-one",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "The {8}.",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_masked-one_choices",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "...",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_masked-one_a",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "Ha-ha-ha, you're only asking that because you've already given up!",
                "But when I was little, I saw this drawing of sunflowers, and now I can’t ever look at sunflowers the same! They aren’t just sunflowers to me anymore, for better or for worse.",
                "And I figure if I’m famous, I’ll be able to meet whoever drew them. No chance I could do it right now, just look at me.",
        },
        choiceText = "Why do you want to be famous in the first place?",
        
    }
},
{
    "ellenias-room_ellenia_psychic_masked-one_b",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "No... actually, I haven't. But no one has, you got that?!",
                "Anyways part of the excitement is not knowing, ha-ha!",
                "Besides, simpletons like you wouldn’t understand.",
        },
        choiceText = "Wait, you’ve seen the {8}?",
        
    }
},
{
    "ellenias-room_ellenia_psychic_masked-one_ab_a",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "Anyways, see these paintings over here...",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_snake-painting",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "They say a few {19} have actually seen the {8}. and the ones that do meet a terrible fate.",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_snake-painting_a",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "Choked. Like being constricted by a snake.",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_quest-painting",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "Meat. They become prey.",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_bear-claws-painting",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "Carved. Imagine a bear’s claws knifing through you.",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_bear-claws-painting_a",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "There’s nothing pretty about it.",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_explain",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "But you see, I’m not after beauty. no! I’m after what's never been seen before!",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_explain_a",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "Oh you've heard about the {22}.",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_ending",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "Honestly, I have no time to even think about that. I have to make something I’m truly proud of first...",
                "Anyways,| what do you think of my painting so far???",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_wrong",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "Ha... you pleb, what do you know... I don’t even know why I asked.",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_on-correct",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                ".........",
                "After all these years, I can’t believe it,| but <size=16>you</size>...",
                "Some outsider... a complete stranger...",
                "You’ve been the only one to truly <b>see</b> me.",
                "It seems all my life I’ve been trying to hide myself.| Slowly I’m turning invisible...",
                "I’ve become so scared to try anything,| to even just dip my toes in the water...",
                "But now I know my true <b>purpose</b>.| If I bring my work outside of this wretched {37} -- to the other side -- it can truly be understood.",
                "And I’ll be able to one day find the one who drew those sunflowers.",
                "As a token of my appreciation, take this...",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_item-received",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "I know it’s probably the greatest gift you’ve ever gotten in your miserable life, ha! But I won’t be needing it where I’m going.",
                "It’s time for me to make a name for us {7} once and for all!",
                "Bye!",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_exit",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "Oh and last thing, make sure to <b>take a good look at my painting on that easle</b>. It’ll be worth millions soon!",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_question_repeat",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "You finally have something worthwhile to say about my painting?",
                "Spit it out, what is it already?",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_on-correct_redo",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                ".........",
                "After all these years, I can’t believe it,| but <size=16>you</size>...",
                "Some outsider... a complete stranger...",
                "You’ve been the only one to truly <b>see</b> me.",
                "It seems all my life I’ve been trying to hide myself.| Slowly I’m turning invisible...",
                "I’ve become so scared to try anything,| to even just dip my toes in the water...",
                "But now I know my true <b>purpose</b>.| If I bring my work outside of this wretched {37} -- to the other side -- it can truly be understood.",
                "And I’ll be able to one day find the one who drew those sunflowers.",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_on-correct_redo_a",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "It’s time for me to make a name for us {7} once and for all!",
                "Bye!",
        },
        
    }
},
{
    "ellenias-room_ellenia_eat-reaction",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "( ಠ◡ಠ )",
        },
        
    }
},
// ------------------------------------------------------------------
// Not comfortable
{
    "ellenias-room_ellenia-weekend_psychic_not-comfortable",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "Hey! Who do you think you are?",
                "Do you really think I need opinions from outsiders?",
                "Ha, what a joke.",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic1_not-comfortable",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "What are you still doing here?",
                "Get out already! I don’t need any distractions, especially when I’m creating the next masterpiece, don’t you understand?",
        },
        
    }
},
// ------------------------------------------------------------------
// Comfortable
{
    "ellenias-room_ellenia-weekend_psychic_comfortable",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "Have we met before?",
                "For some reason it feels like we’ve met before.",
                "Anyways, it’s not like I need any advice, especially from a stranger!",
                "But it looks like you have something to say about my painting?",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic1_comfortable",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "You finally have something worthwhile to say about my painting?",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_comfortable_question",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "Spit it out, what is it already?",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_comfortable_on-correct",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                ".........",
                "After all these years, I can’t believe it,| but <size=16>you</size>...",
                "Some outsider... a complete stranger...",
                "You’ve been the only one to truly <b>see</b> me.",
                "It seems all my life I’ve been trying to hide myself.| Slowly I’m turning invisible...",
                "I’ve become so scared to try anything,| to even just dip my toes in the water...",
                "But now I know my true <b>purpose</b>.| If I bring my work outside of this wretched {37} -- to the other side -- it can truly be understood.",
                "And I’ll be able to one day find the one who drew those sunflowers.",
                "As a token of my appreciation, take this...",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_comfortable_has-sticker",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "Wait you’re saying you already have one of these?",
                "That’s really strange, I thought I was the only one who had this type of sticker.",
                "Oh well, no time to waste! Bye!",
        },
        
    }
},
// ------------------------------------------------------------------
// Hurt
{
    "ellenias-room_ellenia-weekend_psychic_hurt",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "It’s nothing really...",
                "I wouldn’t want to bother a stranger anyways.",
        },
        
    }
},
{
    "ellenias-room_player_ellenia-hurt-reaction",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "{12}, we know each other! Get a grip!",
                "You’re playing around right? This is all some sort of sick joke right?",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_choices",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "My memory is hazy.| Please forgive me, I really can’t bring to mind who you are.",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_choices_a",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "It’s really not worth the story. I wouldn't want to burden you with it.",
        },
        choiceText = "Seriously what happened? Please you have to let me know.",
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_choices_b",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "Well good-bye stranger.",
                "I’ll be alright... alright.",
        },
        choiceText = "Well I hope things turn out fine. I should get going at this point.",
        
    }
},
{
    "ellenias-room_player_ellenia-hurt-reaction_choices",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "(This is really strange. This isn't like the {12} I know.)",
        },
        
    }
},
{
    "ellenias-room_player_ellenia-hurt-reaction_choices_a",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {

        },
        choiceText = "It’s okay, I have time.",
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_story",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "Okay... well, I began having my doubts, in painting the {8}...",
                "So I went and found a job over at the {35} in {75}.",
                "You know how it is,| us {7} need to make a living in the end,| and| long story short...",
                "This happened.",
                "<b>My fate has been decided for me...</b>",
                "It’s time I face reality.",
                "I was never going to be a famous painter anyway. It really is actually all my own fault this happened.",
        },
        
    }
},
{
    "ellenias-room_player_ellenia-hurt-reaction_choices_b",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {

        },
        choiceText = "Alright suit yourself. I tried.",
        
    }
},
{
    "ellenias-room_ellenia-weekend_ellenia-hurt-reaction_choices_b_a",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                "Well good-bye stranger.",
                "I’ll be alright... alright.",
        },
        
    }
},
// ------------------------------------------------------------------
// Grand Mirror Room
{
    "grand-mirror-room_grand-mirror_responsibility",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "You've come to visit me all the way down here?| How very nice of you.",
                "It seems you are proving your worth, my dear.",
                "And as a result, I'm beginning to trust you.|.|.| So I'm here to give you some greater responsibilities.",
                "Do you accept?",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "grand-mirror-room_grand-mirror_responsibility_choice0",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "Very well then, my dear, heh heh.",
                "Allow me to demonstrate.",
        },
        choiceText = "Yes",
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "grand-mirror-room_grand-mirror_responsibility_choice1",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "Well that’s just too bad, my dear, heh heh.",
                "Because it's the only way you can get out from down here...",
                "Now allow me to demonstrate.",
        },
        choiceText = "No",
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "grand-mirror-room_grand-mirror_responsibility1",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "Also...| You'll be needing this for the rest of your journey| in case you ever get| <b>l|o|s|t|</b>.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "grand-mirror-room_grand-mirror_responsibility2",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "Why am I being so nice to you?| Well it's simple, my dear.| You might be of some use to me.",
                "Well bye now.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
// ------------------------------------------------------------------
// Wells World
{
    "wells-world_moose_default",
    new Model_Languages
    {
        speaker = "{63}",
        EN = new string[]
        {
                "�.",
        },
        
    }
},
{
    "wells-world_moose_psychic",
    new Model_Languages
    {
        speaker = "{63}",
        EN = new string[]
        {
                "I can’t leave here until I learn this last spell...",
        },
        
    }
},
{
    "wells-world_moose_psychic_a",
    new Model_Languages
    {
        speaker = "{63}",
        EN = new string[]
        {

        },
        choiceText = "I have what you’re looking for.",
        
    }
},
{
    "wells-world_moose_psychic_b",
    new Model_Languages
    {
        speaker = "{63}",
        EN = new string[]
        {
                "You youngins just don't understand. Leave me be.",
                "I have important spells to learn. I can’t be wasting my precious time with loafers like you.",
        },
        choiceText = "Please forget about the spell, you should really just get out of here.",
        
    }
},
{
    "wells-world_moose_psychic_a_has-book",
    new Model_Languages
    {
        speaker = "{63}",
        EN = new string[]
        {
                "You are really proving to be a gem.",
                "Here, I may be rough around the edges but I pay what’s due.",
        },
        
    }
},
{
    "wells-world_moose_psychic_a_has-book_a",
    new Model_Languages
    {
        speaker = "{63}",
        EN = new string[]
        {
                "I’m called {63} in this realm.",
                "And let’s see about this spell... Ah this technique is quite familiar.",
                "Hmm... ah! I think I got it!",
        },
        
    }
},
{
    "wells-world_moose_psychic_a_missing-book",
    new Model_Languages
    {
        speaker = "{63}",
        EN = new string[]
        {
                "You youngins just don't understand. Leave me be.",
                "I have important spells to learn. I can’t be wasting my precious time with loafers like you.",
        },
        
    }
},
{
    "wells-world_moose_eat-reaction",
    new Model_Languages
    {
        speaker = "{63}",
        EN = new string[]
        {
                "(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// Underworld
{
    "underworld_cursed-myne_default",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                "���.",
        },
        
    }
},
{
    "underworld_cursed-myne_psychic",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                "You’ll be just like me, dear.",
        },
        
    }
},
{
    "underworld_cursed-myne_eat-reaction",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                "(. ﾟーﾟ)",
        },
        
    }
},
{
    "underworld_cursed-HMS_default",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                "�� ��.",
        },
        
    }
},
{
    "underworld_cursed-HMS_psychic",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                "He-he, you’re just like me! He-he.",
        },
        
    }
},
{
    "underworld_cursed-HMS_eat-reaction",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                "(. ﾟーﾟ)",
        },
        
    }
},
{
    "underworld_cursed-female_default",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                "���.",
        },
        
    }
},
{
    "underworld_cursed-female_psychic",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                "Don’t be afraid dear, you’ll become just like me.",
        },
        
    }
},
{
    "underworld_cursed-female_eat-reaction",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                "(. ﾟーﾟ)",
        },
        
    }
},
{
    "underworld_cursed-HMS-bent_default",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                "�� ��.",
        },
        
    }
},
{
    "underworld_cursed-HMS-bent_psychic",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                "He-he, we’re the same can’t you see?? He-he.",
        },
        
    }
},
{
    "underworld_cursed-HMS-bent_eat-reaction",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                "(. ﾟーﾟ)",
        },
        
    }
},
{
    "underworld_cursed-abomination_default",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                "�.",
        },
        
    }
},
{
    "underworld_cursed-abomination_psychic",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                "Ugo-ohhhhhhhhhh!",
        },
        
    }
},
{
    "underworld_cursed-abomination_eat-reaction",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                "(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// Celestial Gardens
{
    "celestial-gardens_garden-labyrinth_puppets",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                "Hand puppets...| This room actually reminds me a bit of a theatre stage.",
        },
        
    }
},
// ------------------------------------------------------------------
// Default Entrances
{
    "painting-entrances_default_read",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "An empty painting with a doormat.",
        },
        
    }
},
{
    "painting-entrances_default_prompt",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "“I cannot believe in Western sincerity because it is invisible, but in feudal times we believed that sincerity resides in our entrails, and if we needed to show our sincerity, we had to cut our bellies and take out our visible sincerity.”        – Yukio Mishima",
        },
        
    }
},
{
    "painting-entrances_default_prompt1",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "Now would you like to enter me?",
        },
        
    }
},
{
    "painting-entrances_default-not-empty_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "A painting with a doormat.",
        },
        
    }
},
{
    "painting-entrances_default-short_prompt",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "Are you, am I?",
        },
        
    }
},
{
    "painting-entrances_default-short_prompt1",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "Now would you like to enter me?",
        },
        
    }
},
{
    "painting-entrances_eileens-mind_read",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "This reminds me of a certain romantic painting.",
        },
        
    }
},
{
    "painting-entrances_eileens-mind_prompt",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "Peering out over the raging seas!",
        },
        
    }
},
{
    "painting-entrances_eileens-mind_prompt1",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "Now would you like to enter me?",
        },
        
    }
},
{
    "painting-entrances_dark-corridor_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "I've seen a painting like this somewhere.",
        },
        
    }
},
// ------------------------------------------------------------------
// Quest Paintings
{
    "quest-paintings_eileens-room_default",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "An unfinished painting. Holding hands?",
        },
        
    }
},
{
    "quest-paintings_eileens-room_done",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "No, they're not holding hands. Someone is trying their hardest to pull someone else up.",
        },
        
    }
},
{
    "quest-paintings_ellenias-room_default",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "*shiver*",
                "This painting... it's not done.",
                "There’s also a doormat in front of it, strange.",
        },
        
    }
},
{
    "quest-paintings_ellenias-room_done",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "You know that feeling when you can’t get warm no matter how much you shiver?",
        },
        
    }
},
{
    "quest-paintings_ids-room_default",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "A sketch of a thorny vine.",
        },
        
    }
},
{
    "quest-paintings_ids-room_done",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "It's actually two vines weaving to be one.",
        },
        
    }
},
// ------------------------------------------------------------------
// World Paintings
{
    "ballroom_world-paintings_wells-world",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "I sometimes feel like I'm at the bottom of a well.",
        },
        
    }
},
{
    "ballroom_world-paintings_celestial-gardens-world",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "A labyrinth. There's always an entrance and an exit to a labyrinth, right?",
        },
        
    }
},
{
    "ballroom_world-paintings_xxx-world",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                "Flowers are best when they're dried and hung.",
        },
        
    }
},
// ------------------------------------------------------------------
// Items
{
    "item-object_sticker_boar-needle",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "The @@BoarNeedle @@Sticker_NoBold allows you to enter paintings that have a doormat. Use the @@BoarNeedle's {67} when face-to-face with a painting with a doormat.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "item-object_sticker_animal-within",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "If there are edible obstacles in your way, use the @@AnimalWithin @@Sticker_NoBold to eat through them.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "item-object_sticker_ice-spike",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "The @@IceSpike is a spike so powerful, it'll crack open just about anything.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "item-object_sticker_melancholy-piano",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "Use the @@MelancholyPiano @@Sticker_NoBold to follow the chords of your heart to the next piano.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "item-object_sticker_last-elevator",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "<b>If you ever get lost</b>, the @@LastElevator @@Sticker_NoBold can be used anywhere inside {18} to take the {66} back to the {72}.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "item-object_sticker_let-there-be-light",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "@@LetThereBeLight @@Sticker_NoBold will illuminate certain dark areas.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "item-object_sticker_puppeteer",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "Use the @@Puppeteer @@Sticker_NoBold to control {73}. Not too shabby.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "item-object_usable_super-small-key",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "It's a @@SuperSmallKey! Made specifically for regular sized keyholes.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "item-object_collectible_last-well-map",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "It's the @@LastWellMap. Rumor is everything begins in the Spring, including {18} itself.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "item-object_collectible_last-spell-recipe-book",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                "You've found the @@LastSpellRecipeBook. Promise this is the last one.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
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
        EN = new string[]
        {

        },
        choiceText = "Yes",
        
    }
},
{
    "choices_default_no",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {

        },
        choiceText = "No",
        
    }
},
{
    "choices_default_yes_excited",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {

        },
        choiceText = "Yes!",
        
    }
},

};
}

