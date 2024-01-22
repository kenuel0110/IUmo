using IUmo.Classes;
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

namespace IUmo.Pages
{
    /// <summary>
    /// Interaction logic for Page_select_course.xaml
    /// </summary>
    public partial class Page_select_course : Page
    {

        #region local_varibles
        private MainWindow mainWindow = App.Current.MainWindow as MainWindow;
        private Functions.IOFunctions ioFunctions = new Functions.IOFunctions();
        Classes.Class_JSON_Temp temp_file;
        #endregion


        public Page_select_course()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            mainWindow.btn_file.IsEnabled = false;
            mainWindow.btn_main.IsEnabled = false;
            mainWindow.btn_insert.IsEnabled = false;

            temp_file = ioFunctions.openJSONTemp();
            mainWindow.title_window.Content = System.IO.Path.GetFileName(temp_file.path);
            ioFunctions.chkFirstStart("settings", "courses.json");
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            combobox_course_bottom.BorderBrush = new SolidColorBrush(Color.FromRgb(78, 11, 60));
            if (combobox_course_center != null)
                combobox_course_center.SelectedIndex = combobox_course_bottom.SelectedIndex;
        }
        private void ComboBox_Center_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            combobox_course_center.BorderBrush = new SolidColorBrush(Color.FromRgb(2, 37, 28));
            if (combobox_course_bottom != null)
                combobox_course_bottom.SelectedIndex = combobox_course_center.SelectedIndex;
        }

        private void btn_open_document_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<Class_JSON_Courses_Settings> courses = ioFunctions.openJSONCourses();
            if (combobox_course_bottom.SelectedIndex != 0 || combobox_course_center.SelectedIndex != 0) 
            {
                mainWindow.showLoading();
                var element = courses.FirstOrDefault(x => x.number == combobox_course_bottom.SelectedIndex);
                try
                {
                    mainWindow.init_new_document(element.number, element.groups);
                }
                catch (Exception ex) 
                {
                    mainWindow.hideLoading();
                    mainWindow.showInfoPopup(ex.Message);
                }
                mainWindow.hideLoading();
                NavigationService.Navigate(new Pages.Page_main());
            } 
        }
    }
}
