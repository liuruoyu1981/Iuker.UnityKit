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
using Iuker.Common.Event;

namespace Iuker.Common.DataTypes.ReactiveDatas
{
    /// <summary>
    /// 响应式Int32
    /// </summary>
    public class ReactiveBool : IReactiveStruct<bool>
    {
        public override string ToString()
        {
            return Value.ToString();
        }

        public int CompareTo(ReactiveBool other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(ReactiveBool other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ReactiveBool))
            {
                return false;
            }
            ReactiveBool other = (ReactiveBool)obj;
            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public void Assign(bool newValue)
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
        }

        public static bool operator ==(ReactiveBool e1, ReactiveBool e2)
        {
            if (ReferenceEquals(e1, e2)) { return true; }
            if (e1 != null && e2 != null && e1.Value == e2.Value) return true;
            return false;
        }

        public static bool operator !=(ReactiveBool e1, ReactiveBool e2)
        {
            if (ReferenceEquals(e1, e2)) { return false; }
            if (e1.Value != e2.Value) return true;
            return true;
        }

        private readonly TallyEventHandlerCaller<bool> mOnInitCaller = new TallyEventHandlerCaller<bool>();
        private readonly TallyEventHandlerCaller<bool> mOnUpdateCaller = new TallyEventHandlerCaller<bool>();

        public IReactiveStruct<bool> AddOnInitAction(Action<bool> onInit, int executeCount = -1)
        {
            mOnInitCaller.AddHandler(onInit, executeCount);

            return this;
        }

        public IReactiveStruct<bool> AddOnUpdateAction(Action<bool> onUpdate, int executeCount = -1)
        {
            mOnUpdateCaller.AddHandler(onUpdate);

            return this;
        }

        public bool Value { get; private set; }
        public IReactiveStruct<bool> Init(bool sourceValue)
        {
            Value = sourceValue;
            mOnInitCaller.CallEventHanlder(Value);

            return this;
        }



    }
}