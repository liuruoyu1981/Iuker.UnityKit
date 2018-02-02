namespace Iuker.Js.Parser
{
    /// <summary>
    /// 注释
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// 注释所在源码位置
        /// </summary>
        public Location Location;


        public int[] Range;
        public string Type;
        public string Value;
    }
}