using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibDatabase;

namespace CMSLib
{
    public class Users
    {
        private int _UserId;

        private string _Username;

        private string _Password;

        private string _UserPass;

        private string _Fullname;

        private string _Address;

        private string _Email;

        private string _Mobile;

        private string _Comments;

        private byte _UserStatusId;

        private byte _GenderId;

        private short _DefaultActionId;

        private byte _UserTypeId;

        private DateTime _Birthday;

        private DateTime _CrDateTime;

        private DBAccess db;

        public int UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                this._UserId = value;
            }
        }

        public string UserName
        {
            get
            {
                return this._Username;
            }
            set
            {
                this._Username = value;
            }
        }

        public string UserPass
        {
            get
            {
                return this._UserPass;
            }
            set
            {
                this._UserPass = value;
            }
        }

        public string Fullname
        {
            get
            {
                return this._Fullname;
            }
            set
            {
                this._Fullname = value;
            }
        }

        public string Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                this._Password = value;
            }
        }

        public string Address
        {
            get
            {
                return this._Address;
            }
            set
            {
                this._Address = value;
            }
        }

        public string Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                this._Email = value;
            }
        }

        public string Comments
        {
            get
            {
                return this._Comments;
            }
            set
            {
                this._Comments = value;
            }
        }

        public byte UserStatusId
        {
            get
            {
                return this._UserStatusId;
            }
            set
            {
                this._UserStatusId = value;
            }
        }

        public byte GenderId
        {
            get
            {
                return this._GenderId;
            }
            set
            {
                this._GenderId = value;
            }
        }

        public string Mobile
        {
            get
            {
                return this._Mobile;
            }
            set
            {
                this._Mobile = value;
            }
        }

        public short DefaultActionId
        {
            get
            {
                return this._DefaultActionId;
            }
            set
            {
                this._DefaultActionId = value;
            }
        }

        public DateTime Birthday
        {
            get
            {
                return this._Birthday;
            }
            set
            {
                this._Birthday = value;
            }
        }

        public DateTime CrDateTime
        {
            get
            {
                return this._CrDateTime;
            }
            set
            {
                this._CrDateTime = value;
            }
        }

        public byte UserTypeId
        {
            get
            {
                return this._UserTypeId;
            }
            set
            {
                this._UserTypeId = value;
            }
        }

        public Users()
        {
            this.db = new DBAccess(DbConstants.CONNECTION_STRING);
        }

        public Users(string constr)
        {
            this.db = new DBAccess(constr == "" ? DbConstants.CONNECTION_STRING : constr);
        }

        ~Users()
        {
        }

        public virtual void Dispose()
        {
        }

        private List<Users> Init(SqlCommand cmd)
        {
            SqlConnection sqlConnection = cmd.Connection = this.db.GetConnection();
            List<Users> list = new List<Users>();
            try
            {
                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartDataReader = new SmartDataReader(reader);
                while (smartDataReader.Read())
                {
                    Users users = new Users(this.db.ConnectionString)
                    {
                        UserId = smartDataReader.GetInt32("UserId"),
                        UserName = smartDataReader.GetString("Username"),
                        Password = smartDataReader.GetString("Password"),
                        UserPass = smartDataReader.GetString("UserPass"),
                        Fullname = smartDataReader.GetString("Fullname"),
                        Address = smartDataReader.GetString("Address"),
                        Email = smartDataReader.GetString("Email"),
                        Mobile = smartDataReader.GetString("Mobile"),
                        Comments = smartDataReader.GetString("Comments"),
                        UserStatusId = smartDataReader.GetByte("UserStatusId"),
                        GenderId = smartDataReader.GetByte("GenderId"),
                        DefaultActionId = smartDataReader.GetInt16("DefaultActionId"),
                        Birthday = smartDataReader.GetDateTime("Birthday"),
                        CrDateTime = smartDataReader.GetDateTime("CrDateTime"),
                        UserTypeId = smartDataReader.GetByte("UserTypeId")
                    };
                    list.Add(users);
                }
                smartDataReader.disposeReader(reader);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                this.db.closeConnection(sqlConnection);
            }
            return list;
        }

        public byte Insert(int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Users_Insert");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@UserName", this.UserName));
                //sqlCommand.Parameters.Add(new SqlParameter("@UserPass", this.UserPass));
                sqlCommand.Parameters.Add(new SqlParameter("@Password", this.Password));
                sqlCommand.Parameters.Add(new SqlParameter("@Fullname", this.Fullname));
                sqlCommand.Parameters.Add(new SqlParameter("@Address", this.Address));
                sqlCommand.Parameters.Add(new SqlParameter("@Email", this.Email));
                sqlCommand.Parameters.Add(new SqlParameter("@Mobile", this.Mobile));
                sqlCommand.Parameters.Add(new SqlParameter("@Comments", this.Comments));
                sqlCommand.Parameters.Add(new SqlParameter("@UserStatusId", this.UserStatusId));
                sqlCommand.Parameters.Add(new SqlParameter("@GenderId", this.GenderId));
                sqlCommand.Parameters.Add(new SqlParameter("@DefaultActionId", this.DefaultActionId));
                sqlCommand.Parameters.Add(Birthday == DateTime.MinValue
                    ? new SqlParameter("@Birthday", DBNull.Value)
                    : new SqlParameter("@Birthday", this.Birthday));
                sqlCommand.Parameters.Add(new SqlParameter("@UserTypeId", this.UserTypeId));
                //sqlCommand.Parameters.Add(new SqlParameter("@ActUserID", actUserId));
                sqlCommand.Parameters.Add("@UserId", SqlDbType.Int).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                this.UserId = Convert.ToInt32(sqlCommand.Parameters["@UserId"].Value ?? "0");
                sysMessageId = Convert.ToInt16(sqlCommand.Parameters["@SysMessageId"].Value ?? "0");
                retVal = Convert.ToByte(sqlCommand.Parameters["@SysMessageTypeId"].Value ?? "0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public byte InsertForDemand(int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand("Users_InsertForDemand") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@Username", this.UserName));
                //sqlCommand.Parameters.Add(new SqlParameter("@UserPass", this.UserPass));
                sqlCommand.Parameters.Add(new SqlParameter("@Password", this.Password));
                sqlCommand.Parameters.Add(new SqlParameter("@Fullname", this.Fullname));
                sqlCommand.Parameters.Add(new SqlParameter("@Address", this.Address));
                sqlCommand.Parameters.Add(new SqlParameter("@Email", this.Email));
                sqlCommand.Parameters.Add(new SqlParameter("@Mobile", this.Mobile));
                sqlCommand.Parameters.Add(new SqlParameter("@Comments", this.Comments));
                sqlCommand.Parameters.Add(new SqlParameter("@UserStatusId", this.UserStatusId));
                sqlCommand.Parameters.Add(new SqlParameter("@GenderId", this.GenderId));
                sqlCommand.Parameters.Add(new SqlParameter("@DefaultActionId", this.DefaultActionId));
                sqlCommand.Parameters.Add(Birthday == DateTime.MinValue
                    ? new SqlParameter("@Birthday", DBNull.Value)
                    : new SqlParameter("@Birthday", this.Birthday));
                sqlCommand.Parameters.Add(new SqlParameter("@UserTypeId", this.UserTypeId));
                sqlCommand.Parameters.Add(new SqlParameter("@ActUserID", actUserId));
                sqlCommand.Parameters.Add("@UserId", SqlDbType.Int).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                this.UserId = Convert.ToInt32(sqlCommand.Parameters["@UserId"].Value ?? "0");
                sysMessageId = Convert.ToInt16(sqlCommand.Parameters["@SysMessageId"].Value ?? "0");
                retVal = Convert.ToByte(sqlCommand.Parameters["@SysMessageTypeId"].Value ?? "0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public byte Update(int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Users_Update") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@Username", this.UserName));
                sqlCommand.Parameters.Add(new SqlParameter("@Password", this.Password));
                sqlCommand.Parameters.Add(new SqlParameter("@Fullname", this.Fullname));
                sqlCommand.Parameters.Add(new SqlParameter("@Address", this.Address));
                sqlCommand.Parameters.Add(new SqlParameter("@Email", this.Email));
                sqlCommand.Parameters.Add(new SqlParameter("@Mobile", this.Mobile));
                sqlCommand.Parameters.Add(new SqlParameter("@Comments", this.Comments));
                sqlCommand.Parameters.Add(new SqlParameter("@UserStatusId", this.UserStatusId));
                sqlCommand.Parameters.Add(new SqlParameter("@GenderId", this.GenderId));
                sqlCommand.Parameters.Add(new SqlParameter("@DefaultActionId", this.DefaultActionId));
                sqlCommand.Parameters.Add(Birthday == DateTime.MinValue
                    ? new SqlParameter("@Birthday", DBNull.Value)
                    : new SqlParameter("@Birthday", this.Birthday));
                sqlCommand.Parameters.Add(new SqlParameter("@UserTypeId", this.UserTypeId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", this.UserId));
                //sqlCommand.Parameters.Add(new SqlParameter("@ActUserID", actUserId));
                sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                if (sqlCommand.Parameters["@SysMessageId"].Value != DBNull.Value)
                {
                    sysMessageId = Convert.ToInt16(sqlCommand.Parameters["@SysMessageId"].Value);
                }
                if (sqlCommand.Parameters["@SysMessageTypeId"].Value != DBNull.Value)
                {
                    retVal = Convert.ToByte(sqlCommand.Parameters["@SysMessageTypeId"].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public byte InsertWithIdOrUpdate(int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand("Users_InsertWithIdOrUpdate") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@Username", this.UserName));
                sqlCommand.Parameters.Add(new SqlParameter("@UserPass", this.UserPass));
                sqlCommand.Parameters.Add(new SqlParameter("@Password", this.Password));
                sqlCommand.Parameters.Add(new SqlParameter("@Fullname", this.Fullname));
                sqlCommand.Parameters.Add(new SqlParameter("@Address", this.Address));
                sqlCommand.Parameters.Add(new SqlParameter("@Email", this.Email));
                sqlCommand.Parameters.Add(new SqlParameter("@Mobile", this.Mobile));
                sqlCommand.Parameters.Add(new SqlParameter("@Comments", this.Comments));
                sqlCommand.Parameters.Add(new SqlParameter("@UserStatusId", this.UserStatusId));
                sqlCommand.Parameters.Add(new SqlParameter("@GenderId", this.GenderId));
                sqlCommand.Parameters.Add(new SqlParameter("@DefaultActionId", this.DefaultActionId));
                sqlCommand.Parameters.Add(Birthday == DateTime.MinValue
                    ? new SqlParameter("@Birthday", DBNull.Value)
                    : new SqlParameter("@Birthday", this.Birthday));
                sqlCommand.Parameters.Add(new SqlParameter("@UserTypeId", this.UserTypeId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", this.UserId));
                sqlCommand.Parameters.Add(new SqlParameter("@ActUserID", actUserId));
                sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                if (sqlCommand.Parameters["@SysMessageId"].Value != DBNull.Value)
                {
                    sysMessageId = Convert.ToInt16(sqlCommand.Parameters["@SysMessageId"].Value);
                }
                if (sqlCommand.Parameters["@SysMessageTypeId"].Value != DBNull.Value)
                {
                    retVal = Convert.ToByte(sqlCommand.Parameters["@SysMessageTypeId"].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public byte Delete(int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Users_Delete") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", this.UserId));
                //sqlCommand.Parameters.Add(new SqlParameter("@ActUserId", actUserId));
                sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                if (sqlCommand.Parameters["@SysMessageId"].Value != DBNull.Value)
                {
                    sysMessageId = Convert.ToInt16(sqlCommand.Parameters["@SysMessageId"].Value);
                }
                if (sqlCommand.Parameters["@SysMessageTypeId"].Value != DBNull.Value)
                {
                    retVal = Convert.ToByte(sqlCommand.Parameters["@SysMessageTypeId"].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public Users Get(int userId)
        {
            try
            {
                if (userId > 0)
                {
                    string cmdText = "SELECT * FROM Users WHERE (UserId =" + userId + " )";
                    SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
                    List<Users> list = this.Init(sqlCommand);
                    if (list.Count >= 1)
                    {
                        return list[0];
                    }
                    return new Users();
                }
                return new Users();
            }
            catch
            {
                return new Users();
            }
        }

        public Users Get(string constr, int userId, List<Users> list)
        {
            Users users = new Users(constr);
            try
            {
                if (userId > 0 && list.Count > 0)
                {
                    int num = 0;
                    while (num < list.Count)
                    {
                        users = list[num];
                        if (userId == users.UserId)
                        {
                            num = list.Count;
                        }
                        else
                        {
                            num++;
                            if (num == list.Count)
                            {
                                users.UserId = -1;
                            }
                        }
                    }
                }
                else
                {
                    users = new Users(constr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }

        public Users Get(string userName)
        {
            string cmdText = "SELECT * FROM Users WHERE (username ='" + userName.Trim() + "')";
            try
            {
                SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
                List<Users> list = this.Init(sqlCommand);
                if (list.Count >= 1)
                {
                    return list[0];
                }
                return new Users();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Users> GetUserIsEmployee()
        {
            List<Users> list = new List<Users>();
            try
            {
                string cmdText = "SELECT * FROM V$UserIsEmployee";
                SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
                list = this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }

        public Users GetEmail(string email)
        {
            string cmdText = "SELECT * FROM Users WHERE (Email ='" + email.Trim() + "')";
            try
            {
                SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
                List<Users> list = this.Init(sqlCommand);
                if (list.Count >= 1)
                {
                    return list[0];
                }
                return new Users();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsMember(string roleName)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Users_IsMember") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@RoleName", roleName));
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", this.UserId));
                sqlCommand.Parameters.Add("@IsMember", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                retVal = (byte)sqlCommand.Parameters["@IsMember"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal == 1;
        }

        public static bool IsAdmin(string constr, int userId)
        {
            bool result = false;
            try
            {
                if (userId > 0)
                {
                    UserRoles userRoles = new UserRoles(constr);
                    userRoles = userRoles.GetAdmin(userId);
                    if (userRoles.UserId == userId)
                    {
                        result = true;
                    }
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static bool IsAdmin(int UserId)
        {
            return Users.IsAdmin("", UserId);
        }

        public static bool IsCustomerCare(string constr, int UserId)
        {
            bool result = false;
            try
            {
                if (UserId > 0)
                {
                    UserRoles userRoles = new UserRoles(constr);
                    userRoles = userRoles.GetCustomerCare(UserId);
                    if (userRoles.UserId == UserId)
                    {
                        result = true;
                    }
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static bool IsCustomerCare(int userId)
        {
            return Users.IsCustomerCare("", userId);
        }

        public static bool IsAdminOrCustomerCare(string constr, int userId)
        {
            bool flag = Users.IsAdmin(constr, userId);
            if (!flag)
            {
                flag = Users.IsCustomerCare(constr, userId);
            }
            return flag;
        }

        public static bool IsAdminOrCustomerCare(int userId)
        {
            return Users.IsAdminOrCustomerCare("", userId);
        }

        public List<Users> GetListOrderByUserName()
        {
            return this.GetListOrderByUserName("");
        }

        public List<Users> GetListOrderByUserName(string userName)
        {
            List<Users> list = new List<Users>();
            try
            {
                string str = "SELECT * FROM Users ";
                if (userName.Length > 0)
                {
                    str = str + " WHERE (UserName LIKE '%" + userName + "%')";
                }
                str += " ORDER BY UserName";
                SqlCommand sqlCommand = new SqlCommand(str) { CommandType = CommandType.Text };
                list = this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }

        public List<Users> GetListByUserType(short userTypeId)
        {
            List<Users> list = new List<Users>();
            try
            {
                string arg = "SELECT * FROM Users ";
                arg = arg + " WHERE UserTypeId = " + userTypeId;
                SqlCommand sqlCommand = new SqlCommand(arg) { CommandType = CommandType.Text };
                list = this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }

        public List<Users> GetListBySiteIdOrderByUserName(short siteId, byte userStatus)
        {
            List<Users> list = new List<Users>();
            try
            {
                string cmdText = "Users_GetListBySiteId_OrderByUserName";
                SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@SiteId", siteId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserStatus", userStatus));
                list = this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public List<Users> GetPage(string userName, string fullName, string address, string email, string mobile, byte genderId, byte userStatusId, byte userTypeId, string dateFrom, string dateTo, string orderBy, int pageSize, int pageIndex, ref int rowCount)
        {
            List<Users> result = new List<Users>();
            try
            {
                string cmdText = "Users_GetPage";
                SqlCommand sqlCommand = new SqlCommand(cmdText);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@UserName", userName));
                sqlCommand.Parameters.Add(new SqlParameter("@FullName", fullName));
                sqlCommand.Parameters.Add(new SqlParameter("@Address", address));
                sqlCommand.Parameters.Add(new SqlParameter("@Email", email));
                sqlCommand.Parameters.Add(new SqlParameter("@Mobile", mobile));
                sqlCommand.Parameters.Add(new SqlParameter("@GenderId", genderId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserStatusId", userStatusId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserTypeId", userTypeId));
                sqlCommand.Parameters.Add(new SqlParameter("@DateFrom", dateFrom));
                sqlCommand.Parameters.Add(new SqlParameter("@DateTo", dateTo));
                sqlCommand.Parameters.Add(new SqlParameter("@OrderBy", orderBy));
                sqlCommand.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                sqlCommand.Parameters.Add(new SqlParameter("@PageNumber", pageIndex));
                sqlCommand.Parameters.Add("@RowCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                result = this.Init(sqlCommand);
                rowCount = Convert.ToInt32(sqlCommand.Parameters["@RowCount"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public List<Users> GetListAllEmployee()
        {
            List<Users> list = new List<Users>();
            try
            {
                string str = "SELECT * FROM Users ";
                str += " WHERE UserTypeId BETWEEN 4 AND 7";
                SqlCommand sqlCommand = new SqlCommand(str) { CommandType = CommandType.Text };
                return this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Users> GetAll_NVKD()
        {
            List<Users> list = new List<Users>();
            try
            {
                string str = "SELECT * FROM Users ";
                str += " WHERE UserTypeId = 4 or UserTypeId = 6 or UserTypeId = 7";
                SqlCommand sqlCommand = new SqlCommand(str);
                sqlCommand.CommandType = CommandType.Text;
                return this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Users> GetList()
        {
            List<Users> list = new List<Users>();
            try
            {
                string cmdText = "SELECT * FROM Users";
                SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
                return this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Users> GetAll()
        {
            List<Users> list = new List<Users>();
            try
            {
                string cmdText = "SELECT * FROM V$UsersAll";
                SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
                list = this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }

        public List<Users> GetListInAdvPartners(short advPartnerId)
        {
            List<Users> list = new List<Users>();
            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand("Users_GetListInAdvPartners") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@AdvPartnerId", advPartnerId));
                list = this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }

        public List<Users> GetAllForCorrectError()
        {
            List<Users> list = new List<Users>();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Users_GetAllForCorrectError");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                list = this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }

        public List<Users> GetListInProviders(short providerId)
        {
            List<Users> list = new List<Users>();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Users_GetListInProviders");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@ProviderId", providerId));
                list = this.Init(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public void GrantToProvider(int userId, int[] groupIds)
        {
            DateTime now = DateTime.Now;
            string cmdText = "DELETE FROM UserProviders WHERE userid=@UserId";
            SqlCommand sqlCommand = new SqlCommand(cmdText);
            sqlCommand.Parameters.Add(new SqlParameter("@UserId", userId));
            this.db.ExecuteSQL(sqlCommand);
            for (int i = 0; i < groupIds.Length; i++)
            {
                if (groupIds[i] > 0)
                {
                    SqlCommand sqlCommand2 = new SqlCommand("UserProviders_Insert");
                    sqlCommand2.CommandType = CommandType.StoredProcedure;
                    sqlCommand2.Parameters.Add(new SqlParameter("@UserId", userId));
                    sqlCommand2.Parameters.Add(new SqlParameter("@ProviderID", groupIds[i]));
                    sqlCommand2.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    sqlCommand2.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                    sqlCommand2.Parameters.Add("@SysMessageTypeId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                    this.db.ExecuteSQL(sqlCommand2);
                }
            }
        }

        public void GrantToAdvPartner(int userId, int[] groupIds)
        {
            DateTime now = DateTime.Now;
            string cmdText = "DELETE FROM UserAdvPartners WHERE userid=@UserId";
            SqlCommand sqlCommand = new SqlCommand(cmdText);
            sqlCommand.Parameters.Add(new SqlParameter("@UserId", userId));
            this.db.ExecuteSQL(sqlCommand);
            for (int i = 0; i < groupIds.Length; i++)
            {
                if (groupIds[i] > 0)
                {
                    SqlCommand sqlCommand2 = new SqlCommand("UserAdvPartners_Insert");
                    sqlCommand2.CommandType = CommandType.StoredProcedure;
                    sqlCommand2.Parameters.Add(new SqlParameter("@UserId", userId));
                    sqlCommand2.Parameters.Add(new SqlParameter("@AdvPartnerId", groupIds[i]));
                    sqlCommand2.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    sqlCommand2.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                    sqlCommand2.Parameters.Add("@SysMessageTypeId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                    this.db.ExecuteSQL(sqlCommand2);
                }
            }
        }

        public bool HasPriv(int userId, string url)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Users_HasPriv") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@url", url));
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", userId));
                sqlCommand.Parameters.Add("@HasPriv", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                retVal = (byte)sqlCommand.Parameters["@HasPriv"].Value;
            }
            catch
            {
                retVal = 0;
            }
            return retVal == 1;
        }

        public void Copy(Users mu)
        {
            this.UserId = mu.UserId;
            this.UserName = mu.UserName;
            this.UserPass = mu.UserPass;
            this.Password = mu.Password;
            this.Fullname = mu.Fullname;
            this.Address = mu.Address;
            this.Email = mu.Email;
            this.Mobile = mu.Mobile;
            this.Comments = mu.Comments;
            this.GenderId = mu.GenderId;
            this.UserStatusId = mu.UserStatusId;
            this.DefaultActionId = mu.DefaultActionId;
            this.Birthday = mu.Birthday;
            this.CrDateTime = mu.CrDateTime;
        }

        public void ChangeConstr(string constr)
        {
            this.db = ((constr.Length > 0) ? new DBAccess(constr) : new DBAccess());
        }

        public static string GetRoleDescs(int userId, string delimiter)
        {
            return Users.GetRoleDescs(userId, delimiter, "");
        }

        public static string GetRoleDescs(int userId, string delimiter, string constr)
        {
            string text = "";
            try
            {
                if (userId > 0)
                {
                    if (Users.IsAdmin(userId))
                    {
                        text = "Quản trị hệ thống...";
                    }
                    else
                    {
                        delimiter = ((delimiter == "") ? (delimiter = ";") : delimiter.Trim());
                        Roles roles = new Roles(constr);
                        foreach (Roles item in roles.GetRoleByUserId(userId))
                        {
                            if (item.RoleDesc != null)
                            {
                                text = text + item.RoleDesc.Trim() + delimiter;
                            }
                        }
                        if (text.EndsWith(delimiter))
                        {
                            text = text.Substring(0, text.Length - 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                text = ex.Message;
            }
            return text;
        }

        public static string Static_GetUserNameFullName(string constr, int userId)
        {
            string result = "";
            try
            {
                if (userId > 0)
                {
                    Users users = new Users(constr);
                    users = users.Get(userId);
                    if (users.UserName != null)
                    {
                        result = users.UserName.Trim() + " - " + users.Fullname.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public static string Static_GetUserNameFullName(int userId)
        {
            return Users.Static_GetUserNameFullName("", userId);
        }

        public void UpdateStatusId(int userId, byte userStatusId)
        {
            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand("Users_UpdateStatus") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", userId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserStatusId", userStatusId));
                this.db.ExecuteSQL(sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
