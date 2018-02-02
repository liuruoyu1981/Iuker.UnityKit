/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/14 10:57:57
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


namespace Iuker.MoonSharp.Interpreter.Execution.VM
{
    /// <summary>
    /// 操作码
    /// </summary>
    internal enum OpCode
    {
        // Meta-opcodes
        Nop,        // 不执行任何操作。
        Debug,      // 不执行任何操作。用来帮助调试。

        // 堆栈操作和赋值
        Pop,        // 从v-stack丢弃最上面的n个元素。
        Copy,       // 复制n-th位置的值到栈顶
        Swap,       // 交换两个元素在栈上的位置
        Literal,    // 将字面量（常量值）压入栈中
        Closure,    // 在栈顶创建一个闭包, using the symbols for upvalues and num-val for entry point of the function.
        NewTable,   // 在栈上创建一个空表
        TblInitN,   // 初始化一个命名表
        TblInitI,   // 初始化表位置条目Initializes a table positional entry

        StoreLcl, Local,
        StoreUpv, Upvalue,
        IndexSet, Index,
        IndexSetN, IndexN,
        IndexSetL, IndexL,

        // U3D框架中栈操作
        Clean,      // Cleansup locals setting them as null

        Meta,   // 注入函数元数据用于反射相关 (转储, 调试)
        BeginFn,    // Adjusts for start of function, taking in parameters and allocating locals
        Args,       // Takes the arguments passed to a function and sets the appropriate symbols in the local scope
        Call,       // Calls the function specified on the specified element from the top of the v-stack. If the function is a MoonSharp function, it pushes its numeric value on the v-stack, then pushes the current PC onto the x-stack, enters the function closure and jumps to the function first instruction. If the function is a CLR function, it pops the function value from the v-stack, then invokes the function synchronously and finally pushes the result on the v-stack.
        ThisCall,   // Same as call, but the call is a ':' method invocation
        Ret,        // Pops the top n values of the v-stack. Then pops an X value from the v-stack. Then pops X values from the v-stack. Afterwards, it pushes the top n values popped in the first step, pops the top of the x-stack and jumps to that location.

        // Jumps
        Jump,       // Jumps to the specified PC
        Jf,         // Pops the top of the v-stack and jumps to the specified location if it's false
        JNil,       // Jumps if the top of the stack is nil
        JFor,       // Peeks at the top, top-1 and top-2 values of the v-stack which it assumes to be numbers. Then if top-1 is less than zero, checks if top is <= top-2, otherwise it checks that top is >= top-2. Then if the condition is false, it jumps.
        JtOrPop,    // Peeks at the topmost value of the v-stack as a boolean. If true, it performs a jump, otherwise it removes the topmost value from the v-stack.
        JfOrPop,    // Peeks at the topmost value of the v-stack as a boolean. If false, it performs a jump, otherwise it removes the topmost value from the v-stack.

        // Operators
        Concat,     // Concatenation of the two topmost operands on the v-stack
        LessEq,     // Compare <= of the two topmost operands on the v-stack
        Less,       // Compare < of the two topmost operands on the v-stack
        Eq,         // Compare == of the two topmost operands on the v-stack
        Add,        // Addition of the two topmost operands on the v-stack
        Sub,        // Subtraction of the two topmost operands on the v-stack
        Mul,        // Multiplication of the two topmost operands on the v-stack
        Div,        // Division of the two topmost operands on the v-stack
        Mod,        // Modulus of the two topmost operands on the v-stack
        Not,        // Logical inversion of the topmost operand on the v-stack
        Len,        // Size operator of the topmost operand on the v-stack
        Neg,        // Negation (unary minus) operator of the topmost operand on the v-stack
        Power,      // Power of the two topmost operands on the v-stack
        CNot,       // Conditional NOT - takes second operand from the v-stack (must be bool), if true execs a NOT otherwise execs a TOBOOL


        // Type conversions and manipulations
        MkTuple,    // Creates a tuple from the topmost n values
        Scalar,     // Converts the topmost tuple to a scalar
        Incr,       // Performs an add operation, without extracting the operands from the v-stack and assuming the operands are numbers.
        ToNum,      // Converts the top of the stack to a number
        ToBool,     // Converts the top of the stack to a boolean
        ExpTuple,   // Expands a tuple on the stack


        // Iterators
        IterPrep,   // Prepares an iterator for execution 
        IterUpd,    // Updates the var part of an iterator

        // Meta
        Invalid,    // Crashes the executor with an unrecoverable NotImplementedException. This MUST always be the last opcode in enum
    }
}
