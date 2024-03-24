using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class VisitLogViewModel : ObservableObject
    {
        private readonly ApplicationDbContext _dbContext;
        private ObservableCollection<Elem<VisitLogEntryModel>> items = new ObservableCollection<Elem<VisitLogEntryModel>>();
        public ObservableCollection<Elem<VisitLogEntryModel>> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }  
        
        public VisitLogViewModel(ApplicationDbContext context)
        {
            _dbContext = context;
            //List<VisitLogEntryModel>? list = _dbContext.VisitLogEntries.Include(p => p.Visitor).Include(p => p.Ticket).Where(entry => entry.Ticket.Pet != null).ToList();
            //List<VisitLogEntryModel>? list2 = _dbContext.VisitLogEntries.Include(p => p.Visitor).Include(p => p.Ticket).Where(entry => entry.Ticket.Pet == null).ToList();
            //list.AddRange(list2);
            //list.OrderBy(item => item.Id);
            //for (int i = 0; i < list.Count; i++)
            //{
            //    VisitLogEntryModel? item = list[i];
            //    Items.Add(new Elem<VisitLogEntryModel>(item));
            //}
        }
    }
}
