﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Iuker.Js.Runtime.CallStack
{
    public class JintCallStack : IEnumerable<CallStackElement>
    {
        private readonly Stack<CallStackElement> _stack = new Stack<CallStackElement>();
        private readonly Dictionary<CallStackElement, int> _statistics = new Dictionary<CallStackElement, int>(new CallStackElementComparer());

        public int Push(CallStackElement item)
        {
            _stack.Push(item);
            if (_statistics.ContainsKey(item))
            {
                return ++_statistics[item];
            }

            _statistics.Add(item, 0);
            return 0;
        }

        public void Pop()
        {
            var item = _stack.Pop();
            if (_statistics[item] == 0)
            {
                _statistics.Remove(item);
            }
            else
            {
                _statistics[item]--;
            }

            //return item;
        }

        public void Clear()
        {
            _stack.Clear();
            _statistics.Clear();
        }

        public IEnumerator<CallStackElement> GetEnumerator()
        {
            return _stack.GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join("->", _stack.Select(cse => cse.ToString()).Reverse().ToArray());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}