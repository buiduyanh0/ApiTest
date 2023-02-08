using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTest2.Models
{
    public class ChucVu
    {
        public int IDChucvu { get; set; }
        public string TenChucVu { get; set; }
        public long MaChucVu { get; set; }
        public int IDChucVuChinh { get; set; }
        public Guid objectGUID { get; set; }

        public string Insert(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_ChucVu_Insert", new
            {
                TenChucVu,
                MaChucVu,
                IDChucVuChinh
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
        public static string GetOne(int IDChucvu, out ChucVu chucVu)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM())
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { Document_Id });
            //    msg = dbm.GetOne(out documentCCS);
            //    return msg;
            //}
            msg = DBM.GetOne("usp_ChucVu_GetOne", new { IDChucvu }, out chucVu);
            if (msg.Length > 0) return msg;
            return msg;
        }
        public string Update(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_ChucVu_Update", new
            {
                TenChucVu,
                MaChucVu,
                IDChucVuChinh
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }

        public string Delete(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_ChucVu_Delete", new { IDChucvu });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
    }
}