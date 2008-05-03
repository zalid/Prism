using System;
using System.Windows;

namespace ViewModelComposition.Modules.Project
{
    using Microsoft.Practices.Unity;
    using Prism.Interfaces;
    using ViewModelComposition.Infrastructure;
    using ViewModelComposition.Modules.Project.Services;

    public class ProjectModule : IModule
    {
        private IUnityContainer container;
        private IRegionManagerService regionManagerService;

        public ProjectModule(IUnityContainer container, IRegionManagerService regionManagerService)
        {
            this.container = container;
            this.regionManagerService = regionManagerService;
        }

        public void Initialize()
        {
            this.RegisterViewsAndServices();
        }

        protected void RegisterViewsAndServices()
        {
            // TODO: Should be singleton
            this.container.RegisterType<IProjectService, ProjectService>();


            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("pack://application:,,,/ViewModelComposition.Modules.Project;component/Views/ProjectsListView/ProjectResourceDictionary.xaml");
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }
    }
}