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

namespace Judo
{
    /// <summary>
    /// Логика взаимодействия для fightGroup.xaml
    /// </summary>
    public partial class fightGroup : Window
    {
        public fightGroup()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Windows.OfType<FormJury>().First().Show();
        }

        private void butAdd_Click(object sender, RoutedEventArgs e)
        {

          
        DataGridTextColumn c1 = new DataGridTextColumn();
        c1.Header = "Num";
        c1.Binding = new Binding("Num");
        c1.Width = 110;
            dataGrid.Columns.Add(c1);
        DataGridTextColumn c2 = new DataGridTextColumn();
        c2.Header = "Start";
        c2.Width = 110;
        c2.Binding = new Binding("Start");
            dataGrid.Columns.Add(c2);
        DataGridTextColumn c3 = new DataGridTextColumn();
        c3.Header = "Finich";
        c3.Width = 110;
        c3.Binding = new Binding("Finich");
            dataGrid.Columns.Add(c3);

            dataGrid.Items.Add(new Item() { Num = 1, Start = "2012, 8, 15", Finich = "2012, 9, 15" });
            dataGrid.Items.Add(new Item() { Num = 2, Start = "2012, 12, 15", Finich = "2013, 2, 1" });
            dataGrid.Items.Add(new Item() { Num = 3, Start = "2012, 8, 1", Finich = "2012, 11, 15" });
        }
    }
    public class Item
    {
        public int Num { get; set; }
        public string Start { get; set; }
        public string Finich { get; set; }
    }



    
}
