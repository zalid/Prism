using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CX.Interfaces;
using CX.Facades;
using Infrastructure.Services;
using Module3.Views;
using System.Windows.Controls;
using Infrastructure.Events;
using Module3.Views.CompositeView;
using Infrastructure.Interfaces;

namespace Module3
{
    public class ModuleInitializer3 : IModule
    {
        private ICXContainerFacade container;

        public ModuleInitializer3(ICXContainerFacade container)
        {
            this.container = container;
        }

        #region IInitializable Members

        public void Initialize()
        {
            IRegionManager regionManager = container.Resolve<IRegionManager>();

            CompositeView compositeView = container.Resolve<CompositeView>();
            regionManager["MainDockPanel"].AddElement(compositeView, "Composite View");
        }

        #endregion

        #region IModule Members

        public void RegisterViews()
        {
            container.Register<SimpleView, SimpleView>("SimpleView");
            container.Register<ICoolView, CoolView>("coolView");
            container.Register<CoolViewPresenter, CoolViewPresenter>();
            container.Register<CompositeView, CompositeView>();
            container.Register<CompositeViewPresenter, CompositeViewPresenter>();
            //container.Register<IWeatherMapView, StubWeatherMapView>();
        }


        public void RegisterServices()
        {
        }

        #endregion
    }
}
