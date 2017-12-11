using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Judo
{
    /// <summary>
    /// Логика взаимодействия для WindowControl.xaml
    /// </summary>
    public partial class WindowControl : Window
    {
        SQLData db = new SQLData();
        DataTable dt;
        int index, idmat;
        bool start, start1;
        DispatcherTimer timer;
        DispatcherTimer timer1;
        int IDFirstCompetitor = 0;
        int IDSecondCompetitor = 0;
        int IDBattle = 0;
        int Round = 0;
        public WindowControl()
        {
            InitializeComponent();
        
            start = false;
            start1 = false;
            dt = new DataTable();
            index = 0;
            idmat = 1;
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer1 = new DispatcherTimer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = new TimeSpan(0, 0, 1);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if(Convert.ToInt32(label17.Content) < 20)
            label17.Content = (Convert.ToInt32(label17.Content)+1).ToString();
            else
            {
                timer.Stop();

                if (Convert.ToInt32(label11.Content) == Convert.ToInt32(label14.Content))
                    GetResult("никто");

                if (Convert.ToInt32(label11.Content) > Convert.ToInt32(label14.Content))
                {
                    text1.Background = Brushes.Yellow;
                    GetResult(text1.Text);
                }

                if (Convert.ToInt32(label11.Content) < Convert.ToInt32(label14.Content))
                {
                    text2.Background = Brushes.Yellow;
                    GetResult(text2.Text);
                }

                start = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label18.Content = (Convert.ToInt32(label18.Content) + 1).ToString();
        }

        public void LoadTable()// Вывод данных в таблицу
        {
            dt = db.RunSelect(@"SELECT People.FIO, People_1.FIO AS FIO1, People_1.Weight  AS Weight1, People_1.Age  AS Age1, People.Weight, People.Age,
                                  PeopleBattleGroup.Id AS IDPeopleBattleGroup1, PBG1.Id  as IDPeopleBattleGroup2, Battle.Id as BattleID
                FROM            Battle INNER JOIN
				PeopleBattleGroup On Battle.Id_People1 = PeopleBattleGroup.Id INNER JOIN
                PeopleBattleGroup AS PBG1 On Battle.Id_People2 = PBG1.Id INNER JOIN
				People On PeopleBattleGroup.People_Id = People.Id INNER JOIN
				People AS People_1 On PBG1.People_Id = People_1.Id
				Where (Battle.Result is NULL OR Battle.Result='') and Battle.Id_Mat=" + idmat);
            NextBattle();
        }

        private void NextBattle()
        {
            if(dt.Rows.Count>=index+1)
            {
                text1.Text = dt.Rows[index][0].ToString();
                text2.Text = dt.Rows[index][1].ToString();
                label6.Content = dt.Rows[index][4].ToString() + "-"+ dt.Rows[index][5].ToString();
                label7.Content = dt.Rows[index][2].ToString() + "-" + dt.Rows[index][3].ToString();
                if (dt.Rows.Count > index + 1)
                {
                    label8.Content = dt.Rows[index + 1][0].ToString();
                    label9.Content = dt.Rows[index + 1][1].ToString();
                  
                }
                else
                {
                    label8.Content = "Последний участник";
                    label9.Content = "Последний участник";
                }

                //Ринат
                IDFirstCompetitor = Convert.ToInt32(dt.Rows[index][6]);
                IDSecondCompetitor = Convert.ToInt32(dt.Rows[index][7]);
                IDBattle = Convert.ToInt32(dt.Rows[index][8]);
            }
            

            label11.Content = "0";
            label12.Content = "0";
            label13.Content = "0";
            label14.Content = "0";
            label15.Content = "0";
            label16.Content = "0";
            label17.Content = "0";
            label18.Content = "0";
            text1.Background = Brushes.White;
            text2.Background = Brushes.White;


          

            //Юля 
            index += 1;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Windows.OfType<FormJury>().First().Show();
        }

        private void butTimer_Click(object sender, RoutedEventArgs e)
        {
         if(!start)
            {
                start = true;
                timer.Start();
            }  
         else
            {
                start = false;
                timer.Stop();
            }
        }

        private void butEndBattle_Click(object sender, RoutedEventArgs e)
        {
            NextBattle();
        }

        private void butWazaAriWhite_Click(object sender, RoutedEventArgs e)
        {
            label11.Content = (Convert.ToInt32(label11.Content) + 1).ToString();
            if(Convert.ToInt32(label11.Content)>1)
            {
                text1.Background = Brushes.Yellow; 
                GetResult(text1.Text);
            }
        }
        
        private void GetResult(string name)
        {
            timer.Stop();
            start = false;
            MessageBox.Show("Победил " + name);
            int victorianman;
            if (text1.Text == name)
            {
                 victorianman = IDFirstCompetitor;
            }
            else
            {
                victorianman = IDSecondCompetitor;
            }

                db.RunInsertUpdateDelete("Update Battle set Result = " + victorianman + " Where Id = "+ IDBattle+"");
            // Если это был последний бой 1 раунда, тогда составить новые второго раунда, если возможно
            int Checker = db.RunSelect("Select Result From Battle Where Result is NULL").Rows.Count;
            if (Checker == 0)
            {
                StartNewRound();
            }
            else
            {
                NextBattle();
            }
        }

        public void SetMat(string mat, int idmat)
        {
            tbMAT.Text = mat;
            this.idmat = idmat;
        }

        private void butWazaAriRed_Click(object sender, RoutedEventArgs e)
        {
            label14.Content = (Convert.ToInt32(label14.Content) + 1).ToString();
            if (Convert.ToInt32(label14.Content) > 1)
            {
                text2.Background = Brushes.Yellow;
                GetResult(text2.Text);
            }
        }

        private void butIpponWhite_Click(object sender, RoutedEventArgs e)
        {
            label12.Content = (Convert.ToInt32(label12.Content) + 1).ToString();
            if (Convert.ToInt32(label12.Content) > 0)
            {
                text1.Background = Brushes.Yellow;
                GetResult(text1.Text);
            }
        }

        private void butIpponRed_Click(object sender, RoutedEventArgs e)
        {
            label15.Content = (Convert.ToInt32(label15.Content) + 1).ToString();
            if (Convert.ToInt32(label15.Content) > 0)
            {
                text2.Background = Brushes.Yellow;
                GetResult(text2.Text);
            }
        }

        private void butWarningWhite_Click(object sender, RoutedEventArgs e)
        {
            label13.Content = (Convert.ToInt32(label13.Content) + 1).ToString();
            if (Convert.ToInt32(label13.Content) > 1)
            {
                text2.Background = Brushes.Yellow;
                GetResult(text2.Text);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTable();
        }

        private void butWarningRed_Click(object sender, RoutedEventArgs e)
        {
            label16.Content = (Convert.ToInt32(label16.Content) + 1).ToString();
            if (Convert.ToInt32(label16.Content) > 1)
            {
                text1.Background = Brushes.Yellow;
                GetResult(text1.Text);
            }
        }

        private void butHoldingTime_Click(object sender, RoutedEventArgs e)
        {
            if (!start1)
            {
                start1 = true;
                timer1.Start();
                timer.Stop();
            }
            else
            {
                start1 = false;
                timer1.Stop();
                timer.Start();
            }
        }

        public void StartNewRound()
        {

            DataTable ForRound = db.RunSelect("Select Distinct Raund  From Battle");
            Round = Convert.ToInt32(ForRound.Rows[ForRound.Rows.Count - 1][0]);


            DataTable Mats = db.RunSelect("SELECT * From Mat");
            DataTable AllBattleGroupsInUse = db.RunSelect(@"SELECT Distinct Battle_Id, Id_Weight, Id_Age
                                                                From PeopleBattleGroup inner join BattleGroup on Battle_Id =BattleGroup.Id ");

            string[] alreadyUsedGroups = new string[AllBattleGroupsInUse.Rows.Count];
            DataTable СопутствующиеГруппы;
            DataTable CompetitorsInGroup;
            int[] victoryPeople;
            int matid = 0;
            int counter = 0;
            int counterForWinners = 0;
            for (int group = 0; group < AllBattleGroupsInUse.Rows.Count; group++)
            {
                counterForWinners = 0;
                if (!alreadyUsedGroups.Contains(AllBattleGroupsInUse.Rows[group][0].ToString()))
                { 
                    СопутствующиеГруппы = db.RunSelect("Select Id From BattleGroup where Id_Weight = " + Convert.ToInt32(AllBattleGroupsInUse.Rows[group][1]) + " and Id_Age = " + Convert.ToInt32(AllBattleGroupsInUse.Rows[group][2]) + "");
                    victoryPeople = new int[СопутствующиеГруппы.Rows.Count];
                    foreach (DataRow dr in СопутствующиеГруппы.Rows)
                    {
                        alreadyUsedGroups[counter] = Convert.ToString(dr[0]);
                        counter++;
                    }

                    for (int СопутствующаяГруппа = 0; СопутствующаяГруппа < СопутствующиеГруппы.Rows.Count; СопутствующаяГруппа++)
                    {
                        DataTable mydt = db.RunSelect(@"Select  Result
                                                          From Battle inner join PeopleBattleGroup on Id_People1 = PeopleBattleGroup.Id
                                                                Where Battle_Id = "+ Convert.ToInt32(СопутствующиеГруппы.Rows[СопутствующаяГруппа][0]) + " and Raund = "+Round+" ");

                        ForSearchMax[] current = new ForSearchMax[mydt.Rows.Count];
                        string[] usednames = new string[mydt.Rows.Count];
                        counter = 0;
                        for (int row = 0; row < mydt.Rows.Count; row++)
                        {
                            if (!usednames.Contains(mydt.Rows[row][0].ToString()))
                            {
                                usednames[counter] = mydt.Rows[row][0].ToString();
                                ForSearchMax newnew = new ForSearchMax();
                                newnew.ID = Convert.ToInt32(mydt.Rows[row][0]);
                                current[counter] = newnew;
                                counter++;
                            }
                            else
                            {
                                for (int code = 0; code < current.Length; code++)
                                {
                                    try
                                    {
                                        if (current[code].ID == Convert.ToInt32(mydt.Rows[row][0]))
                                        {
                                            current[code].Povtor += 1;
                                            break;
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }
                        int maxid = SearchMaxNumber(current);                
                        victoryPeople[counterForWinners] = maxid;
                        counterForWinners++;

                    }

                    if (victoryPeople[counterForWinners-1] != 0)
                        for (int i = 0; i < victoryPeople.Length - 1; i++)
                        {
                            for (int j = i + 1; j < victoryPeople.Length; j++)
                            {
                                db.RunInsertUpdateDelete(@"Insert into Battle values(NULL," + Convert.ToInt32(Mats.Rows[matid][0]) + ",2," + victoryPeople[i] + ", " + victoryPeople[j] + ",NULL,NULL,NULL)");
                                matid = (matid + 1) % Mats.Rows.Count;
                            }
                        }
                    else
                    {
                        DataTable dtttt = db.RunSelect(@"Select AgeFrom, AgeTo, WeightFrom,WeightTo, FIO
                                                           From PeopleBattleGroup inner join People on People_Id = People.Id 
                                                                                  inner join BattleGroup on Battle_Id =BattleGroup.Id
                                                                                  inner join WeightClass on   BattleGroup.Id_Weight  = WeightClass.Id
                                                                                  inner join AgeClass on BattleGroup.Id_Age = AgeClass.Id
                                                                   Where PeopleBattleGroup.Id = " + victoryPeople[0] + "");
                        MessageBox.Show("В группе (" + dtttt.Rows[0][0].ToString() + " - " + dtttt.Rows[0][1].ToString() + "лет; " + dtttt.Rows[0][2].ToString() + "-" + dtttt.Rows[0][3].ToString() + " вес)" + " победил " + dtttt.Rows[0][4].ToString());
                        break;
                    }
                    
                }
            }
        }

        public int SearchMaxNumber(ForSearchMax[] current)
        {
            int max = 0;
            int maxId = 0;
            foreach (ForSearchMax d in current)
            {try
                {
                    if (max < d.Povtor) { max = d.Povtor; maxId = d.ID; }
                }
                catch { }
            }
            return maxId;


        }
    }
   public class ForSearchMax
    {
        public int ID;
        public int Povtor = 1;
        
    }
}
