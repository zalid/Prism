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
        private readonly IWindsorContainer _container;
        private readonly IRegionManager _regionManager;

        public PositionModule(IWindsorContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        #region IModule Members

        public void Initialize()
        {
            RegisterViewsAndServices();

            IPositionSummaryPresentationModel presenter = _container.Resolve<IPositionSummaryPresentationModel>();
            IRegion mainRegion = _regionManager.Regions["MainRegion"];
            mainRegion.Add(presenter.View);

            IRegion mainToolbarRegion = _regionManager.Regions["MainToolbarRegion"];
            mainToolbarRegion.Add(new OrdersToolBar());
        }

        protected void RegisterViewsAndServices()
        {
            _container.AddComponentWithLifestyle<IAccountPositionService, AccountPositionService>(LifestyleType.Transient);
            _container.AddComponentWithLifestyle<IPositionSummaryView, PositionSummaryView>(LifestyleType.Transient);
            _container.AddComponentWithLifestyle<IPositionSummaryPresentationModel, PositionSummaryPresentationModel>(LifestyleType.Transient);
            _container.AddComponentWithLifestyle<IOrdersView, OrdersView>(LifestyleType.Transient);
            _container.AddComponentWithLifestyle<IOrdersPresenter, OrdersPresenter>(LifestyleType.Transient);
            _container.AddComponentWithLifestyle<IOrderDetailsView, OrderDetailsView>(LifestyleType.Transient);
            _container.AddComponentWithLifestyle<IOrderDetailsPresenter, OrderDetailsPresenter>(LifestyleType.Transient);
            _container.AddComponentWithLifestyle<IOrderCommandsView, OrderCommandsView>(LifestyleType.Transient);
            _container.AddComponentWithLifestyle<IOrderCompositeView, OrderCompositeView>(LifestyleType.Transient);
            _container.AddComponentWithLifestyle<IOrderCompositePresentationModel, OrderCompositePresentationModel>(LifestyleType.Transient);
            _container.AddComponentWithLifestyle<IOrdersController, OrdersController>(LifestyleType.Singleton);
            _container.AddComponentWithLifestyle<IOrdersService, XmlOrdersService>(LifestyleType.Transient);

        }

        #endregion
    }
}
