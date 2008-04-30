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
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;

namespace Prism.UnityContainerAdaptor
{
    public class PrismExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Context.Strategies.AddNew<BuildKeyMappingStrategy>(UnityBuildStage.TypeMapping);

            this.Context.Registering += new EventHandler<RegisterEventArgs>(Context_Registering);
        }

        void Context_Registering(object sender, RegisterEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Name))
            {
                //Adds a type mapping so that whenever a new type is registered, it is also set to as the default
                this.Context.Policies.Set<IBuildKeyMappingPolicy>(
                    new BuildKeyMappingPolicy(new NamedTypeBuildKey(e.TypeFrom, e.Name)),
                    new NamedTypeBuildKey(e.TypeFrom));
            }
        }
    }
}
