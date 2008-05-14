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
using ViewModelComposition.Modules.Employees.Views.EmployeesView;

namespace ViewModelComposition.Modules.Employees
{
    using System.Windows;
    using Microsoft.Practices.Unity;
    using Prism.Interfaces;
    using ViewModelComposition.Modules.Employees.Controllers;
    using ViewModelComposition.Modules.Employees.Services;

    public class EmployeeModule : IModule
    {
        private IUnityContainer container;
        private IRegionManagerService regionManagerService;

        public EmployeeModule(IUnityContainer container, IRegionManagerService regionManagerService)
        {
            this.container = container;
            this.regionManagerService = regionManagerService;
        }

        public void Initialize()
        {
            this.RegisterViewsAndServices();

            EmployeesPresentationModel presenter = this.container.Resolve<EmployeesPresentationModel>();

            IRegion mainRegion = this.regionManagerService.GetRegion(RegionNames.MainRegion);
            mainRegion.Add(presenter);
        }

        protected void RegisterViewsAndServices()
        {
            this.container.RegisterType<IEmployeesController, EmployeesController>();

            // TODO: should be a singleton;
            this.container.RegisterType<IEmployeeService, EmployeeService>();

            RegisterResource("pack://application:,,,/ViewModelComposition.Modules.Employees;component/Views/EmployeesDetailsView/EmployeeDetailsViewResourceDictionary.xaml");
            RegisterResource("pack://application:,,,/ViewModelComposition.Modules.Employees;component/Views/EmployeesListView/EmployeeListViewDictionary.xaml");
            RegisterResource("pack://application:,,,/ViewModelComposition.Modules.Employees;component/Views/EmployeesView/EmployeesViewResourceDictionary.xaml");
        }

        private void RegisterResource(string uriPack)
        {
            Application.Current.Resources.MergedDictionaries.Add(
                new ResourceDictionary { Source = new Uri(uriPack) }
                );
        }
    }
}
