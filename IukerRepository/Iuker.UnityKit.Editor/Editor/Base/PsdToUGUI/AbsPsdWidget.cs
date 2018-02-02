/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/16 15:17
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

using System.Collections.Generic;

namespace Iuker.UnityKit.Editor.PsdToUGUI
{
    /// <summary>
    /// Psd源文件节点基类
    /// </summary>
    public class AbsPsdWidget
    {
        public PsdNodeType PsdNodeType { get; private set; }

        public string Name { get; private set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public List<AbsPsdWidget> SonPsdNodes { get; private set; }

        public AbsPsdWidget Parent { get; private set; }

        public string Value { get; private set; }

    }
}