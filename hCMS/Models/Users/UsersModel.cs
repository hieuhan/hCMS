using System.Collections.Generic;
using CMSLib;
using hCMS.Library;

namespace hCMS.Models.Users
{
    public class UsersModel : ViewModelBase
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string OrderBy { get; set; }
        public byte GenderId { get; set; }
        public byte UserStatusId { get; set; }
        public byte UserTypeId { get; set; }

        public int RowCount { get; set; }

        public int[] UsersId { get; set; }

        private List<UserStatus> _listUserStatus;
        private List<UserTypes> _listUserTypes;
        public List<CMSLib.Users> ListUsers { get; set; }

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
    }
}