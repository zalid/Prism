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

using System.Windows.Controls;
using HelloWorld;
using Prism.Interfaces;
using Prism.Regions;

namespace PrismSample
{
    internal class Bootstrapper
    {
        public void Initialize()
        {
            RegionAdapterMappings mappings = new RegionAdapterMappings();
            mappings.RegisterMapping(typeof(ItemsControl), new ItemsControlRegionAdapter());

            Shell shell = new Shell();
            IRegionManager rms = new RegionManager(mappings);

            RegionManager.SetRegionManager(shell, rms);
            shell.Show();

            IModule helloWorldModule = new HelloWorldModule(rms);
            helloWorldModule.Initialize();
        }
    }
}
