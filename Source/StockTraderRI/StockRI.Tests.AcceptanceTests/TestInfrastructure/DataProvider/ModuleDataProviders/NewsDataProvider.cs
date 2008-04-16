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
using StockTraderRI.AcceptanceTests.TestInfrastructure.MockModels;
using System.Data;
using StockTraderRI.AcceptanceTests.Helpers;

namespace StockTraderRI.AcceptanceTests.TestInfrastructure.DataProvider.ModuleDataProviders
{
    public class NewsDataProvider: DataProviderBase<News>
    {
        public NewsDataProvider()
            : base()
        { }

        public override string GetDataFilePath()
        {
            return ConfigHandler.GetValue("NewsDataFile");
        }

        public override List<News> GetData()
        {
            DataSet ds = new DataSet();
            ds.ReadXml(GetDataFilePath());
            DataRow dr = null;

            List<News> news = new List<News>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dr = ds.Tables[0].Rows[i];
                news.Add(
                    new News(
                            dr[ConfigHandler.GetTestInputData("TickerSymbol")].ToString(), 
                            dr[ConfigHandler.GetTestInputData("IconUri")].ToString(),
                            dr[2].ToString()
                            )
                        );
            }

            return news;
        }
    }
}
