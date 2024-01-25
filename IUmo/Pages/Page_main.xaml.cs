using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;
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
            new KeyValuePair<String, String>("ЧЕТВЕРГ", "/Icons\\ic_thursday.png"),
            new KeyValuePair<String, String>("ПЯТНИЦА", "/Icons\\ic_friday.png"),
            new KeyValuePair<String, String>("СУББОТА", "/Icons\\ic_saturday.png")
        };

        // Создайте коллекции объектов для источников данных

        ObservableCollection<Classes.Group_data> list_group = new ObservableCollection<Classes.Group_data>();
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

                int i = 0;
                foreach (var item in list_lesson.ToList())
                {
                    i++;
                    if (item is Classes.Item_New_Lesson)
                    {
                        Classes.Item_New_Lesson item_lesson = (Classes.Item_New_Lesson)item;
                        item_lesson.number = i;
                        list_lesson.RemoveAt(i - 1);
                        list_lesson.Insert(i - 1, item_lesson);
                    }
                    else if (item is Classes.Item_Empty_Lesson)
                    {
                        Classes.Item_Empty_Lesson item_empty = (Classes.Item_Empty_Lesson)item;
                        item_empty.number = i;
                        list_lesson.RemoveAt(i - 1);
                        list_lesson.Insert(i - 1, item_empty);
                    }
                    else if (item is Classes.Item_Group)
                    {
                        Classes.Item_Group item_group = (Classes.Item_Group)item;
                        item_group.number = i;
                        list_lesson.RemoveAt(i - 1);
                        list_lesson.Insert(i - 1, item_group);
                    }
                }
            }

        }

        private void DeleteButtonEmpty_Click(object sender, RoutedEventArgs e)
        {
            Classes.Item_Empty_Lesson lesson = (sender as Button)?.DataContext as Classes.Item_Empty_Lesson;
            if (lesson != null)
            {
                list_lesson.Remove(lesson);

                int i = 0;
                foreach (var item in list_lesson.ToList())
                {
                    i++;
                    if (item is Classes.Item_New_Lesson)
                    {
                        Classes.Item_New_Lesson item_lesson = (Classes.Item_New_Lesson)item;
                        item_lesson.number = i;
                        list_lesson.RemoveAt(i - 1);
                        list_lesson.Insert(i - 1, item_lesson);
                    }
                    else if (item is Classes.Item_Empty_Lesson)
                    {
                        Classes.Item_Empty_Lesson item_empty = (Classes.Item_Empty_Lesson)item;
                        item_empty.number = i;
                        list_lesson.RemoveAt(i - 1);
                        list_lesson.Insert(i - 1, item_empty);
                    }
                    else if (item is Classes.Item_Group)
                    {
                        Classes.Item_Group item_group = (Classes.Item_Group)item;
                        item_group.number = i;
                        list_lesson.RemoveAt(i - 1);
                        list_lesson.Insert(i - 1, item_group);
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Group_data group_ = (sender as Button)?.DataContext as Classes.Group_data;
            if (group_ != null)
            {
                List<object> _list_lesson = new List<object>(list_lesson);
                int index = _list_lesson.FindIndex(item =>
                {
                    if (item is Classes.Item_Group itemGroup)
                    {
                        return itemGroup.groups.Any(_item => _item == group_);
                    }
                    return false;
                });

                list_group.Remove(group_);
                if (list_group.Count == 0 && index != -1) 
                {
                    list_lesson.RemoveAt(index);
                    int i = 0;

                    foreach (var item in list_lesson.ToList())
                    {
                        i++;
                        if (item is Classes.Item_New_Lesson)
                        {
                            Classes.Item_New_Lesson item_lesson = (Classes.Item_New_Lesson)item;
                            item_lesson.number = i;
                            list_lesson.RemoveAt(i - 1);
                            list_lesson.Insert(i - 1, item_lesson);
                        }
                        else if (item is Classes.Item_Empty_Lesson)
                        {
                            Classes.Item_Empty_Lesson item_empty = (Classes.Item_Empty_Lesson)item;
                            item_empty.number = i;
                            list_lesson.RemoveAt(i - 1);
                            list_lesson.Insert(i - 1, item_empty);
                        }
                        else if (item is Classes.Item_Group)
                        {
                            Classes.Item_Group item_group = (Classes.Item_Group)item;
                            item_group.number = i;
                            list_lesson.RemoveAt(i - 1);
                            list_lesson.Insert(i - 1, item_group);
                        }
                    }
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
            /*list_lesson.Add(new Classes.Item_Empty_Lesson() { number = 1 });
            list_lesson.Add(new Classes.Item_New_Lesson() { number = 1, title = "ОСНОВЫ РОССИЙСКОЙ ГОСУДАРСТВЕННОСТИ", teacher = "АНИСИМОВА В.А.", cabinet="217", type="ПЗ", editions = new List<string>() {"Прикол", "Прикол2" } });
            
            list_group.Add(new Classes.Group_data() { number = 1, title = "ОСНОВЫ РОССИЙСКОЙ ГОСУДАРСТВЕННОСТИ", teacher = "АНИСИМОВА В.А.", cabinet = "217", type = "ПЗ", editions = new List<string>() { "Прикол", "Прикол2" } });
            list_group.Add(new Classes.Group_data() { number = 2, title = "ФИЗИЧЕСКАЯ КУЛЬТУРА И СПОРТ", teacher = "КУЛАКОВ И.И.", cabinet = "Акт. зал", type = "Л", editions = new List<string>() { "Прикол", "456" } });
            
            list_lesson.Add(new Classes.Item_Group() { number = 1, groups = list_group });
            list_lesson.Add(new Classes.Item_New_Thursday() {});*/
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

        private async void btn_add_lesson_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool result = await mainWindow.add_new_lesson();
            if (result == true)
            {
                temp_file = ioFunctions.openJSONTemp();
                var temp_fileString = temp_file.new_string.ToString(); // Предполагается, что new_string содержит JSON-строку
                var new_string = JsonSerializer.Deserialize<Classes.Add_Item_Lesson>(temp_fileString);
                if (new_string.empty_lesson == true)
                    list_lesson.Add(new Classes.Item_Empty_Lesson() { number = list_lesson.Count + 1 });
                else 
                {
                    list_lesson.Add(
                        new Classes.Item_New_Lesson() { 
                            number = list_lesson.Count + 1,
                            title = new_string.title,
                            teacher = new_string.teacher,
                            cabinet = new_string.cabinet,
                            type = new_string.type,
                            editions = new_string.editions
                        });
                }
            }
        }
    }
}
