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
        public DateTime Date { get; set; }
        public DateTime TimeStart { get; set; }
        [ForeignKey("Visitor")]
        public int VisitorId { get; set; }
        public VisitorModel Visitor { get; set; }
        [ForeignKey("Ticket")]
        public int TicketId { get; set; }
        public TicketModel Ticket { get; set; }
        public int TicketsCount { get; set; }
        public VisitLogEntryModel(int id, DateTime date, DateTime timeStart, int visitorId, int ticketId, int ticketsCount)
        {
            Id = id;
            Date = date;
            TimeStart = timeStart;
            VisitorId = visitorId;
            TicketId = ticketId;
            TicketsCount = ticketsCount;
            Visitor = new VisitorModel(VisitorId);
            Ticket = new TicketModel(TicketId);
        }
        public VisitLogEntryModel(int id)
        {
            DataRow row = DBContext.GetById("visit_log_entries", id);
            Id = id;
            Date = DateTime.Parse(row["date"].ToString());
            TimeStart = DateTime.Parse(row["start_time"].ToString());
            VisitorId = Convert.ToInt32(row["visitor_id"]);
            TicketId = Convert.ToInt32(row["ticket_id"]);
            TicketsCount = Convert.ToInt32(row["tickets_count"]);
            Visitor = new VisitorModel(VisitorId);
            Ticket = new TicketModel(TicketId);
        }
    }
}
