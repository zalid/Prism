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
using System.Windows.Controls;
using Prism.Commands;
using Prism.Utility;
using StockTraderRI.Infrastructure;
using System.ComponentModel;
using StockTraderRI.Infrastructure.Models;
using System.Windows.Data;
using System.Windows.Input;

namespace StockTraderRI.Modules.News.PresentationModels
{
    public class ArticlePresentationModel : INotifyPropertyChanged
    {
        private ICollectionView _articles;
        
        public ArticlePresentationModel()
        {
        }
        
        public ICollectionView Articles
        {
            get { return _articles; }
            set
            {
                if (_articles != value)
                {
                    _articles = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Articles"));
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion    
    }
}
