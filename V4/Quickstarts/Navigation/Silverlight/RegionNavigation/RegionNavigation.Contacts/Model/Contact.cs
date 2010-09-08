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
namespace RegionNavigation.Contacts.Model
{
    public class Contact
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string emailAddress;

        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }

        private string address;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        private string company;

        public string Company
        {
            get { return company; }
            set { company = value; }
        }

        private string avatarUri;

        public string AvatarUri
        {
            get { return avatarUri; }
            set { avatarUri = value; }
        }   
    }
}
