/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/08/20 17:47
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
namespace Iuker.YourSharp.Parser
{
    /// <summary>
    /// 语法树节点位置信息
    /// </summary>
    public class Location
    {
        /// <summary>
        /// 开始位置
        /// </summary>
        public Position Start;

        /// <summary>
        /// 结束位置
        /// </summary>
        public Position End;

        /// <summary>
        /// 源代码
        /// </summary>
        public string Source;

        public Location(int line, int column)
        {
            Start = new Position(line, column);
        }
    }
}