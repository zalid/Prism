using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prism.Interfaces;
using Microsoft.Practices.Unity;

namespace Prism.UnityContainerAdapter
{
    public class UnityPrismContainer : IPrismContainer
    {
        private IUnityContainer _unityContainer;

        public UnityPrismContainer(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _unityContainer.Resolve(type);
        }

        public object TryResolve(Type type)
        {
            object resolved;

            try
            {
                resolved = Resolve(type);
            }
            catch
            {
                resolved = null;
            }

            return resolved;
        }
    }
}
