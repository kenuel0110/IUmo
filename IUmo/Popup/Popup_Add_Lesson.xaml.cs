using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MaterialDesignThemes.Wpf;

namespace IUmo.Popup
{
    /// <summary>
    /// Interaction logic for Popup_Add_Lesson.xaml
    /// </summary>
    public partial class Popup_Add_Lesson : Page
    {

        #region local_varibles
        private MainWindow mainWindow = App.Current.MainWindow as MainWindow;
        private ObservableCollection<String> list_changes = new ObservableCollection<String>();
        bool is_empty_lesson;
        private Functions.IOFunctions ioFunctions = new Functions.IOFunctions();
        Classes.Class_JSON_Temp temp_file;
        #endregion

        #region global_varibles
        public bool Add_Lesson_Result { get; set; }
        #endregion

        public Popup_Add_Lesson()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            temp_file = ioFunctions.openJSONTemp();
            lv_changes.ItemsSource = list_changes;
        }

        private void btn_close_popup_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.popup_window.Visibility = Visibility.Hidden;
            mainWindow.deblurBackground();
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            ((MainWindow)Application.Current.MainWindow).SetDialogResult_AddLesson(false);
        }

        private void tb_title_KeyUp(object sender, KeyEventArgs e)
        {
            string searchText = tb_title.Text.ToLower(); // Получение текста из TextBox и приведение к нижнему регистру

            // Фильтрация списка на основе текста поиска
            //List<string> filteredItems = items.Where(item => item.ToLower().Contains(searchText)).ToList();

            /*if (filteredItems.Count > 0)
            {
                // Отображение Popup с результатами поиска
                Popup.IsOpen = true;
                ListBox.ItemsSource = filteredItems;
            }
            else
            {
                // Скрытие Popup, если результатов нет
                Popup.IsOpen = false;
            }*/
        }

        private void tb_teacher_KeyUp(object sender, KeyEventArgs e)
        {
            string searchText = tb_title.Text.ToLower(); // Получение текста из TextBox и приведение к нижнему регистру

            // Фильтрация списка на основе текста поиска
            //List<string> filteredItems = items.Where(item => item.ToLower().Contains(searchText)).ToList();

            /*if (filteredItems.Count > 0)
            {
                // Отображение Popup с результатами поиска
                Popup.IsOpen = true;
                ListBox.ItemsSource = filteredItems;
            }
            else
            {
                // Скрытие Popup, если результатов нет
                Popup.IsOpen = false;
            }*/
        }

        private void tb_title_GotFocus(object sender, RoutedEventArgs e)
        {
            pop_tb_title.IsOpen = true;
        }

        private void tb_title_LostFocus(object sender, RoutedEventArgs e)
        {
            pop_tb_title.IsOpen = false;
        }

        private void tb_teacher_GotFocus(object sender, RoutedEventArgs e)
        {
            pop_tb_teacher.IsOpen = true;
        }

        private void tb_teacher_LostFocus(object sender, RoutedEventArgs e)
        {
            pop_tb_teacher.IsOpen = false;
        }

        private void combobox_cabinets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void combobox_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void btn_delete_changes_Click(object sender, RoutedEventArgs e)
        {
            String change = (sender as Button)?.DataContext as String;
            if (change != null)
            {
                list_changes.Remove(change);
            }
        }

        private void lv_changes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox _lv_changes = (ListBox)sender;
            if (_lv_changes.SelectedItem != null)
            {
                _lv_changes.SelectedItem = null;
            }
        }

        private void tb_changes_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space || e.Key == Key.Enter)
            {
                if (!string.IsNullOrWhiteSpace(tb_changes.Text.ToString()))
                    list_changes.Add(tb_changes.Text.ToString());
                tb_changes.Clear();
            }
        }

        private void togle_empty_lesson_Checked(object sender, RoutedEventArgs e)
        {
            tb_title.BorderThickness = new Thickness(0);
            tb_teacher.BorderThickness = new Thickness(0);
            tb_title.BorderBrush = Brushes.Transparent;
            tb_teacher.BorderBrush = Brushes.Transparent;
            TextFieldAssist.SetUnderlineBrush(tb_title, Brushes.Transparent);
            TextFieldAssist.SetUnderlineBrush(tb_teacher, Brushes.Transparent);
            HintAssist.SetHelperText(tb_title, "");
            HintAssist.SetHelperText(tb_teacher, "");

            lbl_cabinet_helptext.Text = "";
            combobox_cabinets.Foreground = Brushes.White;
            lbl_type_helptext.Text = "";
            combobox_type.Foreground = Brushes.White;


            if (togle_empty_lesson.IsChecked == true) 
            {
                is_empty_lesson = true;
                tb_title.IsEnabled = false;
                tb_teacher.IsEnabled = false;
                combobox_cabinets.IsEnabled = false;
                combobox_type.IsEnabled = false;
                tb_changes.IsEnabled = false;
                lv_changes.IsEnabled = false;
                pop_tb_teacher.IsOpen = false;
                pop_tb_title.IsOpen = false;
            }
            else
            {
                is_empty_lesson = false;
                tb_title.IsEnabled = true;
                tb_teacher.IsEnabled = true;
                combobox_cabinets.IsEnabled = true;
                combobox_type.IsEnabled = true;
                tb_changes.IsEnabled = true;
                lv_changes.IsEnabled = true;
                pop_tb_teacher.IsOpen = false;
                pop_tb_title.IsOpen = false;
            }
        }

        private void btn_popup_done_Click(object sender, RoutedEventArgs e)
        {
            Classes.Add_Item_Lesson new_item;
            if (is_empty_lesson == true)
            {
                new_item = new Classes.Add_Item_Lesson()
                {
                    title = null,
                    teacher = null,
                    cabinet = null,
                    type = null,
                    editions = null,
                    empty_lesson = true
                };
                ioFunctions.editTemp(new_item);

                mainWindow.popup_window.Visibility = Visibility.Hidden;
                mainWindow.deblurBackground();
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
                mainWindow.SetDialogResult_AddLesson(true);
            }
            else 
            {
                string title = "";
                string teacher = "";
                var cabinet = combobox_cabinets.SelectedItem as ComboBoxItem;
                var type = combobox_type.SelectedItem as ComboBoxItem;
                string _cabinet = "";
                string _type = "";


                if (string.IsNullOrEmpty(tb_title.Text))
                {
                    title = "";
                    tb_title.BorderThickness = new Thickness(2);
                    TextFieldAssist.SetUnderlineBrush(tb_title, Brushes.Transparent);
                    tb_title.BorderBrush = Brushes.DarkRed;
                    HintAssist.SetHelperText(tb_title, "*Поле должно быть заполненно");
                }
                else
                {
                    title = tb_title.Text;
                    tb_title.BorderThickness = new Thickness(0);
                    tb_title.BorderBrush = Brushes.Transparent;
                    TextFieldAssist.SetUnderlineBrush(tb_title, Brushes.Transparent);
                    HintAssist.SetHelperText(tb_title, "");
                }

                if (string.IsNullOrEmpty(tb_teacher.Text))
                {
                    teacher = "";
                    tb_teacher.BorderThickness = new Thickness(2);
                    TextFieldAssist.SetUnderlineBrush(tb_teacher, Brushes.Transparent);
                    tb_teacher.BorderBrush = Brushes.DarkRed;
                    HintAssist.SetHelperText(tb_teacher, "*Поле должно быть заполненно");
                }
                else 
                {
                    teacher = tb_teacher.Text;
                    tb_teacher.BorderThickness = new Thickness(0);
                    tb_teacher.BorderBrush = Brushes.Transparent;
                    TextFieldAssist.SetUnderlineBrush(tb_teacher, Brushes.Transparent);
                    HintAssist.SetHelperText(tb_teacher, "");
                }

                if (cabinet.Content.ToString() == "=========" || combobox_cabinets.SelectedIndex == 0)
                {
                    lbl_cabinet_helptext.Text = "*Выберите другой элемент";
                    _cabinet = "";
                }
                else {
                    lbl_cabinet_helptext.Text = "";
                    _cabinet = cabinet.Content.ToString();
                }

                if (combobox_type.SelectedIndex == 0)
                {
                    lbl_type_helptext.Text = "*Выберите тип";
                    _type = "";
                }
                else
                {
                    lbl_type_helptext.Text = "";
                    _type = type.Content.ToString();
                }

                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(teacher) && !string.IsNullOrEmpty(_cabinet) && !string.IsNullOrEmpty(_type)) 
                {
                    new_item = new Classes.Add_Item_Lesson()
                    {
                        title = title,
                        teacher = teacher,
                        cabinet = _cabinet,
                        type = _type,
                        editions = new List<string>(list_changes),
                        empty_lesson = false
                    };
                    ioFunctions.editTemp(new_item);

                    mainWindow.popup_window.Visibility = Visibility.Hidden;
                    mainWindow.deblurBackground();
                    if (NavigationService.CanGoBack)
                        NavigationService.GoBack();
                    mainWindow.SetDialogResult_AddLesson(true);
                }

            }

        }
    }
}
