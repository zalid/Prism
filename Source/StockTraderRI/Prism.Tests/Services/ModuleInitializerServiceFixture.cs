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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Interfaces;
using Prism.Services;
using Prism.Tests.Mocks;

namespace Prism.Tests.Services
{
    /// <summary>
    /// Summary description for ModuleInitializerServiceFixture
    /// </summary>
    [TestClass]
    public class ModuleInitializerServiceFixture
    {
        [TestMethod]
        public void ShouldResolveModuleAndInitializeSingleModule()
        {
            IPrismContainer container = new MockPrismContainer();
            var service = new ModuleInitializerService(container, null);
            FirstTestModule.wasInitializedOnce = false;
            service.Initialize(new Type[] {typeof (FirstTestModule)});
            Assert.IsTrue(FirstTestModule.wasInitializedOnce);
        }

        [TestMethod]
        public void ShouldResolveAndInitializeMultipleModules()
        {
            IPrismContainer container = new MockPrismContainer();
            var service = new ModuleInitializerService(container, null);
            FirstTestModule.wasInitializedOnce = false;
            SecondTestModule.wasInitializedOnce = false;
            service.Initialize(new Type[] {typeof (FirstTestModule), typeof (SecondTestModule)});
            Assert.IsTrue(FirstTestModule.wasInitializedOnce);
            Assert.IsTrue(SecondTestModule.wasInitializedOnce);
        }

        [TestMethod]
        public void ShouldInitializeModulesInOrderProvided()
        {
            IPrismContainer container = new MockPrismContainer();            
            var service = new ModuleInitializerService(container, null);

            ModuleLoadTracker.ModuleLoadStack.Clear();
            service.Initialize(new Type[] { typeof(SecondTestModule), typeof(FirstTestModule) });
            Assert.AreEqual<Type>(typeof(FirstTestModule), ModuleLoadTracker.ModuleLoadStack.Pop());
            Assert.AreEqual<Type>(typeof(SecondTestModule), ModuleLoadTracker.ModuleLoadStack.Pop());
        }

        [TestMethod]
        public void ShouldLogModuleInitializeErrorsAndContinueLoading()
        {
            IPrismContainer container = new MockPrismContainer();
            MockLogger logger = new MockLogger();
            var service = new ModuleInitializerService(container, logger);

            service.Initialize(new Type[] { typeof(InvalidModule) });
            StringAssert.StartsWith(logger.LastMessage,"Unable to cast object");
        }

        [TestMethod]
        public void ShouldLogModuleInitializationError()
        {
            IPrismContainer container = new MockPrismContainer();
            MockLogger logger = new MockLogger();
            var service = new ModuleInitializerService(container, logger);
            ExceptionThrowingModule.wasInitializedOnce = false;
            service.Initialize(new Type[] { typeof(ExceptionThrowingModule) });
            Assert.AreEqual<string>("Intialization can't be performed",logger.LastMessage);
        }


        public static class ModuleLoadTracker
        {
            public static readonly Stack<Type> ModuleLoadStack = new Stack<Type>();

        }
        public class FirstTestModule : IModule
        {
            public static bool wasInitializedOnce = false;
            
            public void Initialize()
            {
                wasInitializedOnce = true;
                ModuleLoadTracker.ModuleLoadStack.Push(this.GetType());
            }
        }

        public class SecondTestModule : IModule
        {
            public static bool wasInitializedOnce = false;
            public static long initializedOnTickCount;

            public void Initialize()
            {
                wasInitializedOnce = true;
                ModuleLoadTracker.ModuleLoadStack.Push(this.GetType());
            }
        }

        public class ExceptionThrowingModule : IModule
        {
            public static bool wasInitializedOnce = false;
            public static long initializedOnTickCount;

            public void Initialize()
            {
                throw new InvalidOperationException("Intialization can't be performed");
            }
        }

        public class InvalidModule
        {
            
        }
    }
}
