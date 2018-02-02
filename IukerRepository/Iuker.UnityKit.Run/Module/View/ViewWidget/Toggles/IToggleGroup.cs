using System.Collections.Generic;

namespace Iuker.UnityKit.Run.Module.View.ViewWidget
{
    public interface IToggleGroup
    {
        IToggle ActiveToggle { get; }

        List<IToggle> Toggles { get; }

        void SwitchActiveToggle(IToggle target);

    }
}
