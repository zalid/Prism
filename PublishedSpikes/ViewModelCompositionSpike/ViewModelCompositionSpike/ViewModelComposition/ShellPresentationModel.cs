using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Prism;
using Prism.Services;
using ViewModelComposition.Modules.Employees;
using Prism.Interfaces;
using Prism.Regions;

namespace ViewModelComposition
{
    class ShellPresentationModel : Control, IRegionManagerServiceScopeProvider
    {
        private readonly RegionManagerService regionManagerService;
        private ItemsControl mainContent;

        public ShellPresentationModel(RegionManagerService regionManagerService)
        {
            this.regionManagerService = regionManagerService;
            ToolBarContent = new ObservableCollection<object>();
            MainContentCollection = new ObservableCollection<object>();

            Button button = new Button();
            button.Content = "shell button";

            ToolBarContent.Add(button);

            Button button1 = new Button();
            button1.Content = "shell button 1";

            ToolBarContent.Add(button1);

            regionManagerService.Register(RegionNames.MainRegion, new ObservableCollectionRegion<object>(MainContentCollection));
            regionManagerService.Register(RegionNames.ToolBarRegion, new ObservableCollectionRegion<object>(ToolBarContent));
        }

        public ObservableCollection<object> ToolBarContent { get; set; }
        public ObservableCollection<object> MainContentCollection { get; set; }

        #region IRegionManagerServiceScopeProvider Members

        public IRegionManagerService RegionManagerService
        {
            get { return regionManagerService; }
        }

        #endregion
    }

}
