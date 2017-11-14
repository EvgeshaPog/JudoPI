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
using System.Data.OleDb;

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
            else if (flag == butImport)
            {
                InsertCompetitorFromDataGrid();
                InitDataСompetitors();
                DataRowView row = dgCompetitors.Items[dgCompetitors.Items.Count - 1] as DataRowView;
                dgCompetitors.SelectedIndex = dgCompetitors.Items.IndexOf(row);
            }
        }

        private void dpBirth_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            tbAge.Text = GetAge(dpBirth.Text).ToString();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Windows.OfType<FormAdmin>().First().Show();
        }
        private void butImport_Click(object sender, RoutedEventArgs e)
        {
            VisibleTrueImport();
            ImportExcel();
            flag = butImport;
        }
        private int GetAge(string dateBirth)
        {
            DateTime dateNow = DateTime.Now;
            DateTime clientyear = Convert.ToDateTime(dateBirth);
            int year = dateNow.Year - clientyear.Year;
            if (dateNow.Month < clientyear.Month ||
                (dateNow.Month == clientyear.Month && dateNow.Day < clientyear.Day)) year--;
            return year;
        }
        private void InitDataСompetitors()
        {
            string query = @"SELECT People.Id, People.FIO, People.DateOfBirth, People.Age, People.Gender, People.Weight,SportClub.Name as SportClub, City.Name AS City, People.Street 
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
            string gender = "";
            if (tbFirstName.Text != "" && tbLastName.Text != "" && tbAge.Text != "" && tbPatronymic.Text != "" && tbStret.Text != "" && tbWeight.Text != "" && cbSportClub.SelectedIndex != -1 && cbCity.SelectedIndex != -1)
            {
                if (rbF.IsChecked == true)
                    gender = "Ж";
                else gender = "М";
                string query = "Insert into People (FIO, DateOfBirth, Gender, Id_SportClub,Id_City, Street, Weight, Age) values('"
                                + tbLastName.Text + " " + tbFirstName.Text + " " + tbPatronymic.Text + "','"
                                + Convert.ToDateTime(dpBirth.Text).ToString("yyyy-MM-dd") + "','" + gender + "','" + Convert.ToInt32(cbSportClub.SelectedValue.ToString())
                                + "','" + Convert.ToInt32(cbCity.SelectedValue.ToString()) + "','" + tbStret.Text
                                + "','" + Convert.ToInt32(tbWeight.Text) + "','" + Convert.ToDouble(tbAge.Text) + "')";

                MessageBox.Show(sql.RunInsertUpdateDelete(query));
                VisibleFalse();
            }
            else MessageBox.Show("Не все поля заполнены!", "Внимание");
        }
        private void UpdateCompetitor(int idComp)
        {
            string gender = "";
            if (rbF.IsChecked == true)
                gender = "Ж";
            else gender = "М";
            string query = "Update People  set FIO ='" + tbLastName.Text + " " + tbFirstName.Text + " " + tbPatronymic.Text
                                            + "', DateOfBirth ='" + Convert.ToDateTime(dpBirth.Text).ToString("yyyy-MM-dd")
                                            + "', Gender ='" +gender
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
            string query = "SELECT Id from PeopleBattleGroup where People_Id='" + Convert.ToInt32(((DataRowView)dgCompetitors.Items[dgCompetitors.SelectedIndex]).Row[0].ToString()) + "'";
            DataTable dt = sql.RunSelect(query);
            if (dt.Rows.Count == 0)
            {
                string qDelete = "Delete from People   where Id = '" + Convert.ToInt32(((DataRowView)dgCompetitors.Items[dgCompetitors.SelectedIndex]).Row[0].ToString()) + "'";
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
            string gender = row.Row[2].ToString();
            if (gender == "М" || gender == "m")
                rbM.IsChecked=true;
            else rbF.IsChecked=true;
            tbAge.Text = row.Row[4].ToString();
            tbWeight.Text = row.Row[5].ToString();
            tbStret.Text = row.Row[8].ToString();
            dpBirth.Text = row.Row[3].ToString();
            InitDateSportClub();
            InitDateCity();

            cbCity.Text = row.Row[7].ToString();
            cbSportClub.Text = row.Row[6].ToString();
        }
        private void VisibleTrue()
        {
            (groupBox.Content as Grid).Visibility = Visibility.Visible;
            groupBox.Visibility = Visibility.Visible;
            dgCompetitors.Visibility = Visibility.Hidden;
            dgCompetitorsLoad.Visibility = Visibility.Hidden;
            butAdd.IsEnabled = false;
            butDelete.IsEnabled = false;
            butEdit.IsEnabled = false;
            butImport.IsEnabled = false;
            rbM.IsChecked = true;
            rbF.IsChecked = false;
        }
        private void VisibleFalse()
        {
            (groupBox.Content as Grid).Visibility = Visibility.Hidden;
            groupBox.Visibility = Visibility.Hidden;
            dgCompetitorsLoad.Visibility = Visibility.Hidden;
            dgCompetitors.Visibility = Visibility.Visible;
            butAdd.IsEnabled = true;
            butDelete.IsEnabled = true;
            butEdit.IsEnabled = true;
            butImport.IsEnabled = true;
            rbM.IsChecked = true;
            rbF.IsChecked = false;
            ClearTextBox();
        }
        private void VisibleTrueImport()
        {
            (groupBox.Content as Grid).Visibility = Visibility.Visible;
            groupBox.Visibility = Visibility.Visible;
            dgCompetitors.Visibility = Visibility.Hidden;
            dgCompetitorsLoad.Visibility = Visibility.Visible;
            butAdd.IsEnabled = false;
            butDelete.IsEnabled = false;
            butEdit.IsEnabled = false;
            butImport.IsEnabled = false;
            rbM.IsChecked = true;
            rbF.IsChecked = false;
            ClearTextBox();
            ClearDataGrid();
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
        private void ClearDataGrid()
        {

            foreach (Control ctl in containersGb.Children)
            {
                if (ctl.GetType() == typeof(DataGrid))
                    ((DataGrid)ctl).ItemsSource = null;
            }
        }
        private void ImportExcel()
        {
            DataTable dtLoad = new DataTable();
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();
            openDialog.Filter = "Файл Excel|*.XLSX;*.XLS";
            var result = openDialog.ShowDialog();
            if (result == false)
            {
                MessageBox.Show("Файл не выбран!", "Информация", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                return;
            }
            //string fileName = System.IO.Path.GetFileName(openDialog.FileName);
            Microsoft.Office.Interop.Excel.Application ObjExcel = new Microsoft.Office.Interop.Excel.Application();
            //Открываем книгу.                                                                                                                                                        
            Microsoft.Office.Interop.Excel.Workbook ObjWorkBook = ObjExcel.Workbooks.Open(openDialog.FileName, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
            //Выбираем таблицу(лист).
            Microsoft.Office.Interop.Excel.Worksheet ObjWorkSheet = ObjWorkBook.Worksheets.get_Item(1);// получаем ссылку на первый лист //

            var lastCell = ObjWorkSheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell);
            dtLoad.Columns.Add("FIO");
            dtLoad.Columns.Add("DateOfBirth");
            dtLoad.Columns.Add("Gender");
            dtLoad.Columns.Add("Weight");
            dtLoad.Columns.Add("SportClub");
            dtLoad.Columns.Add("City");
            dtLoad.Columns.Add("Street");

            for (int j = 0; j < (int)lastCell.Row; j++)
            {
                if (ObjWorkSheet.Cells[j + 2, 1].Text.ToString() != "" && ObjWorkSheet.Cells[j + 2, 1].Text.ToString() != null)
                {
                    dtLoad.Rows.Add();
                    for (int i = 0; i < (int)lastCell.Column; i++)
                    {
                        if (ObjWorkSheet.Cells[j + 2, i + 1].Text.ToString() != "" && ObjWorkSheet.Cells[j + 2, i + 1].Text.ToString() != null)
                            dtLoad.Rows[j][i] = ObjWorkSheet.Cells[j + 2, i + 1].Text.ToString();//считал текст
                    }
                }
            }

            ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
            ObjExcel.Quit(); // вышел из Excel
            GC.Collect(); // убрал за собой
            dgCompetitorsLoad.ItemsSource = dtLoad.DefaultView;

        }
        private void InsertCompetitorFromDataGrid()
        {
            string query = "", fio = "", street = "", gender="";
            DateTime dateBirth;
            string[] city = new string[2];
            double weight = 0;
            int age = 0, idCity = 0, idSportClub = 0;
            DataRowView row;
            for (int i = 0; i < dgCompetitorsLoad.Items.Count; i++)
            {
                row = dgCompetitorsLoad.Items[i] as DataRowView;
                if (row != null)
                {

                    fio = row.Row[0].ToString();
                    dateBirth = Convert.ToDateTime(row.Row[1].ToString());
                    gender = row.Row[2].ToString();
                    weight = Convert.ToDouble(row.Row[3].ToString());
                    idSportClub = GetIdSportClub(row.Row[4].ToString());
                    city = row.Row[5].ToString().Split(' ');
                    idCity = GetIdCity(city[1], city[0]);
                    street = row.Row[6].ToString();
                    age = GetAge(row.Row[1].ToString());
                    query = "Insert into People (FIO, DateOfBirth,Gender, Id_SportClub,Id_City, Street, Weight, Age) values('"
                            + fio + "','" + dateBirth + "','" + gender+"','" + idSportClub + "','" + idCity + "','"
                            + street + "','" + weight + "','" + age + "')";
                    sql.RunInsertUpdateDelete(query);
                }
            }
            VisibleFalse();
        }
        private int GetIdSportClub(string sportClub)
        {
            string query = @"SELECT Id FROM SportClub Where Name='" + sportClub + "'";
            DataTable dt = sql.RunSelect(query);
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0][0].ToString());
            else
            {
                query = @"Insert into SportClub (Name) values('" + sportClub + "')";
                sql.RunInsertUpdateDelete(query);
                query = @"SELECT Id FROM SportClub Where Name='" + sportClub + "'";
                return Convert.ToInt32(sql.RunSelect(query).Rows[0][0].ToString());
            }
        }
        private int GetIdCity(string city, string index)
        {
            string query = @"SELECT Id FROM City Where Name='" + city + "'";
            DataTable dt = sql.RunSelect(query);
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0][0].ToString());
            else
            {
                query = @"Insert into City (Name, PochtaIndex) values('" + city + "', '" + index + "')";
                sql.RunInsertUpdateDelete(query);
                query = @"SELECT Id FROM City Where Name='" + city + "'";
                return Convert.ToInt32(sql.RunSelect(query).Rows[0][0].ToString());
            }
        }

        private void dgCompetitors_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            groupBox.Header = "Редактирование";
            VisibleTrue();
            LoadForEdit();
            rowCurrent = dgCompetitors.SelectedIndex;
            flag = butEdit;
        }
    }
}
