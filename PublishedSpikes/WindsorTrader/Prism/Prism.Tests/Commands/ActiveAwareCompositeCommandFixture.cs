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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Commands;
using System.Windows.Input;
using Prism.Interfaces;

namespace Prism.Tests.Commands
{
    [TestClass]
    public class ActiveAwareCompositeCommandFixture
    {
        [TestMethod]
        public void ActiveAwareMultiDispatchCommandExecutesActiveRegisteredCommands()
        {
            ActiveAwareCompositeCommand activeAwareCommand = new ActiveAwareCompositeCommand();
            MockActiveAwareCommand command = new MockActiveAwareCommand();
            command.IsActive = true;
            activeAwareCommand.RegisterCommand(command);

            activeAwareCommand.Execute(null);

            Assert.IsTrue(command.WasExecuted);
        }

        [TestMethod]
        public void ActiveAwareMultiDispatchCommandDoesNotExecutesInActiveRegisteredCommands()
        {
            ActiveAwareCompositeCommand activeAwareCommand = new ActiveAwareCompositeCommand();
            MockActiveAwareCommand command = new MockActiveAwareCommand();
            command.IsActive = false;
            activeAwareCommand.RegisterCommand(command);

            activeAwareCommand.Execute(null);

            Assert.IsFalse(command.WasExecuted);


        }

        [TestMethod]
        public void ActiveAwareDispatchCommandDoesNotIncludeInActiveRegisteredCommandInVoting()
        {
            ActiveAwareCompositeCommand activeAwareCommand = new ActiveAwareCompositeCommand();
            MockActiveAwareCommand command = new MockActiveAwareCommand();
            activeAwareCommand.RegisterCommand(command);
            command.IsValid = true;
            command.IsActive = false;

            Assert.IsFalse(activeAwareCommand.CanExecute(null), "Registered Command is inactive so should not participate in CanExecute vote");

            command.IsActive = true;

            Assert.IsTrue(activeAwareCommand.CanExecute(null));

            command.IsValid = false;

            Assert.IsFalse(activeAwareCommand.CanExecute(null));

        }

        [TestMethod]
        public void ActiveAwareDispatchCommandShouldIgnoreInActiveCommandsInCanExecuteVote()
        {
            ActiveAwareCompositeCommand activeAwareCommand = new ActiveAwareCompositeCommand();
            MockActiveAwareCommand commandOne = new MockActiveAwareCommand() {IsActive = false, IsValid = false};
            MockActiveAwareCommand commandTwo = new MockActiveAwareCommand() { IsActive = true, IsValid = true};

            activeAwareCommand.RegisterCommand(commandOne);
            activeAwareCommand.RegisterCommand(commandTwo);

            Assert.IsTrue(activeAwareCommand.CanExecute(null));
        }

        [TestMethod]
        public void ChangeInActivityCausesActiveAwareCommandToRequeryCanExecute()
        {
            ActiveAwareCompositeCommand activeAwareCommand = new ActiveAwareCompositeCommand();
            MockActiveAwareCommand command = new MockActiveAwareCommand();
            activeAwareCommand.RegisterCommand(command);
            command.IsActive = true;

            bool globalCanExecuteChangeFired = false;
            activeAwareCommand.CanExecuteChanged += delegate
                                                        {
                                                            globalCanExecuteChangeFired = true;
                                                        };

            
            Assert.IsFalse(globalCanExecuteChangeFired);
            command.IsActive = false;
            Assert.IsTrue(globalCanExecuteChangeFired);
        }

    }

    internal class MockActiveAwareCommand : IActiveAware, ICommand
    {
        private bool _isActive = false;

        #region IActiveAware Members

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnActiveChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler IsActiveChanged = delegate { };
        #endregion

        virtual protected void OnActiveChanged(object sender, EventArgs e)
        {
            IsActiveChanged(sender, e);
        }
        
        public bool WasExecuted { get; set; }
        public bool IsValid { get; set; }
      

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return IsValid;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public void Execute(object parameter)
        {
            WasExecuted = true;
        }

        #endregion
    }
}
