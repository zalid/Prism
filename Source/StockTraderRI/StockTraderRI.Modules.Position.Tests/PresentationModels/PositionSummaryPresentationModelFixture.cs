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

using System.Collections.ObjectModel;
using System.ComponentModel;
using StockTraderRI.Modules.Position.PresentationModels;
using StockTraderRI.Infrastructure.PresentationModels;

namespace StockTraderRI.Modules.Position.Tests.PresentationModels
{
    [TestClass]
    public class PositionSummaryPresentationModelFixture
    {
        [TestMethod]
        public void ChangingItemInObservableCollectionDoesNotFireCollectionChanged()
        {
            ObservableCollection<TestableNotifyPropertyChangedItem> observableTestItemCollection = new ObservableCollection<TestableNotifyPropertyChangedItem>();
            TestableNotifyPropertyChangedItem testItem = new TestableNotifyPropertyChangedItem("Mary");
            observableTestItemCollection.Add(testItem);

            bool collectionChanged = false;
            observableTestItemCollection.CollectionChanged += delegate
            {
                collectionChanged = true;
            };

            bool propertyChanged = false;
            testItem.PropertyChanged += delegate
            {
                propertyChanged = true;
            };

            testItem.Name = "Bob";

            Assert.IsTrue(propertyChanged);
            Assert.IsFalse(collectionChanged);
        }

        [TestMethod]
        public void CanGetCollectionOfPositions()
        {
            PositionSummaryPresentationModel presentationModel = new PositionSummaryPresentationModel();
            presentationModel.AddPosition("FUND0", 3.99M, 1000, 45.99M, new MarketHistoryCollection());
            presentationModel.AddPosition("FUND1", 50.00M, 100, 65.99M, new MarketHistoryCollection());

            ObservableCollection<PositionSummaryItem> positionSummaries = presentationModel.Data;

            Assert.AreEqual(2, positionSummaries.Count);
            Assert.AreEqual<string>("FUND0", positionSummaries[0].TickerSymbol);
            Assert.AreEqual<string>("FUND1", positionSummaries[1].TickerSymbol);
        }

        [TestMethod]
        public void ChangeInAllPositionsCollectionShouldFireCollectionChangeNotify()
        {
            PositionSummaryPresentationModel presentationModel = new PositionSummaryPresentationModel();
            presentationModel.AddPosition("FUND0", 3.99M, 1000, 45.99M, new MarketHistoryCollection());
            presentationModel.AddPosition("FUND1", 50.00M, 100, 65.99M, new MarketHistoryCollection());

            ObservableCollection<PositionSummaryItem> positionSummaries = presentationModel.Data;
            
            bool collectionChanged = false;
            positionSummaries.CollectionChanged += delegate
            {
                collectionChanged = true;
            };

            presentationModel.AddPosition("FUND2", 3.99M, 1000, 32.99M, new MarketHistoryCollection());
            
            Assert.IsTrue(collectionChanged);
            Assert.AreEqual<string>("FUND2", presentationModel.Data.First(p => p.TickerSymbol == "FUND2").TickerSymbol);
            Assert.AreEqual<long>(1000, presentationModel.Data.First(p => p.TickerSymbol == "FUND2").Shares);

        }

        //[TestMethod]
        //public void ChangingPropertyInPositionFiresCollectionChanged()
        //{

        //}
	
	
	
    }

    class TestableNotifyPropertyChangedItem : INotifyPropertyChanged
    {
        string name;
        public TestableNotifyPropertyChangedItem(string name)
        {
            Name = name;
        }

        public string Name
        {
            set
            {
                this.name = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }

        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate {};

        #endregion
    }
}

/*
Updates to the position collection, fires collection notify event.
Updates to properties in position, fires property change notify event.
 *  Add for each property?
*/