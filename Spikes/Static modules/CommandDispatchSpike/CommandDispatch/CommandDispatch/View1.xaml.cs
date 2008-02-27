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
using System.Windows.Shapes;
using System.Diagnostics;
using System.Reflection;

namespace CommandDispatch
{
    /// <summary>
    /// Interaction logic for View1.xaml
    /// </summary>
    public partial class View1 : UserControl, ISupportCommands
    {
        private CommandDispatcher dispatcher = new CommandDispatcher();
      
        public View1()
        {
            InitializeComponent();

            RegisterCommands();
        }

        private void RegisterCommands()
        {
            dispatcher.Register<MyCommandDelegate>(this.MyCommand);
        }

        #region ISupportCommands Members

        public void HandleCommand(string commandName)
        {
            switch (commandName)
            {
                case "MyCommand":
                    dispatcher.Dispatch<MyCommandDelegate>(a => a());
                    break;
            }
        }

        public void MyCommand()
        {
            MessageBox.Show("MyCommand");
        }

        #endregion

        private void DoSomething()
        {
        }

        public void Save(object sender, EventArgs e)
        {
            
        }

        private void CommandBinding_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
            Trace.WriteLine("Child view voting preview");
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            e.Handled = true;
            Trace.WriteLine("Child view voting");
        }
    }
}
