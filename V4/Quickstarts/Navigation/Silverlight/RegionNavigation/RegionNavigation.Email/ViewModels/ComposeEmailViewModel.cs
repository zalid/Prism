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
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using RegionNavigation.Email.Model;

namespace RegionNavigation.Email.ViewModels
{
    [Export]
    public class ComposeEmailViewModel : NotificationObject, INavigationAware
    {
        private readonly SynchronizationContext synchronizationContext;
        private readonly IEmailService emailService;
        private readonly IRegionManager regionManager;
        private readonly DelegateCommand<object> sendEmailCommand;
        private readonly DelegateCommand<object> goBackCommand;
        private EmailDocument emailDocument;
        private string sendState;

        public ComposeEmailViewModel()
        {
            this.synchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();

            this.sendEmailCommand = new DelegateCommand<object>(this.SendEmail);

            this.sendState = "Normal";
        }

        [ImportingConstructor]
        public ComposeEmailViewModel(IEmailService emailService, IRegionManager regionManager)
            : this()
        {
            this.emailService = emailService;
            this.regionManager = regionManager;
        }

        public ICommand SendEmailCommand
        {
            get { return this.sendEmailCommand; }
        }

        public EmailDocument EmailDocument
        {
            get
            {
                return this.emailDocument;
            }

            set
            {
                if (this.emailDocument != value)
                {
                    this.emailDocument = value;
                    this.RaisePropertyChanged(() => this.EmailDocument);
                }
            }
        }

        public string SendState
        {
            get
            {
                return this.sendState;
            }

            private set
            {
                if (this.sendState != value)
                {
                    this.sendState = value;
                    this.RaisePropertyChanged(() => this.SendState);
                }
            }
        }


        public bool CanNavigateTo(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var emailDocument = new EmailDocument();

            var query = navigationContext.UriQuery;

            var replyTo = query["ReplyTo"];
            Guid replyToId;
            if (replyTo != null && Guid.TryParse(replyTo, out replyToId))
            {
                var replyToEmail = this.emailService.GetEmailDocument(replyToId);
                if (replyToEmail != null)
                {
                    emailDocument.To = replyToEmail.From;
                    emailDocument.Subject = "RE: " + replyToEmail.Subject;

                    emailDocument.Text =
                        Environment.NewLine +
                        replyToEmail.Text
                            .Split(Environment.NewLine.ToCharArray())
                            .Select(l => l.Length > 0 ? ">> " + l : l)
                            .Aggregate((l1, l2) => l1 + Environment.NewLine + l2);
                }
            }
            else
            {
                var to = query["To"];
                if (to != null)
                {
                    emailDocument.To = to;
                }
            }

            this.EmailDocument = emailDocument;
        }

        private void SendEmail(object ignored)
        {
            this.SendState = "Sending";
            this.emailService.BeginSendEmailDocument(
                this.emailDocument,
                r =>
                {
                    this.synchronizationContext.Post(
                        s =>
                        {
                            this.regionManager.RequestNavigate("MainContentRegion", new Uri("EmailsView", UriKind.Relative), nr => { });
                            this.SendState = "Normal";
                        },
                        null);
                },
                null);
        }
    }
}
