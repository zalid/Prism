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

using System.Configuration;

namespace Prism.Configuration
{
    public class ModuleConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ModuleConfigurationElement"/>.
        /// </summary>
        public ModuleConfigurationElement()
        {
        }

        public ModuleConfigurationElement(string assemblyFile, string moduleType, string moduleName, bool startupLoaded)
        {
            base["assemblyFile"] = assemblyFile;
            base["moduleType"] = moduleType;
            base["moduleName"] = moduleName;
            base["startupLoaded"] = startupLoaded;
        }

        /// <summary>
        /// Gets or sets the assembly file.
        /// </summary>
        [ConfigurationProperty("assemblyFile", IsRequired = true)]
        public string AssemblyFile
        {
            get { return (string)base["assemblyFile"]; }
            set { base["assemblyFile"] = value; }
        }

        /// <summary>
        /// Gets or sets the module type.
        /// </summary>
        [ConfigurationProperty("moduleType", IsRequired = true)]
        public string ModuleType
        {
            get { return (string)base["moduleType"]; }
            set { base["moduleType"] = value; }
        }

        /// <summary>
        /// Gets or sets the module name.
        /// </summary>
        [ConfigurationProperty("moduleName", IsRequired = true)]
        public string ModuleName
        {
            get { return (string)base["moduleName"]; }
            set { base["moduleName"] = value; }
        }

        /// <summary>
        /// Gets or sets the module name.
        /// </summary>
        [ConfigurationProperty("startupLoaded", IsRequired = false, DefaultValue = true)]
        public bool StartupLoaded
        {
            get { return (bool)base["startupLoaded"]; }
            set { base["startupLoaded"] = value; }
        }

        /// <summary>
        /// Gets or sets the modules this module depends on.
        /// </summary>
        [ConfigurationProperty("dependencies", IsDefaultCollection = true, IsKey = false)]
        public ModuleDependencyCollection Dependencies
        {
            get { return (ModuleDependencyCollection)base["dependencies"]; }
            set { base["dependencies"] = value; }
        }
    }
}
