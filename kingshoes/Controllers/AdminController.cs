using kingshoes.Models;
using kingshoes.ViewModels;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace kingshoes.Controllers
{
    public class AdminController : Controller
    {
        private KingshoesContext db = new KingshoesContext();
        // GET: admin
        [Route("~/admin")]
        [Route("~/admin/index")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("~/admin/login")]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                // Tìm kiếm tài khoản trong cơ sở dữ liệu
                var loginUser = db.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

                if (loginUser != null)
                {
                    if (loginUser.Decentralization == 0)
                    {
                        // Đăng nhập thành công
                        FormsAuthentication.SetAuthCookie(loginUser.Username, false);
                        Session["UsernameAdmin"] = loginUser.Username;
                        // Kiểm tra quyền của người dùng và chuyển hướng đến trang tương ứng

                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }
                else
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            return View(user);
        }
        [Route("~/admin/logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Admin");
        }
        [Route("~/admin/dashboard")]
        public ActionResult Dashboard()
        {
            return View();
        }
        //Account
        [Route("~/admin/account")]
        public ActionResult Account()
        {
            var dblist = db.Users.ToList();
            return View(dblist);
        }
        public ActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateAccount(User user)
        {
            if (ModelState.IsValid != null)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Account");
            }
            return View();
        }
        public ActionResult EditAccount(int? id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult EditAccount(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Account");
            }
            return View(user);
        }
        public ActionResult DeleteAccount(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Account");
        }
        // product
        [Route("~/admin/product")]
        public ActionResult Product()
        {
            var dblist = db.Products
                .Include(p => p.ProductCategory)
                .OrderBy(p => p.Id)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Size = p.Size,
                    Price = p.Price,
                    Gender = p.Gender,
                    Code = p.Code,
                    ListImage = p.ListImage,
                    CategoryName = p.ProductCategory.Name
                })
                .ToList();

            ViewBag.dblist = dblist;
            return View();
        }


        [Route("~/admin/createproduct")]
        public ActionResult CreateProduct()
        {
            var category = db.Category.ToList();
            ViewBag.category = category;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProductsCreate(Product product, List<HttpPostedFileBase> ListImage)
        {
            try
            {
                if (ModelState.IsValid != null)
                {

                    db.Products.Add(product);
                    db.Products.Add(product);
                    db.SaveChanges();

                    var imagePaths = new List<string>();
                    foreach (var image in ListImage)
                    {
                        if (image != null && image.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(image.FileName);
                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                            var filePath = Path.Combine(Server.MapPath("~/Asset/Uploads"), uniqueFileName);
                            image.SaveAs(filePath);
                            imagePaths.Add(uniqueFileName);
                        }
                    }

                    product.ListImage = string.Join(",", imagePaths);
                    db.SaveChanges();

                    return RedirectToAction("Product");
                }
                var categoryNames = db.Products.ToList();
                ViewBag.CategoryNames = categoryNames;
                return View(product);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        [HttpGet]
        [Route("~/admin/editproduct")]
        public ActionResult EditProduct(int id)
        {
            // Lấy thông tin sản phẩm cần chỉnh sửa từ cơ sở dữ liệu
            var product = db.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                // Xử lý trường hợp sản phẩm không tồn tại
                return HttpNotFound();
            }
            var category = db.Category.ToList();
            ViewBag.category = category;
            return View(product);
        }
        [HttpPost]
        public ActionResult UpdateProduct(Product model, HttpPostedFileBase[] ListImage)
        {
            if (ModelState.IsValid != null)
            {
                // Tạo một thư mục để lưu trữ ảnh sản phẩm trên máy chủ
                string imageDirectory = Server.MapPath("~/Asset/Uploads");

                // Kiểm tra xem người dùng đã tải lên ảnh mới hay chưa
                //if (ListImage != null && ListImage.Length > 0)
                //{
                // Xoá các ảnh cũ trong thư mục
                if (!string.IsNullOrEmpty(model.ListImage))
                {
                    string[] oldImagePaths = model.ListImage.Split(';');
                    foreach (string oldImagePath in oldImagePaths)
                    {
                        string oldImageFullPath = Path.Combine(imageDirectory, oldImagePath);
                        if (System.IO.File.Exists(oldImageFullPath))
                        {
                            System.IO.File.Delete(oldImageFullPath);
                        }
                    }
                }

                // Lưu các ảnh mới vào thư mục trên máy chủ và cập nhật danh sách ListImage
                List<string> newImagePaths = new List<string>();
                foreach (HttpPostedFileBase newImage in ListImage)
                {
                    if (newImage != null && newImage.ContentLength > 0)
                    {
                        string newImageFileName = Guid.NewGuid() + Path.GetExtension(newImage.FileName);
                        string newImageFullPath = Path.Combine(imageDirectory, newImageFileName);
                        newImage.SaveAs(newImageFullPath);
                        newImagePaths.Add(newImageFileName);
                    }
                }
                model.ListImage = string.Join(";", newImagePaths);

                Product existingProduct = db.Products.Find(model.Id);
                if (existingProduct != null)
                {
                    existingProduct.Name = model.Name;
                    existingProduct.Code = model.Code;
                    existingProduct.Price = model.Price;
                    existingProduct.Size = model.Size;
                    existingProduct.Number = model.Number;
                    existingProduct.CategoryId = model.CategoryId;
                    existingProduct.Gender = model.Gender;
                    existingProduct.Describe = model.Describe;

                    // Cập nhật danh sách ảnh
                    existingProduct.ListImage = string.Join(",", newImagePaths);

                    // Save changes to the database
                    db.SaveChanges();

                    return RedirectToAction("Product", "Admin");
                }
                //}
                //else
                //{
                //    // Handle the case where the product with the given ID was not found
                //    ModelState.AddModelError("", "Product not found");
                //}
                return RedirectToAction("Product", "Admin");
            }
            else
            {
                // Hiển thị thông báo lỗi nếu có
                return View(model);
            }
        }
        [Route("~/admin/deleteproduct/{id}")]
        public ActionResult DeleteProduct(int? id)
        {
            // Đầu tiên, lấy thông tin sản phẩm cần xóa
            var product = db.Products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                // Lấy danh sách các tên tệp ảnh liên quan đến sản phẩm
                var imageNames = product.ListImage.Split(',').ToList();

                // Xóa sản phẩm khỏi cơ sở dữ liệu
                db.Products.Remove(product);
                db.SaveChanges();

                // Sau đó, xóa các tệp ảnh từ thư mục
                foreach (var imageName in imageNames)
                {
                    // Đường dẫn thư mục chứa ảnh (sử dụng Server.MapPath)
                    string imageFolderPath = Server.MapPath("~/Asset/Uploads");

                    // Tạo đường dẫn tuyệt đối đến tệp ảnh
                    string imagePath = Path.Combine(imageFolderPath, imageName);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                return RedirectToAction("Product");
            }
            return RedirectToAction("Product");
        }

        [Route("~/admin/category")]
        public ActionResult Category()
        {
            var dblist = db.Category.ToList();
            return View(dblist);
        }
        public ActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Category.Add(category);
                db.SaveChanges();
                return RedirectToAction("Category");
            }
            return View();
        }
        public ActionResult EditCategory(int? id)
        {
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost]
        public ActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Category");
            }
            return View(category);
        }
        public ActionResult DeleteCategory(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Category categorys = db.Category.Find(id);
            db.Category.Remove(categorys);
            db.SaveChanges();
            return RedirectToAction("Category");
        }

        public ActionResult Order()
        {
            var dblist = db.Orders.ToList();
            return View(dblist);
        }
        public ActionResult DeleteOrder(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Order orders = db.Orders.Find(id);
            db.Orders.Remove(orders);
            db.SaveChanges();
            return RedirectToAction("Order");
        }
    }
}