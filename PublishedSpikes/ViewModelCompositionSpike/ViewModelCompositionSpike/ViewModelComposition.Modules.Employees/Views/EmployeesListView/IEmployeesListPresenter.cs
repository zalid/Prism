namespace UIComposition.Modules.Employee
{
    using System;
    using Prism.Utility;

    public interface IEmployeesListPresenter
    {
        event EventHandler<DataEventArgs<BusinessEntities.Employee>> EmployeeSelected;
        IEmployeesListView View { get; set; }
    }
}
