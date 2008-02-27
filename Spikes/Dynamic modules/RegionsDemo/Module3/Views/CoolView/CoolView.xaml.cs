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
using System.Windows.Media.Animation;

namespace Module3.Views
{
    /// <summary>
    /// Interaction logic for CoolView.xaml
    /// </summary>
    public partial class CoolView : UserControl, ICoolView
    {
        private CoolViewPresenter presenter;

        IPresenter IView.Presenter
        {
            get{ return (IPresenter) presenter;}
        }

        public CoolView(CoolViewPresenter presenter)
        {
            InitializeComponent();

            this.presenter = presenter;
            this.presenter.View = this;
        }

        #region ICoolView Members

        public void DoIt()
        {
            Storyboard ellipseStoryboard = (Storyboard)this.FindResource("Timeline1");
            ellipseStoryboard.Begin(this);
        }

        #endregion
    }
}
