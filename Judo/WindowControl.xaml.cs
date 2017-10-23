using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Judo
{
    /// <summary>
    /// Логика взаимодействия для WindowControl.xaml
    /// </summary>
    public partial class WindowControl : Window
    {
        public WindowControl()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Windows.OfType<FormJury>().First().Show();
        }
    }
}
