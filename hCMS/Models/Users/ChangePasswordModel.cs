using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace hCMS.Models.Users
{
    public class ChangePasswordModel:ViewModelBase
    {
        [Display(Name = "Tên truy cập")]
        [Required(ErrorMessage = "{0} (*)")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu cũ")]
        [StringLength(100, ErrorMessage = "{0} bao gồm từ {2} ký tự trở lên", MinimumLength = 6)]
        [Required(ErrorMessage = "Bắt buộc nhập (*)")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [StringLength(100, ErrorMessage = "{0} bao gồm từ {2} ký tự trở lên", MinimumLength = 6)]
        [Required(ErrorMessage = "Bắt buộc nhập (*)")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Mật khẩu xác nhận")]
        [StringLength(100, ErrorMessage = "{0} bao gồm từ {2} ký tự trở lên", MinimumLength = 6)]
        [Required(ErrorMessage = "Bắt buộc nhập (*)")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }
    }
}