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
using Prism;
using Prism.Interfaces;
using Prism.Interfaces.Logging;
using Prism.Regions;
using Prism.Services;
using Prism.UnityContainerAdapter;

namespace DirectoryLookupModularity
{
    public class Bootstrapper
    {
        IUnityContainer container;

        public void Initialize()
        {
            InitializeContainer();
            RegisterGlobalServices();
            RegisterRegions();
            ShowShell();
            InitializeModules();
        }

        private void InitializeContainer()
        {
            container = new UnityContainer();
            container.RegisterInstance<IUnityContainer>(container);
            container.RegisterType<IPrismContainer, UnityPrismContainer>(new ContainerControlledLifetimeManager());
            container.RegisterInstance<IPrismLogger>(new ModularityLogger());
            PrismContainerProvider.Provider = container.Resolve<IPrismContainer>();
        }

        private void RegisterGlobalServices()
        {
            container.RegisterType<IModuleLoaderService, ModuleLoaderService>(new ContainerControlledLifetimeManager());
        }

        private void RegisterRegions()
        {
            container.RegisterType<IRegion<Panel>, PanelRegion>();
        }

        private void ShowShell()
        {
            Shell shell = new Shell();

            IRegionManagerService regionManagerService = RegionManager.GetRegionManagerServiceScope(shell);
            container.RegisterInstance<IRegionManagerService>(regionManagerService);

            shell.Show();
        }

        private void InitializeModules()
        {
            container.RegisterInstance<IModuleEnumerator>(new DirectoryLookupModuleEnumerator(@".\Modules"));

            IModuleEnumerator enumerator = container.Resolve<IModuleEnumerator>();
            IModuleLoaderService loaderService = container.Resolve<IModuleLoaderService>();

            loaderService.Initialize(enumerator.GetStartupLoadedModules());
        }
    }
}
