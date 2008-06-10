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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FamilyShow.Infrastructure;
using Microsoft.FamilyShowLib;
using Prism.Interfaces;

namespace FamilyShow.MapModule
{
    /// <summary>
    /// Interaction logic for MockPersonContentView.xaml
    /// </summary>
    public partial class MockPersonContentView : UserControl, IPersonContentView
    {
        public MockPersonContentView()
        {
            InitializeComponent();
        }

        public MockPersonContentView(IEventAggregator eventAggregator) : this()
        {
            EventAggregator = eventAggregator;
            EventAggregator.Get<PersonContextChangedEvent>().Subscribe(this.PersonChanged, ThreadOption.UIThread);
        }

        private void PersonChanged(Person person)
        {
            PersonInfoText.Text = "This is person " + person.FullName + "view";
        }

        public object Clone()
        {
            MockPersonContentView newView = new MockPersonContentView(EventAggregator);
            return newView;
        }

        private IEventAggregator EventAggregator { get; set; }
        private Person Person { get; set; }
    }
}