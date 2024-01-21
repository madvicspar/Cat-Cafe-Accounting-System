using System.Windows;
using System.Collections.Generic;
using System.Data;
using System;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;

namespace Cats_Cafe_Accounting_System.Utilities
{
    public class DBContext
    {
        public static string connectionStr = "Server=localhost;database=cat_cafe;user=root;password=Vorobushek_16jbl";
        static MySqlConnection connection = new MySqlConnection(connectionStr);

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

            MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
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
        public static bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select *from employee where id=@username and pass=@password";
                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = credential.UserName;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = credential.Password;
                validUser = command.ExecuteScalar() == null ? false : true;
            }
            return validUser;
        }
    }
}
