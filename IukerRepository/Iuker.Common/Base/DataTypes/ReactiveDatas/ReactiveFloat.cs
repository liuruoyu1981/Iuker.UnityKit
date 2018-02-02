/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/06/21 13:13
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
using System.Globalization;
using Iuker.Common.Event;

namespace Iuker.Common.DataTypes.ReactiveDatas
{
    /// <summary>
    /// 响应式float数据
    /// </summary>
    public class ReactiveFloat : IReactiveStruct<float>
    {
        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public int CompareTo(ReactiveFloat other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(ReactiveFloat other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ReactiveFloat))
            {
                return false;
            }
            ReactiveFloat other = (ReactiveFloat)obj;
            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public void Assign(float newValue)
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

        public static bool operator ==(ReactiveFloat e1, ReactiveFloat e2)
        {
            if (ReferenceEquals(e1, e2)) { return true; }
            if (e1.Value == e2.Value) return true;
            return false;
        }

        public static bool operator !=(ReactiveFloat e1, ReactiveFloat e2)
        {
            if (ReferenceEquals(e1, e2)) { return false; }
            if (e1.Value != e2.Value) return true;
            return true;
        }

        public static bool operator <(ReactiveFloat e1, ReactiveFloat e2)
        {
            return e2 != null && (e1 != null && e1.Value < e2.Value);
        }

        public static bool operator <=(ReactiveFloat e1, ReactiveFloat e2)
        {
            return e2 != null && (e1 != null && e1.Value <= e2.Value);
        }

        public static bool operator >(ReactiveFloat e1, ReactiveFloat e2)
        {
            return e2 != null && (e1 != null && e1.Value > e2.Value);
        }

        public static bool operator >=(ReactiveFloat e1, ReactiveFloat e2)
        {
            return e2 != null && (e1 != null && e1.Value >= e2.Value);
        }

        private readonly TallyEventHandlerCaller<float> mOnInitCaller = new TallyEventHandlerCaller<float>();
        private readonly TallyEventHandlerCaller<float> mOnUpdateCaller = new TallyEventHandlerCaller<float>();

        public IReactiveStruct<float> AddOnInitAction(Action<float> onInit, int executeCount = -1)
        {
            mOnInitCaller.AddHandler(onInit, executeCount);

            return this;
        }

        public IReactiveStruct<float> AddOnUpdateAction(Action<float> onUpdate, int executeCount = -1)
        {
            mOnUpdateCaller.AddHandler(onUpdate);

            return this;
        }

        public float Value { get; private set; }
        public IReactiveStruct<float> Init(float sourceValue)
        {
            Value = sourceValue;
            mOnInitCaller.CallEventHanlder(Value);

            return this;
        }
    }
}