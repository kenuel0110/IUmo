using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IUmo.Classes
{
    //Файл настроек
    public class Class_JSON_Setting
    {
        public Class_types.WindowState maximilize_window { get; set; }
        public List<double> size_window { get; set; }
    }

    public class Class_JSON_RecenFiles
    {
        public string name { get; set; }
        public string path { get; set; }
    }

    public class Class_JSON_Courses_Settings
    {
        public int number { get; set; }
        public List<KeyValuePair<String, String>> groups { get; set; }
    }


    public class Class_JSON_Temp
    {
        public string path { get; set; }
        public Class_types.TempType tempType { get; set; }
        public int course { get; set; }
        public Classes.Add_Item_Lesson new_string { get; set; } = new Classes.Add_Item_Lesson();
    }
}
