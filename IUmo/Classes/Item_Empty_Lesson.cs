using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IUmo.Classes
{
    public class Item_Empty_Lesson
    {
        public int number { get; set; }
        public Brush brush_border { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#272727"));
    }
}
