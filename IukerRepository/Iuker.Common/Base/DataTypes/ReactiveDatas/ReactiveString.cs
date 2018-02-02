using System;
using Iuker.Common.Event;

namespace Iuker.Common.DataTypes.ReactiveDatas
{
    public class ReactiveString : IReactiveStruct<string>
    {
        public override string ToString()
        {
            return Value.ToString();
        }

        public int CompareTo(ReactiveString other)
        {
            return String.Compare(Value, other.Value, StringComparison.Ordinal);
        }

        public bool Equals(ReactiveString other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ReactiveString))
            {
                return false;
            }
            ReactiveString other = (ReactiveString)obj;
            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public void Assign(string newValue)
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

        public static bool operator ==(ReactiveString e1, ReactiveString e2)
        {
            if (ReferenceEquals(e1, e2)) { return true; }
            if (e1.Value == e2.Value) return true;
            return false;
        }

        public static bool operator !=(ReactiveString e1, ReactiveString e2)
        {
            if (ReferenceEquals(e1, e2)) { return false; }
            if (e1.Value != e2.Value) return true;
            return true;
        }

        private readonly TallyEventHandlerCaller<string> mOnInitCaller = new TallyEventHandlerCaller<string>();
        private readonly TallyEventHandlerCaller<string> mOnUpdateCaller = new TallyEventHandlerCaller<string>();

        public IReactiveStruct<string> AddOnInitAction(Action<string> onInit, int executeCount = -1)
        {
            mOnInitCaller.AddHandler(onInit, executeCount);

            return this;
        }

        public IReactiveStruct<string> AddOnUpdateAction(Action<string> onUpdate, int executeCount = -1)
        {
            mOnUpdateCaller.AddHandler(onUpdate);

            return this;
        }

        public string Value { get; private set; }
        public IReactiveStruct<string> Init(string sourceValue)
        {
            Value = sourceValue;
            mOnInitCaller.CallEventHanlder(Value);

            return this;
        }

        private Action<string> mUpdateAction;
        public void Bind(Action<string> updateAction)
        {
            mUpdateAction = updateAction;
        }
    }
}