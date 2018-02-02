/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/20/2017 10:09
Email: liuruoyu1981@gmail.com
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
using UnityEngine;

namespace Iuker.UnityKit
{
    /// <summary>
    /// 垂直布局
    /// 利用实现了IDisposable接口的布局类来自动实现Unity3D布局代码成对闭合
    /// </summary>
    public class IukVerticalLayout : IDisposable
    {
        public IukVerticalLayout(params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
        }

        public void Dispose()
        {
            GUILayout.EndVertical();
        }
    }




}