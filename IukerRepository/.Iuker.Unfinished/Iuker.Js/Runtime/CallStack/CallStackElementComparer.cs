using System.Collections.Generic;

namespace Iuker.Js.Runtime.CallStack
{
    public class CallStackElementComparer : IEqualityComparer<CallStackElement>
    {
        public bool Equals(CallStackElement x, CallStackElement y)
        {
            return y != null && x != null && x.Function == y.Function;
        }

        public int GetHashCode(CallStackElement obj)
        {
            return obj.Function.GetHashCode();
        }
    }
}