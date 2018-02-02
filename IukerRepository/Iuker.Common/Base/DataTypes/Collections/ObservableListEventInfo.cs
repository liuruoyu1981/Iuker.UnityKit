/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/28/2017 14:49
Email: liuruoyu1981@gmail.com
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

namespace Iuker.Common.DataTypes.Collections
{
    /// <summary>
    /// 可观察列表事件数据
    /// 用于缓存对于可观察列表项的事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableListEventInfo<T>
    {
        public Action<IList<T>, T> mOnAdd { get; private set; }
        public Action<IList<T>, T> mOnRemove { get; private set; }
        public Action<IList<T>, T> mOnInsert { get; private set; }
        public Action mOnClear { get; private set; }
        public Action<IList<T>, IList<T>> mOnInit { get; private set; }

        public ObservableListEventInfo(Action<IList<T>, IList<T>> onInit, Action<IList<T>, T> onAdd = null, Action<IList<T>, T> onRemove = null,
            Action<IList<T>, T> onInsert = null, Action onClear = null)
        {
            mOnInit = onInit;
            mOnAdd = onAdd;
            mOnRemove = onRemove;
            mOnInsert = onInsert;
            mOnClear = onClear;
        }
    }
}