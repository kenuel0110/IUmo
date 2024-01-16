using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IUmo.Pages
{
    /// <summary>
    /// Interaction logic for Page_start.xaml
    /// </summary>
    public partial class Page_start : Page
    {
        #region local_varibles
        private MainWindow mainWindow = App.Current.MainWindow as MainWindow;
        Functions.IOFunctions ioFunctions = new Functions.IOFunctions();
        List<Classes.Class_JSON_RecenFiles> recent_files = new List<Classes.Class_JSON_RecenFiles>();
        Classes.Class_JSON_Temp temp_file;
        #endregion

        public Page_start()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            
            mainWindow.btn_file.IsEnabled = false;
            mainWindow.btn_main.IsEnabled = false;
            mainWindow.btn_insert.IsEnabled = false;

            ioFunctions.chkFirstStart("settings", "recent_files.json");
            recent_files = ioFunctions.openJSONRecentFiles(this);
            if (recent_files != null)
                recent_files.ForEach(file => lv_recent_files.Items.Add(file.name));

            temp_file = ioFunctions.openJSONTemp();
        }

        private void btn_new_document_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Книга Excel (*.xlsx)|*.xlsx|Все файлы (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                string extension = System.IO.Path.GetExtension(filePath);
                if (extension == ".xlsx")
                {
                    ioFunctions.saveJSONRecentFiles(new Classes.Class_JSON_RecenFiles() { name = System.IO.Path.GetFileName(filePath), path = filePath });
                    temp_file.path = filePath;
                    temp_file.tempType = Classes.Class_types.TempType.Temp_new;
                    ioFunctions.saveTemp(temp_file);
                    this.NavigationService.Navigate(new Pages.Page_select_course());
                }
                else
                {
                    // Файл не имеет расширения .xlsx, обработка ошибки или вывод сообщения
                    popup_f_info.Visibility = Visibility.Visible;
                    lbl_info_popup.Text = "Файл, который вы пытаетесь создать, не является Excel книгой!";
                    //MessageBox.Show("Файл, который вы пытаетесь создать, не является Excel книгой!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btn_open_document_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Книга Excel (*.xlsx)|*.xlsx|Все файлы (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string extension = System.IO.Path.GetExtension(filePath);
                if (extension == ".xlsx")
                {
                    ioFunctions.saveJSONRecentFiles(new Classes.Class_JSON_RecenFiles() { name = System.IO.Path.GetFileName(filePath), path = filePath });
                    temp_file.path = filePath;
                    temp_file.tempType = Classes.Class_types.TempType.Temp_open;
                    ioFunctions.saveTemp(temp_file);
                    this.NavigationService.Navigate(new Pages.Page_select_course());
                }
                else
                {
                    // Файл не имеет расширения .xlsx, обработка ошибки или вывод сообщения
                    popup_f_info.Visibility = Visibility.Visible;
                    lbl_info_popup.Text = "Файл, который вы пытаетесь открыть, не является Excel книгой!";
                    //MessageBox.Show("Файл, который вы пытаетесь открыть, не является Excel книгой!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void border_dragndrop_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
                string extension = System.IO.Path.GetExtension(file[0]);
                if (extension == ".xlsx")
                {
                    img_status_drop.Source = new BitmapImage(new Uri("pack://application:,,,/Illustartions/img_no_dragndrop.png"));
                    border_dragndrop.Background = Brushes.Transparent;
                    border_dragndrop.Opacity = 1;

                    ioFunctions.saveJSONRecentFiles(new Classes.Class_JSON_RecenFiles() { name = System.IO.Path.GetFileName(file[0]), path = file[0] });
                    temp_file.path = file[0];
                    temp_file.tempType = Classes.Class_types.TempType.Temp_open;
                    ioFunctions.saveTemp(temp_file);
                    this.NavigationService.Navigate(new Pages.Page_select_course());
                }
                else
                {
                    // Файл не имеет расширения .xlsx, обработка ошибки или вывод сообщения
                    img_status_drop.Source = new BitmapImage(new Uri("pack://application:,,,/Illustartions/img_no_dragndrop.png"));
                    border_dragndrop.Background = Brushes.Transparent;
                    border_dragndrop.Opacity = 1;
                    popup_f_info.Visibility = Visibility.Visible;
                    lbl_info_popup.Text = "Файл, который вы пытаетесь открыть, не является Excel книгой!";
                    //MessageBox.Show("Файл, который вы пытаетесь открыть, не является Excel книгой!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void border_dragndrop_DragEnter(object sender, DragEventArgs e)
        {
            img_status_drop.Source = new BitmapImage(new Uri("pack://application:,,,/Illustartions/img_dragndrop.png"));
            border_dragndrop.Background = new SolidColorBrush(Color.FromRgb(8, 117, 88));
            border_dragndrop.Opacity = 0.5;
        }

        private void border_dragndrop_DragLeave(object sender, DragEventArgs e)
        {
            img_status_drop.Source = new BitmapImage(new Uri("pack://application:,,,/Illustartions/img_no_dragndrop.png"));
            border_dragndrop.Background = Brushes.Transparent;
            border_dragndrop.Opacity = 1;
        }

        private void btn_close_info_Click(object sender, RoutedEventArgs e)
        {
            popup_f_info.Visibility = Visibility.Hidden;
        }

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Pages.Page_main());
        }*/
    }
}
