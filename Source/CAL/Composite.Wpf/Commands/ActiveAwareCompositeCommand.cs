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
using Microsoft.Practices.Composite;

namespace Microsoft.Practices.Composite.Wpf.Commands
{
    /// <summary>
    /// CompositeCommand that is aware of ICommand's implementing IActiveAware interface
    /// </summary>
    /// <remarks>
    /// IActiveAware commands participate in enablement voting and execution only if they are active.
    /// <see cref="ICommand"/>
    /// </remarks>
    /// <seealso cref="System.Windows.Input"/>
    public class ActiveAwareCompositeCommand : CompositeCommand
    {

        /// <summary>
        /// Determines if the ICommand should execute.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>true if it should execute</returns>
        /// <remarks>If the command implements the IActiveAware interface, the command must be active to be executed.</remarks>
        protected override bool ShouldExecute(System.Windows.Input.ICommand command)
        {
            var activeAwareCommand = command as IActiveAware;

            if (activeAwareCommand == null)
            {
                return base.ShouldExecute(command);
            }
            else
            {
                return (activeAwareCommand.IsActive && base.ShouldExecute(command));
            }
        }


        /// <summary>
        /// Registers the ICommand and registers with the IActiveAware.IsActiveChanged event, if the command implements IActiveAware
        /// </summary>
        /// <param name="command"></param>
        public override void RegisterCommand(System.Windows.Input.ICommand command)
        {
            base.RegisterCommand(command);
            var activeAwareCommand = command as IActiveAware;
            if (activeAwareCommand != null)
            {
                activeAwareCommand.IsActiveChanged += activeAwareCommand_IsActiveChanged;
            }
        }

        /// <summary>
        /// Registers the ICommand and unregisters with the IActiveAware.IsActiveChanged event, if the command implements IActiveAware
        /// </summary>
        /// <param name="command"></param>
        public override void UnregisterCommand(System.Windows.Input.ICommand command)
        {
            base.UnregisterCommand(command);
            var activeAwareCommand = command as IActiveAware;
            if (activeAwareCommand != null)
            {
                activeAwareCommand.IsActiveChanged -= activeAwareCommand_IsActiveChanged;
            }
        }


        /// <summary>
        /// Handler for IsActiveChanged events of registered commands.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void activeAwareCommand_IsActiveChanged(object sender, EventArgs e)
        {
            this.OnCanExecuteChanged(this, EventArgs.Empty);
        }
    }
}