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

using Microsoft.Practices.Composite;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.UnityExtensions;
using Microsoft.Practices.Composite.Wpf.Regions;
using Microsoft.Practices.Unity;

namespace StockTraderRI.Infrastructure.Tests.Mocks
{
    internal class MockContainerConfigurator : IUnityContainerConfigurator
    {
        public MockModuleEnumerator MockModuleEnumerator { get; set; }
        public MockShellView MockShellView { get; set; }

        public MockContainerConfigurator()
        {
            MockModuleEnumerator = new MockModuleEnumerator();
            MockShellView = new MockShellView();
        }
        public void Configure(IUnityContainer container)
        {
            container.RegisterInstance<IUnityContainer>(container);
            container.RegisterType<IContainerFacade, UnityContainerAdapter>(new ContainerControlledLifetimeManager());

            container.RegisterInstance<IModuleEnumerator>(MockModuleEnumerator);
            container.RegisterType<IModuleLoader, MockModuleLoader>();

            container.RegisterInstance<IShellView>(MockShellView);
            container.RegisterType<IRegionManager, RegionManager>();
        }
    }
}