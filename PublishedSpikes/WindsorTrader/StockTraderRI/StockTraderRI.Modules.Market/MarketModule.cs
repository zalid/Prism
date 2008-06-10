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
using Castle.Core;
using Castle.Windsor;
using Prism.Interfaces;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Market.Services;
using StockTraderRI.Modules.Market.TrendLine;

namespace StockTraderRI.Modules.Market
{
    public class MarketModule : IModule
    {
        IWindsorContainer _container;

        public MarketModule(IWindsorContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewsAndServices();
        }

        protected void RegisterViewsAndServices()
        {
            _container.AddComponentWithLifestyle<IMarketHistoryService, MarketHistoryService>(LifestyleType.Transient);
            _container.AddComponentWithLifestyle<IMarketFeedService, MarketFeedService>(LifestyleType.Singleton);
            _container.AddComponentWithLifestyle<ITrendLineView, TrendLineView>(LifestyleType.Transient);
            _container.AddComponentWithLifestyle<ITrendLinePresenter, TrendLinePresenter>(LifestyleType.Transient);
        }

        #endregion
    }
}
