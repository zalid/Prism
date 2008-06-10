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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Interfaces;
using Prism.Regions;
using System.Windows.Controls;

namespace Prism.Tests.Regions
{
    [TestClass]
    public class SimpleRegionFixture
    {
        [TestMethod]
        public void CanAddContentToRegion()
        {
            IRegion region = new SimpleRegion();

            Assert.AreEqual(0, region.Views.Cast<object>().Count());

            region.Add(new object());

            Assert.AreEqual(1, region.Views.Cast<object>().Count());
        }


        [TestMethod]
        public void CanRemoveContentFromRegion()
        {
            IRegion region = new SimpleRegion();
            object view = new object();

            region.Add(view);
            region.Remove(view);

            Assert.AreEqual(0, region.Views.Cast<object>().Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveInexistentViewThrows()
        {
            IRegion region = new SimpleRegion();
            object view = new object();

            region.Remove(view);

            Assert.AreEqual(0, region.Views.Cast<object>().Count());
        }



        [TestMethod]
        public void RegionExposesCollectionOfContainedViews()
        {
            IRegion region = new SimpleRegion();

            object view = new object();

            region.Add(view);

            var views = region.Views;

            Assert.IsNotNull(views);
            Assert.AreEqual(1, views.Cast<object>().Count());
            Assert.AreSame(view, views.Cast<object>().ElementAt(0));
        }


        //[TestMethod]
        //public void ShowOnExistingViewSelectsTheViewContainer()
        //{
        //    IRegion region = new SimpleRegion();

        //    object view1 = new object();
        //    object view2 = new object();

        //    region.Add(view1);
        //    region.Add(view2);

        //    Assert.AreSame(view1, tabControl.SelectedItem);

        //    region.Show(view2);

        //    Assert.AreSame(view2, tabControl.SelectedItem);
        //    Assert.AreEqual(2, region.Views.Cast<object>().Count());
        //}

        [TestMethod]
        public void RegionSetsIsActivePropertyOnIActiveAwareViews()
        {
            TabControl tabControl = new TabControl();
            tabControl.IsSynchronizedWithCurrentItem = true;

            IRegion region = new SimpleRegion();
            tabControl.ItemsSource = region.Views;
            MockActiveAwareView view = new MockActiveAwareView();

            Assert.IsFalse(view.IsActive);

            region.Add(view);
            region.Activate(view);

            Assert.IsTrue(view.IsActive);

            var view2 = new object();
            region.Add(view2);
            region.Activate(view2);

            Assert.IsFalse(view.IsActive);

            tabControl.SelectedItem = view;

            Assert.IsTrue(view.IsActive);

            region.Activate(view2);

            Assert.AreSame(view2, tabControl.SelectedItem);
        }


        [TestMethod]
        public void ShowWithNonAddedViewAddsViewFirst()
        {
            IRegion region = new SimpleRegion();

            object nonAddedView = new object();

            Assert.AreEqual(0, region.Views.Cast<object>().Count());
            region.Activate(nonAddedView);
            Assert.AreEqual(1, region.Views.Cast<object>().Count());
        }

        [TestMethod]
        public void CanAddAndRetrieveNamedViewInstance()
        {
            IRegion region = new SimpleRegion();
            object myView = new object();
            region.Add(myView, "MyView");
            object returnedView = region.GetView("MyView");

            Assert.IsNotNull(returnedView);
            Assert.AreSame(returnedView, myView);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddingDuplicateNamedViewThrows()
        {
            IRegion region = new SimpleRegion();

            region.Add(new object(), "MyView");
            region.Add(new object(), "MyView");
        }

        [TestMethod]
        public void AddNamedViewIsAlsoListedInViewsCollection()
        {
            IRegion region = new SimpleRegion();
            object myView = new object();

            region.Add(myView, "MyView");

            Assert.AreEqual(1, region.Views.Cast<object>().Count());
            Assert.AreSame(myView, region.Views.Cast<object>().ElementAt(0));
        }

        [TestMethod]
        public void GetViewReturnsNullWhenViewDoesNotExistInRegion()
        {
            IRegion region = new SimpleRegion();

            Assert.IsNull(region.GetView("InexistentView"));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetViewWithNullOrEmptyStringThrows()
        {
            IRegion region = new SimpleRegion();

            region.GetView(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNamedViewWithNullOrEmptyStringNameThrows()
        {
            IRegion region = new SimpleRegion();

            region.Add(new object(), string.Empty);
        }

        [TestMethod]
        public void GetViewReturnsNullAfterRemovingViewFromRegion()
        {
            IRegion region = new SimpleRegion();
            object myView = new object();
            region.Add(myView, "MyView");
            region.Remove(myView);

            Assert.IsNull(region.GetView("MyView"));
        }

        [TestMethod]
        public void AddViewPassesSameScopeByDefaultToView()
        {
            var regionManager = new MockRegionManager();
            IRegion region = new SimpleRegion();
            region.RegionManager = regionManager;
            var myView = new DependencyObject();

            region.Add(myView);

            Assert.AreSame(regionManager, myView.GetValue(RegionManager.RegionManagerProperty));
        }

        [TestMethod]
        public void AddViewPassesSameScopeByDefaultToNamedView()
        {
            var regionManager = new MockRegionManager();
            IRegion region = new SimpleRegion();
            region.RegionManager = regionManager;
            var myView = new DependencyObject();

            region.Add(myView, "MyView");

            Assert.AreSame(regionManager, myView.GetValue(RegionManager.RegionManagerProperty));
        }

        [TestMethod]
        public void AddViewPassesDiferentScopeWhenAdding()
        {
            var regionManager = new MockRegionManager();
            IRegion region = new SimpleRegion();
            region.RegionManager = regionManager;
            var myView = new DependencyObject();

            region.Add(myView, "MyView", true);

            Assert.AreNotSame(regionManager, myView.GetValue(RegionManager.RegionManagerProperty));
        }

        [TestMethod]
        public void CreatingNewScopesAsksTheRegionManagerForNewInstance()
        {
            var regionManager = new MockRegionManager();
            IRegion region = new SimpleRegion();
            region.RegionManager = regionManager;
            var myView = new object();

            region.Add(myView, "MyView", true);

            Assert.IsTrue(regionManager.CreateRegionManagerCalled);
        }

        [TestMethod]
        public void AddViewReturnsExistingRegionManager()
        {
            var regionManager = new MockRegionManager();
            IRegion region = new SimpleRegion();
            region.RegionManager = regionManager;
            var myView = new object();

            var returnedRegionManager = region.Add(myView, "MyView", false);

            Assert.AreSame(regionManager, returnedRegionManager);
        }

        [TestMethod]
        public void AddViewReturnsNewRegionManager()
        {
            var regionManager = new MockRegionManager();
            IRegion region = new SimpleRegion();
            region.RegionManager = regionManager;
            var myView = new object();

            var returnedRegionManager = region.Add(myView, "MyView", true);

            Assert.AreNotSame(regionManager, returnedRegionManager);
        }

        [TestMethod]
        public void AddingNonDependencyObjectToRegionDoesNotThrow()
        {
            IRegion region = new SimpleRegion();
            object model = new object();

            region.Add(model);

            Assert.AreEqual(1, region.Views.Cast<object>().Count());
        }

        //[TestMethod]
        //public void ShouldSelectFirstViewAdded()
        //{
        //    IRegion region = new SimpleRegion();
        //    var view = new object();

        //    Assert.AreEqual(0, region.Views.Cast<object>().Count());
        //    region.Add(view);
        //    Assert.AreEqual(view, tabControl.SelectedItem);
        //}

        //[TestMethod]
        //public void ShouldNotSelectAdditionalViewsAdded()
        //{
        //    IRegion region = new SimpleRegion();
        //    var view = new object();
        //    var view2 = new object();

        //    region.Add(view);
        //    region.Add(view2);

        //    Assert.AreNotEqual(view2, tabControl.SelectedItem);
        //}

        [TestMethod]
        public void AddViewRaisesCollectionViewEvent()
        {
            bool viewAddedCalled = false;

            IRegion region = new SimpleRegion();
            region.Views.CollectionChanged += (sender, e) =>
                                                  {
                                                      if (e.Action == NotifyCollectionChangedAction.Add)
                                                          viewAddedCalled = true;
                                                  };

            object model = new object();
            Assert.IsFalse(viewAddedCalled);
            region.Add(model);

            Assert.IsTrue(viewAddedCalled);
        }

        [TestMethod]
        public void ViewAddedEventPassesTheViewAddedInTheEventArgs()
        {
            object viewAdded = null;

            IRegion region = new SimpleRegion();
            region.Views.CollectionChanged += (sender, e) =>
            {
                viewAdded = e.NewItems[0];
            };
            object model = new object();
            Assert.IsNull((viewAdded));
            region.Add(model);

            Assert.AreSame(model, viewAdded);
        }

        [TestMethod]
        public void RemoveViewFiresViewRemovedEvent()
        {
            bool viewRemovedCalled = false;

            IRegion region = new SimpleRegion();
            region.Views.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Remove)
                    viewRemovedCalled = true;
            };

            object model = new object();
            region.Add(model);

            Assert.IsFalse(viewRemovedCalled);

            region.Remove(model);

            Assert.IsTrue(viewRemovedCalled);
        }

        [TestMethod]
        public void ViewRemovedEventPassesTheViewRemovedInTheEventArgs()
        {
            object viewRemoved = null;

            IRegion region = new SimpleRegion();
            region.Views.CollectionChanged += (sender, e) =>
                    {
                        if (e.Action == NotifyCollectionChangedAction.Remove)
                            viewRemoved = e.OldItems[0];
                    };
            object model = new object();
            region.Add(model);

            Assert.IsNull(viewRemoved);

            region.Remove(model);

            Assert.AreSame(model, viewRemoved);
        }

        [TestMethod]
        public void ShowViewFiresViewShowedEvent()
        {
            bool viewShowedCalled = false;

            IRegion region = new SimpleRegion();
            region.Views.CurrentChanged += (o, e) => viewShowedCalled = true;

            object model = new object();

            region.Add(model);

            Assert.IsFalse(viewShowedCalled);

            region.Activate(model);

            Assert.IsTrue(viewShowedCalled);
        }

        [TestMethod]
        public void ViewShowedEventPassesTheViewShowedInTheEventArgs()
        {
            object viewShowed = null;

            IRegion region = new SimpleRegion();
            region.Views.CurrentChanged +=
                ((sender, e) => { viewShowed = ((ICollectionView)sender).CurrentItem; });

            object model = new object();
            region.Add(model);

            Assert.IsNull((viewShowed));

            region.Activate((model));

            Assert.AreSame(model, viewShowed);
        }

        [TestMethod]
        public void AddingSameViewTwiceThrows()
        {
            object view = new object();
            IRegion region = new SimpleRegion();
            region.Add(view);

            try
            {
                region.Add(view);
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual("View already exists in region.", ex.Message);
            }
            catch
            {
                Assert.Fail();
            }

        }


        class MockActiveAwareView : IActiveAware
        {
            #region IActiveAware Members

            public bool IsActive { get; set; }

            public event EventHandler IsActiveChanged;

            #endregion
        }

        internal class MockRegionManager : IRegionManager
        {
            public bool CreateRegionManagerCalled;

            public IRegionManager CreateRegionManager()
            {
                CreateRegionManagerCalled = true;
                return new MockRegionManager();
            }

            public IDictionary<string, IRegion> Regions
            {
                get { throw new NotImplementedException(); }
            }

            public void CreateRegion(DependencyObject element, string regionName)
            {
                throw new NotImplementedException();
            }
        }
    }
}
