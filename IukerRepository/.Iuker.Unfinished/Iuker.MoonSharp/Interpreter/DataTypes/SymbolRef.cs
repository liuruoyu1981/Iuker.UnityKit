using System.Collections.Generic;
using System.IO;

namespace Iuker.MoonSharp.Interpreter.DataTypes
{
    /// <summary>
    /// 该类存储了一个可能存在的左值
    /// </summary>
    public class SymbolRef
    {
        private static SymbolRef s_DefaultEnv = new SymbolRef() { i_Type = SymbolRefType.DefaultEnv };

        // 内部字段，通过直接访问可以提高10%的性能
        internal SymbolRefType i_Type;
        internal SymbolRef i_Env;
        internal int i_Index;
        internal string i_Name;

        /// <summary>
        /// 获取此符号的潜在左值类型
        /// </summary>
        public SymbolRefType Type => i_Type;

        /// <summary>
        /// 获取此符号在其作用域上下文中的索引
        /// </summary>
        public int Index => i_Index;

        /// <summary>
        /// 获取此符号的名字
        /// </summary>
        public string Name => i_Name;

        public SymbolRef Environment => i_Env;

        public static SymbolRef DefaultEnv => s_DefaultEnv;

        /// <summary>
        /// 为一个全局变量创建一个新的符号
        /// </summary>
        /// <param name="name"></param>
        /// <param name="envSymbol"></param>
        /// <returns></returns>
        public static SymbolRef Global(string name, SymbolRef envSymbol) =>
            new SymbolRef
            {
                i_Index = -1,
                i_Type = SymbolRefType.Global,
                i_Env = envSymbol,
                i_Name = name,
            };

        /// <summary>
        /// 为一个局部变量创建一个新的符号
        /// </summary>
        /// <param name="name"></param>
        /// <param name="inex"></param>
        /// <returns></returns>
        internal static SymbolRef Local(string name, int inex) =>
            new SymbolRef { i_Index = inex, i_Name = name, i_Type = SymbolRefType.Local };

        /// <summary>
        /// 为一个外部局部变量创建一个新的符号
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static SymbolRef Upvalue(string name, int index) =>
            new SymbolRef { i_Index = index, i_Name = name, i_Type = SymbolRefType.Upvalue };

        public override string ToString()
        {
            if (i_Type == SymbolRefType.DefaultEnv)
                return "(default _ENV)";
            if (i_Type == SymbolRefType.Global)
                return string.Format("{2} : {0} / {1}", i_Type, i_Env, i_Name);
            return string.Format("{2} : {0}[{1}]", i_Type, i_Index, i_Name);
        }

        /// <summary>
        /// 将实例以二进制形式写入
        /// </summary>
        /// <param name="binaryWriter"></param>
        internal void WriteBinary(BinaryWriter binaryWriter)
        {
            binaryWriter.Write((byte)i_Type);
            binaryWriter.Write(i_Index);
            binaryWriter.Write(i_Name);
        }

        /// <summary>
        /// 通过二进制读取器反序列化一个实例
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns></returns>
        internal static SymbolRef ReadBinary(BinaryReader binaryReader)
        {
            SymbolRef that = new SymbolRef();
            that.i_Type = (SymbolRefType)binaryReader.ReadByte();
            that.i_Index = binaryReader.ReadInt32();
            that.i_Name = binaryReader.ReadString();
            return that;
        }

        internal void WriteBinaryEnv(BinaryWriter binaryWriter, Dictionary<SymbolRef, int> symbolMap)
        {
            if (i_Env != null)
                binaryWriter.Write(symbolMap[i_Env]);
            else
            {
                binaryWriter.Write(-1);
            }
        }

        internal void ReadBinaryEnv(BinaryReader binaryReader, SymbolRef[] symbolRefs)
        {
            int idx = binaryReader.ReadInt32();

            if (idx >= 0)
                i_Env = symbolRefs[idx];
        }

    }
}
