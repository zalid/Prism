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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace MVVM.Client.Infrastructure.Views
{
    public partial class InfoTipView : UserControl
    {
        public static readonly DependencyProperty InformationTemplateProperty =
            DependencyProperty.Register(
                "InformationTemplate",
                typeof(DataTemplate),
                typeof(InfoTipView),
                new PropertyMetadata(null));

        public InfoTipView()
        {
            InitializeComponent();            
        }

        public DataTemplate InformationTemplate
        {
            get { return (DataTemplate)GetValue(InformationTemplateProperty); }
            set { SetValue(InformationTemplateProperty, value); }
        }        
    }
}
