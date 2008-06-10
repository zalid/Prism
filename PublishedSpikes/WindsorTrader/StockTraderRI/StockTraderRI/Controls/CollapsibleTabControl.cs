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

namespace StockTraderRI.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:StockTraderRI.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:StockTraderRI.Controls;assembly=StockTraderRI"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class CollapsibleTabControl : TabControl
    {
        public static ICommand ToggleDockedStateCommand = new RoutedCommand();
        public static DependencyProperty CollapsingStateProperty;

        static CollapsibleTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CollapsibleTabControl), new FrameworkPropertyMetadata(typeof(CollapsibleTabControl)));

            CollapsingStateProperty = DependencyProperty.Register("CollapsingState", 
                typeof(CollapsingState), 
                typeof(CollapsibleTabControl), 
                new PropertyMetadata(CollapsingState.Collapsed));
        }

        public CollapsibleTabControl()
        {
            this.MouseEnter += new MouseEventHandler(CollapsibleTabControl_MouseEnter);
            this.MouseLeave += new MouseEventHandler(CollapsibleTabControl_MouseLeave);
            this.CommandBindings.Add(new CommandBinding(ToggleDockedStateCommand, CollapsibleTabControl_ToggleDockedStateCommandExecutedRoutedEventHandler));
        }

        void CollapsibleTabControl_ToggleDockedStateCommandExecutedRoutedEventHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (CollapsingState == CollapsingState.Docked)
            {
                CollapsingState = CollapsingState.Overlapped;
            }
            else
            {
                CollapsingState = CollapsingState.Docked;
            }
        }

        void CollapsibleTabControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (CollapsingState == CollapsingState.Overlapped)
            {
                CollapsingState = CollapsingState.Collapsed;
            }
        }

        void CollapsibleTabControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (CollapsingState == CollapsingState.Collapsed)
            {
                CollapsingState = CollapsingState.Overlapped;
            }
        }

        public CollapsingState CollapsingState
        {
            get { return (CollapsingState)GetValue(CollapsingStateProperty); }
            set { SetValue(CollapsingStateProperty, value); }
        }
    }


}
