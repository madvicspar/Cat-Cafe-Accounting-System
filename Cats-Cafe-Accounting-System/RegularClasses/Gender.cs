using System;
using Cats_Cafe_Accounting_System.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Cats_Cafe_Accounting_System.RegularClasses
{
    public class Gender
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public Gender() { }
    }
}
