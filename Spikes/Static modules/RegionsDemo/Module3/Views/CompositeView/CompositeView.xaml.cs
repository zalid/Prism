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
using CX.Regions;
using CX.Interfaces;
using Infrastructure.Interfaces;
using CX.Facades;

namespace Module3.Views.CompositeView
{
    /// <summary>
    /// Interaction logic for CompositeView.xaml
    /// </summary>
    public partial class CompositeView : UserControl, IView
    {
        private Presenter<CompositeView> presenter;
        public IPresenter Presenter
        {
            get { return presenter as IPresenter; }
        }


        public CompositeView(ICXContainerFacade container, SimpleView simpleView, ICoolView coolView, CompositeViewPresenter presenter)
        {
            InitializeComponent();
            this.presenter = presenter;
            this.presenter.View = this;
            
            IRegion stackPanelRegion = new PanelRegion(InnerPanel, Presenter);
            stackPanelRegion.AddElement(simpleView, "simple view");
            stackPanelRegion.AddElement(coolView, "cool view");
            IRegion outerPanelRegion = new PanelRegion(OuterPanel, Presenter);

            //try
            //{
            //    IWeatherMapView weatherMapView = container.Resolve<IWeatherMapView>();
            //    outerPanelRegion.AddElement(weatherMapView, "weather map view");
            //}
            //catch (Exception ex) { }

            outerPanelRegion.AddElement(GetWeatherMapView(container), "weather map view");
        }

        private IWeatherMapView GetWeatherMapView(ICXContainerFacade container)
        {
            IEnumerable<IWeatherMapView> weatherMapViews = container.ResolveAll<IWeatherMapView>();
            foreach (IWeatherMapView weatherMapView in weatherMapViews)
            {
                return weatherMapView;
            }
            return null;
        }
    }
}
