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
using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;
using RegionNavigation.Contacts.ViewModels;

namespace RegionNavigation.Contacts.Views
{
    [Export]
    public partial class ContactsView : UserControl, INavigationAware
    {
        public ContactsView()
        {
            InitializeComponent();
        }

        [Import]
        public IRegionManager regionManager;

        [Import]
        public ContactsViewModel ViewModel
        {
            get
            {
                return (ContactsViewModel)this.DataContext;
            }
            set
            {
                this.DataContext = value;
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.UriQuery["Show"] == "Avatars")
            {
                regionManager.RequestNavigate("ContactDetailsRegion", new Uri("ContactAvatarView", UriKind.Relative), nr => { });
            }
            else
            {
                regionManager.RequestNavigate("ContactDetailsRegion", new Uri("ContactDetailView", UriKind.Relative), nr => { });
            }
        }

        public bool CanNavigateTo(NavigationContext navigationContext)
        {
            return true;
        }
    }
}
