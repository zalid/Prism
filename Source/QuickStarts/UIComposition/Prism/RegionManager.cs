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
using Prism.Interfaces;
using System.Windows;
using Prism.Services;
using System.ComponentModel;

namespace Prism
{
    public static class RegionManager
    {
        public static readonly DependencyProperty RegionProperty = DependencyProperty.RegisterAttached(
            "Region",
            typeof(string),
            typeof(RegionManager),
            new PropertyMetadata(OnSetRegionCallback));

        public static readonly DependencyProperty RegionManagerServiceScopeProperty = DependencyProperty.RegisterAttached(
           "RegionManagerServiceScope",
           typeof(RegionManagerService),
           typeof(RegionManager),
           new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));


        public static event EventHandler<RegionPropertyChangedEventArgs> RegionPropertyChanged;


        public static void SetRegion(DependencyObject containerElement, string regionName)
        {
            containerElement.SetValue(RegionProperty, regionName);
        }

        public static void OnSetRegionCallback(DependencyObject containerElement, DependencyPropertyChangedEventArgs args)
        {
            DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(RegionManagerServiceScopeProperty, typeof(DependencyObject));

            IRegionManagerService regionManagerService = containerElement.GetValue(RegionManagerServiceScopeProperty) as IRegionManagerService;

            if (regionManagerService != null)
                regionManagerService.SetRegion(containerElement, args.NewValue.ToString());

            if (dependencyPropertyDescriptor != null)
            {
                dependencyPropertyDescriptor.AddValueChanged(containerElement, delegate(object sender, EventArgs e)
                {
                    IRegionManagerService rms = (sender as DependencyObject).GetValue(RegionManagerServiceScopeProperty) as IRegionManagerService;

                    rms.SetRegion((DependencyObject)sender, args.NewValue.ToString());
                });
            }
        }

        public static void SetRegionManagerServiceScope(DependencyObject containerElement, IRegionManagerService regionManager)
        {
            containerElement.SetValue(RegionManagerServiceScopeProperty, regionManager);
        }

        public static IRegionManagerService GetRegionManagerServiceScope(DependencyObject d)
        {
            return (IRegionManagerService)d.GetValue(RegionManagerServiceScopeProperty);
        }
    }
}
