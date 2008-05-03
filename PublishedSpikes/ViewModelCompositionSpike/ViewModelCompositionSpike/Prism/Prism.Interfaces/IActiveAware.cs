using System;

namespace Prism.Interfaces
{
    public interface IActiveAware
    {
        bool IsActive { get; set; }
        event EventHandler IsActiveChanged;
    }
}