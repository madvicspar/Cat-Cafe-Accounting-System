using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cats_Cafe_Accounting_System.Utilities;
using System.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Cats_Cafe_Accounting_System.Models
{
    public class VisitLogEntryModel : ObservableObject, ICloneable, IEquatable<VisitLogEntryModel>
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
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

        public static VisitLogEntryModel Update(VisitLogEntryModel oldVisitorLogEntryModel, VisitLogEntryModel newVisitorLogEntryModel)
        {
            oldVisitorLogEntryModel.Date = newVisitorLogEntryModel.Date;
            oldVisitorLogEntryModel.StartTime = newVisitorLogEntryModel.StartTime;
            oldVisitorLogEntryModel.TicketId = newVisitorLogEntryModel.TicketId;
            oldVisitorLogEntryModel.Ticket = newVisitorLogEntryModel.Ticket;
            oldVisitorLogEntryModel.VisitorId = newVisitorLogEntryModel.VisitorId;
            oldVisitorLogEntryModel.Visitor = newVisitorLogEntryModel.Visitor;
            oldVisitorLogEntryModel.TicketsCount = newVisitorLogEntryModel.TicketsCount;
            return oldVisitorLogEntryModel;
        }

        public object Clone()
        {
            return new VisitLogEntryModel
            {
                Id = Id,
                Date = Date,
                StartTime = StartTime,
                TicketId = TicketId,
                Ticket = Ticket,
                VisitorId = VisitorId,
                Visitor = Visitor,
                TicketsCount = TicketsCount
            };
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(VisitLogEntryModel? other)
        {
            return other?.Date == Date
                && other?.StartTime == StartTime
                && other?.TicketId == TicketId
                && other?.Ticket == Ticket
                && other?.VisitorId == VisitorId
                && other?.Visitor == Visitor
                && other?.TicketsCount == TicketsCount;
        }
    }
}
