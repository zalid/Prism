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

using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.News.Controllers;

namespace StockTraderRI.Modules.News.Tests.Mocks
{
    internal class MockNewsController : INewsController
    {
        public bool CurrentItemWasCalled = false;
        public bool RunCalled;

        public bool ShowNewsReaderCalled { get; private set; }

        public void CurrentNewsArticleChanged(NewsArticle article)
        {
            CurrentItemWasCalled = true;
        }

        public void ShowNewsReader()
        {
            ShowNewsReaderCalled = true;
        }

        public void Run()
        {
            RunCalled = true;
        }
    }
}