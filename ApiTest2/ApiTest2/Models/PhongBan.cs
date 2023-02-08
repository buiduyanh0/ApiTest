using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTest2.Models
{
    public class PhongBan
    {
        public int IDPhongBan { get; set; }
        public string TenPhongban { get; set; }
        public long MaPhongBan { get; set; }
        public int IDPhongBanChinh { get; set; }
        public Guid objectGUID { get; set; }

        public string Insert(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_PhongBan_Insert", new
            {
                TenPhongban,
                MaPhongBan,
                IDPhongBanChinh
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
        public static string GetOne(int IDPhongBan, out PhongBan phongBan)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM())
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { Document_Id });
            //    msg = dbm.GetOne(out documentCCS);
            //    return msg;
            //}
            msg = DBM.GetOne("usp_ChucVu_GetOne", new { IDPhongBan }, out phongBan);
            if (msg.Length > 0) return msg;
            return msg;
        }
        public string Update(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_ChucVu_Update", new
            {
                TenPhongban,
                MaPhongBan,
                IDPhongBanChinh
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }

        public string Delete(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_ChucVu_Delete", new { IDPhongBan });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
    }
}