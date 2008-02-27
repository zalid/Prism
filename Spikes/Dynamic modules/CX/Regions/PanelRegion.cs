using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CX.Interfaces;
using System.Windows.Controls;
using System.Windows;

namespace CX.Regions
{
    public class PanelRegion : IRegion
    {
        #region IRegion Members

        private Panel panel;
        private IPresenter parent;

        public PanelRegion(Panel panel, IPresenter parent)
        {
            this.panel = panel;
            this.parent = parent;
        }
        public void AddElement<TView>(TView view, string name)
        {
            if (view is IStubView)
                return;

            UIElement element = view as UIElement;
            if (element != null)
            {
                this.panel.Children.Add(element);
                IView castedView = view as IView;
                if (castedView != null)
                {
                    parent.ChildPresenters.Add(castedView.Presenter);
                }
            }
        }

        #endregion
    }
}
