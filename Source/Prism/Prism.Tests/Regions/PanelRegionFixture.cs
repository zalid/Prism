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
using System.Windows;
using Prism.Interfaces;
using Prism.Regions;
using Prism.Utility;

namespace Prism.Tests.Regions
{
    [TestClass]
    public class PanelRegionFixture
    {
        [TestMethod]
        public void CanAddMultipleUIElementsToStackPanelRegion()
        {
            StackPanel stackPanel = new StackPanel();
            IRegion region = CreatePanelRegion(stackPanel);

            Assert.AreEqual(0, stackPanel.Children.Count);

            region.Add(new UIElement());

            Assert.AreEqual(1, stackPanel.Children.Count);

            region.Add(new UIElement());

            Assert.AreEqual(2, stackPanel.Children.Count);
        }

        [TestMethod]
        public void CanRemoveUIElementsFromTabRegion()
        {
            StackPanel stackPanel = new StackPanel();
            IRegion region = CreatePanelRegion(stackPanel);
            UIElement view = new UIElement();

            region.Add(view);
            region.Remove(view);

            Assert.AreEqual(0, stackPanel.Children.Count);
        }

        private static PanelRegion CreatePanelRegion(Panel panel)
        {
            PanelRegion region = new PanelRegion();
            region.Initialize(panel);
            return region;
        }
    }
}
