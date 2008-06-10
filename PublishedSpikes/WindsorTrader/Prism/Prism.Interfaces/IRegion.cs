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

using System.ComponentModel;

namespace Prism.Interfaces
{
    public interface IRegion
    {
        ICollectionView Views { get; }
        IRegionManager Add(object view);
        IRegionManager Add(object view, string viewName);
        IRegionManager Add(object view, string viewName, bool createRegionManagerScope);
        void Remove(object view);
        void Activate(object view);
        /// <summary>
        /// Returns the view instance that was added to the region using a specific viewName.
        /// </summary>
        /// <param name="viewName">The name used when adding the view to the region</param>
        /// <returns>Returns the named view or <see langword="null"/> if the view with <paramref name="viewName"/> does not exist.</returns>
        object GetView(string viewName);

        IRegionManager RegionManager { set; }
    }
}
