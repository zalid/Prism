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
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Position.Controllers;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Modules.Position.PositionSummary;
using StockTraderRI.Modules.Position.Services;

namespace StockTraderRI.Modules.Position
{
    public class PositionModule : IModule
    {
        private IUnityContainer _container;
        private IOrdersController _ordersController;
        private IRegionManager _regionManagerService;

        public PositionModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManagerService = regionManager;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewsAndServices();

            IPositionSummaryPresenter presenter = _container.Resolve<IPositionSummaryPresenter>();
            IRegion mainRegion = _regionManagerService.GetRegion("MainRegion");
            mainRegion.Add((UIElement)presenter.View);

            _ordersController = _container.Resolve<IOrdersController>();

            IRegion mainToolbarRegion = _regionManagerService.GetRegion("MainToolbarRegion");
            mainToolbarRegion.Add(new OrdersToolBar());
        }

        protected void RegisterViewsAndServices()
        {
            _container.RegisterType<IAccountPositionService, AccountPositionService>();
            _container.RegisterType<IPositionSummaryView, PositionSummaryView>();
            _container.RegisterType<IPositionSummaryPresenter, PositionSummaryPresenter>();
            _container.RegisterType<IOrdersView, OrdersView>();
            _container.RegisterType<IOrdersPresenter, OrdersPresenter>();
            _container.RegisterType<IOrderDetailsView, OrderDetailsView>();
            _container.RegisterType<IOrderDetailsPresenter, OrderDetailsPresenter>();
            _container.RegisterType<IOrderCommandsView, OrderCommandsView>();
            _container.RegisterType<IOrderCompositeView, OrderCompositeView>();
            _container.RegisterType<IOrderCompositePresenter, OrderCompositePresenter>();
            _container.RegisterType<IOrdersController, OrdersController>();
            _container.RegisterType<IOrdersService, XmlOrdersService>();

        }

        #endregion
    }
}
