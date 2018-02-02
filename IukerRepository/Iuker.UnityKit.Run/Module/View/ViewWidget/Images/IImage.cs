/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/4/8 下午3:44:58
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

using Iuker.UnityKit.Run.Module.View.MVDA;
using UnityEngine;


namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
    public interface IImage : IukViewWidget, IViewElement
    {
        string ImageName { get; set; }

        Color Color { get; set; }

        /// <summary>
        /// 将图片设置为原生尺寸
        /// </summary>
        void ToNativeSize();

        /// <summary>
        /// 修改图片内部的精灵图片资源
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        IImage SetImage(string imageName);

    }
}
