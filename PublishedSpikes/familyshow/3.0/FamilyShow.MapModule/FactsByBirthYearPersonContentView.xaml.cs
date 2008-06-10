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
using FamilyShow.Infrastructure;

namespace FamilyShow.MapModule
{
    /// <summary>
    /// Interaction logic for FactsByBirthYearPersonContentView.xaml
    /// </summary>
    public partial class FactsByBirthYearPersonContentView : UserControl, IPersonContentView
    {
        public FactsByBirthYearPersonContentView()
        {
            InitializeComponent();
        }

        public FactsByBirthYearPersonContentView(FactsByBirthYearPersonContentPresenter presenter): this()
        {
            this.DataContext = presenter;

            this.contentFrame.NavigationFailed += contentFrame_NavigationFailed;
        }

        void contentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            //Do nothing
        }

        public object Clone()
        {
            FactsByBirthYearPersonContentView newView = new FactsByBirthYearPersonContentView(this.DataContext as FactsByBirthYearPersonContentPresenter);
            return newView;
        }
    }
}
