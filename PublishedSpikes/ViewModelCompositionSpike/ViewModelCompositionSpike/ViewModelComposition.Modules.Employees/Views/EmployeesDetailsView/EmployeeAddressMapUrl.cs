
using System.Windows;
using ViewModelComposition.Modules.Employees.BusinessEntities;

namespace ViewModelComposition.Modules.Employees.Views.EmployeesDetailsView
{
    public class EmployeeAddressMapUrl : DependencyObject
    {
        public EmployeeAddressMapUrl()
        {
            HeaderInfo = "Employee Location";
        }

        const string mapUrlFormat = "http://maps.msn.com/home.aspx?strt1={0}&city1={1}&stnm1={2}";

        public string AddressMapUrl
        {
            get { return (string)GetValue(AddressMapUrlProperty); }
            set { SetValue(AddressMapUrlProperty, value); }
        }

        public static readonly DependencyProperty AddressMapUrlProperty =
            DependencyProperty.Register("AddressMapUrl", typeof(string), typeof(EmployeeAddressMapUrl));


        public string HeaderInfo { get; set; }
        public Employee Employee
        {
            get { return (Employee)GetValue(EmployeeProperty); }
            set { SetValue(EmployeeProperty, value); }
        }

        public static readonly DependencyProperty EmployeeProperty =
            DependencyProperty.Register("Employee", typeof(Employee), typeof(EmployeeAddressMapUrl), new PropertyMetadata(new PropertyChangedCallback(EmployeeChanged)));

        private static void EmployeeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var thisObject = (EmployeeAddressMapUrl)d;
            Employee emp = e.NewValue as Employee;
            thisObject.AddressMapUrl = (emp != null) ? string.Format(mapUrlFormat, emp.Address, emp.City, emp.State) : string.Empty;
        }

    }
}