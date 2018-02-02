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
using System.Collections.Generic;
using System.Linq;
using Iuker.YourSharp.Asts.Semantic;
using Iuker.YourSharp.Asts.SyntaxCheckers;
using Iuker.YourSharp.Common.Utils;
using Iuker.YourSharp.ErrorProcess;
using Iuker.YourSharp.Parser;

namespace Iuker.YourSharp.Asts
{
    /// <summary>
    /// 语法分析器
    /// </summary>
    public class SyntaxAnalyzer
    {
        /// <summary>
        /// 语法情境检查器字典。
        /// </summary>
        private readonly Dictionary<string, ISyntaxChecker> s_CheckerDicntionary = new Dictionary<string, ISyntaxChecker>();

        /// <summary>
        /// 执行引擎
        /// </summary>
        private YourSharpEngine mSharpEngine;

        /// <summary>
        /// 当前命名空间
        /// </summary>
        public string CurrentNameSpace;

        /// <summary>
        /// 词法分析器
        /// </summary>
        private Lexer mLexer;

        /// <summary>
        /// 正在构建语法树的单词集合
        /// </summary>
        public TokenSet BuildingSet { get; private set; }

        /// <summary>
        /// 当前检查的行单词流
        /// </summary>
        public List<Token> CurrentTokens { get; private set; }

        /// <summary>
        /// 当前检查的单词
        /// </summary>
        private Token mCheckingToken;

        /// <summary>
        /// 语法检查结果
        /// </summary>
        private SyntaxAnalyzerResult mCheckResult;

        /// <summary>
        /// 语法分析器当前所处的分析状态
        /// </summary>
        public SyntaxAnalyzeType SyntaxAnalyzeType = SyntaxAnalyzeType.None;

        /// <summary>
        /// 当前分析的源代码
        /// </summary>
        public SourceCode CurrentCode { get; private set; }

        /// <summary>
        /// 当前分析是否处于全局作用域
        /// </summary>
        public bool IsInGlobal => CurrentCode.IsInGlobal;

        /// <summary>
        /// 当前分析的类文件的完整作用域
        /// </summary>
        public string CurrentClassScope { get; private set; }

        public SyntaxAnalyzer Init(YourSharpEngine sharpEngine)
        {
            mSharpEngine = sharpEngine;
            mLexer = sharpEngine.Lexer;

            var checkers = ReflectionUtil.GetTypeList<ISyntaxChecker>();

            foreach (var checker in checkers)
            {
                var instance = Activator.CreateInstance(checker) as ISyntaxChecker;
                if (instance == null)
                {
                    throw new ArgumentNullException(nameof(instance));
                }

                s_CheckerDicntionary.Add(instance.ProcessedSituation, instance);
            }

            return this;
        }

        /// <summary>
        /// 分析单词集合并构建语法树
        /// </summary>
        public void Analyze(SourceCode sourceCode)
        {
            CurrentCode = sourceCode;
            BuildingSet = mLexer.GetTokenSet(sourceCode.Name);

            CurrentTokens = BuildingSet.CurrentTokens;
            mCheckingToken = CurrentTokens.First();

            SyntaxCheck();
        }

