/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/15 10:05:26
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



namespace Iuker.MoonSharp.Tree
{
    public enum TokenType
    {
        Eof,
        HashBang,
        Name,
        And,
        Break,
        Do,
        Else,
        ElseIf,
        End,
        False,
        For,
        Function,
        Lambda,
        Goto,
        If,
        In,
        Local,
        Nil,
        Not,
        Or,
        Repeat,
        Return,
        Then,
        True,
        Until,
        While,
        Op_Equal,
        Op_Assignment,
        Op_LessThan,
        Op_LessThanEqual,
        Op_GreaterThanEqual,
        Op_GreaterThan,
        Op_NotEqual,
        Op_Concat,
        VarArgs,
        Dot,
        Colon,
        DoubleColon,
        Comma,
        Brk_Close_Curly,
        Brk_Open_Curly,
        Brk_Close_Round,
        Brk_Open_Round,
        Brk_Close_Square,
        Brk_Open_Square,
        Op_Len,
        Op_Pwr,
        Op_Mod,
        Op_Div,
        Op_Mul,
        Op_MinusOrSub,
        Op_Add,
        Comment,

        String,
        String_Long,

        Number,
        Number_HexFloat,
        Number_Hex,
        SemiColon,
        Invalid,

        Brk_Open_Curly_Shared,
        Op_Dollar,
    }

}
