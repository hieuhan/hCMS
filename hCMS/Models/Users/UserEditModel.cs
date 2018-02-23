using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CMSLib;
using hCMS.Library;

namespace hCMS.Models.Users
{
    public class UserEditModel
    {
        private List<UserStatus> _listUserStatus;
        private List<UserTypes> _listUserTypes;
        private List<Genders> _listGenders;
        public int UserId { get; set; }

        public byte GenderId { get; set; }

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

        public string FullName { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "{0} (*)")]
        [Display(Name = "Email")]
        [RegularExpression(@"[a-zA-Z0-9_\.]+@[a-zA-Z]+\.[a-zA-Z]+(\.[a-zA-Z]+)*", ErrorMessage = "{0} không hợp lệ !")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^(84|0)\\d{9,10}$", ErrorMessage = "Số điện thoại không đúng ! \n Số điện thoại hợp lệ dạng 84xxxxxxxxx hoặc 0xxxxxxxxx")]
        public string Mobile { get; set; }

        public string Comment { get; set; }

        public short DefaultActionId { get; set; }

        public DateTime BirthDay { get; set; }

        public byte UserTypeId { get; set; }

        public byte UserStatusId { get; set; }

        public List<UserStatus> ListUserStatus
        {
            get => !_listUserStatus.HasValue() ? new UserStatus().GetAll() : _listUserStatus;
            set => _listUserStatus = value;
        }

        public List<UserTypes> ListUserTypes
        {
            get => !_listUserTypes.HasValue() ? UserTypes.Static_GetList() : _listUserTypes;
            set => _listUserTypes = value;
        }

        public List<Genders> ListGenders
        {
            get => !_listGenders.HasValue() ? Genders.Static_GetListAll() : _listGenders;
            set => _listGenders = value;
        }
    }
}