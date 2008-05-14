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
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Interfaces;
using Prism.Regions;
using Prism.Tests.Mocks;

namespace Prism.Tests.Regions
{
    [TestClass]
    public class RegionManagerFixture
    {
        [TestMethod]
        public void RegisterRegion()
        {
            IRegion region1 = new MockRegion();

            RegionManager regionManager = new RegionManager();
            regionManager.Register("MainRegion", region1);

            IRegion region2 = regionManager.GetRegion("MainRegion");
            Assert.AreSame(region1, region2);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ShouldFailIfRegionDoesntExists()
        {
            RegionManager regionManager = new RegionManager();
            regionManager.GetRegion("nonExistentRegion");
        }

        [TestMethod]
        public void CanCheckTheExistenceOfARegion()
        {
            RegionManager regionManager = new RegionManager();
            bool result = regionManager.HasRegion("noRegion");

            Assert.IsFalse(result);

            IRegion region = new MockRegion();

            regionManager.Register("noRegion", region);

            result = regionManager.HasRegion("noRegion");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RegisteringMultipleRegionsWithSameNameThrowsArgumentException()
        {
            var regionManager = new RegionManager();
            regionManager.Register("region name", new MockRegion());

            try
            {
                regionManager.Register("region name", new MockRegion());
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

        [TestMethod]
        public void RegisterPassesItselfAsTheRegionManagerOfTheRegion()
        {
            var regionManager = new RegionManager();
            var region = new MockRegion();
            regionManager.Register("region", region);

            Assert.AreSame(regionManager, region.RegionManager);
        }

        [TestMethod]
        public void CreateRegionManagerCreatesANewInstance()
        {
            var regionManager = new RegionManager();
            var createdRegionManager = regionManager.CreateRegionManager();
            Assert.IsNotNull(createdRegionManager);
            Assert.IsInstanceOfType(createdRegionManager, typeof(RegionManager));
            Assert.AreNotSame(regionManager, createdRegionManager);
        }

        [TestMethod]
        public void ShouldCreateRegionByUsingRegisteredRegionAdapterMappings()
        {
            var mappings = new RegionAdapterMappings();
            var mockRegionAdapter = new MockRegionAdapter();
            mappings.RegisterMapping(typeof(DependencyObject), mockRegionAdapter);
            var regionManager = new RegionManager(mappings);
            var control = new DependencyObject();

            regionManager.CreateRegion(control, "TestRegionName");

            Assert.IsTrue(mockRegionAdapter.InitializeCalled);
            Assert.AreEqual(control, mockRegionAdapter.InitializeArgument);
        }

        [TestMethod]
        public void CanUnregisterRegion()
        {
            var regionManager = new RegionManager();
            IRegion region = new MockRegion();
            regionManager.Register("TestRegion", region);

            regionManager.Unregister("TestRegion");

            Assert.IsFalse(regionManager.HasRegion("TestRegion"));
        }

        [TestMethod]
        public void ShouldRemoveRegionManagerWhenUnregistering()
        {
            var regionManager = new RegionManager();
            var region = new MockRegion();
            regionManager.Register("TestRegion", region);

            regionManager.Unregister("TestRegion");

            Assert.IsNull(region.RegionManager);
        }


        [TestMethod]
        public void RegionGetsRegisteredInRegionManagerWhenAddedIntoAScope()
        {
            var mappings = new RegionAdapterMappings();
            mappings.RegisterMapping(typeof(DependencyObject), new MockRegionAdapter());

            RegionManager regionManager = new RegionManager(mappings);
            var regionScopeControl = new ContentControl();
            RegionManager.SetRegionManager(regionScopeControl, regionManager);

            var control = new FrameworkElement();
            control.SetValue(RegionManager.RegionNameProperty, "TestRegion");

            Assert.IsFalse(regionManager.HasRegion("TestRegion"));
            regionScopeControl.Content = control;
            Assert.IsTrue(regionManager.HasRegion("TestRegion"));
            Assert.IsNotNull(regionManager.GetRegion("TestRegion"));
        }

        [TestMethod]
        public void RegionGetsUnregisteredFromRegionManagerWhenRemovedFromScope()
        {
            var mappings = new RegionAdapterMappings();
            mappings.RegisterMapping(typeof(DependencyObject), new MockRegionAdapter());

            RegionManager regionManager = new RegionManager(mappings);
            var regionScopeControl = new ContentControl();
            RegionManager.SetRegionManager(regionScopeControl, regionManager);
            var control = new FrameworkElement();
            control.SetValue(RegionManager.RegionNameProperty, "TestRegion");
            regionScopeControl.Content = control;
            Assert.IsTrue(regionManager.HasRegion("TestRegion"));

            regionScopeControl.Content = null;

            Assert.IsFalse(regionManager.HasRegion("TestRegion"));
        }

        /*
         * TODO: CreateRegion should throw if mappings is null
         */
    }
}