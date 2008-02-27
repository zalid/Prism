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
            get{ return presenter;}
        }

        public Shell(ShellPresenter presenter, ICXContainerFacade container)
        {
            this.container = container;
            this.presenter = presenter;

            InitializeComponent();
        }

        private void Shell_Initialized(object sender, EventArgs e)
        {
            this.presenter.View = this;

            IRegion dockPanelRegion = new PanelRegion(MainDockPanel, Presenter);
            IRegion stackPanelRegion = new PanelRegion(ButtonsStackPanel, Presenter);

            presenter.DockPanelRegion = dockPanelRegion;
            presenter.StackPanelRegion = stackPanelRegion;

            presenter.OnViewReady();
        }

        private void CommandBinding_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = presenter.OnCommand_PreviewCanExecute(e.Command);
        }

        private void CommandBinding_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            presenter.OnCommand_PreviewExecuted(e.Command);
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }        


    }
}
