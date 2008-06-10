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
using System.Collections.Specialized;

namespace Microsoft.Practices.Composite.Wpf.Regions
{
    public class CollectionActiveAwareBehavior
    {
        private readonly WeakReference _collection;

        public CollectionActiveAwareBehavior(INotifyCollectionChanged collection)
        {
            _collection = new WeakReference(collection);
        }

        public void Attach()
        {
            INotifyCollectionChanged collection = GetCollection();
            if (collection != null)
                collection.CollectionChanged += OnCollectionChanged;
        }

        public void Detach()
        {
            INotifyCollectionChanged collection = GetCollection();
            if (collection != null)
                collection.CollectionChanged -= OnCollectionChanged;
        }

        static void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    IActiveAware activeAware = item as IActiveAware;
                    if (activeAware != null)
                        activeAware.IsActive = true;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    IActiveAware activeAware = item as IActiveAware;
                    if (activeAware != null)
                        activeAware.IsActive = false;
                }
            }
            //TODO: handle other action values (reset, etc)
        }

        private INotifyCollectionChanged GetCollection()
        {
            return _collection.Target as INotifyCollectionChanged;
        }
    }
}
