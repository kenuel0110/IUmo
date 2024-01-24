using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IUmo.Classes
{
    public class Item_New_Lesson
    {
        public int number { get; set; }
        public string title { get; set; }
        public string teacher { get; set; }
        public string cabinet { get; set; }
        public string type { get; set; }
        public bool empty_lesson { get; set; }
        public bool have_group { get; set; }
        public List<string> editions { get; set; }
    }
}
