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
using StockTraderRI.Infrastructure.PresentationModels;
using System.ComponentModel;
using StockTraderRI.Infrastructure.Interfaces;

namespace StockTraderRI.Modules.Market.TrendLine
{
    /// <summary>
    /// Interaction logic for TrendLineView.xaml
    /// </summary>
    public partial class TrendLineView : UserControl, ITrendLineView
    {
        public TrendLineView()
        {
            InitializeComponent();
        }

        #region ITrendLineView Members

        public void UpdateLineChart(MarketHistoryCollection historyCollection)
        {
            lineChart.ItemsSource = historyCollection;
            
        }

        public void SetChartTitle(string chartTitle)
        {
            lineChart.Title = chartTitle;
        }

        #endregion
    }


}
