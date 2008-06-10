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

namespace FamilyShow.MapModule
{
    public class BirthdayCountdownService : IPersonContentService
    {
        public object GetContent(Person person)
        {
            if (person == null)
                return null;

            if (person.BirthDate.HasValue == false)
                return null;

            DateTime birthdayThisYear = new DateTime(DateTime.Now.Year,person.BirthDate.Value.Month,person.BirthDate.Value.Day);
            TimeSpan timeTillBDay = birthdayThisYear.Subtract(DateTime.Now);

            if (timeTillBDay.Days < 0)
            {
                birthdayThisYear = new DateTime(DateTime.Now.Year+1, person.BirthDate.Value.Month, person.BirthDate.Value.Day);
                timeTillBDay = birthdayThisYear.Subtract(DateTime.Now);
            }
            if(timeTillBDay.Hours > 0)
            {
                timeTillBDay = timeTillBDay.Add(new TimeSpan(1, 0, 0, 0));
            }
            
            //TimeSpan timeTillBDay = person.BirthDate.Value.Subtract(DateTime.Now);
            
            return string.Format("{0} day(s) till birthday", timeTillBDay.Days);
        }
    }
}
