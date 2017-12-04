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
        public WindowControl()
        {
            InitializeComponent();
        
            start = false;
            start1 = false;
            dt = new DataTable();
            index = 0;
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
            dt = db.RunSelect(@"SELECT        People.FIO, People_1.FIO AS FIO1, People_1.Weight  AS Weight1, People_1.Age  AS Age1, People.Weight, People.Age
                FROM            Battle INNER JOIN
				PeopleBattleGroup On Battle.Id_People1 = PeopleBattleGroup.Id INNER JOIN
                PeopleBattleGroup AS PBG1 On Battle.Id_People2 = PBG1.Id INNER JOIN
				People On PeopleBattleGroup.People_Id = People.Id INNER JOIN
				People AS People_1 On PBG1.People_Id = People_1.Id
				Where (Battle.Result is NULL OR Battle.Result='') and Battle.Id_Mat="+idmat);
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
            }
            index += 1;

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
            NextBattle();
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
    }
}
