using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace RegionsSpike
{
    public static class Commands
    {
        public static readonly RoutedUICommand ProcessCommand;

        static Commands()
        {
            ProcessCommand = new RoutedUICommand("ProcessCommand", "ProcessCommand", typeof (Commands));
        }


    }
}
