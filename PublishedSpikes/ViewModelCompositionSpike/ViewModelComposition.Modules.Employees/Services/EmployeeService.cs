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

using ViewModelComposition.Modules.Employees.BusinessEntities;

namespace ViewModelComposition.Modules.Employees.Services
{
    using System.Collections.ObjectModel;

    public class EmployeeService : IEmployeeService
    {
        public ObservableCollection<Employee> RetrieveEmployees()
        {
            ObservableCollection<Employee> employees = new ObservableCollection<Employee>();

            employees.Add(new Employee(1) { FirstName = "John", LastName = "Smith", Phone = "+1 (425) 555-0101", Email = "john.smith@example.com", Address = "One Microsoft Way", City = "Redmond", State = "WA" });
            employees.Add(new Employee(2) { FirstName = "Bonnie", LastName = "Skelly", Phone = "+1 (425) 555-0105", Email = "bonnie.skelly@example.com", Address = "One Microsoft Way", City = "Redmond", State = "WA" });

            return employees;
        }
    }
}
