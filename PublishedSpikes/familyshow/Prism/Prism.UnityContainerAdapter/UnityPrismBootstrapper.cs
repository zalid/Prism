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
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Interfaces;
using Prism.Interfaces.Logging;
using Prism.Logging;
using Prism.Regions;
using Prism.Services;
using Prism.UnityContainerAdapter.Properties;

namespace Prism.UnityContainerAdapter
{
    public abstract class UnityPrismBootstrapper
    {
        private IUnityContainer container;

        /// <summary>
        /// 
        /// </summary>
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
            container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());

            IRegionManager regionManager = GetRegionManager();
            container.RegisterInstance<IRegionManager>(regionManager);

            RegisterShellServices();

            DependencyObject shell = ShowShell();

            if (shell != null)
            {
                RegionManager.SetRegionManager(shell, regionManager);
            }

            InitializeModules();
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IUnityContainer CreateContainer()
        {
            return new UnityContainer();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IModuleLoaderService GetModuleLoaderService()
        {
            return container.Resolve<ModuleLoaderService>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IRegionManager GetRegionManager()
        {
            RegionAdapterMappings mappings = GetRegionAdapterMappings();
            return new RegionManager(mappings);
        }

        /// <summary>
        /// 
        /// </summary>
        public IUnityContainer Container
        {
            get { return container; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual RegionAdapterMappings GetRegionAdapterMappings()
        {
            RegionAdapterMappings regionAdapterMappings = new RegionAdapterMappings();
            regionAdapterMappings.RegisterMapping(typeof(ItemsControl), new ItemsControlRegionAdapter());
            regionAdapterMappings.RegisterMapping(typeof(ContentControl), new ContentControlRegionAdapter());

            return regionAdapterMappings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IModuleEnumerator GetModuleEnumerator()
        {
            throw new InvalidOperationException(Resources.NotOverwrittenGetModuleEnumeratorException);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual IPrismLogger GetLogger()
        {
            return new TextLogger();
        }

        protected virtual void RegisterShellServices()
        {
        }

        public abstract DependencyObject ShowShell();
    }
}
