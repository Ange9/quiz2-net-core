using Northwind.Store.Model;
using System.Collections.Generic;

namespace Northwind.Store.UI.Web.Intranet.Areas.Admin.ViewModels
{
    public class OrderIndexViewModel
    {
        public List<Order> Orders { get; set; }
        public double PageCount { get; set; }
        public int Page { get; set; }
    }
}
