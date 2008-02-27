using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using CX.Interfaces;

namespace CX.Facades
{
    public interface ICXContainerFacade
    {
        ICXContainerFacade Register<I, T>() where T : class;
        ICXContainerFacade Register<I, T>(string key) where T : class;
        ICXContainerFacade Register(Type I, Type T);
        ICXContainerFacade Register(Type I, Type T, string key);

        ICXContainerFacade RegisterInstance<T>(object instance);

        ICXContainerFacade RegisterAsSingleton<I, T>() where T : class;
        ICXContainerFacade RegisterAsSingleton<I, T>(string key) where T : class;
        ICXContainerFacade RegisterAsSingleton(Type I, Type T);
        ICXContainerFacade RegisterAsSingleton(Type I, Type T, string key);

        T Resolve<T>();
        T Resolve<T>(string key);
        IEnumerable<T> ResolveAll<T>() where T : class;
    }
}
