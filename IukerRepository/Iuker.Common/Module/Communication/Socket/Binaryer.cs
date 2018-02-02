/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/04/23 20:25:22
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
using System.IO;

namespace Iuker.Common.Module.Socket
{
    /// <summary>
    /// 将数据写入成二进制
    /// </summary>
    public class Binaryer
    {
        private readonly MemoryStream ms = new MemoryStream();
        private readonly BinaryWriter bw;
        private readonly BinaryReader br;
        public void Close()
        {
            bw.Close();
            br.Close();
            ms.Close();
        }

        /// <summary>
        /// 支持传入初始数据的构造
        /// </summary>
        /// <param name="buff"></param>
        public Binaryer(byte[] buff)
        {
            ms = new MemoryStream(buff);
            bw = new BinaryWriter(ms);
            br = new BinaryReader(ms);
        }

        /// <summary>
        /// 获取当前数据 读取到的下标位置
        /// </summary>
        public int Position
        {
            get
            {
                return (int)ms.Position;
            }
        }

        /// <summary>
        /// 获取当前数据长度
        /// </summary>
        public int Length
        {
            get { return (int)ms.Length; }
        }

        /// <summary>
        /// 剩余的数据长度
        /// </summary>
        public int ResidueCount
        {
            get { return (int)(ms.Length - ms.Position); }
        }

        /// <summary>
        /// 默认构造
        /// </summary>
        public Binaryer()
        {
            bw = new BinaryWriter(ms);
            br = new BinaryReader(ms);
        }

        public void Write(int value)
        {
            bw.Write(value);
        }

        public void Write(byte value)
        {
            bw.Write(value);
        }

        public void Write(bool value)
        {
            bw.Write(value);
        }
        public void Write(string value)
        {
            bw.Write(value);
        }
        public void Write(byte[] value)
        {
            bw.Write(value);
        }

        public void Write(double value)
        {
            bw.Write(value);
        }
        public void Write(float value)
        {
            bw.Write(value);
        }
        public void Write(long value)
        {
            bw.Write(value);
        }

        public void Write(ushort value)
        {
            bw.Write(value);
        }


        public void Read(out int value)
        {
            value = br.ReadInt32();
        }
        public void Read(out byte value)
        {
            value = br.ReadByte();
        }

        public void Read(out bool value)
        {
            value = br.ReadBoolean();
        }

        public void Read(out ushort value)
        {
            value = br.ReadUInt16();
        }

        public byte[] ReadBytes(int count)
        {
            var result = br.ReadBytes(count);
            return result;
        }

        public void Read(out string value)
        {
            value = br.ReadString();
        }
        public void Read(out byte[] value, int length)
        {
            value = br.ReadBytes(length);
        }

        public void Read(out double value)
        {
            value = br.ReadDouble();
        }
        public void Read(out float value)
        {
            value = br.ReadSingle();
        }
        public void Read(out long value)
        {
            value = br.ReadInt64();
        }

        public void reposition()
        {
            ms.Position = 0;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public byte[] GetBuff()
        {
            byte[] result = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, (int)ms.Length);
            return result;
        }
    }
}
