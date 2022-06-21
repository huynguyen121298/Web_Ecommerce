using Model.DTO.DTO_Ad;
using Model.DTO.DTO_Client;
using Model.DTO_Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using UI.Areas.Admin.Common;
using UI.Security_;
using UI.Service;

namespace UI.Areas.Admin.Controllers
{
    public class Products_AddController : Controller
    {
        private ServiceRepository service = new ServiceRepository();
        // GET: Admin/Products_Add

        [AuthorizeLoginAdmin]
        public ActionResult Index(DTO_Product dTO_Product)
        {
            try
            {
                DTO_Account dTO_Account = new DTO_Account();
                dTO_Account = (DTO_Account)Session[CommonConstants.ACCOUNT_SESSION];
                HttpResponseMessage responseMessage = service.GetResponse("api/Products_Ad/GetAllProduct_Discount/" + dTO_Account._id);
                responseMessage.EnsureSuccessStatusCode();
                List<DTO_Dis_Product> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
                return View(dTO_Accounts);
            }
            catch
            {
                return View(dTO_Product);
            }
        }

        [AuthorizeLoginAdmin]
        public ActionResult Product_Discount(DTO_Dis_Product dTO_Product)
        {
            try
            {
                DTO_Account dTO_Account = new DTO_Account();
                dTO_Account = (DTO_Account)Session[CommonConstants.ACCOUNT_SESSION];
                HttpResponseMessage responseMessage = service.GetResponse("api/Products_Ad/GetAllProduct_Discount/" + dTO_Account._id);
                responseMessage.EnsureSuccessStatusCode();
                List<DTO_Dis_Product> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
                return View(dTO_Accounts);
            }
            catch
            {
                return View(dTO_Product);
            }
        }

        [AuthorizeLoginAdmin]
        public ActionResult GetAllProductByType()
        {
            DTO_Account dTO_Account = new DTO_Account();
            dTO_Account = (DTO_Account)Session[CommonConstants.ACCOUNT_SESSION];
            HttpResponseMessage responseMessage = service.GetResponse("api/Products_Ad/GetAllProductByType/" + dTO_Account._id);
            responseMessage.EnsureSuccessStatusCode();
            List<List<DTO_Dis_Product>> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<List<DTO_Dis_Product>>>().Result;
            return View(dTO_Accounts);
        }

        [AuthorizeLoginAdmin]
        public ActionResult Index2(string id)
        {
            var dTO_Account = (DTO_Account)Session[CommonConstants.ACCOUNT_SESSION];
            HttpResponseMessage responseMessage = service.GetResponse("api/Products_Ad/GetAllProductByIdItem/" + id +"/" +dTO_Account._id);
            responseMessage.EnsureSuccessStatusCode();
            List<DTO_Dis_Product> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
            return View(dTO_Accounts);
        }

        [AuthorizeLoginAdmin]
        public ActionResult Details(string Id)
        {
            HttpResponseMessage responseMessage = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
            responseMessage.EnsureSuccessStatusCode();
            DTO_Product_Item_Type dTO_Accounts = responseMessage.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;
            return View(dTO_Accounts);
        }

        [AuthorizeLoginAdmin]
        public ActionResult Create()
        {
            DTO_Product_Item_Type pro = new DTO_Product_Item_Type();

            return View(pro);
            //return View();
        }

        public string ProcessUpload(HttpPostedFileBase file)
        {
            file.SaveAs(Server.MapPath("~/images_product/" + file.FileName));

            return "/images_product/" + file.FileName;
        }

        [AuthorizeLoginAdmin]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(FormCollection collection, DTO_Product_Item_Type dTO_Product_Item_Type, HttpPostedFileBase ImageUpload,
            HttpPostedFileBase ImageUpload2, HttpPostedFileBase ImageUpload3)
        {
            var dTO_Account = (DTO_Account)Session[CommonConstants.ACCOUNT_SESSION];
            dTO_Product_Item_Type.AccountId = dTO_Account._id;
            dTO_Product_Item_Type.Type_Product = Request.Form["typeProduct"];

            var items = new List<DTO_Item>();
            var item = new DTO_Item();
            item.Quantity = dTO_Product_Item_Type.QuantityBuy;

            if (Request.Form["selectColor"] !=null)
            {               
                item.Color = Request.Form["selectColor"];
            }

            if (dTO_Product_Item_Type.Type_Product == "Thời trang nam" || dTO_Product_Item_Type.Type_Product == "Thời trang nữ")
            {
                var sizes = Request.Form["sizeProduct"];
                if (sizes!=null)
                {
                   
                    item.Size = sizes;
                }
                else
                    ViewData["ErrorMessage"] = "Bạn chưa chọn size cho sản phẩm";
            }
            items.Add(item);
            dTO_Product_Item_Type.Items = items;

            try
            {
                if (ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(ImageUpload.FileName);
                    string extension = Path.GetExtension(ImageUpload.FileName);
                    fileName = fileName + extension;
                    dTO_Product_Item_Type.Photo = "~/images_product/" + fileName;
                    ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/images_product/"), fileName));
                }

                if (ImageUpload2 != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(ImageUpload2.FileName);
                    string extension = Path.GetExtension(ImageUpload2.FileName);
                    fileName = fileName + extension;
                    dTO_Product_Item_Type.Photo2 = "~/images_product/" + fileName;
                    ImageUpload2.SaveAs(Path.Combine(Server.MapPath("~/images_product/"), fileName));
                }

                if (ImageUpload3 != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(ImageUpload3.FileName);
                    string extension = Path.GetExtension(ImageUpload3.FileName);
                    fileName = fileName + extension;
                    dTO_Product_Item_Type.Photo3 = "~/images_product/" + fileName;
                    ImageUpload3.SaveAs(Path.Combine(Server.MapPath("~/images_product/"), fileName));
                }

                if (ImageUpload == null && ImageUpload2 == null && ImageUpload3 == null)
                {
                    ViewData["ErrorMessage"] = "Bạn chưa chọn hình ảnh cho sản phẩm";
                    return View(dTO_Product_Item_Type);
                }

                HttpResponseMessage responseUser = service.PostResponse("api/Products_Ad/CreateProduct/", dTO_Product_Item_Type);
                responseUser.EnsureSuccessStatusCode();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewData["ErrorMessage2"] = "Hình ảnh sản phẩm đã tổn tại,vui lòng chọn hình ảnh khác";
                return View(dTO_Product_Item_Type);
            }
        }

