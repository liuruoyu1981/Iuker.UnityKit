using System.Text.RegularExpressions;
using Iuker.Js.Native.Object;

namespace Iuker.Js.Native.RegExp
{
    /// <summary>
    /// JavaScript正则表达对象
    /// </summary>
    public class RegExpInstance : ObjectInstance
    {
        

        public Regex Value { get; set; }
        //public string Pattern { get; set; }
        public string Source { get; set; }
        public string Flags { get; set; }
        public bool Global { get; set; }
        public bool IgnoreCase { get; set; }
        public bool Multiline { get; set; }

        public Match Match(string input, double start)
        {
            return Value.Match(input, (int)start);
        }

        public RegExpInstance(Engine engine) : base(engine)
        {
        }
    }
}
