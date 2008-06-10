//===============================================================================
// Microsoft patterns & practices
// Composite WPF (PRISM)
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

using System;

namespace Prism.Events
{

    /// <summary>
    /// Subsription token returned from <see cref="PrismEvent{TPayload}"/> on subscribe.
    /// </summary>
    public class SubscriptionToken : IEquatable<SubscriptionToken>
    {
        private readonly Guid _token;
        public SubscriptionToken()
        {
            _token = Guid.NewGuid();
        }

        public bool Equals(SubscriptionToken subscriptionToken)
        {
            if (subscriptionToken == null) return false;
            return Equals(_token, subscriptionToken._token);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as SubscriptionToken);
        }

        public override int GetHashCode()
        {
            return _token.GetHashCode();
        }
    }
}