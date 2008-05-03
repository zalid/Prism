using System.Windows;
using ViewModelComposition.Modules.Employees.BusinessEntities;

namespace ViewModelComposition.Modules.Employees.Views.EmployeesDetailsView
{
    public class HeaderedEmployeeData : DependencyObject
    {
        public HeaderedEmployeeData()
        {
            //TODO: We only wrapped this to get the tab header to show up correctly.
            HeaderInfo = "Employee Info";
        }


        public string HeaderInfo { get; set; }

        public Employee Employee
        {
            get { return (Employee)GetValue(EmployeeProperty); }
            set { SetValue(EmployeeProperty, value); }
        }

        public static readonly DependencyProperty EmployeeProperty =
            DependencyProperty.Register("Employee", typeof(Employee), typeof(HeaderedEmployeeData));


    }
}