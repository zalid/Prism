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
using ViewModelComposition.Modules.Employees.BusinessEntities;

namespace ViewModelComposition.Modules.Employees
{
    using System;
    using Prism.Utility;
    using ViewModelComposition.Modules.Employees.Services;

    public class EmployeesListPresentationModel
    {
        private Employee _selectedEmployee;
        public event EventHandler<DataEventArgs<Employee>> EmployeeSelected = delegate { };

        public EmployeesListPresentationModel(IEmployeeService employeeService)
        {
            Employees = employeeService.RetrieveEmployees();
        }

        public ObservableCollection<Employee> Employees { get; private set; }

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                this.EmployeeSelected(this, new DataEventArgs<Employee>(value));
            }
        }
    }
}
