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
using System.Windows;

namespace ViewModelComposition.Modules.Project
{
    using Microsoft.Practices.Unity;
    using Prism.Interfaces;
    using ViewModelComposition.Infrastructure;
    using ViewModelComposition.Modules.Project.Services;

    public class ProjectModule : IModule
    {
        private IUnityContainer container;
        private IRegionManagerService regionManagerService;

        public ProjectModule(IUnityContainer container, IRegionManagerService regionManagerService)
        {
            this.container = container;
            this.regionManagerService = regionManagerService;
        }

        public void Initialize()
        {
            this.RegisterViewsAndServices();
        }

        protected void RegisterViewsAndServices()
        {
            // TODO: Should be singleton
            this.container.RegisterType<IProjectService, ProjectService>();


            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("pack://application:,,,/ViewModelComposition.Modules.Project;component/Views/ProjectsListView/ProjectResourceDictionary.xaml");
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }
    }
}