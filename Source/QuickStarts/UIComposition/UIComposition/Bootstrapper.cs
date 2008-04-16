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

namespace UIComposition
{
    using System;
    using System.Windows.Controls;
    using Microsoft.Practices.Unity;
    using Prism;
    using Prism.Interfaces;
    using Prism.Regions;
    using Prism.Services;
    using Prism.UnityContainerAdapter;
    using UIComposition.Modules.Employee;
    using UIComposition.Modules.Project;
    using UIComposition.Infrastructure.Controls;
    using UIComposition.Infrastructure.Regions;

    internal class Bootstrapper : IDisposable
    {
        private IUnityContainer container;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Initialize()
        {
            this.InitializeContainer();
            this.RegisterRegions();
            this.InitializeShell();
            this.InitializeModules();
        }

        private void InitializeContainer()
        {
            this.container = new UnityContainer();
            this.container.RegisterInstance<IUnityContainer>(this.container);
            PrismContainerProvider.Provider = new UnityPrismContainer(this.container);
        }

        private void RegisterRegions()
        {
            this.container.RegisterType<IRegion<DeckPanel>, DeckRegion>();
            this.container.RegisterType<IRegion<Panel>, PanelRegion>();
            this.container.RegisterType<IRegion<ItemsControl>, ItemsControlRegion>();
        }

        private void InitializeShell()
        {
            Shell shell = this.container.Resolve<Shell>();

            IRegionManagerService rms = RegionManager.GetRegionManagerServiceScope(shell);
            this.container.RegisterInstance<IRegionManagerService>(rms);

            if (shell != null)
            {
                shell.Show();
            }
        }

        private void InitializeModules()
        {
            IModule employeeModule = this.container.Resolve<EmployeeModule>();
            employeeModule.Initialize();

            IModule projectModule = this.container.Resolve<ProjectModule>();
            projectModule.Initialize();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.container.Dispose();
            }
        }
    }
}