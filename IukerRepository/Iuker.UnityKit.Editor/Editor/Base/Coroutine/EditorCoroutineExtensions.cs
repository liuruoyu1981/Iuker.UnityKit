/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/11 22:57:59
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
using UnityEditor;

namespace Iuker.UnityKit.Editor.Coroutine
{
    public static class EditorCoroutineExtensions
    {
        #region EditorWindow

        public static void StartCoroutine(this EditorWindow thisRef, IEnumerator coroutine)
        {
            EditorCoroutine.StartCoroutine(coroutine, thisRef);
        }

        public static void StartCoroutine(this EditorWindow thisRef, string methodName)
        {
            EditorCoroutine.StartCoroutine(methodName, thisRef);
        }

        public static void StartCoroutine(this EditorWindow thisRef, string methodName, object value)
        {
            EditorCoroutine.StartCoroutine(methodName, value, thisRef);
        }

        public static void StopCoroutine(this EditorWindow thisRef, IEnumerator coroutine)
        {
            EditorCoroutine.StopCoroutine(coroutine, thisRef);
        }

        public static void StopCoroutine(this EditorWindow thisRef, string methodName)
        {
            EditorCoroutine.StopCoroutine(methodName, thisRef);
        }

        public static void StopAllCoroutines(this EditorWindow thisRef)
        {
            EditorCoroutine.StopAllCoroutines(thisRef);
        }

        #endregion

    }
}
