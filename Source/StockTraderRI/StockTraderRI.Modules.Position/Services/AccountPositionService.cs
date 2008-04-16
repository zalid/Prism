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
using StockTraderRI.Infrastructure.Interfaces;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;
using StockTraderRI.Infrastructure.Models;

namespace StockTraderRI.Modules.Position.Services
{
    public class AccountPositionService : IAccountPositionService
    {
        List<AccountPosition> _positions = new List<AccountPosition>();

        public AccountPositionService()
        {
            InitializePositions();
        }

        #region IAccountPositionService Members

        public event EventHandler<AccountPositionModelEventArgs> Updated = delegate { };

        public IList<AccountPosition> GetAccountPositions()
        {
            return _positions;
        }
        #endregion

        private void InitializePositions()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<AccountPosition>));
            XmlTextReader xmlReader = new XmlTextReader("Data/AccountPositions.xml");
            try
            {
                _positions = (List<AccountPosition>)xmlSerializer.Deserialize(xmlReader);
            }
            catch { }
        }

    }
}
