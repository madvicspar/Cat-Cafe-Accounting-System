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
        public VisitLogEntryModel(int id, DateOnly date, TimeOnly timeStart, int visitorId, int ticketId, int ticketsCount)
        {
            Id = id;
            Date = date;
            StartTime = timeStart;
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
            Date = DateOnly.Parse(row["date"].ToString());
            StartTime = TimeOnly.Parse(row["start_time"].ToString());
            VisitorId = Convert.ToInt32(row["visitor_id"]);
            TicketId = Convert.ToInt32(row["ticket_id"]);
            TicketsCount = Convert.ToInt32(row["tickets_count"]);
            Visitor = new VisitorModel(VisitorId);
            Ticket = new TicketModel(TicketId);
        }
        public VisitLogEntryModel()
        {
            Visitor = new VisitorModel();
            Ticket = new TicketModel();
        }
    }
}
