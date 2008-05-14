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

using System.Windows;
using ViewModelComposition.Modules.Employees.BusinessEntities;

namespace ViewModelComposition.Modules.Employees.Views.EmployeesDetailsView
{
    public class HeaderedEmployeeData : DependencyObject
    {
        public HeaderedEmployeeData()
        {
            //TODO: We only wrapped this to get the tab header to show up correctly.
            HeaderInfo = "Employee Info";
        }


        public string HeaderInfo { get; set; }

        public Employee Employee
        {
            get { return (Employee)GetValue(EmployeeProperty); }
            set { SetValue(EmployeeProperty, value); }
        }

        public static readonly DependencyProperty EmployeeProperty =
            DependencyProperty.Register("Employee", typeof(Employee), typeof(HeaderedEmployeeData));


    }
}