using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CX.Interfaces;
using Infrastructure.Events;

namespace Module3.Views
{
    public class CoolViewPresenter : Presenter<ICoolView>, IProcessListener
    {
        #region IModuleButtonListener Members

        public CoolViewPresenter(IEventBrokerService eventBrokerService)
        {
            eventBrokerService.Subscribe<IProcessListener>(this);
            Dispatcher.Register<IProcessListener>(this);
        }

        public void Processed()
        {
            View.DoIt();
        }

        #endregion

    }
}
