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
using System.Windows.Input;

namespace Prism.Commands
{
    /// <summary>
    /// An <see cref="ICommand"/> whose delegates can be attached for <see cref="Execute"/> and <see cref="CanExecute"/>.
    /// </summary>
    /// <typeparam name="T">Parameter type.</typeparam>
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> executeMethod = null;
        private readonly Func<T, bool> canExecuteMethod = null;


        /// <summary>
        /// Constructor.  Initializes delegate command with Execute delegate.
        /// </summary>
        /// <param name="executeMethod"></param>
        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, null)
        {
        }

      
        /// <summary>
        /// Constructor.  Initializes delegate command with Execute delegate and CanExecute delegate
        /// </summary>
        /// <param name="executeMethod">Delegate to execute when Execute is called on the command.  This can be null to just hook up a CanExecute delegate.</param>
        /// <param name="canExecuteMethod">Delegate to execute when CanExecute is called on the command.  This can be null.</param>
        /// <remarks></remarks>
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            if (executeMethod == null && canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod", Properties.Resources.DelegateCommandDelegatesCannotBeNull);

            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(T parameter)
        {
            if (canExecuteMethod == null) return true;
            return canExecuteMethod(parameter);
        }

        public void Execute(T parameter)
        {
            if (executeMethod == null) return;
            executeMethod(parameter);
        }

        #region ICommand
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged;

        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
        }
        #endregion

        protected virtual void OnCanExecuteChanged(object sender, System.EventArgs e)
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(sender, e);
        }

        /// <summary>
        /// Raises <see cref="CanExecuteChanged"/> so every command invoker can requery to check if the command can execute.
        /// <remarks>Note that this will trigger the execution of <see cref="CanExecute"/> once for each invoker.</remarks>
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
