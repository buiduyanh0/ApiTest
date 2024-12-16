using ApiTest2.Models;
using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTest2.Services
{
    public class ClassRegistrationServices
    {
        public static string DoDelete(DBM dbm, string studentcode, string classcode, ClassRegistration classRegistration)
        {
            string msg = classRegistration.Delete(dbm);
            if (msg.Length > 0) return msg;

            string processContent = "đã xóa lớp đăng ký";

            return Log.WriteHistoryLog(dbm, processContent, classRegistration.ObjectGUID, 0, "", 0);
        }

        public class ClassRegistrationAddorUpdateInfo
        {
            public string ClassCode { get; set; }
            public string StudentCode { get; set; }
            public DateTime RegisterTime { get; set; }
            public Guid ObjectGUID { get; set; }
            public int IsActive { get; set; }
        }
        public static string InsertorUpdateToDB(int id, ClassRegistrationAddorUpdateInfo oClientRequestInfo, out ClassRegistration classRegistration)
        {
            classRegistration = new ClassRegistration
            {
                ClassRegistrationId = id,
                ClassCode = oClientRequestInfo.ClassCode,
                StudentCode = oClientRequestInfo.StudentCode,
                IsActive = oClientRequestInfo.IsActive,
                ObjectGUID = Guid.NewGuid()
            };

            DBM dbm = new DBM();
            dbm.BeginTransac();

            string msg = classRegistration.InsertorUpdate(dbm);
            if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

            dbm.CommitTransac();

            msg = ClassRegistration.GetOneRegistrationByStudentCode(oClientRequestInfo.StudentCode, oClientRequestInfo.ClassCode, out ClassRegistration classRegistration1);
            if (msg.Length > 0) return msg;

            msg = Log.WriteHistoryLog(classRegistration.ClassRegistrationId == 0 ? "thêm mới" : "sửa lớp học", classRegistration.ObjectGUID, 0, "", 0);
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
