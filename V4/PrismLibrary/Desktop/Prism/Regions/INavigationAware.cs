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

namespace Microsoft.Practices.Prism.Regions
{
    /// <summary>
    /// Provides a way for objects involved in navigation to be notified of navigation activities.
    /// </summary>
    public interface INavigationAware
    {
        /// <summary>
        /// Indicates that the receiver has been the target of a navigation request.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        void OnNavigatedTo(NavigationContext navigationContext);

        /// <summary>
        /// Determines whether this instance accepts being the target of a navigation request.
        /// </summary>
        /// <param name="navigationContext">The navigation context.</param>
        /// <returns>
        /// <see langword="true"/> if this instance accepts the navigation request; otherwise, <see langword="false"/>.
        /// </returns>
        bool CanNavigateTo(NavigationContext navigationContext);
    }

}