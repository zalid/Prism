using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CX.Interfaces;

namespace CX.Commands
{
    public class CommandDispatcher 
    {
        private Dictionary<Type, object> commands = new Dictionary<Type, object>();
        private IPresenter presenter;

        public CommandDispatcher(IPresenter presenter)
        {
            this.presenter = presenter;
        }

        public void Register<TDelegate>(TDelegate dlg)
        {
            commands[typeof(TDelegate)] = dlg;
        }

        public void Dispatch<TDelegate>(CommandDelegate<TDelegate> dlg) 
        {
            if (commands.ContainsKey(typeof(TDelegate)))
            {
                TDelegate d = (TDelegate) commands[typeof(TDelegate)];
                
                dlg(d);
            }

            foreach(IPresenter childPresenter in presenter.ChildPresenters)
            {
                childPresenter.Dispatcher.Dispatch(dlg);
            }
        }

        public delegate void CommandDelegate<TDelegate>(TDelegate t);
    }
}
