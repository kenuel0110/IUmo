using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IUmo.Popup
{
    /// <summary>
    /// Interaction logic for Popup_show_info.xaml
    /// </summary>
    public partial class Popup_show_info : Page
    {

        #region local_varibles
        private MainWindow mainWindow = App.Current.MainWindow as MainWindow;
        #endregion
        public Popup_show_info(string message)
        {
            InitializeComponent();
            init(message);
        }

        private void init(string message)
        {
            lbl_info.Text = message;
        }

        private void btn_close_popup_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.popup_window.Visibility = Visibility.Hidden;
            mainWindow.deblurBackground();
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}
