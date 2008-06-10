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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Interfaces;
using Prism.Services;

namespace Prism.Tests.Services
{
    [TestClass]
    public class ConfigModuleEnumeratorFixture
    {
        //[TestCleanup]
        //public void CleanUp()
        //{
        //    DeleteDirectory(@".\MockModules");
        //    DeleteDirectory(@".\MockAttributedModule");
        //}

        [TestMethod]
        public void CanInitConfigModuleEnumerator()
        {
            IModuleEnumerator enumerator = new ConfigModuleEnumerator();

            Assert.IsNotNull(enumerator);
        }

        [TestMethod]
        public void ShouldReturnAListOfModuleInfo()
        {
            CompilerHelper.CompileFile(@"Prism.Tests.Mocks.Modules.MockModuleA.cs",
            @".\MocksModules\MockModuleA.dll");

            IModuleEnumerator enumerator = new ConfigModuleEnumerator();

            ModuleInfo[] modules = enumerator.GetModules();

            Assert.IsNotNull(modules);
            Assert.AreEqual(1, modules.Length);
            Assert.IsNotNull(modules[0].AssemblyFile);
            Assert.IsTrue(modules[0].AssemblyFile.Contains(@"MocksModules\MockModuleA.dll"));
            Assert.IsNotNull(modules[0].ModuleType);
            Assert.AreEqual("Prism.Tests.Mocks.Modules.MockModuleA", modules[0].ModuleType);
        }

        //[TestMethod]
        //public void ShouldGetModuleNameFromAttribute()
        //{
        //    CompilerHelper.CompileFile(@"Prism.Tests.Mocks.Modules.MockAttributedModule.cs",
        //   @".\AttributedModules\MockAttributedModule.dll");

        //    IModuleEnumerator enumerator = new ConfigModuleEnumerator();

        //    ModuleInfo[] modules = enumerator.GetModules();

        //    Assert.AreEqual(1, modules.Length);
        //    Assert.AreEqual("TestModule", modules[0].ModuleName);
        //}

        //private void DeleteDirectory(string path)
        //{
        //    if (Directory.Exists(path))
        //    {
        //        Directory.Delete(path, true);
        //    }
        //}
    }
}
