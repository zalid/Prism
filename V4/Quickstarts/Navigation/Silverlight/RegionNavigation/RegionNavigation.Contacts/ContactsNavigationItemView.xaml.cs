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
using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;

namespace RegionNavigation.Contacts
{
    [Export]
    public partial class ContactsNavigationItemView : UserControl
    {
        [Import]
        public IRegionManager regionManager;

        public ContactsNavigationItemView()
        {
            InitializeComponent();
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            this.regionManager.RequestNavigate("MainContentRegion", new Uri("ContactsView?Show=Details", UriKind.Relative), nr => { });
        }

        private void AvatarsButton_Click(object sender, RoutedEventArgs e)
        {
            this.regionManager.RequestNavigate("MainContentRegion", new Uri("ContactsView?Show=Avatars", UriKind.Relative), nr => { });
        }
    }
}
