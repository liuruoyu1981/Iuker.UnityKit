/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/14 19:27:52
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
using UnityEngine;

namespace Iuker.UnityKit.Editor.Setting
{
    /// <summary>
    /// 首选项设置基类
    /// </summary>
    [Serializable]
    public class IukerSettingBase
    {
        protected string Key { get; private set; }

        protected IukerSettingBase(string key)
        {
            Key = key;
        }

        public string ToJson
        {
            get
            {
                return JsonUtility.ToJson(this);
            }
        }
    }
}