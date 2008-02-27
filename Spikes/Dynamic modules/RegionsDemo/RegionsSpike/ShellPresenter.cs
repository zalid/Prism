using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CX.Facades;
using CX.Interfaces;
using System.Windows.Input;
using Infrastructure.Events;

namespace RegionsSpike
{
    public class ShellPresenter : Presenter<IShell>
    {
        
        public ShellPresenter(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public IRegion DockPanelRegion { get; set; }
        public IRegion StackPanelRegion { get; set; }
        private IRegionManager regionManager;

        protected override void OnViewSet()
        {
        }

        public override void OnViewReady()
        {
            regionManager.AddRegion(DockPanelRegion, "MainDockPanel");
            regionManager.AddRegion(StackPanelRegion, "ButtonsStackPanel");
        }


        public void OnCommand_PreviewExecuted(ICommand command)
        {
            if (command == Commands.ProcessCommand)
            {
                CommandDispatcher.Dispatch<IProcessListener>(p => p.Processed());
            }
        }

        public bool OnCommand_PreviewCanExecute(ICommand command) {
            System.Diagnostics.Trace.WriteLine("Shellview voting");
            return true;
        }

    }
}
