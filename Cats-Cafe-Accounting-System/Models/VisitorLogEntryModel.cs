using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cats_Cafe_Accounting_System.Models
{
    public class VisitorLogEntryModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        [ForeignKey("Visitor")]
        public int VisitorId { get; set; }
        public VisitorModel Visitor { get; set; }
        [ForeignKey("Ticket")]
        public int TicketId { get; set; }
        public TicketModel Ticket { get; set; }
        public int TicketsCount { get; set; }
    }
}
