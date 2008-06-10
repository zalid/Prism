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
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Interfaces;
using Prism.Regions;
using Prism.Tests.Mocks;

namespace Prism.Tests.Regions
{
    [TestClass]
    public class ContentControlRegionAdapterFixture
    {
        [TestMethod]
        public void ContentAlwaysShowsViewsCurrentItem()
        {
            var control = new ContentControl();
            var adapter = new TestableContentControlRegionAdapter();

            var region = (NewMockRegion)adapter.Initialize(control);

            Assert.IsNotNull(region);

            var view1 = new object();
            var view2 = new object();
            region.UnderlyingCollection.Add(view1);
            region.UnderlyingCollection.Add(view2);

            region.Views.MoveCurrentTo(view1);
            Assert.AreSame(control.Content, view1);

            region.Views.MoveCurrentTo(view2);
            Assert.AreSame(control.Content, view2);
        }

        [TestMethod]
        public void ControlWithExistingContentThrows()
        {
            var control = new ContentControl() { Content = new object() };

            IRegionAdapter adapter = new TestableContentControlRegionAdapter();

            try
            {
                var region = (NewMockRegion)adapter.Initialize(control);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
                StringAssert.Contains(ex.Message, "ContentControl's Content property must not be set when also used with Prism Regions.");
            }
        }

        [TestMethod]
        public void ControlWithExistingBindingOnItemsSourceWithNullValueThrows()
        {
            var control = new ContentControl();
            Binding binding = new Binding("ObjectContents");
            binding.Source = new SimpleModel() { ObjectContents = null };
            BindingOperations.SetBinding(control, ContentControl.ContentProperty, binding);

            IRegionAdapter adapter = new TestableContentControlRegionAdapter();

            try
            {
                var region = (NewMockRegion)adapter.Initialize(control);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
                StringAssert.Contains(ex.Message, "ContentControl's Content property must not be set when also used with Prism Regions.");
            }
        }

        class SimpleModel
        {
            public Object ObjectContents { get; set; }
        }

    }

    internal class TestableContentControlRegionAdapter : ContentControlRegionAdapter
    {
        private NewMockRegion region = new NewMockRegion();

        public override IRegion CreateRegion()
        {
            return region;
        }
    }
}
