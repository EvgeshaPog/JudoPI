﻿using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Judo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLData db;
        DataTable dt;

        public MainWindow()
        {
            InitializeComponent();
            db = new SQLData();
            dt = new DataTable();
        }

        private void butLogin_Click(object sender, RoutedEventArgs e)
        {
            if (tbLogin.Text != "" && tbPassword.Password.ToString() != "")
            {
                dt = db.RunSelect("Select FIO, Admin from [User] where Login='" + tbLogin.Text + "' and Password='" + tbPassword.Password.ToString() + "'");

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][1].ToString() == "True")
                    {
                        FormAdmin fa = new FormAdmin();
                        fa.Title = dt.Rows[0][0].ToString();
                        fa.Show();
                        this.Hide();
                    }
                    else
                    {
                       FormJury fj = new FormJury();
                        fj.Title = dt.Rows[0][0].ToString();
                        fj.Show();
                        this.Hide();
                    }
                }
                else
                    MessageBox.Show("Неверно введен логин или пароль!");
            }
            else
                MessageBox.Show("Заполните все поля!");
        }

        private void butRegistration_Click(object sender, RoutedEventArgs e)
        {
            NewUser nu = new NewUser();
            nu.Show();
            Hide();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            tbLogin.Text = "";
            tbPassword.Password = "";
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
           FormGuest fg = new FormGuest();
            fg.Show();
            Hide();
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            Skleroz s = new Skleroz();
            s.Show();
            Hide();
        }
    }
}
