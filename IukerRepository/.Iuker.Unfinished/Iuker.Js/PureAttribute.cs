/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/19 10:19
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
using System.Diagnostics;

namespace Iuker.Js
{
    //
    // 摘要:
    //     指示一个类型或方法为纯类型或纯方法，即它不进行任何可视的状态更改。
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.Delegate, AllowMultiple = false, Inherited = true)]
    [Conditional("CONTRACTS_FULL")]
    public sealed class PureAttribute : Attribute
    {
        //
        // 摘要:
        //     初始化 System.Diagnostics.Contracts.PureAttribute 类的新实例。
        public PureAttribute()
        {

        }
    }
}
