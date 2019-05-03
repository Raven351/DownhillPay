using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DownhillPayClient.UserControls
{

    public interface IUserControlWindow
    {
        MainWindow MainWindow
        {
            get;
        }

        UserControl PreviousControl
        {
            get;
            set;
        }

        UserControl ChangeToControl(UserControl previousControl);
    }
}
