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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.UIItems;
using StockTraderRI.AcceptanceTests.TestInfrastructure.MockModels;
using Core.UIItems.ListViewItems;
using Core.UIItems.Finders;
using StockTraderRI.AcceptanceTests.TestInfrastructure;
using StockTraderRI.AcceptanceTests.Helpers;

namespace StockTraderRI.AcceptanceTests.AutomatedTests
{
    [TestClass]
    [DeploymentItem(@".\StockTraderRI\bin\Debug")]
    [DeploymentItem(@".\StockRI.Tests.AcceptanceTests\bin\Debug")]
    public class PositionModuleFixture : FixtureBase
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            base.TestInitialize();
        }

        /// <summary>
        /// TestCleanup performs clean-up activities after each test method execution
        /// </summary>
        [TestCleanup()]
        public void MyTestCleanup()
        {
            base.TestCleanup();
        }

        /// <summary>
        /// The current account position view should have symbol details with 6 columns 
        /// (Symbol, Shares, Last, Cost Basis, Market Value, Gain Loss %) and a News button column
        /// 
        /// Repro Steps:
        /// 1. Launch the Stock Trader application
        /// 2. Check for the following columns: 
        /// (Symbol, Shares, Last, Cost Basis, Market Value, Gain Loss %) and a News button column
        /// 
        /// Expected Result:
        /// Account Position Table should have 7 columns and the column names should be as follows:
        /// Symbol, Shares, Last, Cost Basis, Market Value, Gain Loss %, News
        /// </summary>
        [TestMethod]
        public void AccountPositionTableColumns()
        {
            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));
            ListViewHeader listHeader = list.Header;

            Assert.AreEqual(7, listHeader.Columns.Count);

            //The Columns in the Position Table are partly coming from two different XML files and 
            //the rest are computed columns. the only place where the column names are defined is the 
            //PositionView XAML file, hence the hard-coding.

            Assert.AreEqual(ConfigHandler.GetTestInputData("PositionTableSymbol"), listHeader.Columns[0].Name);
            Assert.AreEqual(ConfigHandler.GetTestInputData("PositionTableShares"), listHeader.Columns[1].Name);
            Assert.AreEqual(ConfigHandler.GetTestInputData("PositionTableLast"), listHeader.Columns[2].Name);
            Assert.AreEqual(ConfigHandler.GetTestInputData("PositionTableCost"), listHeader.Columns[3].Name);
            Assert.AreEqual(ConfigHandler.GetTestInputData("PositionTableMarketValue"), listHeader.Columns[4].Name);
            Assert.AreEqual(ConfigHandler.GetTestInputData("PositionTableGainLoss"), listHeader.Columns[5].Name);
            Assert.AreEqual(ConfigHandler.GetTestInputData("PositionTableHeaderNews"), listHeader.Columns[6].Name);
        }

        /// <summary>
        /// The current account position view should display details of symbols from the AccountPosition.xml
        /// 
        /// Repro Steps:
        /// 1. Launch the Stock Trader application
        /// 2. Check the number of symbols displayed in the account position table.
        /// 
        /// Expected Result:
        /// As many rows as present in AccountPosition.xml is displayed.
        /// </summary>
        [TestMethod]
        public void AccountPositionTableRowCount()
        {
            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));
            //read number of account positions from the AccountPosition.xml data file
            int positionRowCount = testDataInfrastructure.GetCount<AccountPositionDataProvider, AccountPosition>();

            Assert.AreEqual(positionRowCount, list.Rows.Count);
        }

        /// <summary>
        /// The current account position view should display derived data of symbols 
        /// (after processing data from AccountPosition.xml and Market.xml)
        /// 
        /// Repro Steps:
        /// 1. Launch the Stock Trader application
        /// 2. Check the derived data (like Market Value and Gain Loss %) of symbols displayed in the account position table.
        /// 
        /// Expected Result:
        /// Market Value = shares * currentPrice
        /// Gain Loss % = (CurrentPrice*Shares - CostBasis) * 100 / CostBasis
        /// </summary>
        [TestMethod]
        public void AccountPositionDerivedData()
        {
            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));
            string symbol;
            AccountPosition p;
            Market m;
            List<AccountPosition> position = testDataInfrastructure.GetData<AccountPositionDataProvider, AccountPosition>();
            List<Market> market = testDataInfrastructure.GetData<MarketDataProvider, Market>();

            //test driven by the number of rows displayed in the Account Position table in the UI
            for (int i = 0; i < list.Rows.Count; i++)
            {
                symbol = list.Rows[i].Cells[ConfigHandler.GetTestInputData("PositionTableSymbol")].Text;
                p = position.Find(ap => ap.TickerSymbol.Equals(symbol));
                m = market.Find(ma => ma.TickerSymbol.Equals(symbol));

                Assert.AreEqual(list.Rows[i].Cells[ConfigHandler.GetTestInputData("PositionTableMarketValue")].Text, (p.Shares * m.LastPrice).ToString());
                Assert.AreEqual(list.Rows[i].Cells[ConfigHandler.GetTestInputData("PositionTableGainLoss")].Text, Math.Round((m.LastPrice * p.Shares - p.CostBasis) / p.CostBasis * 100, 2).ToString());
            }
        }
    }
}
