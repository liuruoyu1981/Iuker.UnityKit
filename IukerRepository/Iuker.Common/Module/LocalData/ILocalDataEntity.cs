/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/17 14:07:17
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


namespace Iuker.Common.Module.LocalData
{
    /// <summary>
    /// 本地数据接口
    /// </summary>
    public interface ILocalDataEntity<T>
    {
        /// <summary>
        /// 生成一个本地数据实体对象（对应于本地数据源的一条数据）
        /// </summary>
        /// <param name="row">数据源中的一条数据，如txt文本中的一行</param>
        /// <returns></returns>
        T CreateEntity(List<string> row);

        /// <summary>
        /// 初始化实体数据，返回该类型数据的集合
        /// </summary>
        /// <returns></returns>
        List<T> CreateEntitys(List<string> objList);
    }
}
