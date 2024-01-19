using System.ComponentModel.DataAnnotations;

namespace Cats_Cafe_Accounting_System.RegularClasses
{
    public class Gender
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public Gender(int _id, string _title)
        {
            Id = _id;
            Title = _title;
        }
    }
}
