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
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Microsoft.Practices.Composite.Wpf.Commands
{
    /// <summary>
    /// The CompositeCommand composites one or more ICommands.
    /// </summary>
    public class CompositeCommand : ICommand
    {
        private List<ICommand> registeredCommands = new List<ICommand>();
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
        public virtual void RegisterCommand(ICommand command)
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
        public virtual void UnregisterCommand(ICommand command)
        {
            registeredCommands.Remove(command);
            command.CanExecuteChanged -= RegisteredCommand_CanExecuteChanged;
            OnCanExecuteChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Re-raises OnCanExecuteChanged.
        /// </summary>
        private void RegisteredCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            OnCanExecuteChanged(sender, e);
        }

        /// <summary>
        /// Handles firing of the CanExecuteChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers")]
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

        /// <summary>
        /// Forwards CanExecute to the registered commands and returns true if all the commands can be executed.
        /// </summary>
        ///<param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if all the registered commands can be executed; otherwise, false.</returns>
        public virtual bool CanExecute(object parameter)
        {
            bool hasEnabledCommandsThatShouldBeExecuted = false;

            foreach (ICommand command in registeredCommands)
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

        ///<summary>
        ///Occurs when any of the registered commands raise <seealso cref="CanExecuteChanged"/>.
        ///</summary>
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

        public IList<ICommand> RegisteredCommands
        {
            get { return registeredCommands.AsReadOnly(); }
        }
    }
}