using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DAL.Common
{
    public class AppSettings
    {
        public int NotificationRunEvery { get; set; }
        public int NumberOfDaysToNotify { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string DefaultPassword { get; set; }
        public string AdminDefaultPassword { get; set; }
        public string AdminDefaultEmail { get; set; }
    }
}
