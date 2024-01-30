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
using WpfAnimatedGif;

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
        public Popup_show_info(string message, Classes.Class_types.Info_IMG image)
        {
            InitializeComponent();
            init(message, image);
        }

        private void init(string message, Classes.Class_types.Info_IMG image)
        {
            border_image.Visibility = Visibility.Visible;
            lbl_info.Text = message;
            switch (image) 
            {
               case Classes.Class_types.Info_IMG.img_None:
                    border_image.Visibility = Visibility.Hidden;
                    break;
                case Classes.Class_types.Info_IMG.img_blocked:
                    ImageBehavior.SetAnimatedSource(image_info, new BitmapImage(new Uri("pack://application:,,,/Illustartions/img_blocked.gif")));
                    break;
                case Classes.Class_types.Info_IMG.img_Error:
                    ImageBehavior.SetAnimatedSource(image_info, new BitmapImage(new Uri("pack://application:,,,/Illustartions/img_Error.gif")));
                    break;
                case Classes.Class_types.Info_IMG.img_finding:
                    ImageBehavior.SetAnimatedSource(image_info, new BitmapImage(new Uri("pack://application:,,,/Illustartions/img_finding.gif")));
                    break;
                case Classes.Class_types.Info_IMG.img_noFind:
                    ImageBehavior.SetAnimatedSource(image_info, new BitmapImage(new Uri("pack://application:,,,/Illustartions/img_noFind.gif")));
                    break;
            }
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
