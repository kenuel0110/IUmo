using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

        ObservableCollection<object> list_lesson = new ObservableCollection<object>();
        List<string> list_sheets = new List<string>();
        string preview_cb_sheet;

        Classes.Class_types.DayOfWeek dayOfWeek = Classes.Class_types.DayOfWeek.None;
        Classes.Class_types.NumDen current_numden = Classes.Class_types.NumDen.NumDen_None;

        Classes.Class_JSON_Temp temp_file;
        int curent_item_group = -1;
        #endregion
        public Page_main()
        {
            InitializeComponent();
            init();
        }

        private void fixLessonNumeration()
        {
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

        private void DeleteButtonLesson_Click(object sender, RoutedEventArgs e)
        {
            Classes.Item_New_Lesson lesson = (sender as Button)?.DataContext as Classes.Item_New_Lesson;
            if (lesson != null)
            {
                list_lesson.Remove(lesson);
                fixLessonNumeration();
            }

        }

        private void DeleteButtonEmpty_Click(object sender, RoutedEventArgs e)
        {
            Classes.Item_Empty_Lesson lesson = (sender as Button)?.DataContext as Classes.Item_Empty_Lesson;
            if (lesson != null)
            {
                list_lesson.Remove(lesson);
                fixLessonNumeration();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Group_data group_ = (sender as Button)?.DataContext as Classes.Group_data;
            if (group_ != null)
            {
                var item_Group = list_lesson[curent_item_group] as Classes.Item_Group;
                if (item_Group != null)
                {
                    var _list_group = item_Group.groups;

                    _list_group.Remove(group_);

                    int i = 0;

                    foreach (var group in _list_group.ToList())
                    {
                        i++;

                        Classes.Group_data item_group = (Classes.Group_data)group;
                        item_group.number = i;
                        _list_group.RemoveAt(i - 1);
                        _list_group.Insert(i - 1, item_group);
                    }

                    if (_list_group.Count == 0)
                    {
                        list_lesson.RemoveAt(curent_item_group);
                        fixLessonNumeration();
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

            list_sheets = mainWindow.get_Document_sheets();

            if (temp_file.tempType == Classes.Class_types.TempType.Temp_new)
                list_sheets.RemoveAt(0);

            combobox_list.ItemsSource = list_sheets;
            combobox_list.SelectedIndex = 0;

            mainWindow.set_current_sheet(list_sheets[combobox_list.SelectedIndex]);
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
            temp_file = ioFunctions.openJSONTemp();
            switch (pageName)
            {
                case "ЧИСЛИТЕЛЬ":
                    if (current_numden != Classes.Class_types.NumDen.NumDen_Numerator)
                    {
                        if (list_sheets.Count > 0)
                        {
                            btn_add_lesson.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#272727"));
                            if (list_lesson.Count > 0)
                            {
                                ioFunctions.saveJSONDayOfWeek(list_lesson, dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden);
                            }
                            list_lesson.Clear();
                            current_numden = Classes.Class_types.NumDen.NumDen_Numerator;
                            //MessageBox.Show(ioFunctions.openJSONDayOfWeek(dayOfWeek, current_numden).Count.ToString());

                            if (ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden) != null)
                            {
                                foreach (var item in ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden).OrderBy(x =>
                                {
                                    if (x is Classes.Item_New_Lesson item)
                                        return item.number;
                                    else if (x is Classes.Item_Group group)
                                        return group.number;
                                    else if (x is Classes.Item_Empty_Lesson emptyLesson)
                                        return emptyLesson.number;
                                    else if (x is Classes.Item_New_Thursday thursday)
                                        return -1; // Переместите 'Item_New_Thursday' в начало коллекции
                                    else
                                        return 0; // Если класс неизвестен, сохраните текущий порядок
                                }))
                                {
                                    list_lesson.Add(item);
                                }
                            }
                        }
                        current_numden = Classes.Class_types.NumDen.NumDen_Numerator;
                    }
                    break;

                case "ЗНАМЕНАТЕЛЬ":
                    if (current_numden != Classes.Class_types.NumDen.NumDen_Denominator)
                    {
                        if (list_sheets.Count > 0)
                        {
                            btn_add_lesson.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1c242a"));
                            if (list_lesson.Count > 0)
                            {
                                ioFunctions.saveJSONDayOfWeek(list_lesson, dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden);
                            }
                            list_lesson.Clear();
                            current_numden = Classes.Class_types.NumDen.NumDen_Denominator;
                            if (ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden) != null)
                            {
                                foreach (var item in ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden).OrderBy(x =>
                                {
                                    if (x is Classes.Item_New_Lesson item)
                                        return item.number;
                                    else if (x is Classes.Item_Group group)
                                        return group.number;
                                    else if (x is Classes.Item_Empty_Lesson emptyLesson)
                                        return emptyLesson.number;
                                    else if (x is Classes.Item_New_Thursday thursday)
                                        return -1; // Переместите 'Item_New_Thursday' в начало коллекции
                                    else
                                        return 0; // Если класс неизвестен, сохраните текущий порядок
                                }))
                                {
                                    list_lesson.Add(item);
                                }
                            }
                        }
                        current_numden = Classes.Class_types.NumDen.NumDen_Denominator;
                    }
                    break;
            }
        }

        private void ComboBox_Center_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            combobox_list.BorderBrush = new SolidColorBrush(Color.FromRgb(2, 37, 28));

            if (preview_cb_sheet != null && preview_cb_sheet != list_sheets[combobox_list.SelectedIndex])
                ioFunctions.saveJSONDayOfWeek(list_lesson, dayOfWeek, preview_cb_sheet, current_numden);

            list_lesson.Clear();

            TabItem selectedTab = (TabItem)tabControl_num_den.SelectedItem;
            string pageName = selectedTab.Header.ToString();
            if (pageName == "ЧИСЛИТЕЛЬ")
                current_numden = Classes.Class_types.NumDen.NumDen_Numerator;
            else if (pageName == "ЗНАМЕНАТЕЛЬ")
                current_numden = Classes.Class_types.NumDen.NumDen_Denominator;

            lv_day_of_weeks.SelectedIndex = 0;
            if (preview_cb_sheet != null && preview_cb_sheet != list_sheets[combobox_list.SelectedIndex]) 
            {
                try
                {
                    if (ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden) != null)
                    {
                        foreach (var item in ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden).OrderBy(x =>
                        {
                            if (x is Classes.Item_New_Lesson item)
                                return item.number;
                            else if (x is Classes.Item_Group group)
                                return group.number;
                            else if (x is Classes.Item_Empty_Lesson emptyLesson)
                                return emptyLesson.number;
                            else if (x is Classes.Item_New_Thursday thursday)
                                return -1; // Переместите 'Item_New_Thursday' в начало коллекции
                            else
                                return 0; // Если класс неизвестен, сохраните текущий порядок
                        }))
                        {
                            list_lesson.Add(item);
                        }
                    }
                }
                catch { }
            }
            preview_cb_sheet = list_sheets[combobox_list.SelectedIndex];
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
                {
                    if (current_numden == Classes.Class_types.NumDen.NumDen_Numerator)
                        list_lesson.Add(new Classes.Item_Empty_Lesson() { number = list_lesson.Count + 1 });
                    else if (current_numden == Classes.Class_types.NumDen.NumDen_Denominator)
                        list_lesson.Add(new Classes.Item_Empty_Lesson() { number = list_lesson.Count + 1, brush_border = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1c242a")) });
                }
                else
                {
                    if (current_numden == Classes.Class_types.NumDen.NumDen_Numerator)
                        list_lesson.Add(
                            new Classes.Item_New_Lesson()
                            {
                                number = list_lesson.Count + 1,
                                title = new_string.title,
                                teacher = new_string.teacher,
                                cabinet = new_string.cabinet,
                                type = new_string.type,
                                editions = new_string.editions
                            });
                    else if (current_numden == Classes.Class_types.NumDen.NumDen_Denominator)
                        list_lesson.Add(new Classes.Item_New_Lesson()
                        {
                            number = list_lesson.Count + 1,
                            title = new_string.title,
                            teacher = new_string.teacher,
                            cabinet = new_string.cabinet,
                            type = new_string.type,
                            editions = new_string.editions,
                            brush_border = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1c242a"))
                        });
                }
            }
        }

        private void lv_day_of_weeks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_add_lesson.IsEnabled = true;
            var item_dayweek = lv_day_of_weeks.SelectedItem;
            if (item_dayweek != null)
            {
                var _item_dayweek = (KeyValuePair<string, string>)item_dayweek;
                switch (_item_dayweek.Key)
                {
                    case "ПОНЕДЕЛЬНИК":
                        if (dayOfWeek != Classes.Class_types.DayOfWeek.Monday)
                        {
                            //if (temp_file.tempType == Classes.Class_types.TempType.Temp_new)
                            //{
                            if (list_lesson.Count > 0 && list_sheets.Count > 0)
                            {
                                ioFunctions.saveJSONDayOfWeek(list_lesson, dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden);
                            }
                            // }
                            list_lesson.Clear();
                            dayOfWeek = Classes.Class_types.DayOfWeek.Monday;
                            if (list_sheets.Count > 0)
                            {
                                if (ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden) != null)
                                {
                                    foreach (var item in ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden).OrderBy(x =>
                                    {
                                        if (x is Classes.Item_New_Lesson item)
                                            return item.number;
                                        else if (x is Classes.Item_Group group)
                                            return group.number;
                                        else if (x is Classes.Item_Empty_Lesson emptyLesson)
                                            return emptyLesson.number;
                                        else if (x is Classes.Item_New_Thursday thursday)
                                            return -1; // Переместите 'Item_New_Thursday' в начало коллекции
                                    else
                                            return 0; // Если класс неизвестен, сохраните текущий порядок
                                }))
                                    {
                                        list_lesson.Add(item);
                                    }
                                }
                            }
                        }
                        break;
                    case "ВТОРНИК":
                        if (dayOfWeek != Classes.Class_types.DayOfWeek.Tuesday)
                        {
                            //if (temp_file.tempType == Classes.Class_types.TempType.Temp_new)
                            //{
                            if (list_lesson.Count > 0 && list_sheets.Count > 0)
                            {
                                ioFunctions.saveJSONDayOfWeek(list_lesson, dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden);
                            }
                            // }
                            list_lesson.Clear();
                            dayOfWeek = Classes.Class_types.DayOfWeek.Tuesday;
                            if (list_sheets.Count > 0)
                            {
                                if (ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden) != null)
                                {
                                    foreach (var item in ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden).OrderBy(x =>
                                    {
                                        if (x is Classes.Item_New_Lesson item)
                                            return item.number;
                                        else if (x is Classes.Item_Group group)
                                            return group.number;
                                        else if (x is Classes.Item_Empty_Lesson emptyLesson)
                                            return emptyLesson.number;
                                        else if (x is Classes.Item_New_Thursday thursday)
                                            return -1; // Переместите 'Item_New_Thursday' в начало коллекции
                                    else
                                            return 0; // Если класс неизвестен, сохраните текущий порядок
                                }))
                                    {
                                        list_lesson.Add(item);
                                    }
                                }
                            }
                        }
                        break;

                        case "СРЕДА":
                        if (dayOfWeek != Classes.Class_types.DayOfWeek.Wednesday)
                        {
                            //if (temp_file.tempType == Classes.Class_types.TempType.Temp_new)
                            //{
                            if (list_lesson.Count > 0 && list_sheets.Count > 0)
                            {
                                ioFunctions.saveJSONDayOfWeek(list_lesson, dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden);
                            }
                            // }
                            list_lesson.Clear();
                            dayOfWeek = Classes.Class_types.DayOfWeek.Wednesday;
                            if (list_sheets.Count > 0)
                            {
                                if (ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden) != null)
                                {
                                    foreach (var item in ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden).OrderBy(x =>
                                    {
                                        if (x is Classes.Item_New_Lesson item)
                                            return item.number;
                                        else if (x is Classes.Item_Group group)
                                            return group.number;
                                        else if (x is Classes.Item_Empty_Lesson emptyLesson)
                                            return emptyLesson.number;
                                        else if (x is Classes.Item_New_Thursday thursday)
                                            return -1; // Переместите 'Item_New_Thursday' в начало коллекции
                                    else
                                            return 0; // Если класс неизвестен, сохраните текущий порядок
                                }))
                                    {
                                        list_lesson.Add(item);
                                    }
                                }
                            }
                        }
                        break;

                    case "ЧЕТВЕРГ":
                        if (dayOfWeek != Classes.Class_types.DayOfWeek.Thursday)
                        {
                            //if (temp_file.tempType == Classes.Class_types.TempType.Temp_new)
                            //{
                            if (list_lesson.Count > 0 && list_sheets.Count > 0)
                            {
                                ioFunctions.saveJSONDayOfWeek(list_lesson, dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden);
                            }
                            // }
                            list_lesson.Clear();
                            list_lesson.Add(new Classes.Item_New_Thursday() { });
                            dayOfWeek = Classes.Class_types.DayOfWeek.Thursday;
                            ioFunctions.saveJSONDayOfWeek(list_lesson, dayOfWeek, list_sheets[combobox_list.SelectedIndex], Classes.Class_types.NumDen.NumDen_Numerator);
                            ioFunctions.saveJSONDayOfWeek(list_lesson, dayOfWeek, list_sheets[combobox_list.SelectedIndex], Classes.Class_types.NumDen.NumDen_Denominator);
                            list_lesson.Clear();
                            if (list_sheets.Count > 0)
                            {
                                if (ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden) != null)
                                {
                                    foreach (var item in ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden).OrderBy(x =>
                                    {
                                        if (x is Classes.Item_New_Lesson item)
                                            return item.number;
                                        else if (x is Classes.Item_Group group)
                                            return group.number;
                                        else if (x is Classes.Item_Empty_Lesson emptyLesson)
                                            return emptyLesson.number;
                                        else if (x is Classes.Item_New_Thursday thursday)
                                            return -1; // Переместите 'Item_New_Thursday' в начало коллекции
                                    else
                                            return 0; // Если класс неизвестен, сохраните текущий порядок
                                }))
                                    {
                                        list_lesson.Add(item);
                                    }
                                }
                            }
                        }
                        btn_add_lesson.IsEnabled = false;
                        break;

                    case "ПЯТНИЦА":
                        if (dayOfWeek != Classes.Class_types.DayOfWeek.Friday)
                        {
                            //if (temp_file.tempType == Classes.Class_types.TempType.Temp_new)
                            //{
                            if (list_lesson.Count > 0 && list_sheets.Count > 0)
                            {
                                ioFunctions.saveJSONDayOfWeek(list_lesson, dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden);
                            }
                            // }
                            list_lesson.Clear();
                            dayOfWeek = Classes.Class_types.DayOfWeek.Friday;
                            if (list_sheets.Count > 0)
                            {
                                if (ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden) != null)
                                {
                                    foreach (var item in ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden).OrderBy(x =>
                                    {
                                        if (x is Classes.Item_New_Lesson item)
                                            return item.number;
                                        else if (x is Classes.Item_Group group)
                                            return group.number;
                                        else if (x is Classes.Item_Empty_Lesson emptyLesson)
                                            return emptyLesson.number;
                                        else if (x is Classes.Item_New_Thursday thursday)
                                            return -1; // Переместите 'Item_New_Thursday' в начало коллекции
                                    else
                                            return 0; // Если класс неизвестен, сохраните текущий порядок
                                }))
                                    {
                                        list_lesson.Add(item);
                                    }
                                }
                            }
                        }
                        break;

                    case "СУББОТА":
                        if (dayOfWeek != Classes.Class_types.DayOfWeek.Saturday)
                        {
                            //if (temp_file.tempType == Classes.Class_types.TempType.Temp_new)
                            //{
                            if (list_lesson.Count > 0 && list_sheets.Count > 0)
                            {
                                ioFunctions.saveJSONDayOfWeek(list_lesson, dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden);
                            }
                            // }
                            list_lesson.Clear();
                            dayOfWeek = Classes.Class_types.DayOfWeek.Saturday;
                            if (list_sheets.Count > 0)
                            {
                                if (ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden) != null)
                                {
                                    foreach (var item in ioFunctions.openJSONDayOfWeek(dayOfWeek, list_sheets[combobox_list.SelectedIndex], current_numden).OrderBy(x =>
                                    {
                                        if (x is Classes.Item_New_Lesson item)
                                            return item.number;
                                        else if (x is Classes.Item_Group group)
                                            return group.number;
                                        else if (x is Classes.Item_Empty_Lesson emptyLesson)
                                            return emptyLesson.number;
                                        else if (x is Classes.Item_New_Thursday thursday)
                                            return -1; // Переместите 'Item_New_Thursday' в начало коллекции
                                    else
                                            return 0; // Если класс неизвестен, сохраните текущий порядок
                                }))
                                    {
                                        list_lesson.Add(item);
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }

        private async void btn_Add_Group_Item_New_Lesson_Click(object sender, RoutedEventArgs e)
        {
            bool result = await mainWindow.add_new_group();
            if (result == true)
            {
                temp_file = ioFunctions.openJSONTemp();
                var temp_fileString = temp_file.new_string.ToString(); // Предполагается, что new_string содержит JSON-строку
                var new_string = JsonSerializer.Deserialize<Classes.Group_data>(temp_fileString);
                if (new_string != null)
                {
                    Classes.Item_New_Lesson lesson = (sender as Button)?.DataContext as Classes.Item_New_Lesson;
                    if (lesson != null)
                    {
                        int index = list_lesson.IndexOf(lesson);
                        //list_lesson.Remove(lesson);

                        if (index != -1)
                        {
                            int i = 0;
                            foreach (var item in list_lesson.ToList())
                            {
                                i++;
                                if (item is Classes.Item_New_Lesson)
                                {
                                    if (i - 1 != index)
                                    {
                                        Classes.Item_New_Lesson item_lesson = (Classes.Item_New_Lesson)item;
                                        item_lesson.number = i;
                                        list_lesson.RemoveAt(i - 1);
                                        list_lesson.Insert(i - 1, item_lesson);
                                    }
                                    else if (i - 1 == index)
                                    {
                                        new_string.number = 2;
                                        Classes.Item_New_Lesson item_lesson = (Classes.Item_New_Lesson)item;
                                        Classes.Item_Group _item_Group = null;
                                        if (current_numden == Classes.Class_types.NumDen.NumDen_Numerator)
                                        {
                                            Classes.Item_Group item_Group = new Classes.Item_Group()
                                            {
                                                number = i,
                                                groups = new ObservableCollection<Classes.Group_data>()
                                                {
                                                    new Classes.Group_data()
                                                    {
                                                        number = 1,
                                                        title = item_lesson.title,
                                                        teacher = item_lesson.teacher,
                                                        cabinet = item_lesson.cabinet,
                                                        type = item_lesson.type,
                                                        editions = item_lesson.editions
                                                    },
                                                    new_string
                                                }
                                            };
                                            _item_Group = item_Group;
                                        }
                                        else if (current_numden == Classes.Class_types.NumDen.NumDen_Denominator)
                                        {
                                            Classes.Item_Group item_Group = new Classes.Item_Group()
                                            {
                                                number = i,
                                                groups = new ObservableCollection<Classes.Group_data>()
                                                {
                                                    new Classes.Group_data()
                                                    {
                                                        number = 1,
                                                        title = item_lesson.title,
                                                        teacher = item_lesson.teacher,
                                                        cabinet = item_lesson.cabinet,
                                                        type = item_lesson.type,
                                                        editions = item_lesson.editions
                                                    },
                                                    new_string
                                                },
                                                brush_border = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1c242a"))
                                            };
                                            _item_Group = item_Group;
                                        }
                                        if (_item_Group != null)
                                        {
                                            list_lesson.RemoveAt(i - 1);
                                            list_lesson.Insert(i - 1, _item_Group);
                                        }
                                    }
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
            }
        }

        private async void btn_Add_Group_Item_Group(object sender, RoutedEventArgs e)
        {
            bool result = await mainWindow.add_new_group();
            if (result == true)
            {
                temp_file = ioFunctions.openJSONTemp();
                var temp_fileString = temp_file.new_string.ToString(); // Предполагается, что new_string содержит JSON-строку
                var new_string = JsonSerializer.Deserialize<Classes.Group_data>(temp_fileString);
                if (new_string != null)
                {
                    Classes.Group_data group_ = (sender as Button)?.DataContext as Classes.Group_data;
                    if (group_ != null)
                    {
                        var item_Group = list_lesson[curent_item_group] as Classes.Item_Group;
                        if (item_Group != null)
                        {
                            var _list_group = item_Group.groups;

                            int index_group = _list_group.IndexOf(group_);

                            _list_group.Insert(index_group + 1, new_string);
                            int i = 0;

                            foreach (var group in _list_group.ToList())
                            {
                                i++;

                                Classes.Group_data item_group = (Classes.Group_data)group;
                                item_group.number = i;
                                _list_group.RemoveAt(i - 1);
                                _list_group.Insert(i - 1, item_group);
                            }
                        }
                    }
                }
            }
        }

        private void border_Item_Group_MouseEnter(object sender, MouseEventArgs e)
        {
            var itemGroup = (sender as FrameworkElement)?.DataContext;
            var itemsControl = FindParent<ItemsControl>(sender as DependencyObject);
            var index = itemsControl?.Items.IndexOf(itemGroup);

            curent_item_group = Convert.ToInt32(index);
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (true)
            {
                var parentObject = VisualTreeHelper.GetParent(child);
                if (parentObject == null)
                {
                    return null;
                }

                if (parentObject is T parent)
                {
                    return parent;
                }

                child = parentObject;
            }
        }

        private void btn_move_lesson_bottom_Click(object sender, RoutedEventArgs e)
        {
            if (list_lesson.Count > 0)
            {
                object lesson = (sender as Button)?.DataContext;
                if (lesson != null)
                {
                    if (lesson is Classes.Item_New_Lesson)
                    {
                        var item = lesson as Classes.Item_New_Lesson;
                        int index = list_lesson.IndexOf(item);
                        if (index < list_lesson.Count - 1)
                        {
                            list_lesson.RemoveAt(index);
                            list_lesson.Insert(index + 1, item);
                        }
                    }
                    if (lesson is Classes.Item_Empty_Lesson)
                    {
                        var item = lesson as Classes.Item_Empty_Lesson;
                        int index = list_lesson.IndexOf(item);
                        if (index < list_lesson.Count - 1)
                        {
                            list_lesson.RemoveAt(index);
                            list_lesson.Insert(index + 1, item);
                        }
                    }
                    if (lesson is Classes.Item_Group)
                    {
                        var item = lesson as Classes.Item_Group;
                        int index = list_lesson.IndexOf(item);
                        if (index < list_lesson.Count - 1)
                        {
                            list_lesson.RemoveAt(index);
                            list_lesson.Insert(index + 1, item);
                        }
                    }
                    fixLessonNumeration();
                }
            }
        }

        private void btn_move_lesson_upper_Click(object sender, RoutedEventArgs e)
        {
            if (list_lesson.Count > 0)
            {
                object lesson = (sender as Button)?.DataContext;
                if (lesson != null)
                {
                    if (lesson is Classes.Item_New_Lesson)
                    {
                        var item = lesson as Classes.Item_New_Lesson;
                        int index = list_lesson.IndexOf(item);
                        if (index > 0)
                        {
                            list_lesson.RemoveAt(index);
                            list_lesson.Insert(index - 1, item);
                        }
                    }
                    if (lesson is Classes.Item_Empty_Lesson)
                    {
                        var item = lesson as Classes.Item_Empty_Lesson;
                        int index = list_lesson.IndexOf(item);
                        if (index > 0)
                        {
                            list_lesson.RemoveAt(index);
                            list_lesson.Insert(index - 1, item);
                        }
                    }
                    if (lesson is Classes.Item_Group)
                    {
                        var item = lesson as Classes.Item_Group;
                        int index = list_lesson.IndexOf(item);
                        if (index > 0)
                        {
                            list_lesson.RemoveAt(index);
                            list_lesson.Insert(index - 1, item);
                        }
                    }
                    fixLessonNumeration();
                }
            }
        }
    }
}
