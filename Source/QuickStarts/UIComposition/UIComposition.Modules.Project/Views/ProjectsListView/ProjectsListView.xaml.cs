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

namespace UIComposition.Modules.Project
{
    using System.Windows.Controls;
    using Prism.Interfaces;
    using Prism.Utility;
    using UIComposition.Infrastructure;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for ProjectsListView.xaml
    /// </summary>
    public partial class ProjectsListView : UserControl, IProjectsListView
    {
        public ProjectsListView()
        {
            InitializeComponent();
        }

        public ObservableCollection<BusinessEntities.Project> Model
        {
            get { return this.DataContext as ObservableCollection<BusinessEntities.Project>; }
            set { this.DataContext = value; }
        }

        public IMetadataInfo GetMetadataInfo()
        {
            return new MetadataInfo() { Title = "Current Projects" };
        }
    }
}
