/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/03/04 21:19:54
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
using UnityEngine;

namespace Iuker.UnityKit.Run.Assist
{
    /// <summary>
    /// 可视化路径点
    /// </summary>
    public class VisualPathNode : IukMono
    {
        public bool isRed = true;
        public List<Transform> wayPointList = new List<Transform>();

        public  void Init()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var son = transform.GetChild(i);
                wayPointList.Add(son);
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = isRed ? Color.red : Color.blue;
            for (int i = 0; i < wayPointList.Count - 1; i++)
            {
                Gizmos.DrawLine(wayPointList[i].position, wayPointList[i + 1].position);
            }
        }

    }
}
