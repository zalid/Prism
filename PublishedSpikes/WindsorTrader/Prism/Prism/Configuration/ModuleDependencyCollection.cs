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
using System.Configuration;

namespace Prism.Configuration
{
    /// <summary>
    /// A collection of <see cref="ModuleDependencyConfigurationElement"/>.
    /// </summary>
    public class ModuleDependencyCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ModuleDependencyCollection"/>.
        /// </summary>
        public ModuleDependencyCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ModuleDependencyCollection"/>.
        /// </summary>
        /// <param name="dependencies">An array of <see cref="ModuleDependencyConfigurationElement"/> with initial list of dependencies.</param>
        public ModuleDependencyCollection(ModuleDependencyConfigurationElement[] dependencies)
        {
            if (dependencies == null)
                throw new ArgumentNullException("dependencies");

            foreach (ModuleDependencyConfigurationElement dependency in dependencies)
            {
                BaseAdd(dependency);
            }
        }

        /// <summary>
        /// Gets the type of <see cref="ConfigurationElementCollectionType"/>.
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        /// <summary>
        /// Gets the name to identify this collection in the configuration.
        /// </summary>
        protected override string ElementName
        {
            get { return "dependency"; }
        }

        /// <summary>
        /// Gets the <see cref="ModuleDependencyConfigurationElement"/> located at the specified index in the collection.
        /// </summary>
        /// <param name="index">The index of the element in the collection.</param>
        /// <returns>A <see cref="ModuleDependencyConfigurationElement"/>.</returns>
        public ModuleDependencyConfigurationElement this[int index]
        {
            get { return (ModuleDependencyConfigurationElement)base.BaseGet(index); }
        }

        /// <summary>
        /// Creates a new <see cref="ModuleDependencyConfigurationElement"/>.
        /// </summary>
        /// <returns>A <see cref="ModuleDependencyConfigurationElement"/>.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModuleDependencyConfigurationElement();
        }

        /// <summary>
        /// Gets the element key for specified element.
        /// </summary>
        /// <param name="element">The <see cref="ConfigurationElement"/> to get the key for.</param>
        /// <returns>An <see langword="object"/>.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModuleDependencyConfigurationElement)element).ModuleName;
        }
    }
}
