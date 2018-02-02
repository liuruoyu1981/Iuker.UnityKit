namespace Iuker.Js.Native.Number.Date
{
    public static class NumberExtensions
    {
        public static long UnsignedShift(this long l, int shift) => (long)((ulong)l >> shift);
    }
}
