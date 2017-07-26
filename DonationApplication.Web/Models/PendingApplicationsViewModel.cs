using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DonationApplication.Data;

namespace DonationApplication.Web.Models
{
    public class PendingApplicationsViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Application> Applications { get; set; }
    }
}