using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandDispatch
{
    public class CommandDispatcher
    {
        private Dictionary<Type, object> commands = new Dictionary<Type, object>();

        public void Register<TDelegate>(TDelegate dlg)
        {
            Delegate cmdDlg = dlg as Delegate;
            if (cmdDlg != null)
            {
                commands[typeof(TDelegate)] = cmdDlg;
            }

        }

        public void Dispatch<TDelegate>(PrismDelegate<TDelegate> dlg) 
        {
            if (commands.ContainsKey(typeof(TDelegate)))
            {
                TDelegate d = (TDelegate) commands[typeof(TDelegate)];
                
                dlg(d);

                // How to invoke if we have a collection of
                // delegates and PrismDelegate is expecting a specific delegate
                
                //dlg(d) doesn't work
            }
        }

        public delegate void PrismDelegate<TDelegate>(TDelegate t);

    }
}
