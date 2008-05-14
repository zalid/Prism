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

using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Prism.Interfaces;
using Prism.Regions;
using Prism.Services;
using Prism.UnityContainerAdapter;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.Market;
using StockTraderRI.Modules.News;
using StockTraderRI.Modules.Position;
using StockTraderRI.Modules.Watch;

namespace StockTraderRI
{
    class StockTraderRIContainerConfigurator : IUnityContainerConfigurator
    {
        public void Configure(IUnityContainer container)
        {
            container.RegisterInstance<IUnityContainer>(container);
            container.RegisterType<IPrismContainer, UnityPrismContainer>(new ContainerControlledLifetimeManager());

            container.RegisterType<IModuleLoaderService, ModuleLoaderService>();
            container.RegisterType<IShellView, Shell>();
            container.RegisterType<IRegionManager, RegionManager>(new ContainerControlledLifetimeManager());

            StaticModuleEnumerator moduleEnumerator = new StaticModuleEnumerator();
            moduleEnumerator.AddModule(typeof(NewsModule));
            moduleEnumerator.AddModule(typeof(MarketModule));
            moduleEnumerator.AddModule(typeof(WatchModule), new[] { "MarketModule" });
            moduleEnumerator.AddModule(typeof(PositionModule), new[] { "MarketModule", "NewsModule" });
            container.RegisterInstance<IModuleEnumerator>(moduleEnumerator);

            RegionAdapterMappings mappings = new RegionAdapterMappings();
            mappings.RegisterMapping(typeof(ItemsControl), new ItemsControlRegionAdapter());
            mappings.RegisterMapping(typeof(ContentControl), new ContentControlRegionAdapter());

            container.RegisterInstance<RegionAdapterMappings>(mappings);
        }
    }
}
