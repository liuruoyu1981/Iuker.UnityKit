using System.Text;
using System.Text.RegularExpressions;
using Iuker.Common;
using Iuker.Common.Base.Interfaces;
using Iuker.UnityKit.Run.ViewWidget;
using UnityEngine;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget.Texts
{
    /// <inheritdoc />
    /// <summary>
    /// 支持富文本的Text控件
    /// 1. 支持图片表情（动态及静态）
    /// 2. 支持超链接
    /// </summary>
    public class IukerRichText : IukText
    {
        private static readonly Regex InputTagRegex = new Regex(@"\[(\-{0,1}\d{0,})#(.+?)\]", RegexOptions.Singleline);

        private static readonly IObjectPool<StringBuilder> StringBuilderPool = new ObjectPool<StringBuilder>(new StringBuilderFactory(), 50);

        private class StringBuilderFactory : IObjectFactory<StringBuilder>
        {
            public StringBuilder CreateObject()
            {
                return new StringBuilder();
            }
        }


        private StringBuilder textSb;
        private int index;
        private string spriteTag;
        private Match match;
        private Group matchFirst;
        private Group matchSecond;
        private int tempId;

        #region Test


        #endregion

        #region MonoPipeline

        protected override void Start()
        {
            ActiveText();
        }

        private void ActiveText()
        {
            supportRichText = true;
            alignByGeometry = true;
            SetVerticesDirty();
        }

        #endregion

        private string GetTextByRegex()
        {
            textSb = StringBuilderPool.Take();
            index = 0;

            foreach (Match m in InputTagRegex.Matches(text))
            {
                match = m;
                tempId = 0;
                matchFirst = match.Groups[1];
                matchSecond = match.Groups[2];
                if (!string.IsNullOrEmpty(matchFirst.Value) && !matchFirst.Value.Equals("-"))
                {
                    tempId = int.Parse(matchFirst.Value);
                }

                spriteTag = matchSecond.Value;
                tempId.Less(0)
                    .TrueDo(UpdateHyperLink)
                    .FalseDo(UpdateExpression);
                index = match.Index + match.Length;
            }

            return RestoreSbAndReturnContent();
        }

        /// <summary>
        /// 将字符串构建器实例归还到对象池中并返回最终文本结果
        /// </summary>
        private string RestoreSbAndReturnContent()
        {
            textSb.Append(text.Substring(index, text.Length - index));
            var result = textSb.ToString();
            textSb.Clear();
            StringBuilderPool.Restore(textSb);
            return result;
        }

        /// <summary>
        /// 更新超链接
        /// </summary>
        private void UpdateHyperLink()
        {
            textSb.Append(text.Substring(index, match.Index - index));
            textSb.Append("<color=blue>");
            var startIndex = textSb.Length * 4;
            textSb.Append("[" + matchSecond.Value + "]");
            var endIndex = textSb.Length * 4 - 2;
            textSb.Append("</color>");

            var hyperlinkInfo = new HyperlinkInfo
            {
                Id = Mathf.Abs(tempId),
                StartIndex = startIndex, // 超链接里的文本起始顶点索引
                EndIndex = endIndex,
                Name = match.Groups[2].Value
            };

        }

        /// <summary>
        /// 更新表情
        /// </summary>
        private void UpdateExpression()
        {

        }

    }
}