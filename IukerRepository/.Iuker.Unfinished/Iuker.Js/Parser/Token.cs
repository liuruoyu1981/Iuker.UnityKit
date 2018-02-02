namespace Iuker.Js.Parser
{
    public class Token
    {
        public static readonly Token Empty = new Token();

        public Tokens Type;
        public string Literal;
        public object Value;
        public int[] Range;
        public int? LineNumber;
        public int LineStart;
        public bool Octal;
        public Location Location;
        public int Precedence;
    }
}
