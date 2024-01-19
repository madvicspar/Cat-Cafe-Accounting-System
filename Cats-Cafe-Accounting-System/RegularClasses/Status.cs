using System.ComponentModel.DataAnnotations;

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
    }
}
