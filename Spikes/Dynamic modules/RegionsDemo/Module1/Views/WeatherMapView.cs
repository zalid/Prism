using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infrastructure.Interfaces;

namespace WeatherModule.Views
{
    public partial class WeatherMapView : UserControl, IWeatherMapView
    {
        public WeatherMapView()
        {
            InitializeComponent();
        }
    }
}
