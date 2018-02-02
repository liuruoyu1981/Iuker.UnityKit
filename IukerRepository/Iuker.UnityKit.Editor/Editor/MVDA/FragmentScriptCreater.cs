using System;
using System.Collections.Generic;
using Iuker.Common.Base;
using Iuker.Common.Utility;
using Iuker.UnityKit.Run.Base;
using Iuker.UnityKit.Run.LinqExtensions;
using Iuker.UnityKit.Run.Base.Config.Develop;
using Iuker.UnityKit.Run.Module.View.ViewWidget;
using UnityEngine;

namespace Iuker.UnityKit.Editor.MVDA
{
#if DEBUG
    /// <summary>
    /// 视图碎片脚本创建器
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170913 12:06:54")]
    [ClassPurposeDesc("视图碎片脚本创建器", "视图碎片脚本创建器")]
#endif
    public class FragmentScriptCreater : ViewScriptCreaterBase
    {
        public static void FragmenntTest()
        {
            var fragmentScriptCreater = new FragmentScriptCreater();
            fragmentScriptCreater.CreateBodyScript(U3dConstants.UnityRootDir + "Fragment.cs");
        }

        private CsharpScriptAppender mAppender;

        private readonly string[] mNameSpaces = {
            "using Iuker.UnityKit.Run.Module.View.MVDA;"
        };

        private void CreateBodyScript(string targetPath)
        {
            using (mAppender = new CsharpScriptAppender(EditorConstant.HostClientName, EditorConstant.HostClientEmail, _selectedGameObject.name, mNameSpaces, "AbsFragment", RootConfig.CrtProjectName))
            {
                mAppender.WriteMethod("protected override void InitViewWidgets()", GetAllTypeWidgetGetCode);
            }
            FileUtility.WriteAllText(targetPath, mAppender.CodeContent);
        }

        private void GetAllTypeWidgetGetCode(bool isInNs)
        {
            GetViewWidgetCode("容器对象", typeof(GameObject).Name, _containerList, "ContainerDictionary");
            GetViewWidgetCode("按钮对象", typeof(IButton).Name, _buttonList, "ButtonDictionary");
            GetViewWidgetCode("文本对象", typeof(IText).Name, _textList, "TextDictionary");
            GetViewWidgetCode("输入框对象", typeof(IInputField).Name, _inputFieldList, "InputFieldDictionary");
            GetViewWidgetCode("Image对象", typeof(IImage).Name, _imageList, "ImageDictionary");
            GetViewWidgetCode("RawImage对象", typeof(IRawImage).Name, _rawImageList, "RawImageDictionary");
            GetViewWidgetCode("Toggle对象", typeof(IToggle).Name, _toggleList, "ToggleDictionary");
            GetViewWidgetCode("Slider对象", typeof(ISlider).Name, _sliderList, "SliderDictionary");
            GetViewWidgetCode("TabGroup对象", typeof(ITabGroup).Name, _tabgroupList, "TabGroupDictionary");
        }

        /// <summary>
        /// 获得指定类型视图控件的获取代码字符串
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="node"></param>
        /// <param name="interfaceType"></param>
        /// <param name="elementList"></param>
        /// <param name="dictionaryName"></param>
        /// <param name="isInNameSpace"></param>
        private string GetViewWidgetCode(string node, string interfaceType, List<string> elementList, string dictionaryName, bool isInNameSpace = true)
        {
            mAppender.AppendLine(isInNameSpace ? string.Format("            //   {0}", node) : string.Format(
                "        //  {0}", node));
            elementList.ForEach(r =>
            {
                var targetObj = ViewWidgetsDictionary[r];
                var getPath = GetWidgetPath(targetObj, targetObj.Parent(), "");
                _viewWidgetGetPathList.Add(new WidgetPathInfo(targetObj.name, getPath));
                mAppender.AppendLineInMethod(
                    interfaceType != "GameObject"
                        ? string.Format("InitViewWidget<{0}>({1}).AddTo({1},{2});", interfaceType, getPath,
                            dictionaryName)
                        : string.Format("InitViewWidget({0});", getPath), isInNameSpace);
            });
            mAppender.AppendLine();
            var codBlock = mAppender.ToString();
            return codBlock;
        }

        private static string GetWidgetPath(GameObject source, GameObject parent, string path)
        {
            while (true)
            {
                try
                {
                    var parentName = parent.name;
                    var grandfatherName = parent.Parent().name;
                    if (!parentName.StartsWith("view") && !parentName.StartsWith("fragment"))
                    {
                        path = parentName + "/" + path;
                        parent = parent.Parent();
                        continue;
                    }
                    if (!ViewRootHashSet.Contains(grandfatherName))
                    {
                        path = parentName + "/" + path;
                        parent = parent.Parent();
                        continue;
                    }
                    var result = "\"" + path + source.name + "\"";
                    return result;
                }
                catch (Exception e)
                {
                    Debug.Log(string.Format("在获取源控件{0}的路径时发生了异常，异常信息为{1}!", source.name, e.Message));
                    throw;
                }

            }
        }



    }
}
