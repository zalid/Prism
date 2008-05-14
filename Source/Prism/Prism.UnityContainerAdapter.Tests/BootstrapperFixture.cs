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

using System;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Interfaces;
using Prism.Interfaces.Logging;
using Prism.Regions;
using Prism.Services;
using Prism.UnityContainerAdapter.Tests.Mocks;

namespace Prism.UnityContainerAdapter.Tests
{
    [TestClass]
    public class BootstrapperFixture
    {
        [TestMethod]
        public void CanCreateConcreteBootstrapper()
        {
            new DefaultBootstrapper();
        }

        [TestMethod]
        public void CanRunBootstrapper()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();
        }

        [TestMethod]
        public void ShouldInitializeContainer()
        {
            var bootstrapper = new DefaultBootstrapper();
            var container = bootstrapper.GetBaseContainer();

            Assert.IsNull(container);

            bootstrapper.Run();

            container = bootstrapper.GetBaseContainer();

            Assert.IsNotNull(container);
            Assert.IsInstanceOfType(container, typeof(UnityContainer));
        }

        [TestMethod]
        public void ShouldCallInitializeModules()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.InitializeModulesCalled);
        }

        [TestMethod]
        public void ShouldRegisterDefaultMappings()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();

            Assert.IsNotNull(bootstrapper.DefaultRegionAdapterMappings);
            Assert.IsNotNull(bootstrapper.DefaultRegionAdapterMappings.GetMapping(typeof(ItemsControl)));
            Assert.IsNotNull(bootstrapper.DefaultRegionAdapterMappings.GetMapping(typeof(ContentControl)));
        }

        [TestMethod]
        public void ShouldCallGetLogger()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.GetLoggerCalled);
        }

        [TestMethod]
        public void ShouldCallGetEnumerator()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.GetEnumeratorCalled);
        }

        [TestMethod]
        public void GetModuleLoaderServiceShouldHaveDefault()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.Run();

            Assert.IsNotNull(bootstrapper.DefaultModuleLoaderService);
            Assert.IsInstanceOfType(bootstrapper.DefaultModuleLoaderService, typeof(ModuleLoaderService));
        }

        [TestMethod]
        public void NullLoggerThrows()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.PrismLogger = null;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IPrismLogger");
        }

        [TestMethod]
        public void NullModuleEnumeratorThrowsOnDefaultModuleInitialization()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.ModuleEnumerator = null;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IModuleEnumerator");
        }

        [TestMethod]
        public void NullModuleLoaderThrowsOnDefaultModuleInitialization()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.ReturnNullModuleLoaderService = true;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IModuleLoaderService");
        }

        [TestMethod]
        public void NotOvewrittenGetModuleEnumeratorThrowsOnDefaultModuleInitialization()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.OverrideGetModuleEnumerator = false;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "GetModuleEnumerator");
        }

        [TestMethod]
        public void ShouldRegisterUnityContainer()
        {
            var bootstrapper = new MockedBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.MockUnityContainer.Instances.ContainsKey(typeof(IUnityContainer)));
            Assert.AreSame(bootstrapper.MockUnityContainer, bootstrapper.MockUnityContainer.Instances[typeof(IUnityContainer)]);
        }

        [TestMethod]
        public void ShouldRegisterPrismContainer()
        {
            var bootstrapper = new MockedBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.MockUnityContainer.Types.ContainsKey(typeof(IPrismContainer)));
            Assert.IsTrue(typeof(UnityPrismContainer).IsAssignableFrom(bootstrapper.MockUnityContainer.Types[typeof(IPrismContainer)]));
        }

        [TestMethod]
        public void ShouldRegisterDefaultServices()
        {
            var bootstrapper = new MockedBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.MockUnityContainer.Instances.ContainsKey(typeof(IModuleLoaderService)));
            Assert.AreSame(bootstrapper.ModuleLoaderService, bootstrapper.MockUnityContainer.Instances[typeof(IModuleLoaderService)]);
            Assert.IsTrue(bootstrapper.MockUnityContainer.Instances.ContainsKey(typeof(IRegionManager)));
            Assert.AreSame(bootstrapper.RegionManager, bootstrapper.MockUnityContainer.Instances[typeof(IRegionManager)]);
            Assert.IsTrue(bootstrapper.MockUnityContainer.Types.ContainsKey(typeof(IEventAggregator)));
        }

        [TestMethod]
        public void ShouldRegisterModuleEnumerator()
        {
            var bootstrapper = new MockedBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.MockUnityContainer.Instances.ContainsKey(typeof(IModuleEnumerator)));
        }

        [TestMethod]
        public void ShouldCallGetStartupLoadedModules()
        {
            var bootstrapper = new MockedBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ModuleEnumerator.GetStartupLoadedModulesCalled);
        }

        [TestMethod]
        public void ShouldRegisterLogger()
        {
            var bootstrapper = new MockedBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.MockUnityContainer.Instances.ContainsKey(typeof(IPrismLogger)));
        }

        [TestMethod]
        public void ShouldCallInitializeOnModuleLoaderService()
        {
            var bootstrapper = new MockedBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ModuleLoaderService.InitializeCalled);
        }

        [TestMethod]
        public void ShouldCallInitializeOnModuleLoaderServiceWithStartupModules()
        {
            var bootstrapper = new MockedBootstrapper();

            bootstrapper.ModuleEnumerator.StartupLoadedModules = new[] { new ModuleInfo("asm", "type", "name") };

            bootstrapper.Run();

            Assert.IsNotNull(bootstrapper.ModuleLoaderService.InitializeArgumentModuleInfos);
            Assert.AreEqual(1, bootstrapper.ModuleLoaderService.InitializeArgumentModuleInfos.Length);
            Assert.AreEqual("name", bootstrapper.ModuleLoaderService.InitializeArgumentModuleInfos[0].ModuleName);
        }

        [TestMethod]
        public void GetRegionManagerShouldHaveDefault()
        {
            var bootstrapper = new DefaultBootstrapper();
            Assert.IsNull(bootstrapper.DefaultRegionManager);
            bootstrapper.Run();

            Assert.IsNotNull(bootstrapper.DefaultRegionManager);
            Assert.IsInstanceOfType(bootstrapper.DefaultRegionManager, typeof(RegionManager));
        }

        [TestMethod]
        public void ReturningNullContainerThrows()
        {
            var bootstrapper = new MockedBootstrapper();
            bootstrapper.MockUnityContainer = null;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IUnityContainer");
        }


        private static void AssertExceptionThrownOnRun(Bootstrapper bootstrapper, Type expectedExceptionType, string expectedExceptionMessageSubstring)
        {
            bool exceptionThrown = false;
            try
            {
                bootstrapper.Run();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(expectedExceptionType, ex.GetType());
                StringAssert.Contains(ex.Message, expectedExceptionMessageSubstring);
                exceptionThrown = true;
            }
            if (!exceptionThrown)
            {
                Assert.Fail("Exception not thrown.");
            }
        }

        /* Should call several overridable template methods
         * Logger Logs steps
         * Shell
         */
    }


    class DefaultBootstrapper : Bootstrapper
    {
        public bool GetEnumeratorCalled;
        public bool GetLoggerCalled;
        public bool InitializeModulesCalled;
        public IModuleEnumerator ModuleEnumerator = new MockModuleEnumerator();
        public bool ReturnNullModuleLoaderService;
        public bool OverrideGetModuleEnumerator = true;
        public IPrismLogger PrismLogger = new MockPrismLogger();
        public IModuleLoaderService DefaultModuleLoaderService;
        public RegionAdapterMappings DefaultRegionAdapterMappings;
        public IRegionManager DefaultRegionManager;

        public IUnityContainer GetBaseContainer()
        {
            return base.Container;
        }

        protected override RegionAdapterMappings GetRegionAdapterMappings()
        {
            DefaultRegionAdapterMappings = base.GetRegionAdapterMappings();
            return DefaultRegionAdapterMappings;
        }

        protected override IModuleLoaderService GetModuleLoaderService()
        {
            if (ReturnNullModuleLoaderService)
            {
                return null;
            }
            else
            {
                DefaultModuleLoaderService = base.GetModuleLoaderService();
                return DefaultModuleLoaderService;
            }
        }

        protected override IModuleEnumerator GetModuleEnumerator()
        {
            GetEnumeratorCalled = true;
            if (OverrideGetModuleEnumerator)
            {
                return ModuleEnumerator;
            }
            else
            {
                return base.GetModuleEnumerator();
            }
        }

        protected override IPrismLogger GetLogger()
        {
            GetLoggerCalled = true;
            return PrismLogger;
        }

        protected override void InitializeModules()
        {
            InitializeModulesCalled = true;
            base.InitializeModules();
        }

        protected override IRegionManager GetRegionManager()
        {
            DefaultRegionManager = base.GetRegionManager();
            return DefaultRegionManager;
        }
    }

    class MockedBootstrapper : Bootstrapper
    {
        public MockUnityContainer MockUnityContainer = new MockUnityContainer();
        public MockModuleEnumerator ModuleEnumerator = new MockModuleEnumerator();
        public MockModuleLoaderService ModuleLoaderService = new MockModuleLoaderService();
        public IRegionManager RegionManager = new MockRegionManager();

        protected override IUnityContainer CreateContainer()
        {
            return this.MockUnityContainer;
        }

        protected override IModuleEnumerator GetModuleEnumerator()
        {
            return ModuleEnumerator;
        }

        protected override IModuleLoaderService GetModuleLoaderService()
        {
            return ModuleLoaderService;
        }

        protected override IPrismLogger GetLogger()
        {
            return new MockPrismLogger();
        }

        protected override IRegionManager GetRegionManager()
        {
            return RegionManager;
        }
    }
}