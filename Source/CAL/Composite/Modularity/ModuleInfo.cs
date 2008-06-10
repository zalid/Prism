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
using Microsoft.Practices.Composite.Properties;

namespace Microsoft.Practices.Composite.Modularity
{
    /// <summary>
    /// Defines the metadata that describes a module.
    /// </summary>
    [Serializable]
    public class ModuleInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleInfo"/> class with the given
        /// assembly file, module type, module name and dependencies.
        /// </summary>
        public ModuleInfo(string assemblyFile, string moduleType, string moduleName, params string[] dependsOn)
            : this(assemblyFile, moduleType, moduleName, true, dependsOn)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleInfo"/> class with the given
        /// assembly file, module type, module name, startup loaded and dependencies.
        /// </summary>
        public ModuleInfo(string assemblyFile, string moduleType, string moduleName, bool startupLoaded, params string[] dependsOn)
        {
            if (string.IsNullOrEmpty(assemblyFile))
                throw new ArgumentException(Resources.StringCannotBeNullOrEmpty, "assemblyFile");

            if (string.IsNullOrEmpty(moduleType))
                throw new ArgumentException(Resources.StringCannotBeNullOrEmpty, "moduleType");

            AssemblyFile = assemblyFile;
            ModuleType = moduleType;
            ModuleName = moduleName;
            StartupLoaded = startupLoaded;
            DependsOn = dependsOn != null ? new List<string>(dependsOn) : new List<string>();
        }

        /// <summary>
        /// The assembly file where the module is located.
        /// </summary>
        public string AssemblyFile { get; private set; }

        /// <summary>
        /// The type of the module.
        /// </summary>
        public string ModuleType { get; private set; }

        /// <summary>
        /// The name of the module.
        /// </summary>
        public string ModuleName { get; private set; }

        /// <summary>
        /// The list of modules that this module depends upon.
        /// </summary>
        public IList<string> DependsOn { get; private set; }

        /// <summary>
        /// Specifies whether the module should be loaded at startup. 
        /// </summary>
        /// <param name="value">
        /// When <see langword="true"/> (default value), it indicates that this module should be loaded at startup. 
        /// Otherwise you should explicitly load this module on demand.
        /// </param>
        public bool StartupLoaded { get; private set; }
    }
}
