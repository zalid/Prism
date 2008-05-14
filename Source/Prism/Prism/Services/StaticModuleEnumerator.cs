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
using Prism.Interfaces;

namespace Prism.Services
{
    public class StaticModuleEnumerator : IModuleEnumerator
    {
        readonly List<ModuleInfo> _modules = new List<ModuleInfo>();
        public ModuleInfo[] GetModules()
        {
            return _modules.ToArray();
        }

        public ModuleInfo[] GetStartupLoadedModules()
        {
            return _modules.ToArray();
        }

        public ModuleInfo[] GetModule(string moduleName)
        {
            return _modules.Where(moduleInfo => moduleInfo.ModuleName == moduleName).ToArray();
        }

        public void AddModule(Type moduleType, params string[] dependsOn)
        {
            ModuleInfo moduleInfo = new ModuleInfo(moduleType.Assembly.Location
                                  , moduleType.FullName
                                  , moduleType.Name
                                  , dependsOn);
            _modules.Add(moduleInfo);
        }
    }
}
