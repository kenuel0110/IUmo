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
        #region global_varibles
        //private Functions.PageFunctions.NavigationService _navigationService;
        //public Classes.Class_Events.Class_Current_Page page_class = new Classes.Class_Events.Class_Current_Page();
        #endregion

        #region local_varibles
        Functions.IOFunctions ioFunctions = new Functions.IOFunctions();
        double newWindowHeight;
        double newWindowWidth;
        Classes.Class_types.WindowState maximilize_window_;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing; //Счётчик закрытия
            //page_class.Current_page_changed += pageChangedHandler;
            //page_class.current_page = Classes.Class_types.Pages.Page_None;
            init();
        }

        //Инициализация
        private void init()
        {
            ioFunctions.chkFirstStart("settings", "settings.json");
            Classes.Class_JSON_Setting setting = ioFunctions.openJSONSetting();
            WindowSizeState(setting.maximilize_window);
            Application.Current.MainWindow.Height = setting.size_window[0];
            Application.Current.MainWindow.Width = setting.size_window[1];
            newWindowHeight = setting.size_window[0];
            newWindowWidth = setting.size_window[1];
            main_frame.NavigationService.Navigate(new Pages.Page_select_course());
            //_navigationService = new Functions.PageFunctions.NavigationService(main_frame);
           // _navigationService.NavigateToPage(Classes.Class_types.Pages.Page_Start);
           // page_class.current_page = _navigationService.currentPage;
        }

        //Событие изменения переменной current_page
        private void pageChangedHandler(object sender, Classes.Class_types.Pages page)
        {
            switch (page) {
                case Classes.Class_types.Pages.Page_None:
                    btn_file.IsEnabled = false;
                    btn_main.IsEnabled = false;
                    btn_insert.IsEnabled = false;
                    break;
                case Classes.Class_types.Pages.Page_Start:
                    btn_file.IsEnabled = false;
                    btn_main.IsEnabled = false;
                    btn_insert.IsEnabled = false;
                    break;
                case Classes.Class_types.Pages.Page_Course:
                    btn_file.IsEnabled = false;
                    btn_main.IsEnabled = false;
                    btn_insert.IsEnabled = false;
                    break;
                case Classes.Class_types.Pages.Page_Main:
                    btn_file.IsEnabled = true;
                    btn_main.IsEnabled = true;
                    btn_insert.IsEnabled = true;
                    break;
            }
        }
        //Собитие закрытия
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ioFunctions.saveJSONSetting(
                new Classes.Class_JSON_Setting{
                    maximilize_window = maximilize_window_,
                    size_window = new List<double>() {newWindowHeight, newWindowWidth}
                });
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

        //Событие изменение состояния окна
        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (this.WindowState) 
            {
                case WindowState.Maximized:
                    if (img_btn_max_min.Source != new BitmapImage(new Uri("pack://application:,,,/Icons/btn_restore.png")))
                    {
                        main_grid.Margin = new Thickness(5);
                        maximilize_window_ = Classes.Class_types.WindowState.True;
                        img_btn_max_min.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/btn_restore.png")); 
                    }
                    break;

                case WindowState.Normal:
                    if (img_btn_max_min.Source != new BitmapImage(new Uri("pack://application:,,,/Icons/btn_maximilize.png")))
                    {
                        main_grid.Margin = new Thickness(0);
                        maximilize_window_ = Classes.Class_types.WindowState.False;
                        img_btn_max_min.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/btn_maximilize.png"));
                    }
                    break;
            }
        }

        //Изменение состояния окна
        private void WindowSizeState(Classes.Class_types.WindowState key = Classes.Class_types.WindowState.None)
        {
            if (this.WindowState == WindowState.Maximized || key == Classes.Class_types.WindowState.False)
            {
                this.WindowState = WindowState.Normal;
                main_grid.Margin = new Thickness(0);
                maximilize_window_ = Classes.Class_types.WindowState.False;
                img_btn_max_min.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/btn_maximilize.png"));
            }
            else if (this.WindowState == WindowState.Normal || key == Classes.Class_types.WindowState.True)
            {
                this.WindowState = WindowState.Maximized;
                main_grid.Margin = new Thickness(5);
                maximilize_window_ = Classes.Class_types.WindowState.True;
                img_btn_max_min.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/btn_restore.png"));
            }
        }

        //Собитие изменения размеров окна
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            newWindowHeight = e.NewSize.Height;
            newWindowWidth = e.NewSize.Width;
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
