using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {

        private readonly IConfiguration _config;

        public HomeController(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Create() {

            const string sql2 = @"SELECT m.metricID, m.description, m.campaignID, m.display_order
                                    FROM Metrics m
                                 WHERE m.campaignID = 22";

            await using var connection = DBConnection.GetOpenConnection(_config.GetConnectionString("CRM"));
            var data2 = connection.Query<Metrics>(sql2).ToList();

            var model = new MetricsVM();
            model.MetricsList = data2;

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Update(int metricID)
        {
            const string sql = @"SELECT COUNT(m.display_order), m.display_order
                                    FROM Metrics m
                                 WHERE m.campaignID = 22
                                 group by m.display_order";

            const string sql2 = @"SELECT m.metricID, m.description, m.campaignID, m.display_order
                                    FROM Metrics m
                                 WHERE m.MetricID = @metricID";

            await using var connection = DBConnection.GetOpenConnection(_config.GetConnectionString("CRM"));
            var data = connection.Query<Metrics>(sql).ToList();
            var data2 = connection.Query<Metrics>(sql2, new { metricID }).First();

            var model = new MetricsVM();

            model.DDL = new SelectList(data, "display_order", "display_order");
            model.Metrics = data2;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Update(MetricsVM model)
        {
            //get current display order
            const string sql2 = @"SELECT m.metricID, m.description, m.campaignID, m.display_order
                                    FROM Metrics m
                                 WHERE m.MetricID = @metricID";

            //save new display order to newIndex variable
            int newIndex = model.Metrics.display_order;


            // Getting old Index
            await using var connection = DBConnection.GetOpenConnection(_config.GetConnectionString("CRM"));
            var data2 = connection.Query<Metrics>(sql2, new { model.Metrics.metricID }).First();
            int oldIndex = data2.display_order;

            //Get range to update 
            const string sql3 = @"SELECT m.metricID, m.description, m.campaignID, m.display_order
                                    FROM Metrics m
                                    WHERE m.display_order between @oldIndex AND @newIndex";
            var data3 = connection.Query<Metrics>(sql3, new { newIndex = newIndex, oldIndex = oldIndex }).ToList();

            //check if range in bound
            if(data3.Count != 0)
            {
                if (newIndex < oldIndex)
                {
                    foreach (var item in data3)
                    {
                        if (item.display_order == oldIndex)
                        {
                            item.display_order = newIndex;
                        }
                        else
                        {
                            item.display_order += 1;
                        }
                        _ = await connection.UpdateAsync(item);
                    }
                }
                else
                {
                    foreach (var item in data3)
                    {
                        if (item.display_order == oldIndex)
                        {
                            item.display_order = newIndex;
                        }
                        else
                        {
                            item.display_order -= 1;
                        }
                        _ = await connection.UpdateAsync(item);
                    }
                }
                // Return success
            }
            else
            {
                const string sql4 = @"SELECT m.metricID, m.description, m.campaignID, m.display_order
                                    FROM Metrics m
                                    WHERE m.display_order between @newIndex AND @oldIndex";
                var data4 = connection.Query<Metrics>(sql4, new { newIndex = newIndex, oldIndex = oldIndex }).ToList();
                if (newIndex < oldIndex)
                {
                    foreach (var item in data4)
                    {
                        if (item.display_order == oldIndex)
                        {
                            item.display_order = newIndex;
                        }
                        else
                        {
                            item.display_order += 1;
                        }
                        _ = await connection.UpdateAsync(item);
                    }
                }
                else
                {
                    foreach (var item in data4)
                    {
                        if (item.display_order == oldIndex)
                        {
                            item.display_order = newIndex;
                        }
                        else
                        {
                            item.display_order -= 1;
                        }
                        _ = await connection.UpdateAsync(item);
                    }
                }
            }
            return View("Create");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
