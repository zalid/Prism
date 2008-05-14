//===============================================================================
// Microsoft patterns & practices
// Composite WPF (PRISM)
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================


using Microsoft.Practices.Unity;
using Prism.Interfaces;
using UIComposition.Modules.Project;
namespace UIComposition.Modules.Employee.Controllers
{
    public class EmployeesController : IEmployeesController
    {
        private IUnityContainer container;

        public EmployeesController(IUnityContainer container)
        {
            this.container = container;
        }

        public virtual void OnEmployeeSelected(IRegionManager regionManager, BusinessEntities.Employee employee)
        {
            IRegion detailsRegion = regionManager.GetRegion(RegionNames.DetailsRegion);
            object existingView = detailsRegion.GetView(employee.EmployeeId.ToString());

            if (existingView == null)
            {
                IProjectsListPresenter projectsListPresenter = this.container.Resolve<IProjectsListPresenter>();
                projectsListPresenter.SetProjects(employee.EmployeeId);

                IEmployeesDetailsPresenter detailsPresenter = this.container.Resolve<IEmployeesDetailsPresenter>();
                detailsPresenter.SetSelectedEmployee(employee);

                IRegionManager detailsRegionManager = detailsRegion.Add(detailsPresenter.View, employee.EmployeeId.ToString(), true);
                IRegion region = detailsRegionManager.GetRegion(RegionNames.TabRegion);
                region.Add(projectsListPresenter.View, "CurrentProjectsView");
                detailsRegion.Show(detailsPresenter.View);
            }
            else
            {
                detailsRegion.Show(existingView);
            }
        }
    }
}
