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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Automation;
using Core;
using Core.UIItems.Finders;
using Core.UIItems;
using Core.UIItems.MenuItems;
using Core.UIItems.TabItems;
using Core.UIItems.WindowItems;
using StockTraderRI.AcceptanceTests.TestInfrastructure;
using StockTraderRI.AcceptanceTests.AutomatedTests;
using StockTraderRI.AcceptanceTests.Helpers;
using StockTraderRI.AcceptanceTests.TestInfrastructure.MockModels;
using System.IO;
using System.Globalization;

namespace StockTraderRI.AcceptanceTests.AutomatedTests.ModuleFixtures
{
    public partial class BuySellModuleFixture
    {
        /// <summary>
        /// Check if user can sell more than the number of shares he/she holds.
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a stock in Position Table and select Sell Option
        /// 3. Get number of shares for selected symbol and enter {no. of shares + 1} in the shares field
        /// 5. Click on "Submit" button
        /// 
        /// Expected Result:
        /// User should not sell the share more than the number of shares he holds.
        /// </summary>
        [TestMethod]
        public void AttemptSellMoreThanHeldNumberOfShares()
        {
            string symbol;
            string positionTableSymbolHeader = ConfigHandler.GetTestInputData("PositionTableSymbol");
            string positionTableSharesHeader = ConfigHandler.GetTestInputData("PositionTableShares");
            int symbolShare = 0;

            symbol = ConfigHandler.GetTestInputData("Symbol");
            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));
            
            symbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(symbol)).Cells[positionTableSharesHeader].Text);
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order symbolOrder = new Order(
                                        symbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        symbolShare + 1,
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(symbolOrder);

            Button submitButton = window.Get<Button>(ConfigHandler.GetControlId("BuySellSubmitButton"));
            Assert.IsFalse(submitButton.Enabled);
        }

        /// <summary>
        /// Check if user can sell multiple shares more than number of shares he holds.
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a stock in Position Table and Select Sell Option
        /// 3. Enter the data for Required fields and enter {number of shares + 1} in shares field
        /// 4. Repeat steps 2 & 3 for different stocks
        /// 5. Click on "Submit All" button
        /// 
        /// Expected Result:
        /// User should not sell the multiple shares more than the number of shares he holds.
        /// </summary>
        [TestMethod]
        public void AttemptSellMoreThanHeldNumberOfSharesForMultipleShares()
        {
            string symbol;
            string anotherSymbol;
            int symbolShare = 0;
            int anotherSymbolShare = 0;
            string positionTableSymbolHeader = ConfigHandler.GetTestInputData("PositionTableSymbol");
            string positionTableSharesHeader = ConfigHandler.GetTestInputData("PositionTableShares");
            
            symbol = ConfigHandler.GetTestInputData("Symbol");
            anotherSymbol = ConfigHandler.GetTestInputData("PositionSymbol");
            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));

            symbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(symbol)).Cells[positionTableSharesHeader].Text);
            anotherSymbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(anotherSymbol)).Cells[positionTableSharesHeader].Text);

            //Enter details for first symbol
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order symbolOrder = new Order(
                                        symbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        symbolShare + 1,
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(symbolOrder);
            ////////////////////////

            //if the Buy/Sell collapsible panel is not pinned, then loss focus of it so as to enable rest of the screen
            //to accept user input
            list.Hover();

            //Enter details for second symbol
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, anotherSymbol);
            Order anotherSymbolOrder = new Order(
                                        anotherSymbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        anotherSymbolShare + 1,
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(anotherSymbolOrder);
            ////////////////////////

            Button submitAllButton = window.Get<Button>(ConfigHandler.GetControlId("BuySellSubmitAllButton"));
            Assert.IsFalse(submitAllButton.Enabled);
        }

        /// <summary>
        /// Check if user can sell multiple shares more than number of shares he holds.
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a stock in Position Table and Select Sell Option
        /// 3. Enter the data for Required fields and enter {number of shares + 1} in shares field
        /// 4. Repeat steps 2 & 3 for different stocks and enter only held number of shares.
        /// 5. Click on "Submit All" button
        /// 
        /// Expected Result:
        /// User should not sell the multiple shares more than the number of shares he holds.
        /// </summary>
        [TestMethod]
        public void AttemptSellMoreThanHeldNumberOfSharesForOneShareAndHeldNumberOfSharesForAnotherUsingSubmitAll()
        {
            string symbol;
            string anotherSymbol;
            int symbolShare = 0;
            int anotherSymbolShare = 0;
            string positionTableSymbolHeader = ConfigHandler.GetTestInputData("PositionTableSymbol");
            string positionTableSharesHeader = ConfigHandler.GetTestInputData("PositionTableShares");

            symbol = ConfigHandler.GetTestInputData("Symbol");
            anotherSymbol = ConfigHandler.GetTestInputData("PositionSymbol");
            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));

            symbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(symbol)).Cells[positionTableSharesHeader].Text);
            anotherSymbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(anotherSymbol)).Cells[positionTableSharesHeader].Text);

            //Enter details for first symbol
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order symbolOrder = new Order(
                                        symbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        symbolShare + 1,
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(symbolOrder);
            ////////////////////////

            //if the Buy/Sell collapsible panel is not pinned, then loss focus of it so as to enable rest of the screen
            //to accept user input
            list.Hover();

            //Enter details for second symbol
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, anotherSymbol);
            Order anotherSymbolOrder = new Order(
                                        anotherSymbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        anotherSymbolShare,
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(anotherSymbolOrder);
            ////////////////////////

            Button submitAllButton = window.Get<Button>(ConfigHandler.GetControlId("BuySellSubmitAllButton"));
            Assert.IsFalse(submitAllButton.Enabled);
        }

        /// <summary>
        /// Check if user can sell multiple shares more than number of shares he holds.
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a stock in Position Table and Select Sell Option
        /// 3. Enter the data for Required fields and enter {number of shares + 1} in shares field
        /// 4. Repeat steps 2 & 3 for different stocks and enter only held number of shares.
        /// 5. Click on "Submit All" button
        /// 
        /// Expected Result:
        /// User should not sell the multiple shares more than the number of shares he holds.
        /// </summary>
        [TestMethod]
        public void AttemptSellMoreThanHeldNumberOfSharesForOneShareAndHeldNumberOfSharesForAnotherUsingSubmit()
        {
            string symbol;
            string anotherSymbol;
            int symbolShare = 0;
            int anotherSymbolShare = 0;
            string positionTableSymbolHeader = ConfigHandler.GetTestInputData("PositionTableSymbol");
            string positionTableSharesHeader = ConfigHandler.GetTestInputData("PositionTableShares");

            symbol = ConfigHandler.GetTestInputData("Symbol");
            anotherSymbol = ConfigHandler.GetTestInputData("PositionSymbol");
            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));

            symbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(symbol)).Cells[positionTableSharesHeader].Text);
            anotherSymbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(anotherSymbol)).Cells[positionTableSharesHeader].Text);

            //Enter details for first symbol
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order symbolOrder = new Order(
                                        symbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        symbolShare + 1,
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(symbolOrder);
            ////////////////////////

            //if the Buy/Sell collapsible panel is not pinned, then loss focus of it so as to enable rest of the screen
            //to accept user input
            list.Hover();

            //Enter details for second symbol
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, anotherSymbol);
            Order anotherSymbolOrder = new Order(
                                        anotherSymbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        anotherSymbolShare,
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(anotherSymbolOrder);
            ////////////////////////

            Tab buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Sell.ToString() + " " + symbol).AndControlType(typeof(Tab)));
            buySellSymbolTab.Click();
            Button submitButton = window.Get<Button>(ConfigHandler.GetControlId("BuySellSubmitButton"));
            Assert.IsFalse(submitButton.Enabled);

            buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Sell.ToString() + " " + anotherSymbol).AndControlType(typeof(Tab)));
            buySellSymbolTab.Click();
            submitButton = window.Get<Button>(ConfigHandler.GetControlId("BuySellSubmitButton"));
            Assert.IsTrue(submitButton.Enabled);
            submitButton.Click();

            //validate if the buy transaction was successful
            List<Order> orders = testDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(anotherSymbol)).Equals(anotherSymbolOrder));
        }

        /// <summary>
        /// Check if user can sell share across multiple sell requests.
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a stock in Position Table and Select Sell Option
        /// 3. Enter the data for Required fields with half the number of shares
        /// 4. Repeat steps 2 & 3
        /// 5. Click on "Submit All" button
        /// 
        /// Expected Result:
        /// User should sell share across multiple sell requests.
        /// </summary>
        [TestMethod]
        public void SellShareSpreadAcrossMultipleSellRequests()
        {
            string symbol;
            int symbolShare = 0;
            string positionTableSymbolHeader = ConfigHandler.GetTestInputData("PositionTableSymbol");
            string positionTableSharesHeader = ConfigHandler.GetTestInputData("PositionTableShares");

            symbol = ConfigHandler.GetTestInputData("Symbol");
            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));
            symbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(symbol)).Cells[positionTableSharesHeader].Text);

            //Enter symbol details with shares / 2 in first tab
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order sellSymbolOrder = new Order(
                                        symbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        Convert.ToInt32(symbolShare / 2),
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(sellSymbolOrder);
            ///////////////////////////////

            //if the Buy/Sell collapsible panel is not pinned, then loss focus of it so as to enable rest of the screen
            //to accept user input
            list.Hover();

            //Enter symbol details with shares / 2 in second tab
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            PopulateBuySellPanelWithData(sellSymbolOrder);
            ///////////////////////////////

            Button submitAllButton = window.Get<Button>(ConfigHandler.GetControlId("BuySellSubmitAllButton"));
            submitAllButton.Click();

            Tab buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Sell.ToString() + " " + symbol).AndControlType(typeof(Tab)));
            Assert.IsNull(buySellSymbolTab);

            //validate if the sell transaction was successful
            List<Order> orders = testDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsTrue(orders.FindAll(o => o.Equals(sellSymbolOrder)).Count.Equals(2));
        }

        /// <summary>
        /// Check if user can sell share across multiple sell requests.
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a stock in Position Table and Select Sell Option
        /// 3. Enter the data for Required fields with half the number of shares
        /// 4. Repeat steps 2 & 3 with step 3 changed to enter more than half number of shares
        /// 5. Click on "Submit All" button
        /// 
        /// Expected Result:
        /// User should sell share across multiple sell requests.
        /// </summary>
        [TestMethod]
        public void AttemptSellShareSpreadAcrossMultipleSellRequestsWithAllNumberOfSharesAddingToMoreThanHeldNumberOfShares()
        {
            string symbol;
            int symbolShare = 0;

            symbol = ConfigHandler.GetTestInputData("Symbol");
            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));
            symbolShare = (int)list.GetData(symbol, PositionTableColumnHeader.NumberOfShares);

            //Enter symbol details with shares / 2 in first tab
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order sellSymbolOrder = new Order(
                                        symbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        Convert.ToInt32(symbolShare / 2),
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(sellSymbolOrder);
            ///////////////////////////////

            //if the Buy/Sell collapsible panel is not pinned, then loss focus of it so as to enable rest of the screen
            //to accept user input
            list.Hover();

            //Enter symbol details with shares / 2 in second tab
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order sellSymbolAnotherOrder = new Order(
                                        symbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        Convert.ToInt32(symbolShare / 2) + 1,
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(sellSymbolAnotherOrder);
            ///////////////////////////////

            Button submitAllButton = window.Get<Button>(ConfigHandler.GetControlId("BuySellSubmitAllButton"));
            submitAllButton.Click();

            Tab buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Sell.ToString() + " " + symbol).AndControlType(typeof(Tab)));
            Assert.IsNotNull(buySellSymbolTab);

            //validate if the sell transaction was successful
            List<Order> orders = testDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsNotNull(orders.Find(o => o.Equals(sellSymbolOrder)));
            Assert.IsNull(orders.Find(o => o.Equals(sellSymbolAnotherOrder)));
        }

        /// <summary>
        /// Check if user can buy share across multiple buy requests.
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a stock in Position Table and Select buy Option
        /// 4. Enter the data for Required fields.
        /// 4. Repeat steps 2 & 3 for the same stock
        /// 5. Click on "Submit All" button
        /// 
        /// Expected Result:
        /// User should buy share across multiple buy requests.
        /// </summary>
        [TestMethod]
        public void BuyShareSpreadAcrossMultipleBuyRequests()
        {
            string symbol;
            symbol = ConfigHandler.GetTestInputData("Symbol");

            //Enter details for buying share in one tab
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, symbol);
            Order buySymbolOrder = new Order(
                                        symbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        int.Parse(ConfigHandler.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(buySymbolOrder);
            ///////////////////////////////

            //if the Buy/Sell collapsible panel is not pinned, then loss focus of it so as to enable rest of the screen
            //to accept user input
            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));
            list.Hover();

            //Enter details for buying same share in second tab
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, symbol);
            PopulateBuySellPanelWithData(buySymbolOrder);
            ///////////////////////////////

            Button submitAllButton = window.Get<Button>(ConfigHandler.GetControlId("BuySellSubmitAllButton"));
            submitAllButton.Click();

            Tab buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Buy.ToString() + " " + symbol).AndControlType(typeof(Tab)));
            Assert.IsNull(buySellSymbolTab);

            //validate if the sell transaction was successful
            List<Order> orders = testDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsTrue(orders.FindAll(o => o.Equals(buySymbolOrder)).Count.Equals(2));
        }

        /// <summary>
        /// Check if user can buy the share by launching buy/sell panel with sell request.
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on the selected stock in Position Table and Select sell Option
        /// 3. Enter the data for Required fields and select buy option
        /// 4. Click on "Submit" button
        /// 
        /// Expected Result:
        /// User should buy the share.
        /// </summary>
        [TestMethod]
        public void LaunchBuySellPanelForSellFromPositionTableAndBuyTheShare()
        {
            string symbol;
            symbol = ConfigHandler.GetTestInputData("Symbol");

            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order model = new Order(
                                        symbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        int.Parse(ConfigHandler.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(model);

            Button submitButton = window.Get<Button>(ConfigHandler.GetControlId("BuySellSubmitButton"));
            submitButton.Click();

            //give time for submit processing
            System.Threading.Thread.Sleep(2000);

            // Validate if selected tab in Buy/Sell Panel disappears.
            // AND
            // Validate if the buy transactions were successful
            Tab buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Sell.ToString() + " " + symbol).AndControlType(typeof(Tab)));
            Assert.IsNull(buySellSymbolTab);

            //validate if the buy transaction was successful
            List<Order> orders = testDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(symbol)).Equals(model));
        }

        /// <summary>
        /// Check if user can buy and sell the shares for invalid symbol
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Right-Click on the selected stock in Position Table and Select buy Option
        /// 3. Enter the data for Required fields with invalid symbol
        /// 4  Right-Click on the selected stock in Position Table and Select sell Option
        /// 5. Enter the data for Required fields with invalid symbol
        /// 6. Check for "Submit All" button disable. 
        /// 
        ///
        /// Expected Result:
        /// User should not buy and sell the shares for invalid symbol.
        /// "Submit All" button should be disable.
        /// </summary>
        [TestMethod]
        public void AttemptInvalidBuyAndInvalidSellSimultaneouslyWithMultipleRequests()
        {
            string buySymbol;
            string sellSymbol;
            string invalidSymbol;

            buySymbol = ConfigHandler.GetTestInputData("Symbol");
            sellSymbol = ConfigHandler.GetTestInputData("PositionSymbol3");
            invalidSymbol = ConfigHandler.GetTestInputData("InvalidSymbol");

            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, buySymbol);
            Order buyModel = new Order(
                                        invalidSymbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        int.Parse(ConfigHandler.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(buyModel);

            //if the Buy/Sell collapsible panel is not pinned, then loss focus of it so as to enable rest of the screen
            //to accept user input
            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));
            list.Hover();

            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, sellSymbol);
            Order sellModel = new Order(
                                        invalidSymbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        int.Parse(ConfigHandler.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(sellModel);

            Button submitAllButton = window.Get<Button>(ConfigHandler.GetControlId("BuySellSubmitAllButton"));
            Assert.IsFalse(submitAllButton.Enabled);
        }

        /// <summary>
        /// Check if user can sell the stock when buy/sell panel contains invalid buy and valid sell data
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Get the number of shares for a symbol.
        /// 3. Right-Click on the selected stock in Position Table and Select buy Option
        /// 4  Enter the data for Required fields with invalid buy symbol
        /// 5. Right-Click on the selected stock in Position Table and Select sell Option
        /// 6. Enter the data for Required fields with number of shares for a symbol.
        /// 7. Click on "Submit All"
        ///
        /// 
        ///
        /// Expected Result:
        /// User should sell the shares.
        /// User should not buy the shares for invalid symbol.
        /// </summary>
        [TestMethod]
        public void AttemptInvalidBuyAndValidSellSimultaneouslyWithMultipleRequests()
        {
            string buySymbol;
            string sellSymbol;
            string invalidBuySymbol;
            int sellSymbolShare = 0;
            string positionTableSymbolHeader = ConfigHandler.GetTestInputData("PositionTableSymbol");
            string positionTableSharesHeader = ConfigHandler.GetTestInputData("PositionTableShares");

            buySymbol = ConfigHandler.GetTestInputData("Symbol");
            sellSymbol = ConfigHandler.GetTestInputData("PositionSymbol1");
            invalidBuySymbol = ConfigHandler.GetTestInputData("InvalidSymbol");

            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));
            sellSymbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(sellSymbol)).Cells[positionTableSharesHeader].Text);

            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, buySymbol);
            Order buyModel = new Order(
                                        invalidBuySymbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        int.Parse(ConfigHandler.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(buyModel);

            //if the Buy/Sell collapsible panel is not pinned, then loss focus of it so as to enable rest of the screen
            //to accept user input
            list.Hover();

            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, sellSymbol);
            Order sellModel = new Order(
                                        sellSymbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        sellSymbolShare,
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(sellModel);

            Button submitAllButton = window.Get<Button>(ConfigHandler.GetControlId("BuySellSubmitAllButton"));
            submitAllButton.Click();

            Tab buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Buy.ToString() + " " + buySymbol).AndControlType(typeof(Tab)));
            Assert.IsNotNull(buySellSymbolTab);

            buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Sell.ToString() + " " + sellSymbol).AndControlType(typeof(Tab)));
            Assert.IsNull(buySellSymbolTab);

            //validate the transaction was successful
            List<Order> orders = testDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsNull(orders.Find(o => o.Symbol.Equals(buySymbol)));
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(sellSymbol)).Equals(sellModel));
        }

        /// <summary>
        /// Check if user can buy the stock when buy/sell panel contains valid buy and invalid sell data
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Right-Click on the selected stock in Position Table and Select buy Option
        /// 3  Enter the data for Required fields 
        /// 4. Right-Click on the selected stock in Position Table and Select sell Option
        /// 5. Enter the data for Required fields with invalid sell symbol
        /// 6. check for submit All button disable.
        ///
        /// 
        ///
        /// Expected Result:
        /// User should not buy the shares.
        /// "Submit All" button should be disable
        /// </summary>
        [TestMethod]
        public void AttemptValidBuyAndInvalidSellSimultaneouslyWithMultipleRequests()
        {
            string buySymbol;
            string sellSymbol;
            string invalidSellSymbol;

            buySymbol = ConfigHandler.GetTestInputData("Symbol");
            sellSymbol = ConfigHandler.GetTestInputData("PositionSymbol1");
            invalidSellSymbol = ConfigHandler.GetTestInputData("InvalidSymbol");

            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, buySymbol);
            Order buyModel = new Order(
                                        buySymbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        int.Parse(ConfigHandler.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(buyModel);

            //if the Buy/Sell collapsible panel is not pinned, then loss focus of it so as to enable rest of the screen
            //to accept user input
            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));
            list.Hover();

            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, sellSymbol);
            Order sellModel = new Order(
                                        invalidSellSymbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        int.Parse(ConfigHandler.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                        );
            PopulateBuySellPanelWithData(sellModel);

            Button submitAllButton = window.Get<Button>(ConfigHandler.GetControlId("BuySellSubmitAllButton"));
            Assert.IsFalse(submitAllButton.Enabled);
        }

        /// <summary>
        /// Check if user can buy the multi selected symbols shares.
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Right-Click on the selected stocks in Position Table and Select buy Option
        /// 3. Enter the data for Required fields for symbol.
        /// 4. Get the handle of another symbol's buy/sell panel for buy 
        /// 5. Enter the data for Required fields for symbol.
        /// 6. Click on "Submit All" button
        /// 
        ///
        /// Expected Result:
        /// User should buy the multi-selected symbol shares.
        ///
        /// </summary>
        [TestMethod]
        public void MultiSelectSharesFromPositionTableAndBuyAll()
        {
            string symbol;
            string anotherSymbol;
            string positionTableSymbolHeader = ConfigHandler.GetTestInputData("PositionTableSymbol");

            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));

            symbol = list.Rows[0].Cells[positionTableSymbolHeader].Text;
            anotherSymbol = list.Rows[1].Cells[positionTableSymbolHeader].Text;

            //handle multiple row selection in watchlist and open buysell panel for buy
            list.MultiSelect(0);
            list.MultiSelect(1);
            list.Rows[0].RightClick();

            System.Threading.Thread.Sleep(1000);
            window.PopupMenu("Buy").Click();

            //check for first tab
            Tab buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Buy.ToString() + " " + symbol).AndControlType(typeof(Tab)));
            Assert.IsNotNull(buySellSymbolTab);

            //check for second tab
            buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Buy.ToString() + " " + anotherSymbol).AndControlType(typeof(Tab)));
            Assert.IsNotNull(buySellSymbolTab);
        }

        /// <summary>
        /// Check if user can sell the multi selected symbols shares.
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Get the number of shares for a symbols.
        /// 3. Right-Click on the selected stocks in Position Table and Select sell Option
        /// 4. Enter the data for Required fields along with number of shares for symbol.
        /// 5. Get the handle of another symbol's buy/sell panel for sell 
        /// 6. Enter the data for Required fields along with number of shares for symbol.
        /// 7. Click on "Submit All" button
        ///
        /// Expected Result:
        /// User should sell the multi-selected symbol shares.
        ///
        /// </summary>
        /// //TODO: handle the selection of multiple row in position table
        [TestMethod]
        public void MultiSelectSharesFromPositionTableAndSellAll()
        {
            string symbol;
            string anotherSymbol;
            string positionTableSymbolHeader = ConfigHandler.GetTestInputData("PositionTableSymbol");

            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));

            symbol = list.Rows[0].Cells[positionTableSymbolHeader].Text;
            anotherSymbol = list.Rows[1].Cells[positionTableSymbolHeader].Text;

            list.MultiSelect(0);
            list.MultiSelect(1);
            list.Rows[0].RightClick();

            System.Threading.Thread.Sleep(1000);
            window.PopupMenu("Sell").Click();

            //check first tab
            Tab buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Sell.ToString() + " " + symbol).AndControlType(typeof(Tab)));
            Assert.IsNotNull(buySellSymbolTab);

            //check second tab
            buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Sell.ToString() + " " + anotherSymbol).AndControlType(typeof(Tab)));
            Assert.IsNotNull(buySellSymbolTab);
        }

        /// <summary>
        /// Check if user can sell and buy the same share simultaneously
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Get the number of shares for a symbol
        /// 3. Right-Click on the selected stock in Position Table and Select sell Option
        /// 4. Enter the data for Required fields with number of shares for a symbol 
        /// 5. Right-Click on the same selected stock in Position Table and Select buy Option
        /// 6. Enter the data for Required fields
        /// 5. Click on "Submit All" button
        ///
        /// 
        /// Expected Result:
        /// User should buy and sell same share simultaneously
        ///
        /// </summary>
        [TestMethod]
        public void BuyAndSellSameShareSimultaneouslyWithMultipleRequests()
        {
            string symbol;
            int symbolShare = 0;

            string positionTableSymbolHeader = ConfigHandler.GetTestInputData("PositionTableSymbol");
            string positionTableSharesHeader = ConfigHandler.GetTestInputData("PositionTableShares");

            symbol = ConfigHandler.GetTestInputData("Symbol");

            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));
            symbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(symbol)).Cells[positionTableSharesHeader].Text);

            //Buy the share
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, symbol);
            Order buyModel = new Order(
                                        symbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        symbolShare,
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(buyModel);
            ///////////////////////////////
            
            //if the Buy/Sell collapsible panel is not pinned, then loss focus of it so as to enable rest of the screen
            //to accept user input
            list.Hover();

            //Sell the share
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);
            Order sellModel = new Order(
                                        symbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        symbolShare,
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(sellModel);
            ///////////////////////////////

            Button submitAllButton = window.Get<Button>(ConfigHandler.GetControlId("BuySellSubmitAllButton"));
            submitAllButton.Click();

            Tab buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Sell.ToString() + " " + symbol).AndControlType(typeof(Tab)));
            Assert.IsNull(buySellSymbolTab);

            buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Buy.ToString() + " " + symbol).AndControlType(typeof(Tab)));
            Assert.IsNull(buySellSymbolTab);

            //validate if the sell and buy transaction was successful
            List<Order> orders = testDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsNotNull(orders.Find(o => o.Equals(buyModel)));
            Assert.IsNotNull(orders.Find(o => o.Equals(sellModel)));
        }

        /// <summary>
        /// Check if user can sell and buy different shares across multiple requests.
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Get the number of shares for a symbols.
        /// 3. Right-Click on the selected stock in Position Table and Select Sell Option
        /// 4. Enter the data for Required fields with symbol's number of shares.
        /// 5. Repeat steps 3 & 4 for different stock
        /// 6. Right-Click on the selected stock in Position Table and Select buy Option
        /// 7. Enter the data for Required fields.
        /// 8. Repeat steps 6 & 7 for different stock.
        /// 9. Click on "Submit All" button
        /// 
        /// 
        /// Expected Result:
        /// User should sell and buy different shares simultaneously with multiple buy/sell requests.
        ///
        [TestMethod]
        public void BuyAndSellDifferentSharesSimultaneouslyWithMultipleRequests()
        {
            string sellSymbol;
            string sellAnotherSymbol;
            string buySymbol;
            string buyAnotherSymbol;
            int symbolShare = 0;
            int anotherSymbolShare = 0;
            string positionTableSymbolHeader = ConfigHandler.GetTestInputData("PositionTableSymbol");
            string positionTableSharesHeader = ConfigHandler.GetTestInputData("PositionTableShares");

            sellSymbol = ConfigHandler.GetTestInputData("Symbol");
            sellAnotherSymbol = ConfigHandler.GetTestInputData("PositionSymbol");
            buySymbol = ConfigHandler.GetTestInputData("PositionSymbol1");
            buyAnotherSymbol = ConfigHandler.GetTestInputData("PositionSymbol3");

            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));
            symbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(sellSymbol)).Cells[positionTableSharesHeader].Text);
            anotherSymbolShare = Convert.ToInt32(list.Rows.Find(r => r.Cells[positionTableSymbolHeader].Text.Equals(sellAnotherSymbol)).Cells[positionTableSharesHeader].Text);

            //Sell a share
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, sellSymbol);
            Order sellModel = new Order(
                                        sellSymbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        symbolShare,
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(sellModel);
            //////////////////////////////////////////////

            //if the Buy/Sell collapsible panel is not pinned, then loss focus of it so as to enable rest of the screen
            //to accept user input
            list.Hover();

            //Sell another share
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, sellAnotherSymbol);
            Order anotherSellModel = new Order(
                                        sellAnotherSymbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        anotherSymbolShare,
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(anotherSellModel);
            /////////////////////////////////////////////////////////////

            //if the Buy/Sell collapsible panel is not pinned, then loss focus of it so as to enable rest of the screen
            //to accept user input
            list.Hover();

            //Buy a share
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, buySymbol);
            Order buyModel = new Order(
                                         buySymbol,
                                         Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                         ConfigHandler.GetTestInputData("BuySellOrderType"),
                                         int.Parse(ConfigHandler.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                         ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                         BuySellEnum.Buy.ToString()
                                     );
            PopulateBuySellPanelWithData(buyModel);
            /////////////////////////////////////////////////////////////

            //if the Buy/Sell collapsible panel is not pinned, then loss focus of it so as to enable rest of the screen
            //to accept user input
            list.Hover();

            //Buy another share
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Buy, buyAnotherSymbol);
            Order anotherBuyModel = new Order(
                                        buyAnotherSymbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        int.Parse(ConfigHandler.GetTestInputData("BuySellNumberOfShares"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Buy.ToString()
                                    );
            PopulateBuySellPanelWithData(anotherBuyModel);
            /////////////////////////////////////////////////////////////

            Button submitAllButton = window.Get<Button>(ConfigHandler.GetControlId("BuySellSubmitAllButton"));
            submitAllButton.Click();

            Tab buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Sell.ToString() + " " + sellSymbol).AndControlType(typeof(Tab)));
            Assert.IsNull(buySellSymbolTab);

            buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Sell.ToString() + " " + sellAnotherSymbol).AndControlType(typeof(Tab)));
            Assert.IsNull(buySellSymbolTab);

            buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Buy.ToString() + " " + buySymbol).AndControlType(typeof(Tab)));
            Assert.IsNull(buySellSymbolTab);

            buySellSymbolTab = window.Get<Tab>(
                SearchCriteria.ByText(BuySellEnum.Buy.ToString() + " " + buyAnotherSymbol).AndControlType(typeof(Tab)));
            Assert.IsNull(buySellSymbolTab);

            //validate if the sell transaction was successful
            List<Order> orders = testDataInfrastructure.GetData<OrderDataProvider, Order>();
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(sellSymbol)).Equals(sellModel));
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(sellAnotherSymbol)).Equals(anotherSellModel));
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(buySymbol)).Equals(buyModel));
            Assert.IsTrue(orders.Find(o => o.Symbol.Equals(buyAnotherSymbol)).Equals(anotherBuyModel));
        }

        /// <summary>
        /// Check if overwriting valid number of shares with an invalid string disables the "Submit" button
        /// 
        /// Repro Steps:
        /// 1. Lauch the StockTraderRI
        /// 2. Right-Click on a stock in Position Table and Select Sell Option
        /// 3. Enter the data for Required fields
        /// 4. Overwrite the number of shares with an invalid string
        /// 5. Check if the "Submit" button is enabled
        /// 
        /// Expected Result:
        /// the "Submit" button should be disabled
        /// </summary>
        [TestMethod]
        public void AttemptSellStockByEnteringValidValuesFirstAndThenOverwritingWithInvalidValues()
        {
            string symbol;
            int numberOfShares = 0;

            symbol = ConfigHandler.GetTestInputData("Symbol");
            LaunchBuySellPanelFromPositionTable(BuySellEnum.Sell, symbol);

            ListView list = window.Get<ListView>(ConfigHandler.GetControlId("PositionTableId"));
            numberOfShares = (int)list.GetData(symbol, PositionTableColumnHeader.NumberOfShares);

            Order model = new Order(
                                        symbol,
                                        Decimal.Parse(ConfigHandler.GetTestInputData("BuySellLimitPrice"), CultureInfo.InvariantCulture),
                                        ConfigHandler.GetTestInputData("BuySellOrderType"),
                                        numberOfShares,
                                        ConfigHandler.GetTestInputData("TimeInForceEndOfDay"),
                                        BuySellEnum.Sell.ToString()
                                    );
            PopulateBuySellPanelWithData(model);

            //overwrite invalid "number of shares" value
            //select the collapsible panel first
            UIItem buySellListTab = window.GetCollapsibleRegionHeader("BuySellListHeader");
            buySellListTab.Click();
            //overwrite
            TextBox shareTextBox = window.Get<TextBox>(ConfigHandler.GetControlId("BuySellSharesTextBox"));
            shareTextBox.Text = ConfigHandler.GetTestInputData("InvalidString");

            //check if the submit button is disabled
            Button submitButton = window.Get<Button>(ConfigHandler.GetControlId("BuySellSubmitButton"));
            Assert.IsFalse(submitButton.Enabled);
        }
    }
}
