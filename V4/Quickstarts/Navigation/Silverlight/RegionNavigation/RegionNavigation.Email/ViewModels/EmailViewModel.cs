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
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using RegionNavigation.Email.Model;

namespace RegionNavigation.Email.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class EmailViewModel : NotificationObject, INavigationAware
    {
        private readonly IEmailService emailService;
        private EmailDocument email;

        public EmailViewModel()
        { }

        [ImportingConstructor]
        public EmailViewModel(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public EmailDocument Email
        {
            get
            {
                return this.email;
            }

            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    this.RaisePropertyChanged(() => this.Email);
                }
            }
        }

        public bool CanNavigateTo(NavigationContext navigationContext)
        {
            if (this.email == null)
            {
                return true;
            }

            var emailId = GetEmailId(navigationContext);

            return emailId.HasValue && emailId.Value == this.email.Id;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var emailId = GetEmailId(navigationContext);
            if (emailId.HasValue)
            {
                this.Email = this.emailService.GetEmailDocument(emailId.Value);
            }
        }

        private Guid? GetEmailId(NavigationContext navigationContext)
        {
            var email = navigationContext.UriQuery["EmailId"];
            Guid emailId;
            if (email != null && Guid.TryParse(email, out emailId))
            {
                return emailId;
            }

            return null;
        }
    }
}
