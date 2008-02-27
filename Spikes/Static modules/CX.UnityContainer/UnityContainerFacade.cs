using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CX.Facades;
using Microsoft.Practices.Unity;

namespace CX.UnityContainer
{
    public class UnityContainerFacade : ICXContainerFacade
    {

        private IUnityContainer innerContainer;

        public UnityContainerFacade(IUnityContainer container)
        {
            innerContainer = container;
        }
        #region ICXContainerFacade Members

        public T Resolve<T>()
        {
            return innerContainer.Get<T>();
        }

        public T Resolve<T>(string key)
        {
            return innerContainer.Get<T>(key);
        }

        public ICXContainerFacade Register<I, T>() where T : class
        {
            innerContainer.Register(typeof(I), typeof(T));
            return this;
        }

        public ICXContainerFacade Register<I, T>(string key) where T : class
        {
            innerContainer.Register(typeof(I), typeof(T), key);
            return this;
        }

        public ICXContainerFacade Register(Type I, Type T)
        {
            innerContainer.Register(I, T);
            return this;
        }

        public ICXContainerFacade Register(Type I, Type T, string key)
        {
            innerContainer.Register(I, T, key);
            return this;
        }

        public ICXContainerFacade RegisterAsSingleton<I, T>() where T : class
        {
            innerContainer.Register(typeof(I), typeof(T));
            innerContainer.SetSingleton(typeof(T));
            return this;
        }

        public ICXContainerFacade RegisterAsSingleton<I, T>(string key) where T : class
        {
            throw new NotImplementedException();
        }

        public ICXContainerFacade RegisterAsSingleton(Type I, Type T)
        {
            innerContainer.Register(I, T);
            innerContainer.SetSingleton(T);
            return this;
        }

        public ICXContainerFacade RegisterAsSingleton(Type I, Type T, string key)
        {
            innerContainer.Register(I, T, key);
            innerContainer.SetSingleton(T, key);

            return this;
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return innerContainer.GetAll<T>();

        }

        public ICXContainerFacade RegisterInstance<T>(object instance)
        {
            innerContainer.RegisterInstance<T>((T)instance);
            return this;
        }

        #endregion
    }
}
