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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Interfaces;
using Prism.Regions;
using System.Windows.Controls;
using System.Windows;
using Prism.Services;
using Microsoft.Practices.Unity;
using Prism.UnityContainerAdapter;

namespace Prism.Tests.Services
{
    [TestClass]
    public class RegionManagerServiceFixture
    {
        [TestMethod]
        public void RegisterRegion()
        {
            IRegion region1 = new MockRegion();

            using (RegionManagerService regionManager = new RegionManagerService())
            {
                regionManager.Register("MainRegion", region1);

                IRegion region2 = regionManager.GetRegion("MainRegion");
                Assert.AreSame(region1, region2);
            }
        }

        [TestMethod]
        public void MarkViewForRegion()
        {
            IUnityContainer container = new UnityContainer();
            IPrismContainer prismContainer = new UnityPrismContainer(container);
            container.RegisterType<IRegion<Panel>, PanelRegion>();

            PrismContainerProvider.Provider = prismContainer;

            Panel panel = new StackPanel();
            using (RegionManagerService regionManager = new RegionManagerService())
            {
                regionManager.SetRegion(panel, "MainRegion");

                IRegion region = regionManager.GetRegion("MainRegion");
                UIElement el = new UIElement();
                region.Add(el);

                Assert.AreSame(el, panel.Children[0]);
            }

            PrismContainerProvider.Provider = null;
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ShouldFailIfRegionDoesntExists()
        {
            using (RegionManagerService regionManager = new RegionManagerService())
            {
                regionManager.GetRegion("nonExistentRegion");
            }
        }

        [TestMethod]
        public void CanCheckTheExistenceOfARegion()
        {
            using (RegionManagerService regionManager = new RegionManagerService())
            {
                bool result = regionManager.HasRegion("noRegion");

                Assert.IsFalse(result);

                IRegion region = new MockRegion();

                regionManager.Register("noRegion", region);

                result = regionManager.HasRegion("noRegion");

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void RegionGetsRegisteredWhenSettingValueThroughDependencyProperty()
        {
            IUnityContainer container = new UnityContainer();
            IPrismContainer prismContainer = new UnityPrismContainer(container);
            container.RegisterType<IRegion<Panel>, PanelRegion>();

            PrismContainerProvider.Provider = prismContainer;

            using (RegionManagerService regionManager = new RegionManagerService())
            {
                Panel panel = new StackPanel();

                panel.SetValue(RegionManager.RegionProperty, "MainRegion");

                IRegion region = regionManager.GetRegion("MainRegion");
                Assert.IsNotNull("MainRegion");
            }

            PrismContainerProvider.Provider = null;
        }

        class MockRegion : IRegion
        {
            public UIElement ShowArgumentView;

            private IList<UIElement> views = new List<UIElement>();

            #region IRegion Members

            public void Add(UIElement view)
            {
                views.Add(view);
            }

            public void Remove(UIElement view)
            {
                throw new NotImplementedException();
            }

            public IList<UIElement> Views
            {
                get { return views; }
            }

            public void Show(UIElement view)
            {
                ShowArgumentView = view;
            }

            public void Add(UIElement view, string name)
            {
                throw new NotImplementedException();
            }

            public UIElement GetView(string name)
            {
                throw new NotImplementedException();
            }

            #endregion

            #region IRegionAdapter Members

            public void Initialize(DependencyObject obj)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        
    }
}
