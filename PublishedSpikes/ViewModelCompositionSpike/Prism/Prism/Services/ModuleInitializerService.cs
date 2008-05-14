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
using Prism.Interfaces;
using Prism.Interfaces.Logging;

namespace Prism.Services
{
    /// <summary>
    /// Handles initialization of a set of modules based on types.
    /// </summary>
    public class ModuleInitializerService : IModuleInitializerService
    {
        private IPrismContainer _prismContainer;
        private IPrismLogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        /// <param name="logger"></param>
        public ModuleInitializerService(IPrismContainer container, IPrismLogger logger)
        {
            _prismContainer = container;
            _logger = logger;
        }


        /// <summary>
        /// Initializes the supplied types in the order provided.
        /// </summary>
        /// <param name="typeList">List of types to resolve through the container and initialize</param>
        /// <remarks>
        /// The supplied types must implement <see cref="Prism.Interfaces.IModule"/>IModule to be initialized.
        /// 
        /// If a type cannot be loaded or a module throws an exception, these are logged via the registered IPrismLogger
        /// and initialze attempts to load and initialize the next module.
        /// </remarks>
        public void Initialize(Type[] typeList)
        {
            foreach (var type in typeList)
            {
                try
                {
                    IModule module = (IModule)_prismContainer.Resolve(type);
                    module.Initialize();
                }
                catch(Exception e)
                {
                    _logger.Log(e.Message, Category.Exception,Priority.High);
                }

            }
        }
    }
}