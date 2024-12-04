using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTest2.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Name { get; set; }
        public string GioiTinh { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }
        public int IsTeacher { get; set; }
        public int SuperAdmin { get; set; }
        public Guid ObjectGUID { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int IsActive { get; set; }

        public static string GetAllUser(out List<User> lstuser)
        {
            return DBM.GetList("usp_User_GetAll", new { }, out lstuser);
        }
        public string InsertorUpdate(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_User_InsertorUpdate", new
            {
                UserID,
                UserName,
                Name,
                PasswordHash,
                PasswordSalt,
                GioiTinh,
                IsTeacher,
                SuperAdmin,
                ObjectGUID,
                Birthday,
                Email,
                NumberPhone,
                IsActive
            });

            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
        public static string GetOneUserByUserName(string UserName, out User user)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM())
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { Document_Id });
            //    msg = dbm.GetOne(out documentCCS);
            //    return msg;
            //}
            msg = DBM.GetOne("usp_User_GetOneByUserName", new { UserName }, out user);
            if (msg.Length > 0) return msg;
            return msg;
        }
        public static string GetOneUserByID(int ID, out User user)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM())
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { Document_Id });
            //    msg = dbm.GetOne(out documentCCS);
            //    return msg;
            //}
            msg = DBM.GetOne("usp_User_GetOne", new { ID }, out user);
            if (msg.Length > 0) return msg;
            return msg;
        }

        public string Update(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_User_Update", new
            {
                UserID,
                UserName,
                Name,
                PasswordHash,
                PasswordSalt,
                GioiTinh,
                IsTeacher,
                SuperAdmin,
                Birthday,
                Email,
                NumberPhone
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }

        public string Delete(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_User_Delete", new { UserID });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
    }
}