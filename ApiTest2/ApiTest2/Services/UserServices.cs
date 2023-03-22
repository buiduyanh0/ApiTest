using ApiTest2.Models;
using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ApiTest2.Services
{
    public class UserServices
    {
        public class UserLoginInfo
        {
            public string UserName { get; set; }
            public string pwd { get; set; }
        }
        public static string CheckLogin(UserLoginInfo oClientRequestInfo, out User user)
        {
            string msg = ApiTest2.Models.User.GetOneUserByUserName(oClientRequestInfo.UserName, out user);
            if (msg.Length > 0) return msg;

            var passwordHashLogin = GetInputPasswordHash(oClientRequestInfo.pwd, user.PasswordSalt);
            if (!user.PasswordHash.SequenceEqual(passwordHashLogin)) return "Tên đăng nhập hoặc Mật khẩu không đúng!";

            msg = Log.WriteActivityLog("Đăng nhập thành công", user.UserID, "");
            return msg;
        }

        public static string DoDelete(DBM dbm, int id, User user)
        {
            string msg = user.Delete(dbm);
            if (msg.Length > 0) return msg;

            string processContent = "đã xóa người dùng có ID: " + id;

            return Log.WriteHistoryLog(dbm, processContent, user.ObjectGUID, 0, "", 0);
        }

        public class UserAddorUpdateInfo
        {
            public int UserID { get; set; }
            public string UserName { get; set; }
            public string Name { get; set; }
            public string GioiTinh { get; set; }
            public string pwd { get; set; }
            public int IDPhongban { get; set; }
            public int IDChucVu { get; set; }
            public int IDDonVi { get; set; }
            public DateTime Birthday { get; set; }
            public string Email { get; set; }
            public string NumberPhone { get; set; }
            public int UserUpdate { get; set; }
        }
        public static string InsertorUpdateToDB(int id, UserAddorUpdateInfo oClientRequestInfo, out User user)
        {
            var pwdSalt = GenerateRandomBytes(16);
            var pwdHash = GetInputPasswordHash(oClientRequestInfo.pwd, pwdSalt);
            user = new User
            {
                UserID = id,
                Name = oClientRequestInfo.Name,
                UserName = oClientRequestInfo.UserName,
                PasswordHash = pwdHash,
                PasswordSalt = pwdSalt,
                GioiTinh = oClientRequestInfo.GioiTinh,
                IDPhongban = oClientRequestInfo.IDPhongban,
                IDChucVu = oClientRequestInfo.IDChucVu,
                IDDonVi = oClientRequestInfo.IDDonVi,
                Birthday = oClientRequestInfo.Birthday,
                Email = oClientRequestInfo.Email,
                NumberPhone = oClientRequestInfo.NumberPhone,
                UserUpdate = oClientRequestInfo.UserUpdate,
            };

            DBM dbm = new DBM();
            dbm.BeginTransac();

            string msg = user.InsertorUpdate(dbm);
            if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

            dbm.CommitTransac();

            msg = Log.WriteHistoryLog(user.UserID == 0 ? "thêm mới user" : "sửa user", user.ObjectGUID, 0, "", 0);
            return msg;
        }

        //public class UserUpdateInfo
        //{
        //    public string UserName { get; set; }
        //    public string Name { get; set; }
        //    public string pwd { get; set; }
        //    public string GioiTinh { get; set; }
        //    public int IDPhongban { get; set; }
        //    public int IDChucVu { get; set; }
        //    public int IDDonVi { get; set; }
        //    public DateTime Birthday { get; set; }
        //    public string Email { get; set; }
        //    public string NumberPhone { get; set; }
        //}
        //public static string UserUpdateToDB(int id, UserUpdateInfo oClientRequestInfo, out User user)
        //{
        //    var pwdSalt = GenerateRandomBytes(16);
        //    var pwdHash = GetInputPasswordHash(oClientRequestInfo.pwd, pwdSalt);
        //    user = new User
        //    {
        //        UserID = id,
        //        Name = oClientRequestInfo.Name,
        //        UserName = oClientRequestInfo.UserName,
        //        PasswordHash = pwdHash,
        //        PasswordSalt = pwdSalt,
        //        GioiTinh = oClientRequestInfo.GioiTinh,
        //        IDPhongban = oClientRequestInfo.IDPhongban,
        //        IDChucVu = oClientRequestInfo.IDChucVu,
        //        IDDonVi = oClientRequestInfo.IDDonVi,
        //        Birthday = oClientRequestInfo.Birthday,
        //        Email = oClientRequestInfo.Email,
        //        NumberPhone = oClientRequestInfo.NumberPhone,
        //    };

        //    DBM dbm = new DBM();
        //    dbm.BeginTransac();

        //    string msg = user.Update(dbm);
        //    if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

        //    dbm.CommitTransac();

        //    string processContent = "đã sửa phòng ban có ID: " + id + " nội dung sửa: " + oClientRequestInfo.UserName + oClientRequestInfo.Name + oClientRequestInfo.GioiTinh + oClientRequestInfo.IDPhongban + oClientRequestInfo.IDChucVu + oClientRequestInfo.Birthday + oClientRequestInfo.Email + oClientRequestInfo.NumberPhone;
        //    msg = User.GetOneUserByID(id, out user);
        //    if (msg.Length > 0) return msg;

        //    msg = Log.WriteHistoryLog(dbm, processContent, user.ObjectGUID, 0, "", 0);
        //    return msg;
        //}

        #region for password only
        public static byte[] GetInputPasswordHash(string pwd, byte[] salt)
        {
            int passwordDerivationIteration = 1000;
            int passwordBytesLength = 64;
            var inputPwdBytes = Encoding.UTF8.GetBytes(pwd);
            var inputPwdHasher = new Rfc2898DeriveBytes(inputPwdBytes, salt, passwordDerivationIteration);
            return inputPwdHasher.GetBytes(passwordBytesLength);
        }
        public static byte[] GenerateRandomBytes(int length)
        {
            var result = new byte[length];
            RandomNumberGenerator.Create().GetBytes(result);
            return result;
        }

        #endregion
    }
}