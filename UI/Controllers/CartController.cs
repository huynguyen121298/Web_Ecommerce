using Model.DTO.DTO_Ad;
using Model.DTO_Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web.Mvc;
using UI.Service;

namespace UI.Controllers
{
    public class CartController : Controller
    {
        ProductController productController = new ProductController();
        ServiceRepository service = new ServiceRepository();
        public ActionResult Index()
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];

            if (cart == null)

                return View("Thankyou1");
            else
                return View();
        }
        public ActionResult Checkout()
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
            if (cart == null)
            {
                return View("Thankyou1");
            }
            return View();
        }
        public ActionResult LuaChon()
        {
            return View();
        }
        public ActionResult Buy_()
        {
            bool flag = true;
            List<string> messages = new List<string>();
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
            foreach (var item in cart)
            {
                HttpResponseMessage response = service.GetResponse("api/Product/GetSoLuong/" + item._id);

                response.EnsureSuccessStatusCode();
                int quantity = response.Content.ReadAsAsync<int>().Result;
                int quantityAfterBuy = quantity - (int)item.Quantity;
                if (quantityAfterBuy < 0)
                {
                    flag = false;
                    string message = (item.Name + " đã vượt quá số lượng đang có");
                    messages.Add(message);
                }
            }
            ViewData["messages"] = messages;
            if (flag)
                return RedirectToAction("Index", "Cart");
            return View("~/Views/Product/Details.cshtml");
        }
        public ActionResult saveOrder1(FormCollection fc, DTOCheckoutCustomerOrder check)
        {
            try
            {
                var checkZip = check.Zipcode = fc["zip"];

               
                    var price = Request.Form["gia1"];
                    var price1 = Request.Form["discount1"];
                    List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
                    check.NgayTao = DateTime.Now;
                    check.FirstName = fc["FirstName"];
                    check.LastName = fc["LastName"];
                    check.Email = fc["Email"];

                    check.DiaChi = fc["diaChi"];
                    check.TongTien = Convert.ToInt32(price);

                    check.City = fc["city"];
                    check.SDT = Int32.Parse(fc["sdt"]);
                    check.TrangThai = "Đang chờ";
                    if (checkZip != "")
                    {
                        check.Zipcode = fc["zip"];
                        check.GiamGia = price1 +"%";
                    }

                    check.ProductOrder = new List<DTO_Checkout_Order>();

                    foreach (DTO_Product_Item_Type item in cart)
                    {
                        DTO_Checkout_Order dTO_Checkout_Order = new DTO_Checkout_Order();
                        var total = (item.Quantity * item.Price);
                        dTO_Checkout_Order.Id_SanPham = item._id;
                        dTO_Checkout_Order.TenSP = item.Name;
                        dTO_Checkout_Order.SoLuong = (int)item.Quantity;
                        dTO_Checkout_Order.Gia = total;
                        check.ProductOrder.Add(dTO_Checkout_Order);
                    }
                    HttpResponseMessage responseUser1 = service.PostResponse("api/Cart/InsertBill/", check);

                   

                if (responseUser1.IsSuccessStatusCode)
                {
                    Session.Clear();
                    return View("Thankyou");
                }
                return RedirectToAction("Error", "Home");


            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult saveOrder2(string priceCode)
        {
            try
            {
                HttpResponseMessage responseUser = service.GetResponse("api/Cart/GetGiamGia/" + priceCode);
                responseUser.EnsureSuccessStatusCode();
                Double giamgia = responseUser.Content.ReadAsAsync<Double>().Result;
                string messsage = ("Mã code " + priceCode.ToString() + " hợp lệ");
                ViewBag.Message = messsage;
                return Json(new { checkout = giamgia });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        public static string RenderRazorViewToString(ControllerContext controllerContext, string viewName, object model)
        {
            controllerContext.Controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var ViewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var ViewContext = new ViewContext(controllerContext, ViewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);
                ViewResult.View.Render(ViewContext, sw);
                ViewResult.ViewEngine.ReleaseView(controllerContext, ViewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult YeuThich()
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart_"];
            if (cart == null)
            {
                return View("Thankyou1");
            }


            return View();

        }
        public ActionResult HetHang()
        {
            return View();
        }
        public ActionResult Details_(string Id)
        {
            if (Session["cart_"] == null)
            {
                List<DTO_Product_Item_Type> li = new List<DTO_Product_Item_Type>();

                HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
                responseUser.EnsureSuccessStatusCode();
                DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;

                li.Add(new DTO_Product_Item_Type()
                {
                    _id = proItem._id,
                    Name = proItem.Name,
                    Price = proItem.Price,
                    Details = proItem.Details,
                    Photo = proItem.Photo,
                    Photo2 = proItem.Photo2,
                    Photo3 = proItem.Photo3,
                    Id_Item = proItem.Id_Item,
                    Quantity = 1
                });
                Session["cart_"] = li;
                return Json(new { buy = li });
            }
            else
            {
                List<DTO_Product_Item_Type> li = (List<DTO_Product_Item_Type>)Session["cart_"];
                HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
                responseUser.EnsureSuccessStatusCode();
                DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;

                int index = isExist_(Id);
                if (index != -1)
                {
                    li[index].Quantity++;
                }
                else
                {
                    li.Add(new DTO_Product_Item_Type()
                    {
                        _id = proItem._id,
                        Name = proItem.Name,
                        Price = proItem.Price,
                        Details = proItem.Details,
                        Photo = proItem.Photo,
                        Photo2 = proItem.Photo2,
                        Photo3 = proItem.Photo3,
                        Id_Item = proItem.Id_Item,
                        Quantity = 1
                    });
                    return Json(new { buy = li });
                }


                Session["cart_"] = li;
                ViewBag.IdPorduct = Id;
                return Json(new { buy = li });
            }
        }
        private int isExist_(string Id)
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart_"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i]._id.Equals(Id))
                    return i;
            return -1;
        }
        public ActionResult Remove_(string Id)
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart_"];
            int index = isExist_(Id);
            cart.RemoveAt(index);
            Session["cart_"] = cart;
            return RedirectToAction("YeuThich");
        }

        private int isExist2(string Id)
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i]._id.Equals(Id))
                    return i;
            return -1;
        }
        public ActionResult Remove2(string Id)
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
            int index = isExist2(Id);
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return RedirectToAction("Index");
        }
        public ActionResult Thankyou()
        {

            return View();
        }
        public ActionResult Thankyou1()
        {

            return View();
        }

        public ActionResult Details_Buy(string Id)
        {
            

            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
            if (cart != null)
            {
                foreach (var item in cart)
                {
                    if (item._id == Id)
                    {
                        HttpResponseMessage response2 = service.GetResponse("api/Product/GetSoLuong/" + item._id);
                        response2.EnsureSuccessStatusCode();
                        int quantity = response2.Content.ReadAsAsync<int>().Result;
                        int quantityAfterBuy = quantity - (int)item.Quantity;
                        if (quantityAfterBuy <= 0)
                        {
                            string message = (item.Name + " đã vượt quá số lượng đang có");
                            ViewData["MessQuantity"] = message;

                            return View("LuaChon");
                        }
                    }

                }

            }
            int checkBuy = CheckBuy_(Id);
            if (checkBuy == 0)
            {
                ViewData["MessQuantity"] = ("Sản phẩm đã hết");

                return View("LuaChon");
            }
            return RedirectToAction("Details", "Product");

        }

        public ActionResult BuyLove(string Id)
        {
            

            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
            if (cart != null)
            {
                foreach (var item in cart)
                {
                    if (item._id == Id)
                    {
                        HttpResponseMessage response2 = service.GetResponse("api/Product/GetSoLuong/" + item._id);
                        response2.EnsureSuccessStatusCode();
                        int quantity = response2.Content.ReadAsAsync<int>().Result;
                        int quantityAfterBuy = quantity - (int)item.Quantity;
                        if (quantityAfterBuy <= 0)
                        {
                            string message = (item.Name + " đã vượt quá số lượng đang có");
                            ViewData["MessQuantity"] = message;

                            return View("YeuThich");
                        }
                    }

                }

            }
            int checkBuy = CheckBuy_(Id);
            if (checkBuy == 0)
            {
                ViewData["MessQuantity"] = ("Sản phẩm đã hết");

                return View("YeuThich");
            }
            return RedirectToAction("Details", "Product");

        }

        public int CheckBuy_(string Id)
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
            if (cart != null)
            {
                
                HttpResponseMessage response2 = service.GetResponse("api/Product/GetSoLuong/" + Id);
                response2.EnsureSuccessStatusCode();
                int quantity2 = response2.Content.ReadAsAsync<int>().Result;
                if (quantity2 <= 0)
                {
                    return 0;
                }
                List<DTO_Product_Item_Type> li = (List<DTO_Product_Item_Type>)Session["cart"];
                HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
                responseUser.EnsureSuccessStatusCode();
                DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;

                int index = isExist2(Id);
                if (index != -1)
                {
                    li[index].Quantity++;
                    Session["cart"] = li;
                    return 1;
                }
                else
                {
                    li.Add(new DTO_Product_Item_Type()
                    {

                        Quantity = 1,
                        _id = proItem._id,
                        Name = proItem.Name,
                        Price = proItem.Price,
                        Details = proItem.Details,
                        Photo = proItem.Photo,
                        Photo2 = proItem.Photo2,
                        Photo3 = proItem.Photo3,
                        Id_Item = proItem.Id_Item,
                    });
                    return 2;
                }
            }
            else
            {
                HttpResponseMessage response = service.GetResponse("api/Product/GetSoLuong/" + Id);

                response.EnsureSuccessStatusCode();
                int quantity = response.Content.ReadAsAsync<int>().Result;

                if (quantity <= 0)
                {

                    return 0;
                }
                List<DTO_Product_Item_Type> li = new List<DTO_Product_Item_Type>();

                HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
                responseUser.EnsureSuccessStatusCode();
                DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;
                li.Add(new DTO_Product_Item_Type()
                {
                    Quantity = 1,
                    _id = proItem._id,
                    Name = proItem.Name,
                    Price = proItem.Price,
                    Details = proItem.Details,
                    Photo = proItem.Photo,
                    Photo2 = proItem.Photo2,
                    Photo3 = proItem.Photo3,
                    Id_Item = proItem.Id_Item,
                });
                Session["cart"] = li;
                return 2;
            }
        }
    }
}