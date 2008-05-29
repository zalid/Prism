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
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;
using Prism.Interfaces;
using Prism.Interfaces.Logging;
using Prism.Logging;
using Prism.Regions;
using Prism.Services;
using Prism.UnityContainerAdapter.Tests.Mocks;

namespace Prism.UnityContainerAdapter.Tests
{
    [TestClass]
    public class UnityPrismBootstrapperFixture
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
        public void ShouldCallCreateSell()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.CreateShellCalled);
        }

        [TestMethod]
        public void ShouldCallConfigureTypeMappings()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ConfigureContainerCalled);
        }

        [TestMethod]
        public void ShouldCallConfigureRegionAdapterMappings()
        {
            var bootstrapper = new DefaultBootstrapper();

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ConfigureRegionAdapterMappingsCalled);
        }

        [TestMethod]
        public void NullLoggerThrows()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.ReturnNullDefaultPrismLogger = true;

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
        public void NotOvewrittenGetModuleEnumeratorThrowsOnDefaultModuleInitialization()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.OverrideGetModuleEnumerator = false;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IModuleEnumerator");
        }

        [TestMethod]
        public void GetLoggerShouldHaveDefault()
        {
            var bootstrapper = new DefaultBootstrapper();
            Assert.IsNull(bootstrapper.DefaultPrismLogger);
            bootstrapper.Run();

            Assert.IsNotNull(bootstrapper.DefaultPrismLogger);
            Assert.IsInstanceOfType(bootstrapper.DefaultPrismLogger, typeof(TraceLogger));
        }

        [TestMethod]
        public void ShouldAssignRegionManagerToReturnedShell()
        {
            var bootstrapper = new DefaultBootstrapper();
            var shell = new DependencyObject();
            bootstrapper.CreateShellReturnValue = shell;

            Assert.IsNull(RegionManager.GetRegionManager(shell));

            bootstrapper.Run();

            Assert.IsNotNull(RegionManager.GetRegionManager(shell));
        }

        [TestMethod]
        public void ShouldNotFailIfReturnedNullShell()
        {
            var bootstrapper = new DefaultBootstrapper();
            bootstrapper.CreateShellReturnValue = null;
            bootstrapper.Run();
        }

        [TestMethod]
        public void NullModuleLoaderThrowsOnDefaultModuleInitialization()
        {
            var bootstrapper = new MockedBootstrapper();

            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleEnumerator), bootstrapper.ModuleEnumerator);
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleLoaderService), null);

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IModuleLoaderService");
        }

        [TestMethod]
        public void ShouldRegisterDefaultTypeMappings()
        {
            var bootstrapper = new MockedBootstrapper();

            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleEnumerator), bootstrapper.ModuleEnumerator);
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleLoaderService), new MockModuleLoaderService());

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.MockUnityContainer.Instances.ContainsKey(typeof(IPrismLogger)));
            Assert.AreSame(bootstrapper.Logger, bootstrapper.MockUnityContainer.Instances[typeof(IPrismLogger)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Instances.ContainsKey(typeof(IUnityContainer)));
            Assert.AreSame(bootstrapper.MockUnityContainer, bootstrapper.MockUnityContainer.Instances[typeof(IUnityContainer)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Types.ContainsKey(typeof(IPrismContainer)));
            Assert.AreEqual(typeof(UnityPrismContainer), bootstrapper.MockUnityContainer.Types[typeof(IPrismContainer)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Types.ContainsKey(typeof(IModuleLoaderService)));
            Assert.AreEqual(typeof(ModuleLoaderService), bootstrapper.MockUnityContainer.Types[typeof(IModuleLoaderService)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Types.ContainsKey(typeof(IRegionManager)));
            Assert.AreEqual(typeof(RegionManager), bootstrapper.MockUnityContainer.Types[typeof(IRegionManager)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Types.ContainsKey(typeof(IEventAggregator)));
            Assert.AreEqual(typeof(EventAggregator), bootstrapper.MockUnityContainer.Types[typeof(IEventAggregator)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Types.ContainsKey(typeof(RegionAdapterMappings)));
            Assert.AreEqual(typeof(RegionAdapterMappings), bootstrapper.MockUnityContainer.Types[typeof(RegionAdapterMappings)]);

            Assert.IsTrue(bootstrapper.MockUnityContainer.Instances.ContainsKey(typeof(IModuleEnumerator)));
            Assert.AreSame(bootstrapper.ModuleEnumerator, bootstrapper.MockUnityContainer.Instances[typeof(IModuleEnumerator)]);
        }

        [TestMethod]
        public void ShouldCallGetStartupLoadedModules()
        {
            var bootstrapper = new MockedBootstrapper();

            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleEnumerator), bootstrapper.ModuleEnumerator);
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleLoaderService), new MockModuleLoaderService());

            bootstrapper.Run();

            Assert.IsTrue(bootstrapper.ModuleEnumerator.GetStartupLoadedModulesCalled);
        }

        [TestMethod]
        public void ShouldCallInitializeOnModuleLoaderService()
        {
            var bootstrapper = new MockedBootstrapper();

            var moduleLoaderService = new MockModuleLoaderService();
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleEnumerator), new MockModuleEnumerator());
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleLoaderService), moduleLoaderService);

            bootstrapper.Run();

            Assert.IsTrue(moduleLoaderService.InitializeCalled);
        }

        [TestMethod]
        public void ShouldCallInitializeOnModuleLoaderServiceWithStartupModules()
        {
            var bootstrapper = new MockedBootstrapper();
            var moduleLoaderService = new MockModuleLoaderService();

            bootstrapper.ModuleEnumerator.StartupLoadedModules = new[] { new ModuleInfo("asm", "type", "name") };

            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleEnumerator), bootstrapper.ModuleEnumerator);
            bootstrapper.MockUnityContainer.ResolveBag.Add(typeof(IModuleLoaderService), moduleLoaderService);


            bootstrapper.Run();

            Assert.IsNotNull(moduleLoaderService.InitializeArgumentModuleInfos);
            Assert.AreEqual(1, moduleLoaderService.InitializeArgumentModuleInfos.Length);
            Assert.AreEqual("name", moduleLoaderService.InitializeArgumentModuleInfos[0].ModuleName);
        }

        [TestMethod]
        public void ReturningNullContainerThrows()
        {
            var bootstrapper = new MockedBootstrapper();
            bootstrapper.MockUnityContainer = null;

            AssertExceptionThrownOnRun(bootstrapper, typeof(InvalidOperationException), "IUnityContainer");
        }

        [TestMethod]
        public void ShouldCallTheMethodsInOrder()
        {
            var bootstrapper = new TestableOrderedBootstrapper();
            bootstrapper.Run();

            Assert.IsTrue(CompareOrder("PrismLogger", "CreateContainer", bootstrapper.OrderedMethodCallList) < 0);
            Assert.IsTrue(CompareOrder("CreateContainer", "ConfigureContainer", bootstrapper.OrderedMethodCallList) < 0);
            Assert.IsTrue(CompareOrder("ConfigureContainer", "GetModuleEnumerator", bootstrapper.OrderedMethodCallList) < 0);
            Assert.IsTrue(CompareOrder("GetModuleEnumerator", "ConfigureRegionAdapterMappings", bootstrapper.OrderedMethodCallList) < 0);
            Assert.IsTrue(CompareOrder("ConfigureRegionAdapterMappings", "CreateShell", bootstrapper.OrderedMethodCallList) < 0);
            Assert.IsTrue(CompareOrder("CreateShell", "InitializeModules", bootstrapper.OrderedMethodCallList) < 0);
        }

        [TestMethod]
        public void ShouldLogBootstrapperSteps()
        {
            var bootstrapper = new TestableOrderedBootstrapper();
            bootstrapper.Run();
            var messages = bootstrapper.Logger.Messages;

            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Creating Unity container")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Configuring container")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Configuring region adapters")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Creating shell")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Initializing modules")));
            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Bootstrapper sequence completed")));
        }

        [TestMethod]
        public void ShouldNotRegisterDefaultServicesAndTypes()
        {
            var bootstrapper = new NonconfiguredBootstrapper();
            bootstrapper.Run(false);


            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(IEventAggregator)));
            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(IRegionManager)));
            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(RegionAdapterMappings)));
            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(IPrismContainer)));
            Assert.IsFalse(bootstrapper.HasRegisteredType(typeof(IModuleLoaderService)));
        }

        [TestMethod]
        public void ShoudLogRegisterTypeIfMissingMessage()
        {
            var bootstrapper = new TestableOrderedBootstrapper();
            bootstrapper.AddCustomTypeMappings = true;
            bootstrapper.Run();
            var messages = bootstrapper.Logger.Messages;

            Assert.IsNotNull(messages.FirstOrDefault(msg => msg.Contains("Type 'IRegionManager' was already registered by the application")));
        }

        private static int CompareOrder(string firstString, string secondString, IList<string> list)
        {
            return list.IndexOf(firstString).CompareTo(list.IndexOf(secondString));
        }

        private static void AssertExceptionThrownOnRun(UnityPrismBootstrapper bootstrapper, Type expectedExceptionType, string expectedExceptionMessageSubstring)
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
    }

    class NonconfiguredBootstrapper : UnityPrismBootstrapper
    {
        private MockUnityContainer mockContainer;

        protected override void InitializeModules()
        {
        }

        protected override IUnityContainer CreateContainer()
        {
            mockContainer = new MockUnityContainer();
            return mockContainer;
        }

        protected override DependencyObject CreateShell()
        {
            return null;
        }

        public bool HasRegisteredType(Type t)
        {
            return mockContainer.Types.ContainsKey(t);
        }
    }

    class DefaultBootstrapper : UnityPrismBootstrapper
    {
        public bool GetEnumeratorCalled;
        public bool GetLoggerCalled;
        public bool InitializeModulesCalled;
        public bool CreateShellCalled;
        public bool ReturnNullDefaultPrismLogger;
        public bool OverrideGetModuleEnumerator = true;
        public IModuleEnumerator ModuleEnumerator = new MockModuleEnumerator();
        public IPrismLogger DefaultPrismLogger;
        public RegionAdapterMappings DefaultRegionAdapterMappings;
        public DependencyObject CreateShellReturnValue;
        public bool ConfigureContainerCalled;
        public bool ConfigureRegionAdapterMappingsCalled;

        public IUnityContainer GetBaseContainer()
        {
            return base.Container;
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            ConfigureRegionAdapterMappingsCalled = true;
            var regionAdapterMappings = base.ConfigureRegionAdapterMappings();

            DefaultRegionAdapterMappings = regionAdapterMappings;

            return regionAdapterMappings;
        }

        protected override void ConfigureContainer()
        {
            ConfigureContainerCalled = true;
            base.ConfigureContainer();
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

        protected override IPrismLogger PrismLogger
        {
            get
            {
                GetLoggerCalled = true;
                if (ReturnNullDefaultPrismLogger)
                {
                    return null;
                }
                else
                {
                    DefaultPrismLogger = base.PrismLogger;
                    return DefaultPrismLogger;
                }
            }
        }

        protected override void InitializeModules()
        {
            InitializeModulesCalled = true;
            base.InitializeModules();
        }

        protected override DependencyObject CreateShell()
        {
            CreateShellCalled = true;

            return CreateShellReturnValue;
        }
    }

    class MockedBootstrapper : UnityPrismBootstrapper
    {
        public MockUnityContainer MockUnityContainer = new MockUnityContainer();
        public MockModuleEnumerator ModuleEnumerator = new MockModuleEnumerator();
        public MockPrismLogger Logger = new MockPrismLogger();

        protected override IUnityContainer CreateContainer()
        {
            return this.MockUnityContainer;
        }

        protected override IModuleEnumerator GetModuleEnumerator()
        {
            return ModuleEnumerator;
        }

        protected override IPrismLogger PrismLogger
        {
            get { return Logger; }
        }

        protected override DependencyObject CreateShell()
        {
            return null;
        }
    }

    class TestableOrderedBootstrapper : UnityPrismBootstrapper
    {
        public IList<string> OrderedMethodCallList = new List<string>();
        public MockPrismLogger Logger = new MockPrismLogger();
        public bool AddCustomTypeMappings;

        protected override IUnityContainer CreateContainer()
        {
            OrderedMethodCallList.Add("CreateContainer");
            return base.CreateContainer();
        }

        protected override IPrismLogger PrismLogger
        {
            get
            {
                OrderedMethodCallList.Add("PrismLogger");
                return Logger;
            }
        }

        protected override IModuleEnumerator GetModuleEnumerator()
        {
            OrderedMethodCallList.Add("GetModuleEnumerator");
            return new MockModuleEnumerator();
        }

        protected override void ConfigureContainer()
        {
            OrderedMethodCallList.Add("ConfigureContainer");
            if (AddCustomTypeMappings)
            {
                RegisterTypeIfMissing<IRegionManager, MockRegionManager>(true);
            }
            base.ConfigureContainer();
        }

        protected override void InitializeModules()
        {
            OrderedMethodCallList.Add("InitializeModules");
            base.InitializeModules();
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            OrderedMethodCallList.Add("ConfigureRegionAdapterMappings");
            return base.ConfigureRegionAdapterMappings();
        }

        protected override DependencyObject CreateShell()
        {
            OrderedMethodCallList.Add("CreateShell");
            return null;
        }
    }
}

