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
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using RegionNavigation.Infrastructure;

namespace RegionNavigation.Contacts.Model
{
    [Export(typeof(IContactsService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ContactsService : IContactsService
    {
        private static readonly string Avatar1Uri = @"/RegionNavigation.Contacts;component/Avatars/MC900432625.PNG";
        private static readonly string Avatar2Uri = @"/RegionNavigation.Contacts;component/Avatars/MC900433938.PNG";
        private static readonly string Avatar3Uri = @"/RegionNavigation.Contacts;component/Avatars/MC900433946.PNG";
        private static readonly string Avatar4Uri = @"/RegionNavigation.Contacts;component/Avatars/MC900434899.PNG";

        public IAsyncResult BeginGetContacts(AsyncCallback callback, object userState)
        {
            var asyncResult = new AsyncResult<IEnumerable<Contact>>(callback, userState);
            ThreadPool.QueueUserWorkItem(
                o =>
                {
                    asyncResult.SetComplete(CreateContacts(), false);
                });

            return asyncResult;
        }

        private static IEnumerable<Contact> CreateContacts()
        {
            List<Contact> contacts = new List<Contact>();
            contacts.Add(new Contact() { Name = "Joan Gregory", Company = "Contoso", EmailAddress = "jg@contoso.com", Address = "31 North St.", AvatarUri = Avatar3Uri });
            contacts.Add(new Contact() { Name = "John Smith", Company = "TailSpin", EmailAddress = "john@tailspin.com", Address = "123 Main Street", AvatarUri = Avatar1Uri });
            contacts.Add(new Contact() { Name = "Sally Swan", Company = "Fabrikam", EmailAddress = "sally@fabrikam.com", Address = "15601 West Place", AvatarUri = Avatar4Uri });
            contacts.Add(new Contact() { Name = "Gerald Wagner", Company = "TailSpin", EmailAddress = "gwag@tailspin.com", Address = "1934 Regus Ave", AvatarUri = Avatar2Uri });
            return contacts;
        }

        public IEnumerable<Contact> EndGetContacts(IAsyncResult asyncResult)
        {
            var localAsyncResult = AsyncResult<IEnumerable<Contact>>.End(asyncResult);

            return localAsyncResult.Result;
        }
    }
}
