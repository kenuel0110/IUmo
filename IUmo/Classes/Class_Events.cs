using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IUmo.Classes
{
    public class Class_Events
    {
        public class Class_Current_Page
        {
            private Classes.Class_types.Pages current_page_;

            public event EventHandler<Classes.Class_types.Pages> Current_page_changed;

            public Classes.Class_types.Pages current_page
            {
                get { return current_page_; }
                set
                {
                    if (current_page_ != value)
                    {
                        current_page_ = value;
                        Oncurrent_page_changed();
                    }
                }
            }

            protected virtual void Oncurrent_page_changed()
            {
                Current_page_changed.Invoke(this, current_page_);
            }
        }
    }
}
