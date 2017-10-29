using System;
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
using System.Windows.Shapes;
using System.Net.Mail;
using System.Net;

namespace Judo
{
    /// <summary>
    /// Логика взаимодействия для Skleroz.xaml
    /// </summary>
    public partial class Skleroz : Window
    {
        SQLData db;
        DataTable dt;
        public Skleroz()
        {
            InitializeComponent();
            db = new SQLData();
            dt = new DataTable();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Windows.OfType<MainWindow>().First().Show();
        }

        private void butLogin_Click(object sender, RoutedEventArgs e)
        {
            if (tbEmail.Text != "")
            {
                dt = db.RunSelect("Select Login, Password from [User] where Email='" + tbEmail.Text + "'");

                if (dt.Rows.Count > 0)
                {
                    SendMail();
                }
                else
                    MessageBox.Show("Такая почта не зарегистрирована!");
            }
            else
                MessageBox.Show("Введите почту!");
        }

        private void SendMail()
        {
           try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("garkan12@mail.ru");
                mail.To.Add(new MailAddress(tbEmail.Text));
                mail.Subject = "Напоминание логина и пароля";
                mail.Body = "Ваш логин: "+dt.Rows[0][0].ToString()+"\nВаш пароль: "+dt.Rows[0][1].ToString();
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.mail.ru";
                client.Port = 2525;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("garkan12@mail.ru", "Yjcjyjdf");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                mail.Dispose();
                MessageBox.Show("Логин и пароль направлены на почту");
            }
            catch (Exception e)
            {
              throw new Exception("Mail.Send: " + e.Message);
            }
        }
     }
}
