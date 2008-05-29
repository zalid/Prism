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

using Microsoft.Practices.Unity;
using Prism.Interfaces;

namespace ModuleB
{
    public class ModuleB : IModule
    {
        public ModuleB(IUnityContainer container, IRegionManager regionManager)
        {
            Container = container;
            RegionManager = regionManager;
        }
        public void Initialize()
        {
            ActivityView activityView1 = Container.Resolve<ActivityView>();
            ActivityView activityView2 = Container.Resolve<ActivityView>();

            activityView1.CustomerId = "Customer1";
            activityView2.CustomerId = "Customer2";

            IRegion rightRegion = RegionManager.GetRegion("RightRegion");
            rightRegion.Add(activityView1);
            rightRegion.Add(activityView2);
        }

        public IUnityContainer Container { get; private set; }
        public IRegionManager RegionManager { get; private set; }
    }
}
