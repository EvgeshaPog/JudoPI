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
    /// Логика взаимодействия для FormAdmin.xaml
    /// </summary>
    public partial class FormAdmin : Window
    {
        SQLData db;
        public FormAdmin()
        {
            InitializeComponent();
            db = new SQLData();
            LoadBD();
        }

        private void LoadBD()
        {
            dataGridParty.ItemsSource = db.RunSelect("Select FIO as ФИО, SportClub.Name as Спортклуб, City.Name as Город, Weight as Вес, Age as Возраст from ((People inner join SportClub On People.Id_SportClub=SportClub.Id) inner join City On People.Id_City=City.ID)").DefaultView;
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            App.Current.Windows.OfType<MainWindow>().First().Show();
            Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Gorod g = new Gorod();
            g.Show();
            Hide();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            SportC sc = new SportC();
            sc.Show();
            Hide();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            ClassWeight cw = new ClassWeight();
            cw.Show();
            Hide();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            ClassAge ca = new ClassAge();
            ca.Show();
            Hide();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            Mat m = new Mat();
            m.Show();
            Hide();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            Registration r = new Registration();
            r.Show();
            Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Windows.OfType<MainWindow>().First().Show();
        }

        private void MenuItem_Click7(object sender, RoutedEventArgs e)
        {
            Users u = new Users();
            u.Show();
            Hide();
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {

        }
    }
}
