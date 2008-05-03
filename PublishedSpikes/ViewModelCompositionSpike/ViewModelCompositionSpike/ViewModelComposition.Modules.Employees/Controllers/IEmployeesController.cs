using ViewModelComposition.Modules.Employees.BusinessEntities;
using ViewModelComposition.Modules.Employees.Views.EmployeesView;
namespace ViewModelComposition.Modules.Employees.Controllers
{
    public interface IEmployeesController
    {
        void OnEmployeeSelected(EmployeesPresentationModel View, Employee employee);
        EmployeesPresentationModel EmployeesPresentationModel { get; set; }
    }
}
