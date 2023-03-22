using ApiTest2.Services;
using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTest2.Models
{
    public class ChucVuModel
    {
        public int IDChucVu { get; set; }
        public string ChucVu { get; set; }
        public long MaChucVu { get; set; }
        public int IDChucVuChinh { get; set; }
        public Guid ObjectGUID { get; set; }
        public int UserUpdate { get; set; }

        public static string GetAllChucVu(out List<ChucVuModel> lstchucvu)
        {
            return DBM.GetList("usp_ChucVu_GetAll", new { }, out lstchucvu);
        }
        public string InsertorUpdate(BSS.DBM dbm)
        {
            string msg = "";

            msg = dbm.SetStoreNameAndParams("usp_ChucVu_InsertorUpdate", new
            {
                IDChucVu,
                ChucVu,
                MaChucVu,
                IDChucVuChinh
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
        public static string GetOneChucVuByID(int IDChucvu, out ChucVuModel chucvu)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM(ConnectData.mysqlconnect))
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { IDChucvu });
            //    msg = dbm.GetOne(out chucvu);
            //    return msg;
            //};
            //DBM dbm = new DBM(ConnectData.mysqlconnect);
            msg = DBM.GetOne("usp_ChucVu_GetOne", new { IDChucvu }, out chucvu);
            if (msg.Length > 0) return msg;
            return msg;
        }
        public string Update(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_ChucVu_Update", new
            {
                IDChucVu,
                ChucVu,
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
            msg = dbm.SetStoreNameAndParams("usp_ChucVu_Delete", new { IDChucVu });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
    }
}