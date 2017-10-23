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
    /// Логика взаимодействия для NewUser.xaml
    /// </summary>
    public partial class NewUser : Window
    {
        SQLData db;

        public NewUser()
        {
            InitializeComponent();
            db = new SQLData();
        }

        private void butLogin_Click(object sender, RoutedEventArgs e)
        {
            if (tbFIO.Text!="" && tbLogin.Text != "" && tbPassword.Password.ToString() != "")
            {
                MessageBox.Show(
                    db.RunInsertUpdateDelete(@"Insert into [User](FIO, Login, Password, Admin) 
                    values('" +tbFIO.Text+"','"+ tbLogin.Text + "','" + tbPassword.Password.ToString() + "','" + ((chbAdmin.IsChecked == true) ? 1 : 0)+"')")
                    );
               App.Current.Windows.OfType<MainWindow>().First().Show();
                Close();
            }
            else
                MessageBox.Show("Заполните все поля!");
        }

        private void butRegistration_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Windows.OfType<MainWindow>().First().Show();
            Close();
        }
    }
}
