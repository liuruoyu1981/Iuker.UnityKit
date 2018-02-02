/***********************************************************************************************
/*  Author：        liuruoyu1981
/*  CreateDate:     2018/1/1 上午 08:43:54 
/*  Email:          35490136@qq.com
/*  QQCode:         35490136
/*	Machine:		DESKTOP-M1OBR70
/*  CreateNote: 
***********************************************************************************************/

namespace Iuker.Common.Base
{
    public class LoopArray<T>
    {
        protected T[] Buff;

        public LoopArray(int length)
        {
            Buff = new T[length];
        }

        public LoopArray(T[] array)
        {
            Buff = array;
        }

        public T this[int index]
        {
            get
            {
                if (index == Buff.Length)
                {
                    index = 0;
                }

                return Buff[index];
            }
            set
            {
                if (index == Buff.Length)
                {
                    index = 0;
                }

                Buff[index] = value;
            }
        }



    }
}
