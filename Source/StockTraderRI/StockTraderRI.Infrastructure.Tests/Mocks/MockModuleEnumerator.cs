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

using System.Collections.Generic;
using Microsoft.Practices.Composite.Modularity;

namespace StockTraderRI.Infrastructure.Tests.Mocks
{
    internal class MockModuleEnumerator : IModuleEnumerator
    {
        public List<ModuleInfo> Modules { get; set; }

        public bool GetStartupLoadedModulesCalled { get; set; }

        public MockModuleEnumerator()
        {
            GetStartupLoadedModulesCalled = false;
            Modules = new List<ModuleInfo>();
        }

        public ModuleInfo[] GetModules()
        {
            throw new System.NotImplementedException();

        }

        public ModuleInfo[] GetStartupLoadedModules()
        {
            GetStartupLoadedModulesCalled = true;
            return Modules.ToArray();
        }

        public ModuleInfo[] GetModule(string moduleName)
        {
            throw new System.NotImplementedException();
        }

    }
}