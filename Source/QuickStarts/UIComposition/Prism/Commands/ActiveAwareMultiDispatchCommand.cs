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
using Prism.Interfaces;

namespace Prism.Commands
{
    public class ActiveAwareMultiDispatchCommand : MultiDispatchCommand
    {
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

        public override void RegisterCommand(System.Windows.Input.ICommand command)
        {
            base.RegisterCommand(command);
            var activeAwareCommand = command as IActiveAware;
            if (activeAwareCommand != null)
            {
                activeAwareCommand.IsActiveChanged += activeAwareCommand_IsActiveChanged;
            }
        }

        public override void UnregisterCommand(System.Windows.Input.ICommand command)
        {
            base.UnregisterCommand(command);
            var activeAwareCommand = command as IActiveAware;
            if (activeAwareCommand != null)
            {
                activeAwareCommand.IsActiveChanged -= activeAwareCommand_IsActiveChanged;
            }
        }

        void activeAwareCommand_IsActiveChanged(object sender, EventArgs e)
        {
            this.OnCanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
