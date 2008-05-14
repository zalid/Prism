//===============================================================================
// Microsoft patterns & practices
// Composite WPF (PRISM)
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

using System;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Interfaces;
using Prism.Interfaces.Logging;
using Prism.Regions;
using Prism.Services;
using Prism.UnityContainerAdapter.Properties;

namespace Prism.UnityContainerAdapter
{
    public abstract class Bootstrapper
    {
        private IUnityContainer container;

        public void Run()
        {
            IPrismLogger logger = GetLogger();

            if (logger == null)
            {
                throw new InvalidOperationException(Resources.NullPrismLoggerException);
            }

            container = CreateContainer();
            if (container == null)
            {
                throw new InvalidOperationException(Resources.NullUnityContainerException);
            }

            container.RegisterInstance<IUnityContainer>(container);
            container.RegisterType<IPrismContainer, UnityPrismContainer>(new ContainerControlledLifetimeManager());

            container.RegisterInstance<IPrismLogger>(logger);

            // register services
            container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());

            IRegionManager regionManager = GetRegionManager();
            container.RegisterInstance<IRegionManager>(regionManager);

            InitializeModules();
        }

        protected virtual void InitializeModules()
        {
            IModuleEnumerator moduleEnumerator = GetModuleEnumerator();
            if (moduleEnumerator == null)
            {
                throw new InvalidOperationException(Resources.NullModuleEnumeratorException);
            }

            IModuleLoaderService moduleLoaderService = GetModuleLoaderService();
            if (moduleLoaderService == null)
            {
                throw new InvalidOperationException(Resources.NullModuleLoaderException);
            }

            container.RegisterInstance<IModuleEnumerator>(moduleEnumerator);
            container.RegisterInstance<IModuleLoaderService>(moduleLoaderService);

            ModuleInfo[] moduleInfo = moduleEnumerator.GetStartupLoadedModules();
            moduleLoaderService.Initialize(moduleInfo);
        }

        protected virtual IUnityContainer CreateContainer()
        {
            return new UnityContainer();
        }

        protected virtual IModuleLoaderService GetModuleLoaderService()
        {
            return container.Resolve<ModuleLoaderService>();
        }

        protected virtual IRegionManager GetRegionManager()
        {
            RegionAdapterMappings mappings = GetRegionAdapterMappings();
            return new RegionManager(mappings);
        }

        protected IUnityContainer Container
        {
            get { return container; }
        }

        protected virtual RegionAdapterMappings GetRegionAdapterMappings()
        {
            RegionAdapterMappings regionAdapterMappings = new RegionAdapterMappings();
            regionAdapterMappings.RegisterMapping(typeof(ItemsControl), new ItemsControlRegionAdapter());
            regionAdapterMappings.RegisterMapping(typeof(ContentControl), new ContentControlRegionAdapter());

            return regionAdapterMappings;
        }

        protected virtual IModuleEnumerator GetModuleEnumerator()
        {
            throw new InvalidOperationException(Resources.NotOverwrittenGetModuleEnumeratorException);
        }
        protected abstract IPrismLogger GetLogger();
    }
}
