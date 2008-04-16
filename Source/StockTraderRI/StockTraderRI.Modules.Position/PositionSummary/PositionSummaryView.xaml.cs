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
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Infrastructure.PresentationModels;
using StockTraderRI.Modules.Position;
using System.Collections.ObjectModel;
using StockTraderRI.Modules.Position.PresentationModels;
using Prism.Utility;
using StockTraderRI.Modules.Position.Interfaces;

namespace StockTraderRI.Modules.Position.PositionSummary
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PositionSummaryView : UserControl, IPositionSummaryView
    {
        PositionSummaryPresentationModel _model;

        public PositionSummaryView()
        {
            InitializeComponent();

            positionGrid.PositionSelected += new EventHandler<DataEventArgs<string>>(positionGrid_PositionSelected);
        }


        private void positionGrid_NewsClicked(object sender, DataEventArgs<string> e)
        {
            NewsRequested(this, e);
        }

        void positionGrid_PositionSelected(object sender, DataEventArgs<string> e)
        {
            TickerSymbolSelected(this, e);
        }

        #region IPositionSummaryView Members

        public event EventHandler<DataEventArgs<string>> NewsRequested = delegate { };

        public event EventHandler<DataEventArgs<string>> TickerSymbolSelected = delegate { };

        public PositionSummaryPresentationModel Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                pieChart.ItemsSource = _model.Data;
                DataContext = _model;
            }
        }

        public void ShowTrendLine(ITrendLineView view)
        {
            lineChartPanel.Children.Clear();
            lineChartPanel.Children.Add((UIElement)view);
        }

        #endregion
    }
}
