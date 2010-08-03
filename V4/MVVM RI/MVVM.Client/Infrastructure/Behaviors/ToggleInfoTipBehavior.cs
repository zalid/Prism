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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using MVVM.Client.Infrastructure.Views;

namespace MVVM.Client.Infrastructure.Behaviors
{
    public class ToggleInfoTipBehavior : Behavior<ToggleButton>
    {
        public static readonly DependencyProperty ContentTemplateProperty =
           DependencyProperty.Register(
               "ContentTemplate",
               typeof(DataTemplate),
               typeof(ToggleInfoTipBehavior),
               new PropertyMetadata(null));

        private static WeakReference weakOpenToggleButton;

        private ToggleButton toggleButton;
        private Popup popup;
        private WeakReference weakPopup;


        /// <summary>
        /// Gets or sets the content template for a popup window.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            this.toggleButton = (ToggleButton)this.AssociatedObject;

            this.toggleButton.Checked += this.ToggleButton_IsCheckedChanged;
            this.toggleButton.Unchecked += this.ToggleButton_IsCheckedChanged;
            this.toggleButton.Unloaded += this.ToggleButton_Unloaded;

            ((Control)Application.Current.RootVisual).SizeChanged += this.RootVisual_SizeChanged;
        }

        protected override void OnDetaching()
        {
            this.ClosePopup();

            this.toggleButton.Checked -= this.ToggleButton_IsCheckedChanged;
            this.toggleButton.Unchecked -= this.ToggleButton_IsCheckedChanged;
            this.toggleButton.Unloaded += this.ToggleButton_Unloaded;

            ((Control)Application.Current.RootVisual).SizeChanged -= this.RootVisual_SizeChanged;

            this.toggleButton = null;

            base.OnDetaching();
        }

        private void ToggleButton_Unloaded(object sender, RoutedEventArgs e)
        {
            this.ClosePopup();
        }

        private void ToggleButton_IsCheckedChanged(object sender, RoutedEventArgs e)
        {
            if (this.toggleButton.IsChecked == true)
            {
                this.OpenPopup();
            }
            else
            {
                this.ClosePopup();
            }
        }


        private void RootVisual_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.PositionPopup();
        }

        private void PopupContent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.PositionPopup();
        }

        private void CreatePopup()
        {
            InfoTipView content = new InfoTipView();
            content.InformationTemplate = this.ContentTemplate;
            content.DataContext = this.toggleButton.DataContext;

            popup = new Popup();
            popup.Child = content;
        }

        private void OpenPopup()
        {
            if ((weakOpenToggleButton != null) &&
                (weakOpenToggleButton.IsAlive) &&
                (weakOpenToggleButton.Target != this.toggleButton))
            {
                ((ToggleButton)weakOpenToggleButton.Target).IsChecked = false;
            }

            if (this.popup == null)
            {
                // I try and use the weak reference so toggling is quick.
                if ((this.weakPopup != null) && (this.weakPopup.IsAlive))
                {
                    this.popup = (Popup)this.weakPopup.Target;
                }
                else
                {
                    this.CreatePopup();
                }

                ((Control)this.popup.Child).SizeChanged += this.PopupContent_SizeChanged;
            }

            this.PositionPopup();

            this.popup.IsOpen = true;
            weakOpenToggleButton = new WeakReference(this.toggleButton);
        }

        private void ClosePopup()
        {
            if (this.popup != null)
            {
                this.popup.IsOpen = false;

                if ((weakOpenToggleButton != null) &&
                (weakOpenToggleButton.IsAlive) &&
                (weakOpenToggleButton.Target == this.toggleButton))
                {
                    weakOpenToggleButton = null;
                }

                // I keep a weak reference to the popup so that toggling is quick.
                this.weakPopup = new WeakReference(this.popup);
                ((Control)this.popup.Child).SizeChanged -= this.PopupContent_SizeChanged;
                this.popup = null;
            }
        }

        private void PositionPopup()
        {
            if (this.popup != null)
            {
                Control rootControl = (Control)Application.Current.RootVisual;
                Control childControl = (Control)this.popup.Child;

                GeneralTransform transform = this.toggleButton.TransformToVisual(rootControl);

                Point startPoint;
                Point endPoint;

                // I try lower right
                startPoint = transform.Transform(new Point(this.toggleButton.ActualWidth + 2, this.toggleButton.ActualHeight + 2));
                endPoint = new Point(startPoint.X + childControl.ActualWidth, startPoint.Y + childControl.ActualHeight);

                if (IsPointWithinActualArea(rootControl, startPoint) && IsPointWithinActualArea(rootControl, endPoint))
                {
                    this.popup.HorizontalOffset = startPoint.X;
                    this.popup.VerticalOffset = startPoint.Y;
                    return;
                }

                // I try lower left
                startPoint = transform.Transform(new Point(-2, this.toggleButton.ActualHeight + 2));
                endPoint = new Point(startPoint.X - childControl.ActualWidth, startPoint.Y + childControl.ActualHeight);

                if (IsPointWithinActualArea(rootControl, startPoint) && IsPointWithinActualArea(rootControl, endPoint))
                {
                    this.popup.HorizontalOffset = endPoint.X;
                    this.popup.VerticalOffset = startPoint.Y;
                    return;
                }

                // I try upper right
                startPoint = transform.Transform(new Point(this.toggleButton.ActualWidth + 2, -2));
                endPoint = new Point(startPoint.X + childControl.ActualWidth, startPoint.Y - childControl.ActualHeight);

                if (IsPointWithinActualArea(rootControl, startPoint) && IsPointWithinActualArea(rootControl, endPoint))
                {
                    this.popup.HorizontalOffset = startPoint.X;
                    this.popup.VerticalOffset = endPoint.Y;
                    return;
                }

                // I try upper left
                startPoint = transform.Transform(new Point(-2, -2));
                endPoint = new Point(startPoint.X - childControl.ActualWidth, startPoint.Y - childControl.ActualHeight);

                if (IsPointWithinActualArea(rootControl, startPoint) && IsPointWithinActualArea(rootControl, endPoint))
                {
                    this.popup.HorizontalOffset = endPoint.X;
                    this.popup.VerticalOffset = endPoint.Y;
                    return;
                }

                // I give up and do lower right
                startPoint = transform.Transform(new Point(this.toggleButton.ActualWidth + 2, this.toggleButton.ActualHeight + 2));
                this.popup.HorizontalOffset = startPoint.X;
                this.popup.VerticalOffset = startPoint.Y;
            }
        }

        private static bool IsPointWithinActualArea(Control control, Point point)
        {
            return ((point.X >= 0) &&
                    (point.X <= control.ActualWidth) &&
                    (point.Y >= 0) &&
                    (point.Y <= control.ActualHeight));
        }
    }
}

