using System.Collections.ObjectModel;
using ViewModelComposition.Modules.Project.Services;

namespace ViewModelComposition.Modules.Project
{
    public class ProjectsListPresentationModel
    {
        private IProjectService projectService;

        public ProjectsListPresentationModel(IProjectService projectService)
        {
            this.projectService = projectService;
            Projects = new ObservableCollection<BusinessEntities.Project>();
        }

        public void SetProjects(int employeeId)
        {

            var projects = this.projectService.RetrieveProjects(employeeId);
            Projects.Clear();
            foreach (var project in projects)
            {
                Projects.Add(project);
            }
        }
        public ObservableCollection<BusinessEntities.Project> Projects { get; set; }

        public string HeaderInfo
        {
            get { return "Current Projects"; }
        }
    }
}