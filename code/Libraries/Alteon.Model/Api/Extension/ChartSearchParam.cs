using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alteon.Model.Api.Extension
{
    public class ChartSearchParam
    {
        public FixedTimeSpan timeSpan { get; set; }

        public DateTime? beginTime { get; set; }
        public DateTime? endTime { get; set; }

        public decimal? beginValue { get; set; }
        public decimal? endValue { get; set; }
        public int currentOption { get; set; }
    }
    public enum FixedTimeSpan
    {
        CurrentHour = 1,
        LastHour = 2,
        Today = 3
    }
}
