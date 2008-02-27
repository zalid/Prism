using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using CX.Facades;
using CX.Interfaces;
using CX.Services;
using Castle.Windsor.Configuration.Interpreters;
using CX;
using CX.Containers;
using Microsoft.Practices.Unity;
using CX.UnityContainer;

namespace RegionsSpike
{
    public class Bootstrapper
    {
        public void Initialize()
        {
            IWindsorContainer container = new WindsorContainer();
            container.Kernel.AddComponentInstance("container", typeof(IWindsorContainer), container);
            container.AddComponent<ICXContainerFacade, CXWindsorContainerFacade>();
            ICXContainerFacade cxContainer = container.Resolve<ICXContainerFacade>();

            /*
            IUnityContainer container = new UnityContainer()
                .AddNewExtension<PrismExtension>();
            container.RegisterInstance<IUnityContainer>(container);
            container.Register<ICXContainerFacade, UnityContainerFacade>();
            ICXContainerFacade cxContainer = container.Get<ICXContainerFacade>();
             */

            RegisterViews(cxContainer);
            RegisterServices(cxContainer);

            IShell shell = (IShell) cxContainer.Resolve<IShell>();
            shell.Show();

            InitializeModules(cxContainer);

        }

        private static void InitializeModules(ICXContainerFacade cxContainer)
        {
            InitializeModule<RestaurantModule.ModuleInitializer2>(cxContainer);
            InitializeModule<WeatherModule.ModuleInit>(cxContainer);
            InitializeModule<Module3.ModuleInitializer3>(cxContainer);
        }

        private static void InitializeModule<TModule>(ICXContainerFacade cxContainer) where TModule:IModule 
        {
            Type moduleType = typeof(TModule);
            cxContainer.RegisterAsSingleton(moduleType, moduleType,moduleType.Name);
            IModule module = cxContainer.Resolve<TModule>();

            module.RegisterServices();
            module.RegisterViews();
            module.Initialize();
        }

 
        private static void RegisterViews(ICXContainerFacade cxContainer)
        {
            cxContainer.Register<IShell, Shell>();
            cxContainer.Register<ShellPresenter, ShellPresenter>();
        }

        private static void RegisterServices(ICXContainerFacade cxContainer)
        {
            cxContainer.RegisterAsSingleton<IRegionManager, RegionManager>();
            cxContainer.RegisterInstance<IEventBrokerService>(new EventBrokerService());
        }
    }
}
