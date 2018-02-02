/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/06/21 11:57
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
using Iuker.Common.Event;
using Iuker.Common.Base.DataBinding;

namespace Iuker.Common.DataTypes.ReactiveDatas
{
    /// <summary>
    /// 响应式Int32
    /// </summary>
    public class ReactiveInt32 : IReactiveStruct<int>, IBindingDataSource<int>
    {
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (mOnInitCaller != null ? mOnInitCaller.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (mOnUpdateCaller != null ? mOnUpdateCaller.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (mCloseActions != null ? mCloseActions.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (mBindingWidgets != null ? mBindingWidgets.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Value;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public int CompareTo(ReactiveInt32 other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(ReactiveInt32 other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ReactiveInt32))
            {
                return false;
            }
            ReactiveInt32 other = (ReactiveInt32)obj;
            return Value.Equals(other.Value);
        }


        public void Assign(int newValue)
        {
            if (Value != newValue)
            {
                Value = newValue;
                mOnUpdateCaller.CallEventHanlder(Value);
            }
        }

        public void Close()
        {
            //  调用所有的关闭事件委托
            mCloseActions.ForEach(del => del(this));
        }

        public static bool operator ==(ReactiveInt32 e1, ReactiveInt32 e2)
        {
            if (ReferenceEquals(e1, e2)) { return true; }
            return e2 != null && e1 != null && e1.Value == e2.Value;
        }

        public static bool operator !=(ReactiveInt32 e1, ReactiveInt32 e2)
        {
            if (ReferenceEquals(e1, e2)) { return false; }
            if (e1.Value != e2.Value) return true;
            return true;
        }

        public static bool operator <(ReactiveInt32 e1, ReactiveInt32 e2)
        {
            return e2 != null && (e1 != null && e1.Value < e2.Value);
        }

        public static bool operator <=(ReactiveInt32 e1, ReactiveInt32 e2)
        {
            return e2 != null && (e1 != null && e1.Value <= e2.Value);
        }

        public static bool operator >(ReactiveInt32 e1, ReactiveInt32 e2)
        {
            return e2 != null && (e1 != null && e1.Value > e2.Value);
        }

        public static bool operator >=(ReactiveInt32 e1, ReactiveInt32 e2)
        {
            return e2 != null && (e1 != null && e1.Value >= e2.Value);
        }

        private readonly TallyEventHandlerCaller<int> mOnInitCaller = new TallyEventHandlerCaller<int>();
        private readonly TallyEventHandlerCaller<int> mOnUpdateCaller = new TallyEventHandlerCaller<int>();

        public IReactiveStruct<int> AddOnInitAction(Action<int> onInit, int executeCount = -1)
        {
            mOnInitCaller.AddHandler(onInit, executeCount);

            return this;
        }

        public IReactiveStruct<int> AddOnUpdateAction(Action<int> onUpdate, int executeCount = -1)
        {
            mOnUpdateCaller.AddHandler(onUpdate);

            return this;
        }

        public int Value { get; private set; }
        public IReactiveStruct<int> Init(int sourceValue)
        {
            Value = sourceValue;
            mOnInitCaller.CallEventHanlder(Value);

            return this;
        }


        /// <summary>
        /// 数据关闭事件处理委托列表
        /// </summary>
        private readonly List<Action<IBindingDataSource<int>>> mCloseActions = new List<Action<IBindingDataSource<int>>>();

        #region 双向绑定

        /// <summary>
        /// 所绑定的所有可绑定控件列表
        /// </summary>
        private readonly List<IBindingWidget<int>> mBindingWidgets = new List<IBindingWidget<int>>();


        public void Binding(IBindingWidget<int> widget)
        {
            if (!mBindingWidgets.Contains(widget))
            {
                mBindingWidgets.Add(widget);
                return;
            }
            throw new Exception("This binding widget is binding!");
        }

        public void ReceiveUpdate(IBindingWidget<int> widget)
        {
            var newValue = widget.WidgetValue;
            Assign(newValue);
        }

        public int DataValue { get { return Value; } }

        public void WatchClose(Action<IBindingDataSource<int>> cleanAction)
        {
            if (!mCloseActions.Contains(cleanAction))
            {
                mCloseActions.Add(cleanAction);
                return;
            }
            throw new Exception("This cleanAction widget is added!");
        }

        #endregion


    }
}