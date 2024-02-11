using System.ComponentModel.DataAnnotations;
using Cats_Cafe_Accounting_System.Utilities;
using System.Data;
using Cats_Cafe_Accounting_System.Models;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;

namespace Cats_Cafe_Accounting_System.RegularClasses
{
    public class Breed
    {
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }

        public Breed(string _id, string _title)
        {
            Id = _id;
            Title = _title;
        }
        public Breed(string id)
        {
            DataRow row = DBContext.GetById("breeds", id);
            Id = row["id"].ToString();
            Title = row["title"].ToString();
        }

        public Breed()
        {

        }

        public static List<Breed> GetBreedsFromTable()
        {
            List<Breed> breeds = new List<Breed>();

            DataTable dataTable = DBContext.GetTable("breeds");

            foreach (DataRow row in dataTable.Rows)
            {
                Breed breed = new Breed(row["id"].ToString(), row["title"].ToString());
                breeds.Add(breed);
            }

            return breeds;
        }
    }
}
