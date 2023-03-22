using ApiTest2.Models;
using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTest2.Services
{
    public class DonViServices
    {
        public static string DoDelete(DBM dbm, int id, DonVi donvi)
        {
            string msg = donvi.Delete(dbm);
            if (msg.Length > 0) return msg;

            string processContent = "đã xóa phòng ban có ID: " + id;

            return Log.WriteHistoryLog(dbm, processContent, donvi.ObjectGUID, 0, "", 0); ;
        }

        public class DonViAddorUpdateInfo
        {
            public string MaSoThue { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string NumberPhone { get; set; }
        }
        public static string InsertorUpdateToDB(int id, DonViAddorUpdateInfo oClientRequestInfo, out DonVi donvi)
        {
            donvi = new DonVi
            {
                IDDonVi = id,
                MaSoThue = oClientRequestInfo.MaSoThue,
                Name = oClientRequestInfo.Name,
                Address = oClientRequestInfo.Address,
                Email = oClientRequestInfo.Email,
                NumberPhone = oClientRequestInfo.NumberPhone,
            };

            DBM dbm = new DBM();
            dbm.BeginTransac();

            string msg = donvi.InsertorUpdate(dbm);
            if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

            dbm.CommitTransac();

            msg = Log.WriteHistoryLog(donvi.IDDonVi == 0 ? "thêm mới đơn vị" : "sửa đơn vị", donvi.ObjectGUID, 0, "", 0);
            return msg;
        }

        //public class DonViUpdateInfo
        //{
        //    public string MaSoThue { get; set; }
        //    public string Name { get; set; }
        //    public string Address { get; set; }
        //    public string Email { get; set; }
        //    public string NumberPhone { get; set; }
        //}
        //public static string DonViUpdateToDB(int id, DonViUpdateInfo oClientRequestInfo, out DonVi donvi)
        //{
        //    donvi = new DonVi
        //    {
        //        IDDonVi = id,
        //        MaSoThue = oClientRequestInfo.MaSoThue,
        //        Name = oClientRequestInfo.Address,
        //        Email = oClientRequestInfo.Email,
        //        NumberPhone = oClientRequestInfo.NumberPhone,
        //    };

        //    DBM dbm = new DBM();
        //    dbm.BeginTransac();

        //    string msg = donvi.Update(dbm);
        //    if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

        //    dbm.CommitTransac();

        //    string processContent = "đã sửa phòng ban có ID: " + id + " nội dung sửa: " + oClientRequestInfo.MaSoThue + oClientRequestInfo.Address + oClientRequestInfo.Email + oClientRequestInfo.NumberPhone;
        //    msg = DonVi.GetOneDonViByID(id, out donvi);
        //    if (msg.Length > 0) return msg;

        //    msg = Log.WriteHistoryLog(dbm, processContent, donvi.ObjectGUID, 0, "", 0);
        //    return msg;
        //}
    }
}