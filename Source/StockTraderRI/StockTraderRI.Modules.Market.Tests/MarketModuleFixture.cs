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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Market.Tests.Mocks;
using Prism.Interfaces;

namespace StockTraderRI.Modules.Market.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MarketModuleFixture
    {

        [TestMethod]
        [DeploymentItem("Data/MarketHistory.xml", "Data")]
        public void CanInitModule()
        {
            IUnityContainer container = new UnityContainer();
            MarketModule module = new MarketModule(container);

            module.Initialize();

            Assert.IsNotNull(container.Resolve<IMarketHistoryService>());
            Assert.IsNotNull(container.Resolve<IMarketFeedService>());
            Assert.IsNotNull(container.Resolve<ITrendLineView>());
            Assert.IsNotNull(container.Resolve<ITrendLinePresenter>());


        }
    }
}
