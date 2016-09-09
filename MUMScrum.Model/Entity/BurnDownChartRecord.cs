using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUMScrum.Model.Entity
{
    public class BurndownChartRecord
    {
        public DateTime Date { get; set; }
        public int ActualWork { get; set; }
        public int EstimateWork { get; set; }
        public int RemainingWork
        {
            get
            {
                return EstimateWork - ActualWork;
            }
        }
    }
}
