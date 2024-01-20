using System.ComponentModel.DataAnnotations;
using Cats_Cafe_Accounting_System.Utilities;
using System.Data;

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
            DataRow row = DBContext.GetById("breed", id);
            Id = row["id"].ToString();
            Title = row["title"].ToString();
        }
    }
}
