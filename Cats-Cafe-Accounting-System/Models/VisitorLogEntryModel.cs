using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cats_Cafe_Accounting_System.Utilities;
using System.Data;

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
        public VisitorLogEntryModel(int id, DateTime date, DateTime timeStart, DateTime timeEnd, int visitorId, int ticketId, int ticketsCount)
        {
            Id = id;
            Date = date;
            TimeStart = timeStart;
            TimeEnd = timeEnd;
            VisitorId = visitorId;
            TicketId = ticketId;
            TicketsCount = ticketsCount;
            Visitor = new VisitorModel(VisitorId);
            Ticket = new TicketModel(TicketId);
        }
        public VisitorLogEntryModel(int id)
        {
            DataRow row = DBContext.GetById("pet", id);
            Id = id;
            Date = DateTime.Parse(row["date"].ToString());
            TimeStart = DateTime.Parse(row["start_time"].ToString());
            TimeEnd = DateTime.Parse(row["end_time"].ToString());
            VisitorId = Convert.ToInt32(row["visitor_id"]);
            TicketId = Convert.ToInt32(row["ticket_id"]);
            TicketsCount = Convert.ToInt32(row["tickets_count"]);
            Visitor = new VisitorModel(VisitorId);
            Ticket = new TicketModel(TicketId);
        }
    }
}
