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
    }
}
