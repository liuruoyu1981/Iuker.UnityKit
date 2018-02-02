using System;
using Iuker.Common.Base.Enums;
using Iuker.Common.Base.Interfaces;
using Iuker.UnityKit.Run.Base;

namespace Iuker.UnityKit.Run.Module.Test
{
    public class DefaultU3dModule_Test : AbsU3dModule, IU3dTestModule
    {
        public override string ModuleName
        {
            get
            {
                return ModuleType.Test.ToString();
            }
        }

        public void Register(ITester tester)
        {
        }

        public void ExecuteTest(string test, string action)
        {
        }

        public void SloveException(Exception exception)
        {
        }
    }
}