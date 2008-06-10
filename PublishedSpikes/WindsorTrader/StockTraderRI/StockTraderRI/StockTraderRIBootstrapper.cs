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

using System.Windows;
using Prism.Interfaces;
using Prism.Interfaces.Logging;
using Prism.Services;
using Prism.WindsorContainerAdapter;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.Market;
using StockTraderRI.Modules.News;
using StockTraderRI.Modules.Position;
using StockTraderRI.Modules.Watch;

namespace StockTraderRI
{
    public class StockTraderRIBootstrapper : WindsorPrismBootstrapper
    {
        private readonly Log4NetPrismLogger _prismLogger = new Log4NetPrismLogger();

        protected override IModuleEnumerator GetModuleEnumerator()
        {
            return new StaticModuleEnumerator()
                .AddModule(typeof (NewsModule))
                .AddModule(typeof (MarketModule))
                .AddModule(typeof (WatchModule), "MarketModule")
                .AddModule(typeof(PositionModule), "MarketModule", "NewsModule");
        }

        protected override IPrismLogger PrismLogger
        {
            get { return _prismLogger; }
        }

        protected override void ConfigureContainer()
        {
            Container.AddComponent<IShellView, Shell>();
            Container.AddComponent<ShellPresenter>();
            Container.AddComponent<StockTraderRICommandProxy>();

            base.ConfigureContainer();
        }

        protected override DependencyObject CreateShell()
        {
            
            ShellPresenter presenter = Container.Resolve<ShellPresenter>();
            IShellView view = presenter.View;
            view.ShowView();
            return view as DependencyObject;
        }
    }
}
