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

using Prism.Interfaces;

namespace ModuleA
{
    public class ModuleA : IModule
    {
        private readonly IRegionManagerService _regionManagerService;

        public ModuleA(IRegionManagerService regionManagerService)
        {
            _regionManagerService = regionManagerService;
        }

        public void Initialize()
        {
            _regionManagerService.GetRegion("MainRegion").Add(new DefaultViewA());
        }
    }
}
