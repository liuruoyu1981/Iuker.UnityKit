using System;
using System.Collections.Generic;

namespace Iuker.YourSharp
{
    class SimpleVm
    {
        //private readonly List<dynamic> dataStack = new List<dynamic>();
        private readonly Dictionary<OpCode, Action> dispatchTable;
    }





    public enum OpCode
    {
        Add,
        Sub,
        Mul,
        Div,
        Print,
        Jmp,
        If,
        ReadLine,
        Return,
        Call,
        Exit
    }
}
