using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using CX.Facades;
using CX.Interfaces;
using CX.Regions;
using CX.Services;
using Infrastructure.Services;
using CX;
using Infrastructure.Events;

namespace RegionsSpike
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Shell : Window, IShell
    {

        private ICXContainerFacade container;
        private ShellPresenter presenter;

        public IPresenter Presenter
        {
            get{ return (IPresenter) presenter;}
        }

        public Shell(ShellPresenter presenter, ICXContainerFacade container)
        {
            this.container = container;
            this.presenter = presenter;
            this.presenter.View = this;
            
            InitializeComponent();

        }

        private void Shell_Initialized(object sender, EventArgs e)
        {
            IRegion dockPanelRegion = new PanelRegion(MainDockPanel, Presenter);
            IRegion stackPanelRegion = new PanelRegion(ButtonsStackPanel, Presenter);

            IRegionManager regionManager = container.Resolve<IRegionManager>();
            
            regionManager.AddRegion(dockPanelRegion, "MainDockPanel");
            regionManager.AddRegion(stackPanelRegion, "ButtonsStackPanel");
        }

        private void CommandBinding_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            System.Diagnostics.Trace.WriteLine("Shell view voting");
        }

        private void CommandBinding_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == Commands.ProcessCommand)
            {
                Presenter.Dispatcher.Dispatch<IProcessListener>(p=>p.Processed());
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }        


    }
}
