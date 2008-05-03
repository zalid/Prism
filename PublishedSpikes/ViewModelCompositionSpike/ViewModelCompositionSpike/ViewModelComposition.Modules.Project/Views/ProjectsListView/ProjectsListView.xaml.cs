namespace UIComposition.Modules.Project
{
    using System.Windows.Controls;
    using Prism.Interfaces;
    using Prism.Utility;
    using UIComposition.Infrastructure;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for ProjectsListView.xaml
    /// </summary>
    public partial class ProjectsListView : UserControl, IProjectsListView
    {
        public ProjectsListView()
        {
            InitializeComponent();
        }

        public ProjectsListPresentationModel Model
        {
            get { return this.DataContext as ProjectsListPresentationModel; }
            set { this.DataContext = value; }
        }
    }
}
