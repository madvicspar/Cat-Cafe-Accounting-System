using System;
using Cats_Cafe_Accounting_System.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Collections.Generic;

namespace Cats_Cafe_Accounting_System.RegularClasses
{
    public class Status
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public Status(int _id, string _title)
        {
            Id = _id;
            Title = _title;
        }
        public Status(int id)
        {
            DataRow row = DBContext.GetById("statuses", id);
            Id = Convert.ToInt32(row["id"]);
            Title = row["title"].ToString();
        }

        public Status() { }
        public static List<Status> GetStatusesFromTable()
        {
            List<Status> statuses = new List<Status>();

            DataTable dataTable = DBContext.GetTable("statuses");

            foreach (DataRow row in dataTable.Rows)
            {
                Status status = new Status(Convert.ToInt32(row["id"]), row["title"].ToString());
                statuses.Add(status);
            }

            return statuses;
        }
    }
}
