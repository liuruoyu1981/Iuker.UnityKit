using System;

namespace Iuker.Common.Base
{
    /// <summary>
    /// 用于描述CSharp语法元素用途的特性
    /// </summary>
    public class SelfDescAttribute : Attribute
    {
        /// <summary>
        /// 用途描述（中文）
        /// </summary>
        public string ChineseNote { get; private set; }

        /// <summary>
        /// 用途描述（英文）
        /// </summary>
        public string EnglishNote { get; private set; }

        /// <summary>
        /// 用途描述（日文）
        /// </summary>
        public string JapaneseNote { get; private set; }

        /// <summary>
        /// 用途描述（法文）
        /// </summary>
        public string FranceNote { get; private set; }

        /// <summary>
        /// 用途描述（德文）
        /// </summary>
        public string GermanyNote { get; private set; }

        /// <summary>
        /// 用途描述（韩文）
        /// </summary>
        public string KoreaNote { get; private set; }

        /// <summary>
        /// 用途描述（西班牙文）
        /// </summary>
        public string SpainNote { get; private set; }

        /// <summary>
        /// 用途描述（意大利文）
        /// </summary>
        public string ItalyNote { get; private set; }


        public SelfDescAttribute(string english, string chinese = null, string japenese = null, string france = null,
            string germany = null, string korea = null, string spain = null, string italy = null)
        {
            EnglishNote = english;
            ChineseNote = chinese;
            JapaneseNote = japenese;
            FranceNote = france;
            GermanyNote = germany;
            KoreaNote = korea;
            SpainNote = spain;
            ItalyNote = italy;
        }


    }
}