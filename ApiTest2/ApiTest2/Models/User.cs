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
        public string Name { get; set; }
        public string GioiTinh { get; set; }
        public int IDPhongban { get; set; }
        public int IDChucVu { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }
        public int UserUpdate { get; set; }
        public Guid ObjectGUID { get; set; }

        public string Insert(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_User_Insert", new
            {
                UserName,
                Name,
                GioiTinh,
                IDPhongban,
                IDChucVu,
                Birthday,
                Email,
                NumberPhone,
                UserUpdate
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
        public static string GetOne(int ID, out User user)
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
                GioiTinh,
                IDPhongban,
                IDChucVu,
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