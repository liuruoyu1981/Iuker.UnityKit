/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 20:19:40
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
using System.Collections.Generic;

namespace Iuker.UnityKit.Run.LinqExtensions.Descendants
{
    internal class InternalUnsafeRefStack
    {
        public static Queue<InternalUnsafeRefStack> RefStackPool = new Queue<InternalUnsafeRefStack>();

        public int size = 0;
        public Enumerator_Descendants[] array; // Pop = this.array[--size];

        public InternalUnsafeRefStack(int initialStackDepth)
        {
            array = new Enumerator_Descendants[initialStackDepth];
        }

        public void Push(ref Enumerator_Descendants e)
        {
            if (size == array.Length)
            {
                Array.Resize(ref array, array.Length * 2);
            }
            array[size++] = e;
        }

        public void Reset()
        {
            size = 0;
        }


    }
}
