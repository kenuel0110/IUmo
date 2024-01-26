using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IUmo.Classes
{
    public class Item_New_Lesson
    {
        public int number { get; set; }
        public string title { get; set; }
        public string teacher { get; set; }
        public string cabinet { get; set; }
        public string type { get; set; }
        public List<string> editions { get; set; }
        public Brush brush_border { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#272727"));
    }
}
