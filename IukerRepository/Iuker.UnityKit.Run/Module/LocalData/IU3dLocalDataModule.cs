/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/04 17:24:19
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
using Iuker.Common;
using Iuker.Common.Module.LocalData;

namespace Iuker.UnityKit.Run.Module.LocalData
{
    public interface IU3dLocalDataModule : IModule
    {
        /// <summary>
        /// 获得一类本地数据类型的所有记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetAllRecord<T>(string assetName) where T : ILocalDataEntity<T>, new();

    }
}
