/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 22:04:19
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


namespace Iuker.UnityKit.Editor.Coroutine
{
    public class YieldWaitForSeconds : ICoroutineYield
    {
        /// <summary>
        /// 剩余时间
        /// </summary>
        public float TimeLeft;

        public override bool IsDone(float deltaTime)
        {
            TimeLeft -= deltaTime;
            return TimeLeft < 0;
        }

    }
}
