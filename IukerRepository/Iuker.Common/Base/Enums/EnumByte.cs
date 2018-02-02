using System;
using System.Collections.Generic;
using System.Linq;

namespace Iuker.Common.Base
{
    /// <summary>
    /// 使用byte作为基础值的可继承枚举
    /// </summary>
    public abstract class EnumByte : IComparable<EnumByte>, IEquatable<EnumByte>, IukEnum
    {
        /// <summary>
        /// 子类型实例索引字典
        /// </summary>
        protected static readonly Dictionary<string, byte> SonTypeInstanceIndexDictionary
            = new Dictionary<string, byte>();

        /// <summary>
        /// 子类型实例列表字典
        /// </summary>
        protected static readonly Dictionary<string, List<EnumByte>> SonTypeInstanceListDictionary
            = new Dictionary<string, List<EnumByte>>();

        /// <summary>
        /// 子类型实例枚举含义说明字典
        /// </summary>
        protected static readonly Dictionary<string, Dictionary<byte, string>> SonTypeInstanceExplainDictionary
            = new Dictionary<string, Dictionary<byte, string>>();

        /// <summary>
        /// 字面量
        /// </summary>
        public readonly string Literals;

        /// <summary>
        /// 基础数据值，短整型
        /// </summary>
        public readonly byte Value;

        static EnumByte()
        {

        }

        protected EnumByte(byte index, string sonTypeName, string enumName, string instanceEnumExplain)
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
                SonTypeInstanceListDictionary.Add(sonTypeName, new List<EnumByte> { this });
                SonTypeInstanceExplainDictionary.Add(sonTypeName, new Dictionary<byte, string>());
                SonTypeInstanceExplainDictionary[sonTypeName].Add(Value, instanceEnumExplain);
            }
        }

        protected EnumByte(string sonTypeName, string enumName, string instanceEnumExplain)
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
                SonTypeInstanceListDictionary.Add(sonTypeName, new List<EnumByte> { this });
                SonTypeInstanceExplainDictionary.Add(sonTypeName, new Dictionary<byte, string>());
                SonTypeInstanceExplainDictionary[sonTypeName].Add(Value, instanceEnumExplain);
            }
        }

        /// <summary>
        /// 保护权限构造函数用于静态初始化防止空引用异常
        /// </summary>
        protected EnumByte() { }

        public int CompareTo(EnumByte other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(EnumByte other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EnumByte))
            {
                return false;
            }
            EnumByte other = (EnumByte)obj;
            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return (Literals + Value).GetHashCode();
        }
        public static bool operator ==(EnumByte e1, EnumByte e2)
        {
            if (ReferenceEquals(e1, e2)) { return true; }
            return false;
        }

        public static bool operator !=(EnumByte e1, EnumByte e2)
        {
            if (ReferenceEquals(e1, e2)) { return false; }
            return true;
        }

        public static bool operator <(EnumByte e1, EnumByte e2)
        {
            return e2 != null && (e1 != null && e1.Value < e2.Value);
        }

        public static bool operator <=(EnumByte e1, EnumByte e2)
        {
            return e2 != null && (e1 != null && e1.Value <= e2.Value);
        }

        public static bool operator >(EnumByte e1, EnumByte e2)
        {
            return e2 != null && (e1 != null && e1.Value > e2.Value);
        }

        public static bool operator >=(EnumByte e1, EnumByte e2)
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

        private static string GetTypeName<T>() where T : EnumByte
        {
            var att = Attribute.GetCustomAttribute(typeof(T), typeof(EnumTypeNameAttribute)) as EnumTypeNameAttribute;
            if (att == null) throw new Exception(string.Format("{0}没有实现RyEnumTypeNameAttribute特性", typeof(T).Name));
            return att.TypeName;
        }

        /// <summary>
        /// 获得指定子类枚举的所有实例列表
        /// </summary>
        /// <returns></returns>
        public static List<EnumByte> GetSonTypeList<T>() where T : EnumByte
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
        public static List<string> GetInstanceNameList<T>() where T : EnumByte
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
        public static List<string> GetAllInstanceExplainList<T>() where T : EnumByte
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
        public static List<byte> GetInstanceIndexList<T>() where T : EnumByte
        {
            var tyName = GetTypeName<T>();
            if (SonTypeInstanceListDictionary.ContainsKey(tyName))
            {
                var sonType = SonTypeInstanceListDictionary[tyName];
                var tempList = sonType.Select(e => e.Value).ToList();
                return tempList;
            }
            Debuger.LogError(string.Format("找不到指定的类型{0}类型!", tyName));
            return null;
        }

        public static T GetInstance<T>(System.Enum eEnum) where T : EnumByte
        {
            var instanceName = eEnum.ToString();
            return GetInstance<T>(GetTypeName<T>(), instanceName);
        }

        public static T GetInstance<T>(string tyName, string instanceName) where T : EnumByte
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

        public static T GetInstance<T>(byte index) where T : EnumByte
        {
            var tyName = GetTypeName<T>();
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
