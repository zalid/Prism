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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Wpf.Regions;

namespace Microsoft.Practices.Composite.Wpf.Regions
{
    public class ViewsCollection : IViewsCollection
    {
        private readonly ObservableCollection<ItemMetadata> subjectCollection;
        private readonly Func<ItemMetadata, bool> filter;

        private readonly List<object> filteredCollection = new List<object>();

        public ViewsCollection(ObservableCollection<ItemMetadata> list, Func<ItemMetadata, bool> filter)
        {
            this.subjectCollection = list;
            this.filter = filter;
            Initialize();
            subjectCollection.CollectionChanged += UnderlyingCollectionChanged;
        }

        public void Reset()
        {
            foreach (ItemMetadata itemMetadata in subjectCollection)
            {
                itemMetadata.MetadataChanged -= itemMetadata_MetadataChanged;
            }
            filteredCollection.Clear();
            Initialize();
            InvokeCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void Initialize()
        {
            foreach (ItemMetadata itemMetadata in subjectCollection)
            {
                itemMetadata.MetadataChanged += itemMetadata_MetadataChanged;
                if (filter(itemMetadata))
                {
                    filteredCollection.Add(itemMetadata.Item);
                }
            }
        }

        void UnderlyingCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            List<object> changedItems = new List<object>();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (ItemMetadata itemMetadata in e.NewItems)
                    {
                        itemMetadata.MetadataChanged += itemMetadata_MetadataChanged;
                        if (filter(itemMetadata))
                        {
                            changedItems.Add(itemMetadata.Item);
                        }
                    }
                    AddAndNotify(changedItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (ItemMetadata itemMetadata in e.OldItems)
                    {
                        itemMetadata.MetadataChanged -= itemMetadata_MetadataChanged;
                        if (filteredCollection.Contains(itemMetadata.Item))
                        {
                            changedItems.Add(itemMetadata.Item);
                        }
                    }
                    RemoveAndNotify(changedItems);
                    break;
                default:
                    Reset();
                    break;
            }
        }

        void itemMetadata_MetadataChanged(object sender, EventArgs e)
        {
            ItemMetadata itemMetadata = (ItemMetadata)sender;
            if (filteredCollection.Contains(itemMetadata.Item))
            {
                if (filter(itemMetadata) == false)
                {
                    RemoveAndNotify(itemMetadata.Item);
                }
            }
            else
            {
                if (filter(itemMetadata) == true)
                {
                    AddAndNotify(itemMetadata.Item);
                }
            }
        }


        public bool Contains(object value)
        {
            return filteredCollection.Contains(value);
        }

        public IEnumerator<object> GetEnumerator()
        {
            return filteredCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void AddAndNotify(object item)
        {
            AddAndNotify(new List<object>(1) { item });
        }

        private void AddAndNotify(IList items)
        {
            if (items.Count > 0)
            {
                filteredCollection.AddRange(items.Cast<object>());
                InvokeCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
            }
        }

        private void RemoveAndNotify(object item)
        {
            RemoveAndNotify(new List<object>(1) { item });
        }

        private void RemoveAndNotify(IList items)
        {
            if (items.Count > 0)
            {
                int index = -1;
                if (items.Count == 1)
                {
                    index = filteredCollection.IndexOf(items[0]);
                }
                foreach (object item in items)
                {
                    filteredCollection.Remove(item);
                }
                InvokeCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items, index));
            }
        }

        private void InvokeCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler Handler = CollectionChanged;
            if (Handler != null) Handler(this, e);
        }
    }
}