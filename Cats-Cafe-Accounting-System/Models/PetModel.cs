using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using System.Data;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Cats_Cafe_Accounting_System.Models
{
    public class PetModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("Gender")]
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        [ForeignKey("Status")]
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public string BreedId { get; set; }
        public Breed Breed { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CheckInDate { get; set; }
        public string PassNumber { get; set; }
        public PetModel(int id, string name, int genderId, int statusId, string breedId, DateTime birthday, DateTime checkInDate, string passNumber)
        {
            Id = id;
            Name = name;
            GenderId = genderId;
            StatusId = statusId;
            BreedId = breedId;
            Birthday = birthday;
            CheckInDate = checkInDate;
            PassNumber = passNumber;
            Breed = new Breed(BreedId);
            Gender = new Gender(GenderId);
            Status = new Status(StatusId);
        }
        public PetModel(int id)
        {
            if (id != 0)
            {
                DataRow row = DBContext.GetById("pets", id);
                Id = id;
                Name = row["name"].ToString();
                GenderId = Convert.ToInt32(row["gender_id"]);
                StatusId = Convert.ToInt32(row["status_id"]);
                BreedId = row["breed_id"].ToString();
                Birthday = DateTime.Parse(row["birthday"].ToString());
                CheckInDate = DateTime.Parse(row["check_in_date"].ToString());
                PassNumber = row["pass_number"].ToString();
                Breed = new Breed(BreedId);
                Gender = new Gender(GenderId);
                Status = new Status(StatusId);
            }
        }
        public PetModel()
        {
            
        }
    }
}
