
using Prism.Interfaces;
using ViewModelComposition.Modules.Employees.BusinessEntities;
using ViewModelComposition.Modules.Employees.Views.EmployeesDetailsView;
using ViewModelComposition.Modules.Employees.Views.EmployeesView;
using ViewModelComposition.Modules.Project;

namespace ViewModelComposition.Modules.Employees.Controllers
{
    public class EmployeesController : IEmployeesController
    {
        private EmployeesDetailsPresentationModel employeesDetailsPresentationModel;
        private ProjectsListPresentationModel projectsList;

        public EmployeesController(EmployeesDetailsPresentationModel employeesDetailsPresentationModel, ProjectsListPresentationModel projectsList)
        {
            this.employeesDetailsPresentationModel = employeesDetailsPresentationModel;
            IRegion tabRegion = employeesDetailsPresentationModel.RegionManagerService.GetRegion(RegionNames.TabRegion);
            tabRegion.Add(projectsList);
            this.projectsList = projectsList;
        }

        private EmployeesPresentationModel _EmployeesPresentationModel;

        public EmployeesPresentationModel EmployeesPresentationModel
        {
            get { return _EmployeesPresentationModel; }
            set
            {
                _EmployeesPresentationModel = value;
                _EmployeesPresentationModel.RegionManagerService.GetRegion(RegionNames.DetailsRegion).Add(
                    employeesDetailsPresentationModel);
            }
        }

        public virtual void OnEmployeeSelected(EmployeesPresentationModel employees, Employee employee)
        {
            employeesDetailsPresentationModel.SetSelectedEmployee(employee);
            projectsList.SetProjects(employee.EmployeeId);
        }
    }
}