        private void SyntaxCheck()
        {
            // 检测当前的分析状态
            if (SyntaxAnalyzeType == SyntaxAnalyzeType.ClassHeadDefine) //  类头部定义
            {
                mCheckResult = CheckOnClassHeadDefine_InGlobal();
            }
            else
            {
                switch (mCheckingToken.TokenType)
                {
                    case TokenType.Public:
                    case TokenType.Private:
                    case TokenType.Protected:
                    case TokenType.Internal:
                        mCheckResult = CallChekcer(SyntaxSituation.AccessDefine);
                        break;
                    case TokenType.NameSpace:
                        mCheckResult = CallChekcer(SyntaxSituation.NameSpace);
                        break;
                    case TokenType.Import:
                        mCheckResult = CallChekcer(SyntaxSituation.OnImport);
                        break;

                    default:
                        if (IsReturnType(mCheckingToken))
                        {
                            switch (SyntaxAnalyzeType)
                            {
                                case SyntaxAnalyzeType.FunctionDefine:
                                //return SyntaxAnalyzerResult.Failed;
                                case SyntaxAnalyzeType.FunctionBodyDefine:
                                //return InFunctionBody(current, analyzer);
                                case SyntaxAnalyzeType.ClassBodyDefine:
                                    mCheckResult = CallChekcer(SyntaxSituation.ClassBody_ReturnType);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        break;
                }
            }

            switch (mCheckResult)
            {
                case SyntaxAnalyzerResult.Completed:
                    NextLine();
                    break;
                case SyntaxAnalyzerResult.Failed:
                    throw new SyntaxException(this, mCheckingToken);
            }
        }

        /// <summary>
        /// 调用指定的语法检查器
        /// 这个方法会前移一个单词然后调用对应的语法检查器
        /// </summary>
        /// <param name="situation"></param>
        /// <returns></returns>
        public SyntaxAnalyzerResult CallChekcer(string situation)
        {
            var tempToken = mCheckingToken;
            mCheckingToken = null;
            mCheckingToken = tempToken.NexToken;

            if (s_CheckerDicntionary.ContainsKey(situation))
            {
                var checker = s_CheckerDicntionary[situation];
                return checker.IsOK(mCheckingToken, this);
            }

            return SyntaxAnalyzerResult.CheckerNotExist;
        }

        /// <summary>
        /// 类头部定义=>处于全局作用域
        /// </summary>
        private SyntaxAnalyzerResult CheckOnClassHeadDefine_InGlobal()
        {
            switch (mCheckingToken.TokenType)
            {
                case TokenType.Public:
                case TokenType.Internal:
                    return CallChekcer(SyntaxSituation.ClassHead_Access);
                case TokenType.Class:
                    return CallChekcer(SyntaxSituation.ClassHead_Class);
                default:

                    return SyntaxAnalyzerResult.Failed;
            }
        }


        /// <summary>
        /// 将当前单词流的位置前移指定位置
        /// </summary>
        //public void MoveTo(int count)
        //{
        //    var targetTokenIndex = mCheckingToken.TokenIndex + count;
        //    mCheckingToken = CurrentTokens[targetTokenIndex];
        //}

        /// <summary>
        /// 开始扫描下一行单词
        /// </summary>
        private void NextLine()
        {
            CurrentTokens = BuildingSet.GetTokens();
            if (CurrentTokens != null)
            {
                mCheckingToken = CurrentTokens.First();
                SyntaxCheck();
            }
            else
            {
                Console.Write("语法分析结束");
            }
        }



        #region 语义分析上下文

        /// <summary>
        /// 当前使用的函数语义分析上下文
        /// </summary>
        public FunctionDefineContext CurrentFunctionContext { get; private set; }

        /// <summary>
        /// 当前的类定义上下文
        /// </summary>
        public ClassDefineContext CurrentClassContext { get; private set; }

        /// <summary>
        /// 临时函数语义分析上下文
        /// </summary>
        public FunctionDefineContext TempFunctionDefineContext;

        /// <summary>
        /// 插入一个函数语义分析上下文
        /// 内部将判定该上下文的类型并插入不同的上下文字典
        /// </summary>
        public void InsertFunctionContext(FunctionDefineContext context)
        {
            switch (context.IsInGlobal)
            {
                case true:
                    InsertGlobalFunctionContext(context);
                    break;
                default:
                    if (context.ClassScope != null) //  类函数
                    {
                        InsertClassFunctionContext(context);
                    }
                    else
                    {
                        InsertInstanceFunctionContext(context); //  实例函数
                    }
                    break;
            }
        }

        public GlobalDefineContext GlobalDefineContext { get; private set; } = new GlobalDefineContext();

        public void InsertClassContext(ClassDefineContext context)
        {
            if (GlobalDefineContext.GlobalClassDefineContexts.ContainsKey(context.SingleToken))
            {
                throw new ArgumentException(nameof(context));
            }

            GlobalDefineContext.GlobalClassDefineContexts.Add(context.SingleToken, context);
            CurrentClassContext = context;
        }

        /// <summary>
        /// 插入一个全局函数语义分析上下文
        /// </summary>
        public void InsertGlobalFunctionContext(FunctionDefineContext context)
        {
            if (GlobalDefineContext.GlobalFunctionContexts.ContainsKey(context.SingleToken))    //  已定义
            {
                throw new ArgumentException(nameof(context));
            }

            GlobalDefineContext.GlobalFunctionContexts.Add(context.SingleToken, context);
            CurrentFunctionContext = context;
        }

        /// <summary>
        /// 插入一个类函数语义分析上下文
        /// </summary>
        public void InsertClassFunctionContext(FunctionDefineContext context)
        {

        }

        /// <summary>
        /// 插入一个类实例函数语义分析上下文
        /// </summary>
        public void InsertInstanceFunctionContext(FunctionDefineContext context)
        {

        }

        /// <summary>
        /// 单词字面量是否是合法的返回类型
        /// </summary>
        public bool IsReturnType(Token token) => GlobalDefineContext.IsReturnType(token.StringLiterals);

        /// <summary>
        /// 单词是否是字段或参数声明的合法类型。
        /// </summary>
        public bool IsFiledOrArgumentType(Token token) => GlobalDefineContext.IsFiledType(token.StringLiterals);

        /// <summary>
        /// 单词是否为合法类名
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool IsLegalClassName(Token token)
        {
            if (char.IsLower(token.StringLiterals.First())) //  类名必须以大写开头
            {
                return false;
            }

            var fullName = CurrentNameSpace + "." + token.StringLiterals;
            return GlobalDefineContext.IsLegalClassName(fullName);
        }

        #endregion





    }
}
