﻿using System;
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
            new KeyValuePair<String, String>("ЧЕТВЕРГ", "/Icons\\ic_thursday.png"),
            new KeyValuePair<String, String>("ПЯТНИЦА", "/Icons\\ic_friday.png"),
            new KeyValuePair<String, String>("СУББОТА", "/Icons\\ic_saturday.png")
        };

        // Создайте коллекции объектов для источников данных

        List<Classes.Groups_data> list_group = new List<Classes.Groups_data>();
        ObservableCollection<KeyValuePair<int, List<Classes.Groups_data>>> list_lesson = new ObservableCollection<KeyValuePair<int, List<Classes.Groups_data>>>()
        {
            new KeyValuePair<int, List<Classes.Groups_data>>(0, new List<Classes.Groups_data>(){
                new Classes.Groups_data(){
                title="ОСНОВЫ РОССИЙСКОЙ ГОСУДАРСТВЕННОСТИ", teacher="АНИСИМОВА В.А.",
                cabinet="216", type="ПЗ", editions= new List<string>(){ "25.09","Прикол"}
                }
            })
        };

        Classes.Class_JSON_Temp temp_file;
        #endregion
        public Page_main()
        {
            InitializeComponent();
            init();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Groups_data lesson = (sender as Button)?.DataContext as Classes.Groups_data;
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

            lv_lessons.ItemsSource = list_lesson;

            lv_day_of_weeks.ItemsSource = dayofweeks_list;
            ObservableCollection<KeyValuePair<int, List<Classes.Groups_data>>> lesson = new ObservableCollection<KeyValuePair<int, List<Classes.Groups_data>>>();
            lv_lessons.DataContext = lesson;
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
