﻿using System;
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

        public enum Info_IMG
        {
            img_None = 0,
            img_Error = 1,
            img_finding = 2,
            img_noFind = 3,
            img_blocked = 4

        }
        public enum TempType
        {
            Temp_none = 0,
            Temp_new = 1,
            Temp_open = 2
        }

        public enum TypeItem
        {
            Type_empty = 0,
            Type_base = 1,
            Type_group = 2
        }

        public enum DayOfWeek
        {
            None = 0,
            Monday = 1,
            Tuesday = 2,
            Wednesday = 3,
            Thursday = 4,
            Friday = 5,
            Saturday= 6,
        }

        public enum NumDen
        {
            NumDen_None = 0,
            NumDen_Numerator = 1,
            NumDen_Denominator = 2
        }

    }
}
