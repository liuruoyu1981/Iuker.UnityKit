/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/12 14:49:17
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

using Iuker.Common.Base.Enums;
using Iuker.UnityKit.Run.Base;

namespace Iuker.UnityKit.Run.Module.Http
{
    public class DefaultU3dModule_Http : AbsU3dModule, IU3dHttpModule
    {
        public override string ModuleName
        {
            get
            {
                return ModuleType.Http.ToString();
            }
        }

    }
}
