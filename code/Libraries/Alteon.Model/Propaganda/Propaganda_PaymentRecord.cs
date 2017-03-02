using Alteon.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alteon.Model.Propaganda
{
    public partial class Propaganda_PaymentRecord : EntityBase
    {
        public int Id { get; set; }

        public Nullable<int> User_Id { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<DateTime> PayTime { get; set; }
        public Nullable<DateTime> ValidityPeriodStart { get; set; }
        public Nullable<DateTime> ValidityPeriodEnd { get; set; }
    }
}
