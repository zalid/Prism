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

        public void RegisterServices()
        {
            container.Register<ILocationProviderService, WeatherLocationProviderService>("WeatherLocationProviderService");
        }

        public void RegisterViews()
        {
            container.Register<IWeatherMapView, WeatherMapView>("WeatherMapView");
        }


        public void Initialize()
        {
        }
    }
}
