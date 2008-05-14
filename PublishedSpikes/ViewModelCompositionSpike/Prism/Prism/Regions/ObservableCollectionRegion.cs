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
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Interfaces;

namespace Prism.Regions
{
    public class ObservableCollectionRegion<T> : IRegion
    {
        private readonly ObservableCollection<T> _innerCollection;
        public ObservableCollectionRegion(ObservableCollection<T> collection)
        {
            _innerCollection = collection;
        }

        #region IRegion Members

        public IList<object> Views
        {
            get { return (IList<object>)_innerCollection.ToList(); }
        }

        public void Add(object view)
        {
            _innerCollection.Add((T)view);
        }

        public void Add(object view, string name)
        {
            Add(view);
        }

        public void Remove(object view)
        {
            _innerCollection.Remove((T)view);
        }

        public void Show(object view)
        {
            throw new NotImplementedException();
        }

        public object GetView(string name)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}