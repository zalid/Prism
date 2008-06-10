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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FamilyShow.Infrastructure;
using Microsoft.FamilyShowLib;
using Prism.Interfaces;
using System.ComponentModel;

namespace FamilyShow.MapModule
{
    public class FactsByBirthYearPersonContentPresenter : INotifyPropertyChanged
    {
        public FactsByBirthYearPersonContentPresenter(IEventAggregator eventAggregator)
        {
            eventAggregator.Get<PersonContextChangedEvent>().Subscribe(this.PersonChanged, ThreadOption.UIThread);
        }

        private void PersonChanged(Person person)
        {
            if (person != null && person.BirthDate != null)
            this.URL = new Uri(string.Format("http://wapedia.mobi/en/{0}", person.BirthDate.Value.Year));
            PropertyChanged(this, new PropertyChangedEventArgs("URL"));
        }

        public Uri URL { get; private set;}

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
