using ApiTest2.Models;
using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTest2.Services
{
    public class AttendanceServices
    {
        public static string DoDelete(DBM dbm, int id, Attendance attendance)
        {
            string msg = attendance.Delete(dbm);
            if (msg.Length > 0) return msg;

            string processContent = "đã xóa lớp đăng ký";

            return Log.WriteHistoryLog(dbm, processContent, attendance.ObjectGuid, 0, "", 0);
        }

        public class AttendanceAddorUpdateInfo
        {
            public int AttendanceId { get; set; }
            public string ClassCode { get; set; }
            public string StudentCode { get; set; }
            public DateTime AttentdaceDate { get; set; }
            public byte IsPresent { get; set; }
            public Guid ObjectGuid { get; set; }
            public int IsActive { get; set; }
        }
        public static string InsertorUpdateToDB(int id, AttendanceAddorUpdateInfo oClientRequestInfo, out Attendance attendance)
        {
            attendance = new Attendance
            {
                AttendanceId = id,
                ClassCode = oClientRequestInfo.ClassCode,
                StudentCode = oClientRequestInfo.StudentCode,
                AttentdaceDate = DateTime.Now,
                ObjectGuid = Guid.NewGuid(),
                IsActive = oClientRequestInfo.IsActive
            };

            DBM dbm = new DBM();
            dbm.BeginTransac();

            string msg = attendance.InsertorUpdate(dbm);
            if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

            dbm.CommitTransac();

            msg = Attendance.GetOneAttendanceByID(id, out Attendance attendance1);
            if (msg.Length > 0) return msg;

            msg = Log.WriteHistoryLog(attendance.AttendanceId == 0 ? "thêm mới" : "sửa lớp học", attendance1.ObjectGuid, 0, "", 0);
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
