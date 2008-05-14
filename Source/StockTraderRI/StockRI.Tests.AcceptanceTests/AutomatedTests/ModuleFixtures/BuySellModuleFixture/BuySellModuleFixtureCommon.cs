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
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Automation;
using Core;
using Core.UIItems;
using Core.UIItems.Finders;
using Core.UIItems.ListBoxItems;
using Core.UIItems.MenuItems;
using Core.UIItems.TabItems;
using Core.UIItems.WindowItems;
using StockTraderRI.AcceptanceTests.TestInfrastructure;
using StockTraderRI.AcceptanceTests.AutomatedTests;
using StockTraderRI.AcceptanceTests.Helpers;
using StockTraderRI.AcceptanceTests.TestInfrastructure.MockModels;
using StockTraderRI.AcceptanceTests.ApplicationObserver;


namespace StockTraderRI.AcceptanceTests.AutomatedTests.ModuleFixtures
{
    [TestClass]
    [DeploymentItem(@".\StockTraderRI\bin\Debug")]
    [DeploymentItem(@".\StockRI.Tests.AcceptanceTests\bin\Debug")]   
    public partial class BuySellModuleFixture : FixtureBase
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            // Check whether any exception occured during previous application launches. 
            // If so, fail the test case.
            if (StateDiagnosis.IsFailed)
            {
                Assert.Fail(TestDataInfrastructure.GetTestInputData("ApplicationLoadFailure"));
            }

            base.TestInitialize();

            // Delete the Submitted orders
            string filePath = ConfigHandler.GetValue("OrderProcessingFile");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// TestCleanup performs clean-up activities after each test method execution
        /// </summary>
        [TestCleanup()]
        public void MyTestCleanup()
        {
            base.TestCleanup();
        }

        #region Private Helper methods

        private void AddSymbolToWatchList(string symbol)
        {
            //Button pinImage = window.Get<Button>("HeaderAutoHideButton");
            //pinImage.Click();

            TextBox addWatchTextBox = window.Get<TextBox>(TestDataInfrastructure.GetControlId("WatchListAddTextBox"));
            addWatchTextBox.Text = symbol;

            Button addWatchListButton = window.Get<Button>(TestDataInfrastructure.GetControlId("WatchListAddButton"));
            addWatchListButton.Click();
        }

