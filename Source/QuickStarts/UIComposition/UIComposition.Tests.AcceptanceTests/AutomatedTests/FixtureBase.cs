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
using Core;
using Core.UIItems.WindowItems;
using UIComposition.AcceptanceTests.ApplicationObserver;
using UIComposition.AcceptanceTests.TestInfrastructure;
using UIComposition.AcceptanceTests.Helpers;
using System.Collections.Specialized;
using Core.Configuration;

namespace UIComposition.AcceptanceTests
{
    public class FixtureBase : IStateObserver
    {
        public Application app;
        public Window window;
        public TestDataInfrastructure testDataInfrastructure;

        public void TestInitialize()
        {
            // Instantiate and initiate the diagnosis process. Diagnosis steps are included
            // to identify the successful launch of the application window without any unexpected
            // exceptions.
            StateDiagnosis.Instance.StartDiagnosis(this);

            app = Application.Launch(ConfigHandler.GetValue("UICompositionApp"));
            window = app.GetWindow("UI Composition QuickStart", Core.Factory.InitializeOption.NoCache);
            testDataInfrastructure = new TestDataInfrastructure();

            //Stop the diagnosis.
            StateDiagnosis.Instance.StopDiagnosis(this);
        }

        /// <summary>
        /// TestCleanup performs clean-up activities after each test method execution
        /// </summary>
        public void TestCleanup()
        {
            if (null != app)
            {
                app.Kill();
            }
        }

        private void SetupWhiteConfigParameters()
        {
            NameValueCollection collection = ConfigHandler.GetConfigSection("White/Core");

            Type coreAppXmlConfigType = CoreAppXmlConfiguration.Instance.GetType();
            foreach (string property in collection.Keys)
            {
                if (coreAppXmlConfigType.GetProperty(property).PropertyType.Equals(typeof(Int32)))
                {
                    coreAppXmlConfigType.GetProperty(property).SetValue(CoreAppXmlConfiguration.Instance, Convert.ToInt32(collection[property]), null);
                }
            }
        }

        #region IStateObserver Members

        public void Notify()
        {
            TestCleanup();
        }

        #endregion
    }
}
