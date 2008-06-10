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
using Microsoft.Practices.Unity;
using Prism.UnityContainerAdapter;
using Prism.Interfaces;
using Prism.UnityContainerAdapter.Tests.Mocks;

namespace Prism.UnityContainerAdapter.Tests
{
    [TestClass]
    public class UnityPrismContainerFixture
    {
        [TestMethod]
        public void CanRegisterAndResolveUsingPrismContainer()
        {
            IUnityContainer container = new UnityContainer();
            IPrismContainer prismContainer = new UnityPrismContainer(container);

            container.RegisterType<IService, MockService>();
            IService mockService = prismContainer.Resolve<IService>();

            Assert.IsInstanceOfType(mockService, typeof(IService));
        }

        [TestMethod]
        public void CanRegisterAndResolveSingletonUsingPrismContainer()
        {
            IUnityContainer container = new UnityContainer();
            IPrismContainer prismContainer = new UnityPrismContainer(container);


            container.RegisterType<IService, MockService>(new ContainerControlledLifetimeManager());
            IService mockService1 = prismContainer.Resolve<IService>();
            IService mockService2 = prismContainer.Resolve<IService>();

            Assert.AreSame(mockService1, mockService2);
        }

        [TestMethod]
        public void CanResolveCascadingDependencies()
        {
            IUnityContainer container = new UnityContainer();
            IPrismContainer prismContainer = new UnityPrismContainer(container);

            container.RegisterType<IDependantA, DependantA>();
            container.RegisterType<IDependantB, DependantB>();
            container.RegisterType<IService, MockService>(new ContainerControlledLifetimeManager());

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
            IUnityContainer container = new UnityContainer();
            IPrismContainer prismContainer = new UnityPrismContainer(container);

            container.RegisterType<IService, MockService>(new ContainerControlledLifetimeManager());

            object dependantA = prismContainer.TryResolve(typeof(IService));
            Assert.IsNotNull(dependantA);
        }

        [TestMethod]
        public void TryResolveShouldReturnNullIfElementNotExist()
        {
            IUnityContainer container = new UnityContainer();
            IPrismContainer prismContainer = new UnityPrismContainer(container);

            object dependantA = prismContainer.TryResolve(typeof(IDependantA));
            Assert.IsNull(dependantA);
        }

	
        //[TestMethod]
        //public void CanRetrieveAnInjectedService()
        //{
        //    IUnityContainer container = new UnityContainer();
        //        //.AddNewExtension<PrismExtension>();

        //    container.RegisterInstance<IUnityContainer>(container);
        //    container.Register<IPrismContainer, UnityPrismContainer>();

        //    // Registration that may be normally be done through config, but it's not there yet for Unity...
        //    container.Register<IModuleLoaderService, ModuleLoaderService>();

        //    ICXContainerFacade cxContainer = container.Get<ICXContainerFacade>();

        //    IModuleLoaderService moduleLoader = cxContainer.Resolve<IModuleLoaderService>();
        //    moduleLoader.LocateAndLoadComponents();

        //    cxContainer.Resolve<ILoggerService>("LoggerService");
        //}

        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void AnExceptionIsRaisedIfAComponentIsNotFound()
        //{
        //    IUnityContainer container = new UnityContainer()
        //        .AddNewExtension<PrismExtension>();
            
        //    container.RegisterInstance<IUnityContainer>(container)
        //        .Register<ICXContainerFacade, UnityContainerFacade>()
        //        .Register<IModuleLoaderService, ModuleLoaderService>();

        //    ICXContainerFacade cxContainer = container.Get<ICXContainerFacade>();
            
        //    IModuleLoaderService moduleLoader = cxContainer.Resolve<IModuleLoaderService>();
        //    moduleLoader.LocateAndLoadComponents();

        //    cxContainer.Resolve<IServiceProvider>();
        //}

        //[TestMethod]
        //public void CanResolveCascadingDependencies()
        //{
        //    IUnityContainer container = new UnityContainer()
        //        .AddNewExtension<PrismExtension>();

