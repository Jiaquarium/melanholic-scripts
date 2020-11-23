public static class Const_InputValidation
{
    public static class Entry
    {
        public static int minASCII = 32;
        public static int maxASCII = 126;
        public static int maxCharCount = 280;
    }

    public static class Name
    {
        public static int minASCII = 33; // excludes [space]
        public static int maxASCII = 126;
        public static int maxCharCount = 16;
    }

    public static class Answer
    {
        public static int minASCII = 33; // excludes [space]
        public static int maxASCII = 126;
        public static int maxCharCount = 46; // almost fills the answer input field 
    }
}
