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
    /// Логика взаимодействия для SportC.xaml
    /// </summary>
    public partial class SportC : Window
    {
        string connesctionString = @"Data Source=ACER\MSSQLSERVER1;Initial Catalog=Djudo;Integrated Security=True";
        public SportC()
        {
            InitializeComponent();
            VisibleFalse();
            LoadTable();
        }

        private void but1_Click(object sender, RoutedEventArgs e)
        {
            VisibleTrue();
            gb1.Header = "Добавить спортивный клуб";
        }

        public void LoadTable()// Вывод данных в таблицу
        {
            //string connesctionString = @"Data Source=ACER\MSSQLSERVER1;Initial Catalog=Djudo;Integrated Security=True";
            SqlConnection conn = new SqlConnection(connesctionString);
            conn.Open();
            string CmdString = "SELECT Id as 'ID', Name as 'Название клуба' FROM SportClub";
            SqlCommand cmd = new SqlCommand(CmdString, conn);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable("SportClub");
            sda.Fill(dt);
            dataGridSportC.ItemsSource = dt.DefaultView;
            conn.Close();

        }
        void VisibleFalse()
        {
            gb1.Visibility = Visibility.Hidden;
            but1.IsEnabled = true;
            but2.IsEnabled = true;
            but3.IsEnabled = true;
            dataGridSportC.Visibility = Visibility.Visible;
            //ClearTextBox();
        }
        void VisibleTrue()
        {
            gb1.Visibility = Visibility.Visible;
            but1.IsEnabled = false;
            but2.IsEnabled = false;
            but3.IsEnabled = false;
            dataGridSportC.Visibility = Visibility.Hidden;
        }

        private void but2_Click(object sender, RoutedEventArgs e)
        {
            VisibleTrue();
            SqlConnection conn = new SqlConnection(connesctionString);
            if (dataGridSportC.SelectedItems.Count > 0)
            {
                gb1.Header = "Редактировать спортивный клуб";
                DataRowView row = (DataRowView)dataGridSportC.SelectedItems[0];

                string query = "SELECT [Id] as 'Id', [Name] as 'Название' from SportClub Where [Id]= '" + row["Id"] + "'";
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataSet myDS = new DataSet();
                da.Fill(myDS, "SportClub");
                tb1.Text = myDS.Tables["SportClub"].Rows[0][1].ToString();
                conn.Close();
            }
            else
            {
                VisibleFalse();
                MessageBox.Show("Выберите строку!");
            }
        }

        private void but3_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridSportC.SelectedItem != null && dataGridSportC.SelectedIndex != -1)
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление записи", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    SqlConnection conn = new SqlConnection(connesctionString);
                    DataRowView row = (DataRowView)dataGridSportC.SelectedItems[0];
                    string query = "DELETE FROM [dbo].[SportClub] WHERE [Id] =" + row["Id"];
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadTable();
                }
            }
            else
            {
                MessageBox.Show("Выберите запись!");
            }

        }

        private void but4_Click(object sender, RoutedEventArgs e)
        {
            if (tb1.Text == "")
            {
                MessageBox.Show("Не все поля заполнены!");
                return;
            }
            SqlConnection conn = new SqlConnection(connesctionString);
            if (gb1.Header.ToString() == "Добавить спортивный клуб")
            {
                string query = "insert into [dbo].[SportClub] ([Name]) values  ('" + tb1.Text + "')";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            if (gb1.Header.ToString() == "Редактировать спортивный клуб")
            {
                DataRowView row = (DataRowView)dataGridSportC.SelectedItems[0];
                string query = "update [dbo].[SportClub] set [Name]= '" + tb1.Text + "'  where Id = '" + row["Id"] + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            LoadTable();//вывод данных в таблицу
            VisibleFalse();
        }

        private void but5_Click(object sender, RoutedEventArgs e)
        {
            VisibleFalse();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Windows.OfType<FormAdmin>().First().Show();
        }
    }
}