using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class MetricsVM
    {

        public int SelectedItem { get; set; }
        public IEnumerable<SelectListItem> DDL { get; set; }
        public Metrics Metrics { get; set; }
        public List<Metrics> MetricsList { get; set; }
    }
}
