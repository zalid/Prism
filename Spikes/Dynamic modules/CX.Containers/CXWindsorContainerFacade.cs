using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.Windsor.Configuration;
using System.Xml;
using System.IO;
using System.Reflection;
using Castle.Windsor.Configuration.Interpreters;
using CX.Facades;
using CX.Enums;
using System.Configuration;
using Castle.Core;
using System.Collections;
using CX.Interfaces;

namespace CX.Containers
{
    public class CXWindsorContainerFacade : ICXContainerFacade
    {
        private IWindsorContainer container;

        public CXWindsorContainerFacade(IWindsorContainer container)
        {
            this.container = container;
        }

        public T Resolve<T>()
        {
            return this.container.Resolve<T>();
        }

        public T Resolve<T>(string key)
        {
            return this.container.Resolve<T>(key);
        }

        public ICXContainerFacade Register<I, T>()
            where T : class
        {
            this.container.AddComponent<I, T>();
            return this;
        }

        public ICXContainerFacade Register<I, T>(string key)
            where T : class
        {
            this.container.AddComponent<I, T>(key);
            return this;
        }

        public IEnumerable<T> ResolveAll<T>() 
            where T : class
        {
            return this.container.Kernel.ResolveAll(typeof(T), null) as IEnumerable<T>;
        }

        public ICXContainerFacade Register(Type I, Type T)
        {
            this.container.AddComponent(string.Empty, I, T);
            return this;
        }

        public ICXContainerFacade Register(Type I, Type T, string key)
        {
            this.container.AddComponent(key, I, T);
            return this;
        }

        public ICXContainerFacade RegisterAsSingleton<I, T>() where T : class
        {
            this.container.AddComponentWithLifestyle<I, T>(LifestyleType.Singleton);
            return this;
        }

        public ICXContainerFacade RegisterAsSingleton<I, T>(string key) where T : class
        {
            this.container.AddComponent<I, T>(key);
            return this;
        }

        public ICXContainerFacade RegisterAsSingleton(Type I, Type T)
        {
            this.container.AddComponentWithLifestyle(string.Empty, I, T, LifestyleType.Singleton);
            return this;
        }

        public ICXContainerFacade RegisterAsSingleton(Type I, Type T, string key)
        {
            this.container.AddComponentWithLifestyle(key, I, T, LifestyleType.Singleton);
            return this;
        }

        public ICXContainerFacade RegisterInstance<T>(object instance)
        {
            this.container.Kernel.AddComponentInstance<T>(instance);
            return this;
        }
    }
}
