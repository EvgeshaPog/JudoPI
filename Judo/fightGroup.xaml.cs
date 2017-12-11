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
        public int idOfGrouupforPainting = 0;
        public int currcolor = 0;
        SolidColorBrush CurrentBrush = new SolidColorBrush(Colors.White);
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
                if (newkid.Weight >= 55)
                    newkid.WeightClass = AllWeightClass[AllWeightClass.Length - 1];

                if (newkid.Weight != 5)
                {

                    int IDOfGroup = Convert.ToInt32(db.RunSelect(@"Select BattleGroup.Id 
                                                From (BattleGroup inner join AgeClass on BattleGroup.Id_Age = AgeClass.Id)
                                                                  inner join WeightClass on BattleGroup.Id_Weight = WeightClass.Id
                                                                        WHERE AgeClass.Id = '" + newkid.AgeClass.Num + "' and WeightClass.Id = '" + newkid.WeightClass.Num + "'").Rows[0][0]);
                    db.RunInsertUpdateDelete("Insert into PeopleBattleGroup values('" + newkid.Num + "', '" + IDOfGroup + "','" + DateTime.Now.Date + "')");

                    newkid.IDOfBattleGroup = IDOfGroup;
                    newkid.IDBattleG = Convert.ToInt32(db.RunSelect(@"select top 1 Id  FROM PeopleBattleGroup Order by Id Desc").Rows[0][0]);
                    AllKids[i] = newkid;
                }
            }




         


            //  разбить на группы если в них больше чем 5  (запариться с базой) Метод по соритировке



         

            dt = db.RunSelect("Select Id, Id_Weight, Id_Age From BattleGroup");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                MYSort(Convert.ToInt32(dt.Rows[i][0]), Convert.ToInt32(dt.Rows[i][1]), Convert.ToInt32(dt.Rows[i][2]));
            }



            DataTable Mats = db.RunSelect("SELECT * From Mat");
           DataTable AllBattleGroupsInUse = db.RunSelect("SELECT Distinct Battle_Id From PeopleBattleGroup");
            DataTable CompetitorsInGroup;
            int matid = 0;
            for (int group = 0; group < AllBattleGroupsInUse.Rows.Count; group++)
            {
                CompetitorsInGroup = db.RunSelect("Select Id  From PeopleBattleGroup Where Battle_Id = "+ AllBattleGroupsInUse.Rows[group][0] + "");
                for (int i = 0; i < CompetitorsInGroup.Rows.Count-1; i++)
                {
                    for (int j = i+1; j < CompetitorsInGroup.Rows.Count; j++)
                    {
                        db.RunInsertUpdateDelete(@"Insert into Battle values(NULL," + Convert.ToInt32(Mats.Rows[matid][0]) + ",1," + Convert.ToInt32(CompetitorsInGroup.Rows[i][0]) + ", " + Convert.ToInt32(CompetitorsInGroup.Rows[j][0]) + ",NULL,NULL,NULL)");
                        matid = (matid + 1) % Mats.Rows.Count;
                    }
                }
            }
                //    if (!AllKids[i].HaveBattlealready)
                //    {
                //        for (int j = i + 1; j < AllKids.Length; j++)
                //        {
                //            if (AllKids[i].IDOfBattleGroup == AllKids[j].IDOfBattleGroup)
                //            {
                //                
                //                AllKids[i].HaveBattlealready = true;
                //                AllKids[j].HaveBattlealready = true;
                //                break;
                //            }
                //        }
                //    }
                //}
                // конец











            }










        public void MYSort( int IDGroupOtkuda, int weight, int age )
        {
            DataTable dt = db.RunSelect(@"Select People_Id, DateOfCompeitors  
                                                 From PeopleBattleGroup
                                                    WHERE Battle_Id ='" + IDGroupOtkuda + "'");
            int CountCompetitorsinGroup = dt.Rows.Count;
            int countRepeat = 0;
            int rowscount = 0;
            if (CountCompetitorsinGroup >= 6)
            {
                if ((CountCompetitorsinGroup-5) % 5 >= 3) // их хотя бы больше 5 и при делении на 5, будет группа с хотябы 3 людьми;
                {
                       rowscount = 5;
                       countRepeat = (CountCompetitorsinGroup / 5) -1;

                    for (int i = 0; i <= countRepeat; i++)
                    {
                        db.RunInsertUpdateDelete(@"Insert into BattleGroup values(" + weight + "," + age + ")");
                        int lastgroup = Convert.ToInt32(db.RunSelect(@"select top 1 Id  FROM BattleGroup Order by Id Desc").Rows[0][0]);

                        if (i != countRepeat)
                            for (int rowCompetitor = rowscount; rowCompetitor < rowscount+5; rowCompetitor++)
                            {
                                db.RunInsertUpdateDelete("Update PeopleBattleGroup set Battle_Id = "+ lastgroup + " Where People_Id = "+ Convert.ToInt32(dt.Rows[rowCompetitor][0]) + "");
                            }
                        else
                            for (int rowCompetitor = CountCompetitorsinGroup - 3; rowCompetitor < CountCompetitorsinGroup; rowCompetitor++)
                            {
                                db.RunInsertUpdateDelete("Update PeopleBattleGroup set Battle_Id = " + lastgroup + " Where People_Id = " + Convert.ToInt32(dt.Rows[rowCompetitor][0]) + "");
                            }
                        rowscount += 5;


                    }
                    return;
                }

                if ((CountCompetitorsinGroup-4) % 4 >= 3)
                {
                    rowscount = 4;
                    countRepeat = (CountCompetitorsinGroup / 4) - 1;

                    for (int i = 0; i <= countRepeat; i++)
                    {
                        db.RunInsertUpdateDelete(@"Insert into Battle values(" + weight + "," + age + ")");
                        int lastgroup = Convert.ToInt32(db.RunSelect(@"select top 1 Id  FROM BattleGroup Order by Id Desc").Rows[0][0]);

                        if (i != countRepeat)
                            for (int rowCompetitor = rowscount; rowCompetitor < rowscount + 4; rowCompetitor++)
                            {
                                db.RunInsertUpdateDelete("Update PeopleBattleGroup set Battle_Id = " + lastgroup + " Where People_Id = " + Convert.ToInt32(dt.Rows[rowCompetitor][0]) + "");
                            }
                        else
                            for (int rowCompetitor = CountCompetitorsinGroup - 3; rowCompetitor < CountCompetitorsinGroup; rowCompetitor++)
                            {
                                db.RunInsertUpdateDelete("Update PeopleBattleGroup set Battle_Id = " + lastgroup + " Where People_Id = " + Convert.ToInt32(dt.Rows[rowCompetitor][0]) + "");
                            }
                        rowscount += 4;
                    }
                    return;
                }

                //if (CountCompetitorsinGroup % 3 >= 0)
                //{
                //    countRepeat = (CountCompetitorsinGroup / 3);
                //    return;
                //}

                //if ((CountCompetitorsinGroup - 5) % 5 >= 0)
                //{
                //    countRepeat = (CountCompetitorsinGroup / 3);
                //    return;
                //}

            }
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

        private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            SolidColorBrush[] hb = new SolidColorBrush[9];
            hb[0] = new SolidColorBrush(Colors.AliceBlue);
            hb[1] = new SolidColorBrush(Colors.Crimson);
            hb[2] = new SolidColorBrush(Colors.FloralWhite);
            hb[3] = new SolidColorBrush(Colors.Honeydew);
            hb[4] = new SolidColorBrush(Colors.Gray);
            hb[5] = new SolidColorBrush(Colors.Chartreuse);
            hb[6] = new SolidColorBrush(Colors.LightSkyBlue);
            hb[7] = new SolidColorBrush(Colors.Linen);
            hb[8] = new SolidColorBrush(Colors.Tan);

            

            try
            {
                DataRowView product = (DataRowView)e.Row.DataContext;
                int id = Convert.ToInt32(product.Row.ItemArray[0]);
                int battlegroupcurr = Convert.ToInt32(db.RunSelect("Select Battle_Id From PeopleBattleGroup Where People_Id = " + id + "").Rows[0][0]);


                if (idOfGrouupforPainting != battlegroupcurr)
                {
                    idOfGrouupforPainting = battlegroupcurr;
                    CurrentBrush = hb[currcolor];
                    currcolor++;
                    if (currcolor == 9)
                    { currcolor = 0; }
                }
                e.Row.Background = CurrentBrush;
            }
            catch 
            { }
        }
    }


    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string club { get; set; }
        public string Town { get; set; }
        public string Weight { get; set; }
        public string Age { get; set; }
        public string street { get; set; }
        public string Gender { get; set; }
        public DateTime DAtee { get; set; }


    }
        public class Kid
        {
            string fio, sportclub, city, street, gender;
            int age, num;
            double weight;
            DateTime dateofbirth;
            AgeClass ageclass;
            WeightClass weightclass;
            int iDOfBattleGroup;
            bool haveBattlealready = false;
            int iDBattleG;

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
            public int IDOfBattleGroup { get { return iDOfBattleGroup; } set { iDOfBattleGroup = value; } }
            public bool HaveBattlealready { get { return haveBattlealready; } set { haveBattlealready = value; } }
            public int IDBattleG { get { return iDBattleG; } set { iDBattleG = value; } }
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

        
    }

