using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DonationApplication.Data;

namespace DonationApplication.Web.Models
{
    public class ApplicationViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}