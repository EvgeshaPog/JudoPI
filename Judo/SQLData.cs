﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace Judo
{
    class SQLData
    {
             string connectionString = "Data Source=DESKTOP-V4KR3NR;Initial Catalog=Djudo;Integrated Security=True";// РИНАТ Повелитель Класса SQLDATA
        //      string connectionString = "Data Source=HOUMPC\\HOUMPC;Initial Catalog=Djudo;Integrated Security=SSPI";// Евгения Погорелова
          //   string connectionString = @"Data Source=DESKTOP-K1FLG14\SQLEXPRESS;Initial Catalog=Djudo;Integrated Security=True";// Юлия Носонова
        //     string connectionString = "Data Source=DESKTOP-V4KR3NR;Initial Catalog=Djudo;Integrated Security=True";// Кристина Саган
        //     string connectionString = "Data Source=DESKTOP-V4KR3NR;Initial Catalog=Djudo;Integrated Security=True";// Екатерина Путенихина
         //    string connectionString = "Data Source=DESKTOP-V4KR3NR;Initial Catalog=новая;Integrated Security=True";// РИНАТ Повелитель Класса SQLDATA
             /*  string connectionString = "Data Source=HOUMPC\\HOUMPC;Initial Catalog=Djudo;Integrated Security=SSPI"*/// Евгения Погорелова
        //     string connectionString = "Data Source=DESKTOP-V4KR3NR;Initial Catalog=новая;Integrated Security=True";// Юлия Носонова
        //     string connectionString = "Data Source=DESKTOP-V4KR3NR;Initial Catalog=новая;Integrated Security=True";// Кристина Саган
        //     string connectionString = "Data Source=DESKTOP-V4KR3NR;Initial Catalog=новая;Integrated Security=True";// Екатерина Путенихина
        //    string connectionString = "Data Source=PC;Initial Catalog=Djudo;Integrated Security=True";// Алина Сафина
        //string connectionString = @"Data Source=ACER\MSSQLSERVER1;Initial Catalog=Djudo;Integrated Security=True";// Евгения Комлева
        //     string connectionString = "Data Source=DESKTOP-V4KR3NR;Initial Catalog=Djudo;Integrated Security=True";// Светлана Зарипова
        //     string connectionString = "Data Source=DESKTOP-V4KR3NR;Initial Catalog=Djudo;Integrated Security=True";// Анастасия Обливанцева
      //  string connectionString = @"Data Source=ACER\MSSQLSERVER1;Initial Catalog=Djudo;Integrated Security=True";// Евгения Комлева
        //     string connectionString = "Data Source=DESKTOP-V4KR3NR;Initial Catalog=новая;Integrated Security=True";// Светлана Зарипова
        //     string connectionString = "Data Source=DESKTOP-V4KR3NR;Initial Catalog=новая;Integrated Security=True";// Анастасия Обливанцева

        public DataTable RunSelect(string zapros)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(zapros, connection);
            DataTable dt = new DataTable();
            connection.Open();
            dataadapter.Fill(dt);
            connection.Close();
            return dt;
        }

        public string RunInsertUpdateDelete(string zapros)
        {
            //try
            //{
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                connection.Open();
                cmd.CommandText = zapros;
                cmd.ExecuteNonQuery();
                cmd.Clone();
                connection.Close();
            //}
            //catch (Exception exc)
            //{
                //return ("Произошла ошибка сервера:\r\n" + exc.Message);
            //}
            return "Операция прошла успешно";
         

        }
    }
}
