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

using Commanding.Modules.Order.PresentationModels;
using Microsoft.Practices.Unity;
using Prism.Interfaces;

namespace Commanding.Modules.Order
{
    public class OrderModule : IModule
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;

        public OrderModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            OrdersEditorPresentationModel presentationModel = container.Resolve<OrdersEditorPresentationModel>();

            IRegion mainRegion = regionManager.GetRegion("MainRegion");
            mainRegion.Add(presentationModel.View);

            IRegion globalCommandsRegion = regionManager.GetRegion("GlobalCommandsRegion");
            globalCommandsRegion.Add(new OrdersToolBar());
        }
    }
}