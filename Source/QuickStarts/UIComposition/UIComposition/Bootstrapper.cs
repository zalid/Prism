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
    using Prism.Interfaces;
    using Prism.Regions;
    using UIComposition.Modules.Employee;
    using UIComposition.Modules.Project;

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
            this.RegisterRegionAdapters();
            this.InitializeShell();
            this.InitializeModules();
        }

        private void InitializeContainer()
        {
            this.container = new UnityContainer();
            this.container.RegisterInstance<IUnityContainer>(this.container);
        }

        private void RegisterRegionAdapters()
        {
            RegionAdapterMappings mappings = new RegionAdapterMappings();
            mappings.RegisterMapping(typeof(ItemsControl), new ItemsControlRegionAdapter());
            mappings.RegisterMapping(typeof(ContentControl), new ContentControlRegionAdapter());

            this.container.RegisterInstance<RegionAdapterMappings>(mappings);
        }

        private void InitializeShell()
        {
            this.container.RegisterType<IRegionManager, RegionManager>(new ContainerControlledLifetimeManager());
            Shell shell = this.container.Resolve<Shell>();

            if (shell != null)
            {
                RegionManager.SetRegionManager(shell, container.Resolve<IRegionManager>());
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