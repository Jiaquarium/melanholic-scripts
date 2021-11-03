// Last created by Dialogue Exporter at 2021-11-03 00:55:07

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
                @"Strange, I can't understand what they're saying.",
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
                @"The Piano chords echoed...| but nothing happened...",
        },
        
    }
},
// ------------------------------------------------------------------
// Internal Dialogue
{
    "hotel-lobby_player-internal_time-stopped",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Time......| It seems to be frozen in here.",
                @"I always get this feeling just before the sun is about to come up.",
        },
        
    }
},
{
    "hotel-lobby_player-internal_repeating",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Why do I feel half asleep... what day is it today anyways?",
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
                @"The chords of the piano complement the beating of your heart.",
                @"This location is remembered.",
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
                @"Cold.",
                @"It's ice...",
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
                @"According to my calculations,| today {77} should be at {49}.",
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
                @"If you accept, sign your name next to the X.",
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
    "hotel-lobby_player_new-game-thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {

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
                @"I really would like to leave, but no I can’t.",
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
                @"I’m being watched.",
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
                @"I hate this feeling.",
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
                @"The day shift guy always forgets to rebrew the coffee when he gets off, so I always do it.",
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
                @"It’s an old photo with Dad and my little sis.",
                @"She’s working some job in finance now – honestly I don’t even really know what she does though.",
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
                @"A recipe book for French pastries. Bon appetit!",
                @"I’ve read this a million times by now, sigh.",
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
                @"<b>Tangled Hair</b>, a book of poetry.",
                @"I like poems and all, but if I had a choice, I’d rather be a novelist.",
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
                @"It’s a textbook, <b>The Art of Fractal Geometry</b>.",
                @"Absolutely no clue why we have this here...| anyways, it’s interesting too.",
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
                @"Someone must’ve been leafing through these...",
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
                @"Hm I don’t ever remember us having a book like this.",
                @"Wait a second, there’s something inside.",
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
                @"A painting of the mogul who originally funded construction of the hotel. Not too much is known about him.",
                @"It’s a strange painting. No matter how I look at it I can’t ever make out his face.",
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
                @"Rumor is this wasn’t supposed to be a hotel.",
                @"Eh, I don’t like thinking about this kind of stuff during my shift...",
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
                @"This isn't the same portrait as in the Lobby's Elevator Bay.| This man has a face.",
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
                @"It looks like he's half-smiling, half-crying.",
        },
        
    }
},
{
    "hotel-bay-v1-return_elevator_disabled_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"I should really get back to work.",
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
                @"It’s a mirror. Should I look into it?",
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
// Myne's Mirror Interaction Nodes
{
    "dining_mynes-mirror_interaction-node",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                @"Welcome to...",
                @"『 {37} 』",
                @"Please do not look so frightened, dear.| What is your name?",
                @"{0} you say?| Well, it looks like you are not on the list of guests we were expecting today.",
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
                @"......",
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
                @"Outside?| I really have no clue as to what you are referring to, my dear.",
                @"Are you seeing things? Perhaps you are half-asleep, have you been up all night?",
        },
        choiceText = "What were those outside?",
        
    }
},
{
    "dining_mynes-mirror_interaction-node_end",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                @"Oh dear, I almost forgot to introduce myself, where are my manners? My sincerest apologies.",
                @"I am just a bit flustered is all. It is not every day that someone arrives at one's residence unannounced.",
                @"But a guest is a guest afterall.",
        },
        
    }
},
{
    "dining_mynes-mirror_interaction-node_end1",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                @"My name is {10}.",
                @"<b>I am the creator and original owner of this fine mansion you see here.</b>",
                @"And it seems you have stumbled here not of your own accord.",
                @"Allow me to do you a favor and guide you safely to the exit.",
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
        }
    }
},
{
    "dining_mynes-mirror_interaction-node_1",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                @"<b>You again?</b> I thought I just showed you the exit. Why have you returned?",
                @"My apologies... my tone of voice is unacceptable.",
                @"A guest is a guest afterall.",
        },
        
    }
},
{
    "dining_mynes-mirror_interaction-node_1_choice-prompt",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                @"So may I ask kindly, what brings you back here?",
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

        },
        choiceText = "A sheep. It's telling me to follow it.",
        
    }
},
{
    "dining_mynes-mirror_interaction-node_1_choice1",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {

        },
        choiceText = "I can't sleep.",
        
    }
},
{
    "dining_mynes-mirror_interaction-node_1_end",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                @"Dear! Have you been up all night?",
                @"They say you can hallucinate from lack of sleep.",
                @"Your mind might be playing tricks on you.",
                @"If I may say so, the best solution is for you to find the nearest exit and get some rest, trust me dear.",
                @"Forget about everything you have witnessed here.",
                @"And please do not return, it is for your own good.",
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
                @"...",
                @"Just down the hall on your right, through the {36}, you should be able to find the {66}.",
                @"You may safely and securely exit through there, my dear.",
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
                @"...",
                @"Down the hall on your right, through the {36}, you will find the {66}.",
                @"You should exit through there, my dear.",
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
                @"...",
                @"Take the hall on your right, go through the {36}, and you will find the {66}.",
                @"You really ought to exit through there, my dear.",
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
                @"...",
                @"Just down the hall on your right, through the {36}, is the {66}.",
                @"You may exit through there, my dear.",
        },
        
    }
},
// ------------------------------------------------------------------
// Myne's Mirror Special Conditions Nodes
{
    "dining_mynes-mirror_special-conditions_the-sealing",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                @"{0} was it? You really should not be here.",
                @"What business do you have here?",
        },
        metadata = new Model_Languages.Metadata[]
        {
                null,
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "dining_mynes-mirror_special-conditions_the-sealing_choice0",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                @"He-he-he.",
                @"So it seems you found out about the {22}.",
                @"I think {57} told you everything you need to know.",
                @"It is simple, my dear. At {49} our doors will close.",
        },
        choiceText = "The {22}? What do you know about it?",
        
    }
},
{
    "dining_mynes-mirror_special-conditions_the-sealing1",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                @"Now the best thing for you to do is to leave here quickly. Your safety is my top priority.",
                @"...",
                @"Just down the hall on your right, through the {36}, is the {66}.",
                @"You may exit through there, my dear.",
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
                @"Please have a wonderful rest of your @@Run.",
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
                @"Only thing I hate about games is losing.",
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
                @"It’s a painting... close up it's a lot less flat than I thought. Whoever painted it used a lot of paint.",
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
                @"Anonymous.| <i>Found My Other Half</i>.| 19XX.| Oil on linen.",
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
                @"����.",
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
                @"Grrrrrr......",
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
                @"My duty is to guard, so guard is what I shall do.",
                @"If you know your role then play it with honor!",
                @"Woof!",
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
                @"*wimper*",
                @"I’ve failed my duty.",
                @"It truly is fate.",
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
                @"I’ve lost {3}.",
                @"It’s my job, my role, but I’ve failed. I am terribly sorry.",
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
                @"(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// Mirror Halls
{
    "mirror-hall-2_hint_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"A very poetic self-reminder...",
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
                @"Estimated wait from this point: 0 minutes.",
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
                @"�  �  �  �.",
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
                @"{0}.",
                @"Listen kiddo, I have a hunch.",
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
                @"......",
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
                @"But first that look on your face...",
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
                @"I know.",
                @"......",
                @"This might sound crazy but|.|.|.",
        },
        choiceText = "I met him, {10}.",
        metadata = new Model_Languages.Metadata[]
        {
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
    "basement_ids_n-room_on-nameplate-done_choices_talked-with-myne_b",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Oh it looked like you wanted to say something... nevermind.",
                @"Anyways...... this might sound crazy but...",
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
                @"{0}.",
                @"Listen kiddo, I have a hunch.",
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
                @"This might sound crazy but|.|.|.",
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
    "basement_ids_e-room_dance-intro",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Oh hey, {0}. Nice of you to join me, kiddo, I was getting a little lonely.",
                @"What are we about to do you ask?",
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
                @"You know why us {21} love dancing in the dark?",
                @"There’s just something so romantic about staring into complete blackness, while feeling our warm wool rubbing against one another...",
                @".|.|.|.|.|.",
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
                @"Wow...| you’re kinda terrible.",
                @"It’s okay though, no one's great on their first go.",
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
                @"Wow you can really <b><i>move</i></b>.",
                @"Okay it's settled...| I'll just say it.",
                @"Listen closely kiddo,| I'm <b><i>|p|a|r|t| o|f| y|o|u</i></b>.",
                @"Actually, there's a bit more to my theory. But anyways...",
                @"You should be the one to use this. So here...",
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
                @"Wanna try again?",
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
                @"Yay. Let’s go go go, move that body!",
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
                @"Why must you tease me, hmph.",
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
                @"Two certificates of some sort... I wonder why one is hung higher than the other...",
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
                @"The figures in the painting are dancing. Looks fun.",
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
                @"Anonymous. <i>You and I Forever</i>. 19XX. Oil on Linen.",
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
                @"...the answer to everything...",
                @"...a fresh way to approach a dull thing...",
                @"...a way of doing things...",
                @"...S&M style...",
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
                @"� � �...",
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
                @"The name’s {33}.",
                @"You look like you need a drink.| I can tell from all these years running the {35}.",
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
                @"To be frank,| some disturbing things have been happening at my <b>saloon</b>, so I’m here to speak to the <b>King</b> about it.",
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
                @"Hopefully the <b>King</b> takes action...| and fast.",
                @"I got a business to run.| I’m <b>bleeding</b> sales as we speak, don’t you understand?",
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
                @"(. ﾟーﾟ)",
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
                @"�.",
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
                @"There’s no use. The {22}. It’s been decided.",
                @"I specialize in dealing with spells. No outsiders means no more new spells for me.",
                @"I’ll have to focus on the spells I know then.",
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
                @"(. ﾟーﾟ)",
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
                @"���� ���...",
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
                @"There all just a bunch of {60}.",
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
                @"Oh I’m {58}.",
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
                @"Didn't you hear? The ones left in here are slowly becoming <b>cursed</b>.",
                @"Why am I still here then?| It's simple, I have no where else to go.",
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
                @"Yeah most of them left already...",
                @"They're just a bunch of {60} anyways.",
                @"Why would I want more {60} in here?",
        },
        
    }
},
{
    "ballroom_suzette_psychic2",
    new Model_Languages
    {
        speaker = "{58}",
        EN = new string[]
        {
                @"Well the only thing anyone ever talks about these days is the {22}.| Me?| What do I think about it?",
                @"It's really none of your business.",
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
                @"(. ﾟーﾟ)",
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
                @"����!!",
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
                @"���� ��!",
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
                @"Ha! Good riddance,| to tell you the truth I’m glad we’re going through with the {22}.| Isn’t that right, {62}?",
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
                @"Yeah, you tell’em, {61}!",
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
                @"We opened our doors and now look at us. You give a wolf your foot and it’ll come back in the winter for your hand!",
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
                @"We’ll do just fine by ourselves in here. It’s how our ancestors lived, ain’t it?",
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
                @"We here are {37} natives. Born and raised!| How can you call yourself an {6} when you can’t even toughen out through the {22}?!",
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
                @"Right! A true {6} never complains.| Head down and never frown!",
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
                @"(. ﾟーﾟ)",
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
                @"���� ��� �������...",
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
                @"It’s no secret we’re in a dire situation right now.",
                @"The {42}... They've invaded. Most folks have already fled...",
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
                @"But my plan will save {18}! Mark my words. I have already made preparations.",
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
                @"<b>At exactly</b> {49}<b>, the {22} will be complete.</b>",
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
                @"<b>We will lock our doors and drive out these wicked forces!</b>",
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
                @"(. ﾟーﾟ)",
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
                @"Are you here to try and help?| Rest assured, I am the sacred elder of {18}, {57}. It is my sworn duty to protect our residents!",
                @"What is all this commotion about? We are in a dire situation.",
                @"Intruders have invaded {37}...| The {42}.",
                @"Rumor has it they even have the power to transform the innocent.",
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
    "ballroom_cut-scene_kings-intro_sealing-explanation",
    new Model_Languages
    {
        speaker = "{57}",
        EN = new string[]
        {
                @"Most of our citizens have left already.",
                @"But I have a plan to save {18}!",
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
    "ballroom_cut-scene_kings-intro_lock",
    new Model_Languages
    {
        speaker = "{57}",
        EN = new string[]
        {
                @"<size=18><b>The</b></size> <size=18>{22}.</size>",
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
                @"It came to me in a dream,| no,| a vision.| I have already made preparations.",
                @"<b>At exactly</b> {49}<b>, it will be complete.</b>",
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
                @"<b>We will lock our doors and drive out these wicked forces!</b>",
                @"Mark my word.| I will return {18} to a time of peace!",
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
                @"����.",
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
                @"This is not the answer, for God’s sake! We need to get to the root of the problem.",
                @"Find those damn intruders. Figure out where they’re coming from and attack them first! Not lock ourselves away!",
                @"My whole family is outside of {18}. What do they expect me to do?!",
                @"Of course I want to stay here with {39}, but the closer it gets to {49}..",
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
                @"I was born outside the {37} walls, I don’t think I can stay here forever like the true {7} will.",
                @"But I know I won’t be able to find anyone else like {39}.",
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
                @"(. ﾟーﾟ)",
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
                @"����.",
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
                @"Yeah {38} gets really worked up sometimes. Oh of course, I don’t agree with the King’s decision either, but it’s reality, it’s going to happen. {49} on the dot.",
                @"If he ends up leaving before the {22} to return to his family, I’d understand. It might be a sign rather. The way I look at it, it’s fate.",
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
                @"Oh but please don’t tell him I said any of this. Like I said, he gets really worked up over nothing.",
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
                @"(. ﾟーﾟ)",
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
                @"����...",
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
                @"......",
                @"It's useless to fight against fate.",
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
                @"What could I have done?",
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
                @"(. ﾟーﾟ)",
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
                @"Blank canvases...| I took a portraiture painting class once. Bad memories.",
                @"The hardest part is getting started, really.| That first mark.",
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
                @"But after a few strokes, things got pretty <b>exciting</b>.",
                @"From there you can really go anywhere.",
                @"There was a certain kind of thrill building up this thing and knowing I could destroy it all at once.",
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
                @"And even if I royally mess up, I can just paint over it.",
                @"And over and over and over and over.",
                @"Then the hard part becomes knowing when to stop!",
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
                @"A day in the life of a cat. I’ve always wondered what life would be like as a cat.",
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
                @"����.",
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
                @"{64} here at your humble service.",
                @"Oh, you are able to understand me?",
                @"My mistake. go right along ahead, young one, I am here to serve.",
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
                @"I’m terribly sorry to inform you that {52} is the sisters’ rest day.",
                @"Come back another time, I am here to serve.",
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
                @"(. ﾟーﾟ)",
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
                @"A cat jumping onto a bed! I actually quite enjoy cat paintings.",
                @"Why is it hung so high though? I can't really get a good look at it.",
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
                @"�.",
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
                @"Not in the mood.",
                @"Don’t you dare talk to me again...",
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
                @"Why do you keep trying to talk?",
                @"Told you I’m really not in the mood.",
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
                @"Sorry about the spikes.",
                @"I really really hate this place, I really do.",
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
                @"Hey no one asked for your opinion!",
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
                @"Can’t help it, they’ve always been there.",
                @"Up, down, up, and down they go. All day and all night long.",
                @"I really really hate it.",
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
                @"Anyways, you aren’t a {13}...",
                @"But still you can understand what I’m saying.",
                @"Been a long time since a non-{13} could understand us.",
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
                @"I’m {11}.",
                @"You’ll probably meet my little sister {12} soon.",
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
                @"I’m {11}.",
                @"Overheard you already talking to my little sister {12}.",
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
                @"She’s always in a mood.",
                @"Yells at everyone.",
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
                @"Won’t ever be able to paint the {8} like that.",
                @"I always tell her,| it needs more|| <b><i>{14}</b></i>.",
                @"Won’t listen to me though.",
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
                @"She needs to hear it from someone else.",
                @"That it needs more <b><i>{14}</b></i>.",
                @"She’ll never listen to me, nope.",
                @"Now please leave already.",
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
                @"Why’re you still here?",
                @"Even if she wanted to paint the {8}...",
                @"...she’ll be missing a vital part if she can't visualize...",
                @"...<b><i>{14}</b></i>.",
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
                @"I always suggest she needs to render <b><i>{14}</b></i>.",
                @"But I guess she’s at that age now...",
                @"Where she wants to be the exact opposite of me.",
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
                @"It’s fate.",
                @"There's really no use.",
                @"I guess no one can really escape it.",
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
                @"(. ﾟーﾟ)",
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
                @"{22}: On Love and Death Vol. 1... hmm should I skim through it?",
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
                @"<i>“As a</i> {13}... <i>thou must at all costs seize thy chance to find Selfhood – and put an end to thee eternal cycle.”</i>",
                @"<i>“So thou must remember, once ye find the moment, seize it by its tail because...”</i>",
                @"<i>“The line between love and death is a thin one, my dear.”</i>",
        },
        
    }
},
{
    "ellenias-room_bookshelf_hentai-zine_prompt",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Should I skim through this magazine purposefully shoved into the dark corner of this bookshelf?",
        },
        
    }
},
{
    "ellenias-room_bookshelf_hentai-zine",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"I'm guessing I'm not supposed to see that...",
        },
        
    }
},
{
    "ellenias-room_easle_angry",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"Hey! Don’t look, it’s not finished yet!",
        },
        
    }
},
{
    "ellenias-room_painting_snake-head",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"A snake. Or to be more exact, a cobra.",
        },
        
    }
},
{
    "ellenias-room_painting_bear-claw",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"I can’t imagine what it must feel like to get slashed by these.",
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
                @"���� ���!",
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
                @"Whoa you can talk to me?| You’re obviously not a {13} and definitely not an {6}.",
                @"Wait,| have you not heard of me?! Jeez... outsiders can be pretty uncultured these days...",
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
                @"I’m part of a long line of <b>famous</b> {6} writers and illustrators.",
                @"To tell you the truth, it’s my dream to have my art known across the land! One day!",
                @"My current subject of focus you ask?",
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
                @"<b>The original {8} of this mansion.</b>",
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
                @"...",
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
                @"Well, the reason. Ha! Isn't it obvious?!",
                @"We always want to paint what we can't see!",
                @"Simpletons like you wouldn't understand, okay?",
        },
        choiceText = "Why did you choose that subject?",
        
    }
},
{
    "ellenias-room_ellenia_psychic_masked-one_b",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"No... actually, I haven't. But no one has, you got that?!",
                @"The point of painting is to paint the things you can't see!",
                @"Simpletons like you wouldn't understand, okay?",
        },
        choiceText = "You’ve met the original {8}?",
        
    }
},
{
    "ellenias-room_ellenia_psychic_masked-one_ab_a",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"See these portraits here...",
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
                @"I call this one... <b><i>Labyrinth of Reflections</i></b>.",
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
                @"<b><i>Through the Frozen Garden</i></b>.",
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
                @"<b><i>Everything Returns to Zero</i></b>.",
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
                @"Yes, it's all coming together now!",
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
                @"So, I've been working on this new one for a while now. What do you think?",
                @"C'mon now! Give me one word that describes its essence!",
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
                @"Ha... you pleb, what do you know... I don’t even know why I asked.",
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
                @".........",
                @"After all these years, I can’t believe it,| but <b><i>you</i></b>...",
                @"<size=16><b>An outsider...</b></size>",
                @"Can actually <b>understand</b> me.",
                @"It seems all my life I’ve been trying to hide myself.| Slowly I’m turning invisible...",
                @"So scared to try anything truly new... scared every decision is the wrong one.",
                @"It's so clear now.",
                @"I need to leave this place and take my work to the outside world.",
                @"As a token of my appreciation, take this...",
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
                @"I know it’s probably the greatest gift you’ve ever received in your miserable life, ha!| But I won’t be needing it where I’m going.",
                @"It’s time for me to make a name for us {7} once and for all!",
                @"Bye!",
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
                @"Oh and last thing,| that <b>painting on the easle</b> is a work in progress, you got that?",
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
                @"You finally have something worthwhile to say about my painting?",
                @"Spit it out, what is it already?",
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
                @".........",
                @"After all these years, I can’t believe it,| but <b><i>you</i></b>...",
                @"<size=16><b>An outsider...</b></size>",
                @"Can actually <b>understand</b> me.",
                @"It seems all my life I’ve been trying to hide myself.| Slowly I’m turning invisible...",
                @"So scared to try anything truly new... scared every decision is the wrong one.",
                @"It's so clear now.",
                @"I need to leave this place and take my work to the outside world.",
                @"As a token of my appreciation, take this...",
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
                @"( ಠ◡ಠ )",
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
                @"Hey! Who do you think you are?",
                @"Do you really think I need opinions from outsiders?",
                @"Ha, what a joke.",
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
                @"What are you still doing here?",
                @"Get out already! I don’t need any distractions, especially when I’m creating the next masterpiece, don’t you understand?",
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
                @"Have we met before? For some reason you seem familiar.",
                @"Anyways, it’s not like I need any advice, especially from a stranger!",
                @"But it looks like you have something to say about my painting?",
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
                @"You finally have something worthwhile to say about my painting?",
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
                @"Spit it out, what is it already?",
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
                @".........",
                @"After all these years, I can’t believe it,| but <b><i>you</i></b>...",
                @"<size=16><b>An outsider...</b></size>",
                @"Can actually <b>understand</b> me.",
                @"It seems all my life I’ve been trying to hide myself.| Slowly I’m turning invisible...",
                @"So scared to try anything truly new... scared every decision is the wrong one.",
                @"It's so clear now.",
                @"I need to leave this place and take my work to the outside world.",
                @"As a token of my appreciation, take this...",
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
                @"Wait you’re saying you already have one of these?",
                @"That’s really strange, I thought I was the only one who had this type of {1}.",
                @"Oh well, no time to waste! Bye!",
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
                @"It’s nothing really...",
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
                @"{12}, we know each other! Get a grip!",
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
                @"Sorry I really don't mean to be a nuisance.",
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
                @"It’s really not worth the story. I wouldn't want to burden you with it.",
                @"Sorry for taking up your time.",
        },
        choiceText = "What happened here?",
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_choices_b",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"Well good-bye.",
                @"Sorry for taking up your time.",
        },
        choiceText = "I should really get going.",
        
    }
},
{
    "ellenias-room_player_ellenia-hurt-reaction_choices",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"(This is really strange. This isn't like the {12} I know.)",
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
                @"Okay... well, I began having my doubts, in painting the {8}...",
                @"So I went and found a job over at the {35} in {75}.",
                @"You know how it is,| us {7} need to make a living in the end,| and| long story short...",
                @"This happened.",
                @"<b>My fate has been decided for me...</b>",
                @"It’s time I face reality.",
                @"I was never going to be a famous painter anyway. It really is actually all my own fault this happened.",
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
                @"Well good-bye stranger.",
                @"I’ll be alright... alright.",
        },
        
    }
},
// ------------------------------------------------------------------
// Eileen's Mind
{
    "eileens-mind_myne_challenge_stop",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                @"{0}, how did you find the way in here?",
                @"I am trying to be of assistance to you...| but how can I assist you when you're wandering around like this?",
                @"The best thing for you to do is to turn back now, dear.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                null,
                null,
        }
    }
},
{
    "eileens-mind_myne_challenge_stop1",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                @"Really?| Stop this madness!",
                @"You are officially trespassing, do you not understand?!",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                null,
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
        EN = new string[]
        {
                @"You've come to visit me all the way down here?| How very nice of you.",
                @"It seems you are proving your worth, my dear.",
                @"And as a result, I'm beginning to trust you.|.|.| So I'm here to give you some greater responsibilities.",
                @"Do you accept?",
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
                @"Very well then, my dear, heh heh.",
                @"Allow me to demonstrate.",
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
                @"Well that’s just too bad, my dear, heh heh.",
                @"Because it's the only way you can get out from down here...",
                @"Now allow me to demonstrate.",
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
                @"Also...| You'll be needing this for the rest of your journey| in case you ever get| <b>l|o|s|t|</b>.",
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
                @"Why am I being so nice to you?| Well it's simple, my dear.| You might be of some use to me.",
                @"Well bye now.",
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
// Moose
{
    "wells-world_moose_default",
    new Model_Languages
    {
        speaker = "{63}",
        EN = new string[]
        {
                @"�.",
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
                @"I can’t leave here until I learn this last spell...",
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
                @"You youngins just don't understand. Leave me be.",
                @"I have important spells to learn. I can’t be wasting my precious time with loafers like you.",
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
                @"You are really proving to be a gem.",
                @"Here, I may be rough around the edges but I pay what’s due.",
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
                @"I’m called {63} in this realm.",
                @"And let’s see about this spell... Ah this technique is quite familiar.",
                @"Hmm... ah! I think I got it!",
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
                @"You youngins just don't understand. Leave me be.",
                @"I have important spells to learn. I can’t be wasting my precious time with loafers like you.",
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
                @"(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// Suzette
{
    "wells-world_suzette_default",
    new Model_Languages
    {
        speaker = "{58}",
        EN = new string[]
        {
                @"���.",
        },
        
    }
},
{
    "wells-world_suzette_psychic",
    new Model_Languages
    {
        speaker = "{58}",
        EN = new string[]
        {
                @"A dark place.",
                @"An even darker place than here is where you can find it.",
        },
        
    }
},
{
    "wells-world_suzette_psychic1",
    new Model_Languages
    {
        speaker = "{58}",
        EN = new string[]
        {
                @"It's all I’ve ever known.",
                @"Damn {60}.",
        },
        
    }
},
{
    "wells-world_suzette_eat-reaction",
    new Model_Languages
    {
        speaker = "{58}",
        EN = new string[]
        {
                @"(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
//     Underworld
{
    "underworld_cursed-myne_default",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"���.",
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
                @"You’ll be just like me, dear.",
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
                @"(. ﾟーﾟ)",
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
                @"�� ��.",
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
                @"He-he, you’re just like me! He-he.",
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
                @"(. ﾟーﾟ)",
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
                @"���.",
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
                @"Don’t be afraid dear, you’ll become just like me.",
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
                @"(. ﾟーﾟ)",
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
                @"�� ��.",
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
                @"He-he, we’re the same can’t you see?? He-he.",
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
                @"(. ﾟーﾟ)",
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
                @"�.",
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
                @"Ugo-ohhhhhhhhhh!",
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
                @"(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
//     Rock Garden
{
    "rock-garden_ids_default",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"� � � �.",
        },
        
    }
},
{
    "rock-garden_ids_psychic",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"If it’s a rose it will bloom, if it’s a leaf it will fall... sigh, never underestimate it...",
        },
        
    }
},
{
    "rock-garden_ids_psychic_a",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Hey whoa, I didn’t think anyone would find me down here.",
                @"...But it’s actually really nice to see you again.",
                @"{0}, was it?",
        },
        
    }
},
{
    "rock-garden_ids_psychic_1",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"When we call it sad it is joyful, when we call it joyful it is sad... sigh, never underestimate it...",
        },
        
    }
},
{
    "rock-garden_ids_psychic_1_a",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Well, thanks for listening, I’ll probably head back to my room now.",
                @"Just give me a sec.",
        },
        
    }
},
// ------------------------------------------------------------------
//     Garden Labyrinth
{
    "garden-labyrinth_puppets_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Hand puppets...| This room actually reminds me a bit of a theatre stage.",
        },
        
    }
},
{
    "garden-labyrinth_latte_opening",
    new Model_Languages
    {
        speaker = "{39}",
        EN = new string[]
        {
                @"We would always meet at the courtyard in the <b>center</b> of this garden...",
                @"Usually I know which turns to take, but it's completely left my memory ever since <i><b>they</b></i> started coming in here.",
                @"Honestly, I'm ready to leave this place for good...| but I can't leave without {38}, no.",
                @"I really miss {38}.",
        },
        
    }
},
{
    "garden-labyrinth_kaffe_opening_a",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                @"“Meet me under the <b>garden gazebo</b>.” That's what {39} would always tell me.",
                @"But somehow I've managed to lose her.| I'm not sure how much longer I can go on without {39}.",
                @"This was never this much of a maze before! It didn't used to be like this...",
        },
        
    }
},
{
    "garden-labyrinth_kaffe_blocked",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                @"No, I can't go any further, but I must find {39}.",
                @"No, it's much too dangerous on the other side.",
        },
        
    }
},
{
    "garden-labyrinth_latte_blocked",
    new Model_Languages
    {
        speaker = "{39}",
        EN = new string[]
        {
                @"I don’t think I should go any further than this. I need to find {38}...",
                @"I'm not ready to pass through here though. Who knows what could be on the other side?",
        },
        
    }
},
{
    "garden-labyrinth_kaffe_success",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                @"<size=16>{39}!</size>",
        },
        
    }
},
{
    "garden-labyrinth_latte_success_a",
    new Model_Languages
    {
        speaker = "{39}",
        EN = new string[]
        {
                @"<size=16>My</size> <size=16>{38}!</size>",
                @"Oh I’ve finally found you.",
        },
        
    }
},
{
    "garden-labyrinth_kaffe_success_a_a",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                @"We can finally leave here.| It’s finally over.",
        },
        
    }
},
{
    "garden-labyrinth_latte_success_a_a_a",
    new Model_Languages
    {
        speaker = "{39}",
        EN = new string[]
        {
                @"A new life. Let’s start a new life.| On the other side.",
        },
        
    }
},
{
    "garden-labyrinth_kaffe_default",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                @"����.",
        },
        
    }
},
{
    "garden-labyrinth_kaffe_psychic",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                @"“Meet me under the <b>garden gazebo</b>.” Yes, that was her favorite spot.",
                @"It was in the <b>center</b> of all this mess, but I've lost my way. I’ll never find my way there at this rate.",
        },
        
    }
},
{
    "garden-labyrinth_kaffe_psychic1",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                @"I can’t leave this wretched place alone.",
                @"What’s the point of it all without {39}?",
        },
        
    }
},
{
    "garden-labyrinth_kaffe_psychic2",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                @"You would never understand.",
        },
        
    }
},
{
    "garden-labyrinth_kaffe_eat-reaction",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                @"(. ﾟーﾟ)",
        },
        
    }
},
{
    "garden-labyrinth_latte_default",
    new Model_Languages
    {
        speaker = "{39}",
        EN = new string[]
        {
                @"����.",
        },
        
    }
},
{
    "garden-labyrinth_latte_psychic",
    new Model_Languages
    {
        speaker = "{39}",
        EN = new string[]
        {
                @"We always used to meet at the courtyard under the <b>garden gazebo</b>...",
                @"It was in the <b>center</b> of all this. I know {38} will be waiting for me there, if I could just find it...",
                @"Usually I know which turns to take, but it's completely left my memory ever since <i><b>they</b></i> started coming in here.| Kinda sad how fast my memory fades now, ha.",
        },
        
    }
},
{
    "garden-labyrinth_latte_psychic1",
    new Model_Languages
    {
        speaker = "{39}",
        EN = new string[]
        {
                @"Really strange things have been happening in here lately. If I can be honest with you, I'm ready to leave this place as soon as I can.",
                @"I just need to find {38} first...",
        },
        
    }
},
{
    "garden-labyrinth_latte_psychic2",
    new Model_Languages
    {
        speaker = "{39}",
        EN = new string[]
        {
                @"I really have my doubts about all this.| The {22} you know.",
        },
        
    }
},
{
    "garden-labyrinth_latte_eat-reaction",
    new Model_Languages
    {
        speaker = "{39}",
        EN = new string[]
        {
                @"(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// XXX World
{
    "xxx-world_wandering-cursed-one_default",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"���.",
        },
        
    }
},
{
    "xxx-world_wandering-cursed-one_psychic",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"Just you wait, my dear, you’ll be just like me soon enough.",
        },
        
    }
},
{
    "xxx-world_wandering-cursed-one_eat-reaction",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// Ursie
{
    "urselks-saloon_ursie_default",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {
                @"����...",
        },
        
    }
},
{
    "urselks-saloon_ursie_psychic",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {
                @"Howdy! You sure look like you need a drink.| I can tell these things after all these years managing the {35}.",
                @"Ever since <i>it</i> started coming around here though, strange things began happening... strangeness I can take... but my business is getting ruined!",
        },
        
    }
},
{
    "urselks-saloon_ursie_psychic_prompt",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {
                @"So partner, what do you say, can you help me out here?",
        },
        
    }
},
{
    "urselks-saloon_ursie_psychic_prompt_a",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {

        },
        choiceText = "Sure.",
        
    }
},
{
    "urselks-saloon_ursie_psychic_prompt_a_a",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {
                @"Terrific! Recently <b>strange growths have infested</b> one of my KTV rooms.",
                @"And now all my regulars are scared out of their wits.",
                @"My clients come for the karaoke and <i>company</i>. Although, they stay for the drinks.| And that’s where we profit, cha-ching!",
                @"How is the {35} going to be the best watering well around if I’m bleeding this much business?",
                @"What did you say your name was again, youngin’?",
                @"{0}... interesting, hey I like that name. It has a nice ring to it, seems familiar, not quite sure why.",
                @"...Perhaps you can do something about those strange growths.",
        },
        
    }
},
{
    "urselks-saloon_ursie_psychic_prompt_b",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {

        },
        choiceText = "Sorry, I'm a little busy right now!",
        
    }
},
{
    "urselks-saloon_ursie_psychic_prompt_b_a",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {
                @"Ah, well, that is very unfortunate...",
                @"I really hope the {35} can reach its full potential...",
        },
        
    }
},
{
    "urselks-saloon_ursie_psychic_after-unlock",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {
                @"Alright let’s get the {35} to its glory days!",
        },
        
    }
},
{
    "urselks-saloon_ursie_psychic_prompt_talked",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {
                @"Ah so you <i>can</i> help out?",
        },
        
    }
},
{
    "urselks-saloon_ursie_psychic_quest-active",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {
                @"It’s always been my dream to create the best saloon in all the land, a destination outsiders would travel far and wide just to visit.",
                @"It’s my fate, I know it, and I won’t leave here until I finish,| I promised that to myself at the very least.",
                @"And I don’t break promises.",
        },
        
    }
},
{
    "urselks-saloon_ursie_psychic_quest-active1",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {
                @"You know the {35} used to be the town grapevine. You got the latest news, gossip and of course specialty cocktails here.",
                @"And if you’re lucky you might even meet your special somebody here, he-he.",
        },
        
    }
},
{
    "urselks-saloon_ursie_psychic_quest-active2",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {
                @"If only I could get my KTV room back to operational again, maybe then the {35} could be the crown jewel of {37}.",
        },
        
    }
},
{
    "urselks-saloon_ursie_psychic_quest-active3",
    new Model_Languages
    {
        speaker = "{33}",
        EN = new string[]
        {
                @"You know why I wanted to get into the nightlife business in the first place?",
                @"I thought us {19} tend to show our true selves at night. You know it’s the time where you can really let loose.",
        },
        
    }
},
// ------------------------------------------------------------------
// Melba
{
    "urselks-saloon_melba_default",
    new Model_Languages
    {
        speaker = "{62}",
        EN = new string[]
        {
                @"��!",
        },
        
    }
},
{
    "urselks-saloon_melba_psychic_blocking",
    new Model_Languages
    {
        speaker = "{62}",
        EN = new string[]
        {
                @"Ba-ha! You want to get inside?| No chance, kid.",
                @"We're dealing with an infestation. It's a crisis! If we don't figure it out, we might all be out of work pretty soon here.",
        },
        
    }
},
{
    "urselks-saloon_melba_psychic_blocking1",
    new Model_Languages
    {
        speaker = "{62}",
        EN = new string[]
        {
                @"What're you still doing here, ba-ha!",
                @"We ain't open for business! We got an issue inside, you got that?",
        },
        
    }
},
{
    "urselks-saloon_melba_psychic_not-blocking",
    new Model_Languages
    {
        speaker = "{62}",
        EN = new string[]
        {
                @"Huh, what was I saying? Uguh, my head is throbbing!",
                @"Oh yeah that's right, we don't have time for dilly-dallying, kid!",
        },
        
    }
},
{
    "urselks-saloon_melba_eat-reaction",
    new Model_Languages
    {
        speaker = "{62}",
        EN = new string[]
        {
                @"(. ﾟーﾟ)",
        },
        
    }
},
{
    "urselks-saloon_melba_default_disabled-exit-reaction",
    new Model_Languages
    {
        speaker = "{62}",
        EN = new string[]
        {
                @"<size=16>���� ���!</size>",
        },
        
    }
},
{
    "urselks-saloon_melba_psychic_disabled-exit-reaction",
    new Model_Languages
    {
        speaker = "{62}",
        EN = new string[]
        {
                @"<size=16>Hey stop right there!</size>",
                @"What's your deal, kid? We're dealing with a major crisis inside!",
                @"We have an infestation! We're in no position to serve anyone, especially not an <b>outsider</b>, ba-ha!",
        },
        
    }
},
// ------------------------------------------------------------------
// Peche
{
    "urselks-saloon_peche_default",
    new Model_Languages
    {
        speaker = "{61}",
        EN = new string[]
        {
                @"���!",
        },
        
    }
},
{
    "urselks-saloon_peche_psychic",
    new Model_Languages
    {
        speaker = "{61}",
        EN = new string[]
        {
                @"Hey, bud, are you here to help us out or what?",
                @"Ha! I've never heard of an <b>outsider</b> be of any use to an {6} before!",
        },
        
    }
},
{
    "urselks-saloon_peche_psychic1",
    new Model_Languages
    {
        speaker = "{61}",
        EN = new string[]
        {
                @"You know why our <b>saloon</b> is the best?",
                @"It's because we stick to our roots, you got that, bud?",
                @"Our menu's been the same for centuries! Keep it simple and potent, that's our motto!",
        },
        
    }
},
{
    "urselks-saloon_peche_psychic2",
    new Model_Languages
    {
        speaker = "{61}",
        EN = new string[]
        {
                @"{62} and I have known each other since I can remember! Can't imagine doing anything without him.",
                @"Ah, I talk about {62} too much.",
                @"Hey buzz off already!",
        },
        
    }
},
{
    "urselks-saloon_peche_eat-reaction",
    new Model_Languages
    {
        speaker = "{61}",
        EN = new string[]
        {
                @"(. ﾟーﾟ)",
        },
        
    }
},
// ------------------------------------------------------------------
// Bar
{
    "urselks-saloon_bar_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Martini... 2 oz gin, 1/2 oz dry vermouth, shaken.",
        },
        
    }
},
{
    "urselks-saloon_bar_thought1",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Margarita... 2 oz tequila, 1 oz triple sec, 1 oz lime juice, shaken.",
        },
        
    }
},
{
    "urselks-saloon_bar_thought2",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Empty glasses... a sign that the time has passed.",
        },
        
    }
},
// ------------------------------------------------------------------
// KTV Room 2
{
    "ktv-room-2_growth_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"These pillars seem like they’re pulsing,",
        },
        
    }
},
{
    "ktv-room-2_rock_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Oh it’s a rock... and it’s really smooth...",
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
                @"An empty painting with a doormat.",
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
                @"“I cannot believe in Western sincerity because it is invisible, but in feudal times we believed that sincerity resides in our entrails, and if we needed to show our sincerity, we had to cut our bellies and take out our visible sincerity.”        – Yukio Mishima",
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
                @"Now would you like to enter me?",
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
                @"A painting with a doormat.",
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
                @"Are you, am I?",
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
                @"Now would you like to enter me?",
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
                @"This reminds me of a certain romantic painting.",
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
                @"Peering out over the raging seas!",
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
                @"Now would you like to enter me?",
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
                @"I've seen a painting like this somewhere.",
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
                @"An unfinished painting. Holding hands?",
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
                @"No, they're not holding hands. Someone is trying their hardest to pull someone else up.",
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
                @"*shiver*",
                @"This painting... it's not done.",
                @"There’s also a doormat in front of it, strange.",
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
                @"You know that feeling when you can’t get warm no matter how much you shiver?",
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
                @"A sketch of a thorny vine.",
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
                @"It's actually two vines weaving to be one.",
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
                @"I sometimes feel like I'm at the bottom of a well.",
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
                @"A labyrinth. There's always an entrance and an exit to a labyrinth, right?",
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
                @"Flowers are best when they're dried and hung.",
        },
        
    }
},
// ------------------------------------------------------------------
// Items
{
    "item-object_sticker_psychic-duck",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                @"You got the @@PsychicDuck @@Sticker_Bold!|
The @@Sticker_Bold contains the sealed spirit of a {13}.",
                @"@@Stickers_Bold allow you to inhabit the body of the mask's original owner.",
                @"Open your {32} and set it to {82} in the @@Stickers_Bold Screen.",
                @"Wear the @@Sticker_Bold with {82}. Press {82} again to return to your former self.",
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
    "item-object_sticker_animal-within",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                @"You got the @@AnimalWithin @@Sticker_Bold!|
Its original owner had a penchant for eating souls.",
                @"Use its {79} while wearing the @@Sticker_Bold to chomp through edible obstacles.",
                @"Wear the @@Sticker_Bold with {82}. Press {82} again to return to your former self.",
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
    "item-object_sticker_boar-needle",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                @"You got the @@BoarNeedle @@Sticker_Bold!|
Its original owner desired to see what was invisible.",
                @"The @@BoarNeedle @@Sticker_Bold allows you to enter paintings that have a doormat.",
                @"Wear the @@Sticker_Bold with {82}. Press {82} again to return to your former self.",
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
    "item-object_sticker_ice-spike",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                @"You got the @@IceSpike @@Sticker_Bold!|
Its original owner was said to have been caught in a snowstorm.",
                @"The @@IceSpike @@Sticker_Bold can summon a dark spike so powerful it can crack open just about anything.",
                @"Wear the @@Sticker_Bold with {82}. Press {82} again to return to your former self.",
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
    "item-object_sticker_melancholy-piano",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                @"You got the @@MelancholyPiano @@Sticker_Bold!|
Its original owner played a melancholic tune.",
                @"Use the @@MelancholyPiano @@Sticker_Bold to follow the chords of your heart to the next piano.",
                @"Wear the @@Sticker_Bold with {82}. Press {82} again to return to your former self.",
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
    "item-object_sticker_last-elevator",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                @"You returned to the conscious world wearing a mask.",
                @"You got the @@LastElevator @@Sticker_Bold!|
Not much is known of its original owner.",
                @"If you are ever <b>lost</b>, the @@LastElevator @@Sticker_Bold can be used anywhere inside {18} to take the {66} back to the {72}.",
                @"Wear the @@Sticker_Bold with {82}. Press {82} again to return to your former self.",
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
    "item-object_sticker_let-there-be-light",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                @"You got the @@LetThereBeLight @@Sticker_Bold!|
Its original owner constructed the lighting within {18}.",
                @"The @@LetThereBeLight @@Sticker_Bold will illuminate certain dark areas.",
                @"Wear the @@Sticker_Bold with {82}. Press {82} again to return to your former self.",
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
    "item-object_sticker_puppeteer",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                @"You got the @@Puppeteer @@Sticker_Bold!|
Its original owner spent each waking moment trying to bring life to handmade puppets.",
                @"Use the @@Puppeteer @@Sticker_Bold to control {73}.",
                @"Wear the @@Sticker_Bold with {82}. Press {82} again to return to your former self.",
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
    "item-object_usable_super-small-key",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                @"You found the @@SuperSmallKey!",
                @"It is made specifically for regular sized keyholes.",
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
    "item-object_collectible_last-well-map",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                @"You found the @@LastWellMap!",
                @"It seems to be a treasure map of sorts.",
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
    "item-object_collectible_last-spell-recipe-book",
    new Model_Languages
    {
        speaker = "",
        EN = new string[]
        {
                @"You found the @@LastSpellRecipeBook!",
                @"Does there have to be a last one?",
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

