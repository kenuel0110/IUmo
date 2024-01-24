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
        #endregion
        public Popup_Add_Lesson()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            list_changes.Add("sdsadsad");
            list_changes.Add("234");
            list_changes.Add("ret");
            list_changes.Add("2.12");
            lv_changes.ItemsSource = list_changes;
        }

        private void btn_close_popup_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.popup_window.Visibility = Visibility.Hidden;
            mainWindow.deblurBackground();
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
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
    }
}
