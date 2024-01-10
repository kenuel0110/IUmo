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

namespace IUmo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Перетаскивание окна
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (e.ClickCount == 2)
                {
                    WindowSizeState();
                }
                else
                {
                    Application.Current.MainWindow.DragMove();
                }
        }

        //Главные кнопки окна
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_hide_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btn_max_min_Click(object sender, RoutedEventArgs e)
        {
            WindowSizeState();
        }

        private void WindowSizeState(string key = "null")
        {
            if (this.WindowState == WindowState.Maximized || key == "False")
            {
                this.WindowState = WindowState.Normal;
                main_grid.Margin = new Thickness(0);
                img_btn_max_min.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/btn_maximilize.png"));
            }
            else if (this.WindowState == WindowState.Normal || key == "True")
            {
                this.WindowState = WindowState.Maximized;
                main_grid.Margin = new Thickness(5);
                img_btn_max_min.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/btn_restore.png"));
            }
        }


        //Контекстное меню
        private void btn_file_l_click(object sender, RoutedEventArgs e)
        {
            btn_file_cm.IsOpen = true;
        }

        private void btn_file_r_click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void btn_main_l_click(object sender, RoutedEventArgs e)
        {
            btn_main_cm.IsOpen = true;
        }

        private void btn_main_r_click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void btn_insert_l_click(object sender, RoutedEventArgs e)
        {
            btn_insert_cm.IsOpen = true;
        }

        private void btn_insert_r_click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

    }
}
