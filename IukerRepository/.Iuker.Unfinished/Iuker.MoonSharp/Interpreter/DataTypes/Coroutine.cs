using Iuker.MoonSharp.Interpreter.DataTypes;

namespace Iuker.MoonSharp
{
    /// <summary>
    /// A class representing a script coroutine
    /// </summary>
    public class Coroutine : RefIdObject, IScriptPrivateResource
    {
        public enum CoroutineType
        {
            /// <summary>
            /// A valid coroutine
            /// </summary>
            Coroutine,
            /// <summary>
            /// A CLR callback assigned to a coroutine. 
            /// </summary>
            ClrCallback,
            /// <summary>
            /// A CLR callback assigned to a coroutine and already executed.
            /// </summary>
            ClrCallbackDead
        }

        public CoroutineType Type { get; private set; }

       

        public Script OwnerScript { get; }
    }
}
