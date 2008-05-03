using System;

namespace Prism.Interfaces
{
    public interface IModuleInitializerService
    {
        void Initialize(Type[] typeList);
    }
}