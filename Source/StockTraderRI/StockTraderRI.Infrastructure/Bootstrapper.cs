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
using Prism;
using Prism.Interfaces;
using Prism.Interfaces.Logging;

namespace StockTraderRI.Infrastructure
{
    public class Bootstrapper
    {
        protected IUnityContainer Container { get; set; }
        private IPrismLogger _logger;

        public Bootstrapper(IPrismLogger logger)
        {
            _logger = logger;
        }

        public void Initialize(IUnityContainerConfigurator configurator)
        {
            InitializeContainer(configurator);
            ShowShellView();
            InitializeModules();
        }

        protected void InitializeContainer(IUnityContainerConfigurator configurator)
        {
            _logger.Log("Container initialization started.", Category.Debug, Priority.Low);
            UnityContainer unityContainer = new UnityContainer();
            unityContainer.RegisterInstance<IPrismLogger>(_logger);
            Container = unityContainer;

            configurator.Configure(Container);
            PrismContainerProvider.Provider = Container.Resolve<IPrismContainer>();
        }

        protected void InitializeModules()
        {
            IModuleEnumerator moduleEnumerator = Container.Resolve<IModuleEnumerator>();
            Container.Resolve<IModuleLoaderService>().Initialize(moduleEnumerator.GetStartupLoadedModules());
        }

        private void ShowShellView()
        {
            ShellPresenter shellPresenter = Container.Resolve<ShellPresenter>();

            Container.RegisterInstance<IRegionManagerService>(shellPresenter.View.RegionManagerService);

            shellPresenter.View.ShowView();
        }
    }
}