using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prism.Interfaces;

namespace Prism.Commands
{

    /// <summary>
    /// CompositeCommand that is aware of ICommand's implementing IActiveAware interface
    /// </summary>
    /// <remarks>
    /// IActiveAware commands participate in enablement voting and execution only if they are active.
    /// <see cref="IActiveAware"/>
    /// </remarks>
    /// <seealso cref="System.Windows.Input.ICommand"/>
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
