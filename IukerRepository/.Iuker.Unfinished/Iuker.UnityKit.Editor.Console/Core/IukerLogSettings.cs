/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 6/18/2017 11:35
Email: liuruoyu1981@gmail.com
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
using UnityEngine;

namespace Iuker.UnityKit.Editor.Console.Core
{
    /// <summary>
    /// 控制台设置
    /// </summary>
    public class IukerLogSettings : ScriptableObject
    {
        [SerializeField]
        private List<string> _filter = new List<string>();

        public List<string> FilterLower { get; } = new List<string>();

        public void CacheFilterLower()
        {
            FilterLower.Clear();
            foreach (var f in _filter)
            {
                var fLower = f.Trim().ToLower();
                if (!string.IsNullOrEmpty(fLower))
                {
                    FilterLower.Add(fLower);
                }
            }
        }




    }
}