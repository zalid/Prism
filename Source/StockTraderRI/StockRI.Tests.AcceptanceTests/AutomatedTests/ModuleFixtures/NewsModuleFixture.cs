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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Automation;
using Core.UIItems.Finders;
using Core.UIItems;
using Core.UIItems.TabItems;
using Core;
using Core.UIItems.WindowItems;
using StockTraderRI.AcceptanceTests.TestInfrastructure;
using StockTraderRI.AcceptanceTests.Helpers;
using StockTraderRI.AcceptanceTests.TestInfrastructure.MockModels;
using StockTraderRI.AcceptanceTests.TestInfrastructure.DataProvider.ModuleDataProviders;

namespace StockTraderRI.AcceptanceTests.AutomatedTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    [DeploymentItem(@".\StockTraderRI\bin\Debug")]
    [DeploymentItem(@".\StockRI.Tests.AcceptanceTests\bin\Debug")]
    public class NewsModuleFixture : FixtureBase
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
        /// Check if Selected symbol's news button click adds the tab with  
        /// title as a symbol code.
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Click on the news button of a symbol detail
        /// 3. Check the new tab with symbol code as a title is added to the tab control.
        /// 
        /// Expected Result:
        /// On clicking the News button of a selected symbol, new tab with title as a symbol
        /// code should be created.
        /// </summary>
        [TestMethod]
        public void SelectedSymbolNewsButtonClick()
        {
            Button button = window.Get<Button>(SearchCriteria.ByAutomationId(ConfigHandler.GetTestInputData("Symbol")).AndControlType(typeof(Button)));
            Tab tab = window.Get<Tab>(ConfigHandler.GetControlId("ShellTabControl"));

            Assert.IsTrue((tab == null) ? true : (0 == tab.Pages.Count));

            // Clicking the button should add a new FUND0 Tab
            button.Click();

            tab = window.Get<Tab>(ConfigHandler.GetControlId("ShellTabControl"));

            Assert.AreEqual(1, tab.Pages.Count);

            //The following code should be the correct way to walk the raw tree,   
            //but the search never returns when the name is not found (instead of returning null)
            //TreeWalker walker = new TreeWalker(new PropertyCondition(AutomationElement.NameProperty, "FUND0"));
            //AutomationElement elementNode = walker.GetFirstChild(((UIItem)tab.Pages[0]).AutomationElement);
            //Assert.IsNotNull(elementNode);

            ////Using UI Automation framework to search for an element with name "FUND0"
            ////Not being able to search using White when using WPF templates
            AutomationElement element = (((UIItem)tab.Pages[0]).AutomationElement).SearchInRawTreeByName(ConfigHandler.GetTestInputData("Symbol"));
            Assert.IsNotNull(element);
        }

        /// <summary>
        /// Check on repeated click of a particular symbol's news button should not
        /// add a tab every click.
        ///
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Get the news button of a particular symbol
        /// 3. Click on the news button.
        /// 4. Check the new tab is added to the tab control.
        /// 5. Click on the news button.
        /// 6. Check the new tab is not added.
        /// 
        /// Expected Result:
        /// Only for the first click new tab should be added for a particular symbol.
        /// </summary>
        [TestMethod]
        public void SelectedSymbolRepeatedNewsButtonClick()
        {
            Button button = window.Get<Button>(SearchCriteria.ByAutomationId(ConfigHandler.GetTestInputData("Symbol")).AndControlType(typeof(Button)));
            Tab tab = window.Get<Tab>(ConfigHandler.GetControlId("ShellTabControl"));

            // Clicking the button should add a new FUND0 Tab
            button.Click();

            //Adding sleep to verify whether this is the cause for random test failure on build server
            System.Threading.Thread.Sleep(1000);

            tab = window.Get<Tab>(ConfigHandler.GetControlId("ShellTabControl"));
            Assert.AreEqual(1, tab.Pages.Count);

            // Clicking the same button should not add a new FUND0 Tab
            button.Click();

            tab = window.Get<Tab>(ConfigHandler.GetControlId("ShellTabControl"));
            Assert.AreEqual(1, tab.Pages.Count);
        }

        /// <summary>
        /// On click of a symbol news button, tab control should focus on that appropriate 
        /// tab
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Get the news button of a symbols
        /// 3. Click on the symbol1 news button.
        /// 4. Check the new tab is added to the tab control and selected tab is matching the 
        /// name of that symbol.
        /// 5. Click on the symbol2 news button.
        /// 6. Check the new tab is added to the tab control and selected tab is matching the 
        /// name of that symbol.
        /// 
        /// Expected Result:
        /// Symbol's news button click should bring focus to the appropriate tab.
        /// </summary>
        [TestMethod]
        public void ClickingSymbolBringsFocusToAppropriateTabInNewsRegion()
        {
            Button firstNewsButton = window.Get<Button>(SearchCriteria.ByAutomationId(ConfigHandler.GetTestInputData("Symbol")).AndControlType(typeof(Button)));
            Button secondNewsButton = window.Get<Button>(SearchCriteria.ByAutomationId(ConfigHandler.GetTestInputData("PositionSymbol")).AndControlType(typeof(Button)));
            Tab tab = window.Get<Tab>(ConfigHandler.GetControlId("ShellTabControl"));

            Assert.AreEqual(0, tab.Pages.Count);

            // Clicking the button should add a new FUND0 Tab
            firstNewsButton.Click();

            tab = window.Get<Tab>(ConfigHandler.GetControlId("ShellTabControl"));
            Assert.AreEqual(1, tab.Pages.Count);
            Assert.IsTrue(tab.SelectedTab.NameMatches(ConfigHandler.GetTestInputData("Symbol")));

            // Clicking the news button should add a new FUND6 Tab
            secondNewsButton.Click();

            tab = window.Get<Tab>(ConfigHandler.GetControlId("ShellTabControl"));
            Assert.AreEqual(2, tab.Pages.Count);
            Assert.IsTrue(tab.SelectedTab.NameMatches(ConfigHandler.GetTestInputData("PositionSymbol")));
        }

        /// <summary>
        /// On clicking of a symbol's news button,whose tab is already shown, 
        /// should bring focus on that appropriate tab
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Get the news button of a symbols
        /// 3. Click on the symbols news button.
        /// 4. Check the selected tab should match the appropriate symbol
        /// 
        /// Expected Result:
        /// On clicking of a symbol's news button,whose tab is already shown, 
        /// should bring focus on that appropriate tab
        /// </summary>
        [TestMethod]
        public void ClickingAlreadyShownSymbolBringsFocusToAppropriateTabInNewsRegion()
        {
            Button firstNewsButton = window.Get<Button>(SearchCriteria.ByAutomationId(ConfigHandler.GetTestInputData("Symbol")).AndControlType(typeof(Button)));
            Button secondNewsButton = window.Get<Button>(SearchCriteria.ByAutomationId(ConfigHandler.GetTestInputData("PositionSymbol")).AndControlType(typeof(Button)));

            // Clicking the button should add a new FUND0 Tab
            firstNewsButton.Click();

            // Clicking the news button should add a new FUND6 Tab
            secondNewsButton.Click();

            Tab tab = window.Get<Tab>(ConfigHandler.GetControlId("ShellTabControl"));
            Assert.IsFalse(tab.SelectedTab.NameMatches(ConfigHandler.GetTestInputData("Symbol")));

            // Clicking the FUND0 button again should give focus to the FUND0 Tab
            firstNewsButton.Click();

            tab = window.Get<Tab>(ConfigHandler.GetControlId("ShellTabControl"));
            Assert.AreEqual(2, tab.Pages.Count);
            Assert.IsTrue(tab.SelectedTab.NameMatches(ConfigHandler.GetTestInputData("Symbol")));
        }

        /// <summary>
        /// Check if the News tab of the selected Symbol displays news message from the News Data XML file
        /// 
        /// Repro steps:
        /// 1. Launch the StockTrader RI application
        /// 2. Click on the News button for a symbol
        /// 3. Check if a news tab page is added with the News text display
        /// 4. Click on yet another News button for a symbol
        /// 5. Check if a second news tab page is added with the display of the corresponding News text
        /// 
        /// Expected Result:
        /// The news tab pages are added, with the corresponding News text display.
        /// </summary>
        [TestMethod]
        public void NewsTabDisplaysNewsTextOfSelectedSymbol()
        {
            Button firstNewsButton = window.Get<Button>(SearchCriteria.ByAutomationId(ConfigHandler.GetTestInputData("Symbol")).AndControlType(typeof(Button)));
            Button secondNewsButton = window.Get<Button>(SearchCriteria.ByAutomationId(ConfigHandler.GetTestInputData("PositionSymbol")).AndControlType(typeof(Button)));
            Tab tab = window.Get<Tab>(ConfigHandler.GetControlId("ShellTabControl"));

            // Clicking the button should add a new FUND0 Tab which displays the News Text for FUND0
            firstNewsButton.Click();

            List<News> news = testDataInfrastructure.GetData<NewsDataProvider, News>();
            string newsTextForSymbol = news.Find(n => n.TickerSymbol.Equals(ConfigHandler.GetTestInputData("Symbol"))).NewsItemText;

            Assert.IsNotNull(window.AutomationElement.SearchInRawTreeByName(newsTextForSymbol));

            // Clicking the news button should add a new FUND6 Tab which displays the News Text for FUND6
            secondNewsButton.Click();
            newsTextForSymbol = news.Find(n => n.TickerSymbol.Equals(ConfigHandler.GetTestInputData("PositionSymbol"))).NewsItemText;

            Assert.IsNotNull(window.AutomationElement.SearchInRawTreeByName(newsTextForSymbol));
        }
    }
}
