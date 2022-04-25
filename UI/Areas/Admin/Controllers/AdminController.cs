using Model.DTO.DTO_Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using UI.Security_;
using UI.Service;

namespace UI.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        ServiceRepository service = new ServiceRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MonthlySalesByDate(string _year, string _month)
        {
            //assign incoming values to the variables
            int year = 0, month = 0;
            //check if year is null
            if (string.IsNullOrWhiteSpace(_year) && _month != null)
            {
                year = DateTime.Now.Date.Year;
                month = Convert.ToInt32(_month.Trim());
            }
            else
            {
                year = Convert.ToInt32(_year.Trim());
                month = Convert.ToInt32(_month.Trim());
            }
            //calculate ttal number of days in a particular month for a that year 
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var days = Enumerable.Range(1, daysInMonth);
            var query = db.Sales.Where(x => x.Date.Year == year && x.Date.Month == month).Select(g => new
            {
                Day = g.Date.Day,
                Total = g.GrandTotal
            });
            var model = new SalesVM
            {
                Date = new DateTime(year, month, 1),
                Days = days.GroupJoin(query, d => d, q => q.Day, (d, q) => new DayTotalVM
                {
                    Day = d,
                    Total = q.Sum(x => x.Total)
                }).ToList()
            };
            return View(model);
        }

        [DeatAuthorize(Order = 2)]
        public ActionResult GetAllFeedbacks()
        {
            HttpResponseMessage responseMessage = service.GetResponse("api/Home/getallfeedback");
            responseMessage.EnsureSuccessStatusCode();
            List<DTO_Feedback> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_Feedback>>().Result;
            return View(dTO_Accounts);
        }
    }
}
