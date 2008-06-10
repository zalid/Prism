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

using System.Collections.Generic;
using System.Windows;
using Prism.Interfaces;

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
            if (element != null)
            {
                IRegionManager regionManager = element.GetValue(RegionManagerProperty) as IRegionManager;
                if (regionManager != null)
                {
                    string oldRegionName = args.OldValue as string;
                    if (oldRegionName != null)
                    {
                        regionManager.Regions.Remove(oldRegionName);
                    }

                    string newRegionName = args.NewValue as string;
                    if (newRegionName != null)
                    {
                        regionManager.CreateRegion(element, newRegionName);
                    }
                }
            }
        }

        public static readonly DependencyProperty RegionManagerProperty =
            DependencyProperty.RegisterAttached("RegionManager", typeof(IRegionManager), typeof(RegionManager),
           new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, OnSetRegionManagerCallback));

        public static IRegionManager GetRegionManager(DependencyObject dependencyObject)
        {
            return (IRegionManager)dependencyObject.GetValue(RegionManagerProperty);
        }

        public static void SetRegionManager(DependencyObject dependencyObject, IRegionManager value)
        {
            dependencyObject.SetValue(RegionManagerProperty, value);
        }

        public static void OnSetRegionManagerCallback(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            if (element != null)
            {
                string regionName = element.GetValue(RegionNameProperty) as string;
                if (regionName != null)
                {
                    IRegionManager oldRegionManager = args.OldValue as IRegionManager;
                    if (oldRegionManager != null)
                    {
                        oldRegionManager.Regions.Remove(regionName);
                    }

                    IRegionManager newRegionManager = args.NewValue as IRegionManager;
                    if (newRegionManager != null)
                    {
                        newRegionManager.CreateRegion(element, regionName);
                    }
                }
            }
        }


        #endregion

        private readonly RegionAdapterMappings regionAdapterMappings;
        private readonly IDictionary<string, IRegion> _regions;

        public RegionManager()
        {
            _regions = new RegionsDictionary(this);
        }

        public IDictionary<string, IRegion> Regions
        {
            get { return _regions; }
        }

        public RegionManager(RegionAdapterMappings mappings)
            : this()
        {
            this.regionAdapterMappings = mappings;
        }

        public void CreateRegion(DependencyObject element, string regionName)
        {
            IRegionAdapter regionAdapter = regionAdapterMappings.GetMapping(element.GetType());
            IRegion region = regionAdapter.Initialize(element);
            Regions.Add(regionName, region);
        }


        public IRegionManager CreateRegionManager()
        {
            return new RegionManager(this.regionAdapterMappings);
        }

        class RegionsDictionary : Dictionary<string, IRegion>, IDictionary<string, IRegion>
        {
            private readonly IRegionManager regionManager;

            public RegionsDictionary(IRegionManager regionManager)
            {
                this.regionManager = regionManager;
            }

            void IDictionary<string, IRegion>.Add(string key, IRegion value)
            {
                base.Add(key, value);
                value.RegionManager = regionManager;
            }

            bool IDictionary<string, IRegion>.Remove(string key)
            {
                bool removed = false;
                if (this.ContainsKey(key))
                {
                    IRegion region = this[key];
                    removed = base.Remove(key);
                    region.RegionManager = null;
                }

                return removed;
            }
        }
    }
}