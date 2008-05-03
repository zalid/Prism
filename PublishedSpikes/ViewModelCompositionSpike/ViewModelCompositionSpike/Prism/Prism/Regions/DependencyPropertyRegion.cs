using System;
using System.Collections.Generic;
using System.Windows;
using Prism.Interfaces;

namespace Prism.Regions
{
    public class DependencyPropertyRegion : IRegion
    {
        private readonly DependencyObject dependentObject;
        private readonly DependencyProperty property;

        public DependencyPropertyRegion(DependencyObject obj, DependencyProperty property)
        {
            dependentObject = obj;
            this.property = property;
        }
        #region IRegion Members

        public void Add(object view)
        {
            dependentObject.SetValue(property, view);
        }

        public void Add(object view, string name)
        {
            throw new NotImplementedException();
        }

        public object GetView(string name)
        {
            throw new NotImplementedException();
        }

        public void Remove(object view)
        {
            throw new NotImplementedException();
        }

        public void Show(object view)
        {
            throw new NotImplementedException();
        }

        public IList<object> Views
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}