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
using Prism.Events;
using Prism.Interfaces;
using Prism.Regions;
using Prism.UnityContainerAdapter;
using Prism;

namespace EventAggregation
{
    public class Bootstrapper
    {
        IUnityContainer container;
        internal void Initialize()
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
        }

        private void RegisterGlobalServices()
        {
            container.RegisterType<IRegionManager, RegionManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
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
            var shell = container.Resolve<Shell>();
            RegionManager.SetRegionManager(shell, container.Resolve<IRegionManager>());
            shell.Show();
        }

        private void InitializeModules()
        {
            var moduleA = container.Resolve<ModuleA.ModuleA>();
            var moduleB = container.Resolve<ModuleB.ModuleB>();

            moduleA.Initialize();
            moduleB.Initialize();
        }

    }
}
