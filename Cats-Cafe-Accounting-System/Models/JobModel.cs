using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cats_Cafe_Accounting_System.Models
{
    public class JobModel : ObservableObject
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public float Rate { get; set; }
        public JobModel()
        {

        }
    }
}
