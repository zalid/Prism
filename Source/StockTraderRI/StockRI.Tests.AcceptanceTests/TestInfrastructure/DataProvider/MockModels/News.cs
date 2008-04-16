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
using System.Xml.Serialization;

namespace StockTraderRI.AcceptanceTests.TestInfrastructure.MockModels
{
    [Serializable]
    [XmlRoot("NewsItem")]
    public class News
    {
        private string symbol;
        private string iconUriPath;
        private string text;

        public News() { }

        public News(string symbol)
        {
            this.symbol = symbol;
        }

        public News(string symbol, string text)
        {
            this.symbol = symbol;
            this.text = text;
        }

        public News(string symbol, string iconUriPath, string text)
        {
            this.symbol = symbol;
            this.iconUriPath = iconUriPath;
            this.text = text;
        }

        [XmlAttribute("TickerSymbol")]
        public string TickerSymbol
        {
            get { return this.symbol; }
            set { this.symbol = value; }
        }

        [XmlAttribute("IconUri")]
        public string IconUriPath
        {
            get { return this.iconUriPath; }
            set { this.iconUriPath = value; }
        }

        [XmlElement("NewsItem")]
        public string NewsItemText
        {
            get { return this.text; }
            set { this.text = value; }
        }
    }
}
