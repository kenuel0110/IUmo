using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IUmo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        #region global_varibles
        //private Functions.PageFunctions.NavigationService _navigationService;
        //public Classes.Class_Events.Class_Current_Page page_class = new Classes.Class_Events.Class_Current_Page();
        public Microsoft.Office.Interop.Excel.Application _excel;
        public Workbook _excel_book;
        #endregion

        #region local_varibles
        Functions.IOFunctions ioFunctions = new Functions.IOFunctions();
        double newWindowHeight;
        double newWindowWidth;

        double newWindowPosX;
        double newWindowPosY;

        Classes.Class_types.WindowState maximilize_window_;
        bool setdialogResult_addLesson;
        bool setdialogResult_addGroup;
        private TaskCompletionSource<bool> tcs;

        int column_changes = 3;
        int column_title = 4;
        int column_type = 5;
        int column_teacher = 6;
        int column_cabinet = 7;

        List<int> numerator_monday = new List<int>() { 7, 9, 11, 13, 15, 17 };
        List<int> numerator_tuesday = new List<int>() { 20, 22, 24, 26, 28, 30 };
        List<int> numerator_wednesday = new List<int>() { 33, 35, 37, 39, 41, 43 };
        List<int> numerator_friday = new List<int>() { 59, 61, 63, 65, 67, 69 };
        List<int> numerator_saturday = new List<int>() { 72, 74, 76, 78, 80, 82 };

        List<int> denominator_monday = new List<int>() { 8, 10, 12, 14, 16, 18 };
        List<int> denominator_tuesday = new List<int>() { 21, 23, 25, 27, 29, 31 };
        List<int> denominator_wednesday = new List<int>() { 34, 36, 38, 40, 42, 44 };
        List<int> denominator_friday = new List<int>() { 60, 62, 64, 66, 68, 70 };
        List<int> denominator_saturday = new List<int>() { 73, 75, 77, 79, 81, 83 };

        Worksheet current_worksheet;
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
            _excel = new Microsoft.Office.Interop.Excel.Application();
            ioFunctions.chkFirstStart("settings", "settings.json");
            ioFunctions.chkFirstStart("settings", "teachers.json");
            ioFunctions.createTemp();
            Classes.Class_JSON_Setting setting = ioFunctions.openJSONSetting();
            WindowSizeState(setting.maximilize_window);
            System.Windows.Application.Current.MainWindow.Height = setting.size_window[0];
            System.Windows.Application.Current.MainWindow.Width = setting.size_window[1];

            this.Left = setting.position_window[0];
            this.Top = setting.position_window[1];

            newWindowHeight = setting.size_window[0];
            newWindowWidth = setting.size_window[1];

            newWindowPosX = setting.position_window[0];
            newWindowPosY = setting.position_window[1];

            main_frame.NavigationService.Navigate(new Pages.Page_start());
            //_navigationService = new Functions.PageFunctions.NavigationService(main_frame);
            // _navigationService.NavigateToPage(Classes.Class_types.Pages.Page_Start);
            // page_class.current_page = _navigationService.currentPage;
        }

        

        //Публичные функции связанные с EXCEl

            //новый документ
        public string new_document(string path)
        {
            string state = ioFunctions.copyTemplate(path);
            try
            {
                _excel_book = _excel.Workbooks.Open(path);
            }
            catch (Exception ex)
            {
                state += $" {ex.Message}";
            }
            return state;
        }

        //создание и подготовка нового документа
        public void init_new_document(int course, List<KeyValuePair<String, String>> groups) 
        {
            Worksheet templateSheet = _excel_book.Sheets[1];
            string cell2_string = "";
            switch (course) 
            {
                case 1:
                    if (DateTime.Now.Month >= 9 && DateTime.Now.Month <= 12)
                        cell2_string = $"1 СЕМЕСТР {DateTime.Now.Year}-{DateTime.Now.Year + 1} УЧЕБНОГО ГОДА";
                    else if (DateTime.Now.Month >= 1 && DateTime.Now.Month <= 9)
                        cell2_string = $"2 СЕМЕСТР {DateTime.Now.Year - 1}-{DateTime.Now.Year} УЧЕБНОГО ГОДА";

                    foreach (KeyValuePair<String, String> sheetName in groups)
                    {
                        templateSheet.Copy(After: _excel_book.Sheets[_excel_book.Sheets.Count]);
                        Worksheet copiedSheet = (Worksheet)_excel_book.Sheets[_excel_book.Sheets.Count];

                        
                        Range cell1 = copiedSheet.Cells[2, 4];
                        cell1.Value = "ОЧНАЯ ФОРМА ОБУЧЕНИЯ 1 КУРС";
                        Range cell2 = copiedSheet.Cells[3, 4];
                        cell2.Value = cell2_string;
                        Range cell3 = copiedSheet.Cells[5, 4];
                        cell3.Value = $"{sheetName.Key} ({sheetName.Value})";
                        Range cell4 = copiedSheet.Cells[3, 6];
                        cell4.Value = $"″____″___________ {DateTime.Now.Year} г";
                        copiedSheet.Name = sheetName.Value;

                        _excel_book.Save();
                    }
                    break;

                case 2:
                    if (DateTime.Now.Month >= 9 && DateTime.Now.Month <= 12)
                        cell2_string = $"3 СЕМЕСТР {DateTime.Now.Year}-{DateTime.Now.Year + 1} УЧЕБНОГО ГОДА";
                    else if (DateTime.Now.Month >= 1 && DateTime.Now.Month <= 9)
                        cell2_string = $"4 СЕМЕСТР {DateTime.Now.Year - 1}-{DateTime.Now.Year} УЧЕБНОГО ГОДА";

                    foreach (KeyValuePair<String, String> sheetName in groups)
                    {
                        templateSheet.Copy(After: _excel_book.Sheets[_excel_book.Sheets.Count]);
                        Worksheet copiedSheet = (Worksheet)_excel_book.Sheets[_excel_book.Sheets.Count];


                        Range cell1 = copiedSheet.Cells[2, 4];
                        cell1.Value = "ОЧНАЯ ФОРМА ОБУЧЕНИЯ 2 КУРС";
                        Range cell2 = copiedSheet.Cells[3, 4];
                        cell2.Value = cell2_string;
                        Range cell3 = copiedSheet.Cells[5, 4];
                        cell3.Value = $"{sheetName.Key} ({sheetName.Value})";
                        Range cell4 = copiedSheet.Cells[3, 6];
                        cell4.Value = $"'____'___________ {DateTime.Now.Year} г";
                        copiedSheet.Name = sheetName.Value;

                        _excel_book.Save();
                    }
                    break;

                case 3:
                    if (DateTime.Now.Month >= 9 && DateTime.Now.Month <= 12)
                        cell2_string = $"5 СЕМЕСТР {DateTime.Now.Year}-{DateTime.Now.Year + 1} УЧЕБНОГО ГОДА";
                    else if (DateTime.Now.Month >= 1 && DateTime.Now.Month <= 9)
                        cell2_string = $"6 СЕМЕСТР {DateTime.Now.Year - 1}-{DateTime.Now.Year} УЧЕБНОГО ГОДА";

                    foreach (KeyValuePair<String, String> sheetName in groups)
                    {
                        templateSheet.Copy(After: _excel_book.Sheets[_excel_book.Sheets.Count]);
                        Worksheet copiedSheet = (Worksheet)_excel_book.Sheets[_excel_book.Sheets.Count];


                        Range cell1 = copiedSheet.Cells[2, 4];
                        cell1.Value = "ОЧНАЯ ФОРМА ОБУЧЕНИЯ 3 КУРС";
                        Range cell2 = copiedSheet.Cells[3, 4];
                        cell2.Value = cell2_string;
                        Range cell3 = copiedSheet.Cells[5, 4];
                        cell3.Value = $"{sheetName.Key} ({sheetName.Value})";
                        Range cell4 = copiedSheet.Cells[3,6];
                        cell4.Value = $"'____'___________ {DateTime.Now.Year} г";
                        copiedSheet.Name = sheetName.Value;

                        _excel_book.Save();
                    }
                    break;
                    // и т.д. пока проверим
            }
            _excel_book.Save();
        }

        //открытие документа
        public string open_document(string path)
        {
            string state = "";
            try
            {
                _excel_book = _excel.Workbooks.Open(path);
            }
            catch (Exception ex)
            {
                state = ex.Message;
            }
            return state;
        }

        public List<string> get_Document_sheets() 
        {
            List<string> sheetNames = new List<string>();

            foreach (Worksheet sheet in _excel_book.Sheets)
            {
                sheetNames.Add(sheet.Name);
            }

            return sheetNames;
        }

        public void set_current_sheet(string current) 
        {
            current_worksheet = _excel_book.Sheets[current];
        }

        //_______________________________________________________________________


        //POPUPS
        public void showLoading() 
        {
            blurBackground();
            popup_window.Visibility = Visibility.Visible;
            popup_frame.NavigationService.Navigate(new Popup.Popup_loading());
        }

        public void hideLoading()
        {
            deblurBackground();
            popup_window.Visibility = Visibility.Hidden;
            if (popup_frame.NavigationService.CanGoBack)
                popup_frame.NavigationService.GoBack();
        }

        /// <summary>
        ///Вспомогательная функция для реализации popup как всплывающего окна
        /// </summary>
        internal void SetDialogResult_AddLesson(bool result)
        {
            setdialogResult_addLesson = result;
            if (result == true)
                tcs.SetResult(true);
            else
                tcs.SetResult(false);
            tcs.TrySetCanceled();
        }

        internal void SetDialogResult_AddGroup(bool result)
        {
            setdialogResult_addGroup = result;
            if (result == true)
                tcs.SetResult(true);
            else
                tcs.SetResult(false);
            tcs.TrySetCanceled();
        }


        //Добавить пару PopUP
        public async Task<bool> add_new_lesson()
        {
            blurBackground();
            popup_window.Visibility = Visibility.Visible;
            Popup.Popup_Add_Lesson add_lesson_page = new Popup.Popup_Add_Lesson();
            popup_frame.NavigationService.Navigate(add_lesson_page);
            tcs = new TaskCompletionSource<bool>();

            // Ждем, пока переменная не изменится
            bool result = await tcs.Task;
            return result;
        }


        //Добавить группу PopUP
        public async Task<bool> add_new_group()
        {
            blurBackground();
            popup_window.Visibility = Visibility.Visible;
            Popup.Popup_Add_Group add_lesson_page = new Popup.Popup_Add_Group();
            popup_frame.NavigationService.Navigate(add_lesson_page);
            tcs = new TaskCompletionSource<bool>();

            // Ждем, пока переменная не изменится
            bool result = await tcs.Task;
            return result;
        }

        //Событие изменения переменной current_page
        /*private void pageChangedHandler(object sender, Classes.Class_types.Pages page)
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
        }*/
        //Собитие закрытия
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ioFunctions.removeTemp();
            ioFunctions.saveJSONSetting(
                new Classes.Class_JSON_Setting{
                    maximilize_window = maximilize_window_,
                    size_window = new List<double>() {newWindowHeight, newWindowWidth},
                    position_window = new List<double>() { newWindowPosX, newWindowPosY }
                });
            try
            {
                _excel_book.Close();
                _excel.Quit();
            }
            catch { }
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
                    System.Windows.Application.Current.MainWindow.DragMove();
                }
        }

        //Главные кнопки окна
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
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

        //Popup
        //Открыть popup с информацией
        public void showInfoPopup(string message, Classes.Class_types.Info_IMG image = Classes.Class_types.Info_IMG.img_None)
        {
            popup_window.Visibility = Visibility.Visible;
            popup_frame.NavigationService.Navigate(new Popup.Popup_show_info(message, image));
            blurBackground();
        }
        public void blurBackground()
        {
            border_shadow.Visibility = Visibility.Visible;
            BlurEffect blurEffect = new BlurEffect();
            blurEffect.Radius = 0;
            main_frame.Effect = blurEffect;

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 5;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            DoubleAnimation animation_shadow = new DoubleAnimation();
            animation_shadow.From = 0.0;
            animation_shadow.To = 0.4;
            animation_shadow.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            storyboard.Children.Add(animation_shadow);

            Storyboard.SetTarget(blurEffect, main_frame);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.Effect).(BlurEffect.Radius)"));

            Storyboard.SetTarget(animation_shadow, border_shadow);
            Storyboard.SetTargetProperty(animation_shadow, new PropertyPath("Opacity"));

            // Запустите анимацию
            storyboard.Begin(main_frame);
            border_shadow.Visibility = Visibility.Visible;
        }

        public void deblurBackground()
        {
            BlurEffect blurEffect = new BlurEffect();
            blurEffect.Radius = 5;
            main_frame.Effect = blurEffect;

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 5;
            animation.To = 0;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            DoubleAnimation animation_shadow = new DoubleAnimation();
            animation_shadow.From = 0.4;
            animation_shadow.To = 0.0;
            animation_shadow.Duration = new Duration(TimeSpan.FromSeconds(0.3));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            storyboard.Children.Add(animation_shadow);

            Storyboard.SetTarget(blurEffect, main_frame);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.Effect).(BlurEffect.Radius)"));

            Storyboard.SetTarget(animation_shadow, border_shadow);
            Storyboard.SetTargetProperty(animation_shadow, new PropertyPath("Opacity"));

            // Запустите анимацию
            storyboard.Begin(main_frame);
            border_shadow.Visibility = Visibility.Hidden;
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

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            newWindowPosX = this.Left;
            newWindowPosY = this.Top;
        }
    }
}
