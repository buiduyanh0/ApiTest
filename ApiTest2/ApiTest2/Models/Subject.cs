using ApiTest2.Services;
using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTest2.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public Guid ObjectGUID { get; set; }
        public int IsActive { get; set; }

        public static string GetAllSubject(out List<Subject> lstsubject)
        {
            return DBM.GetList("usp_Subject_GetAll", new { }, out lstsubject);
        }
        public string InsertorUpdate(BSS.DBM dbm)
        {
            string msg = "";

            msg = dbm.SetStoreNameAndParams("usp_Subject_InsertorUpdate", new
            {
                SubjectId,
                SubjectName,
                SubjectCode,
                ObjectGUID,
                IsActive
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
        public static string GetOneSubjectByID(int id, out Subject subject)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM(ConnectData.mysqlconnect))
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { IDChucvu });
            //    msg = dbm.GetOne(out chucvu);
            //    return msg;
            //};
            //DBM dbm = new DBM(ConnectData.mysqlconnect);
            msg = DBM.GetOne("usp_Subject_GetOne", new { id }, out subject);
            if (msg.Length > 0) return msg;
            return msg;
        }
        public static string GetOneSubjectByCode(string subjectCode, out Subject subject)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM(ConnectData.mysqlconnect))
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { IDChucvu });
            //    msg = dbm.GetOne(out chucvu);
            //    return msg;
            //};
            //DBM dbm = new DBM(ConnectData.mysqlconnect);
            msg = DBM.GetOne("usp_Subject_GetOneByCode", new { subjectCode }, out subject);
            if (msg.Length > 0) return msg;
            return msg;
        }
        public string Update(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_Subject_Update", new
            {
                SubjectId,
                SubjectCode,
                SubjectName
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }

        public string Delete(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_Subject_Delete", new { SubjectCode });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
    }
}