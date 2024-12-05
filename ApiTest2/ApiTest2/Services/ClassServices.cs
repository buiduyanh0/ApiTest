using ApiTest2.Models;
using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTest2.Services
{
    public class ClassServices
    {
        public static string DoDelete(DBM dbm, string classcode, Class classs)
        {
            string msg = classs.DeleteClass(dbm);
            if (msg.Length > 0) return msg;

            string processContent = "đã xóa lớp có ID: " + classcode;

            return Log.WriteHistoryLog(dbm, processContent, classs.ObjectGUID, 0, "", 0);
        }

        public class ClassAddorUpdateInfo
        {
            public int ClassId { get; set; }
            public string ClassCode { get; set; }
            public int SubjectCode { get; set; }
            public int Semester { get; set; }
            public int TeacherId { get; set; }
            public int IsActive { get; set; }
            public Guid ObjectGuid { get; set; }
        }
        public static string InsertorUpdateToDB(int id, ClassAddorUpdateInfo oClientRequestInfo, out Class classs)
        {
            classs = new Class
            {
                ClassId = id,
                ClassCode = oClientRequestInfo.ClassCode,
                SubjectCode = oClientRequestInfo.SubjectCode,
                Semester = oClientRequestInfo.Semester,
                TeacherId = oClientRequestInfo.TeacherId,
                IsActive = oClientRequestInfo.IsActive,
                ObjectGUID = Guid.NewGuid()
            };

            DBM dbm = new DBM();
            dbm.BeginTransac();

            string msg = classs.InsertorUpdate(dbm);
            if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

            dbm.CommitTransac();

            msg = Class.GetOneClassByID(id, out Class classs1);
            if (msg.Length > 0) return msg;

            msg = Log.WriteHistoryLog(classs.ClassId == 0 ? "thêm mới" : "sửa lớp học", classs1.ObjectGUID, 0, "", 0);
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