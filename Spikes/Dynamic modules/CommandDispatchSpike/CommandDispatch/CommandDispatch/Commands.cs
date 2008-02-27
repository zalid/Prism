using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CommandDispatch
{
    public delegate void MyCommandDelegate();
    public delegate void SaveDelegate();

    public static class Commands
    {
        public static readonly RoutedUICommand MyCommand;

        static Commands()
        {
            MyCommand = new RoutedUICommand("MyCommand", "MyCommand", typeof(Commands));
        }
    }
}
