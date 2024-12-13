using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTest2.Models
{
    public class Class
    {
        public int ClassId { get; set; }
        public string ClassCode { get; set; }
        public string SubjectCode { get; set; }
        public string Semester { get; set; }
        public string TeacherId { get; set; }
        public Guid ObjectGUID { get; set; }
        public int IsActive { get; set; }

        public static string GetAllClass(out List<Class> lstclass)
        {
            return DBM.GetList("usp_Class_GetAll", new { }, out lstclass);
        }
        public string InsertorUpdate(BSS.DBM dbm)
        {
            string msg = "";

            msg = dbm.SetStoreNameAndParams("usp_Class_InsertorUpdate", new
            {
                ClassId,
                ClassCode,
                SubjectCode,
                Semester,
                TeacherId,
                ObjectGUID,
                IsActive
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
        public static string GetOneClassByID(int id, out Class classs)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM())
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { Document_Id });
            //    msg = dbm.GetOne(out documentCCS);
            //    return msg;
            //}
            msg = DBM.GetOne("usp_Class_GetOneByID", new { id }, out classs);
            if (msg.Length > 0) return msg;
            return msg;
        }
        //public static string GetOneClassByStudentCode(string StudentCode, out Class classs)
        //{
        //    string msg = "";
        //    //using (BSS.DBM dbm = new DBM())
        //    //{
        //    //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { Document_Id });
        //    //    msg = dbm.GetOne(out documentCCS);
        //    //    return msg;
        //    //}
        //    msg = DBM.GetOne("usp_Class_GetOneByStudentCode", new { StudentCode }, out classs);
        //    if (msg.Length > 0) return msg;
        //    return msg;
        //}
        public static string GetOneClassByClassCode(string classcode, out Class classs)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM())
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { Document_Id });
            //    msg = dbm.GetOne(out documentCCS);
            //    return msg;
            //}
            msg = DBM.GetOne("usp_Class_GetOneByClassCode", new { classcode }, out classs);
            if (msg.Length > 0) return msg;
            return msg;
        }
        public string Update(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_Class_Update", new
            {
                ClassId,
                ClassCode,
                SubjectCode,
                Semester,
                TeacherId
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }

        public string DeleteClass(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_Class_Delete", new { ClassCode });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
        //public string DeleteStudentInClass(BSS.DBM dbm)
        //{
        //    string msg = "";
        //    msg = dbm.SetStoreNameAndParams("usp_Class_DeleteStudent", new { StudentCode });
        //    if (msg.Length > 0) return msg;

        //    msg = dbm.ExecStore();
        //    return msg;
        //}
    }
}