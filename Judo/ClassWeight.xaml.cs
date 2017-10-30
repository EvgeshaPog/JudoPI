using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
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
    /// Логика взаимодействия для ClassWeight.xaml
    /// </summary>
    public partial class ClassWeight : Window
    {
        DataTable dt;
        SQLData db = new SQLData();
        public ClassWeight()
        {
            InitializeComponent();
            VisibleFalse();
            LoadTable();
            db = new SQLData();
            dt = new DataTable();
        }
        
        public void LoadTable()// Вывод данных в таблицу
        {
            dt = db.RunSelect("SELECT Id as 'ID', WeightFrom as 'От', WeightTo as 'До' FROM WeightClass");
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
            tbOT.Clear();
            tbDO.Clear();
            VisibleTrue();
            groupBox.Header = "Добавить весовой класс";
        }

        private void but4_Click(object sender, RoutedEventArgs e)//Добавить ОК
        {
            if (tbOT.Text == "" || tbDO.Text == "")
            {
                MessageBox.Show("Не все поля заполнены!");
                return;
            }

            if (groupBox.Header.ToString() == "Добавить весовой класс")
            {
                string query = "insert into [dbo].[WeightClass] ([WeightFrom], [WeightTo]) values  ('" + Convert.ToInt32(tbOT.Text) + "', '" + Convert.ToInt32(tbDO.Text) + "')";
                MessageBox.Show(db.RunInsertUpdateDelete(query));
            }
            if (groupBox.Header.ToString() == "Редактировать весовой класс")
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                string query = "update [dbo].[WeightClass] set [WeightFrom]= '" + Convert.ToInt32(tbOT.Text) + "', [WeightTo]= '" + Convert.ToInt32(tbDO.Text) + "' where Id = '" + row["Id"] + "'";
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
                    string query = "DELETE FROM [dbo].[WeightClass] WHERE [Id] =" + row["Id"];
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
                groupBox.Header = "Редактировать весовой класс";
                DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                string query = "SELECT [Id] as 'Id', [WeightFrom] as 'От', [WeightTo] as 'До' from WeightClass Where [Id]= '" + row["Id"] + "'";
                db.RunSelect(query);
                tbOT.Text = dt.Rows[0][1].ToString();
                tbDO.Text = dt.Rows[0][2].ToString();
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
