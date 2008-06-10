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
using System.Windows;
using Prism.Interfaces;

namespace Prism.UnityContainerAdapter.Tests.Mocks
{
    class MockRegionManager : IRegionManager
    {
        #region IRegionManager Members

        public void Register(string regionName, IRegion region)
        {
            throw new NotImplementedException();
        }

        public void Unregister(string regionName)
        {
            throw new NotImplementedException();
        }

        public IRegion GetRegion(string regionName)
        {
            throw new NotImplementedException();
        }

        public bool HasRegion(string regionName)
        {
            throw new NotImplementedException();
        }

        public void CreateRegion(DependencyObject element, string regionName)
        {
            throw new NotImplementedException();
        }

        public IRegionManager CreateRegionManager()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}