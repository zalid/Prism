using System.Collections.ObjectModel;
using ViewModelComposition.Modules.Employees.BusinessEntities;

namespace ViewModelComposition.Modules.Employees
{
    using System;
    using Prism.Utility;
    using ViewModelComposition.Modules.Employees.Services;

    public class EmployeesListPresentationModel
    {
        private Employee _selectedEmployee;
        public event EventHandler<DataEventArgs<Employee>> EmployeeSelected = delegate { };

        public EmployeesListPresentationModel(IEmployeeService employeeService)
        {
            Employees = employeeService.RetrieveEmployees();
        }

        public ObservableCollection<Employee> Employees { get; private set; }

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                this.EmployeeSelected(this, new DataEventArgs<Employee>(value));
            }
        }
    }
}
