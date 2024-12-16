using ApiTest2.Models;
using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTest2.Services
{
    public class SubjectServices
    {
        public static string DoDelete(DBM dbm, string subjectcode, Subject subject)
        {
            string msg = subject.Delete(dbm);
            if (msg.Length > 0) return msg;

            string processContent = "đã xóa học phần có ID: " + subjectcode;

            return Log.WriteHistoryLog(dbm, processContent, subject.ObjectGUID, 0, "", 0); ;
        }

        public class SubjectAddorUpdateInfo
        {
            public string SubjectName { get; set; }
            public string SubjectCode { get; set; }
            public Guid ObjectGuid { get; set; }
            public int IsActive { get; set; }
        }
        public static string InsertorUpdateToDB(int id, SubjectAddorUpdateInfo oClientRequestInfo, out Subject subject)
        {
            subject = new Subject
            {
                SubjectId = id,
                SubjectCode = oClientRequestInfo.SubjectCode,
                SubjectName = oClientRequestInfo.SubjectName,
                ObjectGUID = Guid.NewGuid(),
                IsActive = oClientRequestInfo.IsActive
            };

            DBM dbm = new DBM();
            dbm.BeginTransac();

            string msg = subject.InsertorUpdate(dbm);
            if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

            dbm.CommitTransac();

            msg = Subject.GetOneSubjectByCode(oClientRequestInfo.SubjectCode, out Subject supject1);
            if (msg.Length > 0) return msg;

            msg = Log.WriteHistoryLog(subject.SubjectId == 0 ? "thêm mới môn học" : "sửa môn học", supject1.ObjectGUID, 0, "", 0);
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