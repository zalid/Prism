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
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.PresentationModels;

namespace StockTraderRI.Modules.Position.Orders
{
    /// <summary>
    /// Interaction logic for OrderCompositeView.xaml
    /// </summary>
    public partial class OrderCompositeView : UserControl, IOrderCompositeView
    {
        public OrderCompositeView()
        {
            InitializeComponent();
        }

        #region IOrderCompositeView Members

        public void SetDetailView(UIElement detailView)
        {
            CompositeExpander.Content = detailView;
            //DetailArea.Children.Add(detailView);
        }

        public void SetCommandView(UIElement commandView)
        {
            CompositeExpander.Header = commandView;
            //CommandArea.Children.Add(commandView);
        }


        public OrderCompositePresentationModel Model
        {
            set
            {
                DataContext = value;
            }
        }

        #endregion

        #region IActiveAware Members

        private bool _isActive = false;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    IsActiveChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler IsActiveChanged = delegate { };

        #endregion

    }
}
