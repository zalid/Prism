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

namespace RegionNavigation.Calendar.Model
{
    public class Meeting
    {
        private string subject;

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private DateTimeOffset startTime;

        public DateTimeOffset StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private DateTimeOffset endTime;

        public DateTimeOffset EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private Guid emailId;

        public Guid EmailId
        {
            get { return emailId; }
            set { emailId = value; }
        }
    }
}
