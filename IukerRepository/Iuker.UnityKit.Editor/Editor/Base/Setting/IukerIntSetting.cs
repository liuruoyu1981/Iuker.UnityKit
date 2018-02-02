/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/14 19:40:13
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
using UnityEditor;

namespace Iuker.UnityKit.Editor.Setting
{
    /// <summary>
    /// 整型值设置
    /// </summary>
    [Serializable]
    public class IukerIntSetting : IukerSettingBase
    {
        private int mValue;

        public int Value
        {
            get { return mValue; }
            private set
            {
                if (mValue == value)
                {
                    return;
                }
                mValue = value;
                EditorPrefs.SetInt(Key, value);
            }
        }

        public IukerIntSetting(string key, int defaultValue) : base(key)
        {
            mValue = EditorPrefs.GetInt(Key, defaultValue);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator int(IukerIntSetting self)
        {
            return self.Value;
        }
    }
}