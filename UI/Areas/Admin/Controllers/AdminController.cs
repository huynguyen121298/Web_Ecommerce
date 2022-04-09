using Model.DTO.DTO_Client;
using System.Collections.Generic;
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
