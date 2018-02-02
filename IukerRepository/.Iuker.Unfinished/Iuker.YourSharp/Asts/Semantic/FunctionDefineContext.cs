/***********************************************************************************************
Author��liuruoyu1981
CreateDate: 2017/05/21 00:41
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************�޸���־***********************************************
1. �޸����ڣ� �޸��ˣ� �޸����ݣ�
2. �޸����ڣ� �޸��ˣ� �޸����ݣ�
3. �޸����ڣ� �޸��ˣ� �޸����ݣ�
4. �޸����ڣ� �޸��ˣ� �޸����ݣ�
5. �޸����ڣ� �޸��ˣ� �޸����ݣ�
****************************************�޸���־***********************************************/


using System;
using System.Collections.Generic;
using Iuker.YourSharp.Asts.SyntaxNodes;
using Iuker.YourSharp.Asts.SyntaxNotes;
using Iuker.YourSharp.Parser;

namespace Iuker.YourSharp.Asts.Semantic
{
    /// <summary>
    /// ��������������
    /// </summary>
    public class FunctionDefineContext : ISemanticContext
    {
        /// <summary>
        /// �����������ͽڵ�
        /// </summary>
        private ReturnTypeNode mReturnTypeNode;

        /// <summary>
        /// ����������ڵ�
        /// </summary>
        private FunctionNameNode mFunctionNameNode;

        //private List<ar>

        /// <summary>
        /// ����Ȩ�޽ڵ�
        /// </summary>
        private AccessDefineNode mAccessDefineNode;

        /// <summary>
        /// ��ǰ����ĺ����Ƿ���ȫ��������,Ĭ��Ϊ�档
        /// </summary>
        public bool IsInGlobal { get; private set; } = true;

        /// <summary>
        /// �������༶�������������ú��������������ռ�
        /// </summary>
        public string ClassScope { get; private set; } = null;

        /// <summary>
        /// ���ú������������ĵĺ���������
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
        /// ��������
        /// </summary>
        //public Dictionary<string, ArgumentNode> ArgumentNodes { get; } = new Dictionary<string, ArgumentNode>();
        public readonly ArgumentsContext ArgumentNodes = new ArgumentsContext();

        /// <summary>
        /// ��ʱ�����ڵ�
        /// </summary>
        public ArgumentNode TempArgumentNode;

        /// <summary>
        /// ������ʱ�����ڵ������
        /// </summary>
        /// <param name="token"></param>
        public void SetTempArgumentName(Token token) => TempArgumentNode.SetArgumentName(token);

        /// <summary>
        /// ������ʱ�����ڵ��Ĭ��ֵ
        /// </summary>
        /// <param name="token"></param>
        public void SetTempArgumentDefaultValue(Token token) => TempArgumentNode.SetArgumentDefaultValue(token);

        /// <summary>
        /// ����ʱ�����ڵ��������ڵ��ֵ�
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
        /// ������������ΨһId
        /// �������������ռ䣨������һ���ຯ�����������������������б�ɶ��ַ�����ɡ�
        /// ���磺YourSharp.Examples->MyFunction=>(arg1_int_arg2_double)
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


        #region ������

        /// <summary>
        /// ���������ֶ��ֵ�
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
