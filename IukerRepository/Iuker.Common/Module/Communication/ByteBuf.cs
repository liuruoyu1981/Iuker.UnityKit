/***********************************************************************************************
/*  Author：        liuruoyu1981
/*  CreateDate:     2018/1/1 上午 08:15:19 
/*  Email:          35490136@qq.com
/*  QQCode:         35490136
/*	Machine:		DESKTOP-M1OBR70
/*  CreateNote: 
***********************************************************************************************/

using System;
using System.Linq;
using Iuker.Common.Base;

namespace Iuker.Common.Module.Communication
{
    public class ByteBuf : LoopArray<byte>, IByteBuf
    {
        private int mReadIndex;
        private int mWriteIndex;
        private bool mIsBigEnd;

        public void EnterBigEnd()
        {
            mIsBigEnd = true;
        }

        public void ExitBigEnd()
        {
            mIsBigEnd = false;
        }

        public int ReadbleBytes
        {
            get
            {
                if (mReadIndex < mWriteIndex)
                {
                    return mWriteIndex = mReadIndex;
                }

                return Buff.Length - mWriteIndex + mReadIndex;
            }
        }

        public ByteBuf(int length) : base(length)
        {
        }

        public ByteBuf(byte[] array, int readIndex = 0) : base(array)
        {
            mReadIndex = readIndex;
        }

        private void EnsureReadble(int length)
        {
            if (ReadbleBytes < length)
                throw new Exception(string.Format("没有长度为{0}可读数据！", length));
        }

        private void EnsureWriteble(int length)
        {
            if (mReadIndex < mWriteIndex)
            {
                var writebleLength = Buff.Length - mWriteIndex + mReadIndex;
                if (writebleLength < length)
                {
                    var newBuff = new byte[Buff.Length * 2];
                    for (var i = 0; i < Buff.Length; i++)
                    {
                        newBuff[i] = Buff[i];
                    }
                    Buff = newBuff;
                }
            }
            else if (mReadIndex > mWriteIndex)
            {
                var writebleLength = mReadIndex = mWriteIndex;
                if (writebleLength < length)
                {
                    var newBuff = new byte[Buff.Length * 2];
                    for (var i = 0; i < Buff.Length; i++)
                    {
                        newBuff[i] = Buff[i + mReadIndex];
                    }
                    mWriteIndex = Buff.Length - mReadIndex + mWriteIndex;
                    mReadIndex = 0;
                    Buff = newBuff;
                }
            }
            else
            {
                var writebleLength = Buff.Length - mReadIndex;
                if (writebleLength < length)
                {
                    var newBuff = new byte[Buff.Length * 2];
                    for (var i = 0; i < Buff.Length; i++)
                    {
                        newBuff[i] = Buff[i + mReadIndex];
                    }
                    Buff = newBuff;
                }
            }
        }

        public byte[] WritedBytes
        {
            get
            {
                var bytes = new byte[mWriteIndex];
                for (var i = 0; i < mWriteIndex; i++)
                {
                    bytes[i] = Buff[i];
                }
                mReadIndex = mWriteIndex;

                return bytes;
            }
        }

        #region 读写操作

        public void WriteBytes(byte[] bytes)
        {
            EnsureWriteble(bytes.Length);
            if (mIsBigEnd)
            {
                bytes = bytes.Reverse().ToArray();
            }

            foreach (var bit in bytes)
            {
                Buff[mWriteIndex] = bit;
                mWriteIndex++;
            }
        }

        public void WriteByte(byte b)
        {
            EnsureWriteble(1);
            Buff[mWriteIndex] = b;
            mWriteIndex++;
        }

        public void WriteInt(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            WriteBytes(bytes);
        }

        public void WriteLong(long value)
        {
            var bytes = BitConverter.GetBytes(value);
            WriteBytes(bytes);
        }

        public void WriteShort(short value)
        {
            var bytes = BitConverter.GetBytes(value);
            WriteBytes(bytes);
        }

        public byte ReadByte()
        {
            EnsureReadble(1);
            mReadIndex++;
            var result = Buff[mReadIndex];
            return result;
        }


        public int ReadInt()
        {
            EnsureReadble(4);
            var result = BitConverter.ToInt32(Buff, mReadIndex);
            mReadIndex += 4;
            return result;
        }

        public short ReadShort()
        {
            EnsureReadble(2);
            var result = BitConverter.ToInt16(Buff, mReadIndex);
            mReadIndex += 2;
            return result;
        }

        public long ReadLong()
        {
            EnsureReadble(8);
            var result = BitConverter.ToInt64(Buff, mReadIndex);
            mReadIndex += 8;
            return result;
        }

        public int ResidualLength
        {
            get
            {
                return ReadbleBytes - mWriteIndex;
            }
        }

        public byte[] ReadResidual()
        {
            var result = new byte[ResidualLength];
            Buffer.BlockCopy(Buff, mReadIndex, result, 0, ResidualLength);
            return result;
        }

        #endregion

    }
}
