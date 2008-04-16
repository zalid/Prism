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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Commands;

namespace Prism.Tests.Commands
{
    /// <summary>
    /// Summary description for DelegateCommandFixture
    /// </summary>
    [TestClass]
    public class DelegateCommandFixture
    {
        [TestMethod]
        public void ExecuteCallsPassedInExecuteDelegate()
        {
            var handlers = new DelegateHandlers();
            DelegateCommand command = new DelegateCommand(handlers.Execute, null);
            object parameter = new object();

            command.Execute(parameter);

            Assert.AreSame(parameter, handlers.ExecuteParameter);
        }

        [TestMethod]
        public void CanExecuteCallsPassedInCanExecuteDelegate()
        {
            var handlers = new DelegateHandlers();
            DelegateCommand command = new DelegateCommand(handlers.Execute, handlers.CanExecute);
            object parameter = new object();

            handlers.CanExecuteReturnValue = true;
            bool retVal = command.CanExecute(parameter);

            Assert.AreSame(parameter, handlers.CanExecuteParameter);
            Assert.AreEqual(handlers.CanExecuteReturnValue, retVal);
        }

        [TestMethod]
        public void CanExecuteReturnsTrueWithouthCanExecuteDelegate()
        {
            var handlers = new DelegateHandlers();
            DelegateCommand command = new DelegateCommand(handlers.Execute);

            bool retVal = command.CanExecute(null);

            Assert.AreEqual(true, retVal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullExecuteDelegateThrows()
        {
            DelegateCommand command = new DelegateCommand(null);
        }

        [TestMethod]
        public void RaiseCanExecuteChangedRaisesCanExecuteChanged()
        {
            var handlers = new DelegateHandlers();
            DelegateCommand command = new DelegateCommand(handlers.Execute);
            bool canExecuteChangedRaised = false;
            command.CanExecuteChanged += delegate { canExecuteChangedRaised = true; };

            command.RaiseCanExecuteChanged();

            Assert.IsTrue(canExecuteChangedRaised);
        }



        class DelegateHandlers
        {
            public bool CanExecuteReturnValue = true;
            public object CanExecuteParameter;
            public object ExecuteParameter;

            public bool CanExecute(object parameter)
            {
                CanExecuteParameter = parameter;
                return CanExecuteReturnValue;
            }

            public void Execute(object parameter)
            {
                ExecuteParameter = parameter;
            }
        }
    }
}
