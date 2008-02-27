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
using System.Diagnostics;
using System.Reflection;

namespace CommandDispatch
{
    /// <summary>
    /// Interaction logic for CompositeView.xaml
    /// </summary>
    public partial class CompositeView : UserControl
    {
        private IList<UserControl> childViews = new List<UserControl>();

        public CompositeView()
        {
            InitializeComponent();
           
            AddChildViews();
        }


        private void AddChildViews()
        {
            View1 view1 = new View1();
            View2 view2 = new View2();

            this.ContainerDockPanel.Children.Add(view2);
            this.ContainerDockPanel.Children.Add(view1);
            DockPanel.SetDock(view2, Dock.Bottom);

            this.childViews.Add(view1);
            this.childViews.Add(view2);
        }

        private void CommandBinding_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            Trace.WriteLine("Composite view voting");
        }

        private void CommandBinding_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            string commandName = ((RoutedUICommand)e.Command).Name;

            // Interface / Command Binding Approach
            // This code is here only to spike purposes. Has to be in other component.

            foreach (UserControl view in childViews)
            {
                ISupportCommands viewCommand = view as ISupportCommands;

                if (viewCommand != null)
                    viewCommand.HandleCommand(commandName);
            }

            // Reflection Approach
            // This code is here only to spike purposes. Has to be in other component.

            //foreach (UserControl view in childViews)
            //{
            //    Type type = view.GetType();
            //    MethodInfo methodInfo = type.GetMethod(commandName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

            //    if (methodInfo != null)
            //        methodInfo.Invoke(view, new object[] { this, new EventArgs() });
            //}
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }        
    }
}
