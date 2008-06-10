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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.Composite.Wpf.Tests.Modularity
{
    [TestClass]
    public class DirectoryLookupModuleEnumeratorFixture
    {
        [TestMethod]
        public void CanInitDirectoryLookupModuleEnumerator()
        {
            string path = @".\MocksModules";
            CompilerHelper.CleanUpDirectory(path);
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
        [ExpectedException(typeof(ArgumentException))]
        public void NonExistentPathThrows()
        {
            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator("NonExistentPath");
        }

        [TestMethod]
        public void ShouldReturnAListOfModuleInfo()
        {
            CompilerHelper.CompileFile(@"Microsoft.Practices.Composite.Wpf.Tests.Mocks.Modules.MockModuleA.cs",
                                       @".\MocksModules\MockModuleA.dll");

            string path = @".\MocksModules";

            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator(path);

            ModuleInfo[] modules = enumerator.GetModules();

            Assert.IsNotNull(modules);
            Assert.AreEqual(1, modules.Length);
            Assert.IsNotNull(modules[0].AssemblyFile);
            Assert.IsTrue(modules[0].AssemblyFile.Contains(@"MocksModules\MockModuleA.dll"));
            Assert.IsNotNull(modules[0].ModuleType);
            Assert.AreEqual("Microsoft.Practices.Composite.Wpf.Tests.Mocks.Modules.MockModuleA", modules[0].ModuleType);
            Assert.IsTrue(modules[0].StartupLoaded);
        }

        [TestMethod]
        public void ShouldNotThrowWithLoadFromByteAssemblies()
        {
            CompilerHelper.CleanUpDirectory(@".\CompileOutput\");
            CompilerHelper.CleanUpDirectory(@".\IgnoreLoadFromByteAssembliesTestDir\");
            var results = CompilerHelper.CompileFile(@"Microsoft.Practices.Composite.Wpf.Tests.Mocks.Modules.MockModuleA.cs",
                                                     @".\CompileOutput\MockModuleA.dll");

            CompilerHelper.CompileFile(@"Microsoft.Practices.Composite.Wpf.Tests.Mocks.Modules.MockAttributedModule.cs",
                                       @".\IgnoreLoadFromByteAssembliesTestDir\MockAttributedModule.dll");

            string path = @".\IgnoreLoadFromByteAssembliesTestDir";

            Evidence evidence = AppDomain.CurrentDomain.Evidence;
            AppDomainSetup setup = AppDomain.CurrentDomain.SetupInformation;
            AppDomain testDomain = null;
            try
            {
                testDomain = AppDomain.CreateDomain("TestDomain", evidence, setup);

                Type remoteEnumType = typeof(RemoteEnumerator);

                RemoteEnumerator remoteEnum = (RemoteEnumerator)testDomain.CreateInstanceFrom(
                                                                    remoteEnumType.Assembly.Location, remoteEnumType.FullName).Unwrap();

                remoteEnum.LoadAssembliesByByte(@".\CompileOutput\MockModuleA.dll");

                ModuleInfo[] infos = remoteEnum.DoEnumeration(path);

                Assert.IsNotNull(
                    infos.FirstOrDefault(x => x.ModuleType == "Microsoft.Practices.Composite.Wpf.Tests.Mocks.Modules.MockAttributedModule")
                    );
            }
            finally
            {
                if (testDomain != null)
                    AppDomain.Unload(testDomain);
            }
        }


        [TestMethod]
        public void ShouldGetModuleNameFromAttribute()
        {
            CompilerHelper.CompileFile(@"Microsoft.Practices.Composite.Wpf.Tests.Mocks.Modules.MockAttributedModule.cs",
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
            CompilerHelper.CompileFile(@"Microsoft.Practices.Composite.Wpf.Tests.Mocks.Modules.MockDependencyModule.cs",
                                       @".\DependantModules\DependencyModule.dll");

            CompilerHelper.CompileFile(@"Microsoft.Practices.Composite.Wpf.Tests.Mocks.Modules.MockDependantModule.cs",
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
            Assert.AreEqual(1, dependantModule.DependsOn.Count);
            Assert.AreEqual(dependencyModule.ModuleName, dependantModule.DependsOn[0]);
        }

        [TestMethod]
        public void UseClassNameAsModuleNameWhenNotSpecifiedInAttribute()
        {
            CompilerHelper.CompileFile(@"Microsoft.Practices.Composite.Wpf.Tests.Mocks.Modules.MockModuleA.cs",
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
            CompilerHelper.CompileFile(@"Microsoft.Practices.Composite.Wpf.Tests.Mocks.Modules.MockAttributedModule.cs",
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
            CompilerHelper.CompileFile(@"Microsoft.Practices.Composite.Wpf.Tests.Mocks.Modules.MockAttributedModule.cs",
                                       @".\AttributedModules\MockAttributedModule.dll");

            string path = @".\AttributedModules";

            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator(path);

            Assert.AreEqual<int>(1, enumerator.GetModules().Length);
            Assert.AreEqual<int>(0, enumerator.GetStartupLoadedModules().Length);
        }

        [TestMethod]
        public void GetModuleReturnsOnlySpecifiedModule()
        {
            CompilerHelper.CompileFile(@"Microsoft.Practices.Composite.Wpf.Tests.Mocks.Modules.MockAttributedModule.cs",
                                       @".\AttributedModules\MockAttributedModule.dll");

            string path = @".\AttributedModules";

            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator(path);
            var modules = enumerator.GetModule("TestModule");
            Assert.IsNotNull(modules);
            Assert.AreEqual(1, modules.Length);
            Assert.AreEqual("TestModule", modules[0].ModuleName);
        }

        [TestMethod]
        public void ShouldNotLoadAssembliesInCurrentAppDomain()
        {
            CompilerHelper.CompileFile(@"Microsoft.Practices.Composite.Wpf.Tests.Mocks.Modules.MockModuleA.cs",
                                       @".\Mocks\Modules\MockModuleA.dll");

            string path = @".\Mocks\Modules";

            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator(path);

            ModuleInfo[] modules = enumerator.GetModules();

            Assembly loadedAssembly = Array.Find<Assembly>(AppDomain.CurrentDomain.GetAssemblies(), assembly => assembly.Location.Equals(modules[0].AssemblyFile, StringComparison.InvariantCultureIgnoreCase));
            Assert.IsNull(loadedAssembly);
        }

        [TestMethod]
        public void ShouldNotGetModuleInfoForAnAssemblyAlreadyLoadedInTheMainDomain()
        {
            string path = @".\Mocks\ModulesMainDomain\";

            CompilerHelper.CleanUpDirectory(path);

            var assemblyPath = Assembly.GetCallingAssembly().Location;

            File.Copy(assemblyPath, path + Path.GetFileName(assemblyPath));

            var enumerator = new DirectoryLookupModuleEnumerator(path);

            Assert.AreEqual(0, enumerator.GetModules().Count());
        }

        [TestMethod]
        public void ShouldLoadAssemblyEvenIfTheyAreReferencingEachOther()
        {
            string path = @".\Mocks\Modules";
            CompilerHelper.CleanUpDirectory(path);
            CompilerHelper.CompileFile(@"Microsoft.Practices.Composite.Wpf.Tests.Mocks.Modules.MockModuleA.cs",
                                       @".\Mocks\Modules\MockModuleA.dll");

            CompilerHelper.CompileFile(@"Microsoft.Practices.Composite.Wpf.Tests.Mocks.Modules.MockModuleReferencingOtherModule.cs",
                                       @".\Mocks\Modules\MockModuleReferencingOtherModule.dll", @".\Mocks\Modules\MockModuleA.dll");

            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator(path);

            ModuleInfo[] modules = enumerator.GetModules();

            Assert.AreEqual(2, modules.Count());
        }

        [TestMethod]
        public void CreateChildAppDomainHasParentEvidenceAndSetup()
        {
            string path = @".\MocksModules";
            CompilerHelper.CleanUpDirectory(path);

            TestableDirectoryLookupModuleEnumerator enumerator = new TestableDirectoryLookupModuleEnumerator(path);
            Evidence parentEvidence = new Evidence();
            AppDomainSetup parentSetup = new AppDomainSetup();
            parentSetup.ApplicationName = "Test Parent";
            AppDomain parentAppDomain = AppDomain.CreateDomain("Parent", parentEvidence, parentSetup);
            AppDomain childDomain = enumerator.BuildChildDomain(parentAppDomain);
            Assert.AreEqual(parentEvidence.Count, childDomain.Evidence.Count);
            Assert.AreEqual("Test Parent", childDomain.SetupInformation.ApplicationName);
            Assert.AreNotEqual(AppDomain.CurrentDomain.Evidence.Count, childDomain.Evidence.Count);
            Assert.AreNotEqual(AppDomain.CurrentDomain.SetupInformation.ApplicationName, childDomain.SetupInformation.ApplicationName);
        }
    }

    public class TestableDirectoryLookupModuleEnumerator : DirectoryLookupModuleEnumerator
    {
        public TestableDirectoryLookupModuleEnumerator(string path) : base(path) { }

        public new AppDomain BuildChildDomain(AppDomain currentDomain)
        {
            return base.BuildChildDomain(currentDomain);
        }
    }

    public class RemoteEnumerator : MarshalByRefObject
    {
        public RemoteEnumerator()
        {
        }

        public void LoadAssembliesByByte(string assemblyPath)
        {
            byte[] assemblyBytes = File.ReadAllBytes(assemblyPath);
            AppDomain.CurrentDomain.Load(assemblyBytes);
        }

        public ModuleInfo[] DoEnumeration(string path)
        {
            IModuleEnumerator enumerator = new DirectoryLookupModuleEnumerator(path);
            ModuleInfo[] modules = enumerator.GetModules();
            return modules;
        }
    }
}