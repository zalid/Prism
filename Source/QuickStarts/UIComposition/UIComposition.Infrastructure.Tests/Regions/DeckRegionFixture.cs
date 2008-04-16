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
using UIComposition.Infrastructure.Controls;
using Prism.Interfaces;
using UIComposition.Infrastructure.Regions;
using System.Windows;

namespace UIComposition.Infrastructure.Tests.Regions
{
    [TestClass]
    public class DeckRegionFixture
    {
        [TestMethod]
        public void CanAddUIElementsToDeckRegion()
        {
            DeckPanel panel = new DeckPanel();
            IRegion region = CreateDeckRegion(panel);

            Assert.AreEqual(0, panel.Children.Count);

            region.Add(new UIElement());

            Assert.AreEqual(1, panel.Children.Count);
        }

        [TestMethod]
        public void CanRemoveUIElementsFromTabRegion()
        {
            DeckPanel panel = new DeckPanel();
            IRegion region = CreateDeckRegion(panel);

            UIElement view = new UIElement();

            region.Add(view);
            region.Remove(view);

            Assert.AreEqual(0, panel.Children.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveInexistentViewThrows()
        {
            DeckPanel panel = new DeckPanel();
            IRegion region = CreateDeckRegion(panel);

            UIElement view = new UIElement();

            region.Remove(view);

            Assert.AreEqual(0, panel.Children.Count);
        }

        [TestMethod]
        public void AddedViewElementGetsVisible()
        {
            DeckPanel panel = new DeckPanel();
            IRegion region = CreateDeckRegion(panel);

            UIElement element = new UIElement();
            element.Visibility = Visibility.Hidden;
            region.Add(element);

            Assert.AreEqual(Visibility.Visible, element.Visibility);
        }


        [TestMethod]
        public void RegionExposesCollectionOfContainedViews()
        {
            DeckPanel panel = new DeckPanel();
            IRegion region = CreateDeckRegion(panel);

            region.Add(new UIElement());

            IList<UIElement> views = region.Views;

            Assert.IsNotNull(views);
            Assert.AreEqual(1, views.Count);
        }

        [TestMethod]
        public void AddViewHideTheExistentViewsAndShowTheViewAdded()
        {
            DeckPanel panel = new DeckPanel();
            IRegion region = CreateDeckRegion(panel);

            UIElement view1 = new UIElement();
            UIElement view2 = new UIElement();

            region.Add(view1);
            region.Add(view2);

            Assert.AreEqual(Visibility.Hidden, view1.Visibility);
            Assert.AreEqual(Visibility.Visible, view2.Visibility);
        }

        [TestMethod]
        public void ShowOnExistingViewHideExistentViewAndShowTheView()
        {
            DeckPanel panel = new DeckPanel();
            IRegion region = CreateDeckRegion(panel);

            UIElement view1 = new UIElement();
            UIElement view2 = new UIElement();

            region.Add(view1);
            region.Add(view2);

            Assert.AreEqual(Visibility.Hidden, view1.Visibility);
            Assert.AreEqual(Visibility.Visible, view2.Visibility);

            region.Show(view1);

            Assert.AreEqual(Visibility.Visible, view1.Visibility);
            Assert.AreEqual(Visibility.Hidden, view2.Visibility);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShowWithNonAddedViewThrows()
        {
            DeckPanel panel = new DeckPanel();
            IRegion region = CreateDeckRegion(panel);

            UIElement nonAddedView = new UIElement();

            region.Show(nonAddedView);
        }

        [TestMethod]
        public void CanAddAndRetrieveNamedViewInstance()
        {
            DeckPanel panel = new DeckPanel();
            IRegion region = CreateDeckRegion(panel);

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
            DeckPanel panel = new DeckPanel();
            IRegion region = CreateDeckRegion(panel);

            region.Add(new UIElement(), "MyView");
            region.Add(new UIElement(), "MyView");
        }

        [TestMethod]
        public void AddNamedViewIsAlsoListedInViewsCollection()
        {
            DeckPanel panel = new DeckPanel();
            IRegion region = CreateDeckRegion(panel);

            UIElement myView = new UIElement();

            region.Add(myView, "MyView");

            Assert.AreEqual(1, region.Views.Count);
            Assert.AreSame(myView, region.Views[0]);
        }

        [TestMethod]
        public void GetViewReturnsNullWhenViewDoesNotExistInRegion()
        {
            DeckPanel panel = new DeckPanel();
            IRegion region = CreateDeckRegion(panel);

            Assert.IsNull(region.GetView("InexistentView"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetViewWithNullOrEmptyStringThrows()
        {
            DeckPanel panel = new DeckPanel();
            IRegion region = CreateDeckRegion(panel);

            region.GetView(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNamedViewWithNullOrEmptyStringNameThrows()
        {
            DeckPanel panel = new DeckPanel();
            IRegion region = CreateDeckRegion(panel);

            region.Add(new UIElement(), string.Empty);
        }

        [TestMethod]
        public void GetViewReturnsNullAfterRemovingViewFromRegion()
        {
            DeckPanel panel = new DeckPanel();
            IRegion region = CreateDeckRegion(panel);

            UIElement myView = new UIElement();

            region.Add(myView, "MyView");
            region.Remove(myView);

            Assert.IsNull(region.GetView("MyView"));
        }


        private IRegion CreateDeckRegion(DeckPanel panel)
        {
            DeckRegion region = new DeckRegion();
            region.Initialize(panel);

            return region;
        }
    }
}