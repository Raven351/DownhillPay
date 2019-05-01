using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DownhillPayClient
{
    public class Views
    {
        public List<UserControl> UserControls { get; set; }

        public Views()
        {
            //userControls = new List<UserControl>();
            //userControls.Where(t => t is UserControl);
        }

        //public UserControl this[Type userControlType]
        //{
        //    get => UserControls.Where(t => t is userControlType);
        //}
    }
}
