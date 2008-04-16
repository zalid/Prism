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
using Prism.Interfaces;
using System.Windows;

namespace UIComposition.Modules.Employee.Tests.Mocks
{
    public class MockRegion : IRegion
    {
        public bool ShowCalled;
        private IList<UIElement> views = new List<UIElement>();
        private Dictionary<string, UIElement> namedViews = new Dictionary<string, UIElement>();

        public void Add(UIElement view)
        {
            views.Add(view);
        }

        public void Remove(UIElement view)
        {
            views.Remove(view);

            string viewName = GetNameFromView(view);

            if (viewName != null)
            {
                namedViews.Remove(viewName);
            }
        }

        private string GetNameFromView(UIElement view)
        {
            if (!namedViews.ContainsValue(view))
                return null;

            foreach (var key in namedViews.Keys)
            {
                if (namedViews[key] == view)
                {
                    return key;
                }
            }
            return null;
        }

        public IList<UIElement> Views
        {
            get { return views; }
        }

        public void Show(UIElement view)
        {
            ShowCalled = true;
        }

        public void Add(UIElement view, string name)
        {
            Add(view);
            namedViews.Add(name, view);
        }

        public UIElement GetView(string name)
        {
            if (namedViews.ContainsKey(name))
                return namedViews[name];

            return null;
        }
    }
}
