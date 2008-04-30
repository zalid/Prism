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
using Prism.UnityContainerAdapter;
using Prism.Tests.Mocks;
using System.Data.Linq;

namespace Prism.Tests.Services
{
    [TestClass]
    public class RegionManagerServiceFixture
    {
        [TestInitialize]
        [TestCleanup]
        public void TestCleanup()
        {
            PrismContainerProvider.Provider = null;
        }

        [TestMethod]
        public void RegisterRegion()
        {
            IRegion region1 = new MockRegion();

            RegionManagerService regionManager = new RegionManagerService();
            regionManager.Register("MainRegion", region1);

            IRegion region2 = regionManager.GetRegion("MainRegion");
            Assert.AreSame(region1, region2);
        }

        [TestMethod]
        public void MarkControlForRegion()
        {
            IPrismContainer prismContainer = new MockPrismContainer();

            PrismContainerProvider.Provider = prismContainer;

            Panel panel = new StackPanel();
            RegionManagerService regionManager = new RegionManagerService();
            regionManager.SetRegion(panel, "MainRegion");

            MockRegion region = (MockRegion)regionManager.GetRegion("MainRegion");

            Assert.AreSame(panel, region.InitializedControl);

            PrismContainerProvider.Provider = null;
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ShouldFailIfRegionDoesntExists()
        {
            RegionManagerService regionManager = new RegionManagerService();
            regionManager.GetRegion("nonExistentRegion");
        }

        [TestMethod]
        public void CanCheckTheExistenceOfARegion()
        {
            RegionManagerService regionManager = new RegionManagerService();
            bool result = regionManager.HasRegion("noRegion");

            Assert.IsFalse(result);

            IRegion region = new MockRegion();

            regionManager.Register("noRegion", region);

            result = regionManager.HasRegion("noRegion");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RegionGetsRegisteredInRegionManagerWhenAddedIntoAScope()
        {
            IPrismContainer prismContainer = new MockPrismContainer();

            PrismContainerProvider.Provider = prismContainer;

            RegionManagerService regionManager = new RegionManagerService();
            var regionScopeControl = new ContentControl();
            RegionManager.SetRegionManagerServiceScope(regionScopeControl, regionManager);

            Panel panel = new StackPanel();
            panel.SetValue(RegionManager.RegionProperty, "MainRegion");

            regionScopeControl.Content = panel;
            Assert.IsNotNull(regionManager.GetRegion("MainRegion"));

            PrismContainerProvider.Provider = null;
        }

        [TestMethod]
        public void RegisteringMultipleRegionsWithSameNameThrowsArgumentException()
        {
            var regionManagerService = new RegionManagerService();
            regionManagerService.Register("region name", new MockRegion());

            try
            {
                regionManagerService.Register("region name", new MockRegion());
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Region with the given name is already registered: region name", ex.Message);
            }
            catch
            {
                Assert.Fail();                
            }

        }
	
    }
}
