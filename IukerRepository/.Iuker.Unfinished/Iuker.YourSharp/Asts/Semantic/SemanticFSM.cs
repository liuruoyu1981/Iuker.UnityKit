/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/20 16:10
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

namespace YourSharp.Asts.Semantic
{
    /// <summary>
    /// 语法分析情境状态机
    /// </summary>
    public class SemanticFSM
    {
        public SyntaxAnalyzeStatus SyntaxAnalyzeStatus { get; private set; } = SyntaxAnalyzeStatus.Default;


        /// <summary>
        /// 转换当前语法分析机的状态
        /// </summary>
        /// <param name="lasStatus">之前的状态</param>
        public void TransitionStatus(SyntaxAnalyzeStatus lasStatus)
        {
            switch (lasStatus)
            {
                case SyntaxAnalyzeStatus.Default:
                    break;
                case SyntaxAnalyzeStatus.FunctionDefine:
                    break;
                case SyntaxAnalyzeStatus.FunctionBodyDefine:
                    break;
                case SyntaxAnalyzeStatus.FunctionDefineOver:
                    break;
                case SyntaxAnalyzeStatus.GlobalFiledDefine:
                    break;
                case SyntaxAnalyzeStatus.GlobalFunctionHeadDefine:
                    break;
                case SyntaxAnalyzeStatus.GlobalFunction_BodyDefine:
                    break;
                case SyntaxAnalyzeStatus.ClassFunctionHeadDefine:
                    break;
                case SyntaxAnalyzeStatus.ClassFunctionBodyDefine:
                    break;
                case SyntaxAnalyzeStatus.InstanceFunction_HeadDefine:
                    break;
                case SyntaxAnalyzeStatus.InstanceFunction_BodyDefine:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lasStatus), lasStatus, null);
            }

        }


    }
}
