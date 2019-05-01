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
    //public class UserControlWindow : UserControl
    //{
    //    private readonly MainWindow parentWindow;
    //    private readonly UserControlWindow previousUserControl;
    //    public UserControlWindow(MainWindow mainWindow) => parentWindow = mainWindow;
    //    public UserControlWindow(MainWindow mainWindow, UserControlWindow previousControl) : this(mainWindow) => previousUserControl = previousControl;
    //}
}
