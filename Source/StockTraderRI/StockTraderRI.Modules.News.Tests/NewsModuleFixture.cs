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

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Interfaces;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.News.Article;
using StockTraderRI.Modules.News.Controllers;
using StockTraderRI.Modules.News.Tests.Mocks;

namespace StockTraderRI.Modules.News.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class NewsModuleFixture
    {
        [TestMethod]
        [DeploymentItem("Data/News.xml", "Data")]
        public void NewsModuleRegistersNewsViewAndNewsFeedService()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IRegionManager, MockRegionManager>();
            container.RegisterInstance<IUnityContainer>(container);
            TestableNewsModule newsModule = new TestableNewsModule(container);

            newsModule.InvokeRegisterViewsAndServices();

            Assert.IsNotNull(container.Resolve<IArticleView>());
            Assert.IsNotNull(container.Resolve<INewsFeedService>());
            Assert.IsNotNull(container.Resolve<INewsController>());
            Assert.IsNotNull(container.Resolve<IArticlePresenter>());
            Assert.IsNotNull(container.Resolve<INewsReaderPresenter>());
            Assert.IsNotNull(container.Resolve<INewsReaderView>());
        }

        internal class TestableNewsModule : NewsModule
        {
            public TestableNewsModule(IUnityContainer container)
                : base(container)
            {

            }

            public void InvokeRegisterViewsAndServices()
            {
                base.RegisterViewsAndServices();
            }
        }
    }
}
