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

/*
 * Derived class that filters data in the diagram view.
*/

using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.FamilyShowLib;

namespace Microsoft.FamilyShow
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    class FamilyDisplayListView : FilterSortListView
    {
        /// <summary>
        /// Called for each item in the list. Return true if the item should be in
        /// the current result set, otherwise return false to exclude the item.
        /// </summary>
        protected override bool FilterCallback(object item)
        {
            Person person = item as Person;
            if (person == null)
                return false;

            if (this.Filter.Matches(person.Name) ||
                this.Filter.MatchesYear(person.BirthDate) ||
                this.Filter.MatchesYear(person.DeathDate) ||
                this.Filter.Matches(person.Age))
                return true;

            return false;
        }
    }
}
