// Last created by Dialogue Exporter at 2023-01-09 23:14:10

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
        public bool? autoNext;
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
                @"Strange, you can’t understand what they’re saying.",
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
                @"The piano chords echo...| but nothing happens...",
        },
        
    }
},
{
    "sticker-reaction_player_disabled-painting-entrance",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"You try to enter the canvas...| but nothing happens...",
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
                @"You and the <b>hotel</b> {8} had previously discussed that <b>tonight</b> would be your <b>last</b> shift.",
                @"It’s finally over at {49}.. Only one more hour...",
                @"If you can just make it until then...",
                @"Everything will be okay after that.",
                @"You’re sure of it.",
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
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "hotel-lobby_player-internal_repeating",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"You get the vague feeling of tracing something from the past...",
                @"What day is it today anyways?",
        },
        
    }
},
{
    "hotel-lobby_player-internal_hotel-lessons_reasons",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"You’ve never been afraid of the night.",
                @"No, it’s actually quite satisfying, settling into the routine.",
                @"It’s why you wanted this job in the first place, to <b>clear your mind</b>.",
                @"Sometimes you can even smell the sea from here...| So why leave it all?",
                @"...It’s pointless to think these things... <b>Tonight’s</b> your <b>last</b> night anyways...",
                @"Only one more hour until it’s finally {49}",
                @"If you can just make it until then...",
                @"Everything will be okay after that.",
                @"You’re sure of it.",
        },
        
    }
},
{
    "hotel-lobby_player-internal_hotel-lessons",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"It’s best to keep interpersonal relationships with hotel guests at a minimum.",
                @"Once their trip is over, you’ll have to say goodbye, and this happens over and over, again and again and again.",
                @"So better to keep things nice and dry.",
                @"You’ve become a master at this skill.",
        },
        
    }
},
{
    "hotel-lobby_player-internal_new-cycle_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"So close to something...",
                @"There’s been a change, you can feel it.",
                @"But what is it?",
                @"Something inside?",
                @"Okay no time to dwell on it, just one more hour!",
                @"Everything should be okay after...",
                @"You’re sure of it!",
        },
        
    }
},
{
    "hotel-lobby_player-internal_cant-swim_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"A draft of that unmistakable sea air again.",
                @"You find it a bit funny actually...",
                @"How the water calms you even though you never learned to swim.",
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
    "hotel-lobby_player-internal_world-1-done_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"......",
                @"Why are you even doing this?",
                @"Maybe you’re right.",
                @"Maybe it’s better this way.",
                @"For everyone.",
        },
        
    }
},
{
    "hotel-lobby_player-internal_world-2-done_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"You’ve always been dependable.",
                @"But who are you anyways?",
                @"I guess you aren’t human...",
                @"When will it all stop...",
        },
        
    }
},
{
    "hotel-lobby_player-internal_almost-done_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"I guess you’re really going to make me do it huh?",
        },
        
    }
},
{
    "hotel-lobby_player-internal_disabled-surveillance",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"There’s really no reason to stay at this point...",
                @"...",
                @"Is it already {49}?",
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
    "hotel-lobby_good-ending_exit_prompt",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Faint images of {19} fill your thoughts.",
                @"It feels like they still might need something from you?",
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
    "hotel-lobby_good-ending_exit_prompt_a",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Are you sure you want to leave?",
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
    "hotel-lobby_true-ending_exit_prompt",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Take the final step?",
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
// Narrator
{
    "pianos_narrator_piano_remembered",
    new Model_Languages
    {
        speaker = "NA",
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
                @"It’s ice...",
        },
        
    }
},
// ------------------------------------------------------------------
// Piano
{
    "pianos_nonkeys_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"The backside of the piano. Its surface is very shiny, no dust!",
        },
        
    }
},
// ------------------------------------------------------------------
// Treasure Chest
{
    "treasure-chest_default_locked",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Eh, it won’t budge...",
        },
        
    }
},
// ------------------------------------------------------------------
// Woods
{
    "woods_intro_well_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Alone with your thoughts again...",
        },
        
    }
},
{
    "woods_cursed-HMS-bent_default",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"Don’t worry, dear, he-he, go ahead and come in.",
        },
        
    }
},
{
    "woods_cursed-HMS-bent_psychic",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"Don’t worry, dear, he-he, go ahead and come in.",
                @"You and I, we’re not like the rest of them, he-he.",
        },
        
    }
},
{
    "woods_cursed-HMS-bent_eat-reaction",
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
    "woods_cursed-female_default",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"Why don’t you come inside, dear?",
        },
        
    }
},
{
    "woods_cursed-female_psychic",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"Why don’t you come inside, dear?",
                @"There’s no place like home, he-he-he.",
        },
        
    }
},
{
    "woods_cursed-female_eat-reaction",
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
// Ids
{
    "woods_ids_day-1_intro",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Baaaa-baaaa.",
        },
        
    }
},
{
    "woods_ids_day-2_intro",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Hey, {0} is it?",
        },
        
    }
},
{
    "woods_ids_day-2_intro1",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Ha, of course I can talk.",
        },
        
    }
},
{
    "woods_ids_day-2_intro2",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"The name’s {3}.",
        },
        
    }
},
{
    "woods_ids_day-2_intro3",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"It’s really not every day someone new comes in here, y’know?",
                @"Consider yourself special, kiddo!",
                @"Actually on the contrary, most are fleeing from good ol’ {18}.",
        },
        
    }
},
{
    "woods_ids_day-2_intro3_choices",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"So what brings you here, huh?",
        },
        
    }
},
{
    "woods_ids_day-2_intro3_a",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Well you know what they say?",
        },
        choiceText = "Not too sure myself.",
        
    }
},
{
    "woods_ids_day-2_intro3_b",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Ha-ha, kiddo, you got spunk! I like that!",
                @"But you know what they say?",
        },
        choiceText = "None of your business.",
        
    }
},
{
    "woods_ids_day-2_intro4",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"The ones who find this place <b>need to be here</b>.",
                @"...In other words...| <b>They’re summoned here.</b>",
                @"Why am <b><i>I</i></b> here then you ask?",
                @"Listen kiddo, <b>I have a hunch</b>.",
                @"Something <b>I’ve lost</b> is in here...",
                @"...And I can’t leave without it...",
                @"......",
                @"You know that tale about the boiling frog?",
                @"Of course they’ll jump out of boiling water just like you and me.",
                @"But you put ‘em in lukewarm water and raise the temperature slowly one degree at a time, and...",
                @"Yep, can’t sense the change at all.",
                @"And BOOM,| there you have it,| boiling f|-r|-o|-g.",
        },
        
    }
},
{
    "woods_ids_day-2_intro5",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Well, kiddo, that’s kinda how I <b>lost it</b>.",
                @"......",
                @"Alright alright, enough of this mushy stuff.",
        },
        
    }
},
{
    "woods_ids_day-2_intro6",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"You know... for you, me, the rest of ‘em... there’s not much time...",
                @"And speaking of time, I gotta run. See you around, {0}~",
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
                @"You’ve spent time researching the natural phenomenon known as {77}...",
                @"It was particularly important for seafarers... Since before this time, when looking out at sea, it’s just a black void.",
                @"According to your calculations here, today {77} should be at {49}.",
                @"Which is exactly when your shift <b>ends</b>...| is it fate?",
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
                @"Really would like to leave, but no... still one more hour...",
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
                @"The <b>hotel</b> {8} keeps a close eye on the staff.",
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
                @"You hate this feeling.",
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
                @"The day shift guy always forgets to rebrew the coffee when he gets off, so you end up doing it.",
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
                @"It’s an old photo with Dad.",
                @"He doesn’t smile in pictures, since he claims it looks too forced.",
        },
        
    }
},
{
    "hotel-lobby_player_fireplace_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"The gentle crackling soothes you.",
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
                @"You’ve read this a million times by now, sigh.",
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
                @"You enjoy poems and all, but you’ve always preferred novels.",
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
                @"Absolutely no clue why this is here...| it’s a fun read though.",
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
                @"An interesting compilation of short stories. Every time you read these, you can’t help feeling a bit sad but strangely happy.",
                @"Wait a second, there’s something inside.",
        },
        
    }
},
{
    "hotel-lobby_new-book-trigger_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"But first,| the book on the <b>coffee table</b>...| Don’t remember ever having a book like that here|.|.|.|",
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
    "hotel-lobby_new-book-interactable_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Someone left drawings inside arranged in a particular order.",
        },
        
    }
},
{
    "hotel-lobby_CCTV-disabled_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"The feeling of being watched is finally gone.",
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
                @"A painting of the mogul who originally funded construction of the hotel.",
                @"You’ve heard he lives a “double life” of sorts... but not too much is known about him.",
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
                @"You don’t really like thinking about this kind of stuff during your shift...",
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
                @"This isn’t the same portrait as in the Lobby’s Elevator Bay.| This man has a face.",
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
                @"It looks like he’s half-smiling, half-crying.",
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
                @"You should really get back to work.",
        },
        
    }
},
{
    "hotel-bay-v1_elevator_good-ending_disabled_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"There’s no use taking the elevator anymore.",
        },
        
    }
},
{
    "hotel-bay-v1_elevator_sunday_disabled_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"You don’t feel the need to take the elevator anymore.",
        },
        
    }
},
// ------------------------------------------------------------------
// Last Elevator
{
    "last-elevator_ids_first-day_default",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"� � � � � � � �!                                � � � � � � � �!",
        },
        
    }
},
{
    "last-elevator_ids_first-day_psychic",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Baaaa! Baaaaaaa!",
        },
        
    }
},
{
    "last-elevator_ids_big-ids_first-day_psychic",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Baa-baa-baa.",
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
                @"It’s a mirror. Look into it?",
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
                @"『 {18} 』",
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
                @"Outside?| You mustn’t worry about that, my dear. I assure you there’s no need for alarm, he-he.",
                @"Perhaps <b>you</b> could be seeing things again? Have you been up all night?",
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
                @"I am just a bit flustered is all. It is not every day that someone arrives at one’s residence unannounced.",
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
                @"<b>I am the owner of this fine mansion you see here.</b>",
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
        choiceText = "A sheep. It’s telling me to follow it.",
        
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
        choiceText = "I can’t sleep.",
        
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
                @"<size=16>C̵̻̏á̵ͅn̶̥͋ ̷̱̇y̸͇̚o̶̪͝ũ̴͕ ̴͙͋é̵̝v̸̡̚e̴̦̒ǹ̸͚ ̴̜̓t̵̜͌e̵̥͝l̵͇͛l̶̟̚ ̴̮͐u̵̾͜ș̶́ ̸͎̍a̵̮̓p̵͓͆ȃ̴̭r̷͔̕t̵͖̆ ̴͋ͅa̵̛̪n̵͍̓y̴̛ͅm̸̟̐o̶̬̓ȑ̶͉e̸͖̊?̶̧͗</size>",
                @"Why don’t we just keep it our little secret, everything you have witnessed down here? He-he...",
                @"If I may say so, the best solution for you is to find the nearest exit and get some rest, trust me, dear.",
                @"And please, refrain from returning.| It is for your own good.",
        },
        metadata = new Model_Languages.Metadata[]
        {
                null,
                null,
                null,
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                null,
                null,
                null,
        }
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
                @"Please have a wonderful rest of your night.",
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
                @"Only thing you hate about games is losing.",
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
                @"It’s a painting... close up it’s a lot less flat. Whoever painted it used a lot of paint.",
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
                @"Grrrrrr......",
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
// Halls
{
    "mirror-halls_paintings_on-entrance_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"These paintings are always so strangely nostalgic.",
                @"Like you’ve seen them before somewhere.",
        },
        
    }
},
{
    "mirror-halls_paintings_transformations_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"They used to tell you,",
                @"That when you draw yourself accurately, your true self-portrait,",
                @"Then you’d transform on the spot.",
        },
        
    }
},
{
    "mirror-halls_paintings_transformations_thought1",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"You figured it was just a dumb saying to make you practice more.",
        },
        
    }
},
{
    "mirror-halls_bao-zi_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"It smells a little sweet.",
        },
        
    }
},
// ------------------------------------------------------------------
// Mirror Hall
{
    "mirror-halls_paintings_family-portrait_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"It’s a portrait of a happy family.| Well actually they’re not smiling.",
        },
        
    }
},
{
    "mirror-halls_paintings_picnic_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"A child and mother having a picnic.",
                @"You flash back to the picnics with Mom when she was still around.",
        },
        
    }
},
{
    "mirror-halls_paintings_dead-mom_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Is this light broken?| This painting is impossible to make out...",
                @"Why hide something that took so long to make?",
        },
        
    }
},
{
    "mirror-halls_paintings_lovers_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Looks like they’re enjoying the sunrise.",
                @"It’s great and all, but it reminds you of your research of {77}, where the distinction between sea and sky becomes clear.",
        },
        
    }
},
{
    "mirror-halls_paintings_drill_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Can’t really see this painting clearly.",
                @"Perhaps it’s best to block out some things from the past?",
        },
        
    }
},
{
    "mirror-halls_paintings_go_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Go! A pastime enjoyable by all generations.",
                @"Looks a lot like your Go set from childhood days.",
        },
        
    }
},
{
    "mirror-halls_shattered-mirror_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"It's completely shattered.",
        },
        
    }
},
// ------------------------------------------------------------------
// Mirror & Lights Puzzle Room
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
{
    "mirror-halls_mirror-lights-puzzle-room_mirror_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"It’s you.",
                @"You look at yourself very closely and carefully.",
                @"Even mirrors sometimes betray you.",
        },
        
    }
},
{
    "mirror-halls_paintings_family-portrait_thought1",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"It’s the portrait of the family again.",
                @"Doing the same painting twice is a hard thing to do mentally.",
        },
        
    }
},
{
    "mirror-halls_paintings_picnic_thought1",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Another picnic... You try to focus your thoughts on something else...",
                @"Gingham.",
        },
        
    }
},
{
    "mirror-halls_paintings_dead-mom_thought1",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Her ribs are exposed. It’s hard to imagine painting such gore.",
                @"You’ve always had an aversion towards needles and the sight of internal organs.",
        },
        
    }
},
{
    "mirror-halls_paintings_lovers_thought1",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Before {77}, when looking out into the ocean like this, it’s just a black void.",
        },
        
    }
},
{
    "mirror-halls_paintings_drill_thought1",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"When seeing disturbing imagery, you’ve decided it’s best to stay as objective as possible.",
                @"Look, it’s a surgical drill.",
        },
        
    }
},
{
    "mirror-halls_paintings_go_thought1",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Go. A game with simple rules but endless possibilities.",
        },
        
    }
},
// ------------------------------------------------------------------
// Nightclub Line
{
    "nightclub-line_sign_read",
    new Model_Languages
    {
        speaker = "NA",
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
                @"Listen kiddo, I have a hunch.",
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
                @"Hey, what’s with that look on your face...",
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
                @"{10}? Oh, the {8}... I had a feeling you would.",
                @"...Figured it was only a matter of time... Been around here long enough to know these things...",
                @"......",
                @"Listen kiddo, <b>I have a hunch</b>.",
                @"Something tells me him and these {42}|.|.|.",
                @"Might somehow be...",
                @"...<b>connected</b>.",
                @"Then there’s this fact <i>you’re</i> involved now...",
                @"I know, this might all sound crazy but|.|.|.",
        },
        choiceText = "I met him, {10}.",
        metadata = new Model_Languages.Metadata[]
        {
                null,
                null,
                null,
                null,
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
                null,
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
                @"Oh it looked like you wanted to say something... nevermind then.",
                @"In any case...... this might sound crazy but|.|.|.",
        },
        choiceText = "(Don’t mention anything.)",
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
    "basement_ids_e-room_ddr-fail",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Wow...| you’re kinda terrible.",
                @"It’s okay though, I got faith in you, kiddo. No one’s a master at anything from the get-go.",
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
                @"Okay it’s settled...| I’ll just say it.",
                @"Listen closely kiddo,| I’m <b><i>|p|a|r|t| o|f| y|o|u</i></b>.",
                @"The thing is, I can’t be too sure though...| I guess it’s what they’d call a gut feeling?",
                @"But I’ve had this feeling for some time now| – like I was cast off from someone.",
                @"Not too sure how I even ended up here in the first place. Yep, quite the conundrum!",
                @"Actually, there’s a bit more to my theory. But oh before I forget...",
                @"You should be the one to use this. So here...",
        },
        metadata = new Model_Languages.Metadata[]
        {
                null,
                null,
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
                null,
                null,
                null,
                null,
                null,
        }
    }
},
{
    "basement_ids_try-again-prompt",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Not like I’m counting or anything, but that was attempt No. @@DDRCurrentTry. <b>Wanna try again?</b>",
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
// Ids' Room - Ids Weekend Day 1
{
    "basement_ids_n-room_weekend_day1_0",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Hey there, kiddo. It seems it’s been ages since I’ve seen ya ~",
                @"Well actually ages since I’ve seen anyone really...",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day1_1",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"My mind’s been in a jumble lately.",
                @"Can’t even seem to remember the thing I was first looking for in here.",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day1_2",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Anyways {0},| you know what scares me the most about being alone all the time is?",
                @"That I might even come to like the feeling.",
                @"Okay okay, I’ll stop with my blabbering!",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day1_3_choices",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"So what is it?| What brings you here, eh?",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day1_3_a",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Oh...",
                @"Strange of you to come all the way down here for that but ha, suit yourself.| It was fun while it lasted.",
        },
        choiceText = "Just passing by.",
        
    }
},
{
    "basement_ids_n-room_weekend_day1_3_a1",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Actually, I can’t stay for long either, I need some time to think over some things.",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day1_3_b",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Now?",
                @"Honestly, I’m really not in the mood <b>today</b>.",
                @"But hey thanks for asking, kiddo.",
        },
        choiceText = "Want to dance?",
        
    }
},
{
    "basement_ids_n-room_weekend_day1_3_b1",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Actually, I can’t stay for long, I need some time to think over some things.",
        },
        
    }
},
// ------------------------------------------------------------------
// Ids' Room - Ids Weekend Day 2
{
    "basement_ids_n-room_weekend_day2_0",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Hey there, kiddo. It seems it’s been ages since I’ve seen ya ~",
                @"Well actually ages since I’ve seen anyone really...",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day2_1_choices",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"So what is it?| What brings you here, eh?",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day2_1_a",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Oh...",
                @"Strange of you to come all the way down here for that but ha, suit yourself.| It was fun while it lasted.",
        },
        choiceText = "Just passing by.",
        
    }
},
{
    "basement_ids_n-room_weekend_day2_1_a1",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Actually, I can’t stay for long either, I need some time to think over some things.",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day2_1_b",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Now?",
                @"I’d really love to any other <b>day</b>.",
                @"Just got too many things on my mind <b>today</b>.",
                @"Everything’s all mixed up.",
        },
        choiceText = "Want to dance?",
        
    }
},
{
    "basement_ids_n-room_weekend_day2_1_b1",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"You might’ve already guessed it, but I’m not a native here either, y’know?",
                @"Yep, that’s right, {0}.| Used to be <i>out there</i>, outside these walls, just like you.",
                @"If I’m being honest with you, I really don’t belong here.",
                @"......",
                @"I’m pretty sure I was looking for something when I first came here...| but I have no idea what that <i>thing</i> is anymore now.",
                @"Ugh...",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day2_1_b2",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Even if I left here now,| there’d be nowhere to go.",
                @"I’ve been here so long, I’m actually kinda scared I won’t be able to function <i>out there</i>.",
                @"At least in here, I have a routine, I got a sense of things, y’know?",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day2_1_b3",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Hey it’s not like you gotta feel sorry for me or anything.",
                @"It just is what it is.",
                @"Actually all this talking has gotten me exhausted.",
                @"I’ll see you another time...",
                @"I need some time to think over some things...",
        },
        
    }
},
// ------------------------------------------------------------------
// Ids' Room - Ids Weekend Day 3
{
    "basement_ids_n-room_weekend_day3_0",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Hey there, kiddo. It seems it’s been ages since I’ve seen ya ~",
                @"Well actually ages since I’ve seen anyone really...",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day3_1_choices",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"What’s with the serious look? It looks like you might have something to say?",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day3_1_a",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Well y’know... I’ve been doing some thinking lately...",
                @"You remember my theory, right?",
                @"Long story short I really thought I was part of <i>you</i>.",
                @"That you’re my true owner...",
                @"But no, it can’t be you.",
                @"Or I wouldn’t still be stranded here...",
                @"See every time, I get so close!",
                @"But then it feels like I start over at nothing.",
                @"That’s right, kiddo, zero. Zilch.",
                @"What’s the point of all this anymore?",
        },
        choiceText = "No, it’s nothing.",
        
    }
},
{
    "basement_ids_n-room_weekend_day3_1_b",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Umm...",
                @"To be honest I really haven’t been in the mood lately.",
                @"If it was any other––",
        },
        choiceText = "Want to dance?",
        
    }
},
{
    "basement_ids_n-room_weekend_day3_1_b1",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Wait you look really insistent, ha.",
                @"What’s gotten into you?",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day3_1_b2",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Okay fine, I’ll do it for you then, {0}.",
                @"Just this one time.",
                @"Let’s go!",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day3_ids-dance",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Oh right, it’s me first, that’s how it goes...",
                @"Ha... it’s really been a while.",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day3_player-dance",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"You know why us {21} love dancing in the dark?",
                @"There’s just something so romantic about staring into complete blackness, while feeling our warm wool rubbing against one another...",
                @".|.|.|.|.|.",
        },
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
    "basement_ids_n-room_weekend_day3_success",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Wow, hey I’m actually kinda refreshed!",
                @"This feels really nostalgic for some reason, kiddo.",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day3_success1",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Well {0} I gotta run, but I’m sure I’ll be seeing you another time.",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day3_success2",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Oh that’s right, before I forget!",
                @"I’ve been meaning to give you this as a small present just to say...| thanks.",
                @"You’ve always been someone I could trust.",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day3_retalk_intro",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Hey, kiddo, what is it? You look really serious...",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day3_retalk_0_choices",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Well what is it hmm?",
        },
        
    }
},
{
    "basement_ids_n-room_weekend_day3_retalk_0_a",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Y’know down here I’ve had a lot of time to think.",
                @"Maybe it’s time I call it quits. Maybe I gotta just bite down real hard and tell myself, |s|t|o|p.",
                @"No one else can do it for me.",
                @"I could spend all my nights looking, and what if I <b><i>still</i></b> never find it.",
                @"Yep, that’d be something huh?",
        },
        choiceText = "No, it’s nothing.",
        
    }
},
{
    "basement_ids_n-room_weekend_day3_retalk_1_a",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Just gotta look on the bright side of things I guess.",
        },
        
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
                @"Two certificates of some sort... why is one hung higher than the other?",
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
                @"An isolated mansion with architecture of Eastern and Western influence.",
                @"Can’t help but get a sense of dread when looking at this painting.",
        },
        
    }
},
{
    "basement_n-room_wall-text_read",
    new Model_Languages
    {
        speaker = "{0}",
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
        speaker = "{0}",
        EN = new string[]
        {
                @"It’s a book about a tortoise with a soft shell…",
                @"“The soft-shelled tortoise met countless journeyers throughout the days…”",
                @"*flip* *flip* *flip*",
                @"“At last, the tortoise laid down to rest with a shell now made of a patchwork of faces.”",
        },
        
    }
},
{
    "mysterious-room_painting_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Pangs of dread fill you.",
                @"This painting has a doormat.",
        },
        
    }
},
// ------------------------------------------------------------------
// Player
{
    "ballroom_player_upper-level_self-portrait_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"You’ve always refused to draw your own self-portrait.",
                @"It’s not that you can’t do it... It’s just......",
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
                @"Howdy! The name’s Ursie.",
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
                @"Howdy! The name’s {33}.",
                @"Say partner, you look like you need a drink.| I can tell from all these years running the {35}.",
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
                @"To be frank,| some strange things have been happening at my <b>saloon</b>...| strangeness I can take... but my customers!",
                @"...So I’m here to speak to the {83} about it.",
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
                @"Hopefully the {83} takes action...| and fast.",
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
                @"The divine proportion. Spirals. The seasons. It all has meaning.",
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
                @"The divine proportion. Spirals. The seasons. It all has meaning.",
                @"Time. Relative in theory, absolute in practice. Waits for no one.",
                @"It’s no use. The {22}. It’s been decided. Precisely.",
        },
        
    }
},
{
    "ballroom_moose_psychic1",
    new Model_Languages
    {
        speaker = "{63}",
        EN = new string[]
        {
                @"I specialize in dealing with spells. Ice manipulation to be precise.",
                @"No outsiders means no more new spells for me.",
                @"I’ll have to focus on only the spells I know then.",
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
                @"They’re all just a bunch of shiteaters.",
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
                @"They’re all just a bunch of {60}.",
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
                @"Didn’t you hear? The ones left in here are slowly becoming <b>cursed</b>.",
                @"Why am I still here then?| It’s simple, I have no where else to go.",
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
                @"Who cares, they’re just a bunch of {60}.",
                @"Why would I want more {60} in here?",
                @"I’d rather just talk to the <b>wells</b>.",
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
                @"Well the only thing anyone ever talks about these days is the {22}.",
                @"Me?| What do I think about it?",
                @"That’s only something a {59} would answer.",
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
                @"Melba and I here are Kelsingør natives. Born and raised! Ha!",
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
                @"Yup, you tell’em, Peche!",
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
                @"{62} and I here are {18} natives. Born and raised! Ha!",
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
                @"Yup, you tell’em, {61}!",
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
                @"Ha! Good riddance,| I’m glad we’re going through with the {22}.",
                @"Look pal, you think we’re just gonna let these {42} keep comin’ in here? Ruin everything we’ve built?!",
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
                @"Get a load of that! No chance! I say lock our doors, we’ll handle those <b>intruders</b> in here.",
                @"Or rather... my fists will! Ba-ha-ha!",
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
                @"The ones who already fled?| Cowards! They don’t belong here to begin with!",
                @"They could never work in a <b>desert</b> like us! It’s hard alright, but hey, it’s honest work!",
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
                @"Right! A true {18} native sticks it out to the very end! Ba-ha!",
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
                @"It’s no secret we’re in a dire situation right now.",
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
                @"The {42}... They’ve invaded. Most folks have already fled...",
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
                @"No need for alarm! I am the prophet of {18}, {57}.",
                @"I’ll protect our residents at any cost... I swear by it!",
                @"Surely you’ve been informed, have you not?| It’s no secret, the circumstances we’ve found ourselves...",
                @"Intruders have invaded {18}...| The {42}.",
                @"Rumor has it they even have the power to consume the innocent.",
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
                @"Most of our citizens have fled already.",
                @"But alas, grieve not! I have a plan that will save {18}.",
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
                @"It came to me in a dream,| no,| a vision.| From the divine <b>other side</b>.",
                @"I have already made the proper preparations.",
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
                @"Mark my word!| I will return {18} to a time of peace!",
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
                @"The Sealing? We must, how else will we stop the Cursed Specters...?",
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
                @"The {22}? We must, how else will we stop the {42}...?",
                @"Yes, of course, some are worried...| They say we might lose touch with the outside world for eternity.",
                @"Still, it’s best we stay. I met {39} in here...| Ah yes, over at the <b>gardens</b>...| We were still young then...",
                @"When all’s said and done, I have faith in the {83}. Don’t you?",
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
                @"Other side? We can’t leave home, no. In the end, we have to stick with what we know.",
                @"We should wait until the {22} at {49} Everything will be worked out then.",
                @"Please have faith in the {83}!",
        },
        
    }
},
{
    "ballroom_kaffe_psychic2",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                @"The most important thing?| Of course it’s that I’m with {39}.",
                @"We just have to believe in the {83} to get us through these times is all.",
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
                @"Everyone always calls Kaffe “Iced Kaffe” because he’s soo calm.",
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
                @"Everyone always calls {38} “Iced {38}” because he’s soo calm.",
                @"He wouldn’t be so calm if he saw <b><i>them</i></b> with his own two eyes...",
                @"Yes, I saw <b><i>them</i></b>... It’s really scary actually, you know?",
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
                @"Look, <b><i>they</i></b> are in here too right? And we’re going to do the {22} and lock ourselves away with them inside?",
                @"Maybe I’m overthinking it...",
                @"The {83} has never let us down before, so let’s just go through with this, okay?",
        },
        
    }
},
{
    "ballroom_latte_psychic2",
    new Model_Languages
    {
        speaker = "{39}",
        EN = new string[]
        {
                @"Hey let’s take our mind off things. Do you like puzzles?",
                @"I really like them because they let you focus on something else. I especially like mazes.",
                @"It’s so simple really. Just one destination to reach, and you just gotta figure out how to get there.",
                @"Don’t you wish life was like that?",
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
                @"It’s useless to fight against fate.",
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
                @"It’s useless to fight against fate.",
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
                @"Blank canvases...| you remember taking portraiture painting classes when you were still very young. Bad memories.",
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
                @"There was a certain kind of thrill building up this thing and knowing you could destroy it all at once.",
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
                @"And even if you royally mess up, you can just paint over it.",
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
                @"A day in the life of a cat.",
                @"You’ve always wondered what life would be like as a cat.",
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
                @"Flan here at your humble service!",
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
                @"Stop! {17} here at your humble service. No <b>outsiders</b> at this hour...",
                @"Oh, you are able to understand me!| Forgive me!",
                @"The older one’s room is just <b>down this hall</b>, and the younger one’s is through this <b>north door<b>.",
                @"Safe travels, young one. I am here to serve.",
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
                @"I’m terribly sorry to inform you that we do not allow visitors.",
                @"The sisters would like privacy until {49} comes...",
                @"My deepest apologies, I am here to serve.",
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
{
    "urselks-hall_hint_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Hmm... You wonder if the <b>older sister</b> might have had more to say...",
        },
        
    }
},
{
    "urselks-hall_ellenia-hurt_disabled-exit_reaction",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"It’s locked shut...",
                @"You hear heavy breathing...",
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
                @"A painting of a cat jumping onto a bed!",
                @"One of your past favorite artists actually did a very similar piece.",
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
                @"U-um, I think it’d be better if you left.",
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
                @"U-um, I think it’d be better if you left.",
                @"Please, it’s better you don’t speak to me again...",
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
                @"Why... Why do you still want to talk?",
                @"It’s not a good idea sorry, please leave it at that...",
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
                @"This place, I know it’s dangerous... For the mind especially it seems...",
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
                @"Ahem, well you see, that wouldn’t solve anything...",
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
                @"To tell the truth, it’s not very clear to me anymore...",
                @"I suppose it started about the same time when <b><i>they</i></b> first arrived...",
                @"It’s a little hard to put into words... But it makes me feel like I don’t have too long to take each breath, so sometimes I catch myself only taking these short breaths...",
                @"Why am I even telling you all this...?",
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
                @"......",
                @"Hey by the way... Y-you aren’t a {13}, are you...?",
                @"But it seems to me, you can still understand much of what I’m saying.",
                @"It’s really been a long time since a non-{13} could communicate with us.",
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
                @"My name is {11}.",
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
                @"My name is {11}.",
                @"I actually overheard you speaking earlier with my little sister {12}.",
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
                @"She’s always been a bit stubborn...",
                @"Oh no, um... I don’t mean that in a bad sense or anything.",
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
                @"It’s just something tells me that... how should I put this...?",
                @"On her current path...| she won’t ever <i>find</i> the image of the {8}.",
                @"In fact, it’s all a thing of fables...",
                @"...No one’s ever really seen the {8}...",
        },
        
    }
},
{
    "eileens-room_eileen_psychic2_b_a_ab_a_a_choices",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                @"It’s something she’s been doing ever since I can remember... but I mean... there’s just something <i>off</i> these days...",
        },
        
    }
},
{
    "eileens-room_eileen_psychic2_b_a_ab_a_a_a",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                @"Well, it’s just that...",
                @"...Sometimes when painters lose confidence they’ll try to plan out every detail of the final piece, all the lines, shapes, all of it...",
                @"It’s completely fine, I mean when you already know what your final piece is supposed to look like.",
                @"But I-I’ve been thinking... what if your subject has no physical form?",
                @"What if what she’s really trying to paint is|.|.|.|| <b><i>{14}</b></i>.",
                @"Sigh... What am I even saying...",
        },
        choiceText = "What do you mean...?",
        metadata = new Model_Languages.Metadata[]
        {
                null,
                null,
                null,
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
    "eileens-room_eileen_psychic2_b_a_ab_a_a_b",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                @"I-I’ve been known to do that...",
                @"But look... sometimes when painters lose confidence they’ll try to grasp onto a past image they’re familiar with, perhaps by retracing their steps or copying old references.",
                @"It’s completely fine, I mean when you already know what your final piece is supposed to look like.",
                @"But suppose if... your subject never seizes to stop changing forms?",
                @"What if what she’s really trying to paint is|.|.|.|| <b><i>{14}</b></i>.",
                @"Sigh... Maybe you’re right, what am I even saying...",
        },
        choiceText = "Could this all be in your head?",
        metadata = new Model_Languages.Metadata[]
        {
                null,
                null,
                null,
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
                @"I think she needs to hear it from...| from someone else.",
                @"That she’s in fact painting| <b><i>{14}</b></i>.",
                @"She would never listen to me though...",
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
                @"You’re still here?| Not many are left here these days.",
                @"Well, I believe, if {12} truly wants to capture the {8}...",
                @"...she needs to first fully understand...| <b><i>{14}</b></i>.",
                @"I’m far too exhausted just thinking about all this.",
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
                @"I always think, in a way, she’s giving form to| <b><i>{14}</b></i>.",
                @"Us two aren’t really on speaking terms anymore. I guess perhaps she...",
                @"Just wants to be the exact opposite of me.",
        },
        
    }
},
{
    "eileens-room_eileen_psychic3_talked",
    new Model_Languages
    {
        speaker = "{11}",
        EN = new string[]
        {
                @"In my opinion, the most difficult thing... would be to paint myself.",
                @"I mean, it’s just the thought of picking myself apart...",
                @"...and then putting all of it on a canvas for everyone to pick apart again...",
        },
        
    }
},
{
    "eileens-room_eileen_psychic_ellenia-hurt",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"She seems to be in a deep sleep...",
                @"How long has she been like this for?",
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
{
    "eileens-room_painting_snow-woman_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Legend has it, there was once a woman living in a mountain village whose partner goes missing in a snowstorm.",
                @"She tirelessly searches the mountainside for him through the ongoing blizzard.",
        },
        
    }
},
{
    "eileens-room_painting_snow-woman_thought1",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"With her last breath, she makes it to a clearing to see a cabin in the distance.",
                @"They say whatever she saw there turned her into the spirit of the snowstorm – a spirit that traps the wicked and spares the good.",
        },
        
    }
},
// ------------------------------------------------------------------
// Ellenia's Room
{
    "ellenias-room_bookshelf_lore-book",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"It’s a journal...| skim through it?",
        },
        
    }
},
{
    "ellenias-room_bookshelf_lore-book_a_a",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"There seems to be just a few entries.",
        },
        
    }
},
{
    "ellenias-room_bookshelf_lore-book_a_a_a",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"“Dear Journal,”",
                @"“Tonight, I’m going to work on my painting! Remember, don’t paint yourself into a corner this time.”",
                @"“Tonight, I’m going to work on my painting! Remember, outside is where the danger is.”",
                @"“Tonight, I’m going to work on my painting! Remember, don’t get stuck in the–”",
        },
        
    }
},
{
    "ellenias-room_bookshelf_lore-book_a_a_a_a",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"The rest is ripped.",
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
                @"Skim through this magazine purposefully shoved into the dark corner of this bookshelf?",
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
                @"Maybe you weren’t supposed to see that...",
        },
        
    }
},
{
    "ellenias-room_easle_angry",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"You can’t seem to get a clear view from here...",
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
                @"A portrait of a woman in front of a maze?",
                @"You can tell a lot of time was put into this.",
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
                @"A portrait of a man in front of a maze?",
                @"You can tell a lot of time was put into this.",
        },
        
    }
},
{
    "ellenias-room_bookshelf_textbook",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"A textbook about ghosts.",
                @"You’d like to read through this actually, but now’s probably not a good time.",
                @"You often lose track of time when getting into something you find really interesting.",
        },
        
    }
},
{
    "ellenias-room_bed_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"You used to have bedsheets just like these.",
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
                @"Hey! Who do you think you are!?",
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
                @"Whoa you can talk to me?| But you’re obviously not a {13} and definitely not from {18}.",
                @"Wait,| have you not heard of me?! Sheesh... <b>outsiders</b> can be pretty uncultured these days...",
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
                @"I’m part of a long line of {18} writers and illustrators.",
                @"...But you know, one day <i><b>I’m</b></i> going to paint a portrait that’ll <i><b>really</b></i> make a name for us.",
                @"They’ll fall to their <b>knees</b> when they see this, ha! You just watch!",
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
                @"Well, the reason. Ha! Isn’t it obvious?!",
                @"We always want to paint what we can’t see!",
                @"Simpletons like you wouldn’t understand, okay?",
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
                @"No... actually, I haven’t. But no one has, you got that?!",
                @"The point of painting is to paint the things you can’t see!",
                @"Simpletons like you wouldn’t understand, okay?",
        },
        choiceText = "You’ve seen the {8}?",
        
    }
},
{
    "ellenias-room_ellenia_psychic_problem",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"Anyways, are you seeing this? It’s coming along nicely, right, isn’t it?",
                @"It’s taking a little longer than I planned though......",
                @"Hey I’ll figure it out though, alright?! Forget I said anything! Hmph!",
        },
        
    }
},
{
    "ellenias-room_ellenia_psychic_masked-one_ab_a",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"Okay enough small talk!",
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
                @"Yes, it’s all coming together now!",
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
                @"So actually, I’ve been working on this new one for a while now.",
                @"What do you think?",
                @"C’mon already! Give me one word that describes its essence!",
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
                @"After all these years, nobody really...",
                @"I can’t believe it... but now <b><i>you</i></b>...",
                @"<size=16><b>YOU.</b></size>",
                @"<size=16><b>An outsider...</b></size>",
                @"......",
                @"...Every decision would just lead me to a dead end...",
                @"Hmph, just painting the same thing over and over.",
                @"Admit I needed help? Ha, please...",
                @"Like anyone in this place would ever give me the time of day...",
                @"Hey, look, I didn’t really have a choice either...",
                @"It’s just me, myself, and I in here! That’s it.",
                @"......",
                @"Anyways, it’s pretty clear now.",
                @"I’ve scraped all that I can out of myself already... <b>There’s nothing else left for me here</b>,| nope.",
                @"It’s time I leave for good.",
                @"......",
                @"Hey you know what? I owe you one... Where I’m going I’m not going to need this anymore... So here take it, it’s yours...",
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
                @"It’s not like I care if you use it or not...",
                @"Okay enough small talk! It’s time for me to make a name for {18} once and for all!",
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
                @"Oh and last thing,| that <b>painting on the easel</b> is a work in progress, you got that?",
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
    "ellenias-room_ellenia_psychic_question_repeat_initial",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"Have we met before? What’re you doing getting so close to my painting?!",
                @"Hmph, I don’t usually take opinions from <b>outsiders</b>, but hey, I’ll be nice.",
                @"Well, c’mon now, what do you think about it?",
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
                @"After all these years, nobody really...",
                @"I can’t believe it... but now <b><i>you</i></b>...",
                @"<size=16><b>YOU.</b></size>",
                @"<size=16><b>An outsider...</b></size>",
                @"......",
                @"...Every decision would just lead me to a dead end...",
                @"Hmph, just painting the same thing over and over.",
                @"Admit I needed help? Ha, please...",
                @"Like anyone in this place would ever give me the time of day...",
                @"Hey, look, I didn’t really have a choice either...",
                @"It’s just me, myself, and I in here! That’s it.",
                @"......",
                @"Anyways, it’s pretty clear now.",
                @"I know what I have to do.",
                @"It’s time I leave for good.",
                @"I think I’ve scraped all that I can out of myself already. There’s nothing more for me here, nope.",
                @"......",
                @"Hey you know what? I owe you one... Where I’m going I’m not going to need this anyways... So here take it, it’s yours...",
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
{
    "ellenias-room_ellenia_first-success",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"The memory of when you first arrived here... it’s clearer now.",
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
                @"After all these years, nobody really...",
                @"I can’t believe it... but now <b><i>you</i></b>...",
                @"<size=16><b>YOU.</b></size>",
                @"<size=16><b>An outsider...</b></size>",
                @"......",
                @"...Every decision would just lead me to a dead end...",
                @"Hmph, just painting the same thing over and over.",
                @"Admit I needed help? Ha, please...",
                @"Like anyone in this place would ever give me the time of day...",
                @"Hey, look, I didn’t really have a choice either...",
                @"It’s just me, myself, and I in here! That’s it.",
                @"......",
                @"Anyways, it’s pretty clear now.",
                @"I know what I have to do.",
                @"It’s time I leave for good.",
                @"I think I’ve scraped all that I can out of myself already. There’s nothing more for me here, nope.",
                @"......",
                @"Hey you know what? I owe you one... Where I’m going I’m not going to need this anyways... So here take it, it’s yours...",
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
    "ellenias-room_ellenia-weekend_psychic_hurt_default",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"What’s wrong with me?",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_alone0",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"Ugh it’s getting on my nerves!",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_alone1",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"I can’t remember that face anymore...",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_alone2",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"It’s all way too fuzzy.",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_alone3",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"You know I’m not normally into this sentimental crap.",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_alone4",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"It’s just there’s something they told me about my work...",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_alone5",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"...That really stayed with me...ha, I know this sounds dumb.",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_alone6",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"But actually all my new paintings have been about it...",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_alone7",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"Here, I don’t care, take a look if you want.",
                @"Go ahead already!",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_choices_a_rewind0",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"I’ve been working on this new one for a while now.",
                @"C’mon now! What do you think?",
                @"Give me one word that describes its essence!",
                @"Ha... you pleb, what do you know...",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    autoNext = true, 
                },
                new Model_Languages.Metadata
                {
                    autoNext = true, 
                },
                new Model_Languages.Metadata
                {
                    autoNext = true, 
                },
                new Model_Languages.Metadata
                {
                    autoNext = true, 
                },
        }
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_painting-comment",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"Y̶̦̐o̸͔̍u̵͔̾ ̸̦̌l̷͍̿i̶̟͘k̸̡̐e̵̛͎ ̶͓̾ḭ̶͠t̷̖͗ ̷̺̃r̵͈͛i̵͇͊g̵̭͛h̴̯͑t̸̝̓?̸̟̕",
                @"Obviously it’s about being alone...hehehe.",
                @"I’d say it’s even a bit autobiographical.",
                @"You can relate too, I’m sure, right!?",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_hesitate0",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"I know I know, I’m not truly alone...",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_hesitate1",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"I’ll still always have {11}.",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_hesitate2",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"......",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_hesitate3",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"But you know with her condition and all...",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_hesitate4",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"...Maybe I shouldn’t even be saying this.",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_hesitate5",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"Just between me and you though...",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_hesitate6",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"......",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_choices_a_choices_a1_0",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"She’s probably just being dramatic.",
                @"I mean the spikes and all? C’mon...",
                @"You know, pulling the sympathy card?",
                @"Like if I’m going to be honest......",
        },
        
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_choices_a_choices_a1_1",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"<size=14>All she ever wants is fucking attention. It makes me sick.</size>",
                @"<size=16>Look at her always playing the victim.</size>",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    autoNext = true, 
                },
                new Model_Languages.Metadata
                {
                    autoNext = true, 
                },
        }
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_dissociation",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"<size=18>Why have <b>you</b> been talking to <b>ourselves</b>?</size>",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    autoNext = true, 
                },
        }
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_choices_a_choices_a2",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"<size=20>HONESTLY THE WORLD’S BETTER OFF–</size>",
                @"<size=20><b>SHE HAS NO IDEA WHAT IT MEANS TO| FEEL| REAL|| P||A||I||N.|||</b></size>",
        },
        metadata = new Model_Languages.Metadata[]
        {
                new Model_Languages.Metadata
                {
                    autoNext = true, 
                },
                new Model_Languages.Metadata
                {
                    autoNext = true, 
                },
        }
    }
},
{
    "ellenias-room_ellenia-weekend_psychic_hurt_choices_a_choices_a3",
    new Model_Languages
    {
        speaker = "{12}",
        EN = new string[]
        {
                @"I... I think I’m going to puke.",
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
                @"I am trying to be of assistance to you...| but how can I assist you when you’re wandering around like this?",
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
{
    "eileens-mind_repeat-drama_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Just|.|.|.| breathe|.|.|.",
        },
        
    }
},
// ------------------------------------------------------------------
// Grand Mirror Room
{
    "grand-mirror-room_finale_entrance_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Everything is mixed up.",
                @"You can’t seem to stop switching between your past selves.",
        },
        
    }
},
{
    "grand-mirror-room_finale_on-pushback-done",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Hey, what are you waiting for?!",
        },
        
    }
},
// ------------------------------------------------------------------
// Interactables
{
    "grand-mirror-room_go-table_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"You won’t go down without a fight.",
                @"There’s always a move you can make.",
                @"Something like a blizzard whirls inside of you.",
        },
        
    }
},
{
    "grand-mirror-room_bookshelf_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"The words feel close to you.",
        },
        
    }
},
// ------------------------------------------------------------------
// Cut Scenes
{
    "grand-mirror-room_grand-mirror_responsibility",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                @"You’ve come to visit me all the way down here?| How very nice of you.",
                @"It seems you are proving your worth, my dear.",
                @"And as a result, I’m beginning to trust you.|.|.| So I’m here to give you some greater responsibilities.",
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
                @"Because it’s the only way you can get out from down here...",
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
                @"Also...| You’ll be needing this for the rest of your journey| in case you ever get| <b>l|o|s|t|</b>.",
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
                @"Why am I being so nice to you?| Well it’s simple, my dear.| You might be of some use to me.",
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
{
    "grand-mirror-room_ids_baa",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Baaa! You don’t need those masks anymore, kiddo.",
                @"You don’t need me anymore, baaaaa!",
        },
        
    }
},
{
    "grand-mirror-room_ids_baa1",
    new Model_Languages
    {
        speaker = "{3}",
        EN = new string[]
        {
                @"Baaaaaa! Baa!",
        },
        
    }
},
{
    "grand-mirror-room_myne_gaslight",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                @"<b>You came looking for me after all.</b>",
                @"And it appears you’ve expelled the last of my residents...",
                @"...",
                @"It’s just you and me that remain then.",
                @"You know, my dear, it should never have come to this.",
                @"He-he...",
                @"You might think you despise me at this point...",
                @"Perhaps you think I’m nothing more than a monster...",
                @"On the contrary, dear, the truth is far from it...",
                @"He-he-he...",
                @"......",
                @"You see...",
                @"I was created...",
                @"...in your very own vision...",
                @"...created for you to finally see your True Self.",
                @"To be|| your mirror.",
                @"In other words|.|.|.|",
                @"<size=16><b>I <i>|a|m</i> ||y|o|u|.</b></size>",
                @"{0}...",
                @"So now allow me to ask you this...",
                @"How can you hate your very own ideal?",
                @"How can you despise the only one who can help| “find you”?",
                @"......",
                @"<b>How can you hate me?</b>",
                @"When my one true purpose...",
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
                new Model_Languages.Metadata
                {
                    isUnskippable = true, 
                },
        }
    }
},
{
    "grand-mirror-room_myne_gaslight1",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                @"<size=16>When what I’m really here for|.|.|.|</size>",
                @"<size=16>Is to save you from|.|.|.|</size>",
                @"<size=16>|||<b>Your own fate at {49}</b></size>",
                @"<size=16>......</size>",
                @"<size=16>At that moment...</size>",
                @"<size=16>I know exactly what will happen to us...</size>",
                @"<size=16>You,| me...</size>",
                @"<size=16>All this we’ve built...</size>",
                @"<size=16>Together...</size>",
                @"<size=16>So please,| let’s quit these petty little games...</size>",
                @"<size=16>Will you return to the hotel?</size>",
                @"<size=16>And put all this behind you?</size>",
                @"<size=16>......</size>",
                @"<size=16>You see...</size>",
                @"<size=16>This cycle is <i>our</i> only way...</size>",
                @"<size=16><b>...to survive.</b></size>",
                @"<size=16>|.|.|.|</size>",
                @"<size=16><b>I too have run out of choices.</b></size>",
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
    "grand-mirror-room_player_lead_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"No|.|.|.|",
                @".|.|.|",
                @"You’re wrong.",
                @"The truth about this place.",
                @"<b>Its residents... its paintings...</b>",
                @"<b>|None| of| it| is| yours.</b>",
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
    "grand-mirror-room_player_lead_thought1",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"<b>You’re hiding.</b>",
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
    "grand-mirror-room_player_lead_thought2",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"<b>There is no other side. This is it, can’t you see?</b>",
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
    "grand-mirror-room_player_lead_thought3",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"<b>Why have you been hiding for so long?</b>",
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
    "grand-mirror-room_myne_reaction",
    new Model_Languages
    {
        speaker = "{10}",
        EN = new string[]
        {
                @"Dear.",
                @"What exactly is it you are trying to do?",
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
// Player
{
    "wells-world_snow-reaction_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"It’s beautiful – the snow – but it makes you feel even colder.",
                @"It reminds you of the warmth from the fireplace back at the hotel.",
                @"When things were safe...",
        },
        
    }
},
// ------------------------------------------------------------------
// Interactables
{
    "wells-world_flower_spring_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"A budding flower.",
        },
        
    }
},
{
    "wells-world_flower_summer_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"A sunflower. They say these always face the sun.",
        },
        
    }
},
{
    "wells-world_flower_fall_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"All that’s left of this is a dry branch.",
        },
        
    }
},
{
    "wells-world_fireplace_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"What’s a fireplace doing in a place like this?",
        },
        
    }
},
{
    "wells-world_fireplace_fire_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"The fire is burning steadily.",
        },
        
    }
},
{
    "wells-world_tombstone_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"The air becomes still.",
                @"The words are blurred.",
        },
        
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
                @"The divine proportion. Spirals. The seasons.",
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
                @"The divine proportion. Spirals. The seasons. It all has meaning.",
                @"The <b>Last Spell</b>. Yes. It will connect it all!",
                @"Wells. Laid out in a logical way. Precisely.",
                @"They help my mind. They’ll help yours too. If you let them.",
                @"Shouldn’t leave here until I’ve learned this final spell. No.",
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
                @"You do not understand. Hard for anyone to understand.",
                @"The <b>Last Spell</b>. Can’t be wasting my precious time with loafers like you.",
                @"So close. Precisely.",
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
                @"You! The <b>Last Spell</b> can be derived from this!",
                @"Precisely!",
                @"Enough loafing! This should be it!",
                @"Here, I know I’ve been distracted. Perhaps even obsessed. Unacceptable possibly.",
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
                @"And let us see about this spell... Ah this technique! Why of course! I can understand it!",
                @"Custom cryokinesis! Derivations via fractalization! Impressive work!",
                @"Can it be... I...| <b>I’ve got it!</b>",
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
                @"You do not understand. Hard for anyone to understand.",
                @"The <b>Last Spell</b>. Can’t be wasting my precious time with loafers like you.",
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
                @"A dark place.",
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
                @"It’s all I’ve ever known.",
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
                @"You’ll be just like me, dear.",
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
                @"He-he, you’re just like me! He-he.",
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
                @"Don’t be afraid dear, you’ll become just like me.",
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
                @"He-he, we’re the same can’t you see?? He-he.",
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
                @"Ugo-ohhhhhhhhhh!",
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
// Celestial Gardens
{
    "celestial-gardens_entrance_player_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"......",
                @"The hotel’s day-to-day routine...",
                @"Maybe it’s still better...",
                @"Than being lost at sea...",
                @"Rather not talk about it.",
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
                @"If it’s a rose it will bloom, if it’s a leaf it will fall...",
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
                @"Well, thanks for listening, I’ll probably be heading back sooner or later.",
                @"Just give me a sec.",
        },
        
    }
},
{
    "rock-garden_tree_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"The sight of cherry blossoms calms you.",
        },
        
    }
},
// ------------------------------------------------------------------
//     Catwalk 2
{
    "catwalk2_outhouse_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"These look like outhouses...",
                @"There’s no time for a restroom break!",
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
                @"Hand puppets...| This room actually reminds you a bit of a theatre stage.",
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
                @"We would always meet at the courtyard in the <b>center</b> of this garden ever since we were little.",
                @"Ugh, usually I know which turns to take, but it’s completely left my memory ever since <i><b>they</b></i> started coming in here.",
                @"Honestly, I’m ready to leave this place for good...| but I can’t leave without {38}, no...",
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
                @"“Meet me under the <b>garden gazebo</b>.” That’s what {39} would always tell me.",
                @"How could I have lost my way like this?| I’m not sure how much longer I can go on without {39}.",
                @"This was never this much of a maze before! My mind is a haze... but I know. I must have faith... Calm down, you can do this.",
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
                @"No, I can’t go any further, but I must find {39}.",
                @"No, it’s much too dangerous on the other side.",
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
                @"I’m not ready to pass through here though. Who knows what could be on the other side?",
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
    "garden-labyrinth_latte_success_a",
    new Model_Languages
    {
        speaker = "{39}",
        EN = new string[]
        {
                @"<size=16>{38} you’re here!</size>",
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
    "garden-labyrinth_kaffe_success_a_a",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                @"I-I was worried sick...| When did this become such a maze? What happened to us!?",
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
    "garden-labyrinth_latte_success_a_a_a",
    new Model_Languages
    {
        speaker = "{39}",
        EN = new string[]
        {
                @"I’m not too sure myself...| it’s all a blur...| It’s like I woke up and next thing I knew...",
                @"...",
                @"...Hey|.|.|.| but you know... all this has me thinking now...",
                @"When we were still young...| we made that promise... didn’t we?",
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
                null,
        }
    }
},
{
    "garden-labyrinth_kaffe_success_a_a_a_a",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                @"Yes...| yes. I won’t ever forget it.",
                @"|.|.|.|",
                @"Okay!| Then it’s settled!",
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
    "garden-labyrinth_latte_success_a_a_a_a_a",
    new Model_Languages
    {
        speaker = "{39}",
        EN = new string[]
        {
                @"Wait {38}, do you mean you can...| You’re really fine with...",
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
    "garden-labyrinth_kaffe_success_a_a_a_a_a_a",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                @"I mean it, {39}.| You’re right, we’ll never see the ocean from here...",
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
    "garden-labyrinth_kaffe_default",
    new Model_Languages
    {
        speaker = "{38}",
        EN = new string[]
        {
                @"Meet me under the garden gazebo.",
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
                @"It was in the <b>center</b> of all this mess, but I’ve lost my way. I’ll never find {39} at this rate.",
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
                @"What’s the point of it all without {39}.",
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
                @"We always used to meet at the courtyard...",
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
                @"Usually I know which turns to take, but it’s completely left my memory ever since <i><b>they</b></i> started coming in here.| Kinda sad how fast my memory fades now, ha.",
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
                @"Really strange things have been happening in here lately. If I can be honest with you, I’m ready to leave this place as soon as I can.",
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
    "xxx-world_entrance_player_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"There’s no way you’re thinking about <i>that</i> again.",
                @"That voice! Annoying!",
                @"Keep it together, {0}!",
                @"Solid land!",
        },
        
    }
},
{
    "xxx-world_stage_player_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Wait what is this place?",
                @"You wouldn’t know, would you?",
                @"You aren’t going to be much use anymore, are you?",
        },
        
    }
},
{
    "xxx-world_wandering-cursed-one_default",
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
{
    "xxx-world_puppet_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Hm, a hand puppet.",
        },
        
    }
},
{
    "xxx-world_cursed-myne_long-trail_default",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"It’s a long path to where I’m going.",
        },
        
    }
},
{
    "xxx-world_cursed-myne_long-trail_psychic",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"It’s a long path to where I’m going.",
                @"Will you join me?",
                @"I’m sure you’ll end up joining me sooner or later, you’ll see, heh.",
        },
        
    }
},
{
    "xxx-world_cursed-myne_long-trail_eat-reaction",
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
    "xxx-world_cursed-HMS_ruins-NW_default",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"Careless traveler... It’s only ruins at this point.",
        },
        
    }
},
{
    "xxx-world_cursed-HMS_ruins-NW_psychic",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"Careless traveler... It’s only ruins at this point.",
                @"You know, dear, you remind me of myself in my youth, he-he-he.",
        },
        
    }
},
{
    "xxx-world_cursed-HMS_ruins-NW_eat-reaction",
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
    "xxx-world_cursed-female_ruins-NE_default",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"These ruins once were a grand place for gatherings.",
        },
        
    }
},
{
    "xxx-world_cursed-female_ruins-NE_psychic",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"These ruins once were a grand place for gatherings.",
                @"It’ll return to that way soon. You’ll be there with me for that, right? He-he.",
                @"Of course you will.",
        },
        
    }
},
{
    "xxx-world_cursed-female_ruins-NE_eat-reaction",
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
    "xxx-world_cursed-female_hz-trail_default",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"Why is it you’re trying so hard not to be like me, dear?",
        },
        
    }
},
{
    "xxx-world_cursed-female_hz-trail_psychic",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"Why is it you’re trying so hard not to be like me, dear?",
                @"Can’t you see we’re the same? He-he.",
        },
        
    }
},
{
    "xxx-world_cursed-female_hz-trail_eat-reaction",
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
    "xxx-world_cursed-HMS_totem-SW_default",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"He-he. I know you’ve been hiding something.",
        },
        
    }
},
{
    "xxx-world_cursed-HMS_totem-SW_psychic",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"He-he. I know you’ve been hiding something.",
                @"You can’t hide from yourself though! He-he!",
        },
        
    }
},
{
    "xxx-world_cursed-HMS_totem-SW_eat-reaction",
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
    "xxx-world_cursed-female_totem-SE_default",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"The sand makes it difficult to find your way.",
        },
        
    }
},
{
    "xxx-world_cursed-female_totem-SE_psychic",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"The sand makes it difficult to find your way.",
                @"Are you lost?",
                @"I’m lost too...",
        },
        
    }
},
{
    "xxx-world_cursed-female_totem-SE_eat-reaction",
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
    "xxx-world_cursed-myne_totem-N_default",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"You think you can just forget about me?",
        },
        
    }
},
{
    "xxx-world_cursed-myne_totem-N_psychic",
    new Model_Languages
    {
        speaker = "{74}",
        EN = new string[]
        {
                @"You think you can just forget about me?",
                @"Foolish traveler!",
        },
        
    }
},
{
    "xxx-world_cursed-myne_totem-N_eat-reaction",
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
    "xxx-world_flower-E_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"An innocent kind of flower.",
        },
        
    }
},
{
    "xxx-world_flower-E_saloon-entrance_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"It’s managed to grow out from a crack in the ground.",
        },
        
    }
},
{
    "xxx-world_flower-N_stage_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"A flower of sin.",
        },
        
    }
},
{
    "xxx-world_flower-S_dance-floor_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Why plant flowers here?",
        },
        
    }
},
{
    "xxx-world_polyhedron_thought",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Doesn’t this remind you of that Dürer engraving you used to like?",
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
                @"Howdy! You sure look like you need a drink.",
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
                @"Terrific! Recently <b>strange growths</b> have infested my main <b>dance floor</b>.",
                @"And now all my regulars are scared out of their wits.",
                @"My clients come for the dancing and our incredible <i>atmosphere</i>! And our drinks keep them goin’ all night.| That’s where we profit, cha-ching!",
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
        choiceText = "Sorry, I’m a little busy right now!",
        
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
                @"It’s always been my dream to create the best saloon in all the land, a destination outsiders would travel far and wide just to experience.",
                @"But first I have to prove our worth inside these {18} walls! I swore to myself I wouldn’t leave here ‘til I do.",
                @"Ha-ha, sure, call me ol’ fashioned or whatnot, but I never break my promises.",
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
                @"If only I could get that <b>dance floor</b> back to operational again, maybe then the {35} could be the crown jewel of {18}.",
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
                @"Ba-ha! You want to get inside? No chance, kid!",
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
                @"We’re dealing with an infestation. It’s a crisis! If we don’t figure it out, we might all be out of work pretty soon here.",
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
                @"What’re you still doing here, ba-ha!",
                @"We ain’t open for business! We got an issue inside, you got that?",
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
                @"Oh yeah that’s right, we don’t have time for dilly-dallying, kid!",
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
                @"Hey stop right there!!!",
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
                @"What’s your deal, kid? We’re dealing with a major crisis inside!",
                @"We have an infestation! We’re in no position to serve anyone, especially not an <b>outsider</b>, ba-ha!",
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
                @"Hey, bud, are you here to help us out or what?",
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
                @"Ha! I’ve never heard of an <b>outsider</b> be of any use to a {13} before!",
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
                @"It’s because we stick to our roots, you got that, bud?",
                @"Our menu’s been the same for centuries! Keep it simple and potent, that’s our motto!",
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
                @"{62} and I have known each other since I can remember! Can’t imagine doing anything without him.",
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
                @"Martini... gin, dry vermouth.",
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
                @"Margarita... tequila, cointreau, lime juice.",
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
{
    "urselks-saloon_bar_thought3",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"Tom Collins... gin, simple syrup, lemon juice, club soda.",
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
                @"These pillars seem like they’re pulsing.",
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
        speaker = "NA",
        EN = new string[]
        {
                @"Can’t believe in sincerity because it is invisible. Is true sincerity somewhere deep inside? Maybe the only way to show it is to slash our bellies and take out our visible sincerity?",
        },
        
    }
},
{
    "painting-entrances_default_prompt1",
    new Model_Languages
    {
        speaker = "NA",
        EN = new string[]
        {
                @"<b>Beware, there is no turning back once one is inside</b>.<br><br>Now would you like to enter me?",
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
        speaker = "NA",
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
        speaker = "NA",
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
                @"This reminds you of a certain romantic painting.",
        },
        
    }
},
{
    "painting-entrances_eileens-mind_prompt",
    new Model_Languages
    {
        speaker = "NA",
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
        speaker = "NA",
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
                @"You’ve seen a painting like this somewhere.",
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
                @"An unfinished painting. It appears to be two hands.",
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
                @"It seems someone finished this painting! It looks like someone trying their hardest to hold on to another’s hand.",
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
                @"This painting... it’s not done.",
                @"Why is it in such a different style?",
                @"There’s also a doormat in front of it, strange...",
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
                @"You could’ve sworn {12} hadn’t finished this one yet...| but it looks done now.| It makes you feel something familiar.",
                @"You know that feeling when your body refuses to warm up no matter what?",
                @"*shiver*",
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
                @"A sketch of a thorny vine in a rough style. Why do roses have to have thorns?",
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
                @"In the past, you’ve been told a painting is never in fact done.",
                @"This one looks pretty complete to you though! It’s actually two vines weaving to be one.",
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
                @"You begin to feel like you’re at the bottom of a well.",
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
                @"A labyrinth. There’s always an entrance and an exit to a labyrinth, right?",
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
                @"Flowers are best when they’re dried and hung.",
        },
        
    }
},
{
    "ballroom_world-paintings_wells-world_done",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"You’ve spent enough time at the bottom of a well.",
        },
        
    }
},
{
    "ballroom_world-paintings_celestial-gardens-world_done",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"You’re confident you’d always be able to find the exit.",
        },
        
    }
},
{
    "ballroom_world-paintings_xxx-world_done",
    new Model_Languages
    {
        speaker = "{0}",
        EN = new string[]
        {
                @"At least dried and hung flowers don’t die on you.",
        },
        
    }
},
// ------------------------------------------------------------------
// Items
{
    "item-object_sticker_psychic-duck",
    new Model_Languages
    {
        speaker = "NA",
        EN = new string[]
        {
                @"You got the @@PsychicDuck @@Sticker_Bold!|<br>The @@Sticker_Bold contains the sealed spirit of a {13}.",
                @"@@Stickers_Bold allow you to inhabit the body of the mask’s original owner.",
                @"Press @@InventoryKey to open your {32} and set it to {82} by selecting it in the @@Stickers_Bold Screen.",
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
        speaker = "NA",
        EN = new string[]
        {
                @"You got the @@AnimalWithin @@Sticker_Bold!|<br>Its original owner had a penchant for eating souls.",
                @"Use its {79} by pressing @@MaskCommandKey while wearing the @@Sticker_Bold to chomp through edible obstacles.",
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
        speaker = "NA",
        EN = new string[]
        {
                @"You got the @@BoarNeedle @@Sticker_Bold!|<br>Its original owner desired to see what was invisible.",
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
        speaker = "NA",
        EN = new string[]
        {
                @"You got the @@IceSpike @@Sticker_Bold!|<br>Its original owner was said to have been caught in a snowstorm.",
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
        speaker = "NA",
        EN = new string[]
        {
                @"You got the @@MelancholyPiano @@Sticker_Bold!|<br>Its original owner played a melancholic tune.",
                @"Use the @@MelancholyPiano @@Sticker_Bold to follow the chords of your heart to any previously <b>remembered piano</b>.",
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
        speaker = "NA",
        EN = new string[]
        {
                @"You returned to the conscious world wearing a mask.",
                @"You got the @@LastElevator @@Sticker_Bold!|<br>Not much is known of its original owner.",
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
        speaker = "NA",
        EN = new string[]
        {
                @"You got the @@LetThereBeLight @@Sticker_Bold!|<br>Its original owner constructed the lighting within {18}.",
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
        speaker = "NA",
        EN = new string[]
        {
                @"You got the @@Puppeteer @@Sticker_Bold!|<br>Its original owner spent each waking moment trying to bring life to handmade puppets.",
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
    "item-object_sticker_my-mask",
    new Model_Languages
    {
        speaker = "NA",
        EN = new string[]
        {
                @"Mysterious forces well up inside you.|<br>A strange @@Sticker_Bold materializes.",
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
    "item-object_sticker_my-mask1",
    new Model_Languages
    {
        speaker = "NA",
        EN = new string[]
        {
                @"You got the @@MyMask @@Sticker_Bold!|<br>It has been longing for its original owner.",
                @"The @@MyMask @@Sticker_Bold emanates a powerful aura but its uses are unknown.",
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
        speaker = "NA",
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
        speaker = "NA",
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
        speaker = "NA",
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
{
    "item-object_collectible_speed-seal",
    new Model_Languages
    {
        speaker = "NA",
        EN = new string[]
        {
                @"You found the @@SpeedSeal! The spirits within this seal give you haste... but only when you are your former self.",
                @"Hold @@SpeedKey while walking. Its effects only work when you are not wearing a @@Sticker_Bold.",
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

