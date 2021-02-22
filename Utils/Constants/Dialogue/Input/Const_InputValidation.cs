public static class Const_InputValidation
{
    public static class Entry
    {
        public const int minASCII = 32;
        public const int maxASCII = 126;
        public const int maxCharCount = 280;
    }

    public static class Name
    {
        public const int minASCII = 33; // excludes [space]
        public const int maxASCII = 126;
        public const int maxCharCount = 16;
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
    }
}
