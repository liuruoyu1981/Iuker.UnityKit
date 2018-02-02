using Iuker.Js.Native;
using Iuker.Js.Native.Object;

namespace Iuker.Js.Runtime.Environments
{
    /// <summary>
    /// 词法环境记录的基本实现
    /// http://www.ecma-international.org/ecma-262/5.1/#sec-10.2.1
    /// </summary>
    public abstract class EnvironmentRecord : ObjectInstance
    {
        protected EnvironmentRecord(Engine engine) : base(engine)
        {
        }

        /// <summary>
        /// 确定环境记录是否对标识符有绑定。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract bool HasBinnding(string name);

        /// <summary>
        /// 在环境记录中创建一个新的可变绑定。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="canBeDeleted"></param>
        public abstract void CreateMutableBinding(string name, bool canBeDeleted = false);

        /// <summary>
        /// 设置环境记录中已经存在的可变绑定的值。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="strict"></param>
        public abstract void SetMutableBinding(string name, JsValue value, bool strict);

        /// <summary>
        /// 从环境记录返回已经存在的绑定的值。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="strict"></param>
        /// <returns></returns>
        public abstract JsValue GetBindingValue(string name, bool strict);













































    }
}
