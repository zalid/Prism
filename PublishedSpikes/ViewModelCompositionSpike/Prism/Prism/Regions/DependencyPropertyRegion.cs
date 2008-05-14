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
using System.Windows;
using Prism.Interfaces;

namespace Prism.Regions
{
    public class DependencyPropertyRegion : IRegion
    {
        private readonly DependencyObject dependentObject;
        private readonly DependencyProperty property;

        public DependencyPropertyRegion(DependencyObject obj, DependencyProperty property)
        {
            dependentObject = obj;
            this.property = property;
        }
        #region IRegion Members

        public void Add(object view)
        {
            dependentObject.SetValue(property, view);
        }

        public void Add(object view, string name)
        {
            throw new NotImplementedException();
        }

        public object GetView(string name)
        {
            throw new NotImplementedException();
        }

        public void Remove(object view)
        {
            throw new NotImplementedException();
        }

        public void Show(object view)
        {
            throw new NotImplementedException();
        }

        public IList<object> Views
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}