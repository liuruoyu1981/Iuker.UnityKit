namespace Iuker.Common.Module.Communication
{
    public interface IByteBuf
    {
        void WriteInt(int value);

        void WriteLong(long value);

        void WriteShort(short value);

        int ReadInt();

        short ReadShort();

        long ReadLong();

        int ReadbleBytes { get; }
    }
}
