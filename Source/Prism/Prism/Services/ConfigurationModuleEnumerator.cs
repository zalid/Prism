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
using Prism.Configuration;
using Prism.Interfaces;

namespace Prism.Services
{
    public class ConfigurationModuleEnumerator : IModuleEnumerator
    {
        private IList<ModuleInfo> _modules;
        private readonly ConfigurationStore _store;

        public ConfigurationModuleEnumerator(ConfigurationStore store)
        {
            if (store == null)
                throw new ArgumentNullException("store");

            this._store = store;
        }
        public ModuleInfo[] GetModules()
        {
            EnsureModulesDiscovered();
            return _modules.ToArray();
        }

        private void EnsureModulesDiscovered()
        {
            if (_modules == null)
            {
                _modules = new List<ModuleInfo>();

                ModulesConfigurationSection section = _store.GetModuleConfigurationSection();

                foreach (ModuleConfigurationElement element in section.Modules)
                {
                    IList<string> dependencies = new List<string>();

                    if (element.Dependencies.Count > 0)
                    {
                        foreach (ModuleDependencyConfigurationElement dependency in element.Dependencies)
                        {
                            dependencies.Add(dependency.ModuleName);
                        }
                    }

                    ModuleInfo moduleInfo = new ModuleInfo(element.AssemblyFile, element.ModuleType, element.ModuleName,
                                                           element.StartupLoaded, dependencies.ToArray());
                    _modules.Add(moduleInfo);
                }
            }
        }

        public ModuleInfo[] GetStartupLoadedModules()
        {
            EnsureModulesDiscovered();
            return _modules.Where(moduleInfo => moduleInfo.StartupLoaded == true).ToArray();
        }

        public ModuleInfo[] GetModule(string moduleName)
        {
            EnsureModulesDiscovered();
            return _modules.Where(moduleInfo => moduleInfo.ModuleName == moduleName).ToArray();
        }
    }
}
