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
using Microsoft.Practices.Prism.Regions;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionNavigation.Email.Model;
using RegionNavigation.Email.ViewModels;

namespace RegionNavigation.Email.Tests
{
    [TestClass]
    public class EmailViewModelFixture : SilverlightTest
    {
        [TestMethod]
        public void WhenNavigatedTo_ThenRequestsEmailFromService()
        {
            var email = new EmailDocument();

            var emailServiceMock = new Mock<IEmailService>();
            emailServiceMock
                .Setup(svc => svc.GetEmailDocument(email.Id))
               .Returns(email)
               .Verifiable();

            var viewModel = new EmailViewModel(emailServiceMock.Object);

            var notified = false;
            viewModel.PropertyChanged += (s, o) => notified = o.PropertyName == "Email";
            viewModel.OnNavigatedTo(
                new NavigationContext(
                    new Mock<IRegionNavigationService>().Object,
                    new Uri("location?EmailId=" + email.Id.ToString("N"), UriKind.Relative)));

            Assert.IsTrue(notified);
            emailServiceMock.VerifyAll();
        }

        [TestMethod]
        public void WhenAskedCanNavigateForSameQuery_ThenReturnsTrue()
        {
            var email = new EmailDocument();

            var emailServiceMock = new Mock<IEmailService>();
            emailServiceMock
                .Setup(svc => svc.GetEmailDocument(email.Id))
               .Returns(email)
               .Verifiable();

            var viewModel = new EmailViewModel(emailServiceMock.Object);

            viewModel.OnNavigatedTo(new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("location?EmailId=" + email.Id.ToString("N"), UriKind.Relative)));

            bool canNavigate =
                viewModel.CanNavigateTo(new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("location?EmailId=" + email.Id.ToString("N"), UriKind.Relative)));

            Assert.IsTrue(canNavigate);
        }

        [TestMethod]
        public void WhenAskedCanNavigateForDifferentQuery_ThenReturnsFalse()
        {
            var email = new EmailDocument();

            var emailServiceMock = new Mock<IEmailService>();
            emailServiceMock
                .Setup(svc => svc.GetEmailDocument(email.Id))
               .Returns(email)
               .Verifiable();

            var viewModel = new EmailViewModel(emailServiceMock.Object);

            viewModel.OnNavigatedTo(new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("location?EmailId=" + email.Id.ToString("N"), UriKind.Relative)));

            bool canNavigate =
                viewModel.CanNavigateTo(new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("location?EmailId=" + Guid.NewGuid().ToString("N"), UriKind.Relative)));

            Assert.IsFalse(canNavigate);
        }
    }
}
