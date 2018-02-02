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
    public class ReactiveLong : IReactiveStruct<long>
    {
        public override string ToString()
        {
            return Value.ToString();
        }

        public int CompareTo(ReactiveLong other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(ReactiveLong other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ReactiveLong))
            {
                return false;
            }
            ReactiveLong other = (ReactiveLong)obj;
            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public void Assign(long newValue)
        {
            if (Value != newValue)
            {
                Value = newValue;
                mOnUpdateCaller.CallEventHanlder(Value);
            }
        }

        public void Close()
        {

        }

        public static bool operator ==(ReactiveLong e1, ReactiveLong e2)
        {
            if (ReferenceEquals(e1, e2)) { return true; }
            if (e1.Value == e2.Value) return true;
            return false;
        }

        public static bool operator !=(ReactiveLong e1, ReactiveLong e2)
        {
            if (ReferenceEquals(e1, e2)) { return false; }
            if (e1.Value != e2.Value) return true;
            return true;
        }

        public static bool operator <(ReactiveLong e1, ReactiveLong e2)
        {
            return e2 != null && (e1 != null && e1.Value < e2.Value);
        }

        public static bool operator <=(ReactiveLong e1, ReactiveLong e2)
        {
            return e2 != null && (e1 != null && e1.Value <= e2.Value);
        }

        public static bool operator >(ReactiveLong e1, ReactiveLong e2)
        {
            return e2 != null && (e1 != null && e1.Value > e2.Value);
        }

        public static bool operator >=(ReactiveLong e1, ReactiveLong e2)
        {
            return e2 != null && (e1 != null && e1.Value >= e2.Value);
        }

        private readonly TallyEventHandlerCaller<long> mOnInitCaller = new TallyEventHandlerCaller<long>();
        private readonly TallyEventHandlerCaller<long> mOnUpdateCaller = new TallyEventHandlerCaller<long>();

        public IReactiveStruct<long> AddOnInitAction(Action<long> onInit, int executeCount = -1)
        {
            mOnInitCaller.AddHandler(onInit, executeCount);

            return this;
        }

        public IReactiveStruct<long> AddOnUpdateAction(Action<long> onUpdate, int executeCount = -1)
        {
            mOnUpdateCaller.AddHandler(onUpdate);

            return this;
        }

        public long Value { get; private set; }
        public IReactiveStruct<long> Init(long sourceValue)
        {
            Value = sourceValue;
            mOnInitCaller.CallEventHanlder(Value);

            return this;
        }
    }
}