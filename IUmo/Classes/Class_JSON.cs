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
}
