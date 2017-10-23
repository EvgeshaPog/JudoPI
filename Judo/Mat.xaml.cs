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
using System.Data;
using System.Data.SqlClient;

namespace Judo
{
    /// <summary>
    /// Логика взаимодействия для Mat.xaml
    /// </summary>
    public partial class Mat : Window

    {

       string connectionString = @"Data Source=DESKTOP-I5A2IJU\SAGANKRIS;Initial Catalog=Djudo;Integrated Security=True";// Кристина Саган

        public Mat()
        {
            InitializeComponent();
            LoadTable();
        }

        public void LoadTable()
        {
           // string connectionString = @"Data Source=DESKTOP-I5A2IJU\SAGANKRIS;Initial Catalog=Djudo;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string CmdString = "SELECT Id as 'ID', Name as 'Наименование' FROM Mat";
            SqlCommand cmd = new SqlCommand(CmdString, connection);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("SportClub");
            sda.Fill(dt);
            dg.ItemsSource = dt.DefaultView;
            connection.Close();

        }

        void ChangeVisible()
        {
            gb.Visibility = Visibility.Visible;
            dg.Visibility = Visibility.Hidden;
            butAdd.Visibility = Visibility.Hidden;
            butEdit.Visibility = Visibility.Hidden;
            butDelete.Visibility = Visibility.Hidden;
        }

        void ChangeClear()
        {
            tbName.Clear();
            gb.Visibility = Visibility.Hidden;
            dg.Visibility = Visibility.Visible;
            butAdd.Visibility = Visibility.Visible;
            butEdit.Visibility = Visibility.Visible;
            butDelete.Visibility = Visibility.Visible;

        }

        private void butAdd_Click(object sender, RoutedEventArgs e)
        {
            ChangeVisible();
            gb.Header = "Добавление мата";
        }

        private void butEdit_Click(object sender, RoutedEventArgs e)
        {
            
            SqlConnection conn = new SqlConnection(connectionString);
            if (dg.SelectedItems.Count > 0)
            {
                ChangeVisible();
                gb.Header = "Редактирование мата";
                DataRowView row = (DataRowView)dg.SelectedItems[0];

                string query = "SELECT [Id] as 'Id', [Name] as 'Наименовние' from Mat Where [Id]= '" + row["Id"] + "'";
                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                DataSet DS = new DataSet();
                sda.Fill(DS, "Mat");
                tbName.Text = DS.Tables["Mat"].Rows[0][1].ToString();
                conn.Close();
            }
            else
            { 
                MessageBox.Show("Выберите строку, которую необходимо отредактировать!");
            }
        }

        private void butOK_Click(object sender, RoutedEventArgs e)
        {

            if (tbName.Text == "")
            {
                MessageBox.Show("Не все поля заполнены!");
                return;
            }
            SqlConnection conn = new SqlConnection(connectionString);
            if (gb.Header.ToString() == "Добавление мата")
            {
                string query = "insert into [dbo].[Mat] ([Name]) values  ('" + tbName.Text + "')";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            if (gb.Header.ToString() == "Редактирование мата")
            {
                DataRowView row = (DataRowView)dg.SelectedItems[0];
                string query = "update [dbo].[Mat] set [Name]= '" + tbName.Text + "'  where Id = '" + row["Id"] + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            LoadTable();
            ChangeClear();
        }

            private void butCancel_Click(object sender, RoutedEventArgs e)
        {
            ChangeClear();
        }

        private void butDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItem != null && dg.SelectedIndex != -1)
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление записи", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    SqlConnection connection = new SqlConnection(connectionString);
                    DataRowView row = (DataRowView)dg.SelectedItems[0];
                    string query = "DELETE FROM [dbo].[Mat] WHERE [Id] =" + row["Id"];
                    SqlCommand cmd = new SqlCommand(query, connection);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    LoadTable();
                }
            }
            else
            {
                MessageBox.Show("Выберите запись, которую необходимо удалить!");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Windows.OfType<FormAdmin>().First().Show();
        }
    }
}
