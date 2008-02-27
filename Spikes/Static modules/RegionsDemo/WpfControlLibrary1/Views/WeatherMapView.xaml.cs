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
using Infrastructure.Interfaces;

namespace WeatherModule.Views
{
    /// <summary>
    /// Interaction logic for WeatherMapView.xaml
    /// </summary>
    public partial class WeatherMapView : UserControl, IWeatherMapView
    {
        public WeatherMapView()
        {
            InitializeComponent();
        }
    }
}
