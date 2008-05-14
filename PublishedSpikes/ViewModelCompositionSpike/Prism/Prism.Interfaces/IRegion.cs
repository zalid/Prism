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

namespace Prism.Interfaces
{
    public interface IRegion
    {
        IList<object> Views { get; }
        void Add(object view);
        void Add(object view, string name);
        void Remove(object view);
        void Show(object view);
        /// <summary>
        /// Returns the view instance that was added to the region using a specific name.
        /// </summary>
        /// <param name="name">The name used when adding the view to the region</param>
        /// <returns>Returns the named view or <see langword="null"/> if the view with <paramref name="name"/> does not exist.</returns>
        object GetView(string name);

        //event EventHandler<RegionViewEventArgs> ViewActivated;
        //event EventHandler<RegionViewEventArgs> ViewDeactivated;


    }
}
