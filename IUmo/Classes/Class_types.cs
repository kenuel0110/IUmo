using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IUmo.Classes
{
    public class Class_types
    {
        //Развёрнуто ли окно на весь экран
        public enum WindowState
        {
            None = 0,
            False = 1,
            True = 2
        }

        //Странцы используемые в программе
        public enum Pages
        {
            Page_None = 0,
            Page_Start = 1,
            Page_Course = 2,
            Page_Main = 3
        }
        public enum TempType
        {
            Temp_none = 0,
            Temp_new = 1,
            Temp_open = 2
        }
    }
}
