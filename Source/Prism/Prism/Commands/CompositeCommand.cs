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
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Prism.Commands
{
    /// <summary>
    /// The CompositeCommand composites one or more ICommands.
    /// </summary>
    public class CompositeCommand : ICommand
    {
        readonly List<ICommand> registeredCommands = new List<ICommand>();
        private readonly Dispatcher dispatcher;

        public CompositeCommand()
        {
            if (Application.Current != null)
            {
                dispatcher = Application.Current.Dispatcher;
            }
        }

        /// <summary>
        /// Adds commands to the collection and signs up for the CanExecuteChanged events.
        /// </summary>
        /// <param name="command"></param>
        virtual public void RegisterCommand(ICommand command)
        {
            if (!registeredCommands.Contains(command))
            {
                registeredCommands.Add(command);
                command.CanExecuteChanged += RegisteredCommand_CanExecuteChanged;
                OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }


        /// <summary>
        /// Removes command from the collection and removes itself from the CanExecuteChanged events.
        /// </summary>
        /// <param name="command"></param>
        virtual public void UnregisterCommand(ICommand command)
        {
            registeredCommands.Remove(command);
            command.CanExecuteChanged -= RegisteredCommand_CanExecuteChanged;
            OnCanExecuteChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Re-raises OnCanExecuteChanged.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RegisteredCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            OnCanExecuteChanged(sender, e);
        }

        /// <summary>
        /// Returns true if there are any registered commands.
        /// </summary>
        protected bool HasCommand
        {
            get { return (registeredCommands.Count != 0); }
        }


        /// <summary>
        /// Handles firing of the CanExecuteChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnCanExecuteChanged(object sender, EventArgs e)
        {
            if (dispatcher != null && !dispatcher.CheckAccess())
            {
                dispatcher.BeginInvoke(DispatcherPriority.Normal,
                                       new EventHandler(OnCanExecuteChanged),
                                       sender,
                                       e);
                return;
            }
            CanExecuteChanged(sender, e);
        }

        public void ClearExecuteChangedDelegates()
        {
            CanExecuteChanged = delegate { };
        }

        #region ICommand Members

        /// <summary>
        /// Forwards CanExecute to registered commands.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            bool hasEnabledCommandsThatShouldBeExecuted = false;

            foreach (var command in registeredCommands)
            {
                if (ShouldExecute(command))
                {
                    if (!command.CanExecute(parameter))
                    {
                        return false;
                    }

                    hasEnabledCommandsThatShouldBeExecuted = true;
                }
            }
            return hasEnabledCommandsThatShouldBeExecuted;
        }

        public event EventHandler CanExecuteChanged = delegate { };


        /// <summary>
        /// Forwards Execute to registered commands.
        /// </summary>
        /// <param name="parameter"></param>
        public virtual void Execute(object parameter)
        {
            Queue<ICommand> commands = new Queue<ICommand>(registeredCommands);

            while (commands.Count > 0)
            {
                ICommand command = commands.Dequeue();
                if (ShouldExecute(command))
                    command.Execute(parameter);
            }
        }


        /// <summary>
        /// Evaluates if a command should execute.  Base ShouldExecute returns true.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected virtual bool ShouldExecute(ICommand command)
        {
            return true;
        }

        #endregion

        /// <summary>
        /// Returns the delegate count for CanExecutChanged event
        /// </summary>
        /// <returns></returns>
        /// <remarks>This is added for testing only and is not intended to be used from outside code.</remarks>
        protected int CanExecuteChangedDelegateCount
        {
            get { return CanExecuteChanged.GetInvocationList().Count(); }
        }
    }
}
