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
using FamilyShow.Infrastructure;
using FamilyShow.MapModule;
using Microsoft.FamilyShowLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FamilyShow.HistoricalFactModule.Tests
{
    /// <summary>
    /// Summary description for BirthdayCountdownServiceFixture
    /// </summary>
    [TestClass]
    public class BirthdayCountdownServiceFixture
    {
 
        [TestMethod]
        public void TestReturningDaysTillBirthday()
        {
            Person person = new Person(){BirthDate = DateTime.Now.Add(new TimeSpan(10,0,0,0))};
            BirthdayCountdownService service = new BirthdayCountdownService();
            Assert.AreEqual("10 day(s) till birthday",service.GetContent(person).ToString());

           
        }

        [TestMethod]
        public void PersonIsNullOrHasNullBirthDateReturnsNull()
        {
            BirthdayCountdownService service = new BirthdayCountdownService();

            Assert.IsNull(service.GetContent(null));

            Assert.IsNull(service.GetContent(new Person()));


        }

        [TestMethod]
        public void TestReturningDaysTillBirthdayThatIsMoreThanAYearAgo()
        {
            Person person = new Person() { BirthDate = DateTime.Now.Subtract(new TimeSpan(361, 0, 0, 0)) };
            BirthdayCountdownService service = new BirthdayCountdownService();
            Assert.AreEqual("5 day(s) till birthday", service.GetContent(person).ToString());
        }

        [TestMethod]
        public void ReturnsCorrectMessageIfBDayAlreadyOccuredThisYear()
        {
            Person person = new Person() { BirthDate = DateTime.Now.Subtract(new TimeSpan(10, 0, 0, 0)) };
            BirthdayCountdownService service = new BirthdayCountdownService();
            Assert.AreEqual("355 day(s) till birthday", service.GetContent(person).ToString());


        }
    }

}
