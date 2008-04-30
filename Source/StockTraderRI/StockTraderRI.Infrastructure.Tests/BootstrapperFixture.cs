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

using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Interfaces;
using Prism.Interfaces.Logging;
using StockTraderRI.Infrastructure.Tests.Mocks;

namespace StockTraderRI.Infrastructure.Tests
{
    /// <summary>
    /// Summary description for BootstrapperFixture
    /// </summary>
    [TestClass]
    public class BootstrapperFixture
    {
        [TestMethod]
        public void ShouldLogStartOfContainerInitializeSequence()
        {
            MockPrismLogger mockPrismLogger = new MockPrismLogger();
            MockContainerConfigurator containerConfigurator = new MockContainerConfigurator();
            ModuleInfo moduleInfo = new ModuleInfo(typeof(MockModule).Assembly.Location, typeof(MockModule).FullName, "MockModule");
            containerConfigurator.MockModuleEnumerator.Modules.Add(moduleInfo);

            TestableBootstrapper bs = new TestableBootstrapper(mockPrismLogger);

            Assert.AreEqual<int>(0, mockPrismLogger.Messages.Count);
            bs.Initialize(containerConfigurator);
            Assert.AreEqual<int>(1, mockPrismLogger.Messages.Count);
            Assert.IsTrue(mockPrismLogger.Messages.Any(
                              m => m.Equals("Container initialization started.")
                              ));
        }

        [TestMethod]
        public void ShouldRegisterIPrismLoggerWithContainer()
        {
            MockPrismLogger mockPrismLogger = new MockPrismLogger();
            MockContainerConfigurator containerConfigurator = new MockContainerConfigurator();

            TestableBootstrapper bs = new TestableBootstrapper(mockPrismLogger);
            bs.Initialize(containerConfigurator);

            Assert.IsNotNull(bs.GetContainer().Resolve<IPrismLogger>());
        }

        [TestMethod]
        public void ShouldResolveModuleTypesFromModuleEnumeratorService()
        {
            MockPrismLogger mockPrismLogger = new MockPrismLogger();
            MockContainerConfigurator containerConfigurator = new MockContainerConfigurator();
            ModuleInfo moduleInfo = new ModuleInfo(typeof(MockModule).Assembly.Location, typeof(MockModule).FullName, "MockModule");
            containerConfigurator.MockModuleEnumerator.Modules.Add(moduleInfo);

            TestableBootstrapper bs = new TestableBootstrapper(mockPrismLogger);
            bs.Initialize(containerConfigurator);

            Assert.IsTrue(containerConfigurator.MockModuleEnumerator.GetStartupLoadedModulesCalled);
        }

        [TestMethod]
        public void ShouldShowShellView()
        {
            MockPrismLogger mockPrismLogger = new MockPrismLogger();
            MockContainerConfigurator containerConfigurator = new MockContainerConfigurator();
            ModuleInfo moduleInfo = new ModuleInfo(typeof(MockModule).Assembly.Location, typeof(MockModule).FullName, "MockModule");
            containerConfigurator.MockModuleEnumerator.Modules.Add(moduleInfo);

            TestableBootstrapper bs = new TestableBootstrapper(mockPrismLogger);
            bs.Initialize(containerConfigurator);

            Assert.IsTrue(containerConfigurator.MockShellView.ShowCalled);
        }

        [TestMethod]
        public void ShouldRegisterRegionManagerServiceWithContainer()
        {
            MockPrismLogger mockPrismLogger = new MockPrismLogger();
            MockContainerConfigurator containerConfigurator = new MockContainerConfigurator();

            TestableBootstrapper bs = new TestableBootstrapper(mockPrismLogger);
            bs.Initialize(containerConfigurator);

            Assert.IsNotNull(bs.GetContainer().Resolve<IRegionManagerService>());
        }

    }

    internal class TestableBootstrapper : Bootstrapper
    {

        public TestableBootstrapper(IPrismLogger logger)
            : base(logger)
        {
        }

        public IUnityContainer GetContainer()
        {
            return base.Container;
        }
    }
}