        //    container.RegisterInstance<IUnityContainer>(container)
        //        .Register<ICXContainerFacade, UnityContainerFacade>();

        //    ICXContainerFacade cxContainer = container.Get<ICXContainerFacade>();
        //    cxContainer.Register<InterfaceA, ClassA>();
        //    cxContainer.Register<InterfaceB, ClassB>();
        //    cxContainer.Register<InterfaceC, ClassC>();

        //    InterfaceC instanceC = cxContainer.Resolve<InterfaceC>();

        //    Assert.IsNotNull(instanceC);
        //    Assert.IsNotNull(instanceC.InstanceB);
        //    Assert.IsNotNull(instanceC.InstanceB.InstanceA);

        //}

        //[TestMethod]
        //public void CanResolveModuleWithCascadingDependencies()
        //{
        //    IUnityContainer container = new UnityContainer()
        //        .AddNewExtension<PrismExtension>();

        //    container.RegisterInstance<IUnityContainer>(container)
        //        .Register<ICXContainerFacade, UnityContainerFacade>()
        //        .Register<IModuleLoaderService, ModuleLoaderService>();

        //    ICXContainerFacade cxContainer = container.Get<ICXContainerFacade>();
        //    IModuleLoaderService moduleLoader = cxContainer.Resolve<IModuleLoaderService>();
        //    moduleLoader.LocateAndLoadComponents();

        //    IModule module = cxContainer.Resolve<IModule>("Module");

        //    Assert.IsNotNull(module);
        //}

        //[TestMethod]
        //public void CanRetrieveSingletonDependencies()
        //{
        //    IUnityContainer container = new UnityContainer()
        //        .AddNewExtension<PrismExtension>();

        //    container.RegisterInstance<IUnityContainer>(container)
        //        .Register<ICXContainerFacade, UnityContainerFacade>()
        //        .Register<IModuleLoaderService, ModuleLoaderService>();

        //    ICXContainerFacade cxContainer = container.Get<ICXContainerFacade>();
        //    IModuleLoaderService moduleLoader = cxContainer.Resolve<IModuleLoaderService>();
        //    moduleLoader.LocateAndLoadComponents();

        //    IFakeService instance1 = cxContainer.Resolve<IFakeService>();
        //    IFakeService instance2 = cxContainer.Resolve<IFakeService>();

        //    Assert.AreSame(instance2, instance1);
        //}

        //[TestMethod]
        //public void CanRegisterComponents()
        //{
        //    IUnityContainer container = new UnityContainer()
        //        .AddNewExtension<PrismExtension>();

        //    container.RegisterInstance<IUnityContainer>(container)
        //        .Register<ICXContainerFacade, UnityContainerFacade>();

        //    ICXContainerFacade cxContainer = container.Get<ICXContainerFacade>();

        //    cxContainer.Register<IView, MockView1>("mockView1")
        //               .Register<IView, MockView2>("mockView2");

        //    MockView1 mockView1 = (MockView1)cxContainer.Resolve<IView>("mockView1");
        //    MockView2 mockView2 = (MockView2)cxContainer.Resolve<IView>("mockView2");
        //}

        //[TestMethod]
        //public void ResolveAllReturnsAllRegisteredTypes()
        //{
        //    IUnityContainer container = new UnityContainer()
        //        .AddNewExtension<PrismExtension>();

        //    container.RegisterInstance<IUnityContainer>(container)
        //        .Register<ICXContainerFacade, UnityContainerFacade>();

        //    ICXContainerFacade cxContainer = container.Get<ICXContainerFacade>();

        //    cxContainer.Register<IView, MockView1>("mockView1")
        //               .Register<IView, MockView2>("mockView2");

        //    IEnumerable<IView> views = cxContainer.ResolveAll<IView>();
            
        //    Assert.AreEqual(2, views.Count<IView>());
        //}
    }
}
