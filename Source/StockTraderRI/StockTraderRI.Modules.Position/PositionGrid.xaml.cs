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
using System.ComponentModel;
using Prism;
using Prism.Regions;
using StockTraderRI.Modules.Position.PresentationModels;
using StockTraderRI.Infrastructure.Models;
using Prism.Utility;

namespace StockTraderRI.Modules.Position
{
    /// <summary>
    /// Interaction logic for PositionGrid.xaml
    /// </summary>
    public partial class PositionGrid : UserControl
    {
        public event EventHandler<DataEventArgs<string>> PositionSelected = delegate { };

        public PositionGrid()
        {
            InitializeComponent();
        }

        private void _positionTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                PositionSummaryItem currentPosition = e.AddedItems[0] as PositionSummaryItem;
                if (currentPosition != null)
                    PositionSelected(this, new DataEventArgs<string>(currentPosition.TickerSymbol));
            }
        }

    }
}
