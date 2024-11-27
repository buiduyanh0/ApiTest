using BSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ApiTest2.Models
{
    public class ClassRegistration
    {
        public int ClassRegistrationId { get; set; }
        public string ClassCode { get; set; }
        public int StudentId { get; set; }
        public DateTime RegisterTime { get; set; }
        public Guid ObjectGUID { get; set; }
        public int IsActive { get; set; }

        public static string GetAllClassRegistration(out List<ClassRegistration> lstclassRegistration)
        {
            return DBM.GetList("usp_ClassRegistration_GetAll", new { }, out lstclassRegistration);
        }
        public string InsertorUpdate(BSS.DBM dbm)
        {
            string msg = "";

            msg = dbm.SetStoreNameAndParams("usp_Class_InsertorUpdate", new
            {
                ClassRegistrationId,
                ClassCode,
                StudentId
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
        public static string GetOneRegistrationByStudentId(int StudentId, out ClassRegistration classRegistation)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM())
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { Document_Id });
            //    msg = dbm.GetOne(out documentCCS);
            //    return msg;
            //}
            msg = DBM.GetOne("usp_Registration_GetOne", new { StudentId }, out classRegistation);
            if (msg.Length > 0) return msg;
            return msg;
        }
        //public string Update(BSS.DBM dbm)
        //{
        //    string msg = "";
        //    msg = dbm.SetStoreNameAndParams("usp_PhongBan_Update", new
        //    {
        //        ClassId,
        //        ClassCode,
        //        SubjectCode,
        //        Semester,
        //        TeacherId
        //    });
        //    if (msg.Length > 0) return msg;

        //    msg = dbm.ExecStore();
        //    return msg;
        //}

        //public string Delete(BSS.DBM dbm)
        //{
        //    string msg = "";
        //    msg = dbm.SetStoreNameAndParams("usp_PhongBan_Delete", new { ClassId });
        //    if (msg.Length > 0) return msg;

        //    msg = dbm.ExecStore();
        //    return msg;
        //}
    }
}