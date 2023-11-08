using kingshoes.Models;
using kingshoes.ViewModels;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Xml.Linq;


namespace kingshoes.Controllers
{
    public class CartController : Controller
    {
        private KingshoesContext db = new KingshoesContext();
        // Constructor của CartController
        public CartController()
        {
            // Tính thời điểm hết hạn (24 giờ sau CreatedAt)
            var expirationTime = DateTime.Now.AddHours(-24);

            // Lấy danh sách các giỏ hàng hết hạn
            var expiredCarts = db.Carts.Where(c => c.CreatedAt <= expirationTime).ToList();

            // Xoá các giỏ hàng hết hạn
            db.Carts.RemoveRange(expiredCarts);
            db.SaveChanges();
        }

        // GET: Cart
        [Route("/")]
        [Route("cart")]
        public ActionResult Index()
        {
            //var dblist = db.Carts.ToList();
            var dbCart = db.Carts
                           .Include(c => c.Product)
                           .OrderBy(c => c.Id)
                           .Select(
                               c => new CartViewModel
                               {
                                   Id = c.Id,
                                   Name = c.Product.Name,
                                   ListImage = c.Product.ListImage,
                                   Price = c.Product.Price,
                                   Quantity = c.Quantity,
                                   TotalPrice = c.TotalPrice,
                                   ProductId = c.ProductId,
                                   Size = c.Size,
                               }
                               ).ToList();
            decimal totalCartPrice = dbCart.Sum(cart => cart.TotalPrice);
            int totalQuantity = dbCart.Sum(cart => cart.ProductId);

            ViewBag.Cart = dbCart;
            ViewBag.TotalPrice = totalCartPrice;
            ViewBag.totalQuantity = totalQuantity;
            return View();
        }
        public ActionResult AddToCart(Cart cart)
        {
            // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng hay chưa

            Cart existingCartItem = db.Carts.FirstOrDefault(c => c.ProductId == cart.ProductId);
            Product product = db.Products.FirstOrDefault(p => p.Id == cart.ProductId);
            if (existingCartItem != null)
            {
                // Nếu sản phẩm đã tồn tại, tăng số lượng và cập nhật tổng tiền
                existingCartItem.Quantity += cart.Quantity;
                existingCartItem.TotalPrice = existingCartItem.Quantity * product.Price; // Giả sử Product có một thuộc tính Price
            }
            else
            {
                // Nếu sản phẩm chưa tồn tại, thêm sản phẩm mới vào giỏ hàng
                var sessionId = Guid.NewGuid().ToString();
                Cart newCartItem = new Cart
                {
                    CartId = cart.ProductId,
                    CartItem = sessionId,
                    ProductId = cart.ProductId,
                    Quantity = cart.Quantity,
                    TotalPrice = cart.Quantity * product.Price,
                    Size = cart.Size,
                    CreatedAt = DateTime.Now,
                };

                db.Carts.Add(newCartItem);
            }

            db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

            // Trả về một ActionResult tùy thuộc vào nhu cầu của ứng dụng của bạn
            // Ví dụ: bạn có thể chuyển hướng người dùng đến trang giỏ hàng hoặc trả về một thông báo thành công, v.v.
            return RedirectToAction("Index"); // Ví dụ chuyển hướng đến trang giỏ hàng
        }


        [Route("delete")]
        public ActionResult Delete(int? id)
        {
            Cart cart = db.Carts.FirstOrDefault(c => c.Id == id);
            db.Carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Route("checkout")]
        public ActionResult Checkout(int[] idcart, string[] productid, decimal TotalPrice, string[] Quantity)
        {

            var productIds = productid.SelectMany(item => item.Split(',').Select(id => decimal.Parse(id)));
            var productsWithCartInfo = db.Products
                        .Where(p => productIds.Contains(p.Id))
                        .Select(p => new CartItem
                        {
                            Id = p.Id,
                            Name = p.Name,
                            ListImage = p.ListImage,

                        })
                        .ToList();

            // Chuyển mảng Quantity thành mảng số nguyên
            int[] quantityValues = Quantity.Select(q => int.Parse(q)).ToArray();

            // Tính tổng Quantity
            int totalQuantity = quantityValues.Sum();

            List<Cart> cartsWithSizeList = db.Carts.ToList();
            ViewBag.CartsWithSize = cartsWithSizeList;

            List<Cart> quantityList = db.Carts.ToList();
            ViewBag.QuantityList = quantityList;
            // Sử dụng totalQuantity ở đây cho mục đích của bạn
            ViewBag.TotalQuantity = totalQuantity;

            ViewBag.resProducts = productsWithCartInfo;
            ViewBag.resPrices = TotalPrice;

            foreach (var id in idcart)
            {
                Cart cart = db.Carts.FirstOrDefault(c => c.Id == id);
                db.Carts.Remove(cart);
            }
            db.SaveChanges();
            return View();
        }
        [HttpPost]
        public ActionResult Order(Order orders, string[] NameProduct, string[] Size, decimal TotalAmount, string[] Quantity)
        {
            // Tạo danh sách các đơn hàng
            List<Order> ordersList = new List<Order>();
            // Tạo một đối tượng đơn hàng mới với dữ liệu nhận được
            for (int i = 0; i < NameProduct.Length; i++)
            {

                var order = new Order
                {
                    CodeProduct = GenerateRandomCode(), // Mã duy nhất có thể tạo ở đây
                    Name = orders.Name,
                    Phone = orders.Phone,
                    Address = orders.Address,
                    City = orders.City,
                    District = orders.District,
                    Ward = orders.Ward,
                    NameProduct = NameProduct[i],
                    TotalAmount = TotalAmount,
                    Size = Size[i],
                    Quantity = Quantity[i]
                };
                ordersList.Add(order);
                // Tạo một đối tượng đơn hàng cho mỗi sản phẩm

            }
            // Lưu đơn hàng vào cơ sở dữ liệu bằng Entity Framework
            using (var db = new KingshoesContext())
            {
                db.Orders.AddRange(ordersList);
                db.SaveChanges();
            }

            return RedirectToAction("OrderSuccess");
        }

        public ActionResult OrderSuccess()
        {
            return View();
        }
        private static string GenerateRandomCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] codeArray = new char[8]; // Độ dài mã bạn có thể điều chỉnh theo mong muốn

            for (int i = 0; i < codeArray.Length; i++)
            {
                codeArray[i] = chars[random.Next(chars.Length)];
            }

            return new string(codeArray);
        }
    }
}