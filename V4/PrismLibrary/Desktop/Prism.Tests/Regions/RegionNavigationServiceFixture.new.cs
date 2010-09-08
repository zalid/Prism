//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.Prism.Tests.Regions
{
    [TestClass]
    public class RegionNavigationServiceFixture
    {
        [TestMethod]
        public void WhenNavigating_ViewIsActivated()
        {
            // Prepare
            object view = new object();
            Uri viewUri = new Uri(view.GetType().Name, UriKind.Relative);

            IRegion region = new Region();
            region.Add(view);

            string regionName = "RegionName";
            RegionManager regionManager = new RegionManager();
            regionManager.Regions.Add(regionName, region);

            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetInstance<IRegionNavigationJournalEntry>()).Returns(new RegionNavigationJournalEntry());

            IServiceLocator serviceLocator = serviceLocatorMock.Object;
            LocatorNavigationTargetHandler targetHandler = new Mock<LocatorNavigationTargetHandler>(serviceLocator).Object;
            IRegionNavigationJournal journal = new Mock<IRegionNavigationJournal>().Object;

            RegionNavigationService target = new RegionNavigationService(serviceLocator, targetHandler, journal);
            target.Region = region;

            // Act
            bool isNavigationSuccessful = false;
            target.RequestNavigate(viewUri, nr => isNavigationSuccessful = nr.Result == true);

            // Verify
            Assert.IsTrue(isNavigationSuccessful);
            bool isViewActive = region.ActiveViews.Contains(view);
            Assert.IsTrue(isViewActive);
        }

        [TestMethod]
        public void WhenNavigatingWithQueryString_ViewIsActivated()
        {
            // Prepare
            object view = new object();
            Uri viewUri = new Uri(view.GetType().Name + "?MyQuery=true", UriKind.Relative);

            IRegion region = new Region();
            region.Add(view);

            string regionName = "RegionName";
            RegionManager regionManager = new RegionManager();
            regionManager.Regions.Add(regionName, region);

            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetInstance<IRegionNavigationJournalEntry>()).Returns(new RegionNavigationJournalEntry());

            IServiceLocator serviceLocator = serviceLocatorMock.Object;
            LocatorNavigationTargetHandler targetHandler = new Mock<LocatorNavigationTargetHandler>(serviceLocator).Object;
            IRegionNavigationJournal journal = new Mock<IRegionNavigationJournal>().Object;

            RegionNavigationService target = new RegionNavigationService(serviceLocator, targetHandler, journal);
            target.Region = region;

            // Act
            bool isNavigationSuccessful = false;
            target.RequestNavigate(viewUri, nr => isNavigationSuccessful = nr.Result == true);

            // Verify
            Assert.IsTrue(isNavigationSuccessful);
            bool isViewActive = region.ActiveViews.Contains(view);
            Assert.IsTrue(isViewActive);
        }

        [TestMethod]
        public void WhenNavigatingAndViewCannotBeAcquired_ThenNavigationResultHasError()
        {
            // Prepare             
            object view = new object();
            Uri viewUri = new Uri(view.GetType().Name, UriKind.Relative);

            IRegion region = new Region();
            region.Add(view);

            string otherType = "OtherType";

            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetInstance<IRegionNavigationJournalEntry>()).Returns(new RegionNavigationJournalEntry());
            IServiceLocator serviceLocator = serviceLocatorMock.Object;

            Mock<INavigationTargetHandler> targetHandlerMock = new Mock<INavigationTargetHandler>();
            targetHandlerMock.Setup(th => th.GetTargetView(It.IsAny<IRegion>(), It.IsAny<NavigationContext>())).Throws<ArgumentException>();

            IRegionNavigationJournal journal = new Mock<IRegionNavigationJournal>().Object;

            RegionNavigationService target = new RegionNavigationService(serviceLocator, targetHandlerMock.Object, journal);
            target.Region = region;

            // Act
            Exception error = null;
            target.RequestNavigate(
                new Uri(otherType.GetType().Name, UriKind.Relative),
                nr =>
                {
                    error = nr.Error;
                });

            // Verify
            bool isViewActive = region.ActiveViews.Contains(view);
            Assert.IsFalse(isViewActive);
            Assert.IsInstanceOfType(error, typeof(ArgumentException));
        }

        [TestMethod]
        public void WhenNavigatingWithNullUri_Throws()
        {
            // Prepare
            IRegion region = new Region();

            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetInstance<IRegionNavigationJournalEntry>()).Returns(new RegionNavigationJournalEntry());

            IServiceLocator serviceLocator = serviceLocatorMock.Object;
            LocatorNavigationTargetHandler targetHandler = new Mock<LocatorNavigationTargetHandler>(serviceLocator).Object;
            IRegionNavigationJournal journal = new Mock<IRegionNavigationJournal>().Object;

            RegionNavigationService target = new RegionNavigationService(serviceLocator, targetHandler, journal);
            target.Region = region;

            // Act
            NavigationResult navigationResult = null;
            target.RequestNavigate((Uri)null, nr => navigationResult = nr);

            // Verify
            Assert.IsFalse(navigationResult.Result.Value);
            Assert.IsNotNull(navigationResult.Error);
            Assert.IsInstanceOfType(navigationResult.Error, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void WhenNavigatingAndViewImplementsINavigationAware_ThenNavigatedIsInvokedOnNavigation()
        {
            // Prepare
            var region = new Region();

            var viewMock = new Mock<INavigationAware>();
            viewMock.Setup(ina => ina.CanNavigateTo(It.IsAny<NavigationContext>())).Returns(true);
            var view = viewMock.Object;
            region.Add(view);

            var navigationUri = new Uri(view.GetType().Name, UriKind.Relative);

            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetInstance<IRegionNavigationJournalEntry>()).Returns(new RegionNavigationJournalEntry());

            IServiceLocator serviceLocator = serviceLocatorMock.Object;
            LocatorNavigationTargetHandler targetHandler = new Mock<LocatorNavigationTargetHandler>(serviceLocator).Object;
            IRegionNavigationJournal journal = new Mock<IRegionNavigationJournal>().Object;

            RegionNavigationService target = new RegionNavigationService(serviceLocator, targetHandler, journal);
            target.Region = region;

            // Act
            target.RequestNavigate(navigationUri, nr => { });

            // Verify
            viewMock.Verify(v => v.OnNavigatedTo(It.Is<NavigationContext>(nc => nc.Uri == navigationUri && nc.NavigationService == target)));
        }

        [TestMethod]
        public void WhenNavigatingAndDataContextImplementsINavigationAware_ThenNavigatedIsInvokesOnNavigation()
        {
            // Prepare
            var region = new Region();

            Mock<FrameworkElement> mockFrameworkElement = new Mock<FrameworkElement>();
            Mock<INavigationAware> mockINavigationAwareDataContext = new Mock<INavigationAware>();
            mockINavigationAwareDataContext.Setup(ina => ina.CanNavigateTo(It.IsAny<NavigationContext>())).Returns(true);
            mockFrameworkElement.Object.DataContext = mockINavigationAwareDataContext.Object;

            var view = mockFrameworkElement.Object;
            region.Add(view);

            var navigationUri = new Uri(view.GetType().Name, UriKind.Relative);

            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetInstance<IRegionNavigationJournalEntry>()).Returns(new RegionNavigationJournalEntry());

            IServiceLocator serviceLocator = serviceLocatorMock.Object;
            LocatorNavigationTargetHandler targetHandler = new Mock<LocatorNavigationTargetHandler>(serviceLocator).Object;
            IRegionNavigationJournal journal = new Mock<IRegionNavigationJournal>().Object;

            RegionNavigationService target = new RegionNavigationService(serviceLocator, targetHandler, journal);
            target.Region = region;

            // Act
            target.RequestNavigate(navigationUri, nr => { });

            // Verify
            mockINavigationAwareDataContext.Verify(v => v.OnNavigatedTo(It.Is<NavigationContext>(nc => nc.Uri == navigationUri)));
        }

        [TestMethod]
        public void WhenNavigatingAndBothViewAndDataContextImplementINavigationAware_ThenNavigatedIsInvokesOnNavigation()
        {
            // Prepare
            var region = new Region();

            Mock<FrameworkElement> mockFrameworkElement = new Mock<FrameworkElement>();
            Mock<INavigationAware> mockINavigationAwareView = mockFrameworkElement.As<INavigationAware>();
            mockINavigationAwareView.Setup(ina => ina.CanNavigateTo(It.IsAny<NavigationContext>())).Returns(true);

            Mock<INavigationAware> mockINavigationAwareDataContext = new Mock<INavigationAware>();
            mockINavigationAwareDataContext.Setup(ina => ina.CanNavigateTo(It.IsAny<NavigationContext>())).Returns(true);
            mockFrameworkElement.Object.DataContext = mockINavigationAwareDataContext.Object;

            var view = mockFrameworkElement.Object;
            region.Add(view);

            var navigationUri = new Uri(view.GetType().Name, UriKind.Relative);

            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetInstance<IRegionNavigationJournalEntry>()).Returns(new RegionNavigationJournalEntry());

            IServiceLocator serviceLocator = serviceLocatorMock.Object;
            LocatorNavigationTargetHandler targetHandler = new Mock<LocatorNavigationTargetHandler>(serviceLocator).Object;
            IRegionNavigationJournal journal = new Mock<IRegionNavigationJournal>().Object;

            RegionNavigationService target = new RegionNavigationService(serviceLocator, targetHandler, journal);
            target.Region = region;

            // Act
            target.RequestNavigate(navigationUri, nr => { });

            // Verify
            mockINavigationAwareView.Verify(v => v.OnNavigatedTo(It.Is<NavigationContext>(nc => nc.Uri == navigationUri)));
            mockINavigationAwareDataContext.Verify(v => v.OnNavigatedTo(It.Is<NavigationContext>(nc => nc.Uri == navigationUri)));
        }

        [TestMethod]
        public void WhenNavigating_NavigationIsRecordedInJournal()
        {
            // Prepare
            object view = new object();
            Uri viewUri = new Uri(view.GetType().Name, UriKind.Relative);

            IRegion region = new Region();
            region.Add(view);

            string regionName = "RegionName";
            RegionManager regionManager = new RegionManager();
            regionManager.Regions.Add(regionName, region);

            IRegionNavigationJournalEntry journalEntry = new RegionNavigationJournalEntry();

            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetInstance<IRegionNavigationJournalEntry>())
                .Returns(journalEntry);

            IServiceLocator serviceLocator = serviceLocatorMock.Object;
            LocatorNavigationTargetHandler targetHandler = new Mock<LocatorNavigationTargetHandler>(serviceLocator).Object;

            var journalMock = new Mock<IRegionNavigationJournal>();
            journalMock.Setup(x => x.RecordNavigation(journalEntry)).Verifiable();

            IRegionNavigationJournal journal = journalMock.Object;


            RegionNavigationService target = new RegionNavigationService(serviceLocator, targetHandler, journal);
            target.Region = region;

            // Act
            target.RequestNavigate(viewUri, nr => { });

            // Verify
            Assert.IsNotNull(journalEntry);
            Assert.AreEqual(viewUri, journalEntry.Uri);
            journalMock.VerifyAll();
        }

        [TestMethod]
        public void WhenNavigatingAndCurrentlyActiveViewImplementsINavigateWithVeto_ThenNavigationRequestQueriesForVeto()
        {
            // Prepare
            var region = new Region();

            var viewMock = new Mock<INavigationAwareWithVeto>();
            viewMock
                .Setup(ina => ina.RequestCanNavigateFrom(It.IsAny<NavigationContext>(), It.IsAny<Action<bool>>()))
                .Verifiable();

            var view = viewMock.Object;
            region.Add(view);
            region.Activate(view);

            var navigationUri = new Uri(view.GetType().Name, UriKind.Relative);

            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetInstance<IRegionNavigationJournalEntry>()).Returns(new RegionNavigationJournalEntry());

            IServiceLocator serviceLocator = serviceLocatorMock.Object;
            LocatorNavigationTargetHandler targetHandler = new Mock<LocatorNavigationTargetHandler>(serviceLocator).Object;
            IRegionNavigationJournal journal = new Mock<IRegionNavigationJournal>().Object;

            RegionNavigationService target = new RegionNavigationService(serviceLocator, targetHandler, journal);
            target.Region = region;

            // Act
            target.RequestNavigate(navigationUri, nr => { });

            // Verify
            viewMock.VerifyAll();
        }

        [TestMethod]
        public void WhenNavigating_ThenNavigationRequestQueriesForVetoOnAllActiveViewsIfAllSucceed()
        {
            // Prepare
            var region = new Region();

            var view1Mock = new Mock<INavigationAwareWithVeto>();
            view1Mock
                .Setup(ina => ina.RequestCanNavigateFrom(It.IsAny<NavigationContext>(), It.IsAny<Action<bool>>()))
                .Callback<NavigationContext, Action<bool>>((nc, c) => c(true))
                .Verifiable();

            var view1 = view1Mock.Object;
            region.Add(view1);
            region.Activate(view1);

            var view2Mock = new Mock<INavigationAwareWithVeto>();

            var view2 = view2Mock.Object;
            region.Add(view2);

            var view3Mock = new Mock<INavigationAware>();

            var view3 = view3Mock.Object;
            region.Add(view3);
            region.Activate(view3);

            var view4Mock = new Mock<INavigationAwareWithVeto>();
            view4Mock
                .Setup(ina => ina.RequestCanNavigateFrom(It.IsAny<NavigationContext>(), It.IsAny<Action<bool>>()))
                .Callback<NavigationContext, Action<bool>>((nc, c) => c(true))
                .Verifiable();

            var view4 = view4Mock.Object;
            region.Add(view4);
            region.Activate(view4);

            var navigationUri = new Uri(view1.GetType().Name, UriKind.Relative);

            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetInstance<IRegionNavigationJournalEntry>()).Returns(new RegionNavigationJournalEntry());

            IServiceLocator serviceLocator = serviceLocatorMock.Object;
            LocatorNavigationTargetHandler targetHandler = new Mock<LocatorNavigationTargetHandler>(serviceLocator).Object;
            IRegionNavigationJournal journal = new Mock<IRegionNavigationJournal>().Object;

            RegionNavigationService target = new RegionNavigationService(serviceLocator, targetHandler, journal);
            target.Region = region;

            // Act
            target.RequestNavigate(navigationUri, nr => { });

            // Verify
            view1Mock.VerifyAll();
            view2Mock.Verify(ina => ina.RequestCanNavigateFrom(It.IsAny<NavigationContext>(), It.IsAny<Action<bool>>()), Times.Never());
            view3Mock.VerifyAll();
            view4Mock.VerifyAll();
        }

        [TestMethod]
        public void WhenRequestNavigateAwayAcceptsThroughCallback_ThenNavigationProceeds()
        {
            // Prepare
            var region = new Region();

            var view1Mock = new Mock<INavigationAwareWithVeto>();
            view1Mock
                .Setup(ina => ina.RequestCanNavigateFrom(It.IsAny<NavigationContext>(), It.IsAny<Action<bool>>()))
                .Callback<NavigationContext, Action<bool>>((nc, c) => c(true))
                .Verifiable();

            var view1 = view1Mock.Object;

            var view2 = new object();

            region.Add(view1);
            region.Add(view2);

            region.Activate(view1);

            var navigationUri = new Uri(view2.GetType().Name, UriKind.Relative);

            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetInstance<IRegionNavigationJournalEntry>()).Returns(new RegionNavigationJournalEntry());

            IServiceLocator serviceLocator = serviceLocatorMock.Object;
            LocatorNavigationTargetHandler targetHandler = new Mock<LocatorNavigationTargetHandler>(serviceLocator).Object;
            IRegionNavigationJournal journal = new Mock<IRegionNavigationJournal>().Object;

            RegionNavigationService target = new RegionNavigationService(serviceLocator, targetHandler, journal);
            target.Region = region;

            // Act
            var navigationSucceeded = false;
            target.RequestNavigate(navigationUri, nr => { navigationSucceeded = nr.Result == true; });

            // Verify
            view1Mock.VerifyAll();
            Assert.IsTrue(navigationSucceeded);
            CollectionAssert.AreEqual(new object[] { view1, view2 }, region.ActiveViews.ToArray());
        }

        [TestMethod]
        public void WhenRequestNavigateAwayRejectsThroughCallback_ThenNavigationDoesNotProceed()
        {
            // Prepare
            var region = new Region();

            var view1Mock = new Mock<INavigationAwareWithVeto>();
            view1Mock
                .Setup(ina => ina.RequestCanNavigateFrom(It.IsAny<NavigationContext>(), It.IsAny<Action<bool>>()))
                .Callback<NavigationContext, Action<bool>>((nc, c) => c(false))
                .Verifiable();

            var view1 = view1Mock.Object;

            var view2 = new object();

            region.Add(view1);
            region.Add(view2);

            region.Activate(view1);

            var navigationUri = new Uri(view2.GetType().Name, UriKind.Relative);

            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetInstance<IRegionNavigationJournalEntry>()).Returns(new RegionNavigationJournalEntry());

            IServiceLocator serviceLocator = serviceLocatorMock.Object;
            LocatorNavigationTargetHandler targetHandler = new Mock<LocatorNavigationTargetHandler>(serviceLocator).Object;
            IRegionNavigationJournal journal = new Mock<IRegionNavigationJournal>().Object;

            RegionNavigationService target = new RegionNavigationService(serviceLocator, targetHandler, journal);
            target.Region = region;

            // Act
            var navigationFailed = false;
            target.RequestNavigate(navigationUri, nr => { navigationFailed = nr.Result == false; });

            // Verify
            view1Mock.VerifyAll();
            Assert.IsTrue(navigationFailed);
            CollectionAssert.AreEqual(new object[] { view1 }, region.ActiveViews.ToArray());
        }

        [TestMethod]
        public void WhenNavigatingAndDataContextOnCurrentlyActiveViewImplementsINavigateWithVeto_ThenNavigationRequestQueriesForVeto()
        {
            // Prepare
            var region = new Region();

            var viewModelMock = new Mock<INavigationAwareWithVeto>();
            viewModelMock
                .Setup(ina => ina.RequestCanNavigateFrom(It.IsAny<NavigationContext>(), It.IsAny<Action<bool>>()))
                .Verifiable();

            var viewMock = new Mock<FrameworkElement>();

            var view = viewMock.Object;
            view.DataContext = viewModelMock.Object;

            region.Add(view);
            region.Activate(view);

            var navigationUri = new Uri(view.GetType().Name, UriKind.Relative);

            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetInstance<IRegionNavigationJournalEntry>()).Returns(new RegionNavigationJournalEntry());

            IServiceLocator serviceLocator = serviceLocatorMock.Object;
            LocatorNavigationTargetHandler targetHandler = new Mock<LocatorNavigationTargetHandler>(serviceLocator).Object;
            IRegionNavigationJournal journal = new Mock<IRegionNavigationJournal>().Object;

            RegionNavigationService target = new RegionNavigationService(serviceLocator, targetHandler, journal);
            target.Region = region;

            // Act
            target.RequestNavigate(navigationUri, nr => { });

            // Verify
            viewModelMock.VerifyAll();
        }

        [TestMethod]
        public void WhenRequestNavigateAwayOnDataContextAcceptsThroughCallback_ThenNavigationProceeds()
        {
            // Prepare
            var region = new Region();

            var view1DataContextMock = new Mock<INavigationAwareWithVeto>();
            view1DataContextMock
                .Setup(ina => ina.RequestCanNavigateFrom(It.IsAny<NavigationContext>(), It.IsAny<Action<bool>>()))
                .Callback<NavigationContext, Action<bool>>((nc, c) => c(true))
                .Verifiable();

            var view1Mock = new Mock<FrameworkElement>();
            var view1 = view1Mock.Object;
            view1.DataContext = view1DataContextMock.Object;

            var view2 = new object();

            region.Add(view1);
            region.Add(view2);

            region.Activate(view1);

            var navigationUri = new Uri(view2.GetType().Name, UriKind.Relative);

            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetInstance<IRegionNavigationJournalEntry>()).Returns(new RegionNavigationJournalEntry());

            IServiceLocator serviceLocator = serviceLocatorMock.Object;
            LocatorNavigationTargetHandler targetHandler = new Mock<LocatorNavigationTargetHandler>(serviceLocator).Object;
            IRegionNavigationJournal journal = new Mock<IRegionNavigationJournal>().Object;

            RegionNavigationService target = new RegionNavigationService(serviceLocator, targetHandler, journal);
            target.Region = region;

            // Act
            var navigationSucceeded = false;
            target.RequestNavigate(navigationUri, nr => { navigationSucceeded = nr.Result == true; });

            // Verify
            view1DataContextMock.VerifyAll();
            Assert.IsTrue(navigationSucceeded);
            CollectionAssert.AreEqual(new object[] { view1, view2 }, region.ActiveViews.ToArray());
        }

        [TestMethod]
        public void WhenRequestNavigateAwayOnDataContextRejectsThroughCallback_ThenNavigationDoesNotProceed()
        {
            // Prepare
            var region = new Region();

            var view1DataContextMock = new Mock<INavigationAwareWithVeto>();
            view1DataContextMock
                .Setup(ina => ina.RequestCanNavigateFrom(It.IsAny<NavigationContext>(), It.IsAny<Action<bool>>()))
                .Callback<NavigationContext, Action<bool>>((nc, c) => c(false))
                .Verifiable();

            var view1Mock = new Mock<FrameworkElement>();
            var view1 = view1Mock.Object;
            view1.DataContext = view1DataContextMock.Object;

            var view2 = new object();

            region.Add(view1);
            region.Add(view2);

            region.Activate(view1);

            var navigationUri = new Uri(view2.GetType().Name, UriKind.Relative);

            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetInstance<IRegionNavigationJournalEntry>()).Returns(new RegionNavigationJournalEntry());

            IServiceLocator serviceLocator = serviceLocatorMock.Object;
            LocatorNavigationTargetHandler targetHandler = new Mock<LocatorNavigationTargetHandler>(serviceLocator).Object;
            IRegionNavigationJournal journal = new Mock<IRegionNavigationJournal>().Object;

            RegionNavigationService target = new RegionNavigationService(serviceLocator, targetHandler, journal);
            target.Region = region;

            // Act
            var navigationFailed = false;
            target.RequestNavigate(navigationUri, nr => { navigationFailed = nr.Result == false; });

            // Verify
            view1DataContextMock.VerifyAll();
            Assert.IsTrue(navigationFailed);
            CollectionAssert.AreEqual(new object[] { view1 }, region.ActiveViews.ToArray());
        }

        [TestMethod]
        public void WhenNavigationRequestIsInProgress_ThenNewRequestCausesException()
        {
            // Prepare
            var region = new Region();

            var viewMock = new Mock<INavigationAwareWithVeto>();
            NavigationContext navigationContext = null;
            Action<bool> navigationCallback = null;
            viewMock
                .Setup(ina => ina.RequestCanNavigateFrom(It.IsAny<NavigationContext>(), It.IsAny<Action<bool>>()))
                .Callback<NavigationContext, Action<bool>>((nc, c) => { navigationContext = nc; navigationCallback = c; });

            var view = viewMock.Object;
            region.Add(view);
            region.Activate(view);

            var navigationUri = new Uri(view.GetType().Name, UriKind.Relative);

            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock.Setup(x => x.GetInstance<IRegionNavigationJournalEntry>()).Returns(new RegionNavigationJournalEntry());

            var serviceLocator = serviceLocatorMock.Object;
            var targetHandler = new Mock<INavigationTargetHandler>().Object;
            var journal = new Mock<IRegionNavigationJournal>().Object;

            var target = new RegionNavigationService(serviceLocator, targetHandler, journal);
            target.Region = region;

            NavigationResult firstNavigationResult = null;
            target.RequestNavigate(navigationUri, nr => firstNavigationResult = nr);

            // Act
            NavigationResult secondNavigationResult = null;
            target.RequestNavigate(navigationUri, nr => secondNavigationResult = nr);

            // Verify
            viewMock.VerifyAll();
            Assert.IsNull(firstNavigationResult);
            Assert.IsNotNull(secondNavigationResult);
            Assert.IsNotNull(secondNavigationResult.Error);
            Assert.IsInstanceOfType(secondNavigationResult.Error, typeof(InvalidOperationException));
        }
    }
}
