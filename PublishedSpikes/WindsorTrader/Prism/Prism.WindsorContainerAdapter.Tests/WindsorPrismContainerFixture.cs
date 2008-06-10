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
using Castle.Core;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Interfaces;
using Prism.WindsorContainerAdapter.Tests.Mocks;

namespace Prism.WindsorContainerAdapter.Tests
{
    [TestClass]
    public class WindsorPrismContainerFixture
    {

        [TestMethod]
        public void CanRegisterAndResolveUsingPrismContainer()
        {
            IWindsorContainer container = new WindsorContainer();
            IPrismContainer prismContainer = new WindsorPrismContainer(container);

            container.AddComponentWithLifestyle<IService, MockService>("key", LifestyleType.Transient);
            IService mockService = prismContainer.Resolve<IService>();

            Assert.IsInstanceOfType(mockService, typeof(IService));
        }

        [TestMethod]
        public void CanRegisterAndResolveSingletonUsingPrismContainer()
        {
            IWindsorContainer container = new WindsorContainer();
            IPrismContainer prismContainer = new WindsorPrismContainer(container);


            container.AddComponentWithLifestyle<IService, MockService>(LifestyleType.Singleton);
            IService mockService1 = prismContainer.Resolve<IService>();
            IService mockService2 = prismContainer.Resolve<IService>();

            Assert.AreSame(mockService1, mockService2);
        }

        [TestMethod]
        public void CanResolveCascadingDependencies()
        {
            IWindsorContainer container = new WindsorContainer();
            IPrismContainer prismContainer = new WindsorPrismContainer(container);

            container.AddComponentWithLifestyle<IDependantA, DependantA>(LifestyleType.Transient);
            container.AddComponentWithLifestyle<IDependantB, DependantB>(LifestyleType.Transient);
            container.AddComponentWithLifestyle<IService, MockService>(LifestyleType.Singleton);

            IDependantA dependantA = prismContainer.Resolve<IDependantA>();
            Assert.IsNotNull(dependantA);
            Assert.IsInstanceOfType(dependantA, typeof(IDependantA));
            Assert.IsNotNull(dependantA.MyDependantB);
            Assert.IsInstanceOfType(dependantA.MyDependantB, typeof(IDependantB));
            Assert.IsNotNull(dependantA.MyDependantB.MyService);
            Assert.IsInstanceOfType(dependantA.MyDependantB.MyService, typeof(IService));

        }

        [TestMethod]
        public void TryResolveShouldResolveTheElementIfElementExist()
        {
            IWindsorContainer container = new WindsorContainer();
            IPrismContainer prismContainer = new WindsorPrismContainer(container);

            container.AddComponentWithLifestyle<IService, MockService>(LifestyleType.Singleton);

            object dependantA = prismContainer.TryResolve(typeof(IService));
            Assert.IsNotNull(dependantA);
        }

        [TestMethod]
        public void TryResolveShouldReturnNullIfElementNotExist()
        {
            IWindsorContainer container = new WindsorContainer();
            IPrismContainer prismContainer = new WindsorPrismContainer(container);

            object dependantA = prismContainer.TryResolve(typeof(IDependantA));
            Assert.IsNull(dependantA);
        }

        [TestMethod]
        public void ShouldResolveUnregisteredItems()
        {
            IWindsorContainer container = new WindsorContainer();
            IPrismContainer prismContainer = new WindsorPrismContainer(container);

            object mockService = prismContainer.Resolve(typeof(MockService));
            Assert.IsNotNull(mockService);
        }
    }
}
