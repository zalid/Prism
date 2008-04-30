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
using System.Windows;

namespace Prism.Tests.Mocks
{
    internal class MockPrismContainer : IPrismContainer
    {
        public T Resolve<T>()
        {
            return Activator.CreateInstance<T>();
        }

        public object TryResolve(Type type)
        {
            if (type.Equals(typeof(IRegion<DependencyObject>)))
            {
                return new MockRegion();
            }

            return null;
        }


        public object Resolve(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}