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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using RegionNavigation.Email.Model;

namespace RegionNavigation.Email.ViewModels
{
    [Export]
    public class EmailsViewModel : NotificationObject
    {
        private readonly SynchronizationContext synchronizationContext;
        private readonly IEmailService emailService;
        private readonly IRegionManager regionManager;
        private readonly DelegateCommand<object> composeMessageCommand;
        private readonly DelegateCommand<object> replyMessageCommand;
        private readonly DelegateCommand<EmailDocument> openMessageCommand;

        public EmailsViewModel()
        {
            this.synchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();

            this.composeMessageCommand = new DelegateCommand<object>(this.ComposeMessage);
            this.replyMessageCommand = new DelegateCommand<object>(this.ReplyMessage, this.CanReplyMessage);
            this.openMessageCommand = new DelegateCommand<EmailDocument>(this.OpenMessage);

            this.Messages = new ObservableCollection<EmailDocument>();
            this.MessagesView = new PagedCollectionView(this.Messages);
            this.MessagesView.CurrentChanged += (s, e) =>
                this.replyMessageCommand.RaiseCanExecuteChanged();
        }

        [ImportingConstructor]
        public EmailsViewModel(IEmailService emailService, IRegionManager regionManager)
            : this()
        {
            this.emailService = emailService;
            this.regionManager = regionManager;

            this.emailService.BeginGetEmailDocuments(
                r =>
                {
                    var messages = this.emailService.EndGetEmailDocuments(r);

                    this.synchronizationContext.Post(
                        s =>
                        {
                            foreach (var message in messages)
                            {
                                this.Messages.Add(message);
                            }
                        },
                        null);
                },
                null);
        }

        public ObservableCollection<EmailDocument> Messages { get; private set; }

        public ICollectionView MessagesView { get; private set; }

        public ICommand ComposeMessageCommand
        {
            get { return this.composeMessageCommand; }
        }

        public ICommand ReplyMessageCommand
        {
            get { return this.replyMessageCommand; }
        }

        public ICommand OpenMessageCommand
        {
            get { return this.openMessageCommand; }
        }

        private void ComposeMessage(object ignored)
        {
            this.regionManager.RequestNavigate("MainContentRegion", new Uri("ComposeEmailView", UriKind.Relative), nr => { });
        }

        private void ReplyMessage(object ignored)
        {
            var currentEmail = this.MessagesView.CurrentItem as EmailDocument;
            var builder = new StringBuilder();
            builder.Append("ComposeEmailView");
            if (currentEmail != null)
            {
                var query = new UriQuery();
                query.Add("ReplyTo", currentEmail.Id.ToString("N"));
                builder.Append(query);
            }
            this.regionManager.RequestNavigate("MainContentRegion", new Uri(builder.ToString(), UriKind.Relative), nr => { });
        }

        private bool CanReplyMessage(object ignored)
        {
            return this.MessagesView.CurrentItem != null;
        }

        private void OpenMessage(EmailDocument document)
        {
            var builder = new StringBuilder();
            builder.Append("EmailView");
            var query = new UriQuery();
            query.Add("EmailId", document.Id.ToString("N"));
            builder.Append(query);
            this.regionManager.RequestNavigate("MainContentRegion", new Uri(builder.ToString(), UriKind.Relative), nr => { });
        }
    }
}
