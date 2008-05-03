using ViewModelComposition.Modules.Employees.BusinessEntities;

namespace ViewModelComposition.Modules.Employees.Services
{
    using System.Collections.ObjectModel;

    public interface IEmployeeService
    {
        ObservableCollection<Employee> RetrieveEmployees();
    }
}
