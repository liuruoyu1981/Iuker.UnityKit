using System;
using System.Collections;
using UnityEngine;

namespace Iuker.UnityKit.Editor.Coroutine
{
    public class IukerCoroutine
    {
        /// <summary>
        /// 协程包装迭代器
        /// </summary>
        /// <param name="task"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public IEnumerator Coroutine(Action task, float time = 0f)
        {
            yield return new WaitForSeconds(time);
            if (task != null)
            {
                task();
            }
        }




    }
}