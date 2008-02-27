using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CX.Interfaces;
using CX.Facades;
using System.Windows.Controls;
using Infrastructure.Services;
using Infrastructure.Events;
using RestaurantModule.Services;

namespace RestaurantModule
{
    public class ModuleInitializer2 : IModule
    {
        private ICXContainerFacade container;
        IEventBrokerService eventBrokerService;

        public ModuleInitializer2(ICXContainerFacade container, IEventBrokerService eventBrokerService)
        {
            this.container = container;
            this.eventBrokerService = eventBrokerService;
        }

        #region IInitializable Members

        public void Initialize()
        {
            IRegionManager regionManager = container.Resolve<IRegionManager>();
            IEventBrokerService eventBroker = container.Resolve<IEventBrokerService>();

            Button button = new Button();
            button.Content = "Process";

            regionManager["ButtonsStackPanel"].AddElement(button, "button");

            button.Click += (sender, args) =>
            {
                eventBroker.Publish<IProcessListener>(p => p.Processed());
            };
        }

        #endregion

        #region IModule Members

        public void RegisterViews()
        {
        }

        public void RegisterServices()
        {
            container.Register<ILocationProviderService, RestaurantLocationProviderService>("RestaurantLocationProviderService");
        }

        #endregion
    }
}
 