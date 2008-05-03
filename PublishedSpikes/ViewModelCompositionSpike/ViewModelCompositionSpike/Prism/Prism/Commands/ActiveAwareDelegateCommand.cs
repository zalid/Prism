using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prism.Interfaces;

namespace Prism.Commands
{
    public class ActiveAwareDelegateCommand<T> : DelegateCommand<T>, IActiveAware
    {
        bool _isActive;

        public ActiveAwareDelegateCommand(Action<T> executeMethod)
            : base(executeMethod)
        {

        }

        public ActiveAwareDelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : base(executeMethod, canExecuteMethod)
        {

        }

        #region IActiveAware Members

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    IsActiveChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler IsActiveChanged = delegate { };

        #endregion
    }
}
