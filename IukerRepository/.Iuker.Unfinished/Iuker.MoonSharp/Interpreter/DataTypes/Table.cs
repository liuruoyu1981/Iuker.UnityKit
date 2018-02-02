/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/16 11:12:56
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


using System.Collections.Generic;
using Iuker.MoonSharp.Interpreter.DataStructs;
using Iuker.MoonSharp.Interpreter.DataTypes;

namespace Iuker.MoonSharp.Interpreter
{
    /// <summary>
    /// 表示lua表对象的类
    /// </summary>
    public class Table : RefIdObject, IScriptPrivateResource
    {
        private readonly LinkedList<TablePair> m_Values;
        private readonly LinkedListIndex<DynValue, TablePair> m_ValueMap;
        private readonly LinkedListIndex<string, TablePair> m_StringMap;
        private readonly LinkedListIndex<int, TablePair> m_ArrayMap;
        private readonly Script m_Owner;

        int m_InitArray = 0;
        int m_CachedLength = -1;
        bool m_ContainsNilEntries = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="owner">The owner script.</param>
        public Table(Script owner)
        {
            m_Values = new LinkedList<TablePair>();
            m_StringMap = new LinkedListIndex<string, TablePair>(m_Values);
            m_ArrayMap = new LinkedListIndex<int, TablePair>(m_Values);
            m_ValueMap = new LinkedListIndex<DynValue, TablePair>(m_Values);
            m_Owner = owner;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="arrayValues">The values for the "array-like" part of the table.</param>
        public Table(Script owner, params DynValue[] arrayValues)
            : this(owner)
        {
            for (int i = 0; i < arrayValues.Length; i++)
            {
                this.Set(DynValue.NewNumber(i + 1), arrayValues[i]);
            }
        }

        /// <summary>
        /// Gets the script owning this resource.
        /// </summary>
        public Script OwnerScript
        {
            get { return m_Owner; }
        }

        private static DynValue RawGetValue(LinkedListNode<TablePair> linkedListNode)
        {
            return (linkedListNode != null) ? linkedListNode.Value.Value : null;
        }

        /// <summary>
        /// 获取与指定的键相关联的值,
        /// 不会使non-existant为零值。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DynValue RawGet(string key)
        {
            return RawGetValue(m_StringMap.Find(key));
        }

















































































































































        public void Set(DynValue key, DynValue value)
        {
        }

















































































































































































































































































































































































































































        /// <summary>
        /// Gets the length of the "array part".
        /// </summary>
        public int Length
        {
            get
            {
                if (m_CachedLength < 0)
                {
                    m_CachedLength = 0;

                    for (int i = 1; m_ArrayMap.ContainsKey(i) && !m_ArrayMap.Find(i).Value.Value.IsNil(); i++)
                        m_CachedLength = i;
                }

                return m_CachedLength;
            }
        }




    }
}
