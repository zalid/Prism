//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcceptanceTestLibrary.Common;
using AcceptanceTestLibrary.Common.Silverlight;
using AcceptanceTestLibrary.TestEntityBase;
using AcceptanceTestLibrary.ApplicationHelper;
using System.Reflection;
using MVVM_VSM_Navigation.Tests.AcceptanceTest.TestEntities.Page;
using System.Threading;
using System.IO;
using MVVM_VSM_Navigation.Tests.AcceptanceTest.TestEntities.Assertion;

namespace MVVM_VSM_Navigation.Tests.AcceptanceTest
{
#if DEBUG

    [DeploymentItem(@".\MVVM VSM Navigation.Tests.AcceptanceTest\bin\Debug")]
    [DeploymentItem(@"..\MVVM VSM Navigation\Bin\Debug", "SL")]
#else
    [DeploymentItem(@".\MVVM VSM Navigation.Tests.AcceptanceTest\bin\Release")]
    [DeploymentItem(@"..\MVVM VSM Navigation\Bin\Release", "SL")]
#endif

    [TestClass]
    public class MVVM_VSM_Navigation_Silverlight_Tests: FixtureBase<SilverlightAppLauncher>
    {
        private const int BACKTRACKLENGTH = 4;

        #region Additional test attributes

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            string currentOutputPath = (new System.IO.DirectoryInfo(Assembly.GetExecutingAssembly().Location)).Parent.FullName;
            MVVM_VSM_NavigationPage<SilverlightAppLauncher>.Window = base.LaunchApplication(currentOutputPath + GetSilverlightApplication(), GetBrowserTitle())[0];
            Thread.Sleep(5000);
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            PageBase<SilverlightAppLauncher>.DisposeWindow();
            SilverlightAppLauncher.UnloadBrowser(GetBrowserTitle());
        }
        #endregion

        #region Test Methods

        /// <summary>
        /// MVVM VSM Navigation Launch
        /// </summary>
        [TestMethod]
        public void SilverlightMVVM_VSM_NavigationLaunch()
        {
            Assert.IsNotNull(MVVM_VSM_NavigationPage<SilverlightAppLauncher>.Window, "Navigation Prototype application is not launched.");
        }

        /// <summary>
        /// Validations on MVVM VSM Navigation App launch
        /// </summary>
        [TestMethod]
        public void SilverlightMVVM_VSM_Navigation_OnLoad()
        {
            MVVM_VSM_Navigation_Assertion<SilverlightAppLauncher>.AssertMVVM_VSM_Navigation_OnLoad();         
        }
        /// <summary>
        ///  Validations on clicking avatars button
        /// </summary>
        [TestMethod]        
        public void SilverlightMVVM_VSM_Navigation_ClickAvatars()
        {
            MVVM_VSM_Navigation_Assertion<SilverlightAppLauncher>.AssertMVVM_VSM_Navigation_ClickAvatars();      
        }
        /// <summary>
        ///  Validations on selecting Unavailable From Combobox
        /// </summary>
        [TestMethod]
        public void SilverlightMVVM_VSM_Navigation_SelectUnavailable()
        {
            MVVM_VSM_Navigation_Assertion<SilverlightAppLauncher>.AssertMVVM_VSM_Navigation_SelectUnavailable(); 
        }
        /// <summary>
        /// Validations on clicking "Show Details" 
        /// </summary>
        [TestMethod]
        public void SilverlightMVVM_VSM_Navigation_ClickDetails()
        {
            MVVM_VSM_Navigation_Assertion<SilverlightAppLauncher>.AssertMVVM_VSM_Navigation_ClickDetails(); 
        }

        /// <summary>
        /// Validations on clicking "Show Details" in Avatars View
        /// </summary>
        [TestMethod]
        public void SilverlightMVVM_VSM_Navigation_ClickDetailsInAvatarsView()
        {
            MVVM_VSM_Navigation_Assertion<SilverlightAppLauncher>.AssertMVVM_VSM_Navigation_ClickAvatars();  
            MVVM_VSM_Navigation_Assertion<SilverlightAppLauncher>.AssertMVVM_VSM_Navigation_ClickDetailsInAvatarView();
        }
        /// <summary>
        ///  Validations On clicking "Send Message" button
        /// </summary>
        [TestMethod]
        public void SilverlightMVVM_VSM_Navigation_SendMessage()
        {
            MVVM_VSM_Navigation_Assertion<SilverlightAppLauncher>.AssertMVVM_VSM_Navigation_SendMessage(); 
        }

        #endregion

        #region Helper Methods
        private static string GetSilverlightApplication()
        {
            return ConfigHandler.GetValue("SilverlightAppLocation");
        }

        private static string GetSilverlightApplicationPath(int backTrackLength)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            if (!String.IsNullOrEmpty(currentDirectory) && Directory.Exists(currentDirectory))
            {
                for (int iIndex = 0; iIndex < backTrackLength; iIndex++)
                {
                    currentDirectory = Directory.GetParent(currentDirectory).ToString();
                }
            }
            return currentDirectory + GetSilverlightApplication();
        }

        private static string GetBrowserTitle()
        {
            return new ResXConfigHandler(ConfigHandler.GetValue("ControlIdentifiersFile")).GetValue("SilverlightAppTitle");
        }
        
        #endregion
    }
}
