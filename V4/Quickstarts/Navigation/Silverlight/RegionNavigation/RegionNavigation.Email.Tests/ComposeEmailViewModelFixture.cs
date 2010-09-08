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
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionNavigation.Email.Model;
using RegionNavigation.Email.ViewModels;

namespace RegionNavigation.Email.Tests
{
    [TestClass]
    public class ComposeEmailViewModelFixture : SilverlightTest
    {
        [TestMethod]
        public void WhenSendMessageCommandIsExecuted_ThenSendsMessageThroughService()
        {
            var emailServiceMock = new Mock<IEmailService>();

            var regionManagerMock = new Mock<IRegionManager>(MockBehavior.Strict);

            var viewModel = new ComposeEmailViewModel(emailServiceMock.Object, regionManagerMock.Object);
            viewModel.OnNavigatedTo(new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("", UriKind.Relative)));

            viewModel.SendEmailCommand.Execute(null);

            Assert.AreEqual("Sending", viewModel.SendState);

            emailServiceMock.Verify(svc => svc.BeginSendEmailDocument(viewModel.EmailDocument, It.IsAny<AsyncCallback>(), null));
        }

        [TestMethod]
        public void WhenNavigatedToWithAReplyToQueryParameter_ThenRepliesToTheAppropriateMessage()
        {
            var replyToEmail = new EmailDocument { From = "somebody@contoso.com", To = "", Subject = "", Text = "" };

            var emailServiceMock = new Mock<IEmailService>();
            emailServiceMock
                .Setup(svc => svc.GetEmailDocument(replyToEmail.Id))
                .Returns(replyToEmail);

            var regionManagerMock = new Mock<IRegionManager>(MockBehavior.Strict);

            var viewModel = new ComposeEmailViewModel(emailServiceMock.Object, regionManagerMock.Object);
            var uriQuery = new UriQuery();
            uriQuery.Add("ReplyTo", replyToEmail.Id.ToString("N"));
            viewModel.OnNavigatedTo(new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("" + uriQuery.ToString(), UriKind.Relative)));

            Assert.AreEqual("somebody@contoso.com", viewModel.EmailDocument.To);
        }

        [TestMethod]
        public void WhenNavigatedToWithAToQueryParameter_ThenInitializesToField()
        {
            var emailServiceMock = new Mock<IEmailService>();

            var regionManagerMock = new Mock<IRegionManager>(MockBehavior.Strict);

            var viewModel = new ComposeEmailViewModel(emailServiceMock.Object, regionManagerMock.Object);
            var uriQuery = new UriQuery();
            uriQuery.Add("To", "somebody@contoso.com");
            viewModel.OnNavigatedTo(new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("" + uriQuery.ToString(), UriKind.Relative)));

            Assert.AreEqual("somebody@contoso.com", viewModel.EmailDocument.To);
        }

        [TestMethod]
        [Asynchronous]
        [Timeout(5000)]
        public void WhenFinishedSendingMessage_ThenNavigatesToEmaisView()
        {
            var sendEmailResultMock = new Mock<IAsyncResult>();

            var emailServiceMock = new Mock<IEmailService>();
            AsyncCallback callback = null;
            emailServiceMock
                .Setup(svc => svc.BeginSendEmailDocument(It.IsAny<EmailDocument>(), It.IsAny<AsyncCallback>(), null))
                .Callback<EmailDocument, AsyncCallback, object>((e, c, o) => { callback = c; })
                .Returns(sendEmailResultMock.Object);
            emailServiceMock
                .Setup(svc => svc.EndSendEmailDocument(sendEmailResultMock.Object))
                .Verifiable();


            Mock<IRegion> regionMock = new Mock<IRegion>();
            regionMock
                .Setup(x => x.RequestNavigate(new Uri(@"EmailsView", UriKind.Relative), It.IsAny<Action<NavigationResult>>()))
                .Callback<Uri, Action<NavigationResult>>((s, c) => c(new NavigationResult(null, true)))
                .Verifiable();

            Mock<IRegionManager> regionManagerMock = new Mock<IRegionManager>();
            regionManagerMock.Setup(x => x.Regions.ContainsRegionWithName("MainContentRegion")).Returns(true);
            regionManagerMock.Setup(x => x.Regions["MainContentRegion"]).Returns(regionMock.Object);  


            var viewModel = new ComposeEmailViewModel(emailServiceMock.Object, regionManagerMock.Object);
            viewModel.OnNavigatedTo(new NavigationContext(new Mock<IRegionNavigationService>().Object, new Uri("", UriKind.Relative)));

            viewModel.SendEmailCommand.Execute(null);

            this.EnqueueConditional(() => callback != null);

            this.EnqueueCallback(
                () =>
                {
                    callback(sendEmailResultMock.Object);
                });

            this.EnqueueCallback(
                () =>
                {
                    Assert.AreEqual("Normal", viewModel.SendState);

                    regionMock.VerifyAll();
                });

            this.EnqueueTestComplete();
        }
    }
}
