using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.Interfaces
{
    public interface IPrismContainer
    {
        T Resolve<T>();
        object Resolve(Type type);
        object TryResolve(Type type);
    }
}
