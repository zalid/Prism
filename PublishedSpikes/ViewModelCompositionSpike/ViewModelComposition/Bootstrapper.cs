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

namespace ViewModelComposition
{
    using System;
    using Microsoft.Practices.Unity;
    using Prism;
    using Prism.Interfaces;
    using Prism.UnityContainerAdapter;
    using ViewModelComposition.Modules.Employees;
    using ViewModelComposition.Modules.Project;

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
            InitializeContainer();
            InitializeShell();
            InitializeModules();
        }

        private void InitializeContainer()
        {
            this.container = new UnityContainer();
            this.container.RegisterInstance<IUnityContainer>(this.container);
            PrismContainerProvider.Provider = new UnityPrismContainer(this.container);
        }

        private void InitializeShell()
        {
            var shellPresentationModel = container.Resolve<ShellPresentationModel>();
            Shell shell = this.container.Resolve<Shell>();

            if (shell != null)
            {
                shell.DataContext = shellPresentationModel;
                shell.Show();
            }

            this.container.RegisterInstance<IRegionManagerService>(shellPresentationModel.RegionManagerService);
        }

        private void InitializeModules()
        {
            IModule projectModule = this.container.Resolve<ProjectModule>();
            projectModule.Initialize();

            IModule employeeModule = this.container.Resolve<EmployeeModule>();
            employeeModule.Initialize();
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