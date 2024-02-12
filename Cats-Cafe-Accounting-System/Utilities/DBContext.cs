using System.Windows;
using System.Collections.Generic;
using System.Data;
using System;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;
using NPOI.OpenXmlFormats.Dml.Chart;

namespace Cats_Cafe_Accounting_System.Utilities
{
    public class DBContext
    {
        private static readonly string connectionStr = "Server=localhost;database=cat_cafe;user=root;password=Vorobushek_16jbl";
        static readonly MySqlConnection connection = new MySqlConnection(connectionStr);

        public static void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        public static void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        public static DataTable GetTable(string table)
        {
            DataTable dataTable = new DataTable();
            try
            {
                OpenConnection();
                string query = $"SELECT * FROM {table}";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);

                adapter.Fill(dataTable);
                CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");
            }
            return dataTable;
        }

        public static List<string> GetAttributes(string table)
        {
            List<string> attributes = new List<string>();
            DataTable dataTable = GetTable(table);
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                attributes.Add(dataTable.Columns[i].ToString());
            }
            return attributes;
        }

        public static void AddNote(string table, List<string> newvalues, int toSkip = 1)
        {
            List<string> list = GetAttributes(table).Skip(toSkip).ToList();
            newvalues = newvalues.Skip(toSkip).ToList();
            var columns = string.Join(", ", list);
            var values = string.Join(", ", newvalues.Select(v => $"'{v}'"));

            string sqlQuery = $"INSERT INTO {table} ({columns}) VALUES ({values})";

            MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
            OpenConnection();
            cmd.ExecuteNonQuery();
            CloseConnection();
        }

        public static void AddNote<T>(string tableName, T item)
        {
            var prop1 = typeof(T).GetProperties().Where(prop => prop.Name != "Id").ToList();
            var prop2 = GetAttributes(tableName).Skip(1).ToList();

            var columns = string.Join(", ", prop1.Where(x => prop2.Contains(x.Name)).Select(prop => prop.Name ));
            var values = string.Join(", ", prop1.Where(x => prop2.Contains(x.Name)).Select(prop => $"'{prop.GetValue(item)}'"));

            string sqlQuery = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

            MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
            OpenConnection();
            cmd.ExecuteNonQuery();
            CloseConnection();
        }

        public static void UpdateNote<T>(string table, T item)
        {
            var prop1 = typeof(T).GetProperties().Where(prop => prop.Name != "Id").ToList();
            var prop2 = GetAttributes(table).ToList();

            var set = "";
            for (int i = 0; i < prop1.Count; i++)
            {
                var prop = prop1[i];
                var value = "";
                if (prop2.Contains(prop.Name))
                {
                    if (prop.GetValue(item) is DateTime)
                        value = $"'{ToDateTime(prop.GetValue(item).ToString())}'";
                    else
                        value = $"'{prop.GetValue(item)}'";
                    set += $"{prop.Name} = {value}, ";
                }
            }
            set = set.TrimEnd(',', ' ');

            var id = typeof(T).GetProperty("Id").GetValue(item);

            string sqlQuery = $"UPDATE {table} SET {set} WHERE Id = '{id}'";

            MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
            OpenConnection();
            cmd.ExecuteNonQuery();
            CloseConnection();
        }

        public static void UpdateNote(string table, List<string> newvalues, int toSkip = 1)
        {
            int id = int.Parse(newvalues[0]);
            List<string> list = GetAttributes(table).Skip(toSkip).ToList();
            newvalues = newvalues.Skip(toSkip).ToList();

            var set = "";
            for (int i = 0; i < list.Count; i++)
            {
                string attribute = list[i] + " = '" + newvalues[i] + "', ";
                set += attribute;
            }
            set = set.Remove(set.Length - 2);
            string sqlQuery = $"UPDATE {table} SET {set} WHERE id = '{id}'";

            MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
            OpenConnection();
            cmd.ExecuteNonQuery();
            CloseConnection();
        }

        public static void DeleteNote(string table, string id)
        {
            string sqlQuery = $"DELETE FROM {table} WHERE id = '{id}'";
            string sqlForeignQuery = $"DELETE FROM tickets WHERE petId = '{id}'";

            MySqlCommand cmd = new MySqlCommand(sqlForeignQuery, connection);
            OpenConnection();
            cmd.ExecuteNonQuery();
            cmd = new MySqlCommand(sqlQuery, connection);
            OpenConnection();
            cmd.ExecuteNonQuery();
            CloseConnection();
        }
        public static DataRow GetById<T>(string foreignTable, T id)
        {
            try
            {
                OpenConnection();
                string query = $"SELECT * FROM {foreignTable} WHERE {foreignTable}.id = '{id}'";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                CloseConnection();

                if (dataTable.Rows.Count > 0)
                    return dataTable.Rows[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error");
            }

            return null;
        }
        public static bool UsernameIsExist(NetworkCredential credential)
        {
            bool validUser;
            using (var command = new MySqlCommand())
            {
                OpenConnection();
                command.Connection = connection;
                command.CommandText = "select *from employees where username=@username";
                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = credential.UserName;
                validUser = command.ExecuteScalar() == null ? false : true;
                CloseConnection();
            }
            return validUser;
        }
        public static bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var command = new MySqlCommand())
            {
                OpenConnection();
                command.Connection = connection;
                command.CommandText = "select *from employees where username=@username and password=@password";
                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = credential.UserName;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = credential.Password;
                validUser = command.ExecuteScalar() == null ? false : true;
                CloseConnection() ;
            }
            return validUser;
        }

        public static string ToDateTime(string last)
        {
            DateTime dt = DateTime.ParseExact(last, "dd.MM.yyyy h:mm:ss", null);
            return dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString();
        }
    }
}
