using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DonationApplication.Data;

namespace DonationApplication.Web.Models
{
    public class HistoryViewModel
    {
        public User User { get; set; }
        public IEnumerable<Application> Applications { get; set; }
    }
}