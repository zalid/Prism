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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prism.Interfaces;

namespace ModuleB
{
    /// <summary>
    /// Interaction logic for ActivityView.xaml
    /// </summary>
    public partial class ActivityView : UserControl, IActivityView
    {
        private ActivityPresenter presenter;
        
        public ActivityView()
        {
            InitializeComponent();
        }

        public ActivityView(ActivityPresenter presenter) : this()
        {
            this.presenter = presenter;
            presenter.View = this;
        }

        #region IActivityView Members

        public void AddContent(string content)
        {
            this.ContentPanel.Children.Add(new TextBlock() { Text = content });
        }

        public string Title
        {
            set { this.ActivityLabel.Content = value; }
        }

        public string CustomerId
        {
            set { presenter.CustomerID = value; }
        }

        #endregion
    }
}
