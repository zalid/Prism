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
using AcceptanceTestLibrary.Common;
using AcceptanceTestLibrary.UIAWrapper;
using AcceptanceTestLibrary.ApplicationHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Windows.Automation;
using MVVM_VSM_Navigation.Tests.AcceptanceTest.TestEntities.Page;

namespace MVVM_VSM_Navigation.Tests.AcceptanceTest.TestEntities.Assertion
{
    public static class MVVM_VSM_Navigation_Assertion<TApp>
        where TApp : AppLauncherBase, new()
    {
        #region Silverlight
        /// <summary>
        /// Validations on MVVM VSM Navigation App launch
        /// </summary>
        public static void AssertMVVM_VSM_Navigation_OnLoad()
        {
            //Check that List button is loaded
            AutomationElement aeListButton = MVVM_VSM_NavigationPage<TApp>.ListButton;
            Assert.IsNotNull(aeListButton, "List Button is not loaded");

            //Check that Avatars button is loaded
            Assert.IsFalse(MVVM_VSM_NavigationPage<TApp>.AvatarButton.Current.IsOffscreen, "Avatar button is not loaded");
            AutomationElementCollection aeFriendsList = MVVM_VSM_NavigationPage<TApp>.FriendList; 
            Assert.IsTrue(aeFriendsList.Count > 0, "Friends list is not loaded");
            foreach (AutomationElement aeFriend in aeFriendsList)
            {
                Assert.IsFalse(aeFriend.Current.IsOffscreen, "Friend " + aeFriend.Current.Name + "is not visible");
            }
           
            //Check that Combobox with Available and Unavailable items is loaded
            AutomationElementCollection aeComboItems = MVVM_VSM_NavigationPage<TApp>.ComboBoxItems;
            Assert.IsTrue(aeComboItems.Count == 2, "Available Combo box Item is not loaded");
            Assert.IsTrue(aeComboItems[0].Current.Name.Equals("Available"), "Available selection is unavailable");            
            Assert.IsTrue(aeComboItems[1].Current.Name.Equals("Unavailable"), "Unavailable selection is unavailable");            
        }

        /// <summary>
        /// Validations on clicking avatars button
        /// </summary>
        public static void AssertMVVM_VSM_Navigation_ClickAvatars()
        {
            //Select available from combobox
            AutomationElementCollection aeComboItems = MVVM_VSM_NavigationPage<TApp>.ComboBoxItems;
            aeComboItems[0].Select();
            Thread.Sleep(2000);

            //Find Avatas button and click it
            AutomationElement aeAvatarButton = MVVM_VSM_NavigationPage<TApp>.AvatarButton;
        //    aeAvatarButton.SetFocus();
            aeAvatarButton.Select();
            Thread.Sleep(2000);

            //Check that Avatar view is loaded
            AutomationElement aeAvatarsView = MVVM_VSM_NavigationPage<TApp>.AvatarView;
            Assert.IsNotNull(aeAvatarsView, "Avatar View is not loaded");  
        }

        /// <summary>
        /// Validations on selecting Unavailable From Combobox
        /// </summary>
        public static void AssertMVVM_VSM_Navigation_SelectUnavailable()
        {
            //Select unavailable from combobox
            AutomationElementCollection aeComboItems = MVVM_VSM_NavigationPage<TApp>.ComboBoxItems;
            aeComboItems[1].Select();
            Thread.Sleep(2000);
            //Check each item in friends list is disabled.
            foreach (AutomationElement aeFriend in MVVM_VSM_NavigationPage<TApp>.FriendList)
            {
                Assert.IsFalse(aeFriend.Current.IsEnabled, aeFriend.Current.Name + " Icon is available");
            }
            //Check that list and Avatar buttons are disabled
            AutomationElement aeListButton = MVVM_VSM_NavigationPage<TApp>.ListButton;
            Assert.IsFalse(aeListButton.Current.IsEnabled, "List button is enabled");
            AutomationElement aeAvatarButton = MVVM_VSM_NavigationPage<TApp>.AvatarButton;
            Assert.IsFalse(aeAvatarButton.Current.IsEnabled, "Avatar button is enabled");

            AutomationElement aeDisconnectedImage = MVVM_VSM_NavigationPage<TApp>.DisconnectedImage;
            Assert.IsFalse(aeDisconnectedImage.Current.IsOffscreen, "Image is not loaded on disconnection");

        }

        /// <summary>
        /// Validations on clicking "Show Details" 
        /// </summary>
        public static void AssertMVVM_VSM_Navigation_ClickDetails()
        {
            //Clicks on Show Details of a friend in list
            AssertClickDetails();
            //Check Details view and send message button are available 
            AutomationElement aeDetailsHeading = MVVM_VSM_NavigationPage<TApp>.DetailsHeading;
            Assert.IsFalse(aeDetailsHeading.Current.IsOffscreen, "Details are not loaded");

            AutomationElement aeFriendName = MVVM_VSM_NavigationPage<TApp>.FriendImage;
            Assert.IsFalse(aeFriendName.Current.IsOffscreen, "Friend Image is not displayed in details");

            AutomationElement aeSendMessage = MVVM_VSM_NavigationPage<TApp>.SendMessageButton;
            Assert.IsFalse(aeSendMessage.Current.IsOffscreen, "Send Message button is not loaded");
           
        }

        public static void AssertMVVM_VSM_Navigation_ClickDetailsInAvatarView()
        {
            AssertClickDetailsInAvatarsView();
            //Check Details view and send message button are available 
            AutomationElement aeDetailsHeading = MVVM_VSM_NavigationPage<TApp>.DetailsHeading;
            Assert.IsFalse(aeDetailsHeading.Current.IsOffscreen, "Details are not loaded");

            AutomationElement aeFriendName = MVVM_VSM_NavigationPage<TApp>.FriendImage;
            Assert.IsFalse(aeFriendName.Current.IsOffscreen, "Friend Name is not displayed in details");

            AutomationElement aeSendMessage = MVVM_VSM_NavigationPage<TApp>.SendMessageButton;
            Assert.IsFalse(aeSendMessage.Current.IsOffscreen, "Send Message button is not loaded");
        }
        /// <summary>
        /// Validations On clicking "Send Message" button
        /// </summary>
        public static void AssertMVVM_VSM_Navigation_SendMessage()
        {
            //Clicks on Show Details of a friend in list
            AssertClickDetails();
            //Click on send message button
            AutomationElement aeSendMessage = MVVM_VSM_NavigationPage<TApp>.SendMessageButton;
            Assert.IsNotNull(aeSendMessage, "Send Message button is not loaded");
            aeSendMessage.Click();
            Thread.Sleep(2000);

            //Check that send message child window is loaded
            AutomationElement aeSendMessageWindow = MVVM_VSM_NavigationPage<TApp>.SendMessageWindow;
            Assert.IsNotNull(aeSendMessageWindow, "Send Message window is not found");

            //Type text  in send message text box 
            AutomationElement aeMessageTextBox = MVVM_VSM_NavigationPage<TApp>.SendMessageTextBox;
            aeMessageTextBox.SetFocus();
            aeMessageTextBox.SetValue(new ResXConfigHandler(ConfigHandler.GetValue("TestDataInputFile")).GetValue("MessageText"));
            Thread.Sleep(2000);

            //Click OK button
            AutomationElement aeSendButton = MVVM_VSM_NavigationPage<TApp>.SendButton;
            aeSendButton.Click();
            Thread.Sleep(2000);
            //System.Windows.Point Point = new System.Windows.Point((int)Math.Floor(aeSendButton.Current.BoundingRectangle.X + 10), (int)Math.Floor(aeSendButton.Current.BoundingRectangle.Y + 2));
            //System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)Point.X, (int)Point.Y);
            //System.Threading.Thread.Sleep(1000);
            //MouseEvents.Click();
            //Thread.Sleep(2000);
            AutomationElement aeSendingProgressbar = MVVM_VSM_NavigationPage<TApp>.SendingProgressbar;
            Assert.IsNotNull(aeSendingProgressbar, "Sending message progress bar is not shown");
        }

     
        #endregion

        /// <summary>
        /// Clicks on show details button of a friend in list
        /// </summary>
        public static void AssertClickDetails()
        {
            //Select available from combobox
            AutomationElementCollection aeComboItems = MVVM_VSM_NavigationPage<TApp>.ComboBoxItems;
            aeComboItems[0].Select();
            Thread.Sleep(2000);

            AutomationElementCollection aeFriendsList = MVVM_VSM_NavigationPage<TApp>.FriendList;
            //Right click 3rd item in Friends list
         //   aeFriendsList[2].SetFocus();
            aeFriendsList[2].Select();
            Thread.Sleep(2000);           

            //Find Show Details button and click on it
            AutomationElement aeContactDetails = MVVM_VSM_NavigationPage<TApp>.ContactDetails;     
            aeContactDetails.Click();
            Thread.Sleep(2000);
            
        }

        /// <summary>
        /// Clicks on show details button of a friend in list
        /// </summary>
        public static void AssertClickDetailsInAvatarsView()
        {
            //Select available from combobox
            AutomationElementCollection aeComboItems = MVVM_VSM_NavigationPage<TApp>.ComboBoxItems;
            aeComboItems[0].Select();
            Thread.Sleep(2000);
            AutomationElementCollection aeFriendsList = MVVM_VSM_NavigationPage<TApp>.AvatarViewFriends;
            //Right click 3rd item in Friends list
            //aeFriendsList[2].SetFocus();
            aeFriendsList[2].Select();
            Thread.Sleep(3000);          

            //Find Show Details button and click on it
            AutomationElement aeContactDetails = MVVM_VSM_NavigationPage<TApp>.ContactDetails;
            aeContactDetails.Click();
            Thread.Sleep(3000);
           

        }
    }
}
