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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Controls;
using Prism.Regions;
using System.Windows;
using Prism.Interfaces;
using Prism.Utility;

namespace Prism.Tests.Regions
{
    /// <summary>
    /// Summary description for ItemsControlRegionFixture
    /// </summary>
    [TestClass]
    public class ItemsControlRegionFixture
    {
        [TestMethod]
        public void CanAddUIElementsToTabRegion()
        {
            TabControl tabControl = new TabControl();
            IRegion region = CreateItemsControlRegion(tabControl);

            Assert.AreEqual(0, tabControl.Items.Count);

            region.Add(new UIElement());

            Assert.AreEqual(1, tabControl.Items.Count);
        }


        [TestMethod]
        public void CanRemoveUIElementsFromTabRegion()
        {
            TabControl tabControl = new TabControl();
            IRegion region = CreateItemsControlRegion(tabControl);
            UIElement view = new UIElement();

            region.Add(view);
            region.Remove(view);

            Assert.AreEqual(0, tabControl.Items.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveInexistentViewThrows()
        {
            TabControl tabControl = new TabControl();
            IRegion region = CreateItemsControlRegion(tabControl);
            UIElement view = new UIElement();

            region.Remove(view);

            Assert.AreEqual(0, tabControl.Items.Count);
        }

        [TestMethod]
        public void AddedViewElementGetsSelected()
        {
            TabControl tabControl = new TabControl();
            IRegion region = CreateItemsControlRegion(tabControl);

            UIElement element = new UIElement();
            region.Add(element);

            Assert.AreEqual(element, tabControl.SelectedItem);
        }


        [TestMethod]
        public void RegionExposesCollectionOfContainedViews()
        {
            TabControl tabControl = new TabControl();
            IRegion region = CreateItemsControlRegion(tabControl);

            UIElement view = new UIElement();

            region.Add(view);

            IList<UIElement> views = region.Views;

            Assert.IsNotNull(views);
            Assert.AreEqual(1, views.Count);
            Assert.AreSame(view, views[0]);
        }


        [TestMethod]
        public void ShowOnExistingViewSelectsTheViewContainer()
        {
            TabControl tabControl = new TabControl();
            IRegion region = CreateItemsControlRegion(tabControl);

            TextBlock view1 = new TextBlock();
            TextBlock view2 = new TextBlock();

            region.Add(view1);
            region.Add(view2);

            Assert.AreSame(view2, tabControl.SelectedItem);

            region.Show(view1);

            Assert.AreSame(view1, tabControl.SelectedItem);
            Assert.AreEqual(2, region.Views.Count);
        }

        [TestMethod]
        public void RegionSetsIsActivePropertyOnIActiveAwareViews()
        {
            TabControl tabControl = new TabControl();
            IRegion region = CreateItemsControlRegion(tabControl);
            MockActiveAwareView view = new MockActiveAwareView();
            
            Assert.IsFalse(view.IsActive);

            region.Add(view);

            Assert.IsTrue(view.IsActive);

            region.Add(new UIElement());

            Assert.IsFalse(view.IsActive);

            tabControl.SelectedItem = view;

            Assert.IsTrue(view.IsActive);
        }


        //TODO: Do we want this behavior or the other way around (Show adds and activates, and Add just adds)?
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShowWithNonAddedViewThrows()
        {
            TabControl tabControl = new TabControl();
            IRegion region = CreateItemsControlRegion(tabControl);

            TextBlock nonAddedView = new TextBlock();

            region.Show(nonAddedView);
        }

        [TestMethod]
        public void CanAddAndRetrieveNamedViewInstance()
        {
            IRegion region = CreateItemsControlRegion(new ItemsControl());
            UIElement myView = new UIElement();
            
            region.Add(myView, "MyView");
            object returnedView = region.GetView("MyView");

            Assert.IsNotNull(returnedView);
            Assert.AreSame(returnedView, myView);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddingDuplicateNamedViewThrows()
        {
            IRegion region = CreateItemsControlRegion(new ItemsControl());

            region.Add(new UIElement(), "MyView");
            region.Add(new UIElement(), "MyView");
        }

        [TestMethod]
        public void AddNamedViewIsAlsoListedInViewsCollection()
        {
            IRegion region = CreateItemsControlRegion(new ItemsControl());
            UIElement myView = new UIElement();

            region.Add(myView, "MyView");

            Assert.AreEqual(1, region.Views.Count);
            Assert.AreSame(myView, region.Views[0]);
        }

        [TestMethod]
        public void GetViewReturnsNullWhenViewDoesNotExistInRegion()
        {
            IRegion region = CreateItemsControlRegion(new ItemsControl());

            Assert.IsNull(region.GetView("InexistentView"));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetViewWithNullOrEmptyStringThrows()
        {
            IRegion region = CreateItemsControlRegion(new ItemsControl());

            region.GetView(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNamedViewWithNullOrEmptyStringNameThrows()
        {
            IRegion region = CreateItemsControlRegion(new ItemsControl());

            region.Add(new UIElement(), string.Empty);
        }

        [TestMethod]
        public void GetViewReturnsNullAfterRemovingViewFromRegion()
        {
            IRegion region = CreateItemsControlRegion(new ItemsControl());
            UIElement myView = new UIElement();

            region.Add(myView, "MyView");
            region.Remove(myView);

            Assert.IsNull(region.GetView("MyView"));
        }

        private static IRegion CreateItemsControlRegion(ItemsControl control)
        {
            ItemsControlRegion region = new ItemsControlRegion();
            region.Initialize(control);
            return region;
        }

        class MockActiveAwareView : UIElement, IActiveAware
        {
            #region IActiveAware Members

            public bool IsActive
            {
                get;
                set;
            }

            public event EventHandler IsActiveChanged;

            #endregion
        }
    }
}
