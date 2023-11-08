using kingshoes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace kingshoes.Controllers
{
    public class AuthController : Controller
    {
        private KingshoesContext db = new KingshoesContext();
        // GET: Auth
        [Route("login")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                // Tìm kiếm tài khoản trong cơ sở dữ liệu
                var loginUser = db.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

                if (loginUser != null)
                {
                    // Đăng nhập thành công
                    FormsAuthentication.SetAuthCookie(loginUser.Username, false);
                    Session["Username"] = loginUser.Username;
                    // Kiểm tra quyền của người dùng và chuyển hướng đến trang tương ứng
                    if (loginUser.Decentralization == 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Auth");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            return View(user);
        }
        [Route("/register")]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid != null)
            {
                // Your logic to add the user and save changes to the database
                db.Users.Add(user);
                db.SaveChanges();

                return RedirectToAction("Index", "Auth");
            }

            return View();
        }
        [Route("logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut(); // Đăng xuất người dùng
            Session.Clear(); // Xóa dữ liệu phiên

            return RedirectToAction("Index", "Auth"); // Chuyển hướng đến trang chính (hoặc bất kỳ trang nào bạn mong muốn)
        }
    }
}