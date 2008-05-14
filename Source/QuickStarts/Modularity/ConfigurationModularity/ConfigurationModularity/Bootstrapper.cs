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

using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Prism.Interfaces;
using Prism.Interfaces.Logging;
using Prism.Regions;
using Prism.Services;
using Prism.UnityContainerAdapter;

namespace ConfigurationModularity
{
    public class Bootstrapper
    {
        IUnityContainer container;

        public void Initialize()
        {
            InitializeContainer();
            RegisterGlobalServices();
            RegisterRegionAdapters();
            ShowShell();
            InitializeModules();
        }

        private void InitializeContainer()
        {
            container = new UnityContainer();
            container.RegisterInstance<IUnityContainer>(container);
            container.RegisterType<IPrismContainer, UnityPrismContainer>(new ContainerControlledLifetimeManager());
            container.RegisterInstance<IPrismLogger>(new ModularityLogger());
        }

        private void RegisterGlobalServices()
        {
            container.RegisterType<IModuleLoaderService, ModuleLoaderService>(new ContainerControlledLifetimeManager());
        }

        private void RegisterRegionAdapters()
        {
            RegionAdapterMappings mappings = new RegionAdapterMappings();
            mappings.RegisterMapping(typeof(ItemsControl), new ItemsControlRegionAdapter());
            mappings.RegisterMapping(typeof(ContentControl), new ContentControlRegionAdapter());

            this.container.RegisterInstance<RegionAdapterMappings>(mappings);
        }

        private void ShowShell()
        {
            this.container.RegisterType<IRegionManager, RegionManager>(new ContainerControlledLifetimeManager());
            Shell shell = new Shell();

            RegionManager.SetRegionManager(shell, container.Resolve<IRegionManager>());

            shell.Show();
        }

        private void InitializeModules()
        {
            ConfigurationStore store = new ConfigurationStore();
            container.RegisterInstance<IModuleEnumerator>(new ConfigurationModuleEnumerator(store));

            IModuleEnumerator enumerator = container.Resolve<IModuleEnumerator>();
            IModuleLoaderService loaderService = container.Resolve<IModuleLoaderService>();

            loaderService.Initialize(enumerator.GetStartupLoadedModules());
        }
    }
}
