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
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Interfaces;
using Prism.Services;

namespace Prism.Tests.Services
{
    [TestClass]
    public class DirectoryLookupModuleEnumeratorFixture
    {
        [TestMethod]
        public void CanInitDirectoryLookupModuleEnumerator()
        {
            string path = "modules";
            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator(path);

            Assert.IsNotNull(enumerator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullPathThrows()
        {
            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyPathThrows()
        {
            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator(string.Empty);
        }

        [TestMethod]
        public void ShouldReturnAListOfModuleInfo()
        {
            CompilerHelper.CompileFile(@"Prism.Tests.Mocks.Modules.MockModuleA.cs",
            @".\MocksModules\MockModuleA.dll");

            string path = @".\MocksModules";

            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator(path);

            ModuleInfo[] modules = enumerator.GetModules();

            Assert.IsNotNull(modules);
            Assert.AreEqual(1, modules.Length);
            Assert.IsNotNull(modules[0].AssemblyFile);
            Assert.IsTrue(modules[0].AssemblyFile.Contains(@"MocksModules\MockModuleA.dll"));
            Assert.IsNotNull(modules[0].ModuleType);
            Assert.AreEqual("Prism.Tests.Mocks.Modules.MockModuleA", modules[0].ModuleType);
            Assert.IsTrue(modules[0].StartupLoaded);
        }

        [TestMethod]
        public void ShouldGetModuleNameFromAttribute()
        {
            CompilerHelper.CompileFile(@"Prism.Tests.Mocks.Modules.MockAttributedModule.cs",
           @".\AttributedModules\MockAttributedModule.dll");

            string path = @".\AttributedModules";

            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator(path);

            ModuleInfo[] modules = enumerator.GetModules();

            Assert.AreEqual(1, modules.Length);
            Assert.AreEqual("TestModule", modules[0].ModuleName);
        }

        [TestMethod]
        public void ShouldGetDependantModulesFromAttribute()
        {
            CompilerHelper.CompileFile(@"Prism.Tests.Mocks.Modules.MockDependencyModule.cs",
                                       @".\DependantModules\DependencyModule.dll");

            CompilerHelper.CompileFile(@"Prism.Tests.Mocks.Modules.MockDependantModule.cs",
                                       @".\DependantModules\DependantModule.dll");

            string path = @".\DependantModules";

            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator(path);

            ModuleInfo[] modules = enumerator.GetModules();

            Assert.AreEqual(2, modules.Length);
            var dependantModule = modules.First(module => module.ModuleName == "DependantModule");
            var dependencyModule = modules.First(module => module.ModuleName == "DependencyModule");
            Assert.IsNotNull(dependantModule);
            Assert.IsNotNull(dependencyModule);
            Assert.IsNotNull(dependantModule.DependsOn);
            Assert.AreEqual(1, dependantModule.DependsOn.Length);
            Assert.AreEqual(dependencyModule.ModuleName, dependantModule.DependsOn[0]);
        }

        [TestMethod]
        public void UseClassNameAsModuleNameWhenNotSpecifiedInAttribute()
        {
            CompilerHelper.CompileFile(@"Prism.Tests.Mocks.Modules.MockModuleA.cs",
            @".\MocksModules\MockModuleA.dll");

            string path = @".\MocksModules";

            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator(path);

            ModuleInfo[] modules = enumerator.GetModules();

            Assert.IsNotNull(modules);
            Assert.AreEqual("MockModuleA", modules[0].ModuleName);
        }

        [TestMethod]
        public void ShouldGetStartupLoadedFromAttribute()
        {
            CompilerHelper.CompileFile(@"Prism.Tests.Mocks.Modules.MockAttributedModule.cs",
           @".\AttributedModules\MockAttributedModule.dll");

            string path = @".\AttributedModules";

            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator(path);

            ModuleInfo[] modules = enumerator.GetModules();

            Assert.AreEqual(1, modules.Length);
            Assert.AreEqual<bool>(false, modules[0].StartupLoaded);
        }

        [TestMethod]
        public void GetStartupLoadedModulesDoesntRetrieveOnDemandLoaded()
        {
            CompilerHelper.CompileFile(@"Prism.Tests.Mocks.Modules.MockAttributedModule.cs",
           @".\AttributedModules\MockAttributedModule.dll");

            string path = @".\AttributedModules";

            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator(path);

            Assert.AreEqual<int>(1, enumerator.GetModules().Length);
            Assert.AreEqual<int>(0, enumerator.GetStartupLoadedModules().Length);
        }

        [TestMethod]
        public void GetModuleReturnsOnlySpecifiedModule()
        {
            CompilerHelper.CompileFile(@"Prism.Tests.Mocks.Modules.MockAttributedModule.cs",
           @".\AttributedModules\MockAttributedModule.dll");

            string path = @".\AttributedModules";

            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator(path);
            var modules = enumerator.GetModule("TestModule");
            Assert.IsNotNull(modules);
            Assert.AreEqual(1, modules.Length);
            Assert.AreEqual("TestModule", modules[0].ModuleName);
        }

        [TestMethod]
        public void ShouldNotLoadTypesInCurrentAppDomain()
        {
            CompilerHelper.CompileFile(@"Prism.Tests.Mocks.Modules.MockModuleA.cs",
            @".\Mocks\Modules\MockModuleA.dll");

            string path = @".\Mocks\Modules";

            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator(path);

            ModuleInfo[] modules = enumerator.GetModules();

            Assembly loadedAssembly = Array.Find<Assembly>(AppDomain.CurrentDomain.GetAssemblies(), assembly => assembly.Location.Equals(modules[0].AssemblyFile, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsNull(loadedAssembly);
        }

    }
}