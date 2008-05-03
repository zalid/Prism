using System;
using System.Windows;
using Prism.Interfaces;
using Prism.Regions;
using Prism.Services;
using Prism.Utility;
using ViewModelComposition.Modules.Employees.BusinessEntities;
using ViewModelComposition.Modules.Employees.Controllers;

namespace ViewModelComposition.Modules.Employees.Views.EmployeesView
{
    public class EmployeesPresentationModel : DependencyObject, IRegionManagerServiceScopeProvider
    {
        private EmployeesListPresentationModel employeeList;
        private IEmployeesController employeeController;

        public EmployeesPresentationModel(
            EmployeesListPresentationModel employeeList,
            IEmployeesController employeeController)
        {
            RegionManagerService = new RegionManagerService();
            RegionManagerService.Register(RegionNames.DetailsRegion, new DependencyPropertyRegion(this, DetailsProperty));

            this.employeeList = employeeList;
            this.employeeList.EmployeeSelected += new EventHandler<DataEventArgs<Employee>>(this.OnEmployeeSelected);
            this.employeeController = employeeController;
            this.employeeController.EmployeesPresentationModel = this;

            Master = employeeList;
        }


        public object Master
        {
            get { return (object)GetValue(MasterProperty); }
            set { SetValue(MasterProperty, value); }
        }

        public static readonly DependencyProperty MasterProperty =
            DependencyProperty.Register("Master", typeof(object), typeof(EmployeesPresentationModel));


        public object Details
        {
            get { return (object)GetValue(DetailsProperty); }
            set { SetValue(DetailsProperty, value); }
        }

        public static readonly DependencyProperty DetailsProperty =
            DependencyProperty.Register("Details", typeof(object), typeof(EmployeesPresentationModel));

        private void OnEmployeeSelected(object sender, DataEventArgs<Employee> e)
        {
            employeeController.OnEmployeeSelected(this, e.Value);
        }

        public IRegionManagerService RegionManagerService { get; set; }

    }
}