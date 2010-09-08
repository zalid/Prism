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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using System.ComponentModel.Composition;
using RegionNavigation.Contacts.Views;

namespace RegionNavigation.Contacts
{
    [ModuleExport(typeof(ContactsModule))]
    public class ContactsModule : IModule
    {        
        [Import]
        public IRegionManager regionManager;

        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion("MainNavigationRegion", typeof(ContactsNavigationItemView));
            this.regionManager.RegisterViewWithRegion("MainContentRegion", typeof(ContactsView));
            this.regionManager.RegisterViewWithRegion("ContactDetailsRegion", typeof(ContactDetailView));
            this.regionManager.RegisterViewWithRegion("ContactDetailsRegion", typeof(ContactAvatarView));
        }
    }
}
