namespace UIComposition.Modules.Employee
{
    using System;
    using System.Collections.ObjectModel;
    using Prism.Utility;

    public interface IEmployeesListView
    {
        event EventHandler<DataEventArgs<BusinessEntities.Employee>> EmployeeSelected;
        ObservableCollection<BusinessEntities.Employee> Model { get; set; }
    }
}
