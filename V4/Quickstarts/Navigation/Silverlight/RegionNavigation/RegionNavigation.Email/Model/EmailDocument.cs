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
namespace RegionNavigation.Email.Model
{
    public class EmailDocument
    {
        public EmailDocument()
            : this(Guid.NewGuid())
        {
        }

        public EmailDocument(Guid id)
        {
            this.id = id;
        }

        private string from;

        public string From
        {
            get { return from; }
            set { from = value; }
        }

        private string to;

        public string To
        {
            get { return to; }
            set { to = value; }
        }

        private string subject;

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        private Guid id;

        public Guid Id
        {
            get { return id; }
        }
    }
}
