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
using Core.UIItems;
using System.Windows.Automation;
using Core.UIItems.TabItems;
using StockTraderRI.AcceptanceTests.TestInfrastructure;

namespace StockTraderRI.AcceptanceTests.Helpers
{
    public static class UIItemExtensions
    {
        public static void Hover(this UIItem uiItem)
        {
            Core.InputDevices.Mouse.Instance.Location = Core.C.Center(uiItem.Bounds);
            System.Threading.Thread.Sleep(1000);
        }

        public static AutomationElement SearchInRawTreeByName(this AutomationElement rootElement, string name)
        {
            AutomationElement elementNode = TreeWalker.RawViewWalker.GetFirstChild(rootElement);

            while (elementNode != null)
            {
                if (name.Equals(elementNode.Current.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return elementNode;
                }
                AutomationElement returnedAutomationElement = elementNode.SearchInRawTreeByName(name);
                if (returnedAutomationElement != null)
                {
                    return returnedAutomationElement;
                }
                elementNode = TreeWalker.RawViewWalker.GetNextSibling(elementNode);
            }
            return null;
        }

        public static UIItem GetWatchListRegionHeader(this UIItemContainer rootElement)
        {
            Tab tab = rootElement.Get<Tab>(TestDataInfrastructure.GetControlId("CollapsibleRegion"));
            UIItem watchListTab = tab.Pages.Find(x => x.NameMatches(TestDataInfrastructure.GetControlId("WatchListHeader"))) as UIItem;
            return watchListTab;
        }

        public static UIItem GetCollapsibleRegionHeader(this UIItemContainer rootElement, string controlID)
        {
            Tab tab = rootElement.Get<Tab>(TestDataInfrastructure.GetControlId("CollapsibleRegion"));
            UIItem watchListTab = tab.Pages.Find(x => x.NameMatches(TestDataInfrastructure.GetControlId(controlID))) as UIItem;
            return watchListTab;
        }

        /// <summary>
        /// Get the required field data from the specified row of the Position Table
        /// </summary>
        /// <param name="list">the position table UI object</param>
        /// <param name="rowNumber">Row number from which data needs to fished out of the Position table</param>
        /// <param name="header">column header of data that is expected</param>
        /// <returns>value in the specified field of the specified row of the Position table</returns>
        public static object GetData(this ListView list, int rowNumber, PositionTableColumnHeader header)
        {
            switch (header)
            {
                case PositionTableColumnHeader.Symbol:
                    return list.Rows[rowNumber].Cells[TestDataInfrastructure.GetTestInputData("PositionTableSymbol")].Text;
                case PositionTableColumnHeader.NumberOfShares:
                    return Convert.ToInt32(list.Rows[rowNumber].Cells[TestDataInfrastructure.GetTestInputData("PositionTableShares")].Text);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Get the required field data for the specified symbol from the Position Table
        /// </summary>
        /// <param name="list">the position table UI object</param>
        /// <param name="forSymbol">symbol for which data is required</param>
        /// <param name="header">column header of data that is expected</param>
        /// <returns>value in the specified field of the specified symbol row of the Position table</returns>
        public static object GetData(this ListView list, string forSymbol, PositionTableColumnHeader header)
        {
            switch (header)
            {
                case PositionTableColumnHeader.Symbol:
                    return forSymbol;
                case PositionTableColumnHeader.NumberOfShares:
                    return Convert.ToInt32(list.Rows.Find(r => r.Cells[TestDataInfrastructure.GetTestInputData("PositionTableSymbol")].Text.Equals(forSymbol))
                        .Cells[TestDataInfrastructure.GetTestInputData("PositionTableShares")].Text);
                default:
                    return null;
            }
        }
    }
}
