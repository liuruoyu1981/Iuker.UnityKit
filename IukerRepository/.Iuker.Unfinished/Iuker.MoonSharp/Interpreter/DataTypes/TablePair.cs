/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/16 11:14:22
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



namespace Iuker.MoonSharp.Interpreter
{
    /// <summary>
    /// 表示lua表对象所使用的键值对的类
    /// </summary>
    public struct TablePair
    {
        private static TablePair s_NilNode = new TablePair(DynValue.Nil, DynValue.Nil);
        public DynValue key, value;

        public DynValue Key
        {
            get { return key; }
            private set { Key = key; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public DynValue Value
        {
            get { return value; }
            set { if (key.IsNotNil()) Value = value; }
        }

        public TablePair(DynValue key, DynValue val)
        {
            this.key = key;
            this.value = val;
        }

        /// <summary>
        /// 获取一个键值都为Nil的键值对实例
        /// </summary>
        public static TablePair Nil => s_NilNode;
    }
}
