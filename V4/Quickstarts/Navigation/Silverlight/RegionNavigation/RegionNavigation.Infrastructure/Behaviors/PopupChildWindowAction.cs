//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System.Windows;
using System.Windows.Controls;

namespace RegionNavigation.Infrastructure.Behaviors
{
    /// <summary>
    /// Concrete class that pops up a specified child window or a default child window configured with a data template.
    /// </summary>
    public class PopupChildWindowAction : PopupChildWindowActionBase
    {
        public static readonly DependencyProperty ChildWindowProperty =
            DependencyProperty.Register(
                "ChildWindow",
                typeof(ChildWindow),
                typeof(PopupChildWindowAction),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(
                "ContentTemplate",
                typeof(DataTemplate),
                typeof(PopupChildWindowAction),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the child window to pop up.
        /// </summary>
        /// <remarks>
        /// If not specified, a default child window is used instead.
        /// </remarks>
        public ChildWindow ChildWindow
        {
            get { return (ChildWindow)GetValue(ChildWindowProperty); }
            set { SetValue(ChildWindowProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content template for a default child window.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        protected override ChildWindow GetChildWindow(object context)
        {
            var childWindow = this.ChildWindow;
            childWindow.DataContext = context;

            return childWindow;
        }
    }
}