        [AuthorizeLoginAdmin]
        public ActionResult Create_Discount(string id)
        {
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage responseMessage = service.GetResponse("api/Products_Ad/GetProduct_DiscountById/" + id);
            responseMessage.EnsureSuccessStatusCode();
            DTO_Dis_Product dTO_Dis_Product = responseMessage.Content.ReadAsAsync<DTO_Dis_Product>().Result;

            return View(dTO_Dis_Product);
        }

        [AuthorizeLoginAdmin]
        [HttpPost]
        public ActionResult Create_Discount_Product(DTO_Dis_Product tO_Dis_Product)
        {
            var stt = Request.Form["stt"];
            var start = Request.Form["start"];
            var end = Request.Form["end"];
            if (start == "")
            {
                start = null;
            }
            if (end == "")
            {
                end = null;
            }

            tO_Dis_Product.Content = stt + "%";
            tO_Dis_Product.Start = Convert.ToDateTime(start).AddHours(-7);
            tO_Dis_Product.End = Convert.ToDateTime(end).AddHours(-7);
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage responseMessage = service.PostResponse("api/Products_Ad/CreateProduct_Discount/", tO_Dis_Product);
            responseMessage.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }

        [AuthorizeLoginAdmin]
        public ActionResult Edit(string Id)
        {
            ServiceRepository service = new ServiceRepository();
            HttpResponseMessage responseMessage = service.GetResponse("api/Products_Ad/GetProductItemById_admin/" + Id);
            responseMessage.EnsureSuccessStatusCode();

            DTO_Product_Item_Type dtoAccounts = responseMessage.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;
            HttpResponseMessage responseMessage2 = service.GetResponse("api/ItemType/GetById/" + dtoAccounts.IdItemType);

            var itemType = responseMessage2.Content.ReadAsAsync<DTO_Item_Type>().Result;
            Session.Add("typeProduct", itemType.Type_Product) ;
            return View(dtoAccounts);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection collection, DTO_Product_Item_Type dTO_Product_Item_Type, HttpPostedFileBase ImageUpload,
            HttpPostedFileBase ImageUpload2, HttpPostedFileBase ImageUpload3)
        {
            dTO_Product_Item_Type.Type_Product = Request.Form["typeProduct"];
          
            try
            {
                if (ModelState.IsValid)
                {
                    if (ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(ImageUpload.FileName);
                        string extension = Path.GetExtension(ImageUpload.FileName);
                        fileName = fileName + extension;
                        dTO_Product_Item_Type.Photo = "~/images_product/" + fileName;
                        ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/images_product/"), fileName));
                    }

                    if (ImageUpload2 != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(ImageUpload2.FileName);
                        string extension = Path.GetExtension(ImageUpload2.FileName);
                        fileName = fileName + extension;
                        dTO_Product_Item_Type.Photo2 = "~/images_product/" + fileName;
                        ImageUpload2.SaveAs(Path.Combine(Server.MapPath("~/images_product/"), fileName));
                    }

                    if (ImageUpload3 != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(ImageUpload3.FileName);
                        string extension = Path.GetExtension(ImageUpload3.FileName);
                        fileName = fileName + extension;
                        dTO_Product_Item_Type.Photo3 = "~/images_product/" + fileName;
                        ImageUpload3.SaveAs(Path.Combine(Server.MapPath("~/images_product/"), fileName));
                    }
                    if ((ImageUpload == null && ImageUpload2 == null && ImageUpload3 == null)
                        && dTO_Product_Item_Type.Photo == null && dTO_Product_Item_Type.Photo2 == null && dTO_Product_Item_Type.Photo3 == null)
                    {
                        ViewData["ErrorMessage"] = "Bạn chưa chọn hình ảnh cho sản phẩm";
                        return View(dTO_Product_Item_Type);
                    }
                    HttpResponseMessage responseUser = service.PostResponse("api/Products_Ad/UpdateProduct/", dTO_Product_Item_Type);
                    responseUser.EnsureSuccessStatusCode();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [AuthorizeLoginAdmin]
        public ActionResult Delete(string id)
        {
            try
            {
                HttpResponseMessage response = service.DeleteResponse("api/Products_Ad/DeleteProduct/" + id);
                response.EnsureSuccessStatusCode();

                return Json(new { mes = true });
            }
            catch
            {
                return Json(new { mes = false });
            }
        }
    }
}