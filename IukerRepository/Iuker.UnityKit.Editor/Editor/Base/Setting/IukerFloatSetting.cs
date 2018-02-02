/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/14 19:31:44
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
using System.Globalization;
using UnityEditor;

namespace Iuker.UnityKit.Editor.Setting
{
    /// <summary>
    /// 浮点值设置
    /// </summary>
    [Serializable]
    public class IukerFloatSetting : IukerSettingBase
    {
        private float mValue;

        public float Value
        {
            get { return mValue; }
            private set
            {
                var selfInt = (int)mValue * 100000;
                var extenInt = (int)value * 100000;
                if (selfInt == extenInt)
                {
                    return;
                }
                mValue = value;
                EditorPrefs.SetFloat(Key, value);
            }
        }

        public IukerFloatSetting(string key, float defaultValue) : base(key)
        {
            mValue = EditorPrefs.GetFloat(Key, defaultValue);
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public static implicit operator float(IukerFloatSetting self)
        {
            return self.Value;
        }
    }
}