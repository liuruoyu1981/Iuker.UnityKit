using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iuker.YourSharp.Asts.SyntaxNodes;
using Iuker.YourSharp.Common;


namespace Iuker.YourSharp.Asts.Semantic
{
    /// <summary>
    /// 参数语义节点列表
    /// </summary>
    public class ArgumentsContext
    {
        /// <summary>
        /// 函数参数字典
        /// </summary>
        public Dictionary<string, ArgumentNode> ArgumentNodes { get; } = new Dictionary<string, ArgumentNode>();

        public bool ContainsKey(string key) => ArgumentNodes.ContainsKey(key);

        public void Add(string key, ArgumentNode node) => ArgumentNodes.Add(key, node);

        /// <summary>
        /// 返回函数参数列表的格式化字符串。
        /// </summary>
        /// <returns></returns>
        public string GetArgumentsToken()
        {
            var sb = new StringBuilder();
            // 清空字符串构建器以防止存在脏数据
            sb.Clear();

            var argus = ArgumentNodes.Values.ToList();
            // 对参数进行排序以保证参数格式化字符串的唯一相等
            argus = argus.OrderBy(r => r.NodeName).ToList();

            for (var i = 0; i < argus.Count; i++)
            {
                var node = argus[i];

                sb.Append("arg");
                sb.Append(i);
                sb.Append("_");
                sb.Append(node.NodeName);
                if (i != argus.Count - 1)
                {
                    sb.Append("_");
                }
            }

            var result = sb.ToString();
            //CacheFactory.StringBuilderPool.Restore(sb);
            return result;
        }



    }
}
