using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cats_Cafe_Accounting_System.Utilities;
using System.Data;

namespace Cats_Cafe_Accounting_System.Models
{
    public class VisitLogEntryModel
    {
        [Key]
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public int VisitorId { get; set; }
        [ForeignKey("VisitorId")]
        public virtual VisitorModel Visitor { get; set; }
        public int TicketId { get; set; }
        [ForeignKey("TicketId")]
        public virtual TicketModel Ticket { get; set; }
        public int TicketsCount { get; set; }
        public VisitLogEntryModel()
        {
            Visitor = new VisitorModel();
            Ticket = new TicketModel();
        }
    }
}
