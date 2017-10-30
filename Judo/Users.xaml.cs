using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Users.xaml
    /// </summary>
    public partial class Users : Window
    {
        SQLData db;

        public Users()
        {
            InitializeComponent();
            db = new SQLData();
        }

        private void butAdd_Click(object sender, RoutedEventArgs e)
        {
            
            VisibleTrue();
            groupBox.Header = "Добавить";
        }

        void VisibleTrue()
        {
            groupBox.Visibility = Visibility.Visible;
            butAdd.IsEnabled = false;
            butDelete.IsEnabled = false;
            butEdit.IsEnabled = false;
            UsersDataGrid.Visibility = Visibility.Hidden;
        }
        void VisibleFalse()
        {
            groupBox.Visibility = Visibility.Hidden;
            butAdd.IsEnabled = true;
            butDelete.IsEnabled = true;
            butEdit.IsEnabled = true;
            UsersDataGrid.Visibility = Visibility.Visible;
            ClearTextBox();
        }

        private void butEdit_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItems.Count > 0) {
                groupBox.Header = "Редактировать";
                DataRowView row = (DataRowView)UsersDataGrid.SelectedItems[0];


                DataTable datat = db.RunSelect("SELECT [Id] as 'Id', [FIO] as 'ФИО', [Login] as 'Логин', [Password] as 'Пароль', [Admin] as 'Администратор' FROM [dbo].[User] Where [Id]= '" + row["Id"] + "'");
                string[] words = datat.Rows[0][1].ToString().Split(new char[] { ' ' });
                textBox_F.Text = words[0];
                textBox_I.Text = words[1];
                textBox_O.Text = words[2];

                textBox_Login.Text = datat.Rows[0][2].ToString();
                textBox_Password.Text = datat.Rows[0][3].ToString();

                CheckBoxAdmin.IsChecked = (datat.Rows[0][4].ToString() == "1") ? true : false;

                VisibleTrue();
            }
            else MessageBox.Show("Выберите пользователя в таблице");
        }

        private void butDelete_Click(object sender, RoutedEventArgs e)
        { 
        
            if (UsersDataGrid.SelectedItem != null && UsersDataGrid.SelectedIndex != -1)
            {
                //DialogResult dialogResult = MessageBox.Show("Вы уверены, что хотите удалить эту запись таблицы?", "Проверка", MessageBoxButtons.YesNo);
                //if (dialogResult == DialogResult.Yes)
                //{
                DataRowView row = (DataRowView)UsersDataGrid.SelectedItems[0];
                db.RunInsertUpdateDelete("DELETE  FROM [dbo].[User] WHERE [Id] =" + row["Id"]);
                LoadTable();
                //}
            }
        
            else { MessageBox.Show("Выберите запись для удаления"); }
        }

        private void butOK_Click(object sender, RoutedEventArgs e)
        {

            if (textBox_F.Text == "" || textBox_I.Text == "" || textBox_Login.Text == "" || textBox_Password.Text == "")
            {
                MessageBox.Show("Заполните все поля отмеченные звездочкой (*)");
                return;
            }

            if (Regex.IsMatch(textBox_email.Text, @"\A[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\z") == false)
            {
                MessageBox.Show("Не правильно введен адрес почты. Повторите попытку.");
                return;
            }

            string FIO = textBox_F.Text + " " + textBox_I.Text + " " + textBox_O.Text;
            string admin = (CheckBoxAdmin.IsChecked == true) ? "1" : "0";

            if (groupBox.Header.ToString() == "Добавить")
            {
                db.RunInsertUpdateDelete("insert into [dbo].[User] ([FIO], [Email], [Login], [Password], [Admin]) values  ('" + FIO + "', '"+ textBox_email.Text+ "', '" + textBox_Login.Text
                    + "', '" + textBox_Password.Text + "', '" + admin + "')");
            }
            if (groupBox.Header.ToString() == "Редактировать")
            {
                DataRowView row = (DataRowView)UsersDataGrid.SelectedItems[0];
                db.RunInsertUpdateDelete("update [dbo].[User]  set [FIO]= '" + FIO + "', [Email]= '" + textBox_email.Text + "', [Login]= '" + textBox_Login.Text + "', [Password]= '" +
                    textBox_Password.Text + "', [Admin]= '" + admin + "' where Id = '" + row["Id"] + "'");
            }

            LoadTable();
            VisibleFalse();
        }

        public void LoadTable()
        {
            DataTable datat = db.RunSelect("SELECT [Id] as 'Id', [FIO] as 'ФИО', [Email] as 'Почта', [Login] as 'Логин', [Password] as 'Пароль', [Admin] as 'Администратор' FROM [dbo].[User]");
            UsersDataGrid.ItemsSource = datat.DefaultView;
            UsersDataGrid.Columns[0].Visibility = Visibility.Hidden;
        }

        void ClearTextBox()
        {
            foreach (Control ctl in containerCanvas.Children)
            {
                if (ctl.GetType() == typeof(CheckBox))
                    ((CheckBox)ctl).IsChecked = false;
                if (ctl.GetType() == typeof(TextBox))
                    ((TextBox)ctl).Text = String.Empty;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTable();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           
        }

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void butOtm_Click(object sender, RoutedEventArgs e)
        {
            VisibleFalse();
        }
    }
}
