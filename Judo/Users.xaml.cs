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
            LoadTable();
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
            DataGrid.Visibility = Visibility.Hidden;
            ClearTextBox();
        }
        void VisibleFalse()
        {
            groupBox.Visibility = Visibility.Hidden;
            butAdd.IsEnabled = true;
            butDelete.IsEnabled = true;
            butEdit.IsEnabled = true;
            DataGrid.Visibility = Visibility.Visible;
        }

        private void butEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VisibleTrue();
                groupBox.Header = "Редактировать";
                DataRowView row = (DataRowView)DataGrid.SelectedItems[0];
                string query = "SELECT [Id] as 'Id', [FIO] as 'ФИО', [Login] as 'Логин', [Password] as 'Пароль', [Admin] as 'Администратор' FROM [dbo].[User] Where [Id]= '" + row["Id"] + "'";
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataSet myDS = new DataSet();
                da.Fill(myDS, "User");

                string[] words = myDS.Tables["User"].Rows[0][1].ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                textBox_F.Text = words[0];
                textBox_I.Text = words[1];
                textBox_O.Text = words[2];

                textBox_Login.Text = myDS.Tables["User"].Rows[0][2].ToString();
                textBox_Password.Text = myDS.Tables["User"].Rows[0][3].ToString();
                CheckBoxAdmin.IsChecked = (myDS.Tables["User"].Rows[0][4].ToString() == "1") ? true : false;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Что-то пошло не так, попробуйте снова. Ошибка:" + ex.Message);
            }
        }

        private void butDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataGrid.SelectedItem != null && DataGrid.SelectedIndex != -1)
                {
                    //DialogResult dialogResult = MessageBox.Show("Вы уверены, что хотите удалить эту запись таблицы?", "Проверка", MessageBoxButtons.YesNo);
                    //if (dialogResult == DialogResult.Yes)
                    //{
                    DataRowView row = (DataRowView)DataGrid.SelectedItems[0];
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
            catch (Exception ex)
            {
                MessageBox.Show("Что-то пошло не так, попробуйте снова. Ошибка:" + ex.Message);
            }
        }

        private void butOK_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_F.Text == "" || textBox_I.Text == "" || textBox_Login.Text == "" || textBox_Password.Text == "")
            {
                MessageBox.Show("Заполните все поля отмеченные звездочкой (*)");
                return;
            }

            string FIO = textBox_F.Text + "|" + textBox_I.Text + "|" + textBox_O.Text;
            string admin = (CheckBoxAdmin.IsChecked == true) ? "1" : "0";

            if (groupBox.Header.ToString() == "Добавить")
            {
                string query = "insert into [dbo].[User] ([FIO], [Login], [Password], [Admin]) values  ('" + FIO + "', '" + textBox_Login.Text
                    + ", '" + textBox_Password.Text + "', '" + admin + "')";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            if (groupBox.Header.ToString() == "Редактировать")
            {
                DataRowView row = (DataRowView)DataGrid.SelectedItems[0];
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
                DataTable dt = new DataTable("Employee");
                sda.Fill(dt);
                DataGrid.ItemsSource = dt.DefaultView;

                //DataGrid.Columns[0].Visibility = Visibility.Hidden;
            }

        }

        void ClearTextBox()
        {
            foreach (Control c in containerCanvas.Children)
            {
                foreach (Control ctl in containerCanvas.Children)
                {
                    if (ctl.GetType() == typeof(CheckBox))
                        ((CheckBox)ctl).IsChecked = false;
                    if (ctl.GetType() == typeof(TextBox))
                        ((TextBox)ctl).Text = String.Empty;
                }
            }
        }
    }
}
