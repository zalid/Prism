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

namespace UIComposition.Infrastructure.Regions
{
    using System;
    using System.Collections.Generic;
    using Prism.Interfaces;
    using UIComposition.Infrastructure.Controls;
    using System.Windows;
    using System.Globalization;

    public class DeckRegion : IRegion<DeckPanel>
    {
        private Dictionary<string, UIElement> namedViews = new Dictionary<string, UIElement>();

        public DeckPanel WrappedDeckPanel { get; set; }

        public IList<System.Windows.UIElement> Views
        {
            get
            {
                List<UIElement> views = new List<UIElement>(WrappedDeckPanel.Children.Count);

                foreach (UIElement item in WrappedDeckPanel.Children)
                {
                    views.Add(item);
                }

                return views;
            }
        }

        public void Add(System.Windows.UIElement view)
        {
            WrappedDeckPanel.Children.Add(view);
            WrappedDeckPanel.InvalidateMeasure();

            Show(view);
        }

        public void Add(System.Windows.UIElement view, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(name);

            if (namedViews.ContainsKey(name))
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "View with name '{0}' already exists in the region", name));

            Add(view);

            namedViews.Add(name, view);
        }

        public void Remove(System.Windows.UIElement view)
        {
            if (!WrappedDeckPanel.Children.Contains(view))
                throw new ArgumentException("The region does not contains the view you want to remove", "view");

            WrappedDeckPanel.Children.Remove(view);
            WrappedDeckPanel.InvalidateMeasure();

            string viewName = GetNameFromView(view);
            if (viewName != null)
            {
                namedViews.Remove(viewName);
            }
        }

        public void Show(System.Windows.UIElement view)
        {
            if (!Views.Contains(view))
                throw new ArgumentException("The region does not contains the view you want to show", "view");

            HideViews();

            view.Visibility = Visibility.Visible;
        }

        public System.Windows.UIElement GetView(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(name);

            if (namedViews.ContainsKey(name))
            {
                return namedViews[name];
            }
            return null;
        }

        public void Initialize(System.Windows.DependencyObject obj)
        {
            WrappedDeckPanel = (DeckPanel)obj;
        }

        private void HideViews()
        {
            foreach (UIElement view in this.Views)
            {
                view.Visibility = Visibility.Hidden;
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
    }
}
