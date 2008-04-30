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
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Exceptions;
using Prism.Interfaces;
using Prism.Interfaces.Logging;
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullContainerThrows()
        {
            ModuleLoaderService loader = new ModuleLoaderService(null, new MockLogger());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullLoggerThrows()
        {
            ModuleLoaderService loader = new ModuleLoaderService(new MockPrismContainer(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ModuleLoadException))]
        public void InitializationExceptionsAreWrapped()
        {
            Assembly asm = CompilerHelper.CompileFileAndLoadAssembly("Prism.Tests.Mocks.Modules.MockModuleThrowingException.cs",
                @".\MocksModulesThwrowing\MockModuleThrowingException.dll");

            ModuleLoaderService loader = new ModuleLoaderService(new MockPrismContainer(), new MockLogger());

            ModuleInfo info = new ModuleInfo(asm.CodeBase.Replace(@"file:///", ""),
                           "Prism.Tests.Mocks.Modules.MockModuleThrowingException", "MockModuleThrowingException");

            loader.Initialize(new[] { info });
        }

        [TestMethod]
        [ExpectedException(typeof(CyclicDependencyFoundException))]
        public void FailWhenLoadingModulesWithCyclicDependencies()
        {
            List<string> assemblies = new List<string>();

            // Create several modules with this dependency graph (X->Y meaning Y depends on X)
            // 1->2, 2->3, 3->4, 4->5, 4->2
            assemblies.Add(CompilerHelper.GenerateDynamicModule("Module1", "Module1"));
            assemblies.Add(CompilerHelper.GenerateDynamicModule("Module2", "Module2", "Module1", "Module4"));
            assemblies.Add(CompilerHelper.GenerateDynamicModule("Module3", "Module3", "Module2"));
            assemblies.Add(CompilerHelper.GenerateDynamicModule("Module4", "Module4", "Module3"));
            assemblies.Add(CompilerHelper.GenerateDynamicModule("Module5", "Module5", "Module4"));

            List<ModuleInfo> modules = new List<ModuleInfo>();
            modules.Add(new ModuleInfo(assemblies[0], "Module1.TestModules.Module1Class", "Module1"));
            modules.Add(new ModuleInfo(assemblies[1], "Module2.TestModules.Module2Class", "Module2", "Module1", "Module4"));
            modules.Add(new ModuleInfo(assemblies[2], "Module3.TestModules.Module3Class", "Module3", "Module2"));
            modules.Add(new ModuleInfo(assemblies[3], "Module4.TestModules.Module4Class", "Module4", "Module3"));
            modules.Add(new ModuleInfo(assemblies[4], "Module5.TestModules.Module5Class", "Module5", "Module4"));

            ModuleLoaderService loader = new ModuleLoaderService(new MockPrismContainer(), new MockLogger());
            loader.Initialize(modules.ToArray());
        }

        [TestMethod]
        [ExpectedException(typeof(ModuleLoadException))]
        public void FailWhenDependingOnMissingModule()
        {
            string assembly = CompilerHelper.GenerateDynamicModule("ModuleK", null, "ModuleL");
            ModuleInfo module = new ModuleInfo(assembly, "ModuleK.TestsModules.ModuleKClass", "ModuleK", "ModuleL");

            ModuleLoaderService loader = new ModuleLoaderService(new MockPrismContainer(), new MockLogger());
            loader.Initialize(new[] { module });
        }

        [TestMethod]
        public void ShouldResolveModuleAndInitializeSingleModule()
        {
            IPrismContainer container = new MockPrismContainer();
            var service = new ModuleLoaderService(container, new MockLogger());
            FirstTestModule.wasInitializedOnce = false;
            var info = new ModuleInfo(typeof(FirstTestModule).Assembly.Location, typeof(FirstTestModule).FullName, "FirstTestModule");
            service.Initialize(new[] { info });
            Assert.IsTrue(FirstTestModule.wasInitializedOnce);
        }

        [TestMethod]
        public void ShouldResolveAndInitializeMultipleModules()
        {
            IPrismContainer container = new MockPrismContainer();
            var service = new ModuleLoaderService(container, new MockLogger());
            FirstTestModule.wasInitializedOnce = false;
            SecondTestModule.wasInitializedOnce = false;

            var firstModuleInfo = new ModuleInfo(typeof(FirstTestModule).Assembly.Location, typeof(FirstTestModule).FullName, "FirstTestModule");
            var secondModuleInfo = new ModuleInfo(typeof(SecondTestModule).Assembly.Location, typeof(SecondTestModule).FullName, "SecondTestModule");

            service.Initialize(new[] { firstModuleInfo, secondModuleInfo });
            Assert.IsTrue(FirstTestModule.wasInitializedOnce);
            Assert.IsTrue(SecondTestModule.wasInitializedOnce);
        }

        [TestMethod]
        public void ShouldInitializeModulesInOrder()
        {
            IPrismContainer container = new MockPrismContainer();
            var service = new ModuleLoaderService(container, new MockLogger());

            ModuleLoadTracker.ModuleLoadStack.Clear();
            var dependencyModule = new ModuleInfo(typeof(DependencyModule).Assembly.Location, typeof(DependencyModule).FullName, "DependencyModule");
            var dependantModule = new ModuleInfo(typeof(DependantModule).Assembly.Location, typeof(DependantModule).FullName, "DependantModule", new[] { "DependencyModule" });
            service.Initialize(new[] { dependantModule, dependencyModule });

            Assert.AreEqual(typeof(DependantModule), ModuleLoadTracker.ModuleLoadStack.Pop());
            Assert.AreEqual(typeof(DependencyModule), ModuleLoadTracker.ModuleLoadStack.Pop());

        }

        [TestMethod]
        public void ShouldLogModuleInitializeErrorsAndContinueLoading()
        {
            IPrismContainer container = new MockPrismContainer();
            var logger = new MockLogger();
            var service = new CustomModuleInitializerService(container, logger);
            var invalidModule = new ModuleInfo(typeof(InvalidModule).Assembly.Location, typeof(InvalidModule).FullName, "InvalidModule");

            Assert.IsFalse(service.HandleModuleLoadErrorCalled);
            service.Initialize(new[] { invalidModule });
            Assert.IsTrue(service.HandleModuleLoadErrorCalled);
        }

        [TestMethod]
        public void ShouldLogModuleInitializationError()
        {
            IPrismContainer container = new MockPrismContainer();
            var logger = new MockLogger();
            var service = new ModuleLoaderService(container, logger);
            ExceptionThrowingModule.wasInitializedOnce = false;
            var exceptionModule = new ModuleInfo(typeof(ExceptionThrowingModule).Assembly.Location, typeof(ExceptionThrowingModule).FullName, "ExceptionThrowingModule");

            try
            {
                service.Initialize(new[] { exceptionModule });
            }
            catch (ModuleLoadException)
            {
            }
            Assert.IsNotNull(logger.LastMessage);
            StringAssert.Contains(logger.LastMessage, "ExceptionThrowingModule");
        }

        [TestMethod]
        public void ShouldNotInitializeIfAlreadyLoaded()
        {
            ModuleLoadTracker.ModuleLoadStack.Clear();
            IPrismContainer container = new MockPrismContainer();
            var service = new ModuleLoaderService(container, new MockLogger());
            FirstTestModule.wasInitializedOnce = false;
            SecondTestModule.wasInitializedOnce = false;
            var firstModuleInfo = new ModuleInfo(typeof(FirstTestModule).Assembly.Location, typeof(FirstTestModule).FullName, "FirstTestModule");

            service.Initialize(new[] { firstModuleInfo });
            Assert.AreEqual(1, ModuleLoadTracker.ModuleLoadStack.Count, "ModuleLoadStack should only contain 1 module");
            service.Initialize(new[] { firstModuleInfo });
            Assert.AreEqual(1, ModuleLoadTracker.ModuleLoadStack.Count, "ModuleLoadStack should not have module that has already been loaded");

        }

        [TestMethod]
        public void ShouldNotLoadAssemblyThatHasAlreadyBeenLoaded()
        {
            IPrismContainer container = new MockPrismContainer();
            var service = new TestableModuleInitializerService(container, new MockLogger());

            var firstModuleInfo = new ModuleInfo(typeof(FirstTestModule).Assembly.Location, typeof(FirstTestModule).FullName, "FirstTestModule");
            var secondModuleInfo = new ModuleInfo(typeof(SecondTestModule).Assembly.Location, typeof(SecondTestModule).FullName, "SecondTestModule");

            Assert.AreEqual(0, service.LoadedAssemblies.Count);

            service.Initialize(new[] { firstModuleInfo, secondModuleInfo });

            Assert.AreEqual(1, service.LoadedAssemblies.Count);
        }



        public static class ModuleLoadTracker
        {
            public static readonly Stack<Type> ModuleLoadStack = new Stack<Type>();

        }
        public class FirstTestModule : IModule
        {
            public static bool wasInitializedOnce;

            public void Initialize()
            {
                wasInitializedOnce = true;
                ModuleLoadTracker.ModuleLoadStack.Push(GetType());
            }
        }

        public class SecondTestModule : IModule
        {
            public static bool wasInitializedOnce;
            public static long initializedOnTickCount;

            public void Initialize()
            {
                wasInitializedOnce = true;
                ModuleLoadTracker.ModuleLoadStack.Push(GetType());
            }
        }

        public class DependantModule : IModule
        {
            public static bool wasInitializedOnce;

            public void Initialize()
            {
                wasInitializedOnce = true;
                ModuleLoadTracker.ModuleLoadStack.Push(GetType());
            }
        }

        public class DependencyModule : IModule
        {
            public static bool wasInitializedOnce;
            public static long initializedOnTickCount;

            public void Initialize()
            {
                wasInitializedOnce = true;
                ModuleLoadTracker.ModuleLoadStack.Push(GetType());
            }
        }

        public class ExceptionThrowingModule : IModule
        {
            public static bool wasInitializedOnce;
            public static long initializedOnTickCount;

            public void Initialize()
            {
                throw new InvalidOperationException("Intialization can't be performed");
            }
        }

        public class InvalidModule
        {

        }

        public class CustomModuleInitializerService : ModuleLoaderService
        {
            public bool HandleModuleLoadErrorCalled;

            public CustomModuleInitializerService(IPrismContainer container, IPrismLogger logger)
                : base(container, logger)
            {

            }

            public override void HandleModuleLoadError(ModuleInfo moduleInfo, string assemblyName, Exception exception)
            {
                HandleModuleLoadErrorCalled = true;
            }
        }

        public class TestableModuleInitializerService : ModuleLoaderService
        {
            public TestableModuleInitializerService(IPrismContainer container, IPrismLogger logger)
                : base(container, logger)
            {
            }


            public IDictionary<string, Assembly> LoadedAssemblies
            {
                get
                {
                    return base.loadedAssemblies;
                }
            }
        }
    }
}
