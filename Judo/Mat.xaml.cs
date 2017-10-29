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

        SQLData db;
        DataTable dt;
        public Mat()
        {
            InitializeComponent();
            db = new SQLData();
            dt = new DataTable();
            LoadTable();
            
        }

        public void LoadTable()
        {
            dg.ItemsSource = db.RunSelect("Select [ID] as id , Name as 'Наименование' FROM Mat").DefaultView;

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

            if (dg.SelectedItems.Count > 0)
            {
                ChangeVisible();
                gb.Header = "Редактирование мата";

                string run = db.RunInsertUpdateDelete("SELECT  [Name] as 'Наименовние' from Mat ");
                tbName.Text = ((DataRowView)dg.Items[dg.SelectedIndex]).Row[1].ToString();
              
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
           
            if (gb.Header.ToString() == "Добавление мата")
            {
                string tb = db.RunInsertUpdateDelete("insert into [dbo].[Mat] ([Name]) values  ('" + tbName.Text + "')");
              
            }
            if (gb.Header.ToString() == "Редактирование мата")
            {

                DataRowView row = (DataRowView)dg.SelectedItems[0];
                string query = "update [dbo].[Mat] set [Name]= '" + tbName.Text +  "' where Id = '" + row["Id"] + "'";
                MessageBox.Show(db.RunInsertUpdateDelete(query));
               
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
                     string query = db.RunInsertUpdateDelete("DELETE FROM [dbo].[Mat] where Id = '" + ((DataRowView)dg.Items[dg.SelectedIndex]).Row[0].ToString() + "'");
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
