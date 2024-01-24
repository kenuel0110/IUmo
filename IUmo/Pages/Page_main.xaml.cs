using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace IUmo.Pages
{
    /// <summary>
    /// Interaction logic for Page_Main.xaml
    /// </summary>
    public partial class Page_main : Page
    {
        #region local_varibles
        private MainWindow mainWindow = App.Current.MainWindow as MainWindow;
        private Functions.IOFunctions ioFunctions = new Functions.IOFunctions();
        public ObservableCollection<KeyValuePair<String, String>> dayofweeks_list = new ObservableCollection<KeyValuePair<String, String>>(){
            new KeyValuePair<String, String>("ПОНЕДЕЛЬНИК", "/Icons\\ic_monday.png"),
            new KeyValuePair<String, String>("ВТОРНИК", "/Icons\\ic_tuesday.png"),
            new KeyValuePair<String, String>("СРЕДА", "/Icons\\ic_wednesday.png"),
            //new KeyValuePair<String, String>("ЧЕТВЕРГ", "/Icons\\ic_thursday.png"),
            new KeyValuePair<String, String>("ПЯТНИЦА", "/Icons\\ic_friday.png"),
            new KeyValuePair<String, String>("СУББОТА", "/Icons\\ic_saturday.png")
        };

        // Создайте коллекции объектов для источников данных

        List<Classes.Group_data> list_group = new List<Classes.Group_data>();
        ObservableCollection<object> list_lesson = new ObservableCollection<object>();

        Classes.Class_JSON_Temp temp_file;
        #endregion
        public Page_main()
        {
            InitializeComponent();
            init();
        }

        private void DeleteButtonLesson_Click(object sender, RoutedEventArgs e)
        {
            Classes.Item_New_Lesson lesson = (sender as Button)?.DataContext as Classes.Item_New_Lesson;
            if (lesson != null)
            {
                list_lesson.Remove(lesson);
            }

        }

        private void DeleteButtonEmpty_Click(object sender, RoutedEventArgs e)
        {
            Classes.Item_Empty_Lesson lesson = (sender as Button)?.DataContext as Classes.Item_Empty_Lesson;
            if (lesson != null)
            {
                list_lesson.Remove(lesson);
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Group_data lesson = (sender as Button)?.DataContext as Classes.Group_data;
            if (lesson != null)
            {
                list_group.Remove(lesson);
                if (list_group.Count == 0)
                {
                    list_lesson.RemoveAt(list_lesson.Count()-1);
                }
            }

        }

        private void init()
        {
            mainWindow.btn_file.IsEnabled = true;
            mainWindow.btn_main.IsEnabled = true;
            mainWindow.btn_insert.IsEnabled = true;

            temp_file = ioFunctions.openJSONTemp();
            mainWindow.title_window.Content = System.IO.Path.GetFileName(temp_file.path);
            lv_day_of_weeks.ItemsSource = dayofweeks_list;

            //действия с lv_lessons
            ObservableCollection<object> lesson = new ObservableCollection<object>();
            lv_lessons.DataContext = lesson;
            lv_lessons.ItemsSource = list_lesson;
            list_lesson.Add(new Classes.Item_Empty_Lesson() { number = 1 });
            list_lesson.Add(new Classes.Item_New_Lesson() { number = 1, title = "ОСНОВЫ РОССИЙСКОЙ ГОСУДАРСТВЕННОСТИ", teacher = "АНИСИМОВА В.А.", cabinet="217", type="ПЗ", editions = new List<string>() {"Прикол", "Прикол2" } });
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem selectedTab = (TabItem)tabControl_num_den.SelectedItem;
            string pageName = selectedTab.Header.ToString();
            // Здесь можно выполнить необходимые действия при изменении выбранной вкладки
        }

        private void ComboBox_Center_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            combobox_list.BorderBrush = new SolidColorBrush(Color.FromRgb(2, 37, 28));
        }

    }
}
