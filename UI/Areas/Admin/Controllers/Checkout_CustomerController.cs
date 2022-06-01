using Model.DTO.DTO_Ad;
using Model.DTO_Model;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Mvc;
using UI.Areas.Admin.Common;
using UI.Security_;
using UI.Service;

namespace UI.Areas.Admin.Controllers
{
    public class Checkout_CustomerController : Controller
    {
        private ServiceRepository service = new ServiceRepository();

        [AuthorizeLoginAdmin]
        public ActionResult Index(string seachby)
        {
            var timkiemtim = Request.Form["timkiemtim"];

            if (seachby == "id")
            {
                HttpResponseMessage responseMessage1 = service.GetResponse("api/Checkout_Customer/GetListcustomerById/" + timkiemtim);
                responseMessage1.EnsureSuccessStatusCode();
                List<DTOCheckoutCustomerOrder> dtocustomer = responseMessage1.Content.ReadAsAsync<List<DTOCheckoutCustomerOrder>>().Result;
                return View(dtocustomer);
            }
            else
            {
                var dTO_Account = (DTO_Account)Session[CommonConstants.ACCOUNT_SESSION];
                HttpResponseMessage responseMessage = service.GetResponse("api/Checkout_Customer/getallcustomer/" + dTO_Account._id);
                responseMessage.EnsureSuccessStatusCode();
                List<DTOCheckoutCustomerOrder> DTO_Checkout_Customers = responseMessage.Content.ReadAsAsync<List<DTOCheckoutCustomerOrder>>().Result;
                return View(DTO_Checkout_Customers);
            }
        }

        [AuthorizeLoginAdmin]
        public ActionResult Edit(string id)
        {
            HttpResponseMessage responseMessage = service.GetResponse("api/Checkout_Customer/GetcustomerById/" + id);
            responseMessage.EnsureSuccessStatusCode();
            DTOCheckoutCustomerOrder dtocustomer = responseMessage.Content.ReadAsAsync<DTOCheckoutCustomerOrder>().Result;

            return View(dtocustomer);
        }

        [AuthorizeLoginAdmin]
        public ActionResult Details(string id)
        {
            HttpResponseMessage responseMessage = service.GetResponse("api/Checkout_Customer/GetCustomerById/" + id);
            responseMessage.EnsureSuccessStatusCode();
            DTOCheckoutCustomerOrder dtocustomer = responseMessage.Content.ReadAsAsync<DTOCheckoutCustomerOrder>().Result;

            return View(dtocustomer);
        }

        [AuthorizeLoginAdmin]
        [HttpPost]
        public ActionResult Edit(DTOCheckoutCustomerOrder dtocustomer)
        {
            dtocustomer.TrangThai = Request.Form["stt"];
            HttpResponseMessage response = service.PostResponse("api/Checkout_Customer/Update/", dtocustomer);
            response.EnsureSuccessStatusCode();
            if(dtocustomer.TrangThai == "Đã lên đơn")
            {
                var fullName = dtocustomer.FirstName + "" + dtocustomer.LastName;

                var subject = "Đơn hàng được xác nhận";
                var body = "Xin chào " + fullName + ", <br/> Đơn hàng " + dtocustomer._id + " đã được xác nhận vào lúc " + dtocustomer.NgayTao;

                var sendMail = SendEmail(dtocustomer.Email, body, subject);
                if (sendMail == false)
                {
                    ViewBag.Mess = "Có lỗi gửi mail ngoài ý muốn, vui lòng kiểm tra lại";
                    return View();
                }
            }

            return RedirectToAction("Index");
        }

        [AuthorizeLoginAdmin]
        public ActionResult Delete(string id, string reason)
        {
            try
            {
                //var reason = Request.Form["reason"];
                if(reason != "")
                {
                   
                    HttpResponseMessage response = service.DeleteResponse("api/Checkout_Customer/Deletecustomer/" + id);
                    response.EnsureSuccessStatusCode();
                    bool checkSuccess = response.Content.ReadAsAsync<bool>().Result;
                    if(checkSuccess == false)
                        return Json(new { mes = false });

                    HttpResponseMessage responseMessage = service.GetResponse("api/Checkout_Customer/GetCustomerById/" + id);
                    responseMessage.EnsureSuccessStatusCode();
                    DTOCheckoutCustomerOrder dtocustomer = responseMessage.Content.ReadAsAsync<DTOCheckoutCustomerOrder>().Result;

                    var fullName = dtocustomer.FirstName + "" + dtocustomer.LastName;

                    var subject = "Hủy đơn hàng";
                    var body = "Xin chào " + fullName + ", <br/> Đơn hàng " + dtocustomer._id + " được đặt ngày " + dtocustomer.NgayTao + " đã bị từ chối vì lý do "
                        + reason;

                    var sendMail = SendEmail(dtocustomer.Email, body, subject);
                    if (sendMail == false)
                    {
                        ViewBag.Mess = "Có lỗi ngoài ý muốn, vui lòng kiểm tra lại";
                        return Json(new { mes = false });
                    }

                    return Json(new { mes = true });
                }
                ViewBag.Mess = "Lý do không được để trống";
                return Json(new { mes = false });
            }
            catch
            {
                ViewBag.Mess = "Có lỗi ngoài ý muốn, vui lòng kiểm tra lại";
                return Json(new { mes = false });
            }
        }

        private bool SendEmail(string emailAddress, string body, string subject)
        {
            try
            {
                using (MailMessage mm = new MailMessage("huytuannguyen.301198@gmail.com", emailAddress))
                {
                    mm.Subject = subject;
                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("huytuannguyen.301198@gmail.com", "Huyhuy123");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }
                return true;
            }
            catch
            {
                return false;
            }
           
        }
    }
}