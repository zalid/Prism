using ViewModelComposition.Modules.Employees.BusinessEntities;

namespace ViewModelComposition.Modules.Employees.Services
{
    using System.Collections.ObjectModel;

    public class EmployeeService : IEmployeeService
    {
        public ObservableCollection<Employee> RetrieveEmployees()
        {
            ObservableCollection<Employee> employees = new ObservableCollection<Employee>();

            employees.Add(new Employee(1) { FirstName = "John", LastName = "Smith", Phone = "+1 (425) 555-0101", Email = "john.smith@example.com", Address = "One Microsoft Way", City = "Redmond", State = "WA" });
            employees.Add(new Employee(2) { FirstName = "Bonnie", LastName = "Skelly", Phone = "+1 (425) 555-0105", Email = "bonnie.skelly@example.com", Address = "One Microsoft Way", City = "Redmond", State = "WA" });

            return employees;
        }
    }
}
