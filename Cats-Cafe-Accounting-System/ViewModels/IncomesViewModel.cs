using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class IncomesViewModel : ObservableObject
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());
        public SeriesCollection Items { get; set; }
        public string[] Labels { get; set; }
        public IncomesViewModel()
        {
            Items = new SeriesCollection();
            var groupedData = _dbContext.VisitLogEntries
                .Include(p => p.Visitor)
                .Include(p => p.Ticket)
                .AsEnumerable()
                .GroupBy(item => new { item.Date.Year, item.Date.Month })
                .Select(group => new
                {
                    group.Key.Year,
                    group.Key.Month,
                    TotalIncome = group.Select(item => item.Ticket.Price * item.TicketsCount)
                })
                .OrderBy(data => new DateOnly(data.Year, data.Month, 1))
                .ToList();

            Labels = groupedData.Select(data => $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(data.Month))} {data.Year}").ToArray();

            ChartValues<ObservableValue> values = new ChartValues<ObservableValue>();

            foreach (var data in groupedData)
            {
                values.Add(new ObservableValue(data.TotalIncome.Sum()));
            }
            Items.Add(new ColumnSeries
            {
                Title = "Доходы",
                Values = values
            });
        }
    }
}
