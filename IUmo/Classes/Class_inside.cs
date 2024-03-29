﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IUmo.Classes
{
    public class Group_data
    {
        public int number { get; set; }
        public string title { get; set; }
        public string teacher { get; set; }
        public string cabinet { get; set; }
        public string type { get; set; }
        public List<string> editions { get; set; }
    }

    public class Add_Item_Lesson
    {
        public string title { get; set; }
        public string teacher { get; set; }
        public string cabinet { get; set; }
        public string type { get; set; }
        public bool empty_lesson { get; set; }
        public List<string> editions { get; set; }
    }
}
