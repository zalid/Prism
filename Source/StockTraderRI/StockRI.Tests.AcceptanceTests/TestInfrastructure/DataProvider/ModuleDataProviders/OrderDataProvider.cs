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
using System.Data;
using System.Collections.Generic;
using StockTraderRI.AcceptanceTests.Helpers;
using System.IO;


namespace StockTraderRI.AcceptanceTests.TestInfrastructure
{
    public class OrderDataProvider : DataProviderBase<Order>
    {
        public OrderDataProvider()
            : base()
        { }

        public override string GetDataFilePath()
        {
            return ConfigHandler.GetValue("OrderProcessingFile");
        }

        public override List<Order> GetData()
        {
            List<Order> order = new List<Order>();
            string filepath = GetDataFilePath();

            if (File.Exists(filepath))
            {
                DataSet ds = new DataSet();

                ds.ReadXml(filepath);
                DataRow dr = null;


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dr = ds.Tables[0].Rows[i];
                    order.Add(
                        new Order(dr["TickerSymbol"].ToString(),
                        decimal.Parse(dr["StopLimitPrice"].ToString()),
                        dr["OrderType"].ToString(),
                        int.Parse(dr["Shares"].ToString()),
                        dr["TimeInForce"].ToString(),
                        dr["TransactionType"].ToString())
                        );
                }
            }
            return order;
        }
    }
}
