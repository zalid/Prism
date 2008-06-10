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
using System.Text;
using System.Windows;
using System.Windows.Controls;
using FamilyShow.Infrastructure;
using FamilyShow.MapModule;
using Microsoft.Practices.Unity;
using Prism.Interfaces;
using Prism.Services;
using Prism.UnityContainerAdapter;

namespace Microsoft.FamilyShow
{
    class FamilyShowPrismBootstrapper : UnityPrismBootstrapper
    {
        private FrameworkElement shell;

        protected override IModuleEnumerator GetModuleEnumerator()
        {
            StaticModuleEnumerator moduleEnumerator = new StaticModuleEnumerator();
            moduleEnumerator.AddModule(typeof(HistoricalFactModule));
            return moduleEnumerator;
        }

        protected override void RegisterShellServices()
        {
            base.RegisterShellServices();

            Container.RegisterType<PersonContentController>(new ContainerControlledLifetimeManager());
        }
        
        public override System.Windows.DependencyObject ShowShell()
        {
          return null;
        }
        
    }
}
