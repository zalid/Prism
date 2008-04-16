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
using UIComposition.Modules.Employee.Controllers;
using Microsoft.Practices.Unity;
using UIComposition.Modules.Employee.Tests.Mocks;
using UIComposition.Modules.Project;

namespace UIComposition.Modules.Employee.Tests.Controllers
{
    [TestClass]
    public class EmployeesControllerFixture
    {
        IUnityContainer container;

        [TestInitialize]
        public void SetUp()
        {
            container = new MockUnityContainer();
        }

        [TestMethod]
        public void CanInitController()
        {
            IEmployeesController controller = CreateController();

            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void ControllerAddsViewWhenShowDetailsIfNotPresent()
        {
            container.RegisterType<IEmployeesDetailsPresenter, MockEmployeesDetailsPresenter>();
            container.RegisterType<IProjectsListPresenter, MockProjectsListPresenter>();

            MockEmployeesView employeesView = new MockEmployeesView();
            BusinessEntities.Employee employee1 = new BusinessEntities.Employee(10) { LastName = "Mock1", FirstName = "Employee1" };
            BusinessEntities.Employee employee2 = new BusinessEntities.Employee(11) { LastName = "Mock2", FirstName = "Employee2" };

            EmployeesController controller = new EmployeesController(container);

            Assert.AreEqual<int>(0, employeesView.DetailsRegion.Views.Count);

            controller.OnEmployeeSelected(employeesView, employee1);
            controller.OnEmployeeSelected(employeesView, employee2);

            Assert.AreEqual<int>(2, employeesView.DetailsRegion.Views.Count);
        }

        [TestMethod]
        public void ControllerNotAddsViewWhenShowDetailsIfIsAlreadyAddedPresent()
        {
            container.RegisterType<IEmployeesDetailsPresenter, MockEmployeesDetailsPresenter>();
            container.RegisterType<IProjectsListPresenter, MockProjectsListPresenter>();

            MockEmployeesView employeesView = new MockEmployeesView();
            BusinessEntities.Employee employee = new UIComposition.Modules.Employee.BusinessEntities.Employee(10) { LastName = "Con", FirstName = "Aaron" };

            EmployeesController controller = new EmployeesController(container);

            Assert.AreEqual<int>(0, employeesView.DetailsRegion.Views.Count);

            controller.OnEmployeeSelected(employeesView, employee);
            controller.OnEmployeeSelected(employeesView, employee);

            Assert.AreEqual<int>(1, employeesView.DetailsRegion.Views.Count);
            Assert.IsTrue(employeesView.DetailsRegion.ShowCalled);
        }

        private IEmployeesController CreateController()
        {
            return new EmployeesController(container);
        }
    }
}