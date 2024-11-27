using ApiTest2.Models;
using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTest2.Services
{
    public class ChucVuServices
    {
        public static string DoDelete(DBM dbm, int id, Subject chucvu)
        {
            string msg = chucvu.Delete(dbm);
            if (msg.Length > 0) return msg;

            string processContent = "đã xóa phòng ban có ID: " + id;

            return Log.WriteHistoryLog(dbm, processContent, chucvu.ObjectGUID, 0, "", 0);
        }

        public class ChucVuAddorUpdateInfo
        {
            public string ChucVu { get; set; }
            public long MaChucVu { get; set; }
            public int IDChucVuChinh { get; set; }
        }
        public static string InsertorUpdateToDB(int id, ChucVuAddorUpdateInfo oClientRequestInfo, out Subject chucvu)
        {
            chucvu = new Subject
            {
                IDChucVu = id,
                ChucVu = oClientRequestInfo.ChucVu,
                MaChucVu = oClientRequestInfo.MaChucVu,
                IDChucVuChinh = oClientRequestInfo.IDChucVuChinh
            };

            DBM dbm = new DBM();
            dbm.BeginTransac();

            string msg = chucvu.InsertorUpdate(dbm);
            if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

            dbm.CommitTransac();

            msg = Subject.GetOneChucVuByID(id, out chucvu);
            if (msg.Length > 0) return msg;

            msg = Log.WriteHistoryLog(chucvu.IDChucVu == 0 ? "thêm mới chức vụ" : "sửa chức vụ", chucvu.ObjectGUID, 0, "", 0);
            return msg;
        }

        //public class ChucVuUpdateInfo
        //{
        //    public string ChucVu { get; set; }
        //    public long MaChucVu { get; set; }
        //    public int IDChucVuChinh { get; set; }
        //}
        //public static string ChucVuUpdateToDB(int id, ChucVuUpdateInfo oClientRequestInfo, out ChucVuModel chucvu)
        //{
        //    chucvu = new ChucVuModel
        //    {
        //        IDChucVu = id,
        //        ChucVu = oClientRequestInfo.ChucVu,
        //        MaChucVu = oClientRequestInfo.MaChucVu,
        //        IDChucVuChinh = oClientRequestInfo.IDChucVuChinh,
        //    };

        //    DBM dbm = new DBM();
        //    dbm.BeginTransac();

        //    string msg = chucvu.Update(dbm);
        //    if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

        //    dbm.CommitTransac();

        //    string processContent = "đã sửa phòng ban có ID: " + id + " nội dung sửa: " + oClientRequestInfo.ChucVu + oClientRequestInfo.MaChucVu + oClientRequestInfo.IDChucVuChinh;
        //    msg = ChucVuModel.GetOneChucVuByID(id, out chucvu);
        //    if (msg.Length > 0) return msg;

        //    msg = Log.WriteHistoryLog(dbm, processContent, chucvu.ObjectGUID, 0, "", 0);
        //    return msg;
        //}
    }
}