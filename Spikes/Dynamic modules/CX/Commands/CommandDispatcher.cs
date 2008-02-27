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

        public void Dispatch<TDelegate>(CommandDelegate<TDelegate> command) where TDelegate : class
        {
            if (command == null)
                return;

            foreach (IPresenter childPresenter in presenter.ChildPresenters)
                childPresenter.CommandDispatcher.Dispatch(command);

            object commandHandler;

            if (commands.TryGetValue(typeof(TDelegate), out commandHandler))
            {
                TDelegate castedHandler = (TDelegate)commandHandler;
                command(castedHandler);
            }

        }

        public delegate void CommandDelegate<TDelegate>(TDelegate t);
    }
}
