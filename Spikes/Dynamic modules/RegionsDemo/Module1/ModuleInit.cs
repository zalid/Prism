using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CX.Interfaces;
using CX.Facades;
using Infrastructure.Services;
using WeatherModule.Services;
using WeatherModule.Views;
using Infrastructure.Interfaces;

namespace WeatherModule
{
    public class ModuleInit : IModule
    {
        private ICXContainerFacade container;

        public ModuleInit(ICXContainerFacade container)
        {
            this.container = container;
        }

        public void AddServices()
        {
            container.Register<ILocationProviderService, WeatherLocationProviderService>("WeatherLocationProviderService");
        }

        public void AddViews()
        {
            container.Register<IWeatherMapView, WeatherMapView>();
        }


        public void Initialize()
        {
        }
    }
}
