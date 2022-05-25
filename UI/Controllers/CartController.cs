using Model.Common;
using Model.DTO.DTO_Ad;
using Model.DTO.DTO_Client;
using Model.DTO_Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Helper;
using UI.Models;
using UI.Security_;
using UI.Service;

namespace UI.Controllers
{
    public class CartController : Controller
    {
        private ProductController productController = new ProductController();
        private ServiceRepository service = new ServiceRepository();

        [AuthorizeLoginEndUser]
        public ActionResult Index()
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];

            if (cart == null)

                return View("Thankyou1");
            else
                return View();
        }
        [AuthorizeLoginEndUser]
        public ActionResult Checkout()
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
            if (cart == null)
            {
                return View("Thankyou1");
            }
            return View();
        }

        [AuthorizeLoginEndUser]

        public ActionResult LuaChon()
        {
            return View();
        }

        [AuthorizeLoginEndUser]
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

        [AuthorizeLoginEndUser]
        public async Task<ActionResult> saveOrder1(bool isPayOnline = false)
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
            if(cart == null)
            {
                return ViewBag.Error;
            }
            var price = Request.Form["gia1"];
            var checkPayOnl = Request.Form["checkPayOnl"];
            if(isPayOnline == false)
            {
                var check = new DTOCheckoutCustomerOrder();

                check.Zipcode = Request.Form["zip"];
                check.NgayTao = DateTime.Now;
                check.FirstName = Request.Form["FirstName"];
                check.LastName = Request.Form["LastName"];
                check.Email = Request.Form["Email"];
                check.DiaChi = Request.Form["diaChi"];
                check.TongTien = Convert.ToInt32(price);
                check.City = Request.Form["city"];
                check.SDT = Int32.Parse(Request.Form["sdt"]);
                check.TrangThai = "Đang chờ";
                if (check.Zipcode != "")
                {
                    check.Zipcode = Request.Form["zip"];
                    check.GiamGia = Request.Form["discount1"] + "%";
                }
                Session.Add("PayOrder", check);
            }
            
            if (checkPayOnl == String.Empty || checkPayOnl == null)
            {
                

                try
                {
                    var checkSession = (DTOCheckoutCustomerOrder)Session["PayOrder"];
                    if (isPayOnline == true) checkSession.TrangThai = "Hoàn thành";
                    var userLogin = (UserLogin)Session[Constants.USER_SESSION];                   
                   

                    checkSession.ProductOrder = new List<DTO_Checkout_Order>();

                    var notifications = new List<DtoMerchantNotification>();

                    var dtoProductActions = new List<DtoProductAction>();
                    foreach (DTO_Product_Item_Type item in cart)
                    {
                        DTO_Checkout_Order dTO_Checkout_Order = new DTO_Checkout_Order();
                        var total = (item.Quantity * item.Price);
                        dTO_Checkout_Order.Id_SanPham = item._id;
                        dTO_Checkout_Order.TenSP = item.Name;
                        dTO_Checkout_Order.SoLuong = (int)item.Quantity;
                        dTO_Checkout_Order.Gia = total;
                        dTO_Checkout_Order.AccountId = item.AccountId;
                        checkSession.ProductOrder.Add(dTO_Checkout_Order);

                        var notification = new DtoMerchantNotification();
                        notification.DateTime = DateTime.Now;
                        notification.AccountId = item.AccountId;
                        notification.Subject = "Đơn hàng mới";
                        notification.Content = "Đơn hàng " + item.Name + " đã được đặt bởi khách hàng " + checkSession.FirstName + " " + checkSession.LastName;
                        notification.CheckoutId = item._id;
                        notifications.Add(notification);

                        var dtoProductAction = new DtoProductAction()
                        {
                            Status = ProductActionConstant.PRODUCT_BOUGHT,
                            UserId = userLogin._id,
                            ProductId = item._id
                        };
                        dtoProductActions.Add(dtoProductAction);

                    }
                    HttpResponseMessage responseUser1 = service.PostResponse("api/Cart/InsertBill/", checkSession);
                    var idBill = await responseUser1.Content.ReadAsStringAsync();
                    foreach (DTO_Product_Item_Type item in cart)
                    {
                        var notification = new DtoMerchantNotification();
                        notification.DateTime = DateTime.Now;
                        notification.AccountId = item.AccountId;
                        notification.Subject = "Đơn hàng mới";
                        notification.Content = "Đơn hàng " + item.Name + " đã được đặt bởi khách hàng " + checkSession.FirstName + " " + checkSession.LastName;
                        notification.CheckoutId = idBill;
                        notifications.Add(notification);

                        var dtoProductAction = new DtoProductAction()
                        {
                            Status = ProductActionConstant.PRODUCT_BOUGHT,
                            UserId = userLogin._id,
                            ProductId = item._id
                        };
                        dtoProductActions.Add(dtoProductAction);

                    }

                    HttpResponseMessage response2 = service.PostResponse("api/Notification/AddNotification/", notifications);
                    HttpResponseMessage response = service.PostResponse("api/Product/AddProductAction/", dtoProductActions);

                    if (responseUser1.IsSuccessStatusCode && response2.IsSuccessStatusCode && response.IsSuccessStatusCode)
                    {
                        Session.Remove("cart");
                        Session.Remove("PayOrder");
                        return View("Thankyou");
                    }
                    return RedirectToAction("Error", "Home");
                }
                catch
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];
            PayLib pay = new PayLib();

            pay.AddRequestData("vnp_Version", "2.0.0"); 
            pay.AddRequestData("vnp_Command", "pay"); 
            pay.AddRequestData("vnp_TmnCode", tmnCode); 
            pay.AddRequestData("vnp_Amount", price + "00"); 
            pay.AddRequestData("vnp_BankCode", "");
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); 
            pay.AddRequestData("vnp_CurrCode", "VND"); 
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); 
            pay.AddRequestData("vnp_Locale", "vn");
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); 
            pay.AddRequestData("vnp_OrderType", "other"); 
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); 
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);

        }

        [AuthorizeLoginEndUser]
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

        [AuthorizeLoginEndUser]
        public ActionResult YeuThich()
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart_"];
            if (cart == null)
            {
                return View("Thankyou1");
            }

            return View();
        }

        [AuthorizeLoginEndUser]
        public ActionResult HetHang()
        {
            return View();
        }

        [AuthorizeLoginEndUser]
        public ActionResult Details_(string Id)
        {
            var userLogin = (UserLogin)Session[Constants.USER_SESSION];
            if(userLogin != null)
            {
                var dtoProductActions = new List<DtoProductAction>();
                var dtoProductAction = new DtoProductAction()
                {
                    Status = ProductActionConstant.PRODUCT_FAVORITE,
                    UserId = userLogin._id,
                    ProductId = Id
                };
                dtoProductActions.Add(dtoProductAction);
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
                        IdItemType = proItem.IdItemType,
                        Quantity = 1,
                        AccountId = proItem.AccountId
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
                            IdItemType = proItem.IdItemType,
                            Quantity = 1,
                            AccountId = proItem.AccountId
                        });
                    }


                    HttpResponseMessage response = service.PostResponse("api/Product/AddProductAction/", dtoProductActions);
                    response.EnsureSuccessStatusCode();
                    Session["cart_"] = li;
                    ViewBag.IdPorduct = Id;
                    return Json(new { buy = li });
                }
            }
            return Json(new { buy = 0 });

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
            var userLogin = (UserLogin)Session[Constants.USER_SESSION];
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart_"];
            if(cart != null)
            {
                int index = isExist_(Id);
                cart.RemoveAt(index);
                Session["cart_"] = cart;
            }
            

            var dtoProductAction = new DtoProductAction()
            {
                ProductId = Id,
                UserId = userLogin._id,
                Status = ProductActionConstant.PRODUCT_FAVORITE
            };
            HttpResponseMessage response = service.PutResponse("api/Product/DeleteProductAction/", dtoProductAction);
            response.EnsureSuccessStatusCode();
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

        [AuthorizeLoginEndUser]
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

        [AuthorizeLoginEndUser]
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

        [AuthorizeLoginEndUser]
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
                        IdItemType = proItem.IdItemType,
                        AccountId = proItem.AccountId
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
                    IdItemType = proItem.IdItemType,
                    AccountId = proItem.AccountId
                });
                Session["cart"] = li;
                return 2;
            }
        }

        public ActionResult Payment()
        {
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];
            var price =Request.Form["gia1"]+"00";
            PayLib pay = new PayLib();

            pay.AddRequestData("vnp_Version", "2.0.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.0.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", price); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);
        }

        public async Task<ActionResult> PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        var test = await saveOrder1(true);
                        
                        
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }
    }
}