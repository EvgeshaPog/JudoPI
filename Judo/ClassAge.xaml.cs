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
    /// Логика взаимодействия для ClassAge.xaml
    /// </summary>
    public partial class ClassAge : Window
    {
        DataTable dt;
        SQLData db = new SQLData();
        public ClassAge()
        {
            InitializeComponent();
            VisibleFalse();
            LoadTable();
            db = new SQLData();
            dt = new DataTable();
        }
        public void LoadTable()// Вывод данных в таблицу
        {
            dt = db.RunSelect("SELECT Id as 'ID', AgeFrom as 'От', AgeTo as 'До' FROM AgeClass");
            dataGrid.ItemsSource = dt.DefaultView;
            //dataGridGorod.Columns[0].Visibility = Visibility.Hidden;
            //dataGridSportC.Columns[0].Visibility = Visibility.Hidden;
        }
        void VisibleFalse()
        {
            groupBox.Visibility = Visibility.Hidden;
            butAdd.IsEnabled = true;
            butUpdate.IsEnabled = true;
            butRemove.IsEnabled = true;
            dataGrid.Visibility = Visibility.Visible;
        }
        void VisibleTrue()
        {
            groupBox.Visibility = Visibility.Visible;
            butAdd.IsEnabled = false;
            butUpdate.IsEnabled = false;
            butRemove.IsEnabled = false;
            dataGrid.Visibility = Visibility.Hidden;
        }

        private void but1_Click(object sender, RoutedEventArgs e)//Добавить
        {
            tbAgeOT.Clear();
            tbAgeDO.Clear();
            VisibleTrue();
            groupBox.Header = "Добавить возрастной класс";
        }

        private void but4_Click(object sender, RoutedEventArgs e)//Добавить ОК
        {
            if (tbAgeOT.Text == "" || tbAgeDO.Text == "")
            {
                MessageBox.Show("Не все поля заполнены!");
                return;
            }

            if (groupBox.Header.ToString() == "Добавить возрастной класс")
            {
                string query = "insert into [dbo].[AgeClass] ([AgeFrom], [AgeTo]) values  ('" + Convert.ToInt32(tbAgeOT.Text) + "', '" + Convert.ToInt32(tbAgeDO.Text) + "')";
                MessageBox.Show(db.RunInsertUpdateDelete(query));
            }
            if (groupBox.Header.ToString() == "Редактировать возрастной класс")
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                string query = "update [dbo].[AgeClass] set [AgeFrom]= '" + Convert.ToInt32(tbAgeOT.Text) + "', [AgeTo]= '" + Convert.ToInt32(tbAgeDO.Text) + "' where Id = '" + row["Id"] + "'";
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
            if (dataGrid.SelectedItem != null && dataGrid.SelectedIndex != -1)
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление записи", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {

                    DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                    string query = "DELETE FROM [dbo].[AgeClass] WHERE [Id] =" + row["Id"];
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

            if (dataGrid.SelectedItems.Count > 0)
            {
                groupBox.Header = "Редактировать возрастной класс";
                DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                string query = "SELECT [Id] as 'Id', [AgeFrom] as 'От', [AgeTo] as 'До' from AgeClass Where [Id]= '" + row["Id"] + "'";
                db.RunSelect(query);
                tbAgeOT.Text = dt.Rows[0][1].ToString();
                tbAgeDO.Text = dt.Rows[0][2].ToString();
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
