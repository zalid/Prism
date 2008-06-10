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
using Castle.Core;
using Castle.Core.Configuration;
using Castle.MicroKernel;
using Castle.Windsor;
using Prism.Interfaces;

namespace Prism.WindsorContainerAdapter
{
    public class WindsorPrismContainer : IPrismContainer
    {
        private IWindsorContainer _container;

        public WindsorPrismContainer(IWindsorContainer container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            return (T) Resolve(typeof (T));
        }

        public object Resolve(Type type)
        {
            if (type.IsClass && !_container.Kernel.HasComponent(type))
            {
                _container.AddComponentWithLifestyle(type.FullName, type, LifestyleType.Transient);
            }
            return _container.Resolve(type);
        }

        public object TryResolve(Type type)
        {
            if (_container.Kernel.HasComponent(type))
            {
                return Resolve(type);
            }

            return null;
        }
    }
}
