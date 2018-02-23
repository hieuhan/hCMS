using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace hCMS.Models.Users
{
    public class UserLoginModel
    {
        [Display(Name = "Tên truy cập")]
        [Required(ErrorMessage = "Vui lòng nhập {0} (*)")]
        [RegularExpression("^[a-z0-9]*$", ErrorMessage =
            "{0} không bao gồm khoảng trắng, chỉ bao gồm chữ cái thường và số.")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [StringLength(100, ErrorMessage = "Mật khẩu bao gồm 6 ký tự trở lên.", MinimumLength = 6)]
        [Required(ErrorMessage = "Vui lòng nhập {0} (*)")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}