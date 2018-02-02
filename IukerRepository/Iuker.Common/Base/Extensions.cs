/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2/12/2017 11:50:08
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 扩展
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
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Iuker.Common
{
    /// <summary>
    /// Iuker基础扩展和常用静态函数
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 使用指定的分隔符拼合一个字符串列表
        /// </summary>
        /// <param name="source">字符串源列表</param>
        /// <param name="split">拆分字符</param>
        /// <param name="isRemoveFirst">是否去掉结果字符串开始位置的拆分字符</param>
        /// <returns></returns>
        public static string ToUnion(this List<string> source, string split, bool isRemoveFirst = true)
        {
            var result = source.Aggregate<string, string>(null, (current, s) => current + (split + s));
            if (!isRemoveFirst) return result;
            if (result != null)
            {
                result = result.Remove(0, 1);
            }

            return result;
        }

        public static bool IsNull<T>(this T insance) where T : class
        {
            return insance == null;
        }

        public static void IfElse(bool condition, Action trueDo, Action falseDo)
        {
            if (condition)
            {
                trueDo();
            }
            else
            {
                falseDo();
            }
        }

        public static T Assign<T>(this T source, T newData)
        {
            source = newData;
            return source;
        }

        public static void FalseDo(this bool condition, Action falseAction)
        {
            if (!condition)
            {
                falseAction();
            }
        }

        public static void ExTrueDo(bool condition, Action trueAction)
        {
            if (condition)
            {
                trueAction();
            }
        }

        public static bool TrueDo(this bool condition, Action trueAction)
        {
            if (condition)
            {
                trueAction();
            }

            return condition;
        }

        /// <summary>
        /// 使用指定的分隔符拼合一个字符串数组
        /// </summary>
        /// <param name="source">字符串源数组</param>
        /// <param name="split">拆分字符</param>
        /// <param name="isRemoveFirst">是否去掉结果字符串开始位置的拆分字符</param>
        /// <returns></returns>
        public static string ToUnion(this string[] source, string split, bool isRemoveFirst = true)
        {
            return ToUnion(source.ToList(), split, isRemoveFirst);
        }

        public static T As<T>(this object obj) where T : class
        {
            T t = obj as T;
            return t;
        }

        /// <summary>
        /// 分割文本，去掉所有的换行子元素
        /// </summary>
        /// <param name="sourceList"></param>
        /// <returns></returns>
        public static List<string> RemoveAllLineFeed(this List<string> sourceList)
        {
            sourceList.RemoveAll(a => a == "");
            return sourceList;
        }

        /// <summary>
        /// 将给定的操作执行指定的次数
        /// </summary>
        /// <param name="executeCount"></param>
        /// <param name="action"></param>
        public static void Execute(this int executeCount, Action action)
        {
            for (var i = 0; i < executeCount; i++)
            {
                action();
            }
        }

        /// <summary>
        /// 判断给定的整数是否为偶数
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool IsEven(this int count)
        {
            var result = (count % 2) == 0;
            return result;
        }

        #region 基础运算

        /// <summary>
        /// 减法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int Minus(this int source, int target)
        {
            return source - target;
        }

        public static float Minus(this float source, float target)
        {
            return source - target;
        }

        public static long Minus(this long source, long target)
        {
            return source - target;
        }

        public static double Minus(this double source, double target)
        {
            return source - target;
        }

        public static decimal Minus(this decimal source, decimal target)
        {
            return source - target;
        }

        /// <summary>
        /// 加法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int Plus(this int source, int target)
        {
            return source + target;
        }

        /// <summary>
        /// 乘法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int Multiply(this int source, int target)
        {
            return source * target;
        }

        public static bool Greater(this byte left, byte right)
        {
            return left > right;
        }

        public static bool Greater(this int left, int right)
        {
            return left > right;
        }

        public static bool Greater(this ushort left, ushort right)
        {
            return left > right;
        }

        public static bool Greater(this long left, long right)
        {
            return left > right;
        }

        public static bool Greater(this float left, float right)
        {
            return left > right;
        }

        public static bool Greater(this double left, double right)
        {
            return left > right;
        }

        public static bool Greater(this decimal left, decimal right)
        {
            return left > right;
        }

        public static bool GreaterEqual(this byte left, byte right)
        {
            return left >= right;
        }

        public static bool GreaterEqual(this int left, int right)
        {
            return left >= right;
        }

        public static bool GreaterEqual(this ushort left, ushort right)
        {
            return left >= right;
        }

        public static bool GreaterEqual(this long left, long right)
        {
            return left >= right;
        }

        public static bool GreaterEqual(this float left, float right)
        {
            return left >= right;
        }

        public static bool GreaterEqual(this double left, double right)
        {
            return left >= right;
        }

        public static bool GreaterEqual(this decimal left, decimal right)
        {
            return left >= right;
        }

        public static bool Less(this byte left, byte right)
        {
            return left < right;
        }

        public static bool Less(this int left, int right)
        {
            return left < right;
        }

        public static bool Less(this ushort left, ushort right)
        {
            return left < right;
        }

        public static bool Less(this long left, long right)
        {
            return left < right;
        }

        public static bool Less(this float left, float right)
        {
            return left < right;
        }

        public static bool Less(this double left, double right)
        {
            return left < right;
        }

        public static bool Less(this decimal left, decimal right)
        {
            return left < right;
        }

        public static bool LessEqual(this byte left, byte right)
        {
            return left <= right;
        }

        public static bool LessEqual(this int left, int right)
        {
            return left <= right;
        }

        public static bool LessEqual(this ushort left, ushort right)
        {
            return left <= right;
        }

        public static bool LessEqual(this long left, long right)
        {
            return left <= right;
        }

        public static bool LessEqual(this float left, float right)
        {
            return left <= right;
        }

        public static bool LessEqual(this double left, double right)
        {
            return left <= right;
        }

        public static bool LessEqual(this decimal left, decimal right)
        {
            return left <= right;
        }

        public static bool And(this bool left, bool right)
        {
            return left && right;
        }

        #endregion


        #region swith扩展

        public class SwithCase<TCase, TInput>
        {
            private Dictionary<TCase, Action<TInput>> _caseDictionary;

            private readonly TCase _sourceTCase;

            private readonly TInput _sourceTInput;

            public SwithCase(TCase sourceTCase, TInput sourceTInput)
            {
                _sourceTCase = sourceTCase;
                _sourceTInput = sourceTInput;
            }

            public SwithCase<TCase, TInput> Case(TCase tCase, Action<TInput> action)
            {
                if (_caseDictionary == null)
                {
                    _caseDictionary = new Dictionary<TCase, Action<TInput>>();
                }
                _caseDictionary.Add(tCase, action);
                return this;
            }

            /// <summary>
            /// 启动执行流
            /// </summary>
            /// <param name="defaultAction"></param>
            public void Default(Action<TInput> defaultAction = null)
            {
                if (_caseDictionary.ContainsKey(_sourceTCase)) _caseDictionary[_sourceTCase](_sourceTInput);
                else
                {
                    if (defaultAction != null)
                    {
                        defaultAction(_sourceTInput);
                    }
                }
            }
        }

        public static SwithCase<TCase, TInput> Case<TCase, TInput>(this SwithCase<TCase, TInput> swithCase,
            TCase tCase, Action<TInput> action)
        {
            return swithCase.Case(tCase, action);
        }

        public static SwithCase<TCase, TInput> Swith<TCase, TInput>(this TCase sourceTCase, TInput tInput)
        {
            SwithCase<TCase, TInput> swithCase = new SwithCase<TCase, TInput>(sourceTCase, tInput);
            return swithCase;
        }

        #endregion

        #region 字符串扩展

        /// <summary>
        /// 将一个字符串从头部向后截取
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static string StartCut(this string source, int startIndex)
        {
            var result = source.Substring(startIndex, source.Length - 2);
            return result;
        }

        /// <summary>
        /// 将一个字符串从尾部往前反向截取
        /// </summary>
        /// <param name="source"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static string EndCut(this string source, int endIndex)
        {
            var result = source.Substring(source.Length - endIndex, endIndex);
            return result;
        }

        /// <summary>
        /// 将字符串转为枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T AsEnum<T>(this string source)
        {
            var result = (T)Enum.Parse(typeof(T), source);
            return result;
        }

        public static string LastDir(this string source)
        {
            return source.Split('\\').Last();
        }

        public static string FirstDir(this string source)
        {
            return source.Split('/').First();
        }

        public static string GetDir(this string source, int index)
        {
            return source.Split('/')[index];
        }

        public static string GetNewDir(this string source, string newRootDir)
        {
            if (source.EndsWith("/")) source = source.Substring(0, source.Length - 2);
            var last = source.Split('/').Last();
            var result = newRootDir + last;
            return result;
        }

        /// <summary>
        /// 将给定的字符串依据指定的分隔符拆开并将分隔符之后的内容丢弃
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="isContainTarget"></param>
        /// <returns></returns>
        public static string TrimTargetCharAfter(this string source, string target, bool isContainTarget = false)
        {
            var index = source.LastIndexOf(target, StringComparison.Ordinal);
            var result = isContainTarget == false ? source.Substring(0, source.Length - (source.Length - index)) : source.Substring(0, source.Length - (source.Length - index - 1));

            return result;
        }

        /// <summary>
        /// 清理换行符
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string CleanBr(this string source)
        {
            source = source.Replace("\n", "");
            source = source.Replace("\r", "");
            return source;
        }

        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 字符串是否以数字开头
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsStartWithDigit(this string source)
        {
            return char.IsDigit(source.First());
        }

        public static string FileName(this string source)
        {
            var result = Path.GetFileNameWithoutExtension(source);
            return result;
        }

        /// <summary>
        /// 判断一个字符串是否包含大写字母
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool Containcapital(this string source)
        {
            return Regex.IsMatch(source, "[A-Z]");
        }

        public static int ToInt32(this string str)
        {
            var result = Convert.ToInt32(str);
            return result;
        }

        #endregion

        #region 集合扩展

        public static bool IsNullOrZero<T>(this IList<T> list)
        {
            if (list == null) return true;
            if (list.Count == 0) return true;
            return false;
        }

        /// <summary>
        /// 安全添加
        /// 当要添加的目标元素不为空时执行添加操作。
        /// </summary>
        /// <typeparam name="T">集合中元素类型</typeparam>
        /// <param name="list">待添加的目标集合</param>
        /// <param name="t">待添加的目标元素</param>
        /// <returns>返回操作后的目标集合</returns>
        public static ICollection<T> SafeAdd<T>(this ICollection<T> list, T t) where T : class
        {
            if (t != null) list.Add(t);
            return list;
        }

        /// <summary>
        /// 安全移除
        /// 当要移除的目标元素不为空且目标集合包含该元素时执行移除操作
        /// </summary>
        /// <typeparam name="T">集合中元素类型</typeparam>
        /// <param name="list">待移除的目标集合</param>
        /// <param name="t">待移除的目标元素</param>
        /// <returns>返回操作后的目标集合</returns>
        public static ICollection<T> SafeRemove<T>(this ICollection<T> list, T t) where T : class
        {
            if (t != null && list.Contains(t)) list.Remove(t);
            return list;
        }

        /// <summary>
        /// 当目标委托和目标集合不为为空时执行将目标委托从目标元素中移除的操作。
        /// </summary>
        /// <param name="action"></param>
        /// <param name="actionList"></param>
        public static void RemoveSelfFormCollection(this Action action, ICollection<Action> actionList)
        {
            if (action == null) return;
            actionList.Remove(action);
        }

        /// <summary>
        /// 对集合内每个元素执行传入的委托然后返回集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ICollection<T> Each<T>(this ICollection<T> collection, Action<T> action)
        {
            foreach (var x1 in collection)
            {
                action(x1);
            }
            return collection;
        }

        /// <summary>
        /// 将源集合与指定集合中相同的元素移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="removeList"></param>
        /// <returns></returns>
        public static IList<T> RemoveRange<T>(this IList<T> source, IList<T> removeList)
        {
            foreach (var element in removeList)
            {
                source.Remove(element);
            }
            return source;
        }

        /// <summary>
        /// 从给定的字典数组中移除一个指定的键值对
        /// 移除操作只会执行一次
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <param name="key"></param>
        /// <param name="dictionarys"></param>
        public static void RemoveKeyOnce<TK>(TK key, params Dictionary<TK, object>[] dictionarys)
        {
            foreach (var dictionary in dictionarys)
            {
                if (!dictionary.ContainsKey(key)) continue;
                dictionary.Remove(key);
                return;
            }
        }

        /// <summary>
        /// 检查一个整形列表中的所有项是否连续相连
        /// 即后面一项比前面一项大一
        /// </summary>
        /// <param name="list">待检查的整形列表</param>
        /// <param name="fillCount">需要填补的次数</param>
        /// <returns></returns>
        public static bool IsContinuous(this IList<int> list, out int fillCount)
        {
            bool result = false;
            fillCount = 0;
            list = list.OrderBy(r => r).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var current = list[i];
                if (i < list.Count - 1)
                {
                    var after = list[i + 1];
                    if (current != after - 1)
                    {
                        fillCount = after - (current + 1);
                    }
                    result = current == after - 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 判断一个整形列表中所有项的值是否只有两种种类
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsTwoDouble(this IList<int> list)
        {
            HashSet<int> intSet = new HashSet<int>();
            int sameCount = 0;
            foreach (var item in list)
            {
                if (!intSet.Contains(item))
                {
                    sameCount++;
                    intSet.Add(item);
                }
            }

            return intSet.Count == 2;
        }

        /// <summary>
        /// 使用给定的委托判断一个列表中是否所有项都满足委托的判断条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="matcher"></param>
        /// <returns></returns>
        public static bool MatchAll<T>(this IList<T> list, Func<T, bool> matcher)
        {
            bool result = false;
            foreach (var item in list)
            {
                result = matcher(item);
            }

            return result;
        }

        /// <summary>
        /// 判断一个整形列表中是否所有项都相等
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool AllEqual(this List<int> list)
        {
            var standard = list[0];
            var result = list.MatchAll(r => r == standard);
            return result;
        }


        /// <summary>
        /// 对一个列表进行切片操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static List<T> Cut<T>(this List<T> source, int count, int start = 0)
        {
            var newList = new List<T>();
            for (var i = 0; i < count; i++)
            {
                newList.Add(source[start + i]);
            }
            return newList;
        }

        public static List<T> RandomRemove<T>(this List<T> source, int count = 1, List<T> removed = null)
        {
            var childs = new List<T>();
            var random = new Random();
            for (var i = 0; i < count; i++)
            {
                var index = random.Next(0, source.Count);
                var child = source[index];
                childs.Add(child);
            }

            for (var i = 0; i < count; i++)
            {
                if (removed != null)
                {
                    removed.Add(childs[i]);
                }
                source.Remove(childs[i]);
            }

            return childs;
        }

        public static void ForEachByIndex<T>(this List<T> list, Action<IndexObject<T>> Action)
        {
            var indexList = new List<IndexObject<T>>();
            for (var i = 0; i < list.Count; i++)
            {
                var s = list[i];
                var indexObject = new IndexObject<T>();
                indexObject.Init(s, i);
                indexList.Add(indexObject);
            }

            indexList.ForEach(Action);
        }

        public static string SelectUnion(this List<string> list, string hold, params int[] indexs)
        {
            var result = string.Empty;
            var temps = indexs.Select(index => list[index]).ToList();

            return temps.Aggregate(result, (current, s) => current + hold + s);
        }

        /// <summary>
        /// Linq索引包装对象
        /// 持有给定对象并绑定对象的索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class IndexObject<T>
        {
            public int Index { get; private set; }
            public T Source { get; private set; }

            public IndexObject<T> Init(T source, int index)
            {
                Index = index;
                Source = source;

                return this;
            }
        }

        public static T RandomGet<T>(this List<T> source)
        {
            var random = new Random();
            var index = random.Next(0, source.Count);
            var result = source[index];
            return result;
        }

        #region 列表

        //public static List<T> ToList<T>(this IEnumerable<T> enumerable, ref List<T> targetList)
        //{
        //    if (targetList == null) throw new ArgumentNullException(nameof(targetList));
        //    targetList = enumerable.ToList();
        //    return targetList;
        //}

        #endregion

        #region 字典

        /// <summary>
        /// 合并两个字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static void Combin<T, TV>(this IDictionary<T, TV> source, Dictionary<T, TV> target)
        {
            foreach (KeyValuePair<T, TV> keyValuePair in target)
            {
                if (source.ContainsKey(keyValuePair.Key))
                {
                    Debuger.LogWarning(string.Format("Key{0}当前已存在，请检查！", keyValuePair.Key));
                }
                else
                {
                    source.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
        }

        /// <summary>
        /// 对给定字典中的每一对键值对执行给定的委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="del"></param>
        public static void ForEach<T, V>(this Dictionary<T, V> dictionary, Action<T, V> del)
        {
            foreach (var kv in dictionary)
            {
                if (del != null)
                {
                    del(kv.Key, kv.Value);
                }
            }
        }


        #endregion

        #region 栈

        /// <summary>
        /// 对栈中的每一个元素都执行委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack"></param>
        /// <param name="doAction"></param>
        public static void ForEach<T>(this Stack<T> stack, Action<T> doAction)
        {
            foreach (var x1 in stack)
            {
                doAction(x1);
            }
        }

        #endregion


        #endregion

        #region 枚举扩展

        /// <summary>
        /// 获取枚举类子项描述信息
        /// </summary>
        /// <param name="eEnum">枚举子项</param>
        /// <returns></returns>
        public static string Desc(this Enum eEnum)
        {
            string strValue = eEnum.ToString();

            FieldInfo fieldinfo = eEnum.GetType().GetField(strValue);
            object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs.Length == 0)
            {
                return strValue;
            }
            DescriptionAttribute da = (DescriptionAttribute)objs[0];
            return da.Description;
        }

        /// <summary>
        /// 获得指定类型枚举所有字段名列表
        /// </summary>
        /// <param name="eEnum"></param>
        /// <returns></returns>
        public static List<string> GetAllEnumFiled(this Enum eEnum)
        {
            var tempList = eEnum.GetType().GetFields().Select(f => f.Name).ToList();
            tempList.RemoveAt(0);
            return tempList;
        }

        #endregion

        #region 本地数据

        public static List<int> ToListInt(this string source)
        {
            var intList = new List<int>();
            var strList = source.Split(',').ToList();
            intList.AddRange(strList.Select(item => Convert.ToInt32(item)));
            return intList;
        }

        public static List<float> ToListFloat(this string source)
        {
            var floatList = new List<float>();
            var strList = source.Split(',').ToList();
            floatList.AddRange(strList.Select(float.Parse));
            return floatList;
        }

        /// <summary>
        /// 将字符串列表转换为用逗号分隔的字符串，可以传入指定的分隔符
        /// </summary>
        /// <param name="list">要转换的字符串列表</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string ConvertListStringToString(this List<string> list, string separator = ",")
        {
            var line = list.Aggregate(string.Empty, (current, son) => current + separator + son);

            return line.Substring(1, line.Length - 1);
        }

        /// <summary>
        /// 添加一个列表到另一个object列表中
        /// </summary>
        /// <param TypeName="array"></param>
        /// <returns></returns>
        public static List<object> ConverToObjectList(IEnumerable<object> array)
        {
            List<object> list = new List<object>();
            list.AddRange(array);
            return list;
        }


        /// <summary>
        /// CN：将一个Int列表转换为本地数据字符串
        /// EN：Converts an Int list to the local Data string
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ConvertListIntToString(List<int> list)
        {
            string listIntString = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                if (i != list.Count - 1)
                {
                    listIntString = listIntString + list[i] + ",";
                }
                else
                {
                    listIntString = listIntString + list[i];
                }
            }
            return listIntString;
        }

        #endregion

        #region 特性扩展

        public static bool IsObsolete(this PropertyInfo p)
        {
            return Attribute.GetCustomAttribute(p, typeof(ObsoleteAttribute)) != null;
        }

        //public static T GetAttribute<T>(this T instance, Type attributeType) where T : class
        //{
        //    var attribute = Attribute.GetCustomAttribute(instance, attributeType);
        //}



        #endregion

        #region Web扩展

        public static string GetHeadValue(this NameValueCollection nameValueCollection, string key, int index = 0)
        {
            var values = nameValueCollection.GetValues(key);
            if (values != null)
            {
                var result = values[index];
                return result;
            }
            throw new ArgumentNullException("values");
        }

        #endregion

        /// <summary>
        /// 清空一个字符串构建实例
        /// </summary>
        /// <param name="stringBuilder"></param>
        public static void Clear(this StringBuilder stringBuilder)
        {
            stringBuilder.Remove(0, stringBuilder.Length);
        }


        #region 脚本自动化创建（StringBuilder）

        /// <summary>
        /// 写入注释
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="note"></param>
        /// <param name="paramNameList">参数名列表</param>
        /// <param name="paramNoteList">参数注释文字列表</param>
        /// <param name="space">缩进空格符文本</param>
        public static void AppendCsharpNote(this StringBuilder sb, string note, List<string> paramNameList = null, List<string> paramNoteList = null, string space = null)
        {
            sb.AppendLine(space + "/// <summary>");
            sb.AppendLine(space + "/// " + note);
            sb.AppendLine(space + "/// </summary>");
            //拆注释参数集合
            if (paramNameList == null)
                return;
            if (paramNoteList == null)
            {
                foreach (var paramName in paramNameList)
                {
                    sb.AppendLine(space + "/// <param name=\"" + paramName + "\"></param>");
                }
            }
            else
            {
                for (int i = 0; i < paramNameList.Count; i++)
                {
                    string paramName = paramNameList[i];
                    string paramNoe = paramNoteList[i];
                    sb.AppendLine(string.Format(space + "/// <param name=\"{0}\">{1}</param>", paramName, paramNoe));
                }
            }
        }

        /// <summary>
        /// 写入脚本文件的注释头
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="client"></param>
        /// <param name="email"></param>
        /// <param name="notes"></param>
        public static void AppendCsahrpFileInfo(this StringBuilder sb, string client, string email, params string[] notes)
        {
            sb.AppendLine("/***********************************************************************************************");
            sb.AppendLine(string.Format("Author：{0}", client));
            sb.AppendLine("CreateDate: " + DateTime.Now);
            sb.AppendLine(string.Format("Email: {0}", email));
            sb.AppendLine("***********************************************************************************************/");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("/*");
            foreach (var note in notes)
            {
                sb.AppendLine(note);
            }
            sb.AppendLine("*/");
            sb.AppendLine();
        }

        public static StringBuilder AppendTypeScriptFileNode(this StringBuilder sb, string coderName,
            string coderEmail, string purpose)
        {
            sb.AppendLine(string.Format("//   Author    : {0}", coderName));
            sb.AppendLine(string.Format("//   Email     : {0}", coderEmail));
            sb.AppendLine(string.Format("//   Date      : {0}", Constant.Constant.DateAndTime));
            sb.AppendLine(string.Format("//   Purpose   : {0}", purpose));
            sb.AppendLine();

            return sb;
        }

        /// <summary>
        /// 写入脚本文件的命名空间
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="namespaces"></param>
        public static void WriteNameSpace(this StringBuilder sb, params string[] namespaces)
        {
            foreach (var ns in namespaces)
            {
                sb.AppendLine(ns);
            }
            sb.AppendLine();
        }

        /// <summary>
        /// 写入一个TryCatch代码块
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="space"></param>
        public static void WriteTryCatch(this StringBuilder sb, string space = "    ")
        {
            sb.AppendLine(space + "Try");
            sb.AppendLine(space + "{");
            sb.AppendLine(space);
            sb.AppendLine(space + "}");
            sb.AppendLine(space + "{");
        }

        /// <summary>
        /// 写入一个字段或属性的代码字符串
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="content"></param>
        public static void WriteFiledOrProperty(this StringBuilder sb, string content)
        {
            sb.AppendLine(content);
            sb.AppendLine();
        }

        /// <summary>
        /// 写入脚本文件的注释头
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="client"></param>
        /// <param name="email"></param>
        /// <param name="notes"></param>
        public static void WriteFileInfo(this StringBuilder sb, string client, string email, params string[] notes)
        {
            sb.AppendLine("/***********************************************************************************************");
            sb.AppendLine(string.Format("Author：{0}", client));
            sb.AppendLine("CreateDate: " + DateTime.Now);
            sb.AppendLine(string.Format("Email: {0}", email));
            sb.AppendLine("***********************************************************************************************/");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("/*");
            foreach (var note in notes)
            {
                sb.AppendLine(note);
            }
            sb.AppendLine("*/");
            sb.AppendLine();
        }


        public static void WriteStandardMethod(this StringBuilder sb, string methadHead, string methodBody = "    ", string space = null)
        {
            sb.AppendLine(methadHead);
            sb.AppendLine(space + "{");
            sb.AppendLine(space + methodBody);
            sb.AppendLine(space + "}");
            sb.AppendLine();
        }

        /// <summary>
        /// 写入注释
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="note"></param>
        /// <param name="paramNameList">参数名列表</param>
        /// <param name="paramNoteList">参数注释文字列表</param>
        /// <param name="space">缩进空格符文本</param>
        public static void WriteNote(this StringBuilder sb, string note, List<string> paramNameList = null, List<string> paramNoteList = null, string space = null)
        {
            sb.AppendLine(space + "/// <summary>");
            sb.AppendLine(space + "/// " + note);
            sb.AppendLine(space + "/// </summary>");
            //拆注释参数集合
            if (paramNameList == null)
                return;
            if (paramNoteList == null)
            {
                foreach (var paramName in paramNameList)
                {
                    sb.AppendLine(space + "/// <param name=\"" + paramName + "\"></param>");
                }
            }
            else
            {
                for (int i = 0; i < paramNameList.Count; i++)
                {
                    string paramName = paramNameList[i];
                    string paramNoe = paramNoteList[i];
                    sb.AppendLine(string.Format(space + "/// <param name=\"{0}\">{1}</param>", paramName, paramNoe));
                }
            }
        }

        public static void WriteTsComment(this StringBuilder sb, string comment, string space = "        ")
        {
            sb.AppendLine(space + "/**");
            sb.AppendLine(space + "* " + comment);
            sb.AppendLine(space + "*/");
        }

        #endregion

        #region Float

        public static bool EqualFloat(this float source, float right)
        {
            return Math.Abs(source - right) < 0.0001;
        }


        #endregion

        #region Protobuf



        #endregion
    }
}