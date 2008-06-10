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

using System.Collections.ObjectModel;
using Prism.Commands;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.PresentationModels;

namespace StockTraderRI.Modules.Position.PositionSummary
{
    public interface IPositionSummaryPresentationModel
    {
        IPositionSummaryView View { get; set; }
        ObservableCollection<PositionSummaryItem> PositionSummaryItems { get;  }
        DelegateCommand<string> BuyCommand { get; set; }
        DelegateCommand<string> SellCommand { get; set; }
    }
}