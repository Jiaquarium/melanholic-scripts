// Last created by UI Exporter at 2024-03-12 12:53:49

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://docs.google.com/spreadsheets/d/12PJr55wEMnZhO3n-c00xunSQxyDqnkutiAOx4BLh0tQ/edit#gid=814810533

public class Model_LanguagesUI
{
    public string EN { get; set; }
    public string CN { get; set; }
}

public class Script_UIText
{
    public static Dictionary<string, Model_LanguagesUI> Text = new Dictionary<string, Model_LanguagesUI>

{
// ------------------------------------------------------------------
// Intro
{
    "intro_authors_by",
    new Model_LanguagesUI
    {
        EN = @"By",
        CN = @"By"
    }
},
{
    "intro_authors_by_name",
    new Model_LanguagesUI
    {
        EN = @"Jiaquarium",
        CN = @"Jiaquarium"
    }
},
{
    "intro_authors_music",
    new Model_LanguagesUI
    {
        EN = @"Original Soundtrack By",
        CN = @"Original Soundtrack By"
    }
},
{
    "intro_authors_music_name",
    new Model_LanguagesUI
    {
        EN = @"s3-z",
        CN = @"s3-z"
    }
},
{
    "intro_narrator_hotel",
    new Model_LanguagesUI
    {
        EN = @"I work at the front desk of a hotel right by the sea.",
        CN = @"I work at the front desk of a hotel right by the sea."
    }
},
{
    "intro_narrator_hotel1",
    new Model_LanguagesUI
    {
        EN = @"It’s usually pretty slow in the winter season, so the hotel owner and I worked out a pretty neat deal,",
        CN = @"It’s usually pretty slow in the winter season, so the hotel owner and I worked out a pretty neat deal,"
    }
},
{
    "intro_narrator_hotel2",
    new Model_LanguagesUI
    {
        EN = @"I can just use one of the unoccupied rooms whenever I’m off.",
        CN = @"I can just use one of the unoccupied rooms whenever I’m off."
    }
},
{
    "intro_narrator_hotel3",
    new Model_LanguagesUI
    {
        EN = @"But actually now that I think about it...",
        CN = @"But actually now that I think about it..."
    }
},
{
    "intro_narrator_hotel4",
    new Model_LanguagesUI
    {
        EN = @"I’m not quite sure how long it’s been since I’ve left here.",
        CN = @"I’m not quite sure how long it’s been since I’ve left here."
    }
},
{
    "intro_narrator_hotel5",
    new Model_LanguagesUI
    {
        EN = @"Hm, what else do you want to know?",
        CN = @"Hm, what else do you want to know?"
    }
},
{
    "intro_narrator_hotel6",
    new Model_LanguagesUI
    {
        EN = @"What it’s like working the night shift?",
        CN = @"What it’s like working the night shift?"
    }
},
{
    "intro_content-warning",
    new Model_LanguagesUI
    {
        EN = @"This game is not suitable for children or those who may be easily disturbed.",
        CN = @"This game is not suitable for children or those who may be easily disturbed."
    }
},
// ------------------------------------------------------------------
// Good Ending
{
    "good-ending_narrator_monologue",
    new Model_LanguagesUI
    {
        EN = @"This was my last day at the seaside hotel.",
        CN = @"This was my last day at the seaside hotel."
    }
},
{
    "good-ending_narrator_monologue1",
    new Model_LanguagesUI
    {
        EN = @"How are one of these notes supposed to go anyways?",
        CN = @"How are one of these notes supposed to go anyways?"
    }
},
{
    "good-ending_narrator_monologue1_0",
    new Model_LanguagesUI
    {
        EN = @"Needless to say, I’ll never see {18} ever again.",
        CN = @"Needless to say, I’ll never see {18} ever again."
    }
},
{
    "good-ending_narrator_monologue2",
    new Model_LanguagesUI
    {
        EN = @"I never imagined it to all go like this...",
        CN = @"I never imagined it to all go like this..."
    }
},
{
    "good-ending_narrator_monologue3",
    new Model_LanguagesUI
    {
        EN = @"My life revolved around that place for so long.",
        CN = @"My life revolved around that place for so long."
    }
},
{
    "good-ending_narrator_monologue4",
    new Model_LanguagesUI
    {
        EN = @"And now, it’ll just be another fading memory.",
        CN = @"And now, it’ll just be another fading memory."
    }
},
{
    "good-ending_narrator_monologue5",
    new Model_LanguagesUI
    {
        EN = @"To be locked away like the rest.",
        CN = @"To be locked away like the rest."
    }
},
{
    "good-ending_narrator_monologue6",
    new Model_LanguagesUI
    {
        EN = @"......",
        CN = @"......"
    }
},
{
    "good-ending_narrator_monologue7",
    new Model_LanguagesUI
    {
        EN = @"Looking at the sea now...",
        CN = @"Looking at the sea now..."
    }
},
{
    "good-ending_narrator_monologue7_0",
    new Model_LanguagesUI
    {
        EN = @"Was I being naïve?",
        CN = @"Was I being naïve?"
    }
},
{
    "good-ending_narrator_monologue8",
    new Model_LanguagesUI
    {
        EN = @"To think I could actually escape...",
        CN = @"To think I could actually escape..."
    }
},
{
    "good-ending_narrator_monologue9",
    new Model_LanguagesUI
    {
        EN = @"......",
        CN = @"......"
    }
},
{
    "good-ending_narrator_monologue10",
    new Model_LanguagesUI
    {
        EN = @"Were they all just in my head?",
        CN = @"Were they all just in my head?"
    }
},
{
    "good-ending_narrator_monologue11",
    new Model_LanguagesUI
    {
        EN = @"Unfinished paintings...",
        CN = @"Unfinished paintings..."
    }
},
{
    "good-ending_narrator_monologue12",
    new Model_LanguagesUI
    {
        EN = @"You and <i>him</i>.",
        CN = @"You and <i>him</i>."
    }
},
{
    "good-ending_narrator_monologue12_0",
    new Model_LanguagesUI
    {
        EN = @"A self-portrait.",
        CN = @"A self-portrait."
    }
},
{
    "good-ending_narrator_monologue12_1",
    new Model_LanguagesUI
    {
        EN = @"Am I really losing it?",
        CN = @"Am I really losing it?"
    }
},
{
    "good-ending_narrator_monologue13",
    new Model_LanguagesUI
    {
        EN = @"Maybe some questions are better left alone.",
        CN = @"Maybe some questions are better left alone."
    }
},
{
    "good-ending_narrator_monologue14",
    new Model_LanguagesUI
    {
        EN = @"The most important thing though,",
        CN = @"The most important thing though,"
    }
},
{
    "good-ending_narrator_monologue14_0",
    new Model_LanguagesUI
    {
        EN = @"Is that I finally have an end to this chapter of my life.",
        CN = @"Is that I finally have an end to this chapter of my life."
    }
},
{
    "good-ending_narrator_monologue14_1",
    new Model_LanguagesUI
    {
        EN = @"I’ve been at the bottom of a well for far too long now.",
        CN = @"I’ve been at the bottom of a well for far too long now."
    }
},
{
    "good-ending_narrator_monologue15",
    new Model_LanguagesUI
    {
        EN = @"I know you’re just trying to help,",
        CN = @"I know you’re just trying to help,"
    }
},
{
    "good-ending_narrator_monologue15_0",
    new Model_LanguagesUI
    {
        EN = @"Y̵o̴u̴ ̵k̸e̴p̶t̶ ̸t̵a̶k̷i̶n̸g̵ ̴m̶e̸ ̵d̴o̶w̶n̸ ̷t̵h̵e̸r̷e̴ ̷a̵n̷d̷ ̵f̴o̵r̴ ̶w̵h̶a̷t̵?̴",
        CN = @"Y̵o̴u̴ ̵k̸e̴p̶t̶ ̸t̵a̶k̷i̶n̸g̵ ̴m̶e̸ ̵d̴o̶w̶n̸ ̷t̵h̵e̸r̷e̴ ̷a̵n̷d̷ ̵f̴o̵r̴ ̶w̵h̶a̷t̵?̴"
    }
},
{
    "good-ending_narrator_monologue15a_0",
    new Model_LanguagesUI
    {
        EN = @"I don’t blame you.",
        CN = @"I don’t blame you."
    }
},
{
    "good-ending_narrator_monologue15a_1",
    new Model_LanguagesUI
    {
        EN = @"There’s nothing you could do.",
        CN = @"There’s nothing you could do."
    }
},
{
    "good-ending_narrator_monologue15_1",
    new Model_LanguagesUI
    {
        EN = @"||This weight on my shoulders...",
        CN = @"||This weight on my shoulders..."
    }
},
{
    "good-ending_narrator_monologue15_2",
    new Model_LanguagesUI
    {
        EN = @"Everyone gets what they wanted this way.",
        CN = @"Everyone gets what they wanted this way."
    }
},
{
    "good-ending_narrator_monologue15_3",
    new Model_LanguagesUI
    {
        EN = @"No more halfway.",
        CN = @"No more halfway."
    }
},
{
    "good-ending_narrator_monologue15_4",
    new Model_LanguagesUI
    {
        EN = @"My eyes... they feel incredibly heavy.",
        CN = @"My eyes... they feel incredibly heavy."
    }
},
{
    "good-ending_narrator_monologue15_b",
    new Model_LanguagesUI
    {
        EN = @"<b>It’s {49}</b>",
        CN = @"<b>It’s {49}</b>"
    }
},
{
    "good-ending_narrator_monologue16",
    new Model_LanguagesUI
    {
        EN = @"Look,",
        CN = @"Look,"
    }
},
{
    "good-ending_narrator_monologue17",
    new Model_LanguagesUI
    {
        EN = @"I knew it.",
        CN = @"I knew it."
    }
},
{
    "good-ending_narrator_monologue17_1",
    new Model_LanguagesUI
    {
        EN = @"The sky is separating from the sea.",
        CN = @"The sky is separating from the sea."
    }
},
{
    "good-ending_narrator_monologue18",
    new Model_LanguagesUI
    {
        EN = @"......",
        CN = @"......"
    }
},
{
    "good-ending_narrator_monologue19",
    new Model_LanguagesUI
    {
        EN = @"Finally, no more pain.",
        CN = @"Finally, no more pain."
    }
},
{
    "good-ending_the-end_title",
    new Model_LanguagesUI
    {
        EN = @"The End",
        CN = @"The End"
    }
},
{
    "good-ending_the-end_type",
    new Model_LanguagesUI
    {
        EN = @"RĖŇù ŮẋůĉŚū",
        CN = @"RĖŇù ŮẋůĉŚū"
    }
},
{
    "good-ending_the-end_type1",
    new Model_LanguagesUI
    {
        EN = @"I TRUSTED YOU",
        CN = @"I TRUSTED YOU"
    }
},
{
    "good-ending_the-end_type_zalgo0",
    new Model_LanguagesUI
    {
        EN = @"Űţýs ĵẋûŰĖĜ",
        CN = @"Űţýs ĵẋûŰĖĜ"
    }
},
{
    "good-ending_the-end_type_zalgo1",
    new Model_LanguagesUI
    {
        EN = @"ġŉŒŢ ÄẋïÈĳĥ",
        CN = @"ġŉŒŢ ÄẋïÈĳĥ"
    }
},
{
    "good-ending_the-end_type_zalgo2",
    new Model_LanguagesUI
    {
        EN = @"ŖĀŁő sẋšças",
        CN = @"ŖĀŁő sẋšças"
    }
},
{
    "good-ending_the-end_type_zalgo3",
    new Model_LanguagesUI
    {
        EN = @"ăĲÑĢ EẋĎąõà",
        CN = @"ăĲÑĢ EẋĎąõà"
    }
},
{
    "good-ending_the-end_label",
    new Model_LanguagesUI
    {
        EN = @"A Mirror Who Lost Its Reflection",
        CN = @"A Mirror Who Lost Its Reflection"
    }
},
{
    "good-ending_transition0",
    new Model_LanguagesUI
    {
        EN = @"No, this isn’t it...",
        CN = @"No, this isn’t it..."
    }
},
{
    "good-ending_transition1",
    new Model_LanguagesUI
    {
        EN = @"The portrait is close but it won’t be finished this way.",
        CN = @"The portrait is close but it won’t be finished this way."
    }
},
{
    "good-ending_transition-to-title",
    new Model_LanguagesUI
    {
        EN = @"...there has to be more to you.",
        CN = @"...there has to be more to you."
    }
},
// ------------------------------------------------------------------
// True Ending
{
    "true-ending_narrator_monologue0_0",
    new Model_LanguagesUI
    {
        EN = @"This was my last day at the seaside hotel.",
        CN = @"This was my last day at the seaside hotel."
    }
},
{
    "true-ending_narrator_monologue0_1",
    new Model_LanguagesUI
    {
        EN = @"Needless to say, I’ll never see {18} ever again.",
        CN = @"Needless to say, I’ll never see {18} ever again."
    }
},
{
    "true-ending_narrator_monologue1_0",
    new Model_LanguagesUI
    {
        EN = @"It’s strange to think now,",
        CN = @"It’s strange to think now,"
    }
},
{
    "true-ending_narrator_monologue1_1",
    new Model_LanguagesUI
    {
        EN = @"That every decision I made, every thought I had,",
        CN = @"That every decision I made, every thought I had,"
    }
},
{
    "true-ending_narrator_monologue1_2",
    new Model_LanguagesUI
    {
        EN = @"Revolved around that place for so long.",
        CN = @"Revolved around that place for so long."
    }
},
{
    "true-ending_narrator_monologue2_0",
    new Model_LanguagesUI
    {
        EN = @"......",
        CN = @"......"
    }
},
{
    "true-ending_narrator_monologue3_0",
    new Model_LanguagesUI
    {
        EN = @"I guess over time, it really did begin to change me,",
        CN = @"I guess over time, it really did begin to change me,"
    }
},
{
    "true-ending_narrator_monologue3_1",
    new Model_LanguagesUI
    {
        EN = @"Spending so many nights in there.",
        CN = @"Spending so many nights in there."
    }
},
{
    "true-ending_narrator_monologue3_2",
    new Model_LanguagesUI
    {
        EN = @"The only thing I could do was numb myself to it all...",
        CN = @"The only thing I could do was numb myself to it all..."
    }
},
{
    "true-ending_narrator_monologue3_3",
    new Model_LanguagesUI
    {
        EN = @"As I looked out from a face that wasn’t even mine.",
        CN = @"As I looked out from a face that wasn’t even mine."
    }
},
{
    "true-ending_narrator_monologue4_0",
    new Model_LanguagesUI
    {
        EN = @"......",
        CN = @"......"
    }
},
{
    "true-ending_narrator_monologue5_0",
    new Model_LanguagesUI
    {
        EN = @"Looking at the sea now, it’s becoming a bit clearer...",
        CN = @"Looking at the sea now, it’s becoming a bit clearer..."
    }
},
{
    "true-ending_narrator_monologue5_1",
    new Model_LanguagesUI
    {
        EN = @"My feelings in purgatory I was helplessly dragging around.",
        CN = @"My feelings in purgatory I was helplessly dragging around."
    }
},
{
    "true-ending_narrator_monologue5_2",
    new Model_LanguagesUI
    {
        EN = @"In the past, I would’ve locked them away.",
        CN = @"In the past, I would’ve locked them away."
    }
},
{
    "true-ending_narrator_monologue6_0",
    new Model_LanguagesUI
    {
        EN = @"I’ve always feared becoming <i>only them</i>...",
        CN = @"I’ve always feared becoming <i>only them</i>..."
    }
},
{
    "true-ending_narrator_monologue6_1",
    new Model_LanguagesUI
    {
        EN = @"The same hour on a winter night.",
        CN = @"The same hour on a winter night."
    }
},
{
    "true-ending_narrator_monologue6_2",
    new Model_LanguagesUI
    {
        EN = @"I’ve paid my visits.",
        CN = @"I’ve paid my visits."
    }
},
{
    "true-ending_narrator_monologue6_3",
    new Model_LanguagesUI
    {
        EN = @"<b>I’m not just them.</b>",
        CN = @"<b>I’m not just them.</b>"
    }
},
{
    "true-ending_narrator_monologue7_0",
    new Model_LanguagesUI
    {
        EN = @"I’m not running anymore.",
        CN = @"I’m not running anymore."
    }
},
{
    "true-ending_narrator_monologue7_1",
    new Model_LanguagesUI
    {
        EN = @"I’m ready for what’s next.",
        CN = @"I’m ready for what’s next."
    }
},
{
    "true-ending_narrator_monologue7_2",
    new Model_LanguagesUI
    {
        EN = @"I hope you’ll understand.",
        CN = @"I hope you’ll understand."
    }
},
{
    "true-ending_narrator_monologue8_0",
    new Model_LanguagesUI
    {
        EN = @"Hey, Ids, look,",
        CN = @"Hey, Ids, look,"
    }
},
{
    "true-ending_narrator_monologue8_1",
    new Model_LanguagesUI
    {
        EN = @"The sun is coming up.",
        CN = @"The sun is coming up."
    }
},
{
    "true-ending_narrator_monologue8_b",
    new Model_LanguagesUI
    {
        EN = @"<b>It’s {49}</b>",
        CN = @"<b>It’s {49}</b>"
    }
},
{
    "true-ending_narrator_monologue9_0",
    new Model_LanguagesUI
    {
        EN = @"......",
        CN = @"......"
    }
},
{
    "true-ending_narrator_monologue10_0",
    new Model_LanguagesUI
    {
        EN = @"Finally, I don’t feel like sleeping anymore.",
        CN = @"Finally, I don’t feel like sleeping anymore."
    }
},
{
    "true-ending_the-end_type",
    new Model_LanguagesUI
    {
        EN = @"autical Dawn",
        CN = @"autical Dawn"
    }
},
{
    "true-ending_the-end_type_start-word",
    new Model_LanguagesUI
    {
        EN = @"n",
        CN = @"n"
    }
},
{
    "true-ending_the-end_label",
    new Model_LanguagesUI
    {
        EN = @"A True Ending",
        CN = @"A True Ending"
    }
},
// ------------------------------------------------------------------
// Bad Ending
{
    "bad-ending_the-end_type",
    new Model_LanguagesUI
    {
        EN = @"『 THE SEALING HAS COMMENCED 』",
        CN = @"『 THE SEALING HAS COMMENCED 』"
    }
},
{
    "bad-ending_narrator_the-sealing_opening-words",
    new Model_LanguagesUI
    {
        EN = @"As the clock",
        CN = @"As the clock"
    }
},
{
    "bad-ending_narrator_the-sealing",
    new Model_LanguagesUI
    {
        EN = @"struck {49} the ancient spell of the {22} was cast.",
        CN = @"struck {49} the ancient spell of the {22} was cast."
    }
},
{
    "bad-ending_narrator_the-sealing1",
    new Model_LanguagesUI
    {
        EN = @"It is said that {18} along with all those inside were locked away,<br>permanently losing touch with the outside world.",
        CN = @"It is said that {18} along with all those inside were locked away,<br>permanently losing touch with the outside world."
    }
},
{
    "bad-ending_the-end_label",
    new Model_LanguagesUI
    {
        EN = @"The Sealing",
        CN = @"The Sealing"
    }
},
{
    "game-over_message",
    new Model_LanguagesUI
    {
        EN = @"What to do, what to do...",
        CN = @"What to do, what to do..."
    }
},
{
    "game-over_choice_0",
    new Model_LanguagesUI
    {
        EN = @"Try this night again",
        CN = @"Try this night again"
    }
},
{
    "game-over_choice_1",
    new Model_LanguagesUI
    {
        EN = @"Go to main menu",
        CN = @"Go to main menu"
    }
},
// ------------------------------------------------------------------
// Credits
{
    "credits_main-staff_role0",
    new Model_LanguagesUI
    {
        EN = @"Art & Design, Writing, Programming",
        CN = @"Art & Design, Writing, Programming"
    }
},
{
    "credits_main-staff_name0",
    new Model_LanguagesUI
    {
        EN = @"Jiaquarium",
        CN = @"Jiaquarium"
    }
},
{
    "credits_main-staff_role1",
    new Model_LanguagesUI
    {
        EN = @"Music & Sound",
        CN = @"Music & Sound"
    }
},
{
    "credits_main-staff_name1",
    new Model_LanguagesUI
    {
        EN = @"s3-z",
        CN = @"s3-z"
    }
},
{
    "credits_main-staff_role2",
    new Model_LanguagesUI
    {
        EN = @"Art & Design",
        CN = @"Art & Design"
    }
},
{
    "credits_main-staff_name2",
    new Model_LanguagesUI
    {
        EN = @"Estella",
        CN = @"Estella"
    }
},
{
    "credits_special-thanks",
    new Model_LanguagesUI
    {
        EN = @"Special Thanks",
        CN = @"Special Thanks"
    }
},
{
    "credits_extras_role0",
    new Model_LanguagesUI
    {
        EN = @"Agent",
        CN = @"Agent"
    }
},
{
    "credits_extras_name0",
    new Model_LanguagesUI
    {
        EN = @"Judah Silver",
        CN = @"Judah Silver"
    }
},
{
    "credits_extras_role1",
    new Model_LanguagesUI
    {
        EN = @"Dance Tracks",
        CN = @"Dance Tracks"
    }
},
{
    "credits_extras_name1",
    new Model_LanguagesUI
    {
        EN = @"Patricia Taxxon",
        CN = @"Patricia Taxxon"
    }
},
{
    "credits_extras_role2",
    new Model_LanguagesUI
    {
        EN = @"SFX",
        CN = @"SFX"
    }
},
{
    "credits_extras_name2",
    new Model_LanguagesUI
    {
        EN = @"Taira Komori",
        CN = @"Taira Komori"
    }
},
{
    "credits_extras_role3",
    new Model_LanguagesUI
    {
        EN = @"Script Editing",
        CN = @"Script Editing"
    }
},
{
    "credits_extras_name3",
    new Model_LanguagesUI
    {
        EN = @"Lynette Shen",
        CN = @"Lynette Shen"
    }
},
{
    "credits_playtesters_thank-you",
    new Model_LanguagesUI
    {
        EN = @"To our playtesters and colleagues,
This would not have been possible without your feedback, course corrections, and encouragement.",
        CN = @"To our playtesters and colleagues,
This would not have been possible without your feedback, course corrections, and encouragement."
    }
},
{
    "credits_playtesters_thank-you1",
    new Model_LanguagesUI
    {
        EN = @"This would not have been possible without your feedback, course corrections, and encouragement.",
        CN = @"This would not have been possible without your feedback, course corrections, and encouragement."
    }
},
{
    "credits_main-playtesters_role0",
    new Model_LanguagesUI
    {
        EN = @"Key Playtesters",
        CN = @"Key Playtesters"
    }
},
{
    "credits_main-playtesters_name0",
    new Model_LanguagesUI
    {
        EN = @"Alice Hua",
        CN = @"Alice Hua"
    }
},
{
    "credits_main-playtesters_name1",
    new Model_LanguagesUI
    {
        EN = @"Daniel Li",
        CN = @"Daniel Li"
    }
},
{
    "credits_main-playtesters_name2",
    new Model_LanguagesUI
    {
        EN = @"Estella Xian",
        CN = @"Estella Xian"
    }
},
{
    "credits_main-playtesters_name3",
    new Model_LanguagesUI
    {
        EN = @"Nathan Waters",
        CN = @"Nathan Waters"
    }
},
{
    "credits_playtesters_role0",
    new Model_LanguagesUI
    {
        EN = @"Playtesters",
        CN = @"Playtesters"
    }
},
{
    "credits_playtesters_name0",
    new Model_LanguagesUI
    {
        EN = @"Ada Lam",
        CN = @"Ada Lam"
    }
},
{
    "credits_playtesters_name1",
    new Model_LanguagesUI
    {
        EN = @"Arden Zhan",
        CN = @"Arden Zhan"
    }
},
{
    "credits_playtesters_name2",
    new Model_LanguagesUI
    {
        EN = @"Ash Kim",
        CN = @"Ash Kim"
    }
},
{
    "credits_playtesters_name3",
    new Model_LanguagesUI
    {
        EN = @"David Tran",
        CN = @"David Tran"
    }
},
{
    "credits_playtesters_name4",
    new Model_LanguagesUI
    {
        EN = @"Leanna Leung",
        CN = @"Leanna Leung"
    }
},
{
    "credits_playtesters_name5",
    new Model_LanguagesUI
    {
        EN = @"Lynette Shen",
        CN = @"Lynette Shen"
    }
},
{
    "credits_playtesters_name6",
    new Model_LanguagesUI
    {
        EN = @"Melos Han-Tani",
        CN = @"Melos Han-Tani"
    }
},
{
    "credits_playtesters_name7",
    new Model_LanguagesUI
    {
        EN = @"Moe Zhang",
        CN = @"Moe Zhang"
    }
},
{
    "credits_playtesters_name8",
    new Model_LanguagesUI
    {
        EN = @"Randy O’ Connor",
        CN = @"Randy O’ Connor"
    }
},
{
    "credits_playtesters_name9",
    new Model_LanguagesUI
    {
        EN = @"Sayaka Kono",
        CN = @"Sayaka Kono"
    }
},
{
    "credits_playtesters_name10",
    new Model_LanguagesUI
    {
        EN = @"Segyero Yoon",
        CN = @"Segyero Yoon"
    }
},
{
    "credits_playtesters_name11",
    new Model_LanguagesUI
    {
        EN = @"Stephen Zhao",
        CN = @"Stephen Zhao"
    }
},
{
    "credits_playtesters_name12",
    new Model_LanguagesUI
    {
        EN = @"Steven Nguyen",
        CN = @"Steven Nguyen"
    }
},
{
    "credits_playtesters_name13",
    new Model_LanguagesUI
    {
        EN = @"Tedmund Chua",
        CN = @"Tedmund Chua"
    }
},
{
    "credits_playtesters_name14",
    new Model_LanguagesUI
    {
        EN = @"Tim Goco",
        CN = @"Tim Goco"
    }
},
{
    "credits_playtesters_name15",
    new Model_LanguagesUI
    {
        EN = @"Yining Zheng",
        CN = @"Yining Zheng"
    }
},
{
    "credits_publisher_header",
    new Model_LanguagesUI
    {
        EN = @"Publishing Staff - Freedom Games",
        CN = @"Publishing Staff - Freedom Games"
    }
},
{
    "credits_publisher_role0",
    new Model_LanguagesUI
    {
        EN = @"Founders",
        CN = @"Founders"
    }
},
{
    "credits_publisher_role1",
    new Model_LanguagesUI
    {
        EN = @"Staff",
        CN = @"Staff"
    }
},
{
    "credits_publisher_name0",
    new Model_LanguagesUI
    {
        EN = @"Donovan Duncan",
        CN = @"Donovan Duncan"
    }
},
{
    "credits_publisher_name1",
    new Model_LanguagesUI
    {
        EN = @"Ben Robinson",
        CN = @"Ben Robinson"
    }
},
{
    "credits_publisher_name2",
    new Model_LanguagesUI
    {
        EN = @"Alexandre Carchano",
        CN = @"Alexandre Carchano"
    }
},
{
    "credits_publisher_name3",
    new Model_LanguagesUI
    {
        EN = @"Amanda Hoppe",
        CN = @"Amanda Hoppe"
    }
},
{
    "credits_publisher_name4",
    new Model_LanguagesUI
    {
        EN = @"Benjamin Tarsa",
        CN = @"Benjamin Tarsa"
    }
},
{
    "credits_publisher_name5",
    new Model_LanguagesUI
    {
        EN = @"Brian Borg",
        CN = @"Brian Borg"
    }
},
{
    "credits_publisher_name6",
    new Model_LanguagesUI
    {
        EN = @"Carrol Dufault",
        CN = @"Carrol Dufault"
    }
},
{
    "credits_publisher_name7",
    new Model_LanguagesUI
    {
        EN = @"Danny Ryba",
        CN = @"Danny Ryba"
    }
},
{
    "credits_publisher_name8",
    new Model_LanguagesUI
    {
        EN = @"Destinee Cleveland",
        CN = @"Destinee Cleveland"
    }
},
{
    "credits_publisher_name9",
    new Model_LanguagesUI
    {
        EN = @"Elisabeth Reeve",
        CN = @"Elisabeth Reeve"
    }
},
{
    "credits_publisher_name10",
    new Model_LanguagesUI
    {
        EN = @"Emmanuel “Manu” Floret",
        CN = @"Emmanuel “Manu” Floret"
    }
},
{
    "credits_publisher_name11",
    new Model_LanguagesUI
    {
        EN = @"Emmanuel Franco",
        CN = @"Emmanuel Franco"
    }
},
{
    "credits_publisher_name12",
    new Model_LanguagesUI
    {
        EN = @"Evan Bloyet",
        CN = @"Evan Bloyet"
    }
},
{
    "credits_publisher_name13",
    new Model_LanguagesUI
    {
        EN = @"Evan Bryant",
        CN = @"Evan Bryant"
    }
},
{
    "credits_publisher_name14",
    new Model_LanguagesUI
    {
        EN = @"Harrison Floyd",
        CN = @"Harrison Floyd"
    }
},
{
    "credits_publisher_name15",
    new Model_LanguagesUI
    {
        EN = @"Ianna Dria Besa",
        CN = @"Ianna Dria Besa"
    }
},
{
    "credits_publisher_name16",
    new Model_LanguagesUI
    {
        EN = @"John C. Boone II",
        CN = @"John C. Boone II"
    }
},
{
    "credits_publisher_name17",
    new Model_LanguagesUI
    {
        EN = @"Jonathan Motes",
        CN = @"Jonathan Motes"
    }
},
{
    "credits_publisher_name18",
    new Model_LanguagesUI
    {
        EN = @"Jordan Kahn",
        CN = @"Jordan Kahn"
    }
},
{
    "credits_publisher_name19",
    new Model_LanguagesUI
    {
        EN = @"Josh Mitchell",
        CN = @"Josh Mitchell"
    }
},
{
    "credits_publisher_name20",
    new Model_LanguagesUI
    {
        EN = @"Katie VanClieaf",
        CN = @"Katie VanClieaf"
    }
},
{
    "credits_publisher_name21",
    new Model_LanguagesUI
    {
        EN = @"Kerri King",
        CN = @"Kerri King"
    }
},
{
    "credits_publisher_name22",
    new Model_LanguagesUI
    {
        EN = @"Matthew Schwartz",
        CN = @"Matthew Schwartz"
    }
},
{
    "credits_publisher_name23",
    new Model_LanguagesUI
    {
        EN = @"Michel Filipiak",
        CN = @"Michel Filipiak"
    }
},
{
    "credits_publisher_name24",
    new Model_LanguagesUI
    {
        EN = @"Nico Desrochers",
        CN = @"Nico Desrochers"
    }
},
{
    "credits_publisher_name25",
    new Model_LanguagesUI
    {
        EN = @"Paola García",
        CN = @"Paola García"
    }
},
{
    "credits_publisher_name26",
    new Model_LanguagesUI
    {
        EN = @"Patrick “pcj” Johnston",
        CN = @"Patrick “pcj” Johnston"
    }
},
{
    "credits_publisher_name27",
    new Model_LanguagesUI
    {
        EN = @"Pendragon Wachtel",
        CN = @"Pendragon Wachtel"
    }
},
{
    "credits_publisher_name28",
    new Model_LanguagesUI
    {
        EN = @"Veronica Irizarry",
        CN = @"Veronica Irizarry"
    }
},
{
    "credits_publisher_name29",
    new Model_LanguagesUI
    {
        EN = @"Victor Valiente",
        CN = @"Victor Valiente"
    }
},
{
    "credits_publisher_name30",
    new Model_LanguagesUI
    {
        EN = @"Vitor Hugo Moura",
        CN = @"Vitor Hugo Moura"
    }
},
{
    "credits_publisher_name31",
    new Model_LanguagesUI
    {
        EN = @"Vitoria Ama",
        CN = @"Vitoria Ama"
    }
},
{
    "credits_thank-you_title",
    new Model_LanguagesUI
    {
        EN = @"Thank you for sharing in this journey",
        CN = @"Thank you for sharing in this journey"
    }
},
{
    "credits_thank-you_text",
    new Model_LanguagesUI
    {
        EN = @"See you at Nautical Dawn",
        CN = @"See you at Nautical Dawn"
    }
},
{
    "credits_thank-you_text1",
    new Model_LanguagesUI
    {
        EN = @"“There’s a shiteater in all of us.”",
        CN = @"“There’s a shiteater in all of us.”"
    }
},
{
    "credits_extras_translation_cn",
    new Model_LanguagesUI
    {
        EN = @"Chinese Translation",
        CN = @"Chinese Translation"
    }
},
{
    "credits_extras_translation_jp",
    new Model_LanguagesUI
    {
        EN = @"Japanese Translation",
        CN = @"Japanese Translation"
    }
},
// ------------------------------------------------------------------
// Stickers
{
    "item-object-UI_sticker_psychic-duck",
    new Model_LanguagesUI
    {
        EN = @"<b>@@Stickers_NoBold can be stuck onto your</b> {5} to give you <b>special abilities</b>. Go to your {32} to try it out!<br><br>Once switched to your {65}, the @@PsychicDuck @@Sticker_NoBold allows you to engage with {19} in conversation.",
        CN = @"<b>@@Stickers_NoBold can be stuck onto your</b> {5} to give you <b>special abilities</b>. Go to your {32} to try it out!<br><br>Once switched to your {65}, the @@PsychicDuck @@Sticker_NoBold allows you to engage with {19} in conversation."
    }
},
{
    "sticker_psychic-duck",
    new Model_LanguagesUI
    {
        EN = @"Wearing the @@PsychicDuck @@Sticker_Bold allows you to converse with {19}. @@Stickers_Bold can be worn after preparing them. Passive {68} will work as long as you are wearing the @@Sticker_Bold.",
        CN = @"Wearing the @@PsychicDuck @@Sticker_Bold allows you to converse with {19}. @@Stickers_Bold can be worn after preparing them. Passive {68} will work as long as you are wearing the @@Sticker_Bold."
    }
},
{
    "sticker_animal-within",
    new Model_LanguagesUI
    {
        EN = @"Use its {79} (@@MaskCommandKey) to chomp away edible obstacles.",
        CN = @"Use its {79} (@@MaskCommandKey) to chomp away edible obstacles."
    }
},
{
    "sticker_boar-needle",
    new Model_LanguagesUI
    {
        EN = @"Allows you to enter paintings that have a doormat. Use its {79} when face-to-face with a painting that has a doormat.",
        CN = @"Allows you to enter paintings that have a doormat. Use its {79} when face-to-face with a painting that has a doormat."
    }
},
{
    "sticker_ice-spike",
    new Model_LanguagesUI
    {
        EN = @"Summon a spike from the dark spirits of the underworld. Useful for cracking brittle objects that haunted one’s past.",
        CN = @"Summon a spike from the dark spirits of the underworld. Useful for cracking brittle objects that haunted one’s past."
    }
},
{
    "sticker_melancholy-piano",
    new Model_LanguagesUI
    {
        EN = @"Follow the chords of your heart to any previously <b>remembered piano</b>.",
        CN = @"Follow the chords of your heart to any previously <b>remembered piano</b>."
    }
},
{
    "sticker_last-elevator",
    new Model_LanguagesUI
    {
        EN = @"Can be used anywhere inside {18} to take the {66} back to the {72}.",
        CN = @"Can be used anywhere inside {18} to take the {66} back to the {72}."
    }
},
{
    "sticker_let-there-be-light",
    new Model_LanguagesUI
    {
        EN = @"Illuminate dark areas. Certain areas will light up better than others.",
        CN = @"Illuminate dark areas. Certain areas will light up better than others."
    }
},
{
    "sticker_puppeteer",
    new Model_LanguagesUI
    {
        EN = @"Gain control of {73}. Your control will not be perfect though.",
        CN = @"Gain control of {73}. Your control will not be perfect though."
    }
},
{
    "sticker_my-mask",
    new Model_LanguagesUI
    {
        EN = @"A mysterious @@Sticker_Bold birthed from spirits inside and outside of {18}. It emanates a powerful aura, a feeling only its original owner can harness.",
        CN = @"A mysterious @@Sticker_Bold birthed from spirits inside and outside of {18}. It emanates a powerful aura, a feeling only its original owner can harness."
    }
},
// ------------------------------------------------------------------
// Usables
{
    "usable_super-small-key",
    new Model_LanguagesUI
    {
        EN = @"A key specifically made for regular sized keyholes. {9} <b>items</b> have a knack for fading away.",
        CN = @"A key specifically made for regular sized keyholes. {9} <b>items</b> have a knack for fading away."
    }
},
// ------------------------------------------------------------------
// Collectibles
{
    "collectible_last-well-map",
    new Model_LanguagesUI
    {
        EN = @"It seems to be a treasure map of sorts.",
        CN = @"It seems to be a treasure map of sorts."
    }
},
{
    "collectible_last-spell-recipe-book",
    new Model_LanguagesUI
    {
        EN = @"Does there have to be a last one?",
        CN = @"Does there have to be a last one?"
    }
},
{
    "collectible_speed-seal",
    new Model_LanguagesUI
    {
        EN = @"The spirits within this seal give you haste. Hold @@SpeedKey to run. Its effects only work when you are not wearing a @@Sticker_Bold (your former self).",
        CN = @"The spirits within this seal give you haste. Hold @@SpeedKey to run. Its effects only work when you are not wearing a @@Sticker_Bold (your former self)."
    }
},
// ------------------------------------------------------------------
// Tags
{
    "item_tag_impermanent",
    new Model_LanguagesUI
    {
        EN = @"【IMPERMANENT】",
        CN = @"【IMPERMANENT】"
    }
},
// ------------------------------------------------------------------
// Hotel
{
    "hotel-lobby_new-book-interactable_fullart_text",
    new Model_LanguagesUI
    {
        EN = @"Chapter <size=40>1</size>: {84}",
        CN = @"Chapter <size=40>1</size>: {84}"
    }
},
{
    "hotel-lobby_new-book-interactable_fullart_text1",
    new Model_LanguagesUI
    {
        EN = @"Chapter <size=40>2</size>:<br>{85}",
        CN = @"Chapter <size=40>2</size>:<br>{85}"
    }
},
{
    "hotel-lobby_new-book-interactable_fullart_text2",
    new Model_LanguagesUI
    {
        EN = @"Chapter <size=40>3</size>: {75}",
        CN = @"Chapter <size=40>3</size>: {75}"
    }
},
// ------------------------------------------------------------------
// Mirror Halls
{
    "notes_mirror-hall-2_hint",
    new Model_LanguagesUI
    {
        EN = @"Some in shadow, some in light,<br>Choose the switches left and right.",
        CN = @"Some in shadow, some in light,<br>Choose the switches left and right."
    }
},
// ------------------------------------------------------------------
// Ids
{
    "notes_ids_not-home",
    new Model_LanguagesUI
    {
        EN = @"Hiya!<br><br>If you’re reading this I’m most likely dancin’ this very moment. Care to join? You know what they say, a dance move a day keeps the {42} away.<br><br>Please don’t miss me too much!",
        CN = @"Hiya!<br><br>If you’re reading this I’m most likely dancin’ this very moment. Care to join? You know what they say, a dance move a day keeps the {42} away.<br><br>Please don’t miss me too much!"
    }
},
{
    "notes_ids_leave-me",
    new Model_LanguagesUI
    {
        EN = @"At half past five I

Saw him at the Fields of Sin.

I already knew

That at fifteen before dawn

I would hide these tears with blood.",
        CN = @"At half past five I

Saw him at the Fields of Sin.

I already knew

That at fifteen before dawn

I would hide these tears with blood."
    }
},
{
    "notes_ids_winter0",
    new Model_LanguagesUI
    {
        EN = @"Dear Myne,

What’s your favorite season?
Me, personally, I’m into summer,
I mean it makes my coat all itchy,
You know us sheepluffs.
But those lazy afternoons,
When you kinda just linger...",
        CN = @"Dear Myne,

What’s your favorite season?
Me, personally, I’m into summer,
I mean it makes my coat all itchy,
You know us sheepluffs.
But those lazy afternoons,
When you kinda just linger..."
    }
},
{
    "notes_ids_winter1",
    new Model_LanguagesUI
    {
        EN = @"In the shade as the sun hangs
Lower and lower, along with
All your worries.
But the first time I met you,
You know why I knew you were different?
Because in your eyes, I could see...",
        CN = @"In the shade as the sun hangs
Lower and lower, along with
All your worries.
But the first time I met you,
You know why I knew you were different?
Because in your eyes, I could see..."
    }
},
{
    "notes_ids_winter2",
    new Model_LanguagesUI
    {
        EN = @"You only believe in Winter.",
        CN = @"You only believe in Winter."
    }
},
{
    "notes_ids_winter3",
    new Model_LanguagesUI
    {
        EN = @"Yours,
Ids",
        CN = @"Yours,
Ids"
    }
},
// ------------------------------------------------------------------
// Ursie
{
    "notes_ursie_thank-you",
    new Model_LanguagesUI
    {
        EN = @"Howdy {0},

It’s been a long time coming, but I’ve come
to my senses; proving myself inside these
<b>{18}</b> walls means nothing. Why
continue to punish myself?

What’s the point of keeping a promise
that my past self made to my past self?
It’s about time I give myself permission to
move on...",
        CN = @"Howdy {0},

It’s been a long time coming, but I’ve come
to my senses; proving myself inside these
<b>{18}</b> walls means nothing. Why
continue to punish myself?

What’s the point of keeping a promise
that my past self made to my past self?
It’s about time I give myself permission to
move on..."
    }
},
{
    "notes_ursie_thank-you1",
    new Model_LanguagesUI
    {
        EN = @"{61}, {62} and I have hit the
old dusty trail. There’s bigger fish out
there for us to fry.

Thank you for assisting us even at
such lows. I’m not sure how I’ll repay you,
how about we name a cocktail after you.
How’s the <b>{0}</b> <b>Spritz</b> sound?",
        CN = @"{61}, {62} and I have hit the
old dusty trail. There’s bigger fish out
there for us to fry.

Thank you for assisting us even at
such lows. I’m not sure how I’ll repay you,
how about we name a cocktail after you.
How’s the <b>{0}</b> <b>Spritz</b> sound?"
    }
},
{
    "notes_ursie_thank-you_name",
    new Model_LanguagesUI
    {
        EN = @"Sincerely,
{33}",
        CN = @"Sincerely,
{33}"
    }
},
// ------------------------------------------------------------------
// Eileen
{
    "notes_eileen_thank-you",
    new Model_LanguagesUI
    {
        EN = @"To whomever might read this,<br><br>I’m not sure why, but I felt like I should write even if this never gets read. It seems the spikes have stopped for a bit. And although I know it won’t be forever, I can think a bit clearer now.<br><br>I wanted to let you know, I’ve finally decided to leave this place. I mean, for me at least, I know it’ll never be all sunshine and roses...",
        CN = @"To whomever might read this,<br><br>I’m not sure why, but I felt like I should write even if this never gets read. It seems the spikes have stopped for a bit. And although I know it won’t be forever, I can think a bit clearer now.<br><br>I wanted to let you know, I’ve finally decided to leave this place. I mean, for me at least, I know it’ll never be all sunshine and roses..."
    }
},
{
    "notes_eileen_thank-you1",
    new Model_LanguagesUI
    {
        EN = @"But I’ve slowly come to the realization perhaps what I’m experiencing in here is just a fraction of the things I still need to see out there. You know the walls in here don’t change much.<br><br>Anyways, see you on the other side.",
        CN = @"But I’ve slowly come to the realization perhaps what I’m experiencing in here is just a fraction of the things I still need to see out there. You know the walls in here don’t change much.<br><br>Anyways, see you on the other side."
    }
},
{
    "notes_eileen_thank-you_name",
    new Model_LanguagesUI
    {
        EN = @"Bye,<br>{11}",
        CN = @"Bye,<br>{11}"
    }
},
// ------------------------------------------------------------------
// Last Well Map
{
    "notes_last-well-map_hint",
    new Model_LanguagesUI
    {
        EN = @"Have come to trust the seasons<br>they always go in order.",
        CN = @"Have come to trust the seasons<br>they always go in order."
    }
},
{
    "notes_last-well-map_hint1",
    new Model_LanguagesUI
    {
        EN = @"<b>Start in Spring</b> as your soul quietly thaws,",
        CN = @"<b>Start in Spring</b> as your soul quietly thaws,"
    }
},
{
    "notes_last-well-map_hint2",
    new Model_LanguagesUI
    {
        EN = @"<b>End in Winter</b> before it freezes again.",
        CN = @"<b>End in Winter</b> before it freezes again."
    }
},
// ------------------------------------------------------------------
// HUD
{
    "HUD_days_today",
    new Model_LanguagesUI
    {
        EN = @"FINAL NIGHT",
        CN = @"FINAL NIGHT"
    }
},
{
    "HUD_days_today_R2",
    new Model_LanguagesUI
    {
        EN = @"F̶̥̊Ȋ̴̗N̴͉̑A̶̫͠Ḽ̸͝ ̸͈̐N̴͈͊Ì̶̙G̵̕ͅH̵̖̎T̸̼́",
        CN = @"F̶̥̊Ȋ̴̗N̴͉̑A̶̫͠Ḽ̸͝ ̸͈̐N̴͈͊Ì̶̙G̵̕ͅH̵̖̎T̸̼́"
    }
},
{
    "HUD_days_tomorrow",
    new Model_LanguagesUI
    {
        EN = @"NAUTICAL DAWN",
        CN = @"NAUTICAL DAWN"
    }
},
// ------------------------------------------------------------------
// Inventory
{
    "menu_top-bar_stickers",
    new Model_LanguagesUI
    {
        EN = @"Masks",
        CN = @"Masks"
    }
},
{
    "menu_top-bar_items",
    new Model_LanguagesUI
    {
        EN = @"Items",
        CN = @"Items"
    }
},
{
    "menu_top-bar_notes",
    new Model_LanguagesUI
    {
        EN = @"Notes ♪",
        CN = @"Notes ♪"
    }
},
{
    "menu_equipment_label",
    new Model_LanguagesUI
    {
        EN = @"Prepped Masks",
        CN = @"Prepped Masks"
    }
},
{
    "menu_item-choices_prepare",
    new Model_LanguagesUI
    {
        EN = @"Prepare",
        CN = @"Prepare"
    }
},
{
    "menu_item-choices_examine",
    new Model_LanguagesUI
    {
        EN = @"Examine",
        CN = @"Examine"
    }
},
{
    "menu_item-choices_cancel",
    new Model_LanguagesUI
    {
        EN = @"Cancel",
        CN = @"Cancel"
    }
},
{
    "menu_item-choices_drop",
    new Model_LanguagesUI
    {
        EN = @"Drop",
        CN = @"Drop"
    }
},
{
    "menu_item-choices_use",
    new Model_LanguagesUI
    {
        EN = @"Use",
        CN = @"Use"
    }
},
// ------------------------------------------------------------------
// Hints
{
    "hint_notes_sheep-chase",
    new Model_LanguagesUI
    {
        EN = @"A wild sheep chase",
        CN = @"A wild sheep chase"
    }
},
{
    "hint_notes_painting-words",
    new Model_LanguagesUI
    {
        EN = @"A word that lost its image",
        CN = @"A word that lost its image"
    }
},
{
    "hint_notes_chomp",
    new Model_LanguagesUI
    {
        EN = @"Nom nom chomp!",
        CN = @"Nom nom chomp!"
    }
},
{
    "hint_notes_third-eye",
    new Model_LanguagesUI
    {
        EN = @"To know it can hurt you but to know it won’t",
        CN = @"To know it can hurt you but to know it won’t"
    }
},
{
    "hint_notes_snow-woman",
    new Model_LanguagesUI
    {
        EN = @"Back to the elevator of sin",
        CN = @"Back to the elevator of sin"
    }
},
{
    "hint_notes_act-2-start",
    new Model_LanguagesUI
    {
        EN = @"Beginnings of a portrait",
        CN = @"Beginnings of a portrait"
    }
},
{
    "hint_notes_act-2-default",
    new Model_LanguagesUI
    {
        EN = @"Don't turn me invisible again.",
        CN = @"Don't turn me invisible again."
    }
},
{
    "hint_notes_wells-world_start",
    new Model_LanguagesUI
    {
        EN = @"From the bottom of a well",
        CN = @"From the bottom of a well"
    }
},
{
    "hint_notes_wells-world_complete",
    new Model_LanguagesUI
    {
        EN = @"Having let go down there again",
        CN = @"Having let go down there again"
    }
},
{
    "hint_notes_celestial-gardens_start",
    new Model_LanguagesUI
    {
        EN = @"Gotta take control!",
        CN = @"Gotta take control!"
    }
},
{
    "hint_notes_celestial-gardens_complete",
    new Model_LanguagesUI
    {
        EN = @"Found a way back from sea",
        CN = @"Found a way back from sea"
    }
},
{
    "hint_notes_xxx-world_start",
    new Model_LanguagesUI
    {
        EN = @"A desert longing for its oasis",
        CN = @"A desert longing for its oasis"
    }
},
{
    "hint_notes_xxx-world_complete",
    new Model_LanguagesUI
    {
        EN = @"No more mirages",
        CN = @"No more mirages"
    }
},
{
    "hint_notes_good-ending_start",
    new Model_LanguagesUI
    {
        EN = @"Permanence",
        CN = @"Permanence"
    }
},
{
    "hint_notes_good-ending_complete",
    new Model_LanguagesUI
    {
        EN = @"Tonight look at me",
        CN = @"Tonight look at me"
    }
},
{
    "hint_notes_true-ending",
    new Model_LanguagesUI
    {
        EN = @"Dawn of a new day",
        CN = @"Dawn of a new day"
    }
},
// ------------------------------------------------------------------
// Notes Tally Tracker
{
    "notes-tally-tracker_UI_label",
    new Model_LanguagesUI
    {
        EN = @"Found:",
        CN = @"Found:"
    }
},
// ------------------------------------------------------------------
// Input
{
    "input_default_submit",
    new Model_LanguagesUI
    {
        EN = @"Submit",
        CN = @"Submit"
    }
},
{
    "input_default_cancel",
    new Model_LanguagesUI
    {
        EN = @"Cancel",
        CN = @"Cancel"
    }
},
{
    "input_CCTV_disarm",
    new Model_LanguagesUI
    {
        EN = @"『 DISARM SURVEILLANCE SYSTEM 』",
        CN = @"『 DISARM SURVEILLANCE SYSTEM 』"
    }
},
// ------------------------------------------------------------------
// Input Choices
{
    "input-choices_default_yes",
    new Model_LanguagesUI
    {
        EN = @"Yes",
        CN = @"Yes"
    }
},
{
    "input-choices_default_no",
    new Model_LanguagesUI
    {
        EN = @"No",
        CN = @"No"
    }
},
// ------------------------------------------------------------------
// Painting Entrances
{
    "painting-entrances_default_choice_0",
    new Model_LanguagesUI
    {
        EN = @"Yes",
        CN = @"Yes"
    }
},
{
    "painting-entrances_default_choice_1",
    new Model_LanguagesUI
    {
        EN = @"No",
        CN = @"No"
    }
},
// ------------------------------------------------------------------
// Saving & Loading
{
    "saving_default",
    new Model_LanguagesUI
    {
        EN = @"SAVING GAME... Please do not turn off power.",
        CN = @"SAVING GAME... Please do not turn off power."
    }
},
{
    "saving_to-weekend",
    new Model_LanguagesUI
    {
        EN = @"SAVING GAME... Please do not turn off power.",
        CN = @"SAVING GAME... Please do not turn off power."
    }
},
{
    "saving_progress_default0",
    new Model_LanguagesUI
    {
        EN = @"SAVING PROGRESS",
        CN = @"SAVING PROGRESS"
    }
},
{
    "saving_progress_default1",
    new Model_LanguagesUI
    {
        EN = @"SAVING PROGRESS.",
        CN = @"SAVING PROGRESS."
    }
},
{
    "saving_progress_default2",
    new Model_LanguagesUI
    {
        EN = @"SAVING PROGRESS..",
        CN = @"SAVING PROGRESS.."
    }
},
{
    "saving_progress_default3",
    new Model_LanguagesUI
    {
        EN = @"SAVING PROGRESS...",
        CN = @"SAVING PROGRESS..."
    }
},
{
    "saving_complete_default",
    new Model_LanguagesUI
    {
        EN = @"SAVED GAME",
        CN = @"SAVED GAME"
    }
},
// ------------------------------------------------------------------
// Last Elevator Prompt
{
    "last-elevator-prompt_text",
    new Model_LanguagesUI
    {
        EN = @"『 Take the Last Elevator? 』",
        CN = @"『 Take the Last Elevator? 』"
    }
},
// ------------------------------------------------------------------
// Day Notifications
{
    "day-notification_time_sat",
    new Model_LanguagesUI
    {
        EN = @"5:00am",
        CN = @"5:00am"
    }
},
{
    "day-notification_title_sat",
    new Model_LanguagesUI
    {
        EN = @"The Final Night",
        CN = @"The Final Night"
    }
},
{
    "day-notification_subtitle_sat",
    new Model_LanguagesUI
    {
        EN = @"1 Hour Remains",
        CN = @"1 Hour Remains"
    }
},
{
    "day-notification_time_sat_R2",
    new Model_LanguagesUI
    {
        EN = @"5̵̮̆:̸̥̚0̸̩̆0̵̢͒a̸̢͆m̶͎̈",
        CN = @"5̵̮̆:̸̥̚0̸̩̆0̵̢͒a̸̢͆m̶͎̈"
    }
},
{
    "day-notification_title_sat_R2",
    new Model_LanguagesUI
    {
        EN = @"T̷h̷e̷ ̸F̶i̶n̵a̷l̸ ̴N̷i̷g̵h̸t̶",
        CN = @"T̷h̷e̷ ̸F̶i̶n̵a̷l̸ ̴N̷i̷g̵h̸t̶"
    }
},
{
    "day-notification_subtitle_sat_R2",
    new Model_LanguagesUI
    {
        EN = @"1̷̪̓ ̴͕̕H̶̗̽o̵̱͠u̶͓͝ȓ̶͎ ̴̼̽R̸̤͒e̷̖̚m̶͙̐a̶̞̿i̸̲͑ñ̶̞s̶̱̉",
        CN = @"1̷̪̓ ̴͕̕H̶̗̽o̵̱͠u̶͓͝ȓ̶͎ ̴̼̽R̸̤͒e̷̖̚m̶͙̐a̶̞̿i̸̲͑ñ̶̞s̶̱̉"
    }
},
{
    "day-notification_time_sun",
    new Model_LanguagesUI
    {
        EN = @"6:00am",
        CN = @"6:00am"
    }
},
{
    "day-notification_title_sun",
    new Model_LanguagesUI
    {
        EN = @"A New Day",
        CN = @"A New Day"
    }
},
{
    "day-notification_subtitle_sun",
    new Model_LanguagesUI
    {
        EN = @"Nautical Dawn",
        CN = @"Nautical Dawn"
    }
},
// ------------------------------------------------------------------
// Save Files
{
    "save-file_day-name_sat",
    new Model_LanguagesUI
    {
        EN = @"The Final Night",
        CN = @"The Final Night"
    }
},
{
    "save-file_day-name_sat_R2",
    new Model_LanguagesUI
    {
        EN = @"Ṫ̷̻̙h̴̖̲̒ẽ̸̡̐ ̸̨̍F̵̟͊i̶̫͊̿n̵̖̘͌a̴̜̺͒l̶͎̟̒̏ ̵͚̻̑͆N̸͉̗̔ï̴̻ğ̸̝̟h̵̥̦̽̀ť̵̡͋",
        CN = @"Ṫ̷̻̙h̴̖̲̒ẽ̸̡̐ ̸̨̍F̵̟͊i̶̫͊̿n̵̖̘͌a̴̜̺͒l̶͎̟̒̏ ̵͚̻̑͆N̸͉̗̔ï̴̻ğ̸̝̟h̵̥̦̽̀ť̵̡͋"
    }
},
{
    "save-file_day-name_sun",
    new Model_LanguagesUI
    {
        EN = @"A New Day",
        CN = @"A New Day"
    }
},
// ------------------------------------------------------------------
// Start
{
    "start_cta_press",
    new Model_LanguagesUI
    {
        EN = @"Press Enter or Space",
        CN = @"Press Enter or Space"
    }
},
{
    "start_cta_press_dynamic_gamepad",
    new Model_LanguagesUI
    {
        EN = @"Press @@InteractKey",
        CN = @"Press @@InteractKey"
    }
},
{
    "start_options_start",
    new Model_LanguagesUI
    {
        EN = @"Start",
        CN = @"Start"
    }
},
{
    "start_options_start_curse",
    new Model_LanguagesUI
    {
        EN = @"S̶͚͗t̴̹̉a̸͎̅r̴̙̐t̴̢͗",
        CN = @"S̶͚͗t̴̹̉a̸͎̅r̴̙̐t̴̢͗"
    }
},
{
    "start_options_settings",
    new Model_LanguagesUI
    {
        EN = @"Settings",
        CN = @"Settings"
    }
},
{
    "start_options_exit",
    new Model_LanguagesUI
    {
        EN = @"End",
        CN = @"End"
    }
},
{
    "start_demo-version",
    new Model_LanguagesUI
    {
        EN = @"The Demo Vers",
        CN = @"The Demo Vers"
    }
},
// ------------------------------------------------------------------
// Settings
{
    "settings_title",
    new Model_LanguagesUI
    {
        EN = @"『 Settings 』",
        CN = @"『 Settings 』"
    }
},
{
    "settings_controls",
    new Model_LanguagesUI
    {
        EN = @"Controls",
        CN = @"Controls"
    }
},
{
    "settings_sound",
    new Model_LanguagesUI
    {
        EN = @"Sound",
        CN = @"Sound"
    }
},
{
    "settings_graphics",
    new Model_LanguagesUI
    {
        EN = @"System",
        CN = @"System"
    }
},
{
    "settings_back",
    new Model_LanguagesUI
    {
        EN = @"Back",
        CN = @"Back"
    }
},
{
    "settings_main-menu",
    new Model_LanguagesUI
    {
        EN = @"Quit to Main Menu",
        CN = @"Quit to Main Menu"
    }
},
{
    "settings_prompt_main-menu",
    new Model_LanguagesUI
    {
        EN = @"Are you sure? You’ll lose all progress for the current night.",
        CN = @"Are you sure? You’ll lose all progress for the current night."
    }
},
{
    "settings_end-game",
    new Model_LanguagesUI
    {
        EN = @"Quit to Desktop",
        CN = @"Quit to Desktop"
    }
},
{
    "settings_prompt_end-game",
    new Model_LanguagesUI
    {
        EN = @"Are you sure? You’ll lose all progress for the current night.",
        CN = @"Are you sure? You’ll lose all progress for the current night."
    }
},
// ------------------------------------------------------------------
// Save Files
{
    "save-files_button_back",
    new Model_LanguagesUI
    {
        EN = @"Back",
        CN = @"Back"
    }
},
{
    "save-files_button_copy",
    new Model_LanguagesUI
    {
        EN = @"Copy",
        CN = @"Copy"
    }
},
{
    "save-files_button_delete",
    new Model_LanguagesUI
    {
        EN = @"Delete",
        CN = @"Delete"
    }
},
{
    "save-files_banner_copy",
    new Model_LanguagesUI
    {
        EN = @"Select a file to copy.",
        CN = @"Select a file to copy."
    }
},
{
    "save-files_banner_paste",
    new Model_LanguagesUI
    {
        EN = @"Copy to which slot?",
        CN = @"Copy to which slot?"
    }
},
{
    "save-files_banner_delete",
    new Model_LanguagesUI
    {
        EN = @"Select a file to delete.",
        CN = @"Select a file to delete."
    }
},
{
    "save-files_submenu_continue_message",
    new Model_LanguagesUI
    {
        EN = @"Open this file?",
        CN = @"Open this file?"
    }
},
{
    "save-files_submenu_new-game_message",
    new Model_LanguagesUI
    {
        EN = @"Begin a new adventure?",
        CN = @"Begin a new adventure?"
    }
},
{
    "save-files_submenu_delete_message",
    new Model_LanguagesUI
    {
        EN = @"This adventure will be permanently lost, okay?",
        CN = @"This adventure will be permanently lost, okay?"
    }
},
{
    "save-files_submenu_paste_message",
    new Model_LanguagesUI
    {
        EN = @"Overwrite this existing file? The previous adventure will be permanently lost.",
        CN = @"Overwrite this existing file? The previous adventure will be permanently lost."
    }
},
{
    "save-files_saved-game_empty",
    new Model_LanguagesUI
    {
        EN = @"『 Empty 』",
        CN = @"『 Empty 』"
    }
},
{
    "save-files_saved-game_masks",
    new Model_LanguagesUI
    {
        EN = @"Masks",
        CN = @"Masks"
    }
},
{
    "save-files_saved-game_notes",
    new Model_LanguagesUI
    {
        EN = @"Notes",
        CN = @"Notes"
    }
},
{
    "save-files_saved-game_last-played",
    new Model_LanguagesUI
    {
        EN = @"Last played",
        CN = @"Last played"
    }
},
{
    "save-files_saved-game_total-time",
    new Model_LanguagesUI
    {
        EN = @"Total time",
        CN = @"Total time"
    }
},
{
    "intro_loading_title",
    new Model_LanguagesUI
    {
        EN = @"Loading",
        CN = @"Loading"
    }
},
{
    "intro_loading_title1",
    new Model_LanguagesUI
    {
        EN = @"Loading.",
        CN = @"Loading."
    }
},
{
    "intro_loading_title2",
    new Model_LanguagesUI
    {
        EN = @"Loading..",
        CN = @"Loading.."
    }
},
{
    "intro_loading_title3",
    new Model_LanguagesUI
    {
        EN = @"Loading...",
        CN = @"Loading..."
    }
},
{
    "intro_loading_title_complete",
    new Model_LanguagesUI
    {
        EN = @"♥",
        CN = @"♥"
    }
},
{
    "intro_loading_please-wait",
    new Model_LanguagesUI
    {
        EN = @"Working... please wait...<br>Loading time is a bit longer than usual.",
        CN = @"Working... please wait...<br>Loading time is a bit longer than usual."
    }
},
// ------------------------------------------------------------------
// Controls
{
    "controls_title",
    new Model_LanguagesUI
    {
        EN = @"『 Controls 』",
        CN = @"『 Controls 』"
    }
},
{
    "controls_type_name",
    new Model_LanguagesUI
    {
        EN = @"Controller",
        CN = @"Controller"
    }
},
{
    "controls_type_keyboard",
    new Model_LanguagesUI
    {
        EN = @"KEYBOARD",
        CN = @"KEYBOARD"
    }
},
{
    "controls_type_joystick",
    new Model_LanguagesUI
    {
        EN = @"JOYSTICK",
        CN = @"JOYSTICK"
    }
},
{
    "controls_type_joystick_warning_unknown",
    new Model_LanguagesUI
    {
        EN = @"Current joystick is of an unknown format.
Use the keyboard to select a control to begin listening for input.",
        CN = @"Current joystick is of an unknown format.
Use the keyboard to select a control to begin listening for input."
    }
},
{
    "controls_move_name",
    new Model_LanguagesUI
    {
        EN = @"Move",
        CN = @"Move"
    }
},
{
    "controls_interact_name",
    new Model_LanguagesUI
    {
        EN = @"Interact / Confirm",
        CN = @"Interact / Confirm"
    }
},
{
    "controls_inventory_name",
    new Model_LanguagesUI
    {
        EN = @"Inventory",
        CN = @"Inventory"
    }
},
{
    "controls_inventory_name_joystick",
    new Model_LanguagesUI
    {
        EN = @"Inventory / Cancel",
        CN = @"Inventory / Cancel"
    }
},
{
    "controls_wear-mask-1_name",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 1",
        CN = @"Wear Mask 1"
    }
},
{
    "controls_wear-mask-2_name",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 2",
        CN = @"Wear Mask 2"
    }
},
{
    "controls_wear-mask-3_name",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 3",
        CN = @"Wear Mask 3"
    }
},
{
    "controls_wear-mask-4_name",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 4",
        CN = @"Wear Mask 4"
    }
},
{
    "controls_speed_name",
    new Model_LanguagesUI
    {
        EN = @"???",
        CN = @"???"
    }
},
{
    "controls_action_switch-active-sticker",
    new Model_LanguagesUI
    {
        EN = @"{80}",
        CN = @"{80}"
    }
},
{
    "controls_action_active-sticker-command",
    new Model_LanguagesUI
    {
        EN = @"{79}",
        CN = @"{79}"
    }
},
{
    "controls_wear-mask-1_name_unknown-joystick",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 1 (Axis)",
        CN = @"Wear Mask 1 (Axis)"
    }
},
{
    "controls_wear-mask-2_name_unknown-joystick",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 2 (Axis)",
        CN = @"Wear Mask 2 (Axis)"
    }
},
{
    "controls_wear-mask-3_name_unknown-joystick",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 3 (Axis)",
        CN = @"Wear Mask 3 (Axis)"
    }
},
{
    "controls_wear-mask-4_name_unknown-joystick",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 4 (Axis)",
        CN = @"Wear Mask 4 (Axis)"
    }
},
{
    "controls_wear-mask-vert_name_unknown-joystick",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 1 / 3 (Axis)",
        CN = @"Wear Mask 1 / 3 (Axis)"
    }
},
{
    "controls_wear-mask-hz_name_unknown-joystick",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 2 / 4 (Axis)",
        CN = @"Wear Mask 2 / 4 (Axis)"
    }
},
{
    "controls_invert_name",
    new Model_LanguagesUI
    {
        EN = @"▲ Invert Axis",
        CN = @"▲ Invert Axis"
    }
},
{
    "controls_invert_option_0",
    new Model_LanguagesUI
    {
        EN = @"Yes",
        CN = @"Yes"
    }
},
{
    "controls_invert_option_1",
    new Model_LanguagesUI
    {
        EN = @"No",
        CN = @"No"
    }
},
{
    "controls_no-text",
    new Model_LanguagesUI
    {
        EN = @"--",
        CN = @"--"
    }
},
{
    "controls_no-joystick",
    new Model_LanguagesUI
    {
        EN = @"No Joystick Connected",
        CN = @"No Joystick Connected"
    }
},
{
    "controls_unsupported-joystick",
    new Model_LanguagesUI
    {
        EN = @"Unsupported Joystick",
        CN = @"Unsupported Joystick"
    }
},
{
    "controls_move-hz_name",
    new Model_LanguagesUI
    {
        EN = @"Move Left / Right (Axis)",
        CN = @"Move Left / Right (Axis)"
    }
},
{
    "controls_move-vert_name",
    new Model_LanguagesUI
    {
        EN = @"Move Up / Down (Axis)",
        CN = @"Move Up / Down (Axis)"
    }
},
{
    "controls_move-up_name",
    new Model_LanguagesUI
    {
        EN = @"Move Up (D-pad)",
        CN = @"Move Up (D-pad)"
    }
},
{
    "controls_move-left_name",
    new Model_LanguagesUI
    {
        EN = @"Move Left (D-pad)",
        CN = @"Move Left (D-pad)"
    }
},
{
    "controls_move-down_name",
    new Model_LanguagesUI
    {
        EN = @"Move Down (D-pad)",
        CN = @"Move Down (D-pad)"
    }
},
{
    "controls_move-right_name",
    new Model_LanguagesUI
    {
        EN = @"Move Right (D-pad)",
        CN = @"Move Right (D-pad)"
    }
},
{
    "controls_settings_name",
    new Model_LanguagesUI
    {
        EN = @"Settings",
        CN = @"Settings"
    }
},
{
    "controls_hotkeys_joystick-action",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask",
        CN = @"Wear Mask"
    }
},
{
    "controls_title_shortcuts",
    new Model_LanguagesUI
    {
        EN = @"《 Inventory Shortcuts 》",
        CN = @"《 Inventory Shortcuts 》"
    }
},
{
    "controls_explanation_shortcuts",
    new Model_LanguagesUI
    {
        EN = @"〈 Press key while hovering over Mask inside Inventory 〉",
        CN = @"〈 Press key while hovering over Mask inside Inventory 〉"
    }
},
{
    "controls_prep_name",
    new Model_LanguagesUI
    {
        EN = @"Inventory Hotkeys",
        CN = @"Inventory Hotkeys"
    }
},
{
    "controls_button_reset",
    new Model_LanguagesUI
    {
        EN = @"Reset Controls",
        CN = @"Reset Controls"
    }
},
{
    "controls_prompt_reset",
    new Model_LanguagesUI
    {
        EN = @"All controls will be reset to default. Are you sure?",
        CN = @"All controls will be reset to default. Are you sure?"
    }
},
{
    "controls_notify_detecting",
    new Model_LanguagesUI
    {
        EN = @"Detecting input...",
        CN = @"Detecting input..."
    }
},
{
    "controls_error_move",
    new Model_LanguagesUI
    {
        EN = @"Cannot overwrite move keys",
        CN = @"Cannot overwrite move keys"
    }
},
{
    "controls_error_taken",
    new Model_LanguagesUI
    {
        EN = @"Key is taken",
        CN = @"Key is taken"
    }
},
{
    "controls_error_system",
    new Model_LanguagesUI
    {
        EN = @"Cannot overwrite menu keys",
        CN = @"Cannot overwrite menu keys"
    }
},
{
    "controls_error_hotkey",
    new Model_LanguagesUI
    {
        EN = @"Cannot overwrite Hotkeys",
        CN = @"Cannot overwrite Hotkeys"
    }
},
// ------------------------------------------------------------------
// Other Settings
{
    "graphics_title",
    new Model_LanguagesUI
    {
        EN = @"『 System 』",
        CN = @"『 System 』"
    }
},
{
    "sound_title",
    new Model_LanguagesUI
    {
        EN = @"『 Sound 』",
        CN = @"『 Sound 』"
    }
},
{
    "system_master-volume_title",
    new Model_LanguagesUI
    {
        EN = @"Master Volume",
        CN = @"Master Volume"
    }
},
{
    "system_master-volume_current",
    new Model_LanguagesUI
    {
        EN = @"current volume ▶",
        CN = @"current volume ▶"
    }
},
{
    "system_music-volume_title",
    new Model_LanguagesUI
    {
        EN = @"Music",
        CN = @"Music"
    }
},
{
    "system_sfx-volume_title",
    new Model_LanguagesUI
    {
        EN = @"SFX",
        CN = @"SFX"
    }
},
{
    "sound_button_reset",
    new Model_LanguagesUI
    {
        EN = @"Reset Sound",
        CN = @"Reset Sound"
    }
},
{
    "sound_prompt_reset",
    new Model_LanguagesUI
    {
        EN = @"All sound settings will be reset to default. Are you sure?",
        CN = @"All sound settings will be reset to default. Are you sure?"
    }
},
{
    "graphics_resolutions_title",
    new Model_LanguagesUI
    {
        EN = @"Force Windowed Resolution",
        CN = @"Force Windowed Resolution"
    }
},
{
    "graphics_resolutions_current",
    new Model_LanguagesUI
    {
        EN = @"current viewport ▶",
        CN = @"current viewport ▶"
    }
},
{
    "graphics_resolutions_help-text",
    new Model_LanguagesUI
    {
        EN = @"Select to set available resolution",
        CN = @"Select to set available resolution"
    }
},
{
    "graphics_fullscreen_title",
    new Model_LanguagesUI
    {
        EN = @"Full-Screen",
        CN = @"Full-Screen"
    }
},
{
    "graphics_fullscreen_current",
    new Model_LanguagesUI
    {
        EN = @"current mode ▶",
        CN = @"current mode ▶"
    }
},
{
    "graphics_fullscreen_on",
    new Model_LanguagesUI
    {
        EN = @"on",
        CN = @"on"
    }
},
{
    "graphics_fullscreen_off",
    new Model_LanguagesUI
    {
        EN = @"off",
        CN = @"off"
    }
},
{
    "graphics_fullscreen_current_on",
    new Model_LanguagesUI
    {
        EN = @"on",
        CN = @"on"
    }
},
{
    "graphics_fullscreen_current_off",
    new Model_LanguagesUI
    {
        EN = @"off",
        CN = @"off"
    }
},
{
    "graphics_screenshake_title",
    new Model_LanguagesUI
    {
        EN = @"Screenshake",
        CN = @"Screenshake"
    }
},
{
    "graphics_screenshake_on",
    new Model_LanguagesUI
    {
        EN = @"on",
        CN = @"on"
    }
},
{
    "graphics_screenshake_off",
    new Model_LanguagesUI
    {
        EN = @"off",
        CN = @"off"
    }
},
{
    "graphics_screenshake_current_on",
    new Model_LanguagesUI
    {
        EN = @"on",
        CN = @"on"
    }
},
{
    "graphics_screenshake_current_off",
    new Model_LanguagesUI
    {
        EN = @"off",
        CN = @"off"
    }
},
{
    "system_button_reset",
    new Model_LanguagesUI
    {
        EN = @"Reset All to Default",
        CN = @"Reset All to Default"
    }
},
{
    "system_prompt_reset",
    new Model_LanguagesUI
    {
        EN = @"All settings will be reset to default including controls and sound. Are you sure?",
        CN = @"All settings will be reset to default including controls and sound. Are you sure?"
    }
},
{
    "system_button_reset-language",
    new Model_LanguagesUI
    {
        EN = @"Reset Language",
        CN = @"Reset Language"
    }
},
{
    "system_prompt_reset-language",
    new Model_LanguagesUI
    {
        EN = @"Language preference will be reset on the next game restart. Are you sure?",
        CN = @"Language preference will be reset on the next game restart. Are you sure?"
    }
},
// ------------------------------------------------------------------
// Eileen's Mind
{
    "eileens-mind_myne_challenge_passive",
    new Model_LanguagesUI
    {
        EN = @"S̶t̶o̴p̷!̵ ̸T̴h̴e̶r̷e̷’̴s̷ ̸n̴o̵ ̴c̷h̴a̷n̴c̴e̶ ̸y̸o̷u̶ ̸w̴i̶l̸l̷ ̷m̶a̸k̴e̴ ̷i̴t̷ ̷i̵n̷ ̵t̷i̵m̴e̵!̵ ̶I̷t̵’̶s̵ ̶f̷o̷r̵ ̶y̵o̶u̷r̷ ̴o̶w̴n̷ ̵g̵o̵o̶d̶.̸",
        CN = @"S̶t̶o̴p̷!̵ ̸T̴h̴e̶r̷e̷’̴s̷ ̸n̴o̵ ̴c̷h̴a̷n̴c̴e̶ ̸y̸o̷u̶ ̸w̴i̶l̸l̷ ̷m̶a̸k̴e̴ ̷i̴t̷ ̷i̵n̷ ̵t̷i̵m̴e̵!̵ ̶I̷t̵’̶s̵ ̶f̷o̷r̵ ̶y̵o̶u̷r̷ ̴o̶w̴n̷ ̵g̵o̵o̶d̶.̸"
    }
},
{
    "eileens-mind_myne_challenge_passive1",
    new Model_LanguagesUI
    {
        EN = @"Y̶o̶u̵’̷r̶e̵ ̸g̵o̷i̶n̶g̵ ̶t̸o̶ ̵h̴u̶r̶t̴ ̴y̶o̵u̴r̵s̸e̸l̵f̷,̶ ̷c̶a̵n̸'̶t̵ ̸y̴o̸u̷ ̷s̴e̷e̵?̴!̷",
        CN = @"Y̶o̶u̵’̷r̶e̵ ̸g̵o̷i̶n̶g̵ ̶t̸o̶ ̵h̴u̶r̶t̴ ̴y̶o̵u̴r̵s̸e̸l̵f̷,̶ ̷c̶a̵n̸'̶t̵ ̸y̴o̸u̷ ̷s̴e̷e̵?̴!̷"
    }
},
{
    "eileens-mind_myne_challenge_passive2",
    new Model_LanguagesUI
    {
        EN = @"Y̵o̸u̸ ̷i̴m̷b̵e̶c̵i̷l̷e̵!̷ ̸P̶u̴t̸ ̴a̶ ̴s̷t̸o̵p̸ ̴t̷o̴ ̴t̶h̸i̴s̶ ̴r̷i̶g̸h̶t̶ ̷n̸o̸w̵!̶",
        CN = @"Y̵o̸u̸ ̷i̴m̷b̵e̶c̵i̷l̷e̵!̷ ̸P̶u̴t̸ ̴a̶ ̴s̷t̸o̵p̸ ̴t̷o̴ ̴t̶h̸i̴s̶ ̴r̷i̶g̸h̶t̶ ̷n̸o̸w̵!̶"
    }
},
{
    "eileens-mind_myne_challenge_passive3",
    new Model_LanguagesUI
    {
        EN = @"Y̴o̷u̴ ̷w̵i̴l̵l̴ ̷p̸a̷y̴ ̷f̶o̸r̵ ̴t̵h̷i̸s̴,̸ ̴m̶a̵r̴k̵ ̴m̷y̸ ̵w̴o̵r̵d̴s̸!̵",
        CN = @"Y̴o̷u̴ ̷w̵i̴l̵l̴ ̷p̸a̷y̴ ̷f̶o̸r̵ ̴t̵h̷i̸s̴,̸ ̴m̶a̵r̴k̵ ̴m̷y̸ ̵w̴o̵r̵d̴s̸!̵"
    }
},
{
    "eileens-mind_narrator_dramatic_title",
    new Model_LanguagesUI
    {
        EN = @"I CAN DO IT",
        CN = @"I CAN DO IT"
    }
},
{
    "eileens-mind_narrator_dramatic_title1",
    new Model_LanguagesUI
    {
        EN = @"《 IF I BELIEVE 》",
        CN = @"《 IF I BELIEVE 》"
    }
},
{
    "eileens-mind_narrator_dramatic",
    new Model_LanguagesUI
    {
        EN = @"* Maybe I can I mean",
        CN = @"* Maybe I can I mean"
    }
},
{
    "eileens-mind_narrator_dramatic1",
    new Model_LanguagesUI
    {
        EN = @"* Actually chances aren’t too great",
        CN = @"* Actually chances aren’t too great"
    }
},
{
    "eileens-mind_narrator_dramatic2",
    new Model_LanguagesUI
    {
        EN = @"* I’m not too sure about this anymore",
        CN = @"* I’m not too sure about this anymore"
    }
},
{
    "eileens-mind_narrator_dramatic3",
    new Model_LanguagesUI
    {
        EN = @"* Hmm you know what? Yeah maybe today’s not the day",
        CN = @"* Hmm you know what? Yeah maybe today’s not the day"
    }
},
{
    "eileens-mind_narrator_dramatic4",
    new Model_LanguagesUI
    {
        EN = @"* Hey there’s always tomorrow right!",
        CN = @"* Hey there’s always tomorrow right!"
    }
},
{
    "eileens-mind_narrator_dramatic5",
    new Model_LanguagesUI
    {
        EN = @"* Ehhhhhh -_-”",
        CN = @"* Ehhhhhh -_-”"
    }
},
{
    "eileens-mind_narrator_dramatic6",
    new Model_LanguagesUI
    {
        EN = @"* How did I get mixed up in all this?",
        CN = @"* How did I get mixed up in all this?"
    }
},
{
    "eileens-mind_narrator_dramatic7",
    new Model_LanguagesUI
    {
        EN = @"* What was I thinking, this isn’t a good idea!",
        CN = @"* What was I thinking, this isn’t a good idea!"
    }
},
{
    "eileens-mind_narrator_dramatic8",
    new Model_LanguagesUI
    {
        EN = @"* Why does it always end up like this?",
        CN = @"* Why does it always end up like this?"
    }
},
{
    "eileens-mind_narrator_dramatic9",
    new Model_LanguagesUI
    {
        EN = @"* Okay deep breaths, deep breaths",
        CN = @"* Okay deep breaths, deep breaths"
    }
},
{
    "eileens-mind_narrator_dramatic10",
    new Model_LanguagesUI
    {
        EN = @"* Wait how do you breathe again?!? *gasp*",
        CN = @"* Wait how do you breathe again?!? *gasp*"
    }
},
{
    "eileens-mind_narrator_dramatic11",
    new Model_LanguagesUI
    {
        EN = @"* OH NO OH NO OH NO",
        CN = @"* OH NO OH NO OH NO"
    }
},
// ------------------------------------------------------------------
// Demo
{
    "demo-end_note",
    new Model_LanguagesUI
    {
        EN = @"Dear {2},

You’ve reached the end of the demo, and with that, you’ve put an end to the Night Loops...... for now. Thank you so much for playing! I’m hard at work putting the final touches on the full version, which will be released soon.

If you liked what you saw, please consider wishlisting “Night Loops” on Steam :). I hope you enjoy the full version of the game, see you then!

Sincerely,
Jiaquarium",
        CN = @"Dear {2},

You’ve reached the end of the demo, and with that, you’ve put an end to the Night Loops...... for now. Thank you so much for playing! I’m hard at work putting the final touches on the full version, which will be released soon.

If you liked what you saw, please consider wishlisting “Night Loops” on Steam :). I hope you enjoy the full version of the game, see you then!

