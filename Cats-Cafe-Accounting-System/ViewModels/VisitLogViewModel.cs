using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class VisitLogViewModel : ObservableObject
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());
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
        
        public VisitLogViewModel()
        {
            foreach (var item in _dbContext.VisitLogEntries.Include(p => p.Visitor).Include(p => p.Ticket).ToList())
            {
                Items.Add(new Elem<VisitLogEntryModel>(item));
            }
        }
    }
}
