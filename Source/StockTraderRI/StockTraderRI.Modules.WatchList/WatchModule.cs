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
using Microsoft.Practices.Unity;
using Prism.Interfaces;
using StockTraderRI.Modules.Watch.AddWatch;
using StockTraderRI.Modules.Watch.Services;
using StockTraderRI.Modules.Watch.WatchList;

namespace StockTraderRI.Modules.Watch
{
    public class WatchModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManagerService;

        public WatchModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManagerService = regionManager;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewsAndServices();

            IWatchListPresenter watchListPresenter = _container.Resolve<IWatchListPresenter>();
            _regionManagerService.GetRegion("WatchRegion").Add((UIElement)watchListPresenter.View);
            IAddWatchPresenter addWatchPresenter = _container.Resolve<IAddWatchPresenter>();
            _regionManagerService.GetRegion("MainToolbarRegion").Add((UIElement)addWatchPresenter.View);
        }

        protected void RegisterViewsAndServices()
        {
            _container.RegisterType<IWatchListService, WatchListService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IWatchListView, WatchListView>();
            _container.RegisterType<IWatchListPresenter, WatchListPresenter>();
            _container.RegisterType<IAddWatchView, AddWatchView>();
            _container.RegisterType<IAddWatchPresenter, AddWatchPresenter>();
        }

        #endregion
    }
}
