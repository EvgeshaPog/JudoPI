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
    /// Логика взаимодействия для FormJury.xaml
    /// </summary>
    public partial class FormJury : Window
    {
        SQLData db;
        public FormJury()
        {
            InitializeComponent();
            db = new SQLData();
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            App.Current.Windows.OfType<MainWindow>().First().Show();
            Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            fightGroup fg = new fightGroup();
            fg.Show();
            Hide();
        }

        private void dataGridParty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            WindowControl wc = new WindowControl();
            wc.Show();
            Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Windows.OfType<MainWindow>().First().Show();
        }
    }
}
