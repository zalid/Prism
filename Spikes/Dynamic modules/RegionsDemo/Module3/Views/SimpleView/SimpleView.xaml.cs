using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CX.Interfaces;
using Infrastructure;
using Infrastructure.Events;
using CX.Facades;
using Infrastructure.Services;

namespace Module3.Views
{
    public partial class SimpleView : UserControl
    {
        private ICXContainerFacade container;
        
        public SimpleView(ICXContainerFacade container)
        {
            InitializeComponent();
            this.container = container;

            IEnumerable<ILocationProviderService> locationProviderServices = container.ResolveAll<ILocationProviderService>();

            foreach (ILocationProviderService locationProviderService in locationProviderServices)
            {
                foreach (var location in locationProviderService.GetLocations())
                {
                    label1.Content += string.Format("Position({0},{1}) : {2}\n", location.Key.Latitude, location.Key.Longtitude, location.Value);
                }
            }
        }

    }
}