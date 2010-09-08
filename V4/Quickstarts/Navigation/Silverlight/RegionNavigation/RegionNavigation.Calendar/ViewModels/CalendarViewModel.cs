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
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using RegionNavigation.Calendar.Model;

namespace RegionNavigation.Calendar.ViewModels
{
    [Export]
    public class CalendarViewModel
    {
        private readonly SynchronizationContext synchronizationContext;
        private readonly DelegateCommand<Meeting> openMeetingEmailCommand;
        private readonly ObservableCollection<Meeting> meetings;
        private readonly IRegionManager regionManager;
        private readonly ICalendarService calendarService;

        public CalendarViewModel()
        {
            this.synchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();

            this.openMeetingEmailCommand = new DelegateCommand<Meeting>(this.OpenMeetingEmail);

            this.meetings = new ObservableCollection<Meeting>();
        }

        [ImportingConstructor]
        public CalendarViewModel(ICalendarService calendarService, IRegionManager regionManager)
            : this()
        {
            this.calendarService = calendarService;
            this.regionManager = regionManager;

            this.calendarService.BeginGetMeetings(
                r =>
                {
                    var meetings = this.calendarService.EndGetMeetings(r);

                    this.synchronizationContext.Post(
                        s =>
                        {
                            foreach (var meeting in meetings)
                            {
                                this.Meetings.Add(meeting);
                            }
                        },
                        null);
                },
                null);
        }

        public ObservableCollection<Meeting> Meetings
        {
            get
            {
                return this.meetings;
            }
        }

        public ICommand OpenMeetingEmailCommand
        {
            get { return this.openMeetingEmailCommand; }
        }

        private void OpenMeetingEmail(Meeting meeting)
        {
            var builder = new StringBuilder();
            builder.Append("EmailView");
            var query = new UriQuery();
            query.Add("EmailId", meeting.EmailId.ToString("N"));
            builder.Append(query);
            this.regionManager.RequestNavigate("MainContentRegion", new Uri(builder.ToString(), UriKind.Relative), nr => { });
        }
    }
}
