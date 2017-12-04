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
    /// Логика взаимодействия для fightGroup.xaml
    /// </summary>
    public partial class fightGroup : Window
    {
        SQLData db = new SQLData();
        public fightGroup()
        {
            InitializeComponent();

            DataTable dt = db.RunSelect(@"Select Distinct AgeClass.Id,AgeFrom,AgeTo From  (AgeClass inner join BattleGroup on AgeClass.Id =BattleGroup.Id_Age)
                                                                                           inner join PeopleBattleGroup on BattleGroup.Id = PeopleBattleGroup.Battle_Id");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBox.Items.Add(dt.Rows[i][1] + "-" + dt.Rows[i][2]);
            }

            dt = db.RunSelect(@"Select Distinct WeightClass.Id,WeightFrom,WeightTo From (WeightClass inner join BattleGroup on WeightClass.Id = BattleGroup.Id_Weight)
                                                                                             inner join PeopleBattleGroup on BattleGroup.Id = PeopleBattleGroup.Battle_Id");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBox1.Items.Add(dt.Rows[i][1] + "-" + dt.Rows[i][2]);
            }


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Windows.OfType<FormJury>().First().Show();
        }

        private void butAdd_Click(object sender, RoutedEventArgs e)
        {

            DataTable dt = db.RunSelect(@"Select * From AgeClass");
            AgeClass[] AllAgeClass = new AgeClass[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                AgeClass newAgeClass = new AgeClass();
                newAgeClass.Num = Convert.ToInt32(dt.Rows[i][0]);
                newAgeClass.From = Convert.ToInt32(dt.Rows[i][1]);
                newAgeClass.To = Convert.ToInt32(dt.Rows[i][2]);
                AllAgeClass[i] = newAgeClass;
            }


            dt = db.RunSelect(@"Select * From WeightClass");
            WeightClass[] AllWeightClass = new WeightClass[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                WeightClass newWeightClass = new WeightClass();
                newWeightClass.Num = Convert.ToInt32(dt.Rows[i][0]);
                newWeightClass.From = Convert.ToInt32(dt.Rows[i][1]);
                newWeightClass.To = Convert.ToInt32(dt.Rows[i][2]);
                AllWeightClass[i] = newWeightClass;
            }

            dt = db.RunSelect(@"Select People.Id , People.FIO,SportClub.Name, City.Name,
                                                Weight,Age,Street,Gender,DateOfBirth
                                                    From (People inner join SportClub on People.Id_SportClub = SportClub.Id)
                                                                 inner join City      on People.Id_City      = City.Id");
            Kid[] AllKids = new Kid[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Kid newkid = new Kid();
                newkid.Num = Convert.ToInt32(dt.Rows[i][0]);
                newkid.FIO = dt.Rows[i][1].ToString();
                newkid.Sportclub = dt.Rows[i][2].ToString();
                newkid.City = dt.Rows[i][3].ToString();
                newkid.Weight = Convert.ToDouble(dt.Rows[i][4].ToString());
                newkid.Age = Convert.ToInt32(dt.Rows[i][5]);
                newkid.Street = dt.Rows[i][6].ToString();
                newkid.Gender = dt.Rows[i][7].ToString();
                newkid.Dateofbirth = Convert.ToDateTime(dt.Rows[i][8].ToString()).Date;
                foreach (AgeClass ac in AllAgeClass)
                {
                    if (newkid.Age >= ac.From && newkid.Age <= ac.To)
                    {
                        newkid.AgeClass = ac;
                        break;
                    }
                }

                foreach (WeightClass wc in AllWeightClass)
                {
                    if (newkid.Weight >= wc.From && newkid.Weight <= wc.To)
                    {
                        newkid.WeightClass = wc;
                        break;
                    }
                }
                if(newkid.Weight>= 55)
                    newkid.WeightClass = AllWeightClass[AllWeightClass.Length-1];

                if (newkid.Weight != 5)
                {
                    AllKids[i] = newkid;
                    int IDOfGroup = Convert.ToInt32(db.RunSelect(@"Select BattleGroup.Id 
                                                From (BattleGroup inner join AgeClass on BattleGroup.Id_Age = AgeClass.Id)
                                                                  inner join WeightClass on BattleGroup.Id_Weight = WeightClass.Id
                                                                        WHERE AgeClass.Id = '" + newkid.AgeClass.Num + "' and WeightClass.Id = '" + newkid.WeightClass.Num + "'").Rows[0][0]);
                    db.RunInsertUpdateDelete("Insert into PeopleBattleGroup values('" + newkid.Num + "', '" + IDOfGroup + "','" + DateTime.Now.Date + "')");
                }
            }





















        }
        public class SortbyBattleGroups
        {
            public int Num { get; set; }
            public string Start { get; set; }
            public string Finich { get; set; }

            // писать супер код по сортировке



        }
        public class Kid
        {
            string fio, sportclub, city, street, gender;
            int age, num;
            double weight;
            DateTime dateofbirth;
            AgeClass ageclass;
            WeightClass weightclass;

            public int Num { get { return num; } set { num = value; } }
            public string FIO { get { return fio; } set { fio = value; } }
            public string Sportclub { get { return sportclub; } set { sportclub = value; } }
            public string City { get { return city; } set { city = value; } }
            public double Weight { get { return weight; } set { weight = value; } }
            public int Age { get { return age; } set { age = value; } }
            public string Street { get { return street; } set { street = value; } }
            public string Gender { get { return gender; } set { gender = value; } }
            public DateTime Dateofbirth { get { return dateofbirth; } set { dateofbirth = value; } }
            public AgeClass AgeClass { get { return ageclass; } set { ageclass = value; } }
            public WeightClass WeightClass { get { return weightclass; } set { weightclass = value; } }
        }
        public class AgeClass
        {
            int num, from, to;
            public int Num { get { return num; } set { num = value; } }
            public int From { get { return from; } set { from = value; } }
            public int To { get { return to; } set { to = value; } }
        }
        public class WeightClass
        {
            int num, from, to;
            public int Num { get { return num; } set { num = value; } }
            public int From { get { return from; } set { from = value; } }
            public int To { get { return to; } set { to = value; } }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Возраст  1 (6-7 лет)
            if (comboBox.SelectedIndex > -1 && comboBox1.SelectedIndex > -1)
            {
                string[] agespan = comboBox.SelectedValue.ToString().Split('-');
                int IDofAgeClass = Convert.ToInt32(db.RunSelect("Select Id FROM AgeClass WHERE AgeFrom = '" + agespan[0] + "'").Rows[0][0]);
                agespan = comboBox1.SelectedValue.ToString().Split('-');
                int IDofWeightClass = Convert.ToInt32(db.RunSelect("Select Id FROM WeightClass WHERE WeightFrom = '" + agespan[0] + "'").Rows[0][0]);

                DataTable dt = db.RunSelect(@"SELECT People.Id as код  , People.FIO as ФИО,SportClub.Name as Спортклуб, City.Name as Город,
                                                Weight Вес,Age Возраст,Street Улица,Gender Пол,DateOfBirth Датарождения
                                                    From PeopleBattleGroup inner join People on PeopleBattleGroup.People_Id = People.Id
                                                                 inner join SportClub on People.Id_SportClub = SportClub.Id
                                                                 inner join City      on People.Id_City      = City.Id
                                                                 inner join BattleGroup on PeopleBattleGroup.Battle_Id = BattleGroup.Id
                                                                        WHERE Id_Age = '" + IDofAgeClass + "' and Id_Weight = '" + IDofWeightClass + "'");
             
                dataGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Вес 1 (21-23)
        }
    }
}
