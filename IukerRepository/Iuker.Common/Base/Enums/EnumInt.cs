/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/17 21:20:19
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Iuker.Common
{
    public abstract class EnumInt : IComparable<EnumInt>, IEquatable<EnumInt>
    {
        /// <summary>
        /// 子类型实例索引字典
        /// </summary>
        protected static readonly Dictionary<string, int> SonTypeInstanceIndexDictionary
            = new Dictionary<string, int>();

        /// <summary>
        /// 子类型实例列表字典
        /// </summary>
        protected static readonly Dictionary<string, List<EnumInt>> SonTypeInstanceListDictionary
            = new Dictionary<string, List<EnumInt>>();

        /// <summary>
        /// 子类型实例枚举含义说明字典
        /// </summary>
        protected static readonly Dictionary<string, Dictionary<int, string>> SonTypeInstanceExplainDictionary
            = new Dictionary<string, Dictionary<int, string>>();

        /// <summary>
        /// 字面量
        /// </summary>
        public readonly string Literals;

        /// <summary>
        /// 基础数据值，短整型
        /// </summary>
        public readonly int Value;

        static EnumInt()
        {

        }

        /// <summary>
        /// 构建一个基于短整型、可继承的枚举对象
        /// </summary>
        /// <param name="sonTypeName">类型名</param>
        /// <param name="enumName">枚举名，用于转换为字符串使用</param>
        /// <param name="instanceEnumExplain">实例枚举值说明，说明该实例的内在含义</param>
        /// <param name="index"></param>
        protected EnumInt(int index, string sonTypeName, string enumName, string instanceEnumExplain)
        {
            if (SonTypeInstanceIndexDictionary.ContainsKey(sonTypeName))
            {
                Literals = enumName;
                if (SonTypeInstanceExplainDictionary[sonTypeName].ContainsKey(index))
                {
                    throw new Exception(string.Format("{0}超出范围，其目标值为{1}", enumName, index));
                }

                SonTypeInstanceIndexDictionary[sonTypeName] = index;
                Value = SonTypeInstanceIndexDictionary[sonTypeName];
                SonTypeInstanceListDictionary[sonTypeName].Add(this);
                SonTypeInstanceExplainDictionary[sonTypeName].Add(Value, instanceEnumExplain);
            }
            else
            {
                Literals = enumName;
                SonTypeInstanceIndexDictionary.Add(sonTypeName, 0);
                Value = 0;
                SonTypeInstanceListDictionary.Add(sonTypeName, new List<EnumInt> { this });
                SonTypeInstanceExplainDictionary.Add(sonTypeName, new Dictionary<int, string>());
                SonTypeInstanceExplainDictionary[sonTypeName].Add(Value, instanceEnumExplain);
            }
        }

        protected EnumInt(string sonTypeName, string enumName, string instanceEnumExplain)
        {
            if (SonTypeInstanceIndexDictionary.ContainsKey(sonTypeName))
            {
                Literals = enumName;
                SonTypeInstanceIndexDictionary[sonTypeName]++;
                Value = SonTypeInstanceIndexDictionary[sonTypeName];
                SonTypeInstanceListDictionary[sonTypeName].Add(this);
                SonTypeInstanceExplainDictionary[sonTypeName].Add(Value, instanceEnumExplain);
            }
            else
            {
                Literals = enumName;
                SonTypeInstanceIndexDictionary.Add(sonTypeName, 0);
                Value = 0;
                SonTypeInstanceListDictionary.Add(sonTypeName, new List<EnumInt> { this });
                SonTypeInstanceExplainDictionary.Add(sonTypeName, new Dictionary<int, string>());
                SonTypeInstanceExplainDictionary[sonTypeName].Add(Value, instanceEnumExplain);
            }
        }

        /// <summary>
        /// 保护权限构造函数用于静态初始化防止空引用异常
        /// </summary>
        protected EnumInt() { }

        public override string ToString()
        {
            return Literals;
        }

        public int CompareTo(EnumInt other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(EnumInt other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EnumInt))
            {
                return false;
            }
            EnumInt other = (EnumInt)obj;
            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return (Literals + Value).GetHashCode();
        }
        public static bool operator ==(EnumInt e1, EnumInt e2)
        {
            if (ReferenceEquals(e1, e2)) { return true; }
            return false;
        }

        public static bool operator !=(EnumInt e1, EnumInt e2)
        {
            if (ReferenceEquals(e1, e2)) { return false; }
            return true;
        }

        public static bool operator <(EnumInt e1, EnumInt e2)
        {
            return e2 != null && (e1 != null && e1.Value < e2.Value);
        }

        public static bool operator <=(EnumInt e1, EnumInt e2)
        {
            return e2 != null && (e1 != null && e1.Value <= e2.Value);
        }

        public static bool operator >(EnumInt e1, EnumInt e2)
        {
            return e2 != null && (e1 != null && e1.Value > e2.Value);
        }

        public static bool operator >=(EnumInt e1, EnumInt e2)
        {
            return e2 != null && (e1 != null && e1.Value >= e2.Value);
        }

        public virtual string EnumValueExplain
        {
            get
            {
                var des = SonTypeInstanceExplainDictionary[TypeName][Value];
                return des;
            }
        }

        public Type BaseValueType { get { return typeof(ushort); } }
        public abstract string TypeName { get; }

        #region 枚举方法

        private static string GetTypeName<T>() where T : EnumInt
        {
            var att = System.Attribute.GetCustomAttribute(typeof(T), typeof(EnumTypeNameAttribute)) as EnumTypeNameAttribute;
            if (att == null) throw new Exception(string.Format("{0}没有实现RyEnumTypeNameAttribute特性", typeof(T).Name));
            return att.TypeName;
        }

        /// <summary>
        /// 获得指定子类枚举的所有实例列表
        /// </summary>
        /// <returns></returns>
        public static List<EnumInt> GetSonTypeList<T>() where T : EnumInt
        {
            var tyName = typeof(T).Name;
            if (SonTypeInstanceListDictionary.ContainsKey(tyName))
            {
                var sonType = SonTypeInstanceListDictionary[tyName];
                return sonType;
            }
            Debuger.LogError(string.Format("找不到指定的类型{0}类型!", tyName));
            return null;
        }

        /// <summary>
        /// 获得指定子类枚举的字面量列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetInstanceNameList<T>() where T : EnumInt
        {
            var tyName = typeof(T).Name;
            if (SonTypeInstanceListDictionary.ContainsKey(tyName))
            {
                var sonType = SonTypeInstanceListDictionary[tyName];
                var tempList = sonType.Select(e => e.Literals).ToList();
                return tempList;
            }
            Debuger.LogError(string.Format("找不到指定的类型{0}类型!", tyName));
            return null;
        }

        /// <summary>
        /// 获得指定子类枚举的用途说明文字列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllInstanceExplainList<T>() where T : EnumInt
        {
            var tyName = typeof(T).Name;
            if (SonTypeInstanceListDictionary.ContainsKey(tyName))
            {
                var sonType = SonTypeInstanceListDictionary[tyName];
                var explainList = sonType.Select(e => e.EnumValueExplain).ToList();
                return explainList;
            }
            Debuger.LogError(string.Format("找不到指定的类型{0}类型!", tyName));
            return null;
        }

        /// <summary>
        /// 获得该子枚举类型的所有索引值列表
        /// </summary>
        /// <returns></returns>
        public static List<int> GetInstanceIndexList<T>() where T : EnumInt
        {
            var att = System.Attribute.GetCustomAttribute(typeof(T), typeof(EnumTypeNameAttribute)) as EnumTypeNameAttribute;
            if (att == null) throw new Exception(string.Format("{0}没有实现RyEnumTypeNameAttribute特性", typeof(T).Name));

            var tyName = att.TypeName;
            if (SonTypeInstanceListDictionary.ContainsKey(tyName))
            {
                var sonType = SonTypeInstanceListDictionary[tyName];
                var tempList = sonType.Select(e => e.Value).ToList();
                return tempList;
            }
            Debuger.LogError(string.Format("找不到指定的类型{0}类型!", tyName));
            return null;
        }

        public static T GetInstance<T>(string tyName, System.Enum eEnum) where T : EnumInt
        {
            var instanceName = eEnum.ToString();
            return GetInstance<T>(tyName, instanceName);
        }

        public static T GetInstance<T>(string tyName, string instanceName) where T : EnumInt
        {
            if (SonTypeInstanceListDictionary.ContainsKey(tyName))
            {
                var sonType = SonTypeInstanceListDictionary[tyName];
                var instance = sonType.Find(i => i.Literals == instanceName);
                return instance as T;
            }
            Debuger.LogError(string.Format("找不到指定的类型{0}类型!", tyName));
            return null;
        }

        public static T GetInstance<T>(string tyName, int index) where T : EnumInt
        {
            if (SonTypeInstanceListDictionary.ContainsKey(tyName))
            {
                var sonType = SonTypeInstanceListDictionary[tyName];
                var instance = sonType.Find(i => i.Value == index);
                return instance as T;
            }
            Debuger.LogError(string.Format("找不到指定的类型{0}类型!", tyName));
            return null;
        }


        #endregion
    }
}
