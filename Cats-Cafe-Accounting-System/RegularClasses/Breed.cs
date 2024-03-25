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

        public Breed() { }
    }
}