        private void LaunchBuySellPanelFromPositionTable(BuySellEnum buySell, string symbol)
        {
            ListView list = window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));

            switch (buySell)
            {
                case BuySellEnum.Buy:
                    //right click on the added symbol in the Position Table and click Buy
                    list.Rows.Find(r => r.Cells[0].Text.Equals(symbol)).RightClick();
                    System.Threading.Thread.Sleep(2000);
                    window.PopupMenu("Buy").Click();
                    break;
                case BuySellEnum.Sell:
                    //TODO: validate if Symbol can be sold

                    //right click on the added symbol in the Position Table and click sell
                    list.Rows.Find(r => r.Cells[0].Text.Equals(symbol)).RightClick();
                    System.Threading.Thread.Sleep(2000);
                    window.PopupMenu("Sell").Click();
                    break;
            }
        }

        //validate if the screen has all the required controls and default values
        private void ValidateBuySellPanelControls(BuySellEnum buySell)
        {
            //select the collapsible panel first
            UIItem buySellListTab = window.GetCollapsibleRegionHeader("BuySellListHeader");
            buySellListTab.Hover();

            TextBox symbolTextBox = window.Get<TextBox>(TestDataInfrastructure.GetControlId("BuySellSymbolTextBox"));
            Assert.IsNotNull(symbolTextBox);

            RadioButton buyRadButton = window.Get<RadioButton>(TestDataInfrastructure.GetControlId("BuyRadio"));
            Assert.IsNotNull(buyRadButton);
            RadioButton sellRadButton = window.Get<RadioButton>(TestDataInfrastructure.GetControlId("SellRadio"));
            Assert.IsNotNull(sellRadButton);

            //check if Order type combobox is present
            WPFComboBox orderTypeComboBox = window.Get<WPFComboBox>(TestDataInfrastructure.GetControlId("BuySellOrderTypeCombo"));
            Assert.IsNotNull(orderTypeComboBox);

            //check if shares textbox is present
            TextBox shareTextBox = window.Get<TextBox>(TestDataInfrastructure.GetControlId("BuySellSharesTextBox"));
            Assert.IsNotNull(shareTextBox);

            //check if limit / stop price  textbox is present            
            TextBox limitStopPriceTextBox = window.Get<TextBox>(TestDataInfrastructure.GetControlId("BuySellStopLimitPriceTextBox"));
            Assert.IsNotNull(limitStopPriceTextBox);

            WPFComboBox timeInForceComboBox = window.Get<WPFComboBox>(TestDataInfrastructure.GetControlId("BuySellTimeInForceCombo"));
            Assert.IsNotNull(timeInForceComboBox);

            switch (buySell)
            {
                case BuySellEnum.Buy:
                    Assert.IsTrue(buyRadButton.IsSelected);
                    break;
                case BuySellEnum.Sell:
                    Assert.IsTrue(sellRadButton.IsSelected);
                    break;
            }

            //Button buyLastButton = window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellBuyLastButton"));
            //Assert.IsNotNull(buyLastButton);

            //Button sellLastButton = window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSellLastButton"));
            //Assert.IsNotNull(sellLastButton);

            Button submitButton = window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitButton"));
            Assert.IsNotNull(submitButton);

            Button cancelButton = window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellCancelButton"));
            Assert.IsNotNull(cancelButton);

            //check if Submit All and Cancel All buttons are present
            Button submitAllButton = window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellSubmitAllButton"));
            Assert.IsNotNull(submitAllButton);

            Button cancelAllButton = window.Get<Button>(TestDataInfrastructure.GetControlId("BuySellCancelAllButton"));
            Assert.IsNotNull(cancelAllButton);
        }

        private void ValidateBuySellPanelData(Order model)
        {
            //select the collapsible panel first
            UIItem buySellListTab = window.GetCollapsibleRegionHeader("BuySellListHeader");
            buySellListTab.Hover();

            TextBox symbolTextBox = window.Get<TextBox>(TestDataInfrastructure.GetControlId("BuySellSymbolTextBox"));
            Assert.AreEqual(model.Symbol, symbolTextBox.Text);

            if (String.Empty != model.OrderType)
            {
                WPFComboBox orderTypeComboBox = window.Get<WPFComboBox>(TestDataInfrastructure.GetControlId("BuySellOrderTypeCombo"));
                Assert.AreEqual(model.OrderType, orderTypeComboBox.SelectedItem.Text);
            }

            TextBox shareTextBox = window.Get<TextBox>(TestDataInfrastructure.GetControlId("BuySellSharesTextBox"));
            Assert.AreEqual(model.NumberOfShares.ToString(), shareTextBox.Text);

            TextBox limitStopPriceTextBox = window.Get<TextBox>(TestDataInfrastructure.GetControlId("BuySellStopLimitPriceTextBox"));
            Assert.AreEqual(model.LimitStopPrice.ToString(), limitStopPriceTextBox.Text);

            if (String.Empty != model.TimeInForce)
            {
                WPFComboBox timeInForceComboBox = window.Get<WPFComboBox>(TestDataInfrastructure.GetControlId("BuySellTimeInForceCombo"));
                Assert.AreEqual(timeInForceComboBox.SelectedItem.Text, model.FormattedTimeInForce);
            }

            switch (model.TransactionType)
            {
                case "Buy":
                    RadioButton buyRadButton = window.Get<RadioButton>(TestDataInfrastructure.GetControlId("BuyRadio"));
                    Assert.IsTrue(buyRadButton.IsSelected);
                    break;
                case "Sell":
                    RadioButton sellRadButton = window.Get<RadioButton>(TestDataInfrastructure.GetControlId("SellRadio"));
                    Assert.IsTrue(sellRadButton.IsSelected);
                    break;
            }
        }

        private void PopulateBuySellPanelWithData(Order model)
        {
            //select the collapsible panel first
            UIItem buySellListTab = window.GetCollapsibleRegionHeader("BuySellListHeader");
            buySellListTab.Click();         
           
            //enter data in model into the Buy Sell panel controls
            TextBox symbolTextBox = window.Get<TextBox>(TestDataInfrastructure.GetControlId("BuySellSymbolTextBox"));            
            symbolTextBox.Text = model.Symbol;

            WPFComboBox orderTypeComboBox = window.Get<WPFComboBox>(TestDataInfrastructure.GetControlId("BuySellOrderTypeCombo"));
            orderTypeComboBox.Select(model.OrderType);

            TextBox shareTextBox = window.Get<TextBox>(TestDataInfrastructure.GetControlId("BuySellSharesTextBox"));
            shareTextBox.Text = model.NumberOfShares.ToString();

            TextBox limitStopPriceTextBox = window.Get<TextBox>(TestDataInfrastructure.GetControlId("BuySellStopLimitPriceTextBox"));
            limitStopPriceTextBox.Text = model.LimitStopPrice.ToString();

            WPFComboBox timeInForceComboBox = window.Get<WPFComboBox>(TestDataInfrastructure.GetControlId("BuySellTimeInForceCombo"));
            timeInForceComboBox.Select(model.FormattedTimeInForce);

            switch (model.TransactionType)
            {
                case "Buy":
                    RadioButton buyRadButton = window.Get<RadioButton>(TestDataInfrastructure.GetControlId("BuyRadio"));
                    buyRadButton.Select();
                    break;
                case "Sell":
                    RadioButton sellRadButton = window.Get<RadioButton>(TestDataInfrastructure.GetControlId("SellRadio"));
                    sellRadButton.Select();
                    break;
            }
        }
        
        #endregion
    }
}
