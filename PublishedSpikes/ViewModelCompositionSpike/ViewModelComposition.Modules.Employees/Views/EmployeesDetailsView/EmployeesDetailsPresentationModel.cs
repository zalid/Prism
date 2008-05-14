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

using System.Collections.ObjectModel;
using Prism.Interfaces;
using Prism.Regions;
using Prism.Services;
using ViewModelComposition.Modules.Employees.BusinessEntities;

namespace ViewModelComposition.Modules.Employees.Views.EmployeesDetailsView
{
    public class EmployeesDetailsPresentationModel
    {
        public Employee SelectedEmployee { get; set; }

        public EmployeesDetailsPresentationModel()
        {
            EmployeeDetails = new ObservableCollection<object>();
            EmployeeDetails.Insert(0, new HeaderedEmployeeData());
            EmployeeDetails.Insert(1, new EmployeeAddressMapUrl());

            RegionManagerService = new RegionManagerService();
            RegionManagerService.Register("TabRegion", new ObservableCollectionRegion<object>(EmployeeDetails));
        }


        public void SetSelectedEmployee(Employee employee)
        {
            ((HeaderedEmployeeData)EmployeeDetails[0]).Employee = employee;
            ((EmployeeAddressMapUrl)EmployeeDetails[1]).Employee = employee;
        }

        public ObservableCollection<object> EmployeeDetails { get; private set; }

        public IRegionManagerService RegionManagerService { get; private set; }
    }
}