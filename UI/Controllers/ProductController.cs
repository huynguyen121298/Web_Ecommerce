using Model.Common;
using Model.DTO.DTO_Client;
using Model.DTO_Model;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using UI.Models;
using UI.Security_;
using UI.Service;

namespace UI.Controllers
{
    public class ProductController : Controller
    {
        private ServiceRepository service = new ServiceRepository();

        public ActionResult TypeProduct()
        {
            HttpResponseMessage responseMessage = service.GetResponse("api/Products_Ad/GetAllProductByTypeByEndUser");
            responseMessage.EnsureSuccessStatusCode();
            List<List<DTO_Dis_Product>> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<List<DTO_Dis_Product>>>().Result;
            var view = dTO_Accounts.ToPagedList(1, 50);

            return View(view);
        }

        public ActionResult Index(FormCollection fc, int? page, int? gia, int? gia_, string searchName1)
        {
            var searchName = fc["searchName"];
            var priceGiaMin = Request.Form["priceGiaMin"];
            var priceGiaMax = Request.Form["priceGiaMax"];
            var searchMerchant = fc["searchMerchant"];

            if (page == null) page = 1;
            int pageSize = 25;

            int pageNumber = (page ?? 1);

            if ((searchName != null && searchName != "") || (searchName1 != "" && searchName1 != null))
            {
                try
                {
                    HttpResponseMessage responseMessage2 = service.GetResponse("api/product/GetAllProductByName/" + searchName);
                    responseMessage2.EnsureSuccessStatusCode();

                    List<DTO_Dis_Product> dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
                    return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));
                }
                catch
                {
                    HttpResponseMessage responseMessage3 = service.GetResponse("api/product/GetAllProductByName/" + searchName1);
                    responseMessage3.EnsureSuccessStatusCode();

                    List<DTO_Dis_Product> dTO_Accounts3 = responseMessage3.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
                    return View(dTO_Accounts3.ToPagedList(pageNumber, pageSize));
                }
            }

            if (priceGiaMin != null && priceGiaMax != null && priceGiaMin != "" && priceGiaMax != "")
            {
                HttpResponseMessage responseMessage2 = service.GetResponse("api/product/GetAllProductByPrice/" + priceGiaMin + "/" + priceGiaMax);
                responseMessage2.EnsureSuccessStatusCode();

                List<DTO_Dis_Product> dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
                return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));
            }
            if (searchMerchant != null && searchMerchant != "")
            {
                // create session

                Session.Add(Constants.SEARCHMERCHANT, searchMerchant);
                return RedirectToAction("Index", "Merchant");
                //return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));
            }
            try
            {
                HttpResponseMessage responseMessage2 = service.GetResponse("api/product/GetAllProductByPrice/" + gia_ + "/" + gia);
                responseMessage2.EnsureSuccessStatusCode();

                List<DTO_Dis_Product> dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
                return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));
            }
            catch
            {
                HttpResponseMessage responseMessage = service.GetResponse("api/Products_Ad/GetAllProduct_DiscountByEndUser");
                responseMessage.EnsureSuccessStatusCode();
                List<DTO_Dis_Product> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
                return View(dTO_Accounts.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult Index_(DTO_Dis_Product dTO_Product, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 25;
            int pageNumber = (page ?? 1);

            HttpResponseMessage responseMessage = service.GetResponse("api/Products_Ad/GetAllProduct_DiscountByEndUser");
            responseMessage.EnsureSuccessStatusCode();
            List<DTO_Dis_Product> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;

            return View(dTO_Accounts.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Search(int? page, string searchName)
        {
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            HttpResponseMessage responseMessage2 = service.GetResponse("api/product/GetAllProductByName/" + searchName);
            responseMessage2.EnsureSuccessStatusCode();
            List<Model.DTO.DTO_Client.DTO_Product> dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<List<Model.DTO.DTO_Client.DTO_Product>>().Result;

            return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Index2(string id, string seachBy, string search, int? page, int? gia, int? gia_)
        {
            if (page == null) page = 1;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (seachBy == "NameProduct")
            {
                //return View(db.Products.Where(s => s.Name.StartsWith(search)).ToList().ToPagedList(pageNumber, pageSize));
                HttpResponseMessage responseMessage2 = service.GetResponse("api/product/GetAllProductByName/" + search);
                responseMessage2.EnsureSuccessStatusCode();
                List<DTO_Dis_Product> dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;

                return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));
            }
            if (seachBy == "Price")
            {
                HttpResponseMessage responseMessage2 = service.GetResponse("api/product/GetAllProductByPrice/" + gia_ + "/" + gia);
                responseMessage2.EnsureSuccessStatusCode();
                List<DTO_Dis_Product> dTO_Accounts2 = responseMessage2.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;

                return View(dTO_Accounts2.ToPagedList(pageNumber, pageSize));
            }
            HttpResponseMessage responseMessage = service.GetResponse("api/Products_Ad/GetAllProductByIdItem/" + id);
            responseMessage.EnsureSuccessStatusCode();
            List<DTO_Dis_Product> dTO_Accounts = responseMessage.Content.ReadAsAsync<List<DTO_Dis_Product>>().Result;
            if (dTO_Accounts == null)
            {
                return Content("Chưa có sản phẩm bạn đang muốn tìm kiếm");
            }

            var view = dTO_Accounts.ToPagedList(1, 10);
            return View(view);
        }

        public ActionResult Details()
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
            if (cart == null)
            {
                return RedirectToAction("Thankyou1", "Cart");
            }
            return View();
        }

        public PartialViewResult BagCart()
        {
            try
            {
                if (Session["cart"] != null)
                {
                    List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];

                    int total = cart.Count();
                    ViewBag.Quantity = total;
                }
            }
            catch
            {
                ViewBag.Quantity = 0;
            }
            return PartialView("BagCart");
        }

        public PartialViewResult BagProductRecommend()
        {
            HttpResponseMessage response2 = service.GetResponse("api/Product/GetProductRecommends");
            response2.EnsureSuccessStatusCode();
            List<DTO_Product> productRecommends = response2.Content.ReadAsAsync<List<DTO_Product>>().Result;

            return PartialView(productRecommends);
        }

        public PartialViewResult BagCart_()
        {
            try
            {
                var userLogin = (UserLogin)Session[Constants.USER_SESSION];
                if(userLogin == null)
                    return ViewBag.yeuthich = 0;

                HttpResponseMessage responseUser = service.GetResponse("api/Product/GetProductsFavorite/" + userLogin._id);
                responseUser.EnsureSuccessStatusCode();
                var proItem = responseUser.Content.ReadAsAsync<List<DTO_Product>>().Result;
                if (proItem.Any())
                {
                    int total1 = proItem.Count();
                    ViewBag.yeuthich = total1;
                }
            }
            catch
            {
                
            }
            return PartialView("BagCart_");
        }

        private int isExist(string Id)
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];

            for (int i = 0; i < cart.Count; i++)
                if (cart[i]._id.Equals(Id))
                    return i;
            return -1;
        }

        private int isExist2(string Id)
        {
            HttpResponseMessage response2 = service.GetResponse("api/Product/GetSoLuong/" + Id);
            response2.EnsureSuccessStatusCode();
            int quantity2 = response2.Content.ReadAsAsync<int>().Result;
            if (quantity2 <= 0)
            {
                return 0;
            }
            return 1;
        }

        public ActionResult Remove(string Id)
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
            int index = isExist(Id);
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return RedirectToAction("Details");
        }

        [AuthorizeLoginEndUser]
        public ActionResult Buy(string Id)
        {
            int checkBuy = CheckBuy(Id);
            if (checkBuy == 0)
            {
                return RedirectToAction("HetHang", "Cart");
            }

            return RedirectToAction("Details", "Product");
        }

        [AuthorizeLoginEndUser]
        public ActionResult Buy_Favorite(string Id)
        {
            int checkBuy = CheckBuy(Id);
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
            if (checkBuy == 0)
            {
                string message = (" Sản phẩm đã hết hàng");
                return Json(new { buy = 0, status = message });
            }
            if (checkBuy == 1)
            {
                return Json(new { buy = cart, status = "Thành công" });
            }

            return Json(new { buy = cart, status = "Thành công" });
        }

        public ActionResult Giam(string Id)
        {
            List<DTO_Product_Item_Type> li = (List<DTO_Product_Item_Type>)Session["cart"];
            HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
            responseUser.EnsureSuccessStatusCode();
            DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;

            int index = isExist(Id);
            if (index != -1)
            {
                if (li[index].Quantity - 1 > 0)
                {
                    li[index].Quantity--;
                    li[index].Price = proItem.Price * li[index].Quantity;
                }
            }

            Session["cart"] = li;
            return Json(new { soLuong = li });
        }

        [AuthorizeLoginEndUser]
        public ActionResult Update(string Id)
        {
            List<DTO_Product_Item_Type> li = (List<DTO_Product_Item_Type>)Session["cart"];
            HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
            responseUser.EnsureSuccessStatusCode();
            DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;

            int index = isExist(Id);
            if (index != -1)
            {
                li[index].Quantity++;
            }
            else
            {
                var newProduct = new DTO_Product_Item_Type();
                newProduct = proItem;
                newProduct.Quantity = 1;
                li.Add(newProduct);
            }
            Session["cart"] = li;
            return RedirectToAction("Details", "Product");
        }

        public ActionResult Tang(string Id)
        {
            int checkBoy = CheckBuy(Id);
            if (checkBoy == 0)
            {
                string message = (" Sản phẩm này đã vượt quá số lượng");
                return Json(new { soLuong = 0, status = message });
            }
            List<DTO_Product_Item_Type> li = (List<DTO_Product_Item_Type>)Session["cart"];
            HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
            responseUser.EnsureSuccessStatusCode();
            DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;
            int index = isExist(Id);
            if (index != -1)
            {
                li[index].Price = proItem.Price * li[index].Quantity;
            }
            Session["cart"] = li;
            return Json(new { soLuong = li });
        }

        public ActionResult Details1(string Id)
        {
            var userLogin = (UserLogin)Session[Constants.USER_SESSION];
            var productRecommend = new DtoProductRecommend
            {
                ProductId = Id,
                UserId = userLogin._id,
            };
            HttpResponseMessage responseProductRecommend = service.PostResponse("api/Product/InsertProductRecommend/", productRecommend);
            bool checkSuccess = responseProductRecommend.Content.ReadAsAsync<bool>().Result;
            if (!checkSuccess)
                return null;
            if (Session["cart__"] == null)
            {
                List<DTO_Product_Item_Type> li = (List<DTO_Product_Item_Type>)Session["cart"];
                HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
                responseUser.EnsureSuccessStatusCode();
                DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;

                //var product = db.Products.Find(Id);

                Session["cart__"] = proItem;
            }
            else
            {
                List<DTO_Product_Item_Type> li = (List<DTO_Product_Item_Type>)Session["cart"];
                HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
                responseUser.EnsureSuccessStatusCode();
                DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;

                Session["cart__"] = proItem;
            }
            return RedirectToAction("LuaChon", "Cart");
        }

        public PartialViewResult Search()
        {
            return PartialView("Search");
        }

        public int CheckBuy(string Id)
        {
            List<DTO_Product_Item_Type> cart = (List<DTO_Product_Item_Type>)Session["cart"];
            var userLogin = (UserLogin)Session[Constants.USER_SESSION];
            var productRecommend = new DtoProductRecommend
            {
                ProductId = Id,
                UserId = userLogin._id,
            };
            HttpResponseMessage responseProductRecommend = service.PostResponse("api/Product/InsertProductRecommend/", productRecommend);
            bool checkSuccess = responseProductRecommend.Content.ReadAsAsync<bool>().Result;
            if (!checkSuccess)
                return 0;

            if (cart != null)
            {
                foreach (var item in cart)
                {
                   
                    if (item._id == Id)
                    {
                        foreach (var item2 in item.Items)
                        {
                            HttpResponseMessage response = service.GetResponse("api/Product/GetSoLuong/" + item2._id);
                            response.EnsureSuccessStatusCode();
                            int quantity = response.Content.ReadAsAsync<int>().Result;
                            int quantityAfterBuy = quantity - (int)item.Quantity;
                            if (quantityAfterBuy <= 0)
                            {
                                return 0;
                            }
                        }
                    }
                }

                List<DTO_Product_Item_Type> li = (List<DTO_Product_Item_Type>)Session["cart"];
                HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
                responseUser.EnsureSuccessStatusCode();
                DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;

                int index = isExist(Id);
                if (index != -1)
                {
                    li[index].Quantity++;
                    Session["cart"] = li;
                    return 1;
                }
                else
                {
                    var newProduct = new DTO_Product_Item_Type();
                    newProduct = proItem;
                    newProduct.Quantity = 1;
                    li.Add(newProduct);

                    return 2;
                }
            }
            else
            {
                //HttpResponseMessage response = service.GetResponse("api/Product/GetSoLuong/" + Id);

                //response.EnsureSuccessStatusCode();
                //int quantity = response.Content.ReadAsAsync<int>().Result;

                //if (quantity <= 0)
                //{
                //    return 0;
                //}
                List<DTO_Product_Item_Type> li = new List<DTO_Product_Item_Type>();

                HttpResponseMessage responseUser = service.GetResponse("api/Products_Ad/GetProductItemById/" + Id);
                responseUser.EnsureSuccessStatusCode();
                DTO_Product_Item_Type proItem = responseUser.Content.ReadAsAsync<DTO_Product_Item_Type>().Result;

                var newProduct = new DTO_Product_Item_Type();
                newProduct = proItem;
                newProduct.Quantity = 1;
                li.Add(newProduct);

                Session["cart"] = li;
                return 2;
            }
        }
    }
}