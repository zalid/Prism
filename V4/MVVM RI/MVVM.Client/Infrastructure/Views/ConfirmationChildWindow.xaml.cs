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

namespace MVVM.Client.Infrastructure.Views
{
    public partial class ConfirmationChildWindow : ChildWindow
    {
        public static readonly DependencyProperty ConfirmationTemplateProperty =
            DependencyProperty.Register(
                "ConfirmationTemplate",
                typeof(DataTemplate),
                typeof(ConfirmationChildWindow),
                new PropertyMetadata(null));

        public ConfirmationChildWindow()
        {
            InitializeComponent();
        }

        public DataTemplate ConfirmationTemplate
        {
            get { return (DataTemplate)GetValue(ConfirmationTemplateProperty); }
            set { SetValue(ConfirmationTemplateProperty, value); }
        }
    }
}

