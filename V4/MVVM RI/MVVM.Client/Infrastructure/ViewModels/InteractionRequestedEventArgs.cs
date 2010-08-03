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

namespace MVVM.Client.Infrastructure.ViewModels
{
    /// <summary>
    /// Event args for the <see cref="IInteractionRequest.Raised"/> event.
    /// </summary>
    public class InteractionRequestedEventArgs : EventArgs
    {
        public InteractionRequestedEventArgs(Notification context, Action callback)
        {
            this.Context = context;
            this.Callback = callback;
        }

        /// <summary>
        /// Gets the context for a requested interaction.
        /// </summary>
        public Notification Context { get; private set; }

        /// <summary>
        /// Gets the callback to execute when an interaction is completed.
        /// </summary>
        public Action Callback { get; private set; }
    }
}
