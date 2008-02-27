using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using CX.Interfaces;

namespace RegionsSpike
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Bootstrapper bootStrapper = new Bootstrapper();
        public App() {
            bootStrapper.Initialize();
        }
    }
}
