using System;
using System.Collections.Generic;
using Runtime.Models;

namespace Runtime.Interfaces.Services
{
    public interface IReleaseGenerator
    {
        IList<ReleaseDataRow> Generate(DateTime startDate, DateTime endDate, int totalTasks, int dayOfWeek);
    }
}