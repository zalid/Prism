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

namespace Prism.Tests.Regions
{
    [TestClass]
    public class RegionAdapterMappingsFixture
    {
        [TestMethod]
        public void ShouldGetRegisteredMapping()
        {
            var regionAdapterMappings = new RegionAdapterMappings();
            Type registeredType = typeof(ItemsControl);
            var regionAdapter = new MockRegionAdapter();

            regionAdapterMappings.RegisterMapping(registeredType, regionAdapter);
            var returnedAdapter = regionAdapterMappings.GetMapping(registeredType);

            Assert.IsNotNull(returnedAdapter);
            Assert.AreSame(regionAdapter, returnedAdapter);
        }

        [TestMethod]
        public void ShouldGetMappingForDerivedTypesThanTheRegisteredOnes()
        {
            var regionAdapterMappings = new RegionAdapterMappings();
            var regionAdapter = new MockRegionAdapter();

            regionAdapterMappings.RegisterMapping(typeof(ItemsControl), regionAdapter);
            var returnedAdapter = regionAdapterMappings.GetMapping(typeof(ItemsControlDescendant));

            Assert.IsNotNull(returnedAdapter);
            Assert.AreSame(regionAdapter, returnedAdapter);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetMappingOfUnregisteredTypeThrows()
        {
            var regionAdapterMappings = new RegionAdapterMappings();
            regionAdapterMappings.GetMapping(typeof(object));
        }

        [TestMethod]
        public void ShouldGetTheMostSpecializedMapping()
        {
            var regionAdapterMappings = new RegionAdapterMappings();
            var genericAdapter = new MockRegionAdapter();
            var specializedAdapter = new MockRegionAdapter();

            regionAdapterMappings.RegisterMapping(typeof(ItemsControl), genericAdapter);
            regionAdapterMappings.RegisterMapping(typeof(ItemsControlDescendant), specializedAdapter);
            var returnedAdapter = regionAdapterMappings.GetMapping(typeof(ItemsControlDescendant));

            Assert.IsNotNull(returnedAdapter);
            Assert.AreSame(specializedAdapter, returnedAdapter);
        }



        class ItemsControlDescendant : ItemsControl { }

        class MockRegionAdapter : IRegionAdapter
        {
            public IRegion Initialize(DependencyObject obj)
            {
                throw new NotImplementedException();
            }
        }

    }
}
