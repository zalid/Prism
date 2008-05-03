using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.Utility
{
    public class DataEventArgs<T> : EventArgs
    {
        private T _value;

        public DataEventArgs(T value)
        {
            _value = value;
        }

        public T Value
        {
            get { return _value; }
        }
    }
}
