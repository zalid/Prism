//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System.ComponentModel.Composition;
using System.Windows.Controls;
using RegionNavigation.Email.ViewModels;

namespace RegionNavigation.Email.Views
{
    [Export]
    public partial class EmailsView : UserControl
    {
        public EmailsView()
        {
            InitializeComponent();
        }

        [Import]
        public EmailsViewModel ViewModel
        {
            get { return this.DataContext as EmailsViewModel; }
            set { this.DataContext = value; }
        }
    }
}
