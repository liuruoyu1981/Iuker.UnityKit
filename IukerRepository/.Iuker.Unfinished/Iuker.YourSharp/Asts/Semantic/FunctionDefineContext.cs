/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/05/21 00:41
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
using System.Collections.Generic;
using Iuker.YourSharp.Asts.SyntaxNodes;
using Iuker.YourSharp.Asts.SyntaxNotes;
using Iuker.YourSharp.Parser;

namespace Iuker.YourSharp.Asts.Semantic
{
    /// <summary>
    /// 函数定义上下文
    /// </summary>
    public class FunctionDefineContext : ISemanticContext
    {
        /// <summary>
        /// 函数返回类型节点
        /// </summary>
        private ReturnTypeNode mReturnTypeNode;

        /// <summary>
        /// 函数名定义节点
        /// </summary>
        private FunctionNameNode mFunctionNameNode;

        //private List<ar>

        /// <summary>
        /// 访问权限节点
        /// </summary>
        private AccessDefineNode mAccessDefineNode;

        /// <summary>
        /// 当前定义的函数是否处于全局作用域,默认为真。
        /// </summary>
        public bool IsInGlobal { get; private set; } = true;

        /// <summary>
        /// 函数的类级别作用域即声明该函数的完整命名空间
        /// </summary>
        public string ClassScope { get; private set; } = null;

        /// <summary>
        /// 设置函数定义上下文的函数作用域
        /// </summary>
        public void SetScope(string classScope)
        {
            ClassScope = classScope;
            IsInGlobal = false;
        }

        public ISemanticContext Insert(SyntaxNode syntaxNode)
        {
            switch (syntaxNode.NodeType)
            {
                case SyntaxNodeType.ReturnType:
                    mReturnTypeNode = syntaxNode.As<ReturnTypeNode>();
                    break;
                case SyntaxNodeType.FunctionName:
                    mFunctionNameNode = syntaxNode.As<FunctionNameNode>();
                    break;

            }


            return this;
        }

        /// <summary>
        /// 函数参数
        /// </summary>
        //public Dictionary<string, ArgumentNode> ArgumentNodes { get; } = new Dictionary<string, ArgumentNode>();
        public readonly ArgumentsContext ArgumentNodes = new ArgumentsContext();

        /// <summary>
        /// 临时参数节点
        /// </summary>
        public ArgumentNode TempArgumentNode;

        /// <summary>
        /// 设置临时参数节点的名字
        /// </summary>
        /// <param name="token"></param>
        public void SetTempArgumentName(Token token) => TempArgumentNode.SetArgumentName(token);

        /// <summary>
        /// 设置临时参数节点的默认值
        /// </summary>
        /// <param name="token"></param>
        public void SetTempArgumentDefaultValue(Token token) => TempArgumentNode.SetArgumentDefaultValue(token);

        /// <summary>
        /// 将临时参数节点插入参数节点字典
        /// </summary>
        public void InsertTempArgument()
        {
            if (ArgumentNodes.ContainsKey(TempArgumentNode.NodeName))
            {
                throw new ArgumentException(nameof(TempArgumentNode));
            }

            ArgumentNodes.Add(TempArgumentNode.NodeName, TempArgumentNode);
            TempArgumentNode = null;
        }

        /// <summary>
        /// 函数语义对象的唯一Id
        /// 由所在类命名空间（假如是一个类函数），函数名，函数参数列表可读字符串组成。
        /// 例如：YourSharp.Examples->MyFunction=>(arg1_int_arg2_double)
        /// </summary>
        public string SingleToken
        {
            get
            {
                if (IsInGlobal)
                {
                    return mFunctionNameNode.FunctionName;
                }
                return ClassScope + mFunctionNameNode.FunctionName + ArgumentNodes.GetArgumentsToken();
            }
        }


        #region 函数体

        /// <summary>
        /// 函数体内字段字典
        /// </summary>
        public Dictionary<string, SyntaxNode> FunctionFields { get; } = new Dictionary<string, SyntaxNode>();

        public void InsertFunctionFiled(SyntaxNode syntaxNode)
        {
            if (!FunctionFields.ContainsKey(syntaxNode.NodeName))
            {
                FunctionFields.Add(syntaxNode.NodeName, syntaxNode);
            }


        }



        #endregion








    }
}
