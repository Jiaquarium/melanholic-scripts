public static class Const_InputValidation
{
    public enum Validators
    {
        None = 0,
        Entry = 1,
        Name = 2,
        Answer = 3,
        Code = 4
    }

    public const int EscASCIICode = 27;

    public static int GetMaxCharCount(Validators validator) => validator switch
    {
        Validators.Entry => Entry.maxCharCount,
        Validators.Name => Name.maxCharCount,
        Validators.Answer => Answer.maxCharCount,
        Validators.Code => Code.maxCharCount,
        _ => 999,
    };
    
    public static class Entry
    {
        public const int minASCII = 32; // includes [space]
        public const int maxASCII = 126;
        public const int maxCharCount = 280;
    }

    public static class Name
    {
        public const int minASCII = 32; // includes [space]
        public const int maxASCII = 126;
        public const int maxCharCount = 15;
    }

    public static class Answer
    {
        public const int minASCII = 33; // excludes [space]
        public const int maxASCII = 126;
        public const int maxCharCount = 46; // almost fills the answer input field 
    }

    public static class Code
    {
        public const int minASCII = 48; // digit 0
        public const int maxASCII = 57; // digit 9
        public const int maxCharCount = Script_ScarletCipherManager.QuestionCount;
    }
}
