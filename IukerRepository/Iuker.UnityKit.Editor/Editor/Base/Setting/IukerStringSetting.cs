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

namespace Iuker.UnityKit.Editor.Setting
{
    /// <summary>
    /// 浮点值设置
    /// </summary>
    [Serializable]
    public class IukerStringSetting : IukerSettingBase
    {
        private string mValue;

        public string Value
        {
            get { return mValue; }
            private set
            {
                if (mValue == value)
                {
                    return;
                }
                mValue = value;
                IukerEditorPrefs.SetString(Key, value);
            }
        }

        public IukerStringSetting(string key) : base(key)
        {
            mValue = IukerEditorPrefs.GetString(Key);
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(IukerStringSetting self)
        {
            return self.Value;
        }
    }
}