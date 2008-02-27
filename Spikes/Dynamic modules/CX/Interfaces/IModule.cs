using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CX.Facades;

namespace CX.Interfaces
{
    public interface IModule : IModuleInitializable
    {
        void RegisterServices();
        void RegisterViews();
    }

    public interface IModuleInitializable
    {
        void Initialize();
    }
}
