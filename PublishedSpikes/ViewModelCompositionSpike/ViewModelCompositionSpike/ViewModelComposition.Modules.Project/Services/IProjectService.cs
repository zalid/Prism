namespace ViewModelComposition.Modules.Project.Services
{
    using System.Collections.ObjectModel;

    public interface IProjectService
    {
        ObservableCollection<BusinessEntities.Project> RetrieveProjects(int employeeId);
    }
}
