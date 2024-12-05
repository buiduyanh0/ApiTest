using ApiTest2.Models;
using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTest2.Services
{
    public class GradeServices
    {
        public static string DoDelete(DBM dbm, string studentcode, string classcode, Grade grade)
        {
            string msg = grade.Delete(dbm);
            if (msg.Length > 0) return msg;

            string processContent = "đã xóa điểm của sinh viên: " + studentcode;

            return Log.WriteHistoryLog(dbm, processContent, grade.ObjectGUID, 0, "", 0); ;
        }

        public class GradeAddorUpdateInfo
        {
            public int GradeId { get; set; }
            public int ClassCode { get; set; }
            public int StudentCode { get; set; }
            public int Score { get; set; }
            public Guid ObjectGUID { get; set; }
            public int IsActive { get; set; }
        }
        public static string InsertorUpdateToDB(int id, GradeAddorUpdateInfo oClientRequestInfo, out Grade grade)
        {
            grade = new Grade
            {
                GradeId = id,
                ClassCode = oClientRequestInfo.ClassCode,
                StudentCode = oClientRequestInfo.StudentCode,
                Score = oClientRequestInfo.Score,
                ObjectGUID = Guid.NewGuid(),
                IsActive = oClientRequestInfo.IsActive

            };

            DBM dbm = new DBM();
            dbm.BeginTransac();

            string msg = grade.InsertorUpdate(dbm);
            if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

            dbm.CommitTransac();

            msg = Grade.GetOneGradeByID(id, out Grade grade1);
            if (msg.Length > 0) return msg;

            msg = Log.WriteHistoryLog(grade.GradeId == 0 ? "thêm mới chức vụ" : "sửa chức vụ", grade1.ObjectGUID, 0, "", 0);
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