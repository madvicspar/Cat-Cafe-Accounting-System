using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class IncomesViewModel : ObservableObject
    {
        private readonly ApplicationDbContext _dbContext;
        public SeriesCollection Items { get; set; }
        public ObservableCollection<string> Labels { get; set; } = ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"];
        public int SelectedYear { get; set; }
        public List<int> Years { get; set; }

        public ICommand ChangeYearCommand { get; set; }

        public IncomesViewModel(ApplicationDbContext context)
        {
            _dbContext = context;
            Items = new SeriesCollection();
            Years = _dbContext.VisitLogEntries.Select(e => e.Date.Year).Distinct().ToList();
            SelectedYear = Years.Max();

            ChangeYearCommand = new RelayCommand<int>(ExecuteChangeYearCommand);
            RefreshData();
        }

        public void RefreshData()
        {
            var groupedData = _dbContext.VisitLogEntries
                .Include(p => p.Visitor)
                .Include(p => p.Ticket)
                .Where(item => item.Date.Year == SelectedYear)
                .GroupBy(item => new { item.Date.Month })
                .Select(group => new
                {
                    Month = group.Key.Month,
                    TotalIncome = group.Sum(item => item.Ticket.Price * item.TicketsCount)
                })
                .OrderBy(data => data.Month)
                .ToList();

            for (int month = 1; month <= 12; month++)
            {
                var dataForMonth = groupedData.FirstOrDefault(data => data.Month == month);
                if (dataForMonth == null)
                {
                    groupedData.Add(new { Month = month, TotalIncome = 0f }); // Добавляем месяц с нулевым доходом, если его не было в данных
                }
            }

            ChartValues<ObservableValue> values = new ChartValues<ObservableValue>();

            foreach (var data in groupedData.OrderBy(p => p.Month))
            {
                values.Add(new ObservableValue(data.TotalIncome));
            }

            Items.Clear();
            Items.Add(new ColumnSeries
            {
                Title = "Доходы",
                Values = values
            });
        }

        private void ExecuteChangeYearCommand(int value)
        {
            SelectedYear = value;
            RefreshData();
        }
    }
}
