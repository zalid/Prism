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
using StockTraderRI.AcceptanceTests.TestInfrastructure.MockModels;
using StockTraderRI.AcceptanceTests.Helpers;
using System.Data;

namespace StockTraderRI.AcceptanceTests.TestInfrastructure
{
    public class MarketHistoryDataProvider : DataProviderBase<MarketHistoryItem>
    {
        public MarketHistoryDataProvider()
            : base()
        { }

        public override string GetDataFilePath()
        {
            return ConfigHandler.GetValue("MarketHistoryDataFile");
        }

        public override List<MarketHistoryItem> GetData()
        {
            DataSet ds = new DataSet();
            ds.ReadXml(GetDataFilePath());
            DataRow dr = null;

            List<MarketHistoryItem> history = new List<MarketHistoryItem>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dr = ds.Tables[0].Rows[i];
                history.Add(
                    new MarketHistoryItem(
                        dr[ConfigHandler.GetTestInputData("TickerSymbol")].ToString(),
                        DateTime.Parse(dr[ConfigHandler.GetTestInputData("Date")].ToString()),
                        decimal.Parse(dr[2].ToString())
                        ));
            }

            return history;
        }
    }
}
