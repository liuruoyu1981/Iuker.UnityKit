/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/07/25 10:10:39
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Iuker.Common.Utility;
using Iuker.UnityKit.Utility;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.Base.Config.Develop;
using Iuker.UnityKit.Run.Module.ReactiveDataModel;
using UnityEditor;
using UnityEngine;

namespace Iuker.UnityKit.Editor.ReactiveDataModel
{
    /// <summary>
    /// 响应式数据模型创建器
    /// </summary>
    public class ReactiveDataModelCreater
    {
        /// <summary>
        /// 初始化或更新子项目响应式数据模型脚本
        /// </summary>
        [MenuItem("Iuker/创建子项目响应式数据模型脚本")]
        private static void CreateSonProjectReactiveDataModelScripts()
        {
            FileUtility.TryCreateDirectory(RootConfig.GetCurrentSonProject().CsReactiveDataModelDir);
            ReactiveDataModelCreater creater = new ReactiveDataModelCreater();
            creater.GetBaseFrameDataModels();
            creater.mBaseFrameDataModelTypes.ForEach(Debug.Log);
            creater.CreateDataModelScript();
        }

        /// <summary>
        /// 基础框架中定义的响应式数据模型类型列表
        /// </summary>
        private List<Type> mBaseFrameDataModelTypes;

        /// <summary>
        /// 获得基础框架中定义的响应式数据模型类型
        /// </summary>
        private void GetBaseFrameDataModels()
        {
            var assemblyCSharp = Assembly.LoadFile(U3dConstants.AssemblyCSharpPath);
            var allReactiveDataModelTypes = ReflectionUitlity.GetTypeList<IReactiveDataModel>(assemblyCSharp, false, true);
            mBaseFrameDataModelTypes = allReactiveDataModelTypes
               .Where(t => t.Namespace != null && t.Namespace.StartsWith("Iuker.UnityKit.Run")).ToList();
        }

        private void CreateDataModelScript()
        {
            foreach (var modelType in mBaseFrameDataModelTypes)
            {
                CreateDataModelScript(modelType);
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 创建子项目继承框架所提供的基础响应式数据模型脚本
        /// </summary>
        /// <returns></returns>
        private void CreateDataModelScript(Type modelType)
        {
            var sonProject = RootConfig.GetCurrentSonProject();
            var sonProjectName = sonProject.ProjectName;
            var parentModelTypeName = modelType.Name;
            var className = string.Format("{0}_{1}", sonProjectName, parentModelTypeName);
            var scriptPath = sonProject.CsReactiveDataModelDir + className + ".cs";

            //  如果对应的子项目数据模型脚本不存在则创建
            if (!File.Exists(scriptPath))
            {
                StringBuilder sb = new StringBuilder();
                WriteFileInfo(sb, "数据模型脚本继承于框架提供的基础数据模型，可以在这里增加子项目所需要的数据字段");
                WriteNameSpace(sb);

                // 脚本主体内容
                sb.AppendLine(string.Format("namespace {0}", RootConfig.GetCurrentProject().ProjectName));
                sb.AppendLine("{");
                sb.AppendLine(string.Format("    public class {0} : {1}", className, modelType.Name));
                sb.AppendLine("    {");
                ScriptCreateUitlity.WriteFileAndSpace(sb, "        private IU3dFrame mU3DFrame;");

                ScriptCreateUitlity.WriteNote(sb, "数据模型初始化，可以在这里注册模型所关注的一些事件", null, null, "        ");
                ScriptCreateUitlity.WriteStandardMethod(sb, "        public override void Init(IU3dFrame frame)", "    mU3DFrame = frame;", "        ");
                ScriptCreateUitlity.WriteNote(sb, "初始化数据模型所关注的通信协议Id，在项目通信过程中收到对应的协议答复时则会自动回调数据模型的OnNetResponse方法",
                    null, null, "        ");
                ScriptCreateUitlity.WriteStandardMethod(sb, "        public override void InitConcernedProtoIdList()", null, "        ");
                ScriptCreateUitlity.WriteNote(sb, "该方法会在收到相关协议答复时自动调用，在这里做数模型字段的初始化及更新操作", null, null, "        ");
                ScriptCreateUitlity.WriteStandardMethod(sb, "        public override void OnNetResponse(object message)", null, "        ");
                sb.AppendLine("    }");
                sb.AppendLine("}");

                var scriptContent = sb.ToString();
                File.WriteAllText(scriptPath, scriptContent);
            }
            else
            {
                IukEditorUtility.DisplayDialog(string.Format("要创建的目标数据模型脚本{0}当前已存在，若确定要创建请先删除现有的然后重试！", className));
            }
        }

        private void WriteNameSpace(StringBuilder sb)
        {
            sb.AppendLine("using Iuker.UnityKit.Run.Base;");
            sb.AppendLine("using Iuker.UnityKit.Run.Module.ReactiveDataModel.BaseModel;");
            sb.AppendLine();
        }

        private static void WriteFileInfo(StringBuilder sb, string noteText = null)
        {
            sb.AppendLine("/***********************************************************************************************");
            sb.AppendLine(string.Format("Author：{0}", RootConfig.GetSonClientCoder().Name));
            sb.AppendLine("CreateDate: " + DateTime.Now);
            sb.AppendLine(string.Format("Email: {0}", RootConfig.GetSonClientCoder().Email));
            sb.AppendLine("***********************************************************************************************/");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("/*");
            sb.AppendLine("响应式数据模型脚本");
            sb.AppendLine(noteText ?? "该文件由工具自动生成，请勿做任何修改！！！！！！！！！");
            sb.AppendLine("*/");
            sb.AppendLine();
        }





    }
}