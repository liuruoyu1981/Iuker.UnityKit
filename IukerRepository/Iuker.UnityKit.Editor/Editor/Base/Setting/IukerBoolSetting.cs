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
    /// 布尔值设置
    /// </summary>
    [Serializable]
    public class IukerBoolSetting : IukerSettingBase
    {
        private bool mValue;

        public bool Value
        {
            get { return mValue; }
            private set
            {
                if (mValue == value)
                {
                    return;
                }
                mValue = value;
                IukerEditorPrefs.SetBool(Key, value);
            }
        }

        public IukerBoolSetting(string key) : base(key)
        {
            mValue = IukerEditorPrefs.GetBool(Key);
        }

        /// <summary>
        /// 切换该布尔设置的状态
        /// </summary>
        /// <returns></returns>
        public bool Toggle()
        {
            Value = !Value;
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator bool(IukerBoolSetting self)
        {
            return self.Value;
        }
    }
}