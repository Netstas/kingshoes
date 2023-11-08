using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kingshoes.Models
{
    public class User
    {
        public int Id { get; set; }
        [Display(Name = "Tên tài khoản")]
        [Required(ErrorMessage = "Tên đăng nhập là trường bắt buộc.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải có ít nhất 3 ký tự và tối đa 50 ký tự.")]
        public string Username { get; set; }
        [Display(Name = "Số điện thoại")]
        [StringLength(15, ErrorMessage = "Số điện thoại không được quá 15 ký tự.")]
        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string Phone { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu là trường bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; }
        [Display(Name = "Phân quyền")]
        [Range(0, 1, ErrorMessage = "Phân quyền phải là 0 hoặc 1.")]
        public int Decentralization { get; set; } = 1;
    }
}