using ApiTest2.Models;
using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTest2.Services
{
    public class PhongBanServices
    {
        public static string DoDelete(DBM dbm, int id, Class phongban)
        {
            string msg = phongban.Delete(dbm);
            if (msg.Length > 0) return msg;

            string processContent = "đã xóa phòng ban có ID: " + id;

            return Log.WriteHistoryLog(dbm, processContent, phongban.ObjectGUID, 0, "", 0); ;
        }

        public class PhongBanAddorUpdateInfo
        {
            public string TenPhongban { get; set; }
            public long MaPhongBan { get; set; }
            public int IDPhongBanChinh { get; set; }
        }
        public static string InsertorUpdateToDB(int id, PhongBanAddorUpdateInfo oClientRequestInfo, out Class phongban)
        {
            phongban = new Class
            {
                IDPhongBan = id,
                TenPhongBan = oClientRequestInfo.TenPhongban,
                MaPhongBan = oClientRequestInfo.MaPhongBan,
                IDPhongBanChinh = oClientRequestInfo.IDPhongBanChinh,
            };

            DBM dbm = new DBM();
            dbm.BeginTransac();

            string msg = phongban.InsertorUpdate(dbm);
            if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

            dbm.CommitTransac();

            msg = Log.WriteHistoryLog(phongban.IDPhongBan == 0 ? "thêm mới chức vụ" : "sửa chức vụ", phongban.ObjectGUID, 0, "", 0);
            return msg;
        }

        //public class PhongBanUpdateInfo
        //{
        //    public string TenPhongban { get; set; }
        //    public long MaPhongBan { get; set; }
        //    public int IDPhongBanChinh { get; set; }
        //}
        //public static string PhongBanUpdateToDB(int id, PhongBanUpdateInfo oClientRequestInfo, out PhongBan phongban)
        //{
        //    phongban = new PhongBan
        //    {
        //        IDPhongBan = id,
        //        TenPhongBan = oClientRequestInfo.TenPhongban,
        //        MaPhongBan = oClientRequestInfo.MaPhongBan,
        //        IDPhongBanChinh = oClientRequestInfo.IDPhongBanChinh,
        //    };

        //    DBM dbm = new DBM();
        //    dbm.BeginTransac();

        //    string msg = phongban.Update(dbm);
        //    if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

        //    dbm.CommitTransac();

        //    string processContent = "đã sửa phòng ban có ID: " + id + " nội dung sửa: " + oClientRequestInfo.TenPhongban + oClientRequestInfo.MaPhongBan + oClientRequestInfo.IDPhongBanChinh;
        //    msg = PhongBan.GetOnePhongBanByID(id, out phongban);
        //    if (msg.Length > 0) return msg;

        //    msg = Log.WriteHistoryLog(dbm, processContent, phongban.ObjectGUID, 0, "", 0);
        //    return msg;
        //}
    }
}