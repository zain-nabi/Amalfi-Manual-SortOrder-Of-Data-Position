using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    [Table("Metrics")]
    public class Metrics
    {
        [Dapper.Contrib.Extensions.Key]
        public int metricID { get; set; }
        public int display_order { get; set; }
        public string description { get; set; }
        public int campaignID { get; set; }
    }
}
