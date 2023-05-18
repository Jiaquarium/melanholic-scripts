using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Script_LetterSelect : MonoBehaviour
{
    [SerializeField] private Letters letter;
    [SerializeField] private string letterOverride;
    [SerializeField] private Script_LetterSelectGrid letterSelectGrid;
    [SerializeField] private Script_EntryInput entryInput;
    [SerializeField] bool isDelete;

    [SerializeField] TextMeshProUGUI TMPLetterText;

    public Script_LetterSelectGrid LetterSelectGrid => letterSelectGrid;
    public Button MyButton => GetComponent<Button>();

    void OnValidate()
    {
        TMPLetterText.text = string.IsNullOrEmpty(letterOverride) ? GetLetter(letter) : letterOverride;
    }
    
    // ------------------------------------------------------------
    // Unity Events
    
    // OnClick
    public void AddLetter()
    {
        if (isDelete)
            entryInput.DeleteLetter();
        else
            entryInput.AddLetter(GetLetter(letter));
    }

    // ------------------------------------------------------------

    public static string GetLetter(Letters l) => l switch
    {
        Letters.A => "A",
        Letters.B => "B",
        Letters.C => "C",
        Letters.D => "D",
        Letters.E => "E",
        Letters.F => "F",
        Letters.G => "G",
        Letters.H => "H",
        Letters.I => "I",
        Letters.J => "J",
        Letters.K => "K",
        Letters.L => "L",
        Letters.M => "M",
        Letters.N => "N",
        Letters.O => "O",
        Letters.P => "P",
        Letters.Q => "Q",
        Letters.R => "R",
        Letters.S => "S",
        Letters.T => "T",
        Letters.U => "U",
        Letters.V => "V",
        Letters.W => "W",
        Letters.X => "X",
        Letters.Y => "Y",
        Letters.Z => "Z",
        Letters.ALower => "a",
        Letters.BLower => "b",
        Letters.CLower => "c",
        Letters.DLower => "d",
        Letters.ELower => "e",
        Letters.FLower => "f",
        Letters.GLower => "g",
        Letters.HLower => "h",
        Letters.ILower => "i",
        Letters.JLower => "j",
        Letters.KLower => "k",
        Letters.LLower => "l",
        Letters.MLower => "m",
        Letters.NLower => "n",
        Letters.OLower => "o",
        Letters.PLower => "p",
        Letters.QLower => "q",
        Letters.RLower => "r",
        Letters.SLower => "s",
        Letters.TLower => "t",
        Letters.ULower => "u",
        Letters.VLower => "v",
        Letters.WLower => "w",
        Letters.XLower => "x",
        Letters.YLower => "y",
        Letters.ZLower => "z",
        Letters.Alpha1 => "1",
        Letters.Alpha2 => "2",
        Letters.Alpha3 => "3",
        Letters.Alpha4 => "4",
        Letters.Alpha5 => "5",
        Letters.Alpha6 => "6",
        Letters.Alpha7 => "7",
        Letters.Alpha8 => "8",
        Letters.Alpha9 => "9",
        Letters.Alpha0 => "0",
        Letters.Space => " ",
        Letters.Exclaim => "!",
        Letters.Question => "?",
        Letters.Pound => "#",
        Letters.Dollar => "$",
        Letters.Percent => "%",
        Letters.Ampersand => "&",
        Letters.LeftParen => "(",
        Letters.RightParen => ")",
        Letters.Plus => "+",
        Letters.Dash => "-",
        Letters.Underscore => "_",
        Letters.At => "@",
        Letters.LeftBracket => "[",
        Letters.RightBracket => "]",
        Letters.Tilde => "~",
        _ => "?"
    };

    public enum Letters
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z,
        ALower,
        BLower,
        CLower,
        DLower,
        ELower,
        FLower,
        GLower,
        HLower,
        ILower,
        JLower,
        KLower,
        LLower,
        MLower,
        NLower,
        OLower,
        PLower,
        QLower,
        RLower,
        SLower,
        TLower,
        ULower,
        VLower,
        WLower,
        XLower,
        YLower,
        ZLower,
        Alpha1,
        Alpha2,
        Alpha3,
        Alpha4,
        Alpha5,
        Alpha6,
        Alpha7,
        Alpha8,
        Alpha9,
        Alpha0,
        Space,
        Exclaim,
        Question,
        Pound,
        Dollar,
        Percent,
        Ampersand,
        LeftParen,
        RightParen,
        Plus,
        Dash,
        Underscore,
        At,
        LeftBracket,
        RightBracket,
        Tilde,
    }
}
