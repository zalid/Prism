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

using Prism.Interfaces.Properties;

namespace Prism.Interfaces
{
    using System;

    [Serializable]
    public class ModuleInfo
    {

        public ModuleInfo(string assemblyFile, string moduleType, string moduleName, params string[] dependsOn)
            : this(assemblyFile, moduleType, moduleName, true, dependsOn)
        {
        }

        public ModuleInfo(string assemblyFile, string moduleType, string moduleName, bool startupLoaded, params string[] dependsOn)
        {
            if (string.IsNullOrEmpty(assemblyFile))
                throw new ArgumentException(Resources.StringCannotNullOrBeEmpty, "assemblyFile");

            if (string.IsNullOrEmpty(moduleType))
                throw new ArgumentException(Resources.StringCannotNullOrBeEmpty, "moduleType");

            AssemblyFile = assemblyFile;
            ModuleType = moduleType;
            ModuleName = moduleName;
            StartupLoaded = startupLoaded;
            DependsOn = dependsOn ?? new string[] { };
        }

        public string AssemblyFile { get; private set; }
        public string ModuleType { get; private set; }
        public string ModuleName { get; private set; }
        public string[] DependsOn { get; private set; }
        public bool StartupLoaded { get; private set; }
    }
}
