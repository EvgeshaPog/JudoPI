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
    /// Логика взаимодействия для Gorod.xaml
    /// </summary>
    public partial class Gorod : Window
    {
      
        DataTable dt;
        SQLData db = new SQLData();
        public Gorod()
        {
            InitializeComponent();
            VisibleFalse();
            LoadTable();
            db = new SQLData();
            dt = new DataTable();
        }
          

        public void LoadTable()// Вывод данных в таблицу
        {
            dt = db.RunSelect("SELECT Id as 'ID', Name as 'Название', PochtaIndex as 'Почтовый индекс' FROM City");
            dataGridGorod.ItemsSource = dt.DefaultView;
            //dataGridGorod.Columns[0].Visibility = Visibility.Hidden;
            //dataGridSportC.Columns[0].Visibility = Visibility.Hidden;
        }
        void VisibleFalse()
        {
            gb1.Visibility = Visibility.Hidden;
            but1.IsEnabled = true;
            but2.IsEnabled = true;
            but3.IsEnabled = true;
            dataGridGorod.Visibility = Visibility.Visible;
        }
        void VisibleTrue()
        {
            gb1.Visibility = Visibility.Visible;
            but1.IsEnabled = false;
            but2.IsEnabled = false;
            but3.IsEnabled = false;
            dataGridGorod.Visibility = Visibility.Hidden;
        }

        private void but1_Click(object sender, RoutedEventArgs e)//Добавить
        {
            tb1.Clear();
            tb2.Clear();
            VisibleTrue();
            gb1.Header = "Добавить город";
        }

        private void but4_Click(object sender, RoutedEventArgs e)//Добавить ОК
        {
            if (tb1.Text == "" || tb2.Text == "")
            {
                MessageBox.Show("Не все поля заполнены!");
                return;
            }
    
            if (gb1.Header.ToString() == "Добавить город")
            {
                string query = "insert into [dbo].[City] ([Name], [PochtaIndex]) values  ('" + tb1.Text + "', '" + tb2.Text + "')";
                MessageBox.Show(db.RunInsertUpdateDelete(query));
            }
            if (gb1.Header.ToString() == "Редактировать город")
            {
                DataRowView row = (DataRowView)dataGridGorod.SelectedItems[0];
                string query = "update [dbo].[City] set [Name]= '" + tb1.Text + "', [PochtaIndex]= '" + tb2.Text + "' where Id = '" + row["Id"] + "'";
                MessageBox.Show(db.RunInsertUpdateDelete(query));
            }
            LoadTable();//вывод данных в таблицу
            VisibleFalse();
        }

        private void but5_Click(object sender, RoutedEventArgs e)
        {
            VisibleFalse();
        }

        private void but3_Click(object sender, RoutedEventArgs e)// Удаление
        {
            if (dataGridGorod.SelectedItem != null && dataGridGorod.SelectedIndex != -1)
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление записи", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                   
                    DataRowView row = (DataRowView)dataGridGorod.SelectedItems[0];
                    string query = "DELETE FROM [dbo].[City] WHERE [Id] =" + row["Id"];
                    MessageBox.Show(db.RunInsertUpdateDelete(query));
                    LoadTable();
                }
            }
            else
            {
                MessageBox.Show("Выберите запись!");
            }

        }

        private void but2_Click(object sender, RoutedEventArgs e)//Редактировать
        {
            VisibleTrue();
           
            if (dataGridGorod.SelectedItems.Count > 0)
            {
                gb1.Header = "Редактировать город";
                DataRowView row = (DataRowView)dataGridGorod.SelectedItems[0];
                string query = "SELECT [Id] as 'Id', [Name] as 'Название', [PochtaIndex] as 'Почтовый индекс' from City Where [Id]= '" + row["Id"] + "'";
                db.RunSelect(query);
                tb1.Text = dt.Rows[0][1].ToString();
                tb2.Text = dt.Rows[0][2].ToString();
            }
            else
            {
                VisibleFalse(); 
                MessageBox.Show("Выберите строку!");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Windows.OfType<FormAdmin>().First().Show();
        }
    }
}
    

