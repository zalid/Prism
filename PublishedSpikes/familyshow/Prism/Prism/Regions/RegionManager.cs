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
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using Prism.Interfaces;
using Prism.Properties;

namespace Prism.Regions
{
    public class RegionManager : IRegionManager
    {
        #region Static properties (for XAML support)

        public static readonly DependencyProperty RegionNameProperty = DependencyProperty.RegisterAttached(
            "RegionName",
            typeof(string),
            typeof(RegionManager),
            new PropertyMetadata(OnSetRegionNameCallback));


        public static void SetRegionName(DependencyObject containerElement, string regionName)
        {
            containerElement.SetValue(RegionNameProperty, regionName);
        }

        public static void OnSetRegionNameCallback(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            string regionName = args.NewValue.ToString();
            IRegionManager currentRegionManager = element.GetValue(RegionManagerProperty) as IRegionManager;

            if (currentRegionManager != null)
                currentRegionManager.CreateRegion(element, regionName);

            DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(RegionManagerProperty, typeof(DependencyObject));
            if (dependencyPropertyDescriptor != null)
            {

                dependencyPropertyDescriptor.AddValueChanged(element, delegate(object sender, EventArgs e)
                {
                    if (currentRegionManager != null)
                    {
                        currentRegionManager.Unregister(regionName);
                    }
                    IRegionManager newRegionManager = ((DependencyObject)sender).GetValue(RegionManagerProperty) as IRegionManager;
                    if (newRegionManager != null)
                    {
                        newRegionManager.CreateRegion((DependencyObject)sender, regionName);
                    }
                    currentRegionManager = newRegionManager;
                });
            }
        }

        public static readonly DependencyProperty RegionManagerProperty =
            DependencyProperty.RegisterAttached("RegionManager", typeof(IRegionManager), typeof(RegionManager),
           new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static IRegionManager GetRegionManager(DependencyObject obj)
        {
            return (IRegionManager)obj.GetValue(RegionManagerProperty);
        }

        public static void SetRegionManager(DependencyObject obj, IRegionManager value)
        {
            obj.SetValue(RegionManagerProperty, value);
        }

        #endregion

        private readonly RegionAdapterMappings regionAdapterMappings;
        private readonly Dictionary<string, IRegion> _regions = new Dictionary<string, IRegion>();

        public RegionManager()
        {
        }

        public RegionManager(RegionAdapterMappings mappings)
        {
            this.regionAdapterMappings = mappings;
        }

        public void Register(string regionName, IRegion region)
        {
            //Check to ensure a region with the same name is not already registered
            if (HasRegion(regionName))
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.RegionNameExistsException, regionName));

            _regions.Add(regionName, region);
            region.RegionManager = this;
        }

        public IRegion GetRegion(string regionName)
        {
            if (HasRegion(regionName))
                return _regions[regionName];

            throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, Resources.ValueNotFound, regionName));
        }

        public bool HasRegion(string regionName)
        {
            return _regions.ContainsKey(regionName);
        }

        public void CreateRegion(DependencyObject element, string regionName)
        {
            IRegionAdapter regionAdapter = regionAdapterMappings.GetMapping(element.GetType());
            IRegion region = regionAdapter.Initialize(element);
            Register(regionName, region);
        }


        public IRegionManager CreateRegionManager()
        {
            return new RegionManager(this.regionAdapterMappings);
        }

        public void Unregister(string regionName)
        {
            if (HasRegion(regionName))
            {
                IRegion region = _regions[regionName];
                _regions.Remove(regionName);
                region.RegionManager = null;
            }
        }
    }
}