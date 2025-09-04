using System;
using System.Collections.Generic;
using Runtime.Interfaces.Services;
using Runtime.Models;

namespace Runtime.Services
{
    internal sealed class ReleaseGenerator : IReleaseGenerator
    {
        public IList<ReleaseDataRow> Generate(DateTime startDate, DateTime endDate, int totalTasks, int dayOfWeek)
        {
            var rows = new List<ReleaseDataRow>();
            var checkDates = new List<DateTime>();

            var current = startDate;
            var checkDay = ToDayOfWeek(dayOfWeek);

            // find first checkup date
            while (current.DayOfWeek != checkDay)
                current = current.AddDays(1);

            // add all check dates before end
            while (current < endDate)
            {
                checkDates.Add(current);
                current = current.AddDays(7);
            }

            if (checkDates.Count == 0 || checkDates[^1] != endDate)
                checkDates.Add(endDate);

            int n = checkDates.Count;
            const double h = 100.0;
            for (int i = 0; i < n; i++)
            {
                double percent = (i == n - 1) ? h : Math.Round((i + 1) * h / n);
                int tasksCount = (int) Math.Round(totalTasks * percent / h);

                var row = new ReleaseDataRow()
                {
                    Date = checkDates[i].ToString("yyyy-MM-dd"),
                    Plan = tasksCount,
                    Fact = 0
                };

                rows.Add(row);
            }

            return rows;
        }


        private static DayOfWeek ToDayOfWeek(int value)
        {
            if (++value == 7)
                return DayOfWeek.Sunday;

            return (DayOfWeek) value;
        }
    }
}