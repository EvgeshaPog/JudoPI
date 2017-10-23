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

namespace Judo
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }
        SQLData sql = new SQLData();
        Button flag;
        int rowCurrent = 0;
        int idComp = 0;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitDataСompetitors();
            DataRowView row = dgCompetitors.Items[0] as DataRowView;
            dgCompetitors.SelectedIndex = dgCompetitors.Items.IndexOf(row);
            VisibleFalse();
        }
        
        private void butAdd_Click(object sender, RoutedEventArgs e)
        {
            groupBox.Header = "Добавление";
            VisibleTrue();
            InitDateSportClub();
            InitDateCity();
            flag = butAdd;
        }

        private void butEdit_Click(object sender, RoutedEventArgs e)
        {
            groupBox.Header = "Редактирование";
            VisibleTrue();
            LoadForEdit();
            rowCurrent = dgCompetitors.SelectedIndex;
            flag = butEdit;
        }

        private void butDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteCompetitor();
            InitDataСompetitors();
        }
        private void butCancel_Click(object sender, RoutedEventArgs e)
        {
            VisibleFalse();
        }

        private void butSave_Click(object sender, RoutedEventArgs e)
        {
            if (flag == butAdd)
            {
                InsertCompetitor();
                InitDataСompetitors();
                DataRowView row = dgCompetitors.Items[dgCompetitors.Items.Count - 1] as DataRowView;
                dgCompetitors.SelectedIndex = dgCompetitors.Items.IndexOf(row);

            }
            else if (flag == butEdit)
            {
                UpdateCompetitor(idComp);
                InitDataСompetitors();
                DataRowView row = dgCompetitors.Items[rowCurrent] as DataRowView;
                dgCompetitors.SelectedIndex = dgCompetitors.Items.IndexOf(row);
            }
        }

        private void dpBirth_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime dateNow = DateTime.Now;
            DateTime clientyear = Convert.ToDateTime(dpBirth.Text);
            int year = dateNow.Year - clientyear.Year;
            if (dateNow.Month < clientyear.Month ||
                (dateNow.Month == clientyear.Month && dateNow.Day < clientyear.Day)) year--;
            tbAge.Text = year.ToString();
        }
        private void InitDataСompetitors()
        {
            string query = @"SELECT People.Id, People.FIO, People.DateOfBirth, People.Age, People.Weight,SportClub.Name as SportClub, City.Name AS City, People.Street 
                           FROM     People INNER JOIN
                                    SportClub ON People.Id_SportClub = SportClub.Id INNER JOIN
                                    City ON People.Id_City = City.Id";
            dgCompetitors.ItemsSource = sql.RunSelect(query).DefaultView;
        }
        private void InitDateSportClub()
        {
            string query = @"SELECT * FROM SportClub";
            cbSportClub.ItemsSource = sql.RunSelect(query).DefaultView;
            cbSportClub.DisplayMemberPath = "Name";
            cbSportClub.SelectedValuePath = "Id";
        }
        private void InitDateCity()
        {
            string query = @"SELECT * FROM City";
            cbCity.ItemsSource = sql.RunSelect(query).DefaultView;
            cbCity.DisplayMemberPath = "Name";
            cbCity.SelectedValuePath = "Id";
        }
        private void InsertCompetitor()
        {
            if (tbFirstName.Text != "" && tbLastName.Text != "" && tbAge.Text != "" && tbPatronymic.Text != "" && tbStret.Text != "" && tbWeight.Text != "" && cbSportClub.SelectedIndex != -1 && cbCity.SelectedIndex != -1)
            {
                string query = "Insert into People (FIO, DateOfBirth, Id_SportClub,Id_City, Street, Weight, Age) values('"
                                + tbLastName.Text + " " + tbFirstName.Text + " " + tbPatronymic.Text + "','"
                                + Convert.ToDateTime(dpBirth.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToInt32(cbSportClub.SelectedValue.ToString())
                                + "','" + Convert.ToInt32(cbCity.SelectedValue.ToString()) + "','" + tbStret.Text + Convert.ToDouble(tbWeight.Text)
                                + "','" + Convert.ToInt32(tbAge.Text) + "','" + Convert.ToDecimal(tbWeight.Text) + "')";

                MessageBox.Show(sql.RunInsertUpdateDelete(query));
                VisibleFalse();
            }
            else MessageBox.Show("Не все поля заполнены!", "Внимание");
        }
        private void UpdateCompetitor(int idComp)
        {

            string query = "Update People  set FIO ='" + tbLastName.Text + " " + tbFirstName.Text + " " + tbPatronymic.Text
                                            + "', DateOfBirth ='" + Convert.ToDateTime(dpBirth.Text).ToString("yyyy-MM-dd")
                                            + "', Age ='" + Convert.ToInt32(tbAge.Text)
                                            + "', Weight ='" + Convert.ToDouble(tbWeight.Text)
                                            + "', Street ='" + tbStret.Text
                                            + "', Id_SportClub ='" + Convert.ToInt32(cbSportClub.SelectedValue.ToString())
                                            + "', Id_City ='" + Convert.ToInt32(cbCity.SelectedValue.ToString())
                                            + "' where Id = " + idComp;

            MessageBox.Show(sql.RunInsertUpdateDelete(query));
            VisibleFalse();
        }
        private void DeleteCompetitor()
        {
            string query = "SELECT Id from PeopleBattleGroup where People_Id='" + Convert.ToInt32(((DataRowView)dgCompetitors.SelectedItems[dgCompetitors.SelectedIndex]).Row[0].ToString()) + "'";
            DataTable dt = sql.RunSelect(query);
            if (dt.Rows.Count == 0)
            {
                string qDelete = "Delete from People   where Id = '" + Convert.ToInt32(((DataRowView)dgCompetitors.SelectedItems[dgCompetitors.SelectedIndex]).Row[0].ToString()) + "'";
                MessageBox.Show(sql.RunInsertUpdateDelete(qDelete));
            }
            else MessageBox.Show("Нельзя удалить данного участника! Участник есть в бое!");
        }

        private void LoadForEdit()
        {
            DataRowView row = dgCompetitors.Items[dgCompetitors.SelectedIndex] as DataRowView;
            idComp = Convert.ToInt32(row.Row[0].ToString());

            string[] fio = row.Row[1].ToString().Split(' ');
            tbFirstName.Text = fio[0];
            tbLastName.Text = fio[1];
            tbPatronymic.Text = fio[2];
            tbAge.Text = row.Row[3].ToString();
            tbWeight.Text = row.Row[4].ToString();
            tbStret.Text = row.Row[7].ToString();
            dpBirth.Text = row.Row[2].ToString();
            InitDateSportClub();
            InitDateCity();

            cbCity.Text = row.Row[6].ToString();
            cbSportClub.Text = row.Row[5].ToString();
        }
        private void VisibleTrue()
        {
            (groupBox.Content as Grid).Visibility = Visibility.Visible;
            groupBox.Visibility = Visibility.Visible;
            dgCompetitors.Visibility = Visibility.Hidden;
            butAdd.IsEnabled = false;
            butDelete.IsEnabled = false;
            butEdit.IsEnabled = false;
        }
        private void VisibleFalse()
        {
            (groupBox.Content as Grid).Visibility = Visibility.Hidden;
            groupBox.Visibility = Visibility.Hidden;
            dgCompetitors.Visibility = Visibility.Visible;
            butAdd.IsEnabled = true;
            butDelete.IsEnabled = true;
            butEdit.IsEnabled = true;
            ClearTextBox();
        }
        private void ClearTextBox()
        {
            foreach (Control ctl in containersGb.Children)
            {
                if (ctl.GetType() == typeof(TextBox))
                    ((TextBox)ctl).Text = String.Empty;
                if (ctl.GetType() == typeof(ComboBox))
                    ((ComboBox)ctl).SelectedIndex = -1;
            }
            dpBirth.Text = DateTime.Now.ToString();
        }        
    }
}
