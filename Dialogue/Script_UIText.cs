// Last created by UI Exporter at 2024-04-06 17:45:25

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://docs.google.com/spreadsheets/d/12PJr55wEMnZhO3n-c00xunSQxyDqnkutiAOx4BLh0tQ/edit#gid=814810533

public class Model_LanguagesUI
{
    public string EN { get; set; }
    public string CN { get; set; }
    public string JP { get; set; }
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
        CN = @"By",
        JP = @"By"
    }
},
{
    "intro_authors_by_name",
    new Model_LanguagesUI
    {
        EN = @"Jiaquarium",
        CN = @"Jiaquarium",
        JP = @"Jiaquarium"
    }
},
{
    "intro_authors_music",
    new Model_LanguagesUI
    {
        EN = @"Original Soundtrack By",
        CN = @"Original Soundtrack By",
        JP = @"Original Soundtrack By"
    }
},
{
    "intro_authors_music_name",
    new Model_LanguagesUI
    {
        EN = @"s3-z",
        CN = @"s3-z",
        JP = @"s3-z"
    }
},
{
    "intro_narrator_hotel",
    new Model_LanguagesUI
    {
        EN = @"I work at the front desk of a hotel right by the sea.",
        CN = @"I work at the front desk of a hotel right by the sea.",
        JP = @"I work at the front desk of a hotel right by the sea."
    }
},
{
    "intro_narrator_hotel1",
    new Model_LanguagesUI
    {
        EN = @"It’s usually pretty slow in the winter season, so the hotel owner and I worked out a pretty neat deal,",
        CN = @"It’s usually pretty slow in the winter season, so the hotel owner and I worked out a pretty neat deal,",
        JP = @"It’s usually pretty slow in the winter season, so the hotel owner and I worked out a pretty neat deal,"
    }
},
{
    "intro_narrator_hotel2",
    new Model_LanguagesUI
    {
        EN = @"I can just use one of the unoccupied rooms whenever I’m off.",
        CN = @"I can just use one of the unoccupied rooms whenever I’m off.",
        JP = @"I can just use one of the unoccupied rooms whenever I’m off."
    }
},
{
    "intro_narrator_hotel3",
    new Model_LanguagesUI
    {
        EN = @"But actually now that I think about it...",
        CN = @"But actually now that I think about it...",
        JP = @"But actually now that I think about it..."
    }
},
{
    "intro_narrator_hotel4",
    new Model_LanguagesUI
    {
        EN = @"I’m not quite sure how long it’s been since I’ve left here.",
        CN = @"I’m not quite sure how long it’s been since I’ve left here.",
        JP = @"I’m not quite sure how long it’s been since I’ve left here."
    }
},
{
    "intro_narrator_hotel5",
    new Model_LanguagesUI
    {
        EN = @"Hm, what else do you want to know?",
        CN = @"Hm, what else do you want to know?",
        JP = @"Hm, what else do you want to know?"
    }
},
{
    "intro_narrator_hotel6",
    new Model_LanguagesUI
    {
        EN = @"What it’s like working the night shift?",
        CN = @"What it’s like working the night shift?",
        JP = @"What it’s like working the night shift?"
    }
},
{
    "intro_content-warning",
    new Model_LanguagesUI
    {
        EN = @"This game is not suitable for children or those who may be easily disturbed.",
        CN = @"This game is not suitable for children or those who may be easily disturbed.",
        JP = @"このゲームは、子供や心が傷つきやすい方には適していません。"
    }
},
// ------------------------------------------------------------------
// Good Ending
{
    "good-ending_narrator_monologue",
    new Model_LanguagesUI
    {
        EN = @"This was my last day at the seaside hotel.",
        CN = @"This was my last day at the seaside hotel.",
        JP = @"これが、海辺のホテル最後の一日。"
    }
},
{
    "good-ending_narrator_monologue1",
    new Model_LanguagesUI
    {
        EN = @"How are one of these notes supposed to go anyways?",
        CN = @"How are one of these notes supposed to go anyways?",
        JP = @"とにかく、こんなノートどうすればいいんだろう？"
    }
},
{
    "good-ending_narrator_monologue1_0",
    new Model_LanguagesUI
    {
        EN = @"Needless to say, I’ll never see {18} ever again.",
        CN = @"Needless to say, I’ll never see {18} ever again.",
        JP = @"言うまでもなく、もう二度と{18}を見ることはないはず。"
    }
},
{
    "good-ending_narrator_monologue2",
    new Model_LanguagesUI
    {
        EN = @"I never imagined it to all go like this...",
        CN = @"I never imagined it to all go like this...",
        JP = @"こんな風になるなんて、思いもしなかった……"
    }
},
{
    "good-ending_narrator_monologue3",
    new Model_LanguagesUI
    {
        EN = @"My life revolved around that place for so long.",
        CN = @"My life revolved around that place for so long.",
        JP = @"私の人生はずっと、そこを中心に回っていた。"
    }
},
{
    "good-ending_narrator_monologue4",
    new Model_LanguagesUI
    {
        EN = @"And now, it’ll just be another fading memory.",
        CN = @"And now, it’ll just be another fading memory.",
        JP = @"だけどもう、記憶は薄れていくばかり。"
    }
},
{
    "good-ending_narrator_monologue5",
    new Model_LanguagesUI
    {
        EN = @"To be locked away like the rest.",
        CN = @"To be locked away like the rest.",
        JP = @"他の思い出とともに、しまわれて。"
    }
},
{
    "good-ending_narrator_monologue6",
    new Model_LanguagesUI
    {
        EN = @"......",
        CN = @"......",
        JP = @"…………"
    }
},
{
    "good-ending_narrator_monologue7",
    new Model_LanguagesUI
    {
        EN = @"Looking at the sea now...",
        CN = @"Looking at the sea now...",
        JP = @"こうして海を見ていると……"
    }
},
{
    "good-ending_narrator_monologue7_0",
    new Model_LanguagesUI
    {
        EN = @"Was I being naïve?",
        CN = @"Was I being naïve?",
        JP = @"甘く考えてたのかな？"
    }
},
{
    "good-ending_narrator_monologue8",
    new Model_LanguagesUI
    {
        EN = @"To think I could actually escape...",
        CN = @"To think I could actually escape...",
        JP = @"本当に逃げ出せると思うなんて……"
    }
},
{
    "good-ending_narrator_monologue9",
    new Model_LanguagesUI
    {
        EN = @"......",
        CN = @"......",
        JP = @"…………"
    }
},
{
    "good-ending_narrator_monologue10",
    new Model_LanguagesUI
    {
        EN = @"Were they all just in my head?",
        CN = @"Were they all just in my head?",
        JP = @"なにかもみんな、頭の中の出来事だったんだろうか？"
    }
},
{
    "good-ending_narrator_monologue11",
    new Model_LanguagesUI
    {
        EN = @"Unfinished paintings...",
        CN = @"Unfinished paintings...",
        JP = @"未完成の絵……"
    }
},
{
    "good-ending_narrator_monologue12",
    new Model_LanguagesUI
    {
        EN = @"You and <i>him</i>.",
        CN = @"You and <i>him</i>.",
        JP = @"君と、<b>彼</b>"
    }
},
{
    "good-ending_narrator_monologue12_0",
    new Model_LanguagesUI
    {
        EN = @"A self-portrait.",
        CN = @"A self-portrait.",
        JP = @"自画像。"
    }
},
{
    "good-ending_narrator_monologue12_1",
    new Model_LanguagesUI
    {
        EN = @"Am I really losing it?",
        CN = @"Am I really losing it?",
        JP = @"私、本当におかしくなっている？"
    }
},
{
    "good-ending_narrator_monologue13",
    new Model_LanguagesUI
    {
        EN = @"Maybe some questions are better left alone.",
        CN = @"Maybe some questions are better left alone.",
        JP = @"いくつかの疑問は忘れたほうがいいのかも。"
    }
},
{
    "good-ending_narrator_monologue14",
    new Model_LanguagesUI
    {
        EN = @"The most important thing though,",
        CN = @"The most important thing though,",
        JP = @"なんにせよ、大事なことはひとつ。"
    }
},
{
    "good-ending_narrator_monologue14_0",
    new Model_LanguagesUI
    {
        EN = @"Is that I finally have an end to this chapter of my life.",
        CN = @"Is that I finally have an end to this chapter of my life.",
        JP = @"私の人生の、この章がついに終わるということ。"
    }
},
{
    "good-ending_narrator_monologue14_1",
    new Model_LanguagesUI
    {
        EN = @"I’ve been at the bottom of a well for far too long now.",
        CN = @"I’ve been at the bottom of a well for far too long now.",
        JP = @"あまりに長いこと、井戸の底にいすぎていた。"
    }
},
{
    "good-ending_narrator_monologue15",
    new Model_LanguagesUI
    {
        EN = @"I know you’re just trying to help,",
        CN = @"I know you’re just trying to help,",
        JP = @"君が助けてくれようとしてたって、わかってるよ。"
    }
},
{
    "good-ending_narrator_monologue15_0",
    new Model_LanguagesUI
    {
        EN = @"Y̵o̴u̴ ̵k̸e̴p̶t̶ ̸t̵a̶k̷i̶n̸g̵ ̴m̶e̸ ̵d̴o̶w̶n̸ ̷t̵h̵e̸r̷e̴ ̷a̵n̷d̷ ̵f̴o̵r̴ ̶w̵h̶a̷t̵?̴",
        CN = @"Y̵o̴u̴ ̵k̸e̴p̶t̶ ̸t̵a̶k̷i̶n̸g̵ ̴m̶e̸ ̵d̴o̶w̶n̸ ̷t̵h̵e̸r̷e̴ ̷a̵n̷d̷ ̵f̴o̵r̴ ̶w̵h̶a̷t̵?̴",
        JP = @"҉な҉̵҉ん҉̶҉҉҉の҉̶҉た҉̴҉̵҉め҉̵҉̷҉に҉҈҉、҉̵҉あ҉҉҉そ҉̶҉̵҉こ҉҉҉̸҉ま҉̶҉で҉҈҉連҉̷҉れ҉҉҉҈҉て҉҉҉行҉̶҉̷҉っ҉҉҉て҉̷҉く҉̸҉̵҉れ҉҈҉̴҉た҉̵҉の҉̵҉？҉̶҉̸҉"
    }
},
{
    "good-ending_narrator_monologue15a_0",
    new Model_LanguagesUI
    {
        EN = @"I don’t blame you.",
        CN = @"I don’t blame you.",
        JP = @"君を責めてるわけじゃない。"
    }
},
{
    "good-ending_narrator_monologue15a_1",
    new Model_LanguagesUI
    {
        EN = @"There’s nothing you could do.",
        CN = @"There’s nothing you could do.",
        JP = @"君にできることはなかったから。"
    }
},
{
    "good-ending_narrator_monologue15_1",
    new Model_LanguagesUI
    {
        EN = @"||This weight on my shoulders...",
        CN = @"||This weight on my shoulders...",
        JP = @"||肩に感じるこの重み……"
    }
},
{
    "good-ending_narrator_monologue15_2",
    new Model_LanguagesUI
    {
        EN = @"Everyone gets what they wanted this way.",
        CN = @"Everyone gets what they wanted this way.",
        JP = @"みんな、こうして望むものを手に入れるんだ。"
    }
},
{
    "good-ending_narrator_monologue15_3",
    new Model_LanguagesUI
    {
        EN = @"No more halfway.",
        CN = @"No more halfway.",
        JP = @"もう半端者じゃない。"
    }
},
{
    "good-ending_narrator_monologue15_4",
    new Model_LanguagesUI
    {
        EN = @"My eyes... they feel incredibly heavy.",
        CN = @"My eyes... they feel incredibly heavy.",
        JP = @"目が……ものすごく重たい。"
    }
},
{
    "good-ending_narrator_monologue15_b",
    new Model_LanguagesUI
    {
        EN = @"<b>It’s {49}</b>",
        CN = @"<b>It’s {49}</b>",
        JP = @"<b>{49}だ。</b>"
    }
},
{
    "good-ending_narrator_monologue16",
    new Model_LanguagesUI
    {
        EN = @"Look,",
        CN = @"Look,",
        JP = @"見て。"
    }
},
{
    "good-ending_narrator_monologue17",
    new Model_LanguagesUI
    {
        EN = @"I knew it.",
        CN = @"I knew it.",
        JP = @"やっぱり。"
    }
},
{
    "good-ending_narrator_monologue17_1",
    new Model_LanguagesUI
    {
        EN = @"The sky is separating from the sea.",
        CN = @"The sky is separating from the sea.",
        JP = @"空と海とが、離れていく。"
    }
},
{
    "good-ending_narrator_monologue18",
    new Model_LanguagesUI
    {
        EN = @"......",
        CN = @"......",
        JP = @"......"
    }
},
{
    "good-ending_narrator_monologue19",
    new Model_LanguagesUI
    {
        EN = @"Finally, no more pain.",
        CN = @"Finally, no more pain.",
        JP = @"やっと、痛みがなくなった。"
    }
},
{
    "good-ending_the-end_title",
    new Model_LanguagesUI
    {
        EN = @"The End",
        CN = @"The End",
        JP = @"The End"
    }
},
{
    "good-ending_the-end_type",
    new Model_LanguagesUI
    {
        EN = @"RĖŇù ŮẋůĉŚū",
        CN = @"RĖŇù ŮẋůĉŚū",
        JP = @"RĖŇù ŮẋůĉŚū"
    }
},
{
    "good-ending_the-end_type1",
    new Model_LanguagesUI
    {
        EN = @"I TRUSTED YOU",
        CN = @"I TRUSTED YOU",
        JP = @"私は信じてたよ、君を"
    }
},
{
    "good-ending_the-end_type_zalgo0",
    new Model_LanguagesUI
    {
        EN = @"Űţýs ĵẋûŰĖĜ",
        CN = @"Űţýs ĵẋûŰĖĜ",
        JP = @"Űţýs ĵẋûŰĖĜ"
    }
},
{
    "good-ending_the-end_type_zalgo1",
    new Model_LanguagesUI
    {
        EN = @"ġŉŒŢ ÄẋïÈĳĥ",
        CN = @"ġŉŒŢ ÄẋïÈĳĥ",
        JP = @"ġŉŒŢ ÄẋïÈĳĥ"
    }
},
{
    "good-ending_the-end_type_zalgo2",
    new Model_LanguagesUI
    {
        EN = @"ŖĀŁő sẋšças",
        CN = @"ŖĀŁő sẋšças",
        JP = @"ŖĀŁő sẋšças"
    }
},
{
    "good-ending_the-end_type_zalgo3",
    new Model_LanguagesUI
    {
        EN = @"ăĲÑĢ EẋĎąõà",
        CN = @"ăĲÑĢ EẋĎąõà",
        JP = @"ăĲÑĢ EẋĎąõà"
    }
},
{
    "good-ending_the-end_label",
    new Model_LanguagesUI
    {
        EN = @"A Mirror Who Lost Its Reflection",
        CN = @"A Mirror Who Lost Its Reflection",
        JP = @"A Mirror Who Lost Its Reflection"
    }
},
{
    "good-ending_transition0",
    new Model_LanguagesUI
    {
        EN = @"No, this isn’t it...",
        CN = @"No, this isn’t it...",
        JP = @"No, this isn’t it..."
    }
},
{
    "good-ending_transition1",
    new Model_LanguagesUI
    {
        EN = @"The portrait is close but it won’t be finished this way.",
        CN = @"The portrait is close but it won’t be finished this way.",
        JP = @"The portrait is close but it won’t be finished this way."
    }
},
{
    "good-ending_transition-to-title",
    new Model_LanguagesUI
    {
        EN = @"...there has to be more to you.",
        CN = @"...there has to be more to you.",
        JP = @"……君ならもっとできるはず。"
    }
},
// ------------------------------------------------------------------
// True Ending
{
    "true-ending_narrator_monologue0_0",
    new Model_LanguagesUI
    {
        EN = @"This was my last day at the seaside hotel.",
        CN = @"This was my last day at the seaside hotel.",
        JP = @"これが、海辺のホテル最後の一日。"
    }
},
{
    "true-ending_narrator_monologue0_1",
    new Model_LanguagesUI
    {
        EN = @"Needless to say, I’ll never see {18} ever again.",
        CN = @"Needless to say, I’ll never see {18} ever again.",
        JP = @"言うまでもなく、もう二度と{18}を見ることはないはず。"
    }
},
{
    "true-ending_narrator_monologue1_0",
    new Model_LanguagesUI
    {
        EN = @"It’s strange to think now,",
        CN = @"It’s strange to think now,",
        JP = @"いま考えると奇妙な感じだ。"
    }
},
{
    "true-ending_narrator_monologue1_1",
    new Model_LanguagesUI
    {
        EN = @"That every decision I made, every thought I had,",
        CN = @"That every decision I made, every thought I had,",
        JP = @"私の決意も、思考も、どれもぜんぶ。"
    }
},
{
    "true-ending_narrator_monologue1_2",
    new Model_LanguagesUI
    {
        EN = @"Revolved around that place for so long.",
        CN = @"Revolved around that place for so long.",
        JP = @"ずっと、あの場所を中心に回っていた。"
    }
},
{
    "true-ending_narrator_monologue2_0",
    new Model_LanguagesUI
    {
        EN = @"......",
        CN = @"......",
        JP = @"…………"
    }
},
{
    "true-ending_narrator_monologue3_0",
    new Model_LanguagesUI
    {
        EN = @"I guess over time, it really did begin to change me,",
        CN = @"I guess over time, it really did begin to change me,",
        JP = @"時間が経つにつれて、私は大きく変わっていったんだと思う。"
    }
},
{
    "true-ending_narrator_monologue3_1",
    new Model_LanguagesUI
    {
        EN = @"Spending so many nights in there.",
        CN = @"Spending so many nights in there.",
        JP = @"あそこで、数え切れないほどの夜を過ごしたから。"
    }
},
{
    "true-ending_narrator_monologue3_2",
    new Model_LanguagesUI
    {
        EN = @"The only thing I could do was numb myself to it all...",
        CN = @"The only thing I could do was numb myself to it all...",
        JP = @"私にできるのは、すっかり麻痺するだけだった……"
    }
},
{
    "true-ending_narrator_monologue3_3",
    new Model_LanguagesUI
    {
        EN = @"As I looked out from a face that wasn’t even mine.",
        CN = @"As I looked out from a face that wasn’t even mine.",
        JP = @"自分自身の顔で、外を見ようとしていなかったから。"
    }
},
{
    "true-ending_narrator_monologue4_0",
    new Model_LanguagesUI
    {
        EN = @"......",
        CN = @"......",
        JP = @"…………"
    }
},
{
    "true-ending_narrator_monologue5_0",
    new Model_LanguagesUI
    {
        EN = @"Looking at the sea now, it’s becoming a bit clearer...",
        CN = @"Looking at the sea now, it’s becoming a bit clearer...",
        JP = @"こうして海を見ていると、少しはっきりしてきた……"
    }
},
{
    "true-ending_narrator_monologue5_1",
    new Model_LanguagesUI
    {
        EN = @"My feelings in purgatory I was helplessly dragging around.",
        CN = @"My feelings in purgatory I was helplessly dragging around.",
        JP = @"どうしようもなく引きずっていた、煉獄の感情。"
    }
},
{
    "true-ending_narrator_monologue5_2",
    new Model_LanguagesUI
    {
        EN = @"In the past, I would’ve locked them away.",
        CN = @"In the past, I would’ve locked them away.",
        JP = @"昔は、閉じ込めて見ないようにしていた。"
    }
},
{
    "true-ending_narrator_monologue6_0",
    new Model_LanguagesUI
    {
        EN = @"I’ve always feared becoming <i>only them</i>...",
        CN = @"I’ve always feared becoming <i>only them</i>...",
        JP = @"<b>あんな風</b>になってしまうのが、ずっと怖かった……"
    }
},
{
    "true-ending_narrator_monologue6_1",
    new Model_LanguagesUI
    {
        EN = @"The same hour on a winter night.",
        CN = @"The same hour on a winter night.",
        JP = @"冬の夜、こんな時間に。"
    }
},
{
    "true-ending_narrator_monologue6_2",
    new Model_LanguagesUI
    {
        EN = @"I’ve paid my visits.",
        CN = @"I’ve paid my visits.",
        JP = @"もう堂々巡りはおしまい。"
    }
},
{
    "true-ending_narrator_monologue6_3",
    new Model_LanguagesUI
    {
        EN = @"<b>I’m not just them.</b>",
        CN = @"<b>I’m not just them.</b>",
        JP = @"<b>私は、あんな風じゃない。</b>"
    }
},
{
    "true-ending_narrator_monologue7_0",
    new Model_LanguagesUI
    {
        EN = @"I’m not running anymore.",
        CN = @"I’m not running anymore.",
        JP = @"もう、逃げたりなんてしない。"
    }
},
{
    "true-ending_narrator_monologue7_1",
    new Model_LanguagesUI
    {
        EN = @"I’m ready for what’s next.",
        CN = @"I’m ready for what’s next.",
        JP = @"もう、未来を受け入れられる。"
    }
},
{
    "true-ending_narrator_monologue7_2",
    new Model_LanguagesUI
    {
        EN = @"I hope you’ll understand.",
        CN = @"I hope you’ll understand.",
        JP = @"君がわかってくれるといいな。"
    }
},
{
    "true-ending_narrator_monologue8_0",
    new Model_LanguagesUI
    {
        EN = @"Hey, Ids, look,",
        CN = @"Hey, Ids, look,",
        JP = @"ねえ、イデス、ほら。"
    }
},
{
    "true-ending_narrator_monologue8_1",
    new Model_LanguagesUI
    {
        EN = @"The sun is coming up.",
        CN = @"The sun is coming up.",
        JP = @"日が昇るよ。"
    }
},
{
    "true-ending_narrator_monologue8_b",
    new Model_LanguagesUI
    {
        EN = @"<b>It’s {49}</b>",
        CN = @"<b>It’s {49}</b>",
        JP = @"<b>{49}だ。</b>"
    }
},
{
    "true-ending_narrator_monologue9_0",
    new Model_LanguagesUI
    {
        EN = @"......",
        CN = @"......",
        JP = @"…………"
    }
},
{
    "true-ending_narrator_monologue10_0",
    new Model_LanguagesUI
    {
        EN = @"Finally, I don’t feel like sleeping anymore.",
        CN = @"Finally, I don’t feel like sleeping anymore.",
        JP = @"やっと、もう眠たくならずにすむ。"
    }
},
{
    "true-ending_the-end_type",
    new Model_LanguagesUI
    {
        EN = @"autical Dawn",
        CN = @"autical Dawn",
        JP = @"航海薄明"
    }
},
{
    "true-ending_the-end_type_start-word",
    new Model_LanguagesUI
    {
        EN = @"n",
        CN = @"n",
        JP = @"n"
    }
},
{
    "true-ending_the-end_label",
    new Model_LanguagesUI
    {
        EN = @"A True Ending",
        CN = @"A True Ending",
        JP = @"A True Ending"
    }
},
// ------------------------------------------------------------------
// Bad Ending
{
    "bad-ending_the-end_type",
    new Model_LanguagesUI
    {
        EN = @"『 THE SEALING HAS COMMENCED 』",
        CN = @"『 THE SEALING HAS COMMENCED 』",
        JP = @"『封印開始』"
    }
},
{
    "bad-ending_narrator_the-sealing_opening-words",
    new Model_LanguagesUI
    {
        EN = @"As the clock",
        CN = @"As the clock",
        JP = @"時計が"
    }
},
{
    "bad-ending_narrator_the-sealing",
    new Model_LanguagesUI
    {
        EN = @"struck {49} the ancient spell of the {22} was cast.",
        CN = @"struck {49} the ancient spell of the {22} was cast.",
        JP = @"{49}を刻んだ瞬間、{22}の古代呪文が唱えられた。"
    }
},
{
    "bad-ending_narrator_the-sealing1",
    new Model_LanguagesUI
    {
        EN = @"It is said that {18} along with all those inside were locked away,<br>permanently losing touch with the outside world.",
        CN = @"It is said that {18} along with all those inside were locked away,<br>permanently losing touch with the outside world.",
        JP = @"{18}中の者すべてが閉じ込められ、<br>外界との接点を永久に失ったと言われている。"
    }
},
{
    "bad-ending_the-end_label",
    new Model_LanguagesUI
    {
        EN = @"The Sealing",
        CN = @"The Sealing",
        JP = @"封印"
    }
},
{
    "game-over_message",
    new Model_LanguagesUI
    {
        EN = @"What to do, what to do...",
        CN = @"What to do, what to do...",
        JP = @"どうしよう……どうしよう……"
    }
},
{
    "game-over_choice_0",
    new Model_LanguagesUI
    {
        EN = @"Try this night again",
        CN = @"Try this night again",
        JP = @"あの夜にもどる"
    }
},
{
    "game-over_choice_1",
    new Model_LanguagesUI
    {
        EN = @"Go to main menu",
        CN = @"Go to main menu",
        JP = @"メインメニュー"
    }
},
// ------------------------------------------------------------------
// Credits
{
    "credits_main-staff_role0",
    new Model_LanguagesUI
    {
        EN = @"Art & Design, Writing, Programming",
        CN = @"Art & Design, Writing, Programming",
        JP = @"Art & Design, Writing, Programming"
    }
},
{
    "credits_main-staff_name0",
    new Model_LanguagesUI
    {
        EN = @"Jiaquarium",
        CN = @"Jiaquarium",
        JP = @"Jiaquarium"
    }
},
{
    "credits_main-staff_role1",
    new Model_LanguagesUI
    {
        EN = @"Music & Sound",
        CN = @"Music & Sound",
        JP = @"Music & Sound"
    }
},
{
    "credits_main-staff_name1",
    new Model_LanguagesUI
    {
        EN = @"s3-z",
        CN = @"s3-z",
        JP = @"s3-z"
    }
},
{
    "credits_main-staff_role2",
    new Model_LanguagesUI
    {
        EN = @"Art & Design",
        CN = @"Art & Design",
        JP = @"Art & Design"
    }
},
{
    "credits_main-staff_name2",
    new Model_LanguagesUI
    {
        EN = @"Estella",
        CN = @"Estella",
        JP = @"Estella"
    }
},
{
    "credits_special-thanks",
    new Model_LanguagesUI
    {
        EN = @"Special Thanks",
        CN = @"Special Thanks",
        JP = @"Special Thanks"
    }
},
{
    "credits_extras_role0",
    new Model_LanguagesUI
    {
        EN = @"Agent",
        CN = @"Agent",
        JP = @"Agent"
    }
},
{
    "credits_extras_name0",
    new Model_LanguagesUI
    {
        EN = @"Judah Silver",
        CN = @"Judah Silver",
        JP = @"Judah Silver"
    }
},
{
    "credits_extras_role1",
    new Model_LanguagesUI
    {
        EN = @"Dance Tracks",
        CN = @"Dance Tracks",
        JP = @"Dance Tracks"
    }
},
{
    "credits_extras_name1",
    new Model_LanguagesUI
    {
        EN = @"Patricia Taxxon",
        CN = @"Patricia Taxxon",
        JP = @"Patricia Taxxon"
    }
},
{
    "credits_extras_role2",
    new Model_LanguagesUI
    {
        EN = @"SFX",
        CN = @"SFX",
        JP = @"SFX"
    }
},
{
    "credits_extras_name2",
    new Model_LanguagesUI
    {
        EN = @"Taira Komori",
        CN = @"Taira Komori",
        JP = @"Taira Komori"
    }
},
{
    "credits_extras_role3",
    new Model_LanguagesUI
    {
        EN = @"Script Editing",
        CN = @"Script Editing",
        JP = @"Script Editing"
    }
},
{
    "credits_extras_name3",
    new Model_LanguagesUI
    {
        EN = @"Lynette Shen",
        CN = @"Lynette Shen",
        JP = @"Lynette Shen"
    }
},
{
    "credits_playtesters_thank-you",
    new Model_LanguagesUI
    {
        EN = @"To our playtesters and colleagues,
This would not have been possible without your feedback, course corrections, and encouragement.",
        CN = @"To our playtesters and colleagues,
This would not have been possible without your feedback, course corrections, and encouragement.",
        JP = @"テストプレイヤーのみなさんと、仲間たちへ。
みなさんからのフィードバック、軌道修正、励ましなしにこのゲームは実現しませんでした。"
    }
},
{
    "credits_playtesters_thank-you1",
    new Model_LanguagesUI
    {
        EN = @"This would not have been possible without your feedback, course corrections, and encouragement.",
        CN = @"This would not have been possible without your feedback, course corrections, and encouragement.",
        JP = @"みなさんからのフィードバック、軌道修正、励ましなしにこのゲームは実現しませんでした。"
    }
},
{
    "credits_main-playtesters_role0",
    new Model_LanguagesUI
    {
        EN = @"Key Playtesters",
        CN = @"Key Playtesters",
        JP = @"Key Playtesters"
    }
},
{
    "credits_main-playtesters_name0",
    new Model_LanguagesUI
    {
        EN = @"Alice Hua",
        CN = @"Alice Hua",
        JP = @"Alice Hua"
    }
},
{
    "credits_main-playtesters_name1",
    new Model_LanguagesUI
    {
        EN = @"Daniel Li",
        CN = @"Daniel Li",
        JP = @"Daniel Li"
    }
},
{
    "credits_main-playtesters_name2",
    new Model_LanguagesUI
    {
        EN = @"Estella Xian",
        CN = @"Estella Xian",
        JP = @"Estella Xian"
    }
},
{
    "credits_main-playtesters_name3",
    new Model_LanguagesUI
    {
        EN = @"Nathan Waters",
        CN = @"Nathan Waters",
        JP = @"Nathan Waters"
    }
},
{
    "credits_playtesters_role0",
    new Model_LanguagesUI
    {
        EN = @"Playtesters",
        CN = @"Playtesters",
        JP = @"Playtesters"
    }
},
{
    "credits_playtesters_name0",
    new Model_LanguagesUI
    {
        EN = @"Ada Lam",
        CN = @"Ada Lam",
        JP = @"Ada Lam"
    }
},
{
    "credits_playtesters_name1",
    new Model_LanguagesUI
    {
        EN = @"Arden Zhan",
        CN = @"Arden Zhan",
        JP = @"Arden Zhan"
    }
},
{
    "credits_playtesters_name2",
    new Model_LanguagesUI
    {
        EN = @"Ash Kim",
        CN = @"Ash Kim",
        JP = @"Ash Kim"
    }
},
{
    "credits_playtesters_name3",
    new Model_LanguagesUI
    {
        EN = @"David Tran",
        CN = @"David Tran",
        JP = @"David Tran"
    }
},
{
    "credits_playtesters_name4",
    new Model_LanguagesUI
    {
        EN = @"Leanna Leung",
        CN = @"Leanna Leung",
        JP = @"Leanna Leung"
    }
},
{
    "credits_playtesters_name5",
    new Model_LanguagesUI
    {
        EN = @"Lynette Shen",
        CN = @"Lynette Shen",
        JP = @"Lynette Shen"
    }
},
{
    "credits_playtesters_name6",
    new Model_LanguagesUI
    {
        EN = @"Melos Han-Tani",
        CN = @"Melos Han-Tani",
        JP = @"Melos Han-Tani"
    }
},
{
    "credits_playtesters_name7",
    new Model_LanguagesUI
    {
        EN = @"Moe Zhang",
        CN = @"Moe Zhang",
        JP = @"Moe Zhang"
    }
},
{
    "credits_playtesters_name8",
    new Model_LanguagesUI
    {
        EN = @"Randy O’ Connor",
        CN = @"Randy O’ Connor",
        JP = @"Randy O’ Connor"
    }
},
{
    "credits_playtesters_name9",
    new Model_LanguagesUI
    {
        EN = @"Sayaka Kono",
        CN = @"Sayaka Kono",
        JP = @"Sayaka Kono"
    }
},
{
    "credits_playtesters_name10",
    new Model_LanguagesUI
    {
        EN = @"Segyero Yoon",
        CN = @"Segyero Yoon",
        JP = @"Segyero Yoon"
    }
},
{
    "credits_playtesters_name11",
    new Model_LanguagesUI
    {
        EN = @"Stephen Zhao",
        CN = @"Stephen Zhao",
        JP = @"Stephen Zhao"
    }
},
{
    "credits_playtesters_name12",
    new Model_LanguagesUI
    {
        EN = @"Steven Nguyen",
        CN = @"Steven Nguyen",
        JP = @"Steven Nguyen"
    }
},
{
    "credits_playtesters_name13",
    new Model_LanguagesUI
    {
        EN = @"Tedmund Chua",
        CN = @"Tedmund Chua",
        JP = @"Tedmund Chua"
    }
},
{
    "credits_playtesters_name14",
    new Model_LanguagesUI
    {
        EN = @"Tim Goco",
        CN = @"Tim Goco",
        JP = @"Tim Goco"
    }
},
{
    "credits_playtesters_name15",
    new Model_LanguagesUI
    {
        EN = @"Yining Zheng",
        CN = @"Yining Zheng",
        JP = @"Yining Zheng"
    }
},
{
    "credits_publisher_header",
    new Model_LanguagesUI
    {
        EN = @"Publishing Staff - Freedom Games",
        CN = @"Publishing Staff - Freedom Games",
        JP = @"Publishing Staff - Freedom Games"
    }
},
{
    "credits_publisher_role0",
    new Model_LanguagesUI
    {
        EN = @"Founders",
        CN = @"Founders",
        JP = @"Founders"
    }
},
{
    "credits_publisher_role1",
    new Model_LanguagesUI
    {
        EN = @"Staff",
        CN = @"Staff",
        JP = @"Staff"
    }
},
{
    "credits_publisher_name0",
    new Model_LanguagesUI
    {
        EN = @"Donovan Duncan",
        CN = @"Donovan Duncan",
        JP = @"Donovan Duncan"
    }
},
{
    "credits_publisher_name1",
    new Model_LanguagesUI
    {
        EN = @"Ben Robinson",
        CN = @"Ben Robinson",
        JP = @"Ben Robinson"
    }
},
{
    "credits_publisher_name2",
    new Model_LanguagesUI
    {
        EN = @"Alexandre Carchano",
        CN = @"Alexandre Carchano",
        JP = @"Alexandre Carchano"
    }
},
{
    "credits_publisher_name3",
    new Model_LanguagesUI
    {
        EN = @"Amanda Hoppe",
        CN = @"Amanda Hoppe",
        JP = @"Amanda Hoppe"
    }
},
{
    "credits_publisher_name4",
    new Model_LanguagesUI
    {
        EN = @"Benjamin Tarsa",
        CN = @"Benjamin Tarsa",
        JP = @"Benjamin Tarsa"
    }
},
{
    "credits_publisher_name5",
    new Model_LanguagesUI
    {
        EN = @"Brian Borg",
        CN = @"Brian Borg",
        JP = @"Brian Borg"
    }
},
{
    "credits_publisher_name6",
    new Model_LanguagesUI
    {
        EN = @"Bryan Herren",
        CN = @"Bryan Herren",
        JP = @"Bryan Herren"
    }
},
{
    "credits_publisher_name7",
    new Model_LanguagesUI
    {
        EN = @"Carrol Dufault",
        CN = @"Carrol Dufault",
        JP = @"Carrol Dufault"
    }
},
{
    "credits_publisher_name8",
    new Model_LanguagesUI
    {
        EN = @"Danny Ryba",
        CN = @"Danny Ryba",
        JP = @"Danny Ryba"
    }
},
{
    "credits_publisher_name9",
    new Model_LanguagesUI
    {
        EN = @"Destinee Cleveland",
        CN = @"Destinee Cleveland",
        JP = @"Destinee Cleveland"
    }
},
{
    "credits_publisher_name10",
    new Model_LanguagesUI
    {
        EN = @"Elisabeth Reeve",
        CN = @"Elisabeth Reeve",
        JP = @"Elisabeth Reeve"
    }
},
{
    "credits_publisher_name11",
    new Model_LanguagesUI
    {
        EN = @"Emmanuel “Manu” Floret",
        CN = @"Emmanuel “Manu” Floret",
        JP = @"Emmanuel “Manu” Floret"
    }
},
{
    "credits_publisher_name12",
    new Model_LanguagesUI
    {
        EN = @"Emmanuel Franco",
        CN = @"Emmanuel Franco",
        JP = @"Emmanuel Franco"
    }
},
{
    "credits_publisher_name13",
    new Model_LanguagesUI
    {
        EN = @"Evan Bloyet",
        CN = @"Evan Bloyet",
        JP = @"Evan Bloyet"
    }
},
{
    "credits_publisher_name14",
    new Model_LanguagesUI
    {
        EN = @"Evan Bryant",
        CN = @"Evan Bryant",
        JP = @"Evan Bryant"
    }
},
{
    "credits_publisher_name15",
    new Model_LanguagesUI
    {
        EN = @"Harrison Floyd",
        CN = @"Harrison Floyd",
        JP = @"Harrison Floyd"
    }
},
{
    "credits_publisher_name16",
    new Model_LanguagesUI
    {
        EN = @"Ianna Dria Besa",
        CN = @"Ianna Dria Besa",
        JP = @"Ianna Dria Besa"
    }
},
{
    "credits_publisher_name17",
    new Model_LanguagesUI
    {
        EN = @"John C. Boone II",
        CN = @"John C. Boone II",
        JP = @"John C. Boone II"
    }
},
{
    "credits_publisher_name18",
    new Model_LanguagesUI
    {
        EN = @"Jonathan Motes",
        CN = @"Jonathan Motes",
        JP = @"Jonathan Motes"
    }
},
{
    "credits_publisher_name19",
    new Model_LanguagesUI
    {
        EN = @"Jordan Kahn",
        CN = @"Jordan Kahn",
        JP = @"Jordan Kahn"
    }
},
{
    "credits_publisher_name20",
    new Model_LanguagesUI
    {
        EN = @"Josh Mitchell",
        CN = @"Josh Mitchell",
        JP = @"Josh Mitchell"
    }
},
{
    "credits_publisher_name21",
    new Model_LanguagesUI
    {
        EN = @"Katie VanClieaf",
        CN = @"Katie VanClieaf",
        JP = @"Katie VanClieaf"
    }
},
{
    "credits_publisher_name22",
    new Model_LanguagesUI
    {
        EN = @"Kerri King",
        CN = @"Kerri King",
        JP = @"Kerri King"
    }
},
{
    "credits_publisher_name23",
    new Model_LanguagesUI
    {
        EN = @"Matthew Schwartz",
        CN = @"Matthew Schwartz",
        JP = @"Matthew Schwartz"
    }
},
{
    "credits_publisher_name24",
    new Model_LanguagesUI
    {
        EN = @"Michel Filipiak",
        CN = @"Michel Filipiak",
        JP = @"Michel Filipiak"
    }
},
{
    "credits_publisher_name25",
    new Model_LanguagesUI
    {
        EN = @"Nico Desrochers",
        CN = @"Nico Desrochers",
        JP = @"Nico Desrochers"
    }
},
{
    "credits_publisher_name26",
    new Model_LanguagesUI
    {
        EN = @"Paola García",
        CN = @"Paola García",
        JP = @"Paola García"
    }
},
{
    "credits_publisher_name27",
    new Model_LanguagesUI
    {
        EN = @"Patrick “pcj” Johnston",
        CN = @"Patrick “pcj” Johnston",
        JP = @"Patrick “pcj” Johnston"
    }
},
{
    "credits_publisher_name28",
    new Model_LanguagesUI
    {
        EN = @"Pendragon Wachtel",
        CN = @"Pendragon Wachtel",
        JP = @"Pendragon Wachtel"
    }
},
{
    "credits_publisher_name29",
    new Model_LanguagesUI
    {
        EN = @"Veronica Irizarry",
        CN = @"Veronica Irizarry",
        JP = @"Veronica Irizarry"
    }
},
{
    "credits_publisher_name30",
    new Model_LanguagesUI
    {
        EN = @"Victor Valiente",
        CN = @"Victor Valiente",
        JP = @"Victor Valiente"
    }
},
{
    "credits_publisher_name31",
    new Model_LanguagesUI
    {
        EN = @"Vitor Hugo Moura",
        CN = @"Vitor Hugo Moura",
        JP = @"Vitor Hugo Moura"
    }
},
{
    "credits_publisher_name32",
    new Model_LanguagesUI
    {
        EN = @"Vitoria Ama",
        CN = @"Vitoria Ama",
        JP = @"Vitoria Ama"
    }
},
{
    "credits_thank-you_title",
    new Model_LanguagesUI
    {
        EN = @"Thank you for sharing in this journey",
        CN = @"Thank you for sharing in this journey",
        JP = @"この旅へのお付き合い、ありがとうございます。"
    }
},
{
    "credits_thank-you_text",
    new Model_LanguagesUI
    {
        EN = @"See you at Nautical Dawn",
        CN = @"See you at Nautical Dawn",
        JP = @"航海薄明で会いましょう。"
    }
},
{
    "credits_thank-you_text1",
    new Model_LanguagesUI
    {
        EN = @"“There’s a shiteater in all of us.”",
        CN = @"“There’s a shiteater in all of us.”",
        JP = @"“There’s a shiteater in all of us.”"
    }
},
{
    "credits_extras_translation_cn",
    new Model_LanguagesUI
    {
        EN = @"Chinese Translation",
        CN = @"Chinese Translation",
        JP = @"Chinese Translation"
    }
},
{
    "credits_extras_translation_jp",
    new Model_LanguagesUI
    {
        EN = @"Japanese Translation",
        CN = @"Japanese Translation",
        JP = @"Japanese Translation"
    }
},
{
    "credits_extras_translation_cn_name",
    new Model_LanguagesUI
    {
        EN = @"Kakihara MasO",
        CN = @"Kakihara MasO",
        JP = @"Kakihara MasO"
    }
},
{
    "credits_extras_translation_jp_name",
    new Model_LanguagesUI
    {
        EN = @"nicolith",
        CN = @"nicolith",
        JP = @"nicolith"
    }
},
// ------------------------------------------------------------------
// Stickers
{
    "item-object-UI_sticker_psychic-duck",
    new Model_LanguagesUI
    {
        EN = @"<b>@@Stickers_NoBold can be stuck onto your</b> {5} to give you <b>special abilities</b>. Go to your {32} to try it out!<br><br>Once switched to your {65}, the @@PsychicDuck @@Sticker_NoBold allows you to engage with {19} in conversation.",
        CN = @"<b>@@Stickers_NoBold can be stuck onto your</b> {5} to give you <b>special abilities</b>. Go to your {32} to try it out!<br><br>Once switched to your {65}, the @@PsychicDuck @@Sticker_NoBold allows you to engage with {19} in conversation.",
        JP = @"<b>@@Stickers_NoBoldを着用</b>すると、<b>特殊能力</b>が使用可能。{32}で試してみましょう！<br><br>{65}を切り替えると、@@PsychicDuckの@@Sticker_NoBoldで{19}と会話できるようになります。"
    }
},
{
    "sticker_psychic-duck",
    new Model_LanguagesUI
    {
        EN = @"Wearing the @@PsychicDuck @@Sticker_Bold allows you to converse with {19}. @@Stickers_Bold can be worn after preparing them. Passive {68} will work as long as you are wearing the @@Sticker_Bold.",
        CN = @"Wearing the @@PsychicDuck @@Sticker_Bold allows you to converse with {19}. @@Stickers_Bold can be worn after preparing them. Passive {68} will work as long as you are wearing the @@Sticker_Bold.",
        JP = @"@@PsychicDuckの@@Sticker_Boldを着用すると、{19}と会話できる。@@Sticker_Boldは装備すると、着用可能に。常時{68}は@@Sticker_Bold着用するだけで発動。"
    }
},
{
    "sticker_psychic-duck_steam-deck",
    new Model_LanguagesUI
    {
        EN = @"Wearing this @@Sticker_Bold allows you to converse with {19}. @@Stickers_Bold can be worn after preparing them. Passive {68} will work as long as you are wearing the @@Sticker_Bold.",
        CN = @"Wearing this @@Sticker_Bold allows you to converse with {19}. @@Stickers_Bold can be worn after preparing them. Passive {68} will work as long as you are wearing the @@Sticker_Bold.",
        JP = @"Wearing this @@Sticker_Bold allows you to converse with {19}. @@Stickers_Bold can be worn after preparing them. Passive {68} will work as long as you are wearing the @@Sticker_Bold."
    }
},
{
    "sticker_animal-within",
    new Model_LanguagesUI
    {
        EN = @"Use its {79} (@@MaskCommandKey) to chomp away edible obstacles.",
        CN = @"Use its {79} (@@MaskCommandKey) to chomp away edible obstacles.",
        JP = @"{79}（@@MaskCommandKey）で食べられる障害物を噛み砕く。"
    }
},
{
    "sticker_boar-needle",
    new Model_LanguagesUI
    {
        EN = @"Allows you to enter paintings that have a doormat. Use its {79} when face-to-face with a painting that has a doormat.",
        CN = @"Allows you to enter paintings that have a doormat. Use its {79} when face-to-face with a painting that has a doormat.",
        JP = @"ドアマットのある絵に入れる。ドアマットのある絵の前で{79}で使用可能。"
    }
},
{
    "sticker_ice-spike",
    new Model_LanguagesUI
    {
        EN = @"Summon a spike from the dark spirits of the underworld. Useful for cracking brittle objects that haunted one’s past.",
        CN = @"Summon a spike from the dark spirits of the underworld. Useful for cracking brittle objects that haunted one’s past.",
        JP = @"冥界から、闇のトゲを召喚。過去にとらわれた壊れ物を砕くのに役立つ。"
    }
},
{
    "sticker_melancholy-piano",
    new Model_LanguagesUI
    {
        EN = @"Follow the chords of your heart to any previously <b>remembered piano</b>.",
        CN = @"Follow the chords of your heart to any previously <b>remembered piano</b>.",
        JP = @"に残っている和音に導かれ、<b>記憶しているピアノ</b>へもどれる。"
    }
},
{
    "sticker_last-elevator",
    new Model_LanguagesUI
    {
        EN = @"Can be used anywhere inside {18} to take the {66} back to the {72}.",
        CN = @"Can be used anywhere inside {18} to take the {66} back to the {72}.",
        JP = @"{18}内のどこでも{72}にもどって{66}に乗れる。"
    }
},
{
    "sticker_let-there-be-light",
    new Model_LanguagesUI
    {
        EN = @"Illuminate dark areas. Certain areas will light up better than others.",
        CN = @"Illuminate dark areas. Certain areas will light up better than others.",
        JP = @"真っ暗な領域を照らせる。周囲ほど明るくなる。"
    }
},
{
    "sticker_puppeteer",
    new Model_LanguagesUI
    {
        EN = @"Gain control of {73}. Your control will not be perfect though.",
        CN = @"Gain control of {73}. Your control will not be perfect though.",
        JP = @"{73}を操れる。ただし、完璧に制御できるわけではない。"
    }
},
{
    "sticker_my-mask",
    new Model_LanguagesUI
    {
        EN = @"A mysterious @@Sticker_Bold birthed from spirits inside and outside of {18}. It emanates a powerful aura, a feeling only its original owner can harness.",
        CN = @"A mysterious @@Sticker_Bold birthed from spirits inside and outside of {18}. It emanates a powerful aura, a feeling only its original owner can harness.",
        JP = @"{18}内外の霊によって生み出された、不思議な@@Sticker_Bold。強いオーラを放出でき、その力を引き出せるのはもとの持ち主のみ。"
    }
},
// ------------------------------------------------------------------
// Usables
{
    "usable_super-small-key",
    new Model_LanguagesUI
    {
        EN = @"A key specifically made for regular sized keyholes. {9} <b>items</b> have a knack for fading away.",
        CN = @"A key specifically made for regular sized keyholes. {9} <b>items</b> have a knack for fading away.",
        JP = @"通常サイズの鍵穴用のキー。{9}<b></b>は、どこかへ消えていく性質がある。"
    }
},
// ------------------------------------------------------------------
// Collectibles
{
    "collectible_last-well-map",
    new Model_LanguagesUI
    {
        EN = @"It seems to be a treasure map of sorts.",
        CN = @"It seems to be a treasure map of sorts.",
        JP = @"宝の地図みたいなもの。"
    }
},
{
    "collectible_last-spell-recipe-book",
    new Model_LanguagesUI
    {
        EN = @"Does there have to be a last one?",
        CN = @"Does there have to be a last one?",
        JP = @"最後じゃなきゃいけないのかな？"
    }
},
{
    "collectible_speed-seal",
    new Model_LanguagesUI
    {
        EN = @"The spirits within this seal give you haste. Hold @@SpeedKey to run. Its effects only work when you are not wearing a @@Sticker_Bold (your former self).",
        CN = @"The spirits within this seal give you haste. Hold @@SpeedKey to run. Its effects only work when you are not wearing a @@Sticker_Bold (your former self).",
        JP = @"これに封じられている精霊の力で加速できる。@@SpeedKeyで加速。@@Sticker_Boldを着けていないときのみ効果発揮。"
    }
},
// ------------------------------------------------------------------
// Tags
{
    "item_tag_impermanent",
    new Model_LanguagesUI
    {
        EN = @"【IMPERMANENT】",
        CN = @"【IMPERMANENT】",
        JP = @"【消費済み】"
    }
},
// ------------------------------------------------------------------
// Hotel
{
    "hotel-lobby_new-book-interactable_fullart_text",
    new Model_LanguagesUI
    {
        EN = @"Chapter <size=40>1</size>: {84}",
        CN = @"Chapter <size=40>1</size>: {84}",
        JP = @"第<size=40>１</size>章<br>{84}"
    }
},
{
    "hotel-lobby_new-book-interactable_fullart_text1",
    new Model_LanguagesUI
    {
        EN = @"Chapter <size=40>2</size>:<br>{85}",
        CN = @"Chapter <size=40>2</size>:<br>{85}",
        JP = @"第<size=40>２</size>章<br>{85}"
    }
},
{
    "hotel-lobby_new-book-interactable_fullart_text2",
    new Model_LanguagesUI
    {
        EN = @"Chapter <size=40>3</size>: {75}",
        CN = @"Chapter <size=40>3</size>: {75}",
        JP = @"第<size=40>３</size>章<br>{75}"
    }
},
// ------------------------------------------------------------------
// Mirror Halls
{
    "notes_mirror-hall-2_hint",
    new Model_LanguagesUI
    {
        EN = @"Some in shadow, some in light,<br>Choose the switches left and right.",
        CN = @"Some in shadow, some in light,<br>Choose the switches left and right.",
        JP = @"あるものは影に、<br>あるものは光に、<br>左右のスイッチを選べ。"
    }
},
// ------------------------------------------------------------------
// Ids
{
    "notes_ids_not-home",
    new Model_LanguagesUI
    {
        EN = @"Hiya!<br><br>If you’re reading this I’m most likely dancin’ this very moment. Care to join? You know what they say, a dance move a day keeps the {42} away.<br><br>Please don’t miss me too much!",
        CN = @"Hiya!<br><br>If you’re reading this I’m most likely dancin’ this very moment. Care to join? You know what they say, a dance move a day keeps the {42} away.<br><br>Please don’t miss me too much!",
        JP = @"やあ！<br><br>これを読んでるなら、僕はいま踊ってるんじゃないかな。一緒に踊らない？よく言うよね、一日一度のダンスで{42}いらず、って。<br><br>あんまり寂しがらないでね！"
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

I would hide these tears with blood.",
        JP = @"５：３０に

罪の荒野で彼を見かけたんだ。

知ってたよ。

夜明け前の十五分間

この涙を血で隠すことになるって。"
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
When you kinda just linger...",
        JP = @"マインへ

好きな季節はいつ？
僕は、個人的には、夏かな。
まあ、毛皮はかゆくなっちゃうよ。
ボクらモフモフヒツジだしね。
でも、のんびした午後、
ただぼんやり過ごしてると……"
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
Because in your eyes, I could see...",
        JP = @"日陰から眺めていると、
太陽がどんどん沈んでいく。
キミの悩みなんかもぜんぶ一緒に。
だけど、はじめてキミと会ったとき、
ボクとは違うと気付いたのは、なんでだと思う？
それは、キミの目に、ボクには見えたんだ……"
    }
},
{
    "notes_ids_winter2",
    new Model_LanguagesUI
    {
        EN = @"You only believe in Winter.",
        CN = @"You only believe in Winter.",
        JP = @"キミは、冬しか信じない。"
    }
},
{
    "notes_ids_winter3",
    new Model_LanguagesUI
    {
        EN = @"Yours,
Ids",
        CN = @"Yours,
Ids",
        JP = @"親愛なるイデスより"
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
move on...",
        JP = @"よう、{0}

ずいぶんかかったが、やっと気づいたんだ。<b>{18}</b>で自分を証明したって、なんの意味もない。なんで自分を責め続けなきゃならない？

昔の俺が、昔の俺に誓った約束を守る意味なんてあるか？自分を許し、前に進んでもいい頃合いだ……"
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
How’s the <b>{0}</b> <b>Spritz</b> sound?",
        JP = @"{61}と{62}、そして俺は、古くホコリまみれの道を歩いてきた。向こう側には、焼くのにうってつけの大魚があるんだ。

こんなつらい時期に、助けてくれて感謝する。どう礼していいかわからないが、お前の名にちなんだカクテルを作るよ。<b>{0}</b>・<b>スピリッツ</b>ってのはどうだ？"
    }
},
{
    "notes_ursie_thank-you_name",
    new Model_LanguagesUI
    {
        EN = @"Sincerely,
{33}",
        CN = @"Sincerely,
{33}",
        JP = @"またな、{33}"
    }
},
// ------------------------------------------------------------------
// Eileen
{
    "notes_eileen_thank-you",
    new Model_LanguagesUI
    {
        EN = @"To whomever might read this,<br><br>I’m not sure why, but I felt like I should write even if this never gets read. It seems the spikes have stopped for a bit. And although I know it won’t be forever, I can think a bit clearer now.<br><br>I wanted to let you know, I’ve finally decided to leave this place. I mean, for me at least, I know it’ll never be all sunshine and roses...",
        CN = @"To whomever might read this,<br><br>I’m not sure why, but I felt like I should write even if this never gets read. It seems the spikes have stopped for a bit. And although I know it won’t be forever, I can think a bit clearer now.<br><br>I wanted to let you know, I’ve finally decided to leave this place. I mean, for me at least, I know it’ll never be all sunshine and roses...",
        JP = @"誰がこれを読むかわかりませんが、<br><br>なぜだか、誰にも読まれないとしても、書きたくなりました。ちょっとの間、トゲが止まったみたいです。永遠に続くわけじゃないでしょうが、いまは少し頭がすっきりしています。<br><br>私が伝えたいのは、ここを離れる決意がようやくついたんです。つまり、少なくとも私にとっては、ここが日の光やバラで満たされることはありませんから……"
    }
},
{
    "notes_eileen_thank-you1",
    new Model_LanguagesUI
    {
        EN = @"But I’ve slowly come to the realization perhaps what I’m experiencing in here is just a fraction of the things I still need to see out there. You know the walls in here don’t change much.<br><br>Anyways, see you on the other side.",
        CN = @"But I’ve slowly come to the realization perhaps what I’m experiencing in here is just a fraction of the things I still need to see out there. You know the walls in here don’t change much.<br><br>Anyways, see you on the other side.",
        JP = @"しかし、ゆっくりと理解しはじめてきました。私がここに期待しているものは、外で出会うものの、ほんの一部に過ぎないのかもしれません。ここの壁は、ずっと変わってませんから。<br><br>なんであれ、向こう側で会いましょう。"
    }
},
{
    "notes_eileen_thank-you_name",
    new Model_LanguagesUI
    {
        EN = @"Bye,<br>{11}",
        CN = @"Bye,<br>{11}",
        JP = @"では<br>{11}"
    }
},
// ------------------------------------------------------------------
// Last Well Map
{
    "notes_last-well-map_hint",
    new Model_LanguagesUI
    {
        EN = @"Have come to trust the seasons<br>they always go in order.",
        CN = @"Have come to trust the seasons<br>they always go in order.",
        JP = @"季節を信頼するんだ<br>
順番通り来る季節を"
    }
},
{
    "notes_last-well-map_hint1",
    new Model_LanguagesUI
    {
        EN = @"<b>Start in Spring</b> as your soul quietly thaws,",
        CN = @"<b>Start in Spring</b> as your soul quietly thaws,",
        JP = @"<b>春ははじまり</b>、心が静かに解けていく。"
    }
},
{
    "notes_last-well-map_hint2",
    new Model_LanguagesUI
    {
        EN = @"<b>End in Winter</b> before it freezes again.",
        CN = @"<b>End in Winter</b> before it freezes again.",
        JP = @"<b>冬はおわり</b>、心が再び凍りつく。"
    }
},
// ------------------------------------------------------------------
// HUD
{
    "HUD_days_today",
    new Model_LanguagesUI
    {
        EN = @"FINAL NIGHT",
        CN = @"FINAL NIGHT",
        JP = @"最終夜"
    }
},
{
    "HUD_days_today_R2",
    new Model_LanguagesUI
    {
        EN = @"F̶̥̊Ȋ̴̗N̴͉̑A̶̫͠Ḽ̸͝ ̸͈̐N̴͈͊Ì̶̙G̵̕ͅH̵̖̎T̸̼́",
        CN = @"F̶̥̊Ȋ̴̗N̴͉̑A̶̫͠Ḽ̸͝ ̸͈̐N̴͈͊Ì̶̙G̵̕ͅH̵̖̎T̸̼́",
        JP = @"最҉̧͍̟͕̬̣͙̫̝̞̥͖͉終҈̨̫̤̝̬͖͍̤̤̤̰̫̗͖ͅ夜҉̢̗̳͔̞̘̰̜̟͍̥̣ͅ"
    }
},
{
    "HUD_days_tomorrow",
    new Model_LanguagesUI
    {
        EN = @"NAUTICAL DAWN",
        CN = @"NAUTICAL DAWN",
        JP = @"航海薄明"
    }
},
// ------------------------------------------------------------------
// Inventory
{
    "menu_top-bar_stickers",
    new Model_LanguagesUI
    {
        EN = @"Masks",
        CN = @"Masks",
        JP = @"仮面"
    }
},
{
    "menu_top-bar_items",
    new Model_LanguagesUI
    {
        EN = @"Items",
        CN = @"Items",
        JP = @"アイテム"
    }
},
{
    "menu_top-bar_notes",
    new Model_LanguagesUI
    {
        EN = @"Notes ♪",
        CN = @"Notes ♪",
        JP = @"音符 ♪"
    }
},
{
    "menu_equipment_label",
    new Model_LanguagesUI
    {
        EN = @"Prepped Masks",
        CN = @"Prepped Masks",
        JP = @"装備済み仮面"
    }
},
{
    "menu_item-choices_prepare",
    new Model_LanguagesUI
    {
        EN = @"Prepare",
        CN = @"Prepare",
        JP = @"装備"
    }
},
{
    "menu_item-choices_examine",
    new Model_LanguagesUI
    {
        EN = @"Examine",
        CN = @"Examine",
        JP = @"調べる"
    }
},
{
    "menu_item-choices_cancel",
    new Model_LanguagesUI
    {
        EN = @"Cancel",
        CN = @"Cancel",
        JP = @"やめる"
    }
},
{
    "menu_item-choices_drop",
    new Model_LanguagesUI
    {
        EN = @"Drop",
        CN = @"Drop",
        JP = @"落とす"
    }
},
{
    "menu_item-choices_use",
    new Model_LanguagesUI
    {
        EN = @"Use",
        CN = @"Use",
        JP = @"使う"
    }
},
// ------------------------------------------------------------------
// Hints
{
    "hint_notes_sheep-chase",
    new Model_LanguagesUI
    {
        EN = @"A wild sheep chase",
        CN = @"A wild sheep chase",
        JP = @"野羊追跡"
    }
},
{
    "hint_notes_painting-words",
    new Model_LanguagesUI
    {
        EN = @"A word that lost its image",
        CN = @"A word that lost its image",
        JP = @"イメージなき言葉"
    }
},
{
    "hint_notes_chomp",
    new Model_LanguagesUI
    {
        EN = @"Nom nom chomp!",
        CN = @"Nom nom chomp!",
        JP = @"むしゃむしゃガブッ！"
    }
},
{
    "hint_notes_third-eye",
    new Model_LanguagesUI
    {
        EN = @"To know it can hurt you but to know it won’t",
        CN = @"To know it can hurt you but to know it won’t",
        JP = @"傷つけるけれど、傷つけないもの"
    }
},
{
    "hint_notes_snow-woman",
    new Model_LanguagesUI
    {
        EN = @"Back to the elevator of sin",
        CN = @"Back to the elevator of sin",
        JP = @"罪のエレベーターへの帰還"
    }
},
{
    "hint_notes_act-2-start",
    new Model_LanguagesUI
    {
        EN = @"Beginnings of a portrait",
        CN = @"Beginnings of a portrait",
        JP = @"肖像画のはじまり"
    }
},
{
    "hint_notes_act-2-default",
    new Model_LanguagesUI
    {
        EN = @"Don't turn me invisible again.",
        CN = @"Don't turn me invisible again.",
        JP = @"もう見えなくならないで"
    }
},
{
    "hint_notes_wells-world_start",
    new Model_LanguagesUI
    {
        EN = @"From the bottom of a well",
        CN = @"From the bottom of a well",
        JP = @"井戸の底から"
    }
},
{
    "hint_notes_wells-world_complete",
    new Model_LanguagesUI
    {
        EN = @"Having let go down there again",
        CN = @"Having let go down there again",
        JP = @"また下ろう"
    }
},
{
    "hint_notes_celestial-gardens_start",
    new Model_LanguagesUI
    {
        EN = @"Gotta take control!",
        CN = @"Gotta take control!",
        JP = @"操らなくっちゃ！"
    }
},
{
    "hint_notes_celestial-gardens_complete",
    new Model_LanguagesUI
    {
        EN = @"Found a way back from sea",
        CN = @"Found a way back from sea",
        JP = @"海へもどる道を求めて"
    }
},
{
    "hint_notes_xxx-world_start",
    new Model_LanguagesUI
    {
        EN = @"A desert longing for its oasis",
        CN = @"A desert longing for its oasis",
        JP = @"砂は水を求める"
    }
},
{
    "hint_notes_xxx-world_complete",
    new Model_LanguagesUI
    {
        EN = @"No more mirages",
        CN = @"No more mirages",
        JP = @"さよなら幻影"
    }
},
{
    "hint_notes_good-ending_start",
    new Model_LanguagesUI
    {
        EN = @"Permanence",
        CN = @"Permanence",
        JP = @"永劫"
    }
},
{
    "hint_notes_good-ending_complete",
    new Model_LanguagesUI
    {
        EN = @"Tonight look at me",
        CN = @"Tonight look at me",
        JP = @"今夜は私を見て"
    }
},
{
    "hint_notes_true-ending",
    new Model_LanguagesUI
    {
        EN = @"Dawn of a new day",
        CN = @"Dawn of a new day",
        JP = @"新しき日の夜明け"
    }
},
// ------------------------------------------------------------------
// Notes Tally Tracker
{
    "notes-tally-tracker_UI_label",
    new Model_LanguagesUI
    {
        EN = @"Found:",
        CN = @"Found:",
        JP = @"発見済み："
    }
},
// ------------------------------------------------------------------
// Input
{
    "input_default_submit",
    new Model_LanguagesUI
    {
        EN = @"Submit",
        CN = @"Submit",
        JP = @"決定"
    }
},
{
    "input_default_cancel",
    new Model_LanguagesUI
    {
        EN = @"Cancel",
        CN = @"Cancel",
        JP = @"キャンセル"
    }
},
{
    "input_CCTV_disarm",
    new Model_LanguagesUI
    {
        EN = @"『 DISARM SURVEILLANCE SYSTEM 』",
        CN = @"『 DISARM SURVEILLANCE SYSTEM 』",
        JP = @"『 監視システム解除 』"
    }
},
// ------------------------------------------------------------------
// Input Choices
{
    "input-choices_default_yes",
    new Model_LanguagesUI
    {
        EN = @"Yes",
        CN = @"Yes",
        JP = @"はい"
    }
},
{
    "input-choices_default_no",
    new Model_LanguagesUI
    {
        EN = @"No",
        CN = @"No",
        JP = @"いいえ"
    }
},
// ------------------------------------------------------------------
// Painting Entrances
{
    "painting-entrances_default_choice_0",
    new Model_LanguagesUI
    {
        EN = @"Yes",
        CN = @"Yes",
        JP = @"Yes"
    }
},
{
    "painting-entrances_default_choice_1",
    new Model_LanguagesUI
    {
        EN = @"No",
        CN = @"No",
        JP = @"No"
    }
},
// ------------------------------------------------------------------
// Saving & Loading
{
    "saving_default",
    new Model_LanguagesUI
    {
        EN = @"SAVING GAME... Please do not turn off power.",
        CN = @"SAVING GAME... Please do not turn off power.",
        JP = @"セーブ中……電源を消さないでください"
    }
},
{
    "saving_to-weekend",
    new Model_LanguagesUI
    {
        EN = @"SAVING GAME... Please do not turn off power.",
        CN = @"SAVING GAME... Please do not turn off power.",
        JP = @"セーブ中……電源を消さないでください"
    }
},
{
    "saving_progress_default0",
    new Model_LanguagesUI
    {
        EN = @"SAVING PROGRESS",
        CN = @"SAVING PROGRESS",
        JP = @"セーブ中"
    }
},
{
    "saving_progress_default1",
    new Model_LanguagesUI
    {
        EN = @"SAVING PROGRESS.",
        CN = @"SAVING PROGRESS.",
        JP = @"セーブ中…"
    }
},
{
    "saving_progress_default2",
    new Model_LanguagesUI
    {
        EN = @"SAVING PROGRESS..",
        CN = @"SAVING PROGRESS..",
        JP = @"セーブ中……"
    }
},
{
    "saving_progress_default3",
    new Model_LanguagesUI
    {
        EN = @"SAVING PROGRESS...",
        CN = @"SAVING PROGRESS...",
        JP = @"セーブ中………"
    }
},
{
    "saving_complete_default",
    new Model_LanguagesUI
    {
        EN = @"SAVED GAME",
        CN = @"SAVED GAME",
        JP = @"セーブ完了"
    }
},
// ------------------------------------------------------------------
// Last Elevator Prompt
{
    "last-elevator-prompt_text",
    new Model_LanguagesUI
    {
        EN = @"『 Take the Last Elevator? 』",
        CN = @"『 Take the Last Elevator? 』",
        JP = @"『 最終エレベーターに乗りますか？ 』"
    }
},
// ------------------------------------------------------------------
// Day Notifications
{
    "day-notification_time_sat",
    new Model_LanguagesUI
    {
        EN = @"5:00am",
        CN = @"5:00am",
        JP = @"午前5時"
    }
},
{
    "day-notification_title_sat",
    new Model_LanguagesUI
    {
        EN = @"The Final Night",
        CN = @"The Final Night",
        JP = @"最終夜"
    }
},
{
    "day-notification_subtitle_sat",
    new Model_LanguagesUI
    {
        EN = @"1 Hour Remains",
        CN = @"1 Hour Remains",
        JP = @"残り1時間"
    }
},
{
    "day-notification_time_sat_R2",
    new Model_LanguagesUI
    {
        EN = @"5̵̮̆:̸̥̚0̸̩̆0̵̢͒a̸̢͆m̶͎̈",
        CN = @"5̵̮̆:̸̥̚0̸̩̆0̵̢͒a̸̢͆m̶͎̈",
        JP = @"午҉͇̥̦̳̫̯͖̠͔̫͎̣̫̫̎͑͂̓̑̈͋̒̓̏͌̋̓̎́̿̌́̽́̊͂̚ͅ前҉͙͕̜̣͎͔̗͖̰͔̝͓͈̲̅͌͋̂̊͛͂̋̐̓̾͛5̶̲̭̘̩̭̱̰̩͙͈̳̖͎̩̣̲͕͙̑̒̀̇͛̍̑̇̈́̽̐̒時"
    }
},
{
    "day-notification_title_sat_R2",
    new Model_LanguagesUI
    {
        EN = @"T̷h̷e̷ ̸F̶i̶n̵a̷l̸ ̴N̷i̷g̵h̸t̶",
        CN = @"T̷h̷e̷ ̸F̶i̶n̵a̷l̸ ̴N̷i̷g̵h̸t̶",
        JP = @"最҉̧͍̟͕̬̣͙̫̝̞̥͖͉終҈̨̫̤̝̬͖͍̤̤̤̰̫̗͖ͅ夜҉̢̗̳͔̞̘̰̜̟͍̥̣ͅ"
    }
},
{
    "day-notification_subtitle_sat_R2",
    new Model_LanguagesUI
    {
        EN = @"1̷̪̓ ̴͕̕H̶̗̽o̵̱͠u̶͓͝ȓ̶͎ ̴̼̽R̸̤͒e̷̖̚m̶͙̐a̶̞̿i̸̲͑ñ̶̞s̶̱̉",
        CN = @"1̷̪̓ ̴͕̕H̶̗̽o̵̱͠u̶͓͝ȓ̶͎ ̴̼̽R̸̤͒e̷̖̚m̶͙̐a̶̞̿i̸̲͑ñ̶̞s̶̱̉",
        JP = @"残҈̦͍̬̟̩͚͈̘̲͚͚̞̗͜ͅり҉̧͕̮̲͈͔͚̥͈̗1̵̧̣͉͚̟̳͇̯̗̞̠̲̣͕̳͓時҉̡͖̠̙̰̫̱͙̥̩̞̩͚̮͕間҉̥̠̖̱̤͍̟̘͚͙̖̱͜ͅͅ"
    }
},
{
    "day-notification_time_sun",
    new Model_LanguagesUI
    {
        EN = @"6:00am",
        CN = @"6:00am",
        JP = @"午前6時"
    }
},
{
    "day-notification_title_sun",
    new Model_LanguagesUI
    {
        EN = @"A New Day",
        CN = @"A New Day",
        JP = @"新しき一日"
    }
},
{
    "day-notification_subtitle_sun",
    new Model_LanguagesUI
    {
        EN = @"Nautical Dawn",
        CN = @"Nautical Dawn",
        JP = @"航海薄明"
    }
},
// ------------------------------------------------------------------
// Save Files
{
    "save-file_day-name_sat",
    new Model_LanguagesUI
    {
        EN = @"The Final Night",
        CN = @"The Final Night",
        JP = @"最終夜"
    }
},
{
    "save-file_day-name_sat_R2",
    new Model_LanguagesUI
    {
        EN = @"Ṫ̷̻̙h̴̖̲̒ẽ̸̡̐ ̸̨̍F̵̟͊i̶̫͊̿n̵̖̘͌a̴̜̺͒l̶͎̟̒̏ ̵͚̻̑͆N̸͉̗̔ï̴̻ğ̸̝̟h̵̥̦̽̀ť̵̡͋",
        CN = @"Ṫ̷̻̙h̴̖̲̒ẽ̸̡̐ ̸̨̍F̵̟͊i̶̫͊̿n̵̖̘͌a̴̜̺͒l̶͎̟̒̏ ̵͚̻̑͆N̸͉̗̔ï̴̻ğ̸̝̟h̵̥̦̽̀ť̵̡͋",
        JP = @"最҉̧͍̟͕̬̣͙̫̝̞̥͖͉終҈̨̫̤̝̬͖͍̤̤̤̰̫̗͖ͅ夜҉̢̗̳͔̞̘̰̜̟͍̥̣ͅ"
    }
},
{
    "save-file_day-name_sun",
    new Model_LanguagesUI
    {
        EN = @"A New Day",
        CN = @"A New Day",
        JP = @"新しき一日"
    }
},
// ------------------------------------------------------------------
// Start
{
    "start_cta_press",
    new Model_LanguagesUI
    {
        EN = @"Press Enter or Space",
        CN = @"Press Enter or Space",
        JP = @"Enter か Space"
    }
},
{
    "start_cta_press_dynamic_gamepad",
    new Model_LanguagesUI
    {
        EN = @"Press @@InteractKey",
        CN = @"Press @@InteractKey",
        JP = @"Press @@InteractKey"
    }
},
{
    "start_options_start",
    new Model_LanguagesUI
    {
        EN = @"Start",
        CN = @"Start",
        JP = @"開始"
    }
},
{
    "start_options_start_curse",
    new Model_LanguagesUI
    {
        EN = @"S̶͚͗t̴̹̉a̸͎̅r̴̙̐t̴̢͗",
        CN = @"S̶͚͗t̴̹̉a̸͎̅r̴̙̐t̴̢͗",
        JP = @"҉開҉̶҉̵҉始҉̸҉"
    }
},
{
    "start_options_settings",
    new Model_LanguagesUI
    {
        EN = @"Settings",
        CN = @"Settings",
        JP = @"設定"
    }
},
{
    "start_options_exit",
    new Model_LanguagesUI
    {
        EN = @"End",
        CN = @"End",
        JP = @"終了"
    }
},
{
    "start_demo-version",
    new Model_LanguagesUI
    {
        EN = @"The Demo Vers",
        CN = @"The Demo Vers",
        JP = @"The Demo Vers"
    }
},
// ------------------------------------------------------------------
// Settings
{
    "settings_title",
    new Model_LanguagesUI
    {
        EN = @"『 Settings 』",
        CN = @"『 Settings 』",
        JP = @"『設定』"
    }
},
{
    "settings_controls",
    new Model_LanguagesUI
    {
        EN = @"Controls",
        CN = @"Controls",
        JP = @"操作"
    }
},
{
    "settings_sound",
    new Model_LanguagesUI
    {
        EN = @"Sound",
        CN = @"Sound",
        JP = @"音量"
    }
},
{
    "settings_graphics",
    new Model_LanguagesUI
    {
        EN = @"System",
        CN = @"System",
        JP = @"システム"
    }
},
{
    "settings_back",
    new Model_LanguagesUI
    {
        EN = @"Back",
        CN = @"Back",
        JP = @"もどる"
    }
},
{
    "settings_main-menu",
    new Model_LanguagesUI
    {
        EN = @"Quit to Main Menu",
        CN = @"Quit to Main Menu",
        JP = @"メインメニューへもどる"
    }
},
{
    "settings_prompt_main-menu",
    new Model_LanguagesUI
    {
        EN = @"Are you sure? You’ll lose all progress for the current night.",
        CN = @"Are you sure? You’ll lose all progress for the current night.",
        JP = @"本当にもどりますか？今夜の進行状況はすべて失われます"
    }
},
{
    "settings_end-game",
    new Model_LanguagesUI
    {
        EN = @"Quit to Desktop",
        CN = @"Quit to Desktop",
        JP = @"デスクトップへもどる"
    }
},
{
    "settings_prompt_end-game",
    new Model_LanguagesUI
    {
        EN = @"Are you sure? You’ll lose all progress for the current night.",
        CN = @"Are you sure? You’ll lose all progress for the current night.",
        JP = @"本当にもどりますか？今夜の進行状況はすべて失われます"
    }
},
// ------------------------------------------------------------------
// Save Files
{
    "save-files_button_back",
    new Model_LanguagesUI
    {
        EN = @"Back",
        CN = @"Back",
        JP = @"もどる"
    }
},
{
    "save-files_button_copy",
    new Model_LanguagesUI
    {
        EN = @"Copy",
        CN = @"Copy",
        JP = @"コピー"
    }
},
{
    "save-files_button_delete",
    new Model_LanguagesUI
    {
        EN = @"Delete",
        CN = @"Delete",
        JP = @"削除"
    }
},
{
    "save-files_banner_copy",
    new Model_LanguagesUI
    {
        EN = @"Select a file to copy.",
        CN = @"Select a file to copy.",
        JP = @"コピーするファイルを選んでください"
    }
},
{
    "save-files_banner_paste",
    new Model_LanguagesUI
    {
        EN = @"Copy to which slot?",
        CN = @"Copy to which slot?",
        JP = @"どのスロットへコピーしますか？"
    }
},
{
    "save-files_banner_delete",
    new Model_LanguagesUI
    {
        EN = @"Select a file to delete.",
        CN = @"Select a file to delete.",
        JP = @"削除するファイルを選んでください"
    }
},
{
    "save-files_submenu_continue_message",
    new Model_LanguagesUI
    {
        EN = @"Open this file?",
        CN = @"Open this file?",
        JP = @"どのファイルを開きますか？"
    }
},
{
    "save-files_submenu_new-game_message",
    new Model_LanguagesUI
    {
        EN = @"Begin a new adventure?",
        CN = @"Begin a new adventure?",
        JP = @"新しく冒険をはじめますか？"
    }
},
{
    "save-files_submenu_delete_message",
    new Model_LanguagesUI
    {
        EN = @"This adventure will be permanently lost, okay?",
        CN = @"This adventure will be permanently lost, okay?",
        JP = @"この冒険は永久に失われますが、構いませんか？"
    }
},
{
    "save-files_submenu_paste_message",
    new Model_LanguagesUI
    {
        EN = @"Overwrite this existing file? The previous adventure will be permanently lost.",
        CN = @"Overwrite this existing file? The previous adventure will be permanently lost.",
        JP = @"すでに存在するファイルを上書きしますか？以前の冒険は永久に失われます"
    }
},
{
    "save-files_saved-game_empty",
    new Model_LanguagesUI
    {
        EN = @"『 Empty 』",
        CN = @"『 Empty 』",
        JP = @"『からっぽ』"
    }
},
{
    "save-files_saved-game_masks",
    new Model_LanguagesUI
    {
        EN = @"Masks",
        CN = @"Masks",
        JP = @"仮面"
    }
},
{
    "save-files_saved-game_notes",
    new Model_LanguagesUI
    {
        EN = @"Notes",
        CN = @"Notes",
        JP = @"音符"
    }
},
{
    "save-files_saved-game_last-played",
    new Model_LanguagesUI
    {
        EN = @"Last played",
        CN = @"Last played",
        JP = @"最終プレイ"
    }
},
{
    "save-files_saved-game_total-time",
    new Model_LanguagesUI
    {
        EN = @"Total time",
        CN = @"Total time",
        JP = @"総プレイ時間"
    }
},
{
    "intro_loading_title",
    new Model_LanguagesUI
    {
        EN = @"Loading",
        CN = @"Loading",
        JP = @"ロード中"
    }
},
{
    "intro_loading_title1",
    new Model_LanguagesUI
    {
        EN = @"Loading.",
        CN = @"Loading.",
        JP = @"ロード中…"
    }
},
{
    "intro_loading_title2",
    new Model_LanguagesUI
    {
        EN = @"Loading..",
        CN = @"Loading..",
        JP = @"ロード中……"
    }
},
{
    "intro_loading_title3",
    new Model_LanguagesUI
    {
        EN = @"Loading...",
        CN = @"Loading...",
        JP = @"ロード中………"
    }
},
{
    "intro_loading_title_complete",
    new Model_LanguagesUI
    {
        EN = @"♥",
        CN = @"♥",
        JP = @"♥"
    }
},
{
    "intro_loading_please-wait",
    new Model_LanguagesUI
    {
        EN = @"Working... please wait...<br>Loading time is a bit longer than usual.",
        CN = @"Working... please wait...<br>Loading time is a bit longer than usual.",
        JP = @"ロード中……お待ちください……<br>ロードに通常より時間がかかっています"
    }
},
// ------------------------------------------------------------------
// Controls
{
    "controls_title",
    new Model_LanguagesUI
    {
        EN = @"『 Controls 』",
        CN = @"『 Controls 』",
        JP = @"『操作』"
    }
},
{
    "controls_type_name",
    new Model_LanguagesUI
    {
        EN = @"Controller",
        CN = @"Controller",
        JP = @"コントローラー"
    }
},
{
    "controls_type_keyboard",
    new Model_LanguagesUI
    {
        EN = @"KEYBOARD",
        CN = @"KEYBOARD",
        JP = @"キーボード"
    }
},
{
    "controls_type_joystick",
    new Model_LanguagesUI
    {
        EN = @"JOYSTICK",
        CN = @"JOYSTICK",
        JP = @"ジョイスティック"
    }
},
{
    "controls_type_joystick_warning_unknown",
    new Model_LanguagesUI
    {
        EN = @"Current joystick is of an unknown format.
Use the keyboard to select a control to begin listening for input.",
        CN = @"Current joystick is of an unknown format.
Use the keyboard to select a control to begin listening for input.",
        JP = @"現在のジョイスティックは不明なフォーマットです。キーボードを使用し、入力方式を選択してください。"
    }
},
{
    "controls_move_name",
    new Model_LanguagesUI
    {
        EN = @"Move",
        CN = @"Move",
        JP = @"移動"
    }
},
{
    "controls_interact_name",
    new Model_LanguagesUI
    {
        EN = @"Interact / Confirm",
        CN = @"Interact / Confirm",
        JP = @"インタラクト / 決定"
    }
},
{
    "controls_inventory_name",
    new Model_LanguagesUI
    {
        EN = @"Inventory",
        CN = @"Inventory",
        JP = @"インベントリ"
    }
},
{
    "controls_inventory_name_joystick",
    new Model_LanguagesUI
    {
        EN = @"Inventory / Cancel",
        CN = @"Inventory / Cancel",
        JP = @"インベントリ / キャンセル"
    }
},
{
    "controls_wear-mask-1_name",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 1",
        CN = @"Wear Mask 1",
        JP = @"仮面１着用"
    }
},
{
    "controls_wear-mask-2_name",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 2",
        CN = @"Wear Mask 2",
        JP = @"仮面２着用"
    }
},
{
    "controls_wear-mask-3_name",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 3",
        CN = @"Wear Mask 3",
        JP = @"仮面３着用"
    }
},
{
    "controls_wear-mask-4_name",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 4",
        CN = @"Wear Mask 4",
        JP = @"仮面４着用"
    }
},
{
    "controls_speed_name",
    new Model_LanguagesUI
    {
        EN = @"???",
        CN = @"???",
        JP = @"？？？"
    }
},
{
    "controls_action_switch-active-sticker",
    new Model_LanguagesUI
    {
        EN = @"{80}",
        CN = @"{80}",
        JP = @"{80}"
    }
},
{
    "controls_action_active-sticker-command",
    new Model_LanguagesUI
    {
        EN = @"{79}",
        CN = @"{79}",
        JP = @"{79}"
    }
},
{
    "controls_wear-mask-1_name_unknown-joystick",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 1 (Axis)",
        CN = @"Wear Mask 1 (Axis)",
        JP = @"仮面１着用（スティック）"
    }
},
{
    "controls_wear-mask-2_name_unknown-joystick",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 2 (Axis)",
        CN = @"Wear Mask 2 (Axis)",
        JP = @"仮面２着用（スティック）"
    }
},
{
    "controls_wear-mask-3_name_unknown-joystick",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 3 (Axis)",
        CN = @"Wear Mask 3 (Axis)",
        JP = @"仮面３着用（スティック）"
    }
},
{
    "controls_wear-mask-4_name_unknown-joystick",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 4 (Axis)",
        CN = @"Wear Mask 4 (Axis)",
        JP = @"仮面４着用（スティック）"
    }
},
{
    "controls_wear-mask-vert_name_unknown-joystick",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 1 / 3 (Axis)",
        CN = @"Wear Mask 1 / 3 (Axis)",
        JP = @"仮面１/３着用（スティック）"
    }
},
{
    "controls_wear-mask-hz_name_unknown-joystick",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask 2 / 4 (Axis)",
        CN = @"Wear Mask 2 / 4 (Axis)",
        JP = @"仮面２/４着用（スティック）"
    }
},
{
    "controls_invert_name",
    new Model_LanguagesUI
    {
        EN = @"▲ Invert Axis",
        CN = @"▲ Invert Axis",
        JP = @"▲ 軸反転"
    }
},
{
    "controls_invert_option_0",
    new Model_LanguagesUI
    {
        EN = @"Yes",
        CN = @"Yes",
        JP = @"オン"
    }
},
{
    "controls_invert_option_1",
    new Model_LanguagesUI
    {
        EN = @"No",
        CN = @"No",
        JP = @"オフ"
    }
},
{
    "controls_no-text",
    new Model_LanguagesUI
    {
        EN = @"--",
        CN = @"--",
        JP = @"--"
    }
},
{
    "controls_no-joystick",
    new Model_LanguagesUI
    {
        EN = @"No Joystick Connected",
        CN = @"No Joystick Connected",
        JP = @"ジョイスティックが接続されていません"
    }
},
{
    "controls_unsupported-joystick",
    new Model_LanguagesUI
    {
        EN = @"Unsupported Joystick",
        CN = @"Unsupported Joystick",
        JP = @"サポートされていないジョイスティックです"
    }
},
{
    "controls_move-hz_name",
    new Model_LanguagesUI
    {
        EN = @"Move Left / Right (Axis)",
        CN = @"Move Left / Right (Axis)",
        JP = @"左右移動（スティック）"
    }
},
{
    "controls_move-vert_name",
    new Model_LanguagesUI
    {
        EN = @"Move Up / Down (Axis)",
        CN = @"Move Up / Down (Axis)",
        JP = @"上下移動（スティック）"
    }
},
{
    "controls_move-up_name",
    new Model_LanguagesUI
    {
        EN = @"Move Up (D-pad)",
        CN = @"Move Up (D-pad)",
        JP = @"上（十字キー）"
    }
},
{
    "controls_move-left_name",
    new Model_LanguagesUI
    {
        EN = @"Move Left (D-pad)",
        CN = @"Move Left (D-pad)",
        JP = @"左（十字キー）"
    }
},
{
    "controls_move-down_name",
    new Model_LanguagesUI
    {
        EN = @"Move Down (D-pad)",
        CN = @"Move Down (D-pad)",
        JP = @"下（十字キー）"
    }
},
{
    "controls_move-right_name",
    new Model_LanguagesUI
    {
        EN = @"Move Right (D-pad)",
        CN = @"Move Right (D-pad)",
        JP = @"右（十字キー）"
    }
},
{
    "controls_settings_name",
    new Model_LanguagesUI
    {
        EN = @"Settings",
        CN = @"Settings",
        JP = @"設定"
    }
},
{
    "controls_hotkeys_joystick-action",
    new Model_LanguagesUI
    {
        EN = @"Wear Mask",
        CN = @"Wear Mask",
        JP = @"仮面着用"
    }
},
{
    "controls_title_shortcuts",
    new Model_LanguagesUI
    {
        EN = @"《 Inventory Shortcuts 》",
        CN = @"《 Inventory Shortcuts 》",
        JP = @"《 インベントリ・ショートカット 》"
    }
},
{
    "controls_explanation_shortcuts",
    new Model_LanguagesUI
    {
        EN = @"〈 Press key while hovering over Mask inside Inventory 〉",
        CN = @"〈 Press key while hovering over Mask inside Inventory 〉",
        JP = @"〈インベントリ内の仮面にカーソルを合わせてキーを押してください〉"
    }
},
{
    "controls_prep_name",
    new Model_LanguagesUI
    {
        EN = @"Inventory Hotkeys",
        CN = @"Inventory Hotkeys",
        JP = @"インベントリ・ホットキー"
    }
},
{
    "controls_button_reset",
    new Model_LanguagesUI
    {
        EN = @"Reset Controls",
        CN = @"Reset Controls",
        JP = @"操作リセット"
    }
},
{
    "controls_prompt_reset",
    new Model_LanguagesUI
    {
        EN = @"All controls will be reset to default. Are you sure?",
        CN = @"All controls will be reset to default. Are you sure?",
        JP = @"すべての操作がデフォルトにリセットされますが、よろしいですか？"
    }
},
{
    "controls_notify_detecting",
    new Model_LanguagesUI
    {
        EN = @"Detecting input...",
        CN = @"Detecting input...",
        JP = @"入力検出中……"
    }
},
{
    "controls_error_move",
    new Model_LanguagesUI
    {
        EN = @"Cannot overwrite move keys",
        CN = @"Cannot overwrite move keys",
        JP = @"移動キー上書き不可"
    }
},
{
    "controls_error_taken",
    new Model_LanguagesUI
    {
        EN = @"Key is taken",
        CN = @"Key is taken",
        JP = @"キー使用済み"
    }
},
{
    "controls_error_system",
    new Model_LanguagesUI
    {
        EN = @"Cannot overwrite menu keys",
        CN = @"Cannot overwrite menu keys",
        JP = @"メニューキーは上書きできません"
    }
},
{
    "controls_error_hotkey",
    new Model_LanguagesUI
    {
        EN = @"Cannot overwrite Hotkeys",
        CN = @"Cannot overwrite Hotkeys",
        JP = @"ホットキーは上書きできません"
    }
},
// ------------------------------------------------------------------
// Other Settings
{
    "graphics_title",
    new Model_LanguagesUI
    {
        EN = @"『 System 』",
        CN = @"『 System 』",
        JP = @"『システム』"
    }
},
{
    "sound_title",
    new Model_LanguagesUI
    {
        EN = @"『 Sound 』",
        CN = @"『 Sound 』",
        JP = @"『音量』"
    }
},
{
    "system_master-volume_title",
    new Model_LanguagesUI
    {
        EN = @"Master Volume",
        CN = @"Master Volume",
        JP = @"主音量"
    }
},
{
    "system_master-volume_current",
    new Model_LanguagesUI
    {
        EN = @"current volume ▶",
        CN = @"current volume ▶",
        JP = @"現在の音量 ▶"
    }
},
{
    "system_music-volume_title",
    new Model_LanguagesUI
    {
        EN = @"Music",
        CN = @"Music",
        JP = @"音楽"
    }
},
{
    "system_sfx-volume_title",
    new Model_LanguagesUI
    {
        EN = @"SFX",
        CN = @"SFX",
        JP = @"効果音"
    }
},
{
    "sound_button_reset",
    new Model_LanguagesUI
    {
        EN = @"Reset Sound",
        CN = @"Reset Sound",
        JP = @"音量リセット"
    }
},
{
    "sound_prompt_reset",
    new Model_LanguagesUI
    {
        EN = @"All sound settings will be reset to default. Are you sure?",
        CN = @"All sound settings will be reset to default. Are you sure?",
        JP = @"すべての音量設定がデフォルトにリセットされますが、よろしいですか？"
    }
},
{
    "graphics_resolutions_title",
    new Model_LanguagesUI
    {
        EN = @"Force Windowed Resolution",
        CN = @"Force Windowed Resolution",
        JP = @"ウィンドウモード解像度強制"
    }
},
{
    "graphics_resolutions_current",
    new Model_LanguagesUI
    {
        EN = @"current viewport ▶",
        CN = @"current viewport ▶",
        JP = @"現在の表示領域 ▶"
    }
},
{
    "graphics_resolutions_help-text",
    new Model_LanguagesUI
    {
        EN = @"Select to set available resolution",
        CN = @"Select to set available resolution",
        JP = @"利用可能な解像度を選んでください"
    }
},
{
    "graphics_fullscreen_title",
    new Model_LanguagesUI
    {
        EN = @"Full-Screen",
        CN = @"Full-Screen",
        JP = @"全画面"
    }
},
{
    "graphics_fullscreen_current",
    new Model_LanguagesUI
    {
        EN = @"current mode ▶",
        CN = @"current mode ▶",
        JP = @"現在の設定 ▶"
    }
},
{
    "graphics_fullscreen_on",
    new Model_LanguagesUI
    {
        EN = @"on",
        CN = @"on",
        JP = @"オン"
    }
},
{
    "graphics_fullscreen_off",
    new Model_LanguagesUI
    {
        EN = @"off",
        CN = @"off",
        JP = @"オフ"
    }
},
{
    "graphics_fullscreen_current_on",
    new Model_LanguagesUI
    {
        EN = @"on",
        CN = @"on",
        JP = @"オン"
    }
},
{
    "graphics_fullscreen_current_off",
    new Model_LanguagesUI
    {
        EN = @"off",
        CN = @"off",
        JP = @"オフ"
    }
},
{
    "graphics_screenshake_title",
    new Model_LanguagesUI
    {
        EN = @"Screenshake",
        CN = @"Screenshake",
        JP = @"画面のゆれ"
    }
},
{
    "graphics_screenshake_on",
    new Model_LanguagesUI
    {
        EN = @"on",
        CN = @"on",
        JP = @"オン"
    }
},
{
    "graphics_screenshake_off",
    new Model_LanguagesUI
    {
        EN = @"off",
        CN = @"off",
        JP = @"オフ"
    }
},
{
    "graphics_screenshake_current_on",
    new Model_LanguagesUI
    {
        EN = @"on",
        CN = @"on",
        JP = @"オン"
    }
},
{
    "graphics_screenshake_current_off",
    new Model_LanguagesUI
    {
        EN = @"off",
        CN = @"off",
        JP = @"オフ"
    }
},
{
    "system_button_reset",
    new Model_LanguagesUI
    {
        EN = @"Reset All to Default",
        CN = @"Reset All to Default",
        JP = @"すべてデフォルトへリセット"
    }
},
{
    "system_prompt_reset",
    new Model_LanguagesUI
    {
        EN = @"All settings will be reset to default including controls and sound. Are you sure?",
        CN = @"All settings will be reset to default including controls and sound. Are you sure?",
        JP = @"操作および音量の全設定がデフォルトへリセットされますが、よろしいですか？"
    }
},
{
    "system_button_reset-language",
    new Model_LanguagesUI
    {
        EN = @"Reset Language",
        CN = @"Reset Language",
        JP = @"言語設定リセット"
    }
},
{
    "system_prompt_reset-language",
    new Model_LanguagesUI
    {
        EN = @"Language preference will be reset on the next game restart. Are you sure?",
        CN = @"Language preference will be reset on the next game restart. Are you sure?",
        JP = @"ゲームを再起動すると言語設定がリセットされますが、よろしいですか？"
    }
},
// ------------------------------------------------------------------
// Eileen's Mind
{
    "eileens-mind_myne_challenge_passive",
    new Model_LanguagesUI
    {
        EN = @"S̶t̶o̴p̷!̵ ̸T̴h̴e̶r̷e̷’̴s̷ ̸n̴o̵ ̴c̷h̴a̷n̴c̴e̶ ̸y̸o̷u̶ ̸w̴i̶l̸l̷ ̷m̶a̸k̴e̴ ̷i̴t̷ ̷i̵n̷ ̵t̷i̵m̴e̵!̵ ̶I̷t̵’̶s̵ ̶f̷o̷r̵ ̶y̵o̶u̷r̷ ̴o̶w̴n̷ ̵g̵o̵o̶d̶.̸",
        CN = @"S̶t̶o̴p̷!̵ ̸T̴h̴e̶r̷e̷’̴s̷ ̸n̴o̵ ̴c̷h̴a̷n̴c̴e̶ ̸y̸o̷u̶ ̸w̴i̶l̸l̷ ̷m̶a̸k̴e̴ ̷i̴t̷ ̷i̵n̷ ̵t̷i̵m̴e̵!̵ ̶I̷t̵’̶s̵ ̶f̷o̷r̵ ̶y̵o̶u̷r̷ ̴o̶w̴n̷ ̵g̵o̵o̶d̶.̸",
        JP = @"҉や҉҈҉め҉̵҉ろ҉̷҉̶҉間҉̵҉に҉̴҉合҉̴҉̶҉い҉̵҉̴҉っ҉̴҉こ҉̶҉な҉̶҉い҉̴҉そ҉҉҉̷҉れ҉̵҉̴҉が҉҈҉҈҉身҉̵҉の҉̴҉た҉̴҉め҉̵҉だ҉҉҉"
    }
},
{
    "eileens-mind_myne_challenge_passive1",
    new Model_LanguagesUI
    {
        EN = @"Y̶o̶u̵’̷r̶e̵ ̸g̵o̷i̶n̶g̵ ̶t̸o̶ ̵h̴u̶r̶t̴ ̴y̶o̵u̴r̵s̸e̸l̵f̷,̶ ̷c̶a̵n̸'̶t̵ ̸y̴o̸u̷ ̷s̴e̷e̵?̴!̷",
        CN = @"Y̶o̶u̵’̷r̶e̵ ̸g̵o̷i̶n̶g̵ ̶t̸o̶ ̵h̴u̶r̶t̴ ̴y̶o̵u̴r̵s̸e̸l̵f̷,̶ ̷c̶a̵n̸'̶t̵ ̸y̴o̸u̷ ̷s̴e̷e̵?̴!̷",
        JP = @"҉自҉҉҉分҉̸҉̸҉で҉̸҉̵҉自҉̵҉分҉̷҉̵҉を҉̴҉傷҉҉҉҉҉つ҉̴҉̸҉け҉̷҉҉҉る҉̴҉̸҉つ҉̴҉も҉̸҉り҉҈҉な҉̴҉҉҉の҉̷҉か҉̷҉"
    }
},
{
    "eileens-mind_myne_challenge_passive2",
    new Model_LanguagesUI
    {
        EN = @"Y̵o̸u̸ ̷i̴m̷b̵e̶c̵i̷l̷e̵!̷ ̸P̶u̴t̸ ̴a̶ ̴s̷t̸o̵p̸ ̴t̷o̴ ̴t̶h̸i̴s̶ ̴r̷i̶g̸h̶t̶ ̷n̸o̸w̵!̶",
        CN = @"Y̵o̸u̸ ̷i̴m̷b̵e̶c̵i̷l̷e̵!̷ ̸P̶u̴t̸ ̴a̶ ̴s̷t̸o̵p̸ ̴t̷o̴ ̴t̶h̸i̴s̶ ̴r̷i̶g̸h̶t̶ ̷n̸o̸w̵!̶",
        JP = @"҉マ҉̵҉ヌ҉̸҉ケ҉̸҉い҉҈҉҉҉ま҉̷҉す҉̶҉̷҉ぐ҉҉҉や҉̶҉̸҉め҉҉҉҈҉ろ҉̴҉̵҉"
    }
},
{
    "eileens-mind_myne_challenge_passive3",
    new Model_LanguagesUI
    {
        EN = @"Y̴o̷u̴ ̷w̵i̴l̵l̴ ̷p̸a̷y̴ ̷f̶o̸r̵ ̴t̵h̷i̸s̴,̸ ̴m̶a̵r̴k̵ ̴m̷y̸ ̵w̴o̵r̵d̴s̸!̵",
        CN = @"Y̴o̷u̴ ̷w̵i̴l̵l̴ ̷p̸a̷y̴ ̷f̶o̸r̵ ̴t̵h̷i̸s̴,̸ ̴m̶a̵r̴k̵ ̴m̷y̸ ̵w̴o̵r̵d̴s̸!̵",
        JP = @"҉こ҉̴҉の҉҈҉代҉҈҉̷҉償҉̵҉̶҉は҉̵҉高҉̷҉く҉̵҉҈҉つ҉̵҉く҉̴҉̵҉ぞ҉҉҉҈҉警҉̷҉告҉҉҉し҉̸҉̴҉た҉̸҉か҉̴҉ら҉̴҉҉҉な҉̶҉"
    }
},
{
    "eileens-mind_narrator_dramatic_title",
    new Model_LanguagesUI
    {
        EN = @"I CAN DO IT",
        CN = @"I CAN DO IT",
        JP = @"私はできるんだ"
    }
},
{
    "eileens-mind_narrator_dramatic_title1",
    new Model_LanguagesUI
    {
        EN = @"《 IF I BELIEVE 》",
        CN = @"《 IF I BELIEVE 》",
        JP = @"《私が信じれば》"
    }
},
{
    "eileens-mind_narrator_dramatic",
    new Model_LanguagesUI
    {
        EN = @"* Maybe I can I mean",
        CN = @"* Maybe I can I mean",
        JP = @"＊もしかしたら、できるかもしれないけど"
    }
},
{
    "eileens-mind_narrator_dramatic1",
    new Model_LanguagesUI
    {
        EN = @"* Actually chances aren’t too great",
        CN = @"* Actually chances aren’t too great",
        JP = @"＊実際、大して可能性はなさそう"
    }
},
{
    "eileens-mind_narrator_dramatic2",
    new Model_LanguagesUI
    {
        EN = @"* I’m not too sure about this anymore",
        CN = @"* I’m not too sure about this anymore",
        JP = @"＊全然まるで自信がないや"
    }
},
{
    "eileens-mind_narrator_dramatic3",
    new Model_LanguagesUI
    {
        EN = @"* Hmm you know what? Yeah maybe today’s not the day",
        CN = @"* Hmm you know what? Yeah maybe today’s not the day",
        JP = @"＊ううん、どうしよう？よし、今日はやめとこうかな"
    }
},
{
    "eileens-mind_narrator_dramatic4",
    new Model_LanguagesUI
    {
        EN = @"* Hey there’s always tomorrow right!",
        CN = @"* Hey there’s always tomorrow right!",
        JP = @"＊まあ、明日やればいいって！"
    }
},
{
    "eileens-mind_narrator_dramatic5",
    new Model_LanguagesUI
    {
        EN = @"* Ehhhhhh -_-”",
        CN = @"* Ehhhhhh -_-”",
        JP = @"＊えええええぇぇぇ-_-”"
    }
},
{
    "eileens-mind_narrator_dramatic6",
    new Model_LanguagesUI
    {
        EN = @"* How did I get mixed up in all this?",
        CN = @"* How did I get mixed up in all this?",
        JP = @"＊なんでこんなことに巻き込まれちゃったんだろう？"
    }
},
{
    "eileens-mind_narrator_dramatic7",
    new Model_LanguagesUI
    {
        EN = @"* What was I thinking, this isn’t a good idea!",
        CN = @"* What was I thinking, this isn’t a good idea!",
        JP = @"＊なに考えてんだか、こんな考えよくない！"
    }
},
{
    "eileens-mind_narrator_dramatic8",
    new Model_LanguagesUI
    {
        EN = @"* Why does it always end up like this?",
        CN = @"* Why does it always end up like this?",
        JP = @"＊なんでいつもこうなっちゃうのかな？"
    }
},
{
    "eileens-mind_narrator_dramatic9",
    new Model_LanguagesUI
    {
        EN = @"* Okay deep breaths, deep breaths",
        CN = @"* Okay deep breaths, deep breaths",
        JP = @"＊よし、深呼吸、深呼吸"
    }
},
{
    "eileens-mind_narrator_dramatic10",
    new Model_LanguagesUI
    {
        EN = @"* Wait how do you breathe again?!? *gasp*",
        CN = @"* Wait how do you breathe again?!? *gasp*",
        JP = @"＊あれ、深呼吸ってどうすんだっけ！？"
    }
},
{
    "eileens-mind_narrator_dramatic11",
    new Model_LanguagesUI
    {
        EN = @"* OH NO OH NO OH NO",
        CN = @"* OH NO OH NO OH NO",
        JP = @"＊ああ、ダメダメダメダメ"
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
Jiaquarium",
        JP = @"Dear {2},

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
        CN = @"Return to Main Menu",
        JP = @"Return to Main Menu"
    }
},
{
    "demo-end_choice1",
    new Model_LanguagesUI
    {
        EN = @"Quit to Desktop",
        CN = @"Quit to Desktop",
        JP = @"Quit to Desktop"
    }
},
{
    "demo-end_choice2",
    new Model_LanguagesUI
    {
        EN = @"Wishlist on Steam",
        CN = @"Wishlist on Steam",
        JP = @"Wishlist on Steam"
    }
},
// ------------------------------------------------------------------
// Catwalk2
{
    "catwalk2_player_doubts",
    new Model_LanguagesUI
    {
        EN = @"O̷w̷n̶ ̵p̴r̸o̷b̴l̴e̶m̶s̸.̷ ̶O̵w̷n̶ ̷p̸r̵o̸b̵l̸e̶m̶s̸.̴ ̷I̴t̵'̶s̸ ̵M̷y̶ ̷o̴w̵n̴ ̶p̷r̷o̴b̷l̸e̷m̴s̶.̵",
        CN = @"O̷w̷n̶ ̵p̴r̸o̷b̴l̴e̶m̶s̸.̷ ̶O̵w̷n̶ ̷p̸r̵o̸b̵l̸e̶m̶s̸.̴ ̷I̴t̵'̶s̸ ̵M̷y̶ ̷o̴w̵n̴ ̶p̷r̷o̴b̷l̸e̷m̴s̶.̵",
        JP = @"҉自҉̸҉͡҉̧҉分҉̵҉͞҉̨҉の҉҈҉͞҉̧҉問҉҉҉͡҉̧҉題҉҉҉̛҉̧҉自҉̴҉̛҉͢҉分҉̸҉̛҉̧҉の҉̷҉̛҉̧҉問҉̴҉҇҉̧҉題҉̶҉͠҉̧҉私҉̶҉͞҉͢҉自҉҉҉̛҉̢҉信҉̶҉͞҉̨҉の҉̵҉͡҉͜҉問҉̵҉҇҉͜҉題҉҈҉̛҉̢҉"
    }
},
{
    "catwalk2_player_doubts1",
    new Model_LanguagesUI
    {
        EN = @"N̵e̴v̸e̷r̵ ̴h̴a̴d̴ ̴t̷o̸ ̸c̵a̷r̵e̵ ̶f̶o̴r̴ ̷a̷n̷y̷o̷n̵e̷ ̴N̶e̸v̷e̴r̷ ̶e̸l̴s̵e̶?̶ ̴Y̵o̷u̴ ̷r̶e̴a̵l̶l̸y̷ ̶t̵h̶i̸n̴k̵ ̵M̵e̴?̴",
        CN = @"N̵e̴v̸e̷r̵ ̴h̴a̴d̴ ̴t̷o̸ ̸c̵a̷r̵e̵ ̶f̶o̴r̴ ̷a̷n̷y̷o̷n̵e̷ ̴N̶e̸v̷e̴r̷ ̶e̸l̴s̵e̶?̶ ̴Y̵o̷u̴ ̷r̶e̴a̵l̶l̸y̷ ̶t̵h̶i̸n̴k̵ ̵M̵e̴?̴",
        JP = @"҉誰҉̷҉̕҉͜҉の҉̷҉̛҉̨҉こ҉̶҉͝҉̡҉と҉҉҉͞҉͜҉も҉҉҉̕҉̨҉全҉҉҉͞҉̢҉く҉̵҉͝҉̧҉気҉̷҉͝҉̨҉に҉̸҉͠҉͢҉し҉҉҉͠҉̢҉た҉҈҉̕҉͢҉こ҉̶҉͡҉͜҉と҉̴҉͞҉̨҉な҉̶҉͝҉̨҉い҉̴҉͞҉̨҉っ҉҉҉̛҉̢҉て҉̵҉͠҉̧҉本҉҉҉̕҉̨҉当҉̶҉̛҉̡҉に҉̵҉̕҉͢҉私҉҈҉͠҉̢҉が҉̵҉͡҉͜҉そ҉҈҉͡҉͜҉ん҉̸҉̕҉̢҉な҉̴҉͝҉̢҉人҉҈҉̕҉̡҉間҉҉҉̕҉̨҉だ҉̴҉͞҉̢҉っ҉̸҉̕҉̨҉て҉̵҉̛҉̨҉"
    }
},
{
    "catwalk2_player_doubts2",
    new Model_LanguagesUI
    {
        EN = @"I̸t̶ ̵m̶a̷k̷e̶s̶ ̷m̶e̸ ̶s̶i̸c̸k̷.̶ ̵I̶t̴ ̷m̷a̸k̸e̶s̷ ̶m̵e̷ ̵s̴i̸c̸k̵.̴ ̸I̵t̸ ̶m̴a̸k̷e̵s̷ ̴m̵e̸ ̶s̷i̶c̴k̵.̵",
        CN = @"I̸t̶ ̵m̶a̷k̷e̶s̶ ̷m̶e̸ ̶s̶i̸c̸k̷.̶ ̵I̶t̴ ̷m̷a̸k̸e̶s̷ ̶m̵e̷ ̵s̴i̸c̸k̵.̴ ̸I̵t̸ ̶m̴a̸k̷e̵s̷ ̴m̵e̸ ̶s̷i̶c̴k̵.̵",
        JP = @"҉気҉̸҉͞҉̧҉持҉҉҉͝҉͜҉ち҉҈҉̛҉̡҉悪҉̸҉͞҉̡҉い҉҉҉͡҉̨҉気҉̵҉͠҉̡҉持҉҉҉͝҉͢҉ち҉̶҉͝҉͢҉悪҉̵҉͝҉͢҉い҉̷҉͡҉̢҉気҉҈҉҇҉͢҉持҉̷҉͞҉͜҉ち҉̸҉҇҉̧҉悪҉̴҉̕҉̨҉い҉̵҉̛҉͢҉"
    }
},
// ------------------------------------------------------------------
// Grand Mirror Room
{
    "grand-mirror-room_player_welling-up",
    new Model_LanguagesUI
    {
        EN = @"At that moment,| something began to well up inside me.",
        CN = @"At that moment,| something began to well up inside me.",
        JP = @"その瞬間、|私の中でなにかが沸きあがりはじめた。"
    }
},
{
    "grand-mirror-room_player_welling-up1",
    new Model_LanguagesUI
    {
        EN = @"The folds of what seemed like my brain slowly filled with a syrupy, dark substance.",
        CN = @"The folds of what seemed like my brain slowly filled with a syrupy, dark substance.",
        JP = @"脳のヒダみたいなものが、シロップのような暗黒物質でゆっくり満たされる。"
    }
},
{
    "grand-mirror-room_player_welling-up2",
    new Model_LanguagesUI
    {
        EN = @"Why do I feel like I’ve been retracing a memory?",
        CN = @"Why do I feel like I’ve been retracing a memory?",
        JP = @"どうして記憶をたどってるような感じなんだろう？"
    }
},
{
    "grand-mirror-room_player_welling-up3",
    new Model_LanguagesUI
    {
        EN = @"A slippery image of a portrait...",
        CN = @"A slippery image of a portrait...",
        JP = @"手応えのない肖像画のイメージ……"
    }
},
{
    "grand-mirror-room_player_welling-up4",
    new Model_LanguagesUI
    {
        EN = @"What if I|| <i>can’t</i>?",
        CN = @"What if I|| <i>can’t</i>?",
        JP = @"もし、||<b>できなかったら？</b>"
    }
},
{
    "grand-mirror-room_player_change",
    new Model_LanguagesUI
    {
        EN = @"Okay. I’ll talk...",
        CN = @"Okay. I’ll talk...",
        JP = @"よし、話そう……"
    }
},
// ------------------------------------------------------------------
// Quest 1*
{
    "faceoff_ren-myne_quest0_block0_0",
    new Model_LanguagesUI
    {
        EN = @"You said you’d talk, dear, so let’s talk.",
        CN = @"You said you’d talk, dear, so let’s talk.",
        JP = @"あなたが話したいのであれば、お話ししましょう。"
    }
},
{
    "faceoff_ren-myne_quest0_block0_1",
    new Model_LanguagesUI
    {
        EN = @"{0}, I’m only doing this because it’s in <i>our</i> best interest.",
        CN = @"{0}, I’m only doing this because it’s in <i>our</i> best interest.",
        JP = @"{0}さん、私がこうする理由は、これが私達にとって<b>最善</b>だからです。"
    }
},
{
    "faceoff_ren-myne_quest0_block0_2",
    new Model_LanguagesUI
    {
        EN = @"So don’t you go around acting like a saint.",
        CN = @"So don’t you go around acting like a saint.",
        JP = @"だから、あなたは聖者のように振る舞わないでください。"
    }
},
{
    "faceoff_ren-myne_quest0_block2_0",
    new Model_LanguagesUI
    {
        EN = @"You’re not saving anyone down here, no.",
        CN = @"You’re not saving anyone down here, no.",
        JP = @"あなたは、ここで誰かを救うつもりなどありませんよね。"
    }
},
{
    "faceoff_ren-myne_quest0_block2_1",
    new Model_LanguagesUI
    {
        EN = @"On the contrary, dear, you’re hurting <i>us</i> all.",
        CN = @"On the contrary, dear, you’re hurting <i>us</i> all.",
        JP = @"むしろ、あなたは<b>私達</b>を傷つけているだけです。"
    }
},
{
    "faceoff_ren-myne_quest0_block2_2",
    new Model_LanguagesUI
    {
        EN = @"Including yourself.",
        CN = @"Including yourself.",
        JP = @"あなた自身をも。"
    }
},
{
    "faceoff_ren-myne_quest0_block3_0",
    new Model_LanguagesUI
    {
        EN = @"But I get it. You’ve always liked hurting others.",
        CN = @"But I get it. You’ve always liked hurting others.",
        JP = @"ただ、承知しています。あなた、いつも他人を傷つけるのを好んでいますから。"
    }
},
{
    "faceoff_ren-myne_quest0_block3_1",
    new Model_LanguagesUI
    {
        EN = @"It’s just like you.",
        CN = @"It’s just like you.",
        JP = @"それでこそ、あなたです。"
    }
},
{
    "faceoff_ren-myne_quest0_block3_2",
    new Model_LanguagesUI
    {
        EN = @"In the end, you’ll end up hurting everyone around you, dear.",
        CN = @"In the end, you’ll end up hurting everyone around you, dear.",
        JP = @"どうあがいても、あなたは周囲の誰も彼もを傷つけるしかないのです。"
    }
},
{
    "faceoff_ren-myne_quest0_block3_3",
    new Model_LanguagesUI
    {
        EN = @"It’s the same as me.",
        CN = @"It’s the same as me.",
        JP = @"私と同じく。"
    }
},
{
    "faceoff_ren-myne_quest0_block4_0",
    new Model_LanguagesUI
    {
        EN = @"He-he, you think you’re any different than me?",
        CN = @"He-he, you think you’re any different than me?",
        JP = @"ふふっ。あなたと私に、違いがあるとお思いですか？"
    }
},
{
    "faceoff_ren-myne_quest0_block4_1",
    new Model_LanguagesUI
    {
        EN = @"Without me, there is no you.",
        CN = @"Without me, there is no you.",
        JP = @"私がいなければ、あなたもいないのです。"
    }
},
{
    "faceoff_ren-myne_quest0_block4_2",
    new Model_LanguagesUI
    {
        EN = @"You were summoned here for one purpose| – to assist| <i>u|s</i>|,",
        CN = @"You were summoned here for one purpose| – to assist| <i>u|s</i>|,",
        JP = @"あなたがここに召喚された理由はただひとつ、|<b>私|達</b>|の|手伝い。"
    }
},
{
    "faceoff_ren-myne_quest0_block4_3",
    new Model_LanguagesUI
    {
        EN = @"But you are well past expired now, dear.",
        CN = @"But you are well past expired now, dear.",
        JP = @"でも、あなたはもう用済みです。"
    }
},
{
    "faceoff_ren-myne_quest0_block5_0",
    new Model_LanguagesUI
    {
        EN = @"So why don’t you do us all a favor,",
        CN = @"So why don’t you do us all a favor,",
        JP = @"ですから、私達の願いを聞いていただけませんか？"
    }
},
{
    "faceoff_ren-myne_quest0_block6_0",
    new Model_LanguagesUI
    {
        EN = @"Take your useless self out of <i>my</i> mansion.",
        CN = @"Take your useless self out of <i>my</i> mansion.",
        JP = @"<b>私</b>の館から、役立たずのあなた自身を追い出してもらえませんか？"
    }
},
{
    "faceoff_ren-myne_quest0_block7_0",
    new Model_LanguagesUI
    {
        EN = @"Our world is better off without you.",
        CN = @"Our world is better off without you.",
        JP = @"私達の世界は、あなたがいなければより良くなるのです。"
    }
},
// ------------------------------------------------------------------
// Quest 2*
{
    "faceoff_ren-myne_quest1_block0_0",
    new Model_LanguagesUI
    {
        EN = @"{0}, dear, you really understand nothing!",
        CN = @"{0}, dear, you really understand nothing!",
        JP = @"{0}さん、なにひとつ全く理解していないのですね！"
    }
},
{
    "faceoff_ren-myne_quest1_block0_1",
    new Model_LanguagesUI
    {
        EN = @"Don’t you get it?",
        CN = @"Don’t you get it?",
        JP = @"どうしてわかっていただけないのですか？"
    }
},
{
    "faceoff_ren-myne_quest1_block0_2",
    new Model_LanguagesUI
    {
        EN = @"<b>You are the intruder here.</b>",
        CN = @"<b>You are the intruder here.</b>",
        JP = @"<b>ここにとって、あなたこそ侵入者なんです。</b>"
    }
},
{
    "faceoff_ren-myne_quest1_block1_0",
    new Model_LanguagesUI
    {
        EN = @"Someone like you could never understand.",
        CN = @"Someone like you could never understand.",
        JP = @"あなたのような方には、決して理解できないでしょう。"
    }
},
{
    "faceoff_ren-myne_quest1_block1_1",
    new Model_LanguagesUI
    {
        EN = @"Someone who’s never had to care about anyone but themselves.",
        CN = @"Someone who’s never had to care about anyone but themselves.",
        JP = @"自分以外、誰のことも気にかけたことのない人間には。"
    }
},
{
    "faceoff_ren-myne_quest1_block1_2",
    new Model_LanguagesUI
    {
        EN = @"It makes me sick.",
        CN = @"It makes me sick.",
        JP = @"気分が悪くなります。"
    }
},
{
    "faceoff_ren-myne_quest1_block1_glitch-response",
    new Model_LanguagesUI
    {
        EN = @"I can’t stop thinking about you no matter how hard I try.",
        CN = @"I can’t stop thinking about you no matter how hard I try.",
        JP = @"どれだけやめようとしても、あなたのことを考えずにはいられません。"
    }
},
{
    "faceoff_ren-myne_quest1_block2_0",
    new Model_LanguagesUI
    {
        EN = @"You really have no clue, do you, dear?",
        CN = @"You really have no clue, do you, dear?",
        JP = @"あなた、まるでなにもわかってないんですね？"
    }
},
{
    "faceoff_ren-myne_quest1_block2_1",
    new Model_LanguagesUI
    {
        EN = @"You waltz in here, making us suffer again and again...",
        CN = @"You waltz in here, making us suffer again and again...",
        JP = @"ここでワルツを繰り返して、何度も何度も私達を苦しめて……"
    }
},
{
    "faceoff_ren-myne_quest1_block2_2",
    new Model_LanguagesUI
    {
        EN = @"They’re just <i><b>your little visits</b></i>, aren’t they?",
        CN = @"They’re just <i><b>your little visits</b></i>, aren’t they?",
        JP = @"<b>あなたには些細な巡り</b>に過ぎないんでしょうがね？"
    }
},
{
    "faceoff_ren-myne_quest1_block3_0",
    new Model_LanguagesUI
    {
        EN = @"I think about you when I xxxxx xxxxxx. I know you do too.",
        CN = @"I think about you when I xxxxx xxxxxx. I know you do too.",
        JP = @"私は■■■■■■■■とき、あなたを思います。あなたもそうでしょう。"
    }
},
{
    "faceoff_ren-myne_quest1_block3_1",
    new Model_LanguagesUI
    {
        EN = @"B̸u̸t̵ ̴ ŝa̵Ŷ’̵Þ ̴you s̴e̷e̸?̷ ̶ Ī Ĳu̴ ̵need m̸e̵ ̵t̷o̶ ̸ s̶ěĠøÛÊý.",
        CN = @"B̸u̸t̵ ̴ ŝa̵Ŷ’̵Þ ̴you s̴e̷e̸?̷ ̶ Ī Ĳu̴ ̵need m̸e̵ ̵t̷o̶ ̸ s̶ěĠøÛÊý.",
        JP = @"҉で҉̶҉͡҉͢҉も҉̵҉̛҉͜҉あ҉̶҉͠҉̧҉な҉̵҉͡҉̢҉た҉̷҉͞҉̢҉は҉҈҉͞҉̨҉…҉҈҉҇҉͜҉…҉҈҉҇҉̧҉私҉̴҉͞҉͢҉を҉̵҉͞҉̧҉必҉̶҉͝҉͜҉要҉̶҉͡҉̢҉と҉̸҉͝҉͢҉し҉̷҉͡҉̡҉て҉҈҉̕҉̡҉…҉҈҉҇҉̧҉…҉҉҉͡҉̢҉"
    }
},
{
    "faceoff_ren-myne_quest1_block4_0",
    new Model_LanguagesUI
    {
        EN = @"I know you better than you know yourself, dear.",
        CN = @"I know you better than you know yourself, dear.",
        JP = @"あなた自身以上に、私はあなたを知っていますよ。"
    }
},
{
    "faceoff_ren-myne_quest1_block4_1",
    new Model_LanguagesUI
    {
        EN = @"You’ve always been one to self sabotage.",
        CN = @"You’ve always been one to self sabotage.",
        JP = @"あなたはいつだって、自己破壊するのです。"
    }
},
{
    "faceoff_ren-myne_quest1_block4_2",
    new Model_LanguagesUI
    {
        EN = @"You’ll take us all down with you soon enough.",
        CN = @"You’ll take us all down with you soon enough.",
        JP = @"あなたはすぐにでも、私達を巻き込むでしょう。"
    }
},
{
    "faceoff_ren-myne_quest1_block4_3",
    new Model_LanguagesUI
    {
        EN = @"How about you go back up to your floor before that happens.",
        CN = @"How about you go back up to your floor before that happens.",
        JP = @"そうなる前に、自分の階へもどってはいかがですか？"
    }
},
{
    "faceoff_ren-myne_quest1_block4_4",
    new Model_LanguagesUI
    {
        EN = @"And we can forget about all of this.",
        CN = @"And we can forget about all of this.",
        JP = @"そうすれば、私達もすべて忘れますので。"
    }
},
{
    "faceoff_ren-myne_quest1_block4_glitch-response",
    new Model_LanguagesUI
    {
        EN = @"I hate you and I hate that I need you.",
        CN = @"I hate you and I hate that I need you.",
        JP = @"私はあなたを嫌悪します。私があなたを必要とすることを嫌悪します。"
    }
},
{
    "faceoff_ren-myne_quest1_block5_0",
    new Model_LanguagesUI
    {
        EN = @"Remember...",
        CN = @"Remember...",
        JP = @"覚えておいてください……"
    }
},
{
    "faceoff_ren-myne_quest1_block5_1",
    new Model_LanguagesUI
    {
        EN = @"<b>I</b> am the one who built this place.",
        CN = @"<b>I</b> am the one who built this place.",
        JP = @"この場所を作り上げたのは、<b>私</b>です。"
    }
},
{
    "faceoff_ren-myne_quest1_block6_0",
    new Model_LanguagesUI
    {
        EN = @"<b>You are nobody.</b>",
        CN = @"<b>You are nobody.</b>",
        JP = @"<b>あなたは何者でもありません</b>。"
    }
},
// ------------------------------------------------------------------
// Quest 3*
{
    "faceoff_ren-myne_quest2_block0_0",
    new Model_LanguagesUI
    {
        EN = @"Please, I beg you...",
        CN = @"Please, I beg you...",
        JP = @"どうか、お願いします……"
    }
},
{
    "faceoff_ren-myne_quest2_block0_1",
    new Model_LanguagesUI
    {
        EN = @"Just leave...",
        CN = @"Just leave...",
        JP = @"ただちに立ち去ってください……"
    }
},
{
    "faceoff_ren-myne_quest2_block1_0",
    new Model_LanguagesUI
    {
        EN = @"What can I do at this point?",
        CN = @"What can I do at this point?",
        JP = @"いまさら、私にどうしろと？"
    }
},
{
    "faceoff_ren-myne_quest2_block1_1",
    new Model_LanguagesUI
    {
        EN = @"Just tell me, what is it that you want?",
        CN = @"Just tell me, what is it that you want?",
        JP = @"教えてください。あなたの望みはなんなのですか？"
    }
},
{
    "faceoff_ren-myne_quest2_block1_2",
    new Model_LanguagesUI
    {
        EN = @"Please, just leave.",
        CN = @"Please, just leave.",
        JP = @"どうか、立ち去ってください。"
    }
},
{
    "faceoff_ren-myne_quest2_block1_half-glitch-response",
    new Model_LanguagesUI
    {
        EN = @"Í̶̻ ̴̖̑w̴͙̎a̵̡͋n̸̛̪ț̴̄ ̶̥͑t̵̜̾ó̸̻ ̸͍̒ṃ̵́ẽ̴͜e̵̹̋ẗ̴͓ ̵̛͎y̸̞͗ö̶̭ȕ̶̳.̸̩͝",
        CN = @"Í̶̻ ̴̖̑w̴͙̎a̵̡͋n̸̛̪ț̴̄ ̶̥͑t̵̜̾ó̸̻ ̸͍̒ṃ̵́ẽ̴͜e̵̹̋ẗ̴͓ ̵̛͎y̸̞͗ö̶̭ȕ̶̳.̸̩͝",
        JP = @"҉私҉̷҉͠҉̧҉は҉̸҉͝҉̢҉あ҉҈҉͠҉̧҉な҉̵҉͠҉̢҉た҉̶҉̛҉̧҉に҉̸҉͞҉͢҉会҉̶҉͡҉͜҉い҉̶҉͞҉̧҉た҉̸҉͠҉̧҉い҉̵҉͠҉͜҉で҉̵҉̛҉̢҉す҉҈҉͡҉̨҉。҉̵҉͞҉͢҉"
    }
},
{
    "faceoff_ren-myne_quest2_block2_0",
    new Model_LanguagesUI
    {
        EN = @"Stop hurting yourself.",
        CN = @"Stop hurting yourself.",
        JP = @"自分自身を傷つけるのはやめてください。"
    }
},
{
    "faceoff_ren-myne_quest2_block2_half-glitch-response",
    new Model_LanguagesUI
    {
        EN = @"Í̶̻ ̴̖̑w̴͙̎a̵̡͋n̸̛̪ț̴̄ ̶̥͑t̵̜̾ó̸̻ ̸͍̒ṃ̵́ẽ̴͜e̵̹̋ẗ̴͓ ̵̛͎y̸̞͗ö̶̭ȕ̶̳.̸̩͝",
        CN = @"Í̶̻ ̴̖̑w̴͙̎a̵̡͋n̸̛̪ț̴̄ ̶̥͑t̵̜̾ó̸̻ ̸͍̒ṃ̵́ẽ̴͜e̵̹̋ẗ̴͓ ̵̛͎y̸̞͗ö̶̭ȕ̶̳.̸̩͝",
        JP = @"҉私҉̷҉͠҉̧҉は҉̸҉͝҉̢҉あ҉҈҉͠҉̧҉な҉̵҉͠҉̢҉た҉̶҉̛҉̧҉に҉̸҉͞҉͢҉会҉̶҉͡҉͜҉い҉̶҉͞҉̧҉た҉̸҉͠҉̧҉い҉̵҉͠҉͜҉で҉̵҉̛҉̢҉す҉҈҉͡҉̨҉。҉̵҉͞҉͢҉"
    }
},
{
    "faceoff_ren-myne_quest2_block3_0",
    new Model_LanguagesUI
    {
        EN = @"I need things to stay how they are.",
        CN = @"I need things to stay how they are.",
        JP = @"なにもかも、このままでなくては。"
    }
},
// ------------------------------------------------------------------
// Finale
{
    "faceoff_rin-myne_finale_block0_0",
    new Model_LanguagesUI
    {
        EN = @"I’m beginning to understand.",
        CN = @"I’m beginning to understand.",
        JP = @"やっと、わかってきた。"
    }
},
{
    "faceoff_rin-myne_finale_block0_1",
    new Model_LanguagesUI
    {
        EN = @"These paintings.",
        CN = @"These paintings.",
        JP = @"この絵のこと。"
    }
},
{
    "faceoff_rin-myne_finale_block0_2",
    new Model_LanguagesUI
    {
        EN = @"This mansion.",
        CN = @"This mansion.",
        JP = @"この館のこと。"
    }
},
{
    "faceoff_rin-myne_finale_block1_0",
    new Model_LanguagesUI
    {
        EN = @"You and I.",
        CN = @"You and I.",
        JP = @"君と私のこと。"
    }
},
// ------------------------------------------------------------------
// Comments
{
    "ddr_comments_tier1",
    new Model_LanguagesUI
    {
        EN = @"EXCELLENT",
        CN = @"EXCELLENT",
        JP = @"EXCELLENT"
    }
},
{
    "ddr_comments_tier2",
    new Model_LanguagesUI
    {
        EN = @"DECENT",
        CN = @"DECENT",
        JP = @"DECENT"
    }
},
{
    "ddr_comments_tier3",
    new Model_LanguagesUI
    {
        EN = @"Reconsider your life decisions...",
        CN = @"Reconsider your life decisions...",
        JP = @"人生を考え直しな……"
    }
},
// ------------------------------------------------------------------
// Mistakes HUD
{
    "ddr_mistakes_label",
    new Model_LanguagesUI
    {
        EN = @"『 B.A.D. 』",
        CN = @"『 B.A.D. 』",
        JP = @"『 B.A.D. 』"
    }
},
// ------------------------------------------------------------------
// Notifications
{
    "ddr_notifications_close",
    new Model_LanguagesUI
    {
        EN = @"LAST MOVES! GET READY!",
        CN = @"LAST MOVES! GET READY!",
        JP = @"LAST MOVES! GET READY!"
    }
},

};
}

