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

namespace Prism.Tests.Commands
{
    [TestClass]
    public class ActiveAwareDelegateCommandFixture
    {
        [TestMethod]
        public void IsActivePropertyChangeFiresEvent()
        {
            bool fired = false;
            var command = new ActiveAwareDelegateCommand<object>( DoNothing );
            command.IsActiveChanged += delegate { fired = true; };
            command.IsActive = true;

            Assert.IsTrue(fired);
        }

        public void DoNothing(object param)
        { }
	
    }
}
