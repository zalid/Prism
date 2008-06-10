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
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using FamilyShow.Infrastructure;
using Microsoft.FamilyShowLib;
using Microsoft.Practices.Unity;
using Prism.Interfaces;
using System.Windows;

namespace Microsoft.FamilyShow
{
    public class PersonContentController
    {
        private IUnityContainer _container;
        private IEventAggregator _aggregator;

        public PersonContentController(IUnityContainer container, IEventAggregator aggregator)
        {
            _container = container;
            _aggregator = aggregator;
        }

        public void Attach(ItemsControl control)
        {
            IEnumerable<IPersonContentView> views = _container.ResolveAll<IPersonContentView>();
            Window popup = new Window();
            popup.Closing += new System.ComponentModel.CancelEventHandler(popup_Closing);
            foreach(IPersonContentView view in views)
            {
                Viewbox viewbox = new Viewbox() {Height=100, Width=100, Stretch=Stretch.UniformToFill, Margin=new Thickness(5,0,0,0)};
                viewbox.Child = new ContentControl() {Content = view};
                Button enlargeButton = new Button(){Content="Enlarge"};
                var transformGroup = new TransformGroup();
                transformGroup.Children.Add(new RotateTransform(90));
                enlargeButton.LayoutTransform = transformGroup;
                enlargeButton.Tag = view.Clone();
                
                enlargeButton.Click += delegate
                                                 {
                                                     popup.Content = null;
                                                     popup.Content = enlargeButton.Tag;
                                                     popup.ShowDialog();
                                                 };
                control.Items.Add(viewbox);
                control.Items.Add(enlargeButton);
            }
            //control.ItemsSource = views;
        }

        void popup_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Window popup = sender as Window;

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (DispatcherOperationCallback)delegate(object o)
            {
                popup.Hide();
                return null;
            }, null);
        }

        public void SetPersonContext(Person person)
        {
            _aggregator.Get<PersonContextChangedEvent>().Fire(person);
        }
    }
}