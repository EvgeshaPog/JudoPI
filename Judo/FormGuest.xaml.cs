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
    /// Логика взаимодействия для FormGuest.xaml
    /// </summary>
    public partial class FormGuest : Window
    {
        SQLData db;
        public FormGuest()
        {
            InitializeComponent();
            db = new SQLData();
            LoadBD();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Windows.OfType<MainWindow>().First().Show();
        }
           
        

        private void LoadBD()
        {
            dataGridParty.ItemsSource = db.RunSelect("Select FIO as ФИО, SportClub.Name as Спортклуб, City.Name as Город, Weight as Вес, Age as Возраст from ((People inner join SportClub On People.Id_SportClub=SportClub.Id) inner join City On People.Id_City=City.ID)").DefaultView;
        }

    }
}
