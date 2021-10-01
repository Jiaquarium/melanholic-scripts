// Last created by UI Exporter at 2021-09-30 22:46:24

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://docs.google.com/spreadsheets/d/12PJr55wEMnZhO3n-c00xunSQxyDqnkutiAOx4BLh0tQ/edit#gid=814810533

public class Model_LanguagesUI
{
    public string EN { get; set; }
}

public class Script_UIText
{
    public static Dictionary<string, Model_LanguagesUI> Text = new Dictionary<string, Model_LanguagesUI>

{
// ------------------------------------------------------------------
// Intro
{
    "intro_narrator_hotel",
    new Model_LanguagesUI
    {
        EN = @"I work at the front desk of a seaside hotel| about a two hour drive from my hometown."
    }
},
{
    "intro_narrator_hotel1",
    new Model_LanguagesUI
    {
        EN = @"It's fine though.| I can use one of the empty rooms if I don't want to make the trip home."
    }
},
{
    "intro_narrator_hotel2",
    new Model_LanguagesUI
    {
        EN = @"There's always a few empty rooms,| since it's pretty slow this winter season."
    }
},
{
    "intro_narrator_hotel3",
    new Model_LanguagesUI
    {
        EN = @"Actually now that I think about it..."
    }
},
{
    "intro_narrator_hotel4",
    new Model_LanguagesUI
    {
        EN = @"I'm not quite sure how long it's been since I've been home."
    }
},
{
    "intro_narrator_hotel5",
    new Model_LanguagesUI
    {
        EN = @"And oh..."
    }
},
{
    "intro_narrator_hotel6",
    new Model_LanguagesUI
    {
        EN = @"I work the night shift."
    }
},
// ------------------------------------------------------------------
// Endings
{
    "good-ending_narrator_monologue",
    new Model_LanguagesUI
    {
        EN = @"After that day, I never saw {37} ever again."
    }
},
{
    "good-ending_narrator_monologue1",
    new Model_LanguagesUI
    {
        EN = @"I often still think back about what happened there."
    }
},
{
    "good-ending_narrator_monologue2",
    new Model_LanguagesUI
    {
        EN = @"But with each passing moment..."
    }
},
{
    "good-ending_narrator_monologue3",
    new Model_LanguagesUI
    {
        EN = @"The memory fades a bit more."
    }
},
{
    "good-ending_narrator_monologue4",
    new Model_LanguagesUI
    {
        EN = @"I'd like to think my time inside there was worth it."
    }
},
{
    "true-ending_narrator_monologue",
    new Model_LanguagesUI
    {
        EN = @"After that day, I quit my job at the seaside hotel."
    }
},
{
    "true-ending_narrator_monologue1",
    new Model_LanguagesUI
    {
        EN = @"Needless to say, I never saw {37} ever again."
    }
},
{
    "true-ending_narrator_monologue2",
    new Model_LanguagesUI
    {
        EN = @"It's been a few years now..."
    }
},
{
    "true-ending_narrator_monologue3",
    new Model_LanguagesUI
    {
        EN = @"But I know I'll never forget what I saw there."
    }
},
{
    "true-ending_narrator_monologue4",
    new Model_LanguagesUI
    {
        EN = @"I'm confident that now I can say..."
    }
},
{
    "true-ending_narrator_monologue5",
    new Model_LanguagesUI
    {
        EN = @"I saved {18}."
    }
},
// ------------------------------------------------------------------
// Stickers
{
    "item-object-UI_sticker_psychic-duck",
    new Model_LanguagesUI
    {
        EN = @"<b>@@Stickers_NoBold can be stuck onto your</b> {5} to give you <b>special abilities</b>. Go to your {32} to try it out!

Once switched to your {65}, the @@PsychicDuck @@Sticker_NoBold allows you to engage with {19} in conversation."
    }
},
{
    "sticker_psychic-duck",
    new Model_LanguagesUI
    {
        EN = @"Attach the @@PsychicDuck @@Stickers_NoBold to your {5} and switch it to your {65} to be able to converse with {19}. {68} are only in effect as your {65}. A @@PsychicDuck makes a wonderful sidekick!"
    }
},
{
    "sticker_boar-needle",
    new Model_LanguagesUI
    {
        EN = @"Allows you to enter paintings with doormats. The painting must have a doormat."
    }
},
{
    "sticker_animal-within",
    new Model_LanguagesUI
    {
        EN = @"Use its {79} to let you eat away edible obstacles. CHOMP CHOMP!"
    }
},
{
    "sticker_ice-spike",
    new Model_LanguagesUI
    {
        EN = @"A spike so powerful, it'll crack open just about anything."
    }
},
{
    "sticker_melancholy-piano",
    new Model_LanguagesUI
    {
        EN = @"Follow the chords of your heart."
    }
},
{
    "sticker_last-elevator",
    new Model_LanguagesUI
    {
        EN = @"Can be used anywhere inside {18} to take the {66} back to the {72}."
    }
},
{
    "sticker_let-there-be-light",
    new Model_LanguagesUI
    {
        EN = @"Will illuminate certain dark areas."
    }
},
{
    "sticker_puppeteer",
    new Model_LanguagesUI
    {
        EN = @"Use the @@Puppeteer @@Sticker_NoBold to control {73}. Not too shabby."
    }
},
// ------------------------------------------------------------------
// Usables
{
    "usable_super-small-key",
    new Model_LanguagesUI
    {
        EN = @"A @@SuperSmallKey."
    }
},
// ------------------------------------------------------------------
// Collectibles
{
    "collectible_last-well-map",
    new Model_LanguagesUI
    {
        EN = @"Rumor is everything begins in Spring."
    }
},
{
    "collectible_last-spell-recipe-book",
    new Model_LanguagesUI
    {
        EN = @"Promise this is the last time."
    }
},
// ------------------------------------------------------------------
// Ids
{
    "notes_ids_not-home",
    new Model_LanguagesUI
    {
        EN = @"Hiya!

If you’re reading this that means I'm out dancin'. You know what they say, a dance move a day keeps the {42} away.

Please don’t miss me too much!"
    }
},
{
    "notes_ids_leave-me",
    new Model_LanguagesUI
    {
        EN = @"Need to be alone."
    }
},
// ------------------------------------------------------------------
// Ursie
{
    "notes_ursie_thank-you",
    new Model_LanguagesUI
    {
        EN = @"Howdy {0},

I've come to my senses;
I’m confident in my abilities now.
Proving myself inside these
{18} walls means nothing.

{61}, {62} and I have hit the
dusty trail. There’s bigger fish out
there for us to fry."
    }
},
{
    "notes_ursie_thank-you1",
    new Model_LanguagesUI
    {
        EN = @"Thank you for assisting us even at
such lows.

I’m not sure how I’ll repay you,
how about we name a cocktail after you. How’s the {0} <b>Spritz</b> sound?"
    }
},
{
    "notes_ursie_thank-you_name",
    new Model_LanguagesUI
    {
        EN = @"Sincerely,
{33}"
    }
},
// ------------------------------------------------------------------
// Inventory
{
    "menu_top-bar_stickers",
    new Model_LanguagesUI
    {
        EN = @"Masks"
    }
},
{
    "menu_top-bar_items",
    new Model_LanguagesUI
    {
        EN = @"Items"
    }
},
{
    "menu_top-bar_notes",
    new Model_LanguagesUI
    {
        EN = @"Notes"
    }
},
{
    "menu_equipment_label",
    new Model_LanguagesUI
    {
        EN = @"Prepped Masks"
    }
},
// ------------------------------------------------------------------
// Input
{
    "input_default_submit",
    new Model_LanguagesUI
    {
        EN = @"Submit"
    }
},
{
    "input_CCTV_disarm",
    new Model_LanguagesUI
    {
        EN = @"『 DISARM SURVEILLANCE SYSTEM 』"
    }
},
// ------------------------------------------------------------------
// Saving & Loading
{
    "saving_default",
    new Model_LanguagesUI
    {
        EN = @"SAVING GAME... Please do not turn off power."
    }
},
{
    "saving_to-weekend",
    new Model_LanguagesUI
    {
        EN = @"You are given the responsibility of the {71}. {53} is your first day.

SAVING GAME... Please do not turn off power."
    }
},
{
    "game-over_message",
    new Model_LanguagesUI
    {
        EN = @"What to do, what to do..."
    }
},
{
    "game-over_choice_0",
    new Model_LanguagesUI
    {
        EN = @"Start your @@Run over."
    }
},
{
    "game-over_choice_1",
    new Model_LanguagesUI
    {
        EN = @"Go to main menu."
    }
},
// ------------------------------------------------------------------
// Controls
{
    "controls_action_active-sticker-command",
    new Model_LanguagesUI
    {
        EN = @"{79}"
    }
},
{
    "controls_action_switch-active-sticker",
    new Model_LanguagesUI
    {
        EN = @"{80}"
    }
},

};
}

