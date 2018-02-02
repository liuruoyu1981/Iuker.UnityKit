/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/06/19 20:58
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

namespace Iuker.UnityKit.Editor.Protobuf
{
    /// <summary>
    /// protobuf协议字段描述数据
    /// </summary>
    public class ProtobufFieldDesc
    {
        /// <summary>
        /// protobuf协议字段名
        /// </summary>
        public string FieldName { get; private set; }

        /// <summary>
        /// protobuf协议字段类型
        /// </summary>
        public ProtobufFiledType ProtobufFiledType { get; private set; }

        /// <summary>
        /// protobuf协议字段数据类型
        /// </summary>
        public string DataType { get; private set; }

        /// <summary>
        /// protobuf协议字段索引编号
        /// </summary>
        //private int mFiledIndex;

        public string Note { get; private set; }

        /// <summary>
        /// 构建一个protobuf协议描述实体类的实例
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public static ProtobufFieldDesc CreateProtobufMessageDesc(string[] source, int index, string note = null)
        {
            var desc = new ProtobufFieldDesc();
            var newSource = ClearSpace(source);
            desc.ProtobufFiledType = (ProtobufFiledType)Enum.Parse(typeof(ProtobufFiledType), newSource[0]);
            desc.DataType = newSource[1];
            desc.FieldName = newSource[2];
            //desc.mFiledIndex = index;
            desc.Note = note;

            return desc;
        }

        private static string[] ClearSpace(string[] source)
        {
            List<string> newProList = new List<string>();
            foreach (var s in source)
            {
                if (s != "")
                {
                    newProList.Add(s);
                }
            }
            return newProList.ToArray();
        }

    }
}