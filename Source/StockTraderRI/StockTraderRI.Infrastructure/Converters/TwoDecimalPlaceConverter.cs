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
using System.Windows.Data;
using System.Globalization;

namespace StockTraderRI.Infrastructure.Converters
{
    public class TwoDecimalPlaceConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int iNumDigits;

            if (value.GetType() != typeof(decimal))
            {
                throw new ArgumentException("TwoDecimalPlaceConverter only works with decimal values");
            }

            try{
                iNumDigits = int.Parse(parameter.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture);
            }
            catch{
                throw new ArgumentException("TwoDecimalPlaceConverter numDigits parameter should be convertable to an integer specifying number of digits to round to");
            }

            return Math.Round((decimal)value, iNumDigits);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
