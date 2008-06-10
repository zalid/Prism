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

using Castle.Core;
using Castle.Windsor;
using Prism.Interfaces;
using StockTraderRI.Modules.Watch.AddWatch;
using StockTraderRI.Modules.Watch.Services;
using StockTraderRI.Modules.Watch.WatchList;

namespace StockTraderRI.Modules.Watch
{
    public class WatchModule : IModule
    {
        private readonly IWindsorContainer _container;
        private readonly IRegionManager _regionManagerService;

        public WatchModule(IWindsorContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManagerService = regionManager;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewsAndServices();

            IWatchListPresentationModel watchListPresentationModel = _container.Resolve<IWatchListPresentationModel>();
            _regionManagerService.Regions["WatchRegion"].Add(watchListPresentationModel.View);
            IAddWatchPresenter addWatchPresenter = _container.Resolve<IAddWatchPresenter>();
            _regionManagerService.Regions["MainToolbarRegion"].Add(addWatchPresenter.View);
        }

        protected void RegisterViewsAndServices()
        {
            _container.AddComponentWithLifestyle<IWatchListService, WatchListService>(LifestyleType.Singleton);
            _container.AddComponentWithLifestyle<IWatchListView, WatchListView>(LifestyleType.Transient);
            _container.AddComponentWithLifestyle<IWatchListPresentationModel, WatchListPresentationModel>("blah", LifestyleType.Transient);
            _container.AddComponentWithLifestyle<IAddWatchView, AddWatchView>(LifestyleType.Transient);
            _container.AddComponentWithLifestyle<IAddWatchPresenter, AddWatchPresenter>(LifestyleType.Transient);
        }

        #endregion
    }
}
