/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 22:01:19
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

using System.Collections;

namespace Iuker.UnityKit.Editor.Coroutine
{
    /// <summary>
    /// 协程
    /// </summary>
    public class Coroutine
    {
        public ICoroutineYield CurrentYield = new YieldDefault();
        public IEnumerator Routine;
        public string RoutineUniqueHash;
        public string OwnerUniqueHash;
        public string MethodName = "";

        public int OwnerHash;
        public string OwnerType;

        public bool Finished = false;

        public Coroutine(IEnumerator routine,int ownerHash,string ownerType)
        {
            this.Routine = routine;
            this.OwnerHash = ownerHash;
            this.OwnerType = ownerType;
            this.OwnerUniqueHash = ownerHash + "_" + ownerType;

            if (routine != null)
            {
                string[] split = routine.ToString().Split('<', '>');
                if (split.Length == 3)
                {
                    this.MethodName = split[1];
                }
            }

            RoutineUniqueHash = ownerHash + "_" + ownerType + "_" + MethodName;
        }

        public Coroutine(string methodName, int ownerHash, string ownerType)
        {
            this.MethodName = methodName;
            this.OwnerHash = ownerHash;
            this.OwnerType = ownerType;
            this.OwnerUniqueHash = ownerHash + "_" + ownerType;
            this.RoutineUniqueHash = ownerHash + "_" + ownerType + "_" + MethodName;
        }


    }
}