Sincerely,
Jiaquarium"
    }
},
{
    "demo-end_choice0",
    new Model_LanguagesUI
    {
        EN = @"Return to Main Menu",
        CN = @"Return to Main Menu"
    }
},
{
    "demo-end_choice1",
    new Model_LanguagesUI
    {
        EN = @"Quit to Desktop",
        CN = @"Quit to Desktop"
    }
},
{
    "demo-end_choice2",
    new Model_LanguagesUI
    {
        EN = @"Wishlist on Steam",
        CN = @"Wishlist on Steam"
    }
},
// ------------------------------------------------------------------
// Catwalk2
{
    "catwalk2_player_doubts",
    new Model_LanguagesUI
    {
        EN = @"O̷w̷n̶ ̵p̴r̸o̷b̴l̴e̶m̶s̸.̷ ̶O̵w̷n̶ ̷p̸r̵o̸b̵l̸e̶m̶s̸.̴ ̷I̴t̵'̶s̸ ̵M̷y̶ ̷o̴w̵n̴ ̶p̷r̷o̴b̷l̸e̷m̴s̶.̵",
        CN = @"O̷w̷n̶ ̵p̴r̸o̷b̴l̴e̶m̶s̸.̷ ̶O̵w̷n̶ ̷p̸r̵o̸b̵l̸e̶m̶s̸.̴ ̷I̴t̵'̶s̸ ̵M̷y̶ ̷o̴w̵n̴ ̶p̷r̷o̴b̷l̸e̷m̴s̶.̵"
    }
},
{
    "catwalk2_player_doubts1",
    new Model_LanguagesUI
    {
        EN = @"N̵e̴v̸e̷r̵ ̴h̴a̴d̴ ̴t̷o̸ ̸c̵a̷r̵e̵ ̶f̶o̴r̴ ̷a̷n̷y̷o̷n̵e̷ ̴N̶e̸v̷e̴r̷ ̶e̸l̴s̵e̶?̶ ̴Y̵o̷u̴ ̷r̶e̴a̵l̶l̸y̷ ̶t̵h̶i̸n̴k̵ ̵M̵e̴?̴",
        CN = @"N̵e̴v̸e̷r̵ ̴h̴a̴d̴ ̴t̷o̸ ̸c̵a̷r̵e̵ ̶f̶o̴r̴ ̷a̷n̷y̷o̷n̵e̷ ̴N̶e̸v̷e̴r̷ ̶e̸l̴s̵e̶?̶ ̴Y̵o̷u̴ ̷r̶e̴a̵l̶l̸y̷ ̶t̵h̶i̸n̴k̵ ̵M̵e̴?̴"
    }
},
{
    "catwalk2_player_doubts2",
    new Model_LanguagesUI
    {
        EN = @"I̸t̶ ̵m̶a̷k̷e̶s̶ ̷m̶e̸ ̶s̶i̸c̸k̷.̶ ̵I̶t̴ ̷m̷a̸k̸e̶s̷ ̶m̵e̷ ̵s̴i̸c̸k̵.̴ ̸I̵t̸ ̶m̴a̸k̷e̵s̷ ̴m̵e̸ ̶s̷i̶c̴k̵.̵",
        CN = @"I̸t̶ ̵m̶a̷k̷e̶s̶ ̷m̶e̸ ̶s̶i̸c̸k̷.̶ ̵I̶t̴ ̷m̷a̸k̸e̶s̷ ̶m̵e̷ ̵s̴i̸c̸k̵.̴ ̸I̵t̸ ̶m̴a̸k̷e̵s̷ ̴m̵e̸ ̶s̷i̶c̴k̵.̵"
    }
},
// ------------------------------------------------------------------
// Grand Mirror Room
{
    "grand-mirror-room_player_welling-up",
    new Model_LanguagesUI
    {
        EN = @"At that moment,| something began to well up inside me.",
        CN = @"At that moment,| something began to well up inside me."
    }
},
{
    "grand-mirror-room_player_welling-up1",
    new Model_LanguagesUI
    {
        EN = @"The folds of what seemed like my brain slowly filled with a syrupy, dark substance.",
        CN = @"The folds of what seemed like my brain slowly filled with a syrupy, dark substance."
    }
},
{
    "grand-mirror-room_player_welling-up2",
    new Model_LanguagesUI
    {
        EN = @"Why do I feel like I’ve been retracing a memory?",
        CN = @"Why do I feel like I’ve been retracing a memory?"
    }
},
{
    "grand-mirror-room_player_welling-up3",
    new Model_LanguagesUI
    {
        EN = @"A slippery image of a portrait...",
        CN = @"A slippery image of a portrait..."
    }
},
{
    "grand-mirror-room_player_welling-up4",
    new Model_LanguagesUI
    {
        EN = @"What if I|| <i>can’t</i>?",
        CN = @"What if I|| <i>can’t</i>?"
    }
},
{
    "grand-mirror-room_player_change",
    new Model_LanguagesUI
    {
        EN = @"Okay. I’ll talk...",
        CN = @"Okay. I’ll talk..."
    }
},
// ------------------------------------------------------------------
// Quest 1*
{
    "faceoff_ren-myne_quest0_block0_0",
    new Model_LanguagesUI
    {
        EN = @"You said you’d talk, dear, so let’s talk.",
        CN = @"You said you’d talk, dear, so let’s talk."
    }
},
{
    "faceoff_ren-myne_quest0_block0_1",
    new Model_LanguagesUI
    {
        EN = @"{0}, I’m only doing this because it’s in <i>our</i> best interest.",
        CN = @"{0}, I’m only doing this because it’s in <i>our</i> best interest."
    }
},
{
    "faceoff_ren-myne_quest0_block0_2",
    new Model_LanguagesUI
    {
        EN = @"So don’t you go around acting like a saint.",
        CN = @"So don’t you go around acting like a saint."
    }
},
{
    "faceoff_ren-myne_quest0_block2_0",
    new Model_LanguagesUI
    {
        EN = @"You’re not saving anyone down here, no.",
        CN = @"You’re not saving anyone down here, no."
    }
},
{
    "faceoff_ren-myne_quest0_block2_1",
    new Model_LanguagesUI
    {
        EN = @"On the contrary, dear, you’re hurting <i>us</i> all.",
        CN = @"On the contrary, dear, you’re hurting <i>us</i> all."
    }
},
{
    "faceoff_ren-myne_quest0_block2_2",
    new Model_LanguagesUI
    {
        EN = @"Including yourself.",
        CN = @"Including yourself."
    }
},
{
    "faceoff_ren-myne_quest0_block3_0",
    new Model_LanguagesUI
    {
        EN = @"But I get it. You’ve always liked hurting others.",
        CN = @"But I get it. You’ve always liked hurting others."
    }
},
{
    "faceoff_ren-myne_quest0_block3_1",
    new Model_LanguagesUI
    {
        EN = @"It’s just like you.",
        CN = @"It’s just like you."
    }
},
{
    "faceoff_ren-myne_quest0_block3_2",
    new Model_LanguagesUI
    {
        EN = @"In the end, you’ll end up hurting everyone around you, dear.",
        CN = @"In the end, you’ll end up hurting everyone around you, dear."
    }
},
{
    "faceoff_ren-myne_quest0_block3_3",
    new Model_LanguagesUI
    {
        EN = @"It’s the same as me.",
        CN = @"It’s the same as me."
    }
},
{
    "faceoff_ren-myne_quest0_block4_0",
    new Model_LanguagesUI
    {
        EN = @"He-he, you think you’re any different than me?",
        CN = @"He-he, you think you’re any different than me?"
    }
},
{
    "faceoff_ren-myne_quest0_block4_1",
    new Model_LanguagesUI
    {
        EN = @"Without me, there is no you.",
        CN = @"Without me, there is no you."
    }
},
{
    "faceoff_ren-myne_quest0_block4_2",
    new Model_LanguagesUI
    {
        EN = @"You were summoned here for one purpose| – to assist| <i>u|s</i>|,",
        CN = @"You were summoned here for one purpose| – to assist| <i>u|s</i>|,"
    }
},
{
    "faceoff_ren-myne_quest0_block4_3",
    new Model_LanguagesUI
    {
        EN = @"But you are well past expired now, dear.",
        CN = @"But you are well past expired now, dear."
    }
},
{
    "faceoff_ren-myne_quest0_block5_0",
    new Model_LanguagesUI
    {
        EN = @"So why don’t you do us all a favor,",
        CN = @"So why don’t you do us all a favor,"
    }
},
{
    "faceoff_ren-myne_quest0_block6_0",
    new Model_LanguagesUI
    {
        EN = @"Take your useless self out of <i>my</i> mansion.",
        CN = @"Take your useless self out of <i>my</i> mansion."
    }
},
{
    "faceoff_ren-myne_quest0_block7_0",
    new Model_LanguagesUI
    {
        EN = @"Our world is better off without you.",
        CN = @"Our world is better off without you."
    }
},
// ------------------------------------------------------------------
// Quest 2*
{
    "faceoff_ren-myne_quest1_block0_0",
    new Model_LanguagesUI
    {
        EN = @"{0}, dear, you really understand nothing!",
        CN = @"{0}, dear, you really understand nothing!"
    }
},
{
    "faceoff_ren-myne_quest1_block0_1",
    new Model_LanguagesUI
    {
        EN = @"Don’t you get it?",
        CN = @"Don’t you get it?"
    }
},
{
    "faceoff_ren-myne_quest1_block0_2",
    new Model_LanguagesUI
    {
        EN = @"<b>You are the intruder here.</b>",
        CN = @"<b>You are the intruder here.</b>"
    }
},
{
    "faceoff_ren-myne_quest1_block1_0",
    new Model_LanguagesUI
    {
        EN = @"Someone like you could never understand.",
        CN = @"Someone like you could never understand."
    }
},
{
    "faceoff_ren-myne_quest1_block1_1",
    new Model_LanguagesUI
    {
        EN = @"Someone who’s never had to care about anyone but themselves.",
        CN = @"Someone who’s never had to care about anyone but themselves."
    }
},
{
    "faceoff_ren-myne_quest1_block1_2",
    new Model_LanguagesUI
    {
        EN = @"It makes me sick.",
        CN = @"It makes me sick."
    }
},
{
    "faceoff_ren-myne_quest1_block1_glitch-response",
    new Model_LanguagesUI
    {
        EN = @"I can’t stop thinking about you no matter how hard I try.",
        CN = @"I can’t stop thinking about you no matter how hard I try."
    }
},
{
    "faceoff_ren-myne_quest1_block2_0",
    new Model_LanguagesUI
    {
        EN = @"You really have no clue, do you, dear?",
        CN = @"You really have no clue, do you, dear?"
    }
},
{
    "faceoff_ren-myne_quest1_block2_1",
    new Model_LanguagesUI
    {
        EN = @"You waltz in here, making us suffer again and again...",
        CN = @"You waltz in here, making us suffer again and again..."
    }
},
{
    "faceoff_ren-myne_quest1_block2_2",
    new Model_LanguagesUI
    {
        EN = @"They’re just <i><b>your little visits</b></i>, aren’t they?",
        CN = @"They’re just <i><b>your little visits</b></i>, aren’t they?"
    }
},
{
    "faceoff_ren-myne_quest1_block3_0",
    new Model_LanguagesUI
    {
        EN = @"I think about you when I xxxxx xxxxxx. I know you do too.",
        CN = @"I think about you when I xxxxx xxxxxx. I know you do too."
    }
},
{
    "faceoff_ren-myne_quest1_block3_1",
    new Model_LanguagesUI
    {
        EN = @"B̸u̸t̵ ̴ ŝa̵Ŷ’̵Þ ̴you s̴e̷e̸?̷ ̶ Ī Ĳu̴ ̵need m̸e̵ ̵t̷o̶ ̸ s̶ěĠøÛÊý.",
        CN = @"B̸u̸t̵ ̴ ŝa̵Ŷ’̵Þ ̴you s̴e̷e̸?̷ ̶ Ī Ĳu̴ ̵need m̸e̵ ̵t̷o̶ ̸ s̶ěĠøÛÊý."
    }
},
{
    "faceoff_ren-myne_quest1_block4_0",
    new Model_LanguagesUI
    {
        EN = @"I know you better than you know yourself, dear.",
        CN = @"I know you better than you know yourself, dear."
    }
},
{
    "faceoff_ren-myne_quest1_block4_1",
    new Model_LanguagesUI
    {
        EN = @"You’ve always been one to self sabotage.",
        CN = @"You’ve always been one to self sabotage."
    }
},
{
    "faceoff_ren-myne_quest1_block4_2",
    new Model_LanguagesUI
    {
        EN = @"You’ll take us all down with you soon enough.",
        CN = @"You’ll take us all down with you soon enough."
    }
},
{
    "faceoff_ren-myne_quest1_block4_3",
    new Model_LanguagesUI
    {
        EN = @"How about you go back up to your floor before that happens.",
        CN = @"How about you go back up to your floor before that happens."
    }
},
{
    "faceoff_ren-myne_quest1_block4_4",
    new Model_LanguagesUI
    {
        EN = @"And we can forget about all of this.",
        CN = @"And we can forget about all of this."
    }
},
{
    "faceoff_ren-myne_quest1_block4_glitch-response",
    new Model_LanguagesUI
    {
        EN = @"I hate you and I hate that I need you.",
        CN = @"I hate you and I hate that I need you."
    }
},
{
    "faceoff_ren-myne_quest1_block5_0",
    new Model_LanguagesUI
    {
        EN = @"Remember...",
        CN = @"Remember..."
    }
},
{
    "faceoff_ren-myne_quest1_block5_1",
    new Model_LanguagesUI
    {
        EN = @"<b>I</b> am the one who built this place.",
        CN = @"<b>I</b> am the one who built this place."
    }
},
{
    "faceoff_ren-myne_quest1_block6_0",
    new Model_LanguagesUI
    {
        EN = @"<b>You are nobody.</b>",
        CN = @"<b>You are nobody.</b>"
    }
},
// ------------------------------------------------------------------
// Quest 3*
{
    "faceoff_ren-myne_quest2_block0_0",
    new Model_LanguagesUI
    {
        EN = @"Please, I beg you...",
        CN = @"Please, I beg you..."
    }
},
{
    "faceoff_ren-myne_quest2_block0_1",
    new Model_LanguagesUI
    {
        EN = @"Just leave...",
        CN = @"Just leave..."
    }
},
{
    "faceoff_ren-myne_quest2_block1_0",
    new Model_LanguagesUI
    {
        EN = @"What can I do at this point?",
        CN = @"What can I do at this point?"
    }
},
{
    "faceoff_ren-myne_quest2_block1_1",
    new Model_LanguagesUI
    {
        EN = @"Just tell me, what is it that you want?",
        CN = @"Just tell me, what is it that you want?"
    }
},
{
    "faceoff_ren-myne_quest2_block1_2",
    new Model_LanguagesUI
    {
        EN = @"Please, just leave.",
        CN = @"Please, just leave."
    }
},
{
    "faceoff_ren-myne_quest2_block1_half-glitch-response",
    new Model_LanguagesUI
    {
        EN = @"Í̶̻ ̴̖̑w̴͙̎a̵̡͋n̸̛̪ț̴̄ ̶̥͑t̵̜̾ó̸̻ ̸͍̒ṃ̵́ẽ̴͜e̵̹̋ẗ̴͓ ̵̛͎y̸̞͗ö̶̭ȕ̶̳.̸̩͝",
        CN = @"Í̶̻ ̴̖̑w̴͙̎a̵̡͋n̸̛̪ț̴̄ ̶̥͑t̵̜̾ó̸̻ ̸͍̒ṃ̵́ẽ̴͜e̵̹̋ẗ̴͓ ̵̛͎y̸̞͗ö̶̭ȕ̶̳.̸̩͝"
    }
},
{
    "faceoff_ren-myne_quest2_block2_0",
    new Model_LanguagesUI
    {
        EN = @"Stop hurting yourself.",
        CN = @"Stop hurting yourself."
    }
},
{
    "faceoff_ren-myne_quest2_block2_half-glitch-response",
    new Model_LanguagesUI
    {
        EN = @"Í̶̻ ̴̖̑w̴͙̎a̵̡͋n̸̛̪ț̴̄ ̶̥͑t̵̜̾ó̸̻ ̸͍̒ṃ̵́ẽ̴͜e̵̹̋ẗ̴͓ ̵̛͎y̸̞͗ö̶̭ȕ̶̳.̸̩͝",
        CN = @"Í̶̻ ̴̖̑w̴͙̎a̵̡͋n̸̛̪ț̴̄ ̶̥͑t̵̜̾ó̸̻ ̸͍̒ṃ̵́ẽ̴͜e̵̹̋ẗ̴͓ ̵̛͎y̸̞͗ö̶̭ȕ̶̳.̸̩͝"
    }
},
{
    "faceoff_ren-myne_quest2_block3_0",
    new Model_LanguagesUI
    {
        EN = @"I need things to stay how they are.",
        CN = @"I need things to stay how they are."
    }
},
// ------------------------------------------------------------------
// Finale
{
    "faceoff_rin-myne_finale_block0_0",
    new Model_LanguagesUI
    {
        EN = @"I’m beginning to understand.",
        CN = @"I’m beginning to understand."
    }
},
{
    "faceoff_rin-myne_finale_block0_1",
    new Model_LanguagesUI
    {
        EN = @"These paintings.",
        CN = @"These paintings."
    }
},
{
    "faceoff_rin-myne_finale_block0_2",
    new Model_LanguagesUI
    {
        EN = @"This mansion.",
        CN = @"This mansion."
    }
},
{
    "faceoff_rin-myne_finale_block1_0",
    new Model_LanguagesUI
    {
        EN = @"You and I.",
        CN = @"You and I."
    }
},
// ------------------------------------------------------------------
// Comments
{
    "ddr_comments_tier1",
    new Model_LanguagesUI
    {
        EN = @"EXCELLENT",
        CN = @"EXCELLENT"
    }
},
{
    "ddr_comments_tier2",
    new Model_LanguagesUI
    {
        EN = @"DECENT",
        CN = @"DECENT"
    }
},
{
    "ddr_comments_tier3",
    new Model_LanguagesUI
    {
        EN = @"Reconsider your life decisions...",
        CN = @"Reconsider your life decisions..."
    }
},
// ------------------------------------------------------------------
// Mistakes HUD
{
    "ddr_mistakes_label",
    new Model_LanguagesUI
    {
        EN = @"『 B.A.D. 』",
        CN = @"『 B.A.D. 』"
    }
},
// ------------------------------------------------------------------
// Notifications
{
    "ddr_notifications_close",
    new Model_LanguagesUI
    {
        EN = @"LAST MOVES! GET READY!",
        CN = @"LAST MOVES! GET READY!"
    }
},

};
}

