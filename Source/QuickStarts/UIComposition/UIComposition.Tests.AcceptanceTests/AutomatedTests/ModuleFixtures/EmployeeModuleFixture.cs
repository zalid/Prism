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
using UIComposition.AcceptanceTests.Helpers;
using UIComposition.AcceptanceTests.ApplicationObserver;
using System.Threading;
using Core.UIItems;
using Core.UIItems.TabItems;
using UIComposition.AcceptanceTests.TestInfrastructure;
using Core.UIItems.Finders;

namespace UIComposition.AcceptanceTests.AutomatedTests
{
    [TestClass]
    [DeploymentItem(@".\UIComposition\bin\Debug")]
    [DeploymentItem(@".\UIComposition.AcceptanceTests\bin\Debug")] 
    public class EmployeeModuleFixture : FixtureBase
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            // Check whether any exception occured during previous application launches. 
            // If so, fail the test case.
            if (StateDiagnosis.IsFailed)
            {
                Assert.Fail(ConfigHandler.GetTestInputData("ApplicationLoadFailure"));
            }

            base.TestInitialize();
        }

        /// <summary>
        /// TestCleanup performs clean-up activities after each test method execution
        /// </summary>
        [TestCleanup()]
        public void MyTestCleanup()
        {
            base.TestCleanup();
        }

        /// <summary>
        /// Validate if the details view of the selected employee are displayed correctly.
        /// 
        /// Repro Steps:
        /// 1. Launch the QS application
        /// 2. Click on the first employee row in the Employee List table
        /// 3. Check if the Details View Tab is displayed, and the number of tab items is 3.
        /// 4. Check if the tab items headers match with "General", "Location" and "Current Projects"
        /// 
        /// Expected Result:
        /// Details View Tab is dispalyed with 3 tab items. 
        /// The tab items headers match with "General", "Location" and "Current Projects"
        /// </summary>
        [TestMethod]
        [Ignore]
        [WorkItem(0)]
        public void ValidateEmployeeSelection()
        {
            //select first row (employee)
            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("EmployeesList"));
            list.SelectEmployee(0);

            //validate details view
            Tab empDetailsTab = window.Get<Tab>(ConfigHandler.GetControlId("DetailsTabControl"));
            Assert.IsNotNull(empDetailsTab, ConfigHandler.GetTestInputData("EmpDetailsTabNotFound"));

            //validate tab has three tab items, and their names are "General", "Location" and "Current Projects"
            Assert.AreEqual(3, empDetailsTab.Pages.Count, ConfigHandler.GetTestInputData("EmpDetailsTabPagesCount"));
            Assert.IsTrue(
                (empDetailsTab.Pages[0].NameMatches(ConfigHandler.GetTestInputData("EmpDetailsTabGeneral")) &&
                empDetailsTab.Pages[1].NameMatches(ConfigHandler.GetTestInputData("EmpDetailsTabLocation")) &&
                empDetailsTab.Pages[2].NameMatches(ConfigHandler.GetTestInputData("EmpDetailsTabCurrentProjects"))), 
                ConfigHandler.GetTestInputData("EmpDetailsTabPagesIncorrect"));

            //validate controls in each of the tabs
            ValidateGeneralTabControls();
            ValidateLocationTabControls();
            ValidateCurrentProjectsTabControls();
        }

        /// <summary>
        /// Validate General details in the General Tab for selected employee
        /// 
        /// Repro Steps:
        /// 1. Launch the QS Application
        /// 2. Select the first employee row in the Employee List table
        /// 3. Check if the details of the selected employee are displayed in the General tab
        /// 
        /// Expected results:
        /// Employee First Name, Last Name, Phone and Email are correctly displayed in the General Tab
        /// </summary>
        [TestMethod]
        [Ignore]
        public void ValidateEmployeeDetailsGeneralSection()
        {
            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("EmployeesList"));
            list.SelectEmployee(ConfigHandler.GetTestInputData("Emp_1_FirstName"));

            Employee emp = GetEmployeeId1();

            ValidateEmployeeDetailsGeneralTabData(emp);
        }

        /// <summary>
        /// Validate Location details in the Location Tab for selected employee
        /// 
        /// Repro Steps:
        /// 1. Launch the QS Application
        /// 2. Select the first employee row in the Employee List table
        /// 3. Check if the location details of the selected employee are displayed in the Location tab
        /// 
        /// Expected results:
        /// Loaction Tab has a frame with required data
        /// </summary>
        [TestMethod]
        [Ignore]
        public void ValidateEmployeeDetailsLocationSection()
        {
            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("EmployeesList"));
            list.SelectEmployee(ConfigHandler.GetTestInputData("Emp_1_FirstName"));

            Employee emp = GetEmployeeId1();

            ValidateEmployeeDetailsLocationTabData(emp);
        }

        /// <summary>
        /// Validate Current Projects details in the Current Projects Tab for selected employee
        /// 
        /// Repro Steps:
        /// 1. Launch the QS Application
        /// 2. Select the first employee row in the Employee List table
        /// 3. Check if the Current Projects of the selected employee are displayed in the Current Projects tab
        /// 
        /// Expected results:
        /// Current Project and Role of the selected Employee are correctly displayed in the Current Projects Tab
        /// </summary>
        [TestMethod]
        [Ignore]
        public void ValidateEmployeeDetailsCurrentProjectsSection()
        {
            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("EmployeesList"));
            list.SelectEmployee(ConfigHandler.GetTestInputData("Emp_1_FirstName"));

            Employee emp = GetEmployeeId1();

            ValidateEmployeeDetailsCurrentProjectsTabData(emp);
        }

        #region Private Helper methods

        private void ValidateGeneralTabControls()
        {
            Tab empDetailsTab = window.Get<Tab>(ConfigHandler.GetControlId("DetailsTabControl"));
            empDetailsTab.Pages[0].Select();

            //check all Labels (TextBlocks)
            SearchCriteria searchCriteria = SearchCriteria.ByText(ConfigHandler.GetTestInputData("FirstNameLabelText")).AndControlType(typeof(Label));
            Label firstNameLabel = window.Get<Label>(searchCriteria);
            Assert.IsNotNull(firstNameLabel);

            searchCriteria = SearchCriteria.ByText(ConfigHandler.GetTestInputData("LastNameLabelText")).AndControlType(typeof(Label));
            Label lastNameLabel = window.Get<Label>(searchCriteria);
            Assert.IsNotNull(lastNameLabel);

            searchCriteria = SearchCriteria.ByText(ConfigHandler.GetTestInputData("PhoneLabelText")).AndControlType(typeof(Label));
            Label phoneLabel = window.Get<Label>(searchCriteria);
            Assert.IsNotNull(phoneLabel);

            searchCriteria = SearchCriteria.ByText(ConfigHandler.GetTestInputData("EmailLabelText")).AndControlType(typeof(Label));
            Label emailLabel = window.Get<Label>(searchCriteria);
            Assert.IsNotNull(emailLabel);

            //check all Textboxes
            TextBox firstName = window.Get<TextBox>(ConfigHandler.GetControlId("FirstNameTextBox"));
            Assert.IsNotNull(firstName);

            TextBox lastName = window.Get<TextBox>(ConfigHandler.GetControlId("LastNameTextBox"));
            Assert.IsNotNull(lastName);

            TextBox phone = window.Get<TextBox>(ConfigHandler.GetControlId("PhoneTextBox"));
            Assert.IsNotNull(phone);

            TextBox email = window.Get<TextBox>(ConfigHandler.GetControlId("EmailTextBox"));
            Assert.IsNotNull(email);
        }

        private void ValidateLocationTabControls()
        {
            //select the Location tab
            Tab empDetailsTab = window.Get<Tab>(ConfigHandler.GetControlId("DetailsTabControl"));
            empDetailsTab.Pages[1].Select();

            //TODO: validate display of frame
        }

        private void ValidateCurrentProjectsTabControls()
        {
            //select the "Current Projects" tab
            Tab empDetailsTab = window.Get<Tab>(ConfigHandler.GetControlId("DetailsTabControl"));
            empDetailsTab.Pages[2].Select();

            SearchCriteria searchCriteria = SearchCriteria.ByText(ConfigHandler.GetTestInputData("PartOfFollowingProjectsLabel")).AndControlType(typeof(Label));
            Label projectsLabel = window.Get<Label>(searchCriteria);
            Assert.IsNotNull(projectsLabel);

            ListView projectsList = window.Get<ListView>(ConfigHandler.GetControlId("CurrentProjectsList"));
            Assert.IsNotNull(projectsList);
        }

        private void ValidateEmployeeDetailsGeneralTabData(Employee emp)
        {
            Tab empDetailsTab = window.Get<Tab>(ConfigHandler.GetControlId("DetailsTabControl"));
            empDetailsTab.Pages[0].Select();

            TextBox firstName = window.Get<TextBox>(ConfigHandler.GetControlId("FirstNameTextBox"));
            Assert.AreEqual(firstName.Text, emp.FirstName);

            TextBox lastName = window.Get<TextBox>(ConfigHandler.GetControlId("LastNameTextBox"));
            Assert.AreEqual(lastName.Text, emp.LastName);

            TextBox phone = window.Get<TextBox>(ConfigHandler.GetControlId("PhoneTextBox"));
            Assert.AreEqual(phone.Text, emp.Phone);

            TextBox email = window.Get<TextBox>(ConfigHandler.GetControlId("EmailTextBox"));
            Assert.AreEqual(email.Text, emp.Email);
        }

        private void ValidateEmployeeDetailsLocationTabData(Employee emp)
        {
            Tab empDetailsTab = window.Get<Tab>(ConfigHandler.GetControlId("DetailsTabControl"));
            empDetailsTab.Pages[1].Select();

            //TODO: get handle of the frame in location taband validate
        }

        private void ValidateEmployeeDetailsCurrentProjectsTabData(Employee emp)
        {
            Tab empDetailsTab = window.Get<Tab>(ConfigHandler.GetControlId("DetailsTabControl"));
            empDetailsTab.Pages[2].Select();

            ListView projectsList = window.Get<ListView>(ConfigHandler.GetControlId("CurrentProjectsList"));

            //check if the list has two columns
            Assert.AreEqual(2, projectsList.Header.Columns.Count);

            //check if the list has two rows
            Assert.AreEqual(2, projectsList.Rows.Count);
        }

        private Employee GetEmployeeId1()
        {
            Employee emp = new Employee(1)
            {
                FirstName = ConfigHandler.GetTestInputData("Emp_1_FirstName"),
                LastName = ConfigHandler.GetTestInputData("Emp_1_LastName"),
                Phone = ConfigHandler.GetTestInputData("Emp_1_Phone"),
                Email = ConfigHandler.GetTestInputData("Emp_1_Email")
            };

            return emp;
        }

        #endregion
    }
}
