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
using Microsoft.Practices.Unity;
using UIComposition.Modules.Project.Tests.Mocks;
using UIComposition.Infrastructure;
using UIComposition.Modules.Project.Services;
using Prism.Interfaces;

namespace UIComposition.Modules.Project.Tests
{
    [TestClass]
    public class ProjectModuleFixture
    {

        MockUnityContainer container;
        MockRegionManagerService regionManagerService;

        [TestInitialize]
        public void SetUp()
        {
            container = new MockUnityContainer();
            regionManagerService = new MockRegionManagerService();
        }
        [TestMethod]
        public void RegisterViewsAndServices()
        {
            TestableProjectModule module = CreateTestableProjectModule();

            module.InvokeRegisterViewsAndServices();

            Assert.AreEqual(typeof(ProjectService), container.Types[typeof(IProjectService)]);
            Assert.AreEqual(typeof(ProjectsListView), container.Types[typeof(IProjectsListView)]); 
            Assert.AreEqual(typeof(ProjectsListPresenter), container.Types[typeof(IProjectsListPresenter)]);
        }

        [TestMethod]
        public void InitializeShouldCallRegisterAndViewServices()
        {
            container.RegisterInstance<IUnityContainer>(container);
            container.RegisterInstance<IRegionManagerService>(regionManagerService);
            MockRegion mainToolbar = new MockRegion();

            regionManagerService.Register(RegionNames.MainToolBar, mainToolbar);

            ProjectModule module = CreateProjectModule();

            module.Initialize();

            Assert.AreEqual(typeof(ProjectService), container.Types[typeof(IProjectService)]);
            Assert.AreEqual(typeof(ProjectsListView), container.Types[typeof(IProjectsListView)]);
            Assert.AreEqual(typeof(ProjectsListPresenter), container.Types[typeof(IProjectsListPresenter)]);
        }

        private ProjectModule CreateProjectModule()
        {
            ProjectModule module = new ProjectModule(container, regionManagerService);
            return module;
        }

        private TestableProjectModule CreateTestableProjectModule()
        {
            TestableProjectModule module = new TestableProjectModule(container, regionManagerService);
            return module;
        }

    }

    class TestableProjectModule : ProjectModule
    {
        public TestableProjectModule(IUnityContainer container, IRegionManagerService regionManagerService)
            : base(container, regionManagerService)
        {

        }

        public void InvokeRegisterViewsAndServices()
        {
            base.RegisterViewsAndServices();
        }
    }
}
