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
using System.Globalization;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Properties;

namespace Microsoft.Practices.Composite.Wpf.Regions
{
    public abstract class RegionAdapterBase<T> : IRegionAdapter where T : class
    {
        public IRegion Initialize(T regionTarget)
        {
            IRegion region = CreateRegion();
            AttachBehaviors(region, regionTarget);
            Adapt(region, regionTarget);
            return region;
        }

        protected virtual void AttachBehaviors(IRegion region, T regionTarget)
        {
            CollectionActiveAwareBehavior activeAwareBehavior = new CollectionActiveAwareBehavior(region.ActiveViews);
            activeAwareBehavior.Attach();
        }

        protected abstract void Adapt(IRegion region, T regionTarget);
        protected abstract IRegion CreateRegion();

        IRegion IRegionAdapter.Initialize(object regionTarget)
        {
            if (regionTarget == null)
                throw new ArgumentNullException("regionTarget");

            T castedObject = regionTarget as T;
            if (castedObject == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Resources.AdapterInvalidTypeException, typeof(T).Name));

            return Initialize(castedObject);
        }
    }
}
