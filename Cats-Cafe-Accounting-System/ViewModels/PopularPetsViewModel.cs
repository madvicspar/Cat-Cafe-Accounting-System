using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class PopularPetsViewModel : ObservableObject
    {
        private readonly ApplicationDbContext _dbContext;
        private ObservableCollection<Elem<PopularPetsModel>> items = new ObservableCollection<Elem<PopularPetsModel>>();
        public ObservableCollection<Elem<PopularPetsModel>> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private ObservableCollection<Elem<PopularPetsModel>> filterItems = new ObservableCollection<Elem<PopularPetsModel>>();
        public ObservableCollection<Elem<PopularPetsModel>> FilterItems
        {
            get { return filterItems; }
            set
            {
                filterItems = value;
                OnPropertyChanged(nameof(FilterItems));
            }
        }
        public PopularPetsViewModel(ApplicationDbContext contex)
        {
            _dbContext = contex;
            int i = 1;
            foreach (var item in _dbContext.VisitLogEntries.Include(t => t.Ticket).Where(t => t.Ticket.PetId != 0).GroupBy(g => g.TicketId).ToList().OrderBy(x => x.Count()))
            {
                List<VisitLogEntryModel> visits = [.. item];
                int score = 0;
                foreach (var visit in item)
                    score += visit.TicketsCount;
                PopularPetsModel model = new PopularPetsModel() { Pet = item.FirstOrDefault().Ticket.Pet, PetTransferLogEntryModels = visits, Place = i, Score = score };
                Items.Add(new Elem<PopularPetsModel>(model));
                FilterItems.Add(new Elem<PopularPetsModel>(model));
                i++;
            }
        }
    }
}
