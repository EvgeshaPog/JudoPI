using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для Users.xaml
    /// </summary>
    public partial class Users : Window
    {
        SqlConnection con = new SqlConnection("Server=PC; Database=Djudo;Integrated security=True");
        public Users()
        {
            InitializeComponent();
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
            con = new SqlConnection("Server=PC; Database=Djudo;Integrated security=True");
            if (UsersDataGrid.SelectedItems.Count > 0) {
                groupBox.Header = "Редактировать";
                DataRowView row = (DataRowView)UsersDataGrid.SelectedItems[0];

                string query = "SELECT [Id] as 'Id', [FIO] as 'ФИО', [Login] as 'Логин', [Password] as 'Пароль', [Admin] as 'Администратор' FROM [dbo].[User] Where [Id]= '" + row["Id"] + "'";
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataSet myDS = new DataSet();
                da.Fill(myDS, "User");

                string[] words = myDS.Tables["User"].Rows[0][1].ToString().Split(new char[] { ' ' });
                textBox_F.Text = words[0];
                textBox_I.Text = words[1];
                textBox_O.Text = words[2];

                textBox_Login.Text = myDS.Tables["User"].Rows[0][2].ToString();
                textBox_Password.Text = myDS.Tables["User"].Rows[0][3].ToString();

                CheckBoxAdmin.IsChecked = (myDS.Tables["User"].Rows[0][4].ToString() == "1") ? true : false;

                con.Close();
                VisibleTrue();
            }
            else MessageBox.Show("Выберите пользователя в таблице");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Что-то пошло не так, попробуйте снова. Ошибка:" + ex.Message);
            //}
        }

        private void butDelete_Click(object sender, RoutedEventArgs e)
        { 
        
            if (UsersDataGrid.SelectedItem != null && UsersDataGrid.SelectedIndex != -1)
            {
                //DialogResult dialogResult = MessageBox.Show("Вы уверены, что хотите удалить эту запись таблицы?", "Проверка", MessageBoxButtons.YesNo);
                //if (dialogResult == DialogResult.Yes)
                //{
                DataRowView row = (DataRowView)UsersDataGrid.SelectedItems[0];
                string query = "DELETE  FROM [dbo].[User] WHERE [Id] =" + row["Id"];
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
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

            string FIO = textBox_F.Text + " " + textBox_I.Text + " " + textBox_O.Text;
            string admin = (CheckBoxAdmin.IsChecked == true) ? "1" : "0";
            con = new SqlConnection("Server=PC; Database=Djudo;Integrated security=True");



            if (groupBox.Header.ToString() == "Добавить")
            {
                string query = "insert into [dbo].[User] ([FIO], [Login], [Password], [Admin]) values  ('" + FIO + "', '" + textBox_Login.Text
                    + "', '" + textBox_Password.Text + "', '" + admin + "')";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            if (groupBox.Header.ToString() == "Редактировать")
            {
                DataRowView row = (DataRowView)UsersDataGrid.SelectedItems[0];
                string query = "update [dbo].[User]  set [FIO]= '" + FIO + "', [Login]= '" + textBox_Login.Text + "', [Password]= '" +
                    textBox_Password.Text + "', [Admin]= '" + admin + "' where Id = '" + row["Id"] + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            LoadTable();
            VisibleFalse();
        }

        public void LoadTable()
        {
            using (con)
            {
               string CmdString = "SELECT [Id] as 'Id', [FIO] as 'ФИО', [Login] as 'Логин', [Password] as 'Пароль', [Admin] as 'Администратор' FROM [dbo].[User]";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Users");
                sda.Fill(dt);
                UsersDataGrid.ItemsSource = dt.DefaultView;
                UsersDataGrid.Columns[0].Visibility = Visibility.Hidden;
            }
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
    }
}
