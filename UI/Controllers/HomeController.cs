using Model.DTO.DTO_Ad;
using Model.DTO.DTO_Client;
using Model.DTO_Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using UI.Security_;
using UI.Service;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        ServiceRepository service = new ServiceRepository();

        public string userName;
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult ListTypeProduct()
        {
            HttpResponseMessage responseUser = service.GetResponse("api/Home/GetAllItemType");

            responseUser.EnsureSuccessStatusCode();
            List<DTO_Item_Type> result = responseUser.Content.ReadAsAsync<List<DTO_Item_Type>>().Result;

            return PartialView(result);
        }

        [AuthorizeLoginEndUser]
        public ActionResult saveFeedbacks(FormCollection fc, DTO_Feedback fb)
        {

            try
            {

                fb.Name = fc["Name"];
                fb.Email = fc["Email"];
                fb.Details = fc["details"];
                fb.SDT = (fc["SDT"]);
                fb.Content = fc["content"];
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/Home/Create/", fb);
                response.EnsureSuccessStatusCode();
                ViewData["ErrorMessageFeedback"] = ("Gửi phản hồi thành công");
                return View("Index");
            }
            catch
            {
                return View("~/Views/Shared/Error_.cshtml");
            }

        }

        [AuthorizeLoginEndUser]
        public ActionResult saveFeedbacks2(FormCollection fc, DTO_Product_Item_Type fb)
        {

            try
            {

                var fullName = Request.Cookies["firstname"].Value;
                //fb.Comments = fc["content"];
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/Home/Create/", fb);
                response.EnsureSuccessStatusCode();
                ViewData["ErrorMessageFeedback"] = ("Gửi phản hồi thành công");
                return RedirectToAction("Details", "Product");
            }
            catch
            {
                return View("~/Views/Shared/Error_.cshtml");
            }
        }

        [AuthorizeLoginEndUser]
        public ActionResult saveFeedbacksYeuThich(FormCollection fc, DTO_Product_Item_Type product)
        {

            try
            {
              

                var dtoComment = new DtoProductComment()
                {
                    ProductId = product._id,
                    DateTimeComment = DateTime.Now,
                    FullName = Request.Cookies["firstname"].Value,
                    Content = fc["content"]
                };
            
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/Home/AddComment/", dtoComment);
                response.EnsureSuccessStatusCode();
                ViewData["ErrorMessage"] = ("Gửi bình luận thành công");
                return RedirectToAction("YeuThich", "Cart");
            }
            catch
            {
                return View("~/Views/Shared/Error_.cshtml");
            }


        }

        [AuthorizeLoginEndUser]
        public ActionResult saveFeedbacksLuaChon(FormCollection fc, DTO_Feedback fb)
        {

            try
            {
                fb.Name = fc["Name"];
                fb.Email = fc["Email"];
                fb.Details = fc["details"];
                fb.SDT = (fc["SDT"]);
                fb.Content = fc["content"];
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/Home/Create/", fb);
                response.EnsureSuccessStatusCode();
                ViewData["ErrorMessage"] = ("Gửi phản hồi thành công");

                return RedirectToAction("LuaChon", "Cart");
            }
            catch
            {
                return View("~/Views/Shared/Error_.cshtml");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
