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


namespace Microsoft.Practices.Composite.Modularity
{
    public interface IModuleEnumerator
    {
        /// <summary>
        /// Gets a list of metadata information of the modules.
        /// </summary>
        /// <returns>An array of <see cref="ModuleInfo"/>.</returns>
        ModuleInfo[] GetModules();

        /// <summary>
        /// Gets a list of metadata information of the modules that should be loaded at startup.
        /// </summary>
        /// <returns>An array of <see cref="ModuleInfo"/>.</returns>
        ModuleInfo[] GetStartupLoadedModules();

        /// <summary>
        /// Gets the metadata information of a module by its name.
        /// </summary>
        /// <returns>An array of <see cref="ModuleInfo"/>.</returns>
        ModuleInfo[] GetModule(string moduleName);
    }
}