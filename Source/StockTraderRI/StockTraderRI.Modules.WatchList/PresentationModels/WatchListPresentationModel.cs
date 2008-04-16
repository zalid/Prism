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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Prism.Utility;

namespace StockTraderRI.Modules.Watch.PresentationModels
{
    public class WatchListPresentationModel : INotifyPropertyChanged, IHeaderInfoProvider<string>
    {
        private IList<WatchItem> _watchListItems;

        public IList<WatchItem> WatchListItems
        {
            get { return _watchListItems; }
            set
            {
                if (_watchListItems != value)
                {
                    _watchListItems = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("WatchListItems"));
                }
            }
        }

        public string HeaderInfo { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion
    }
}
