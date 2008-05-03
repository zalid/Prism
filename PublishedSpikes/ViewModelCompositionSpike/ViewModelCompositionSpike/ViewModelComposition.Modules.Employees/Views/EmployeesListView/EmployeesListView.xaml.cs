namespace UIComposition.Modules.Employee
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Controls;
    using Prism.Utility;

    /// <summary>
    /// Interaction logic for EmployeesListView.xaml
    /// </summary>
    public partial class EmployeesListView : UserControl, IEmployeesListView
    {
        public EmployeesListView()
        {
            InitializeComponent();
        }

        public ObservableCollection<BusinessEntities.Employee> Model
        {
            get { return this.DataContext as ObservableCollection<BusinessEntities.Employee>; }
            set { this.DataContext = value; }
        }

        public event EventHandler<DataEventArgs<BusinessEntities.Employee>> EmployeeSelected = delegate { };

        private void EmployeesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                BusinessEntities.Employee selected = e.AddedItems[0] as BusinessEntities.Employee;
                if (selected != null)
                {
                    EmployeeSelected(this, new DataEventArgs<BusinessEntities.Employee>(selected));
                }
            }
        }
    }
}
