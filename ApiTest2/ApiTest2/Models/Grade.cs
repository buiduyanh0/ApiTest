using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTest2.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public string ClassCode { get; set; }
        public string StudentCode { get; set; }
        public int Score { get; set; }
        public DateTime UpdateTime { get; set; }
        public Guid ObjectGUID { get; set; }
        public int IsActive { get; set; }

        public static string GetAllGradeByClassCode(string classcode, out List<Grade> lstgrade)
        {
            return DBM.GetList("usp_Grade_GetAllGradeByClassCode", new { classcode }, out lstgrade);
        }
        public static string GetAllGradeByStudentCode(string studentcode,out List<Grade> lstgrade)
        {
            return DBM.GetList("usp_Grade_GetAllGradeByStudentCode", new { studentcode }, out lstgrade);
        }
        public string InsertorUpdate(BSS.DBM dbm)
        {
            string msg = "";

            msg = dbm.SetStoreNameAndParams("usp_Grade_InsertorUpdate", new
            {
                GradeId,
                ClassCode,
                StudentCode,
                Score,
                IsActive,
                ObjectGUID
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
        public static string GetOneGradeByID(int id, out Grade grade)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM())
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { Document_Id });
            //    msg = dbm.GetOne(out documentCCS);
            //    return msg;
            //}
            msg = DBM.GetOne("usp_Grade_GetOne", new { id }, out grade);
            if (msg.Length > 0) return msg;
            return msg;
        }
        public static string GetOneGradeByObjectGuid(Guid objectGuid, out Grade grade)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM())
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { Document_Id });
            //    msg = dbm.GetOne(out documentCCS);
            //    return msg;
            //}
            msg = DBM.GetOne("usp_Grade_GetOneByObjectGuid", new { objectGuid }, out grade);
            if (msg.Length > 0) return msg;
            return msg;
        }
        public static string GetOneGradeByCode(string studentcode, string classcode, out Grade grade)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM())
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { Document_Id });
            //    msg = dbm.GetOne(out documentCCS);
            //    return msg;
            //}
            msg = DBM.GetOne("usp_Grade_GetOneByCode", new { studentcode, classcode }, out grade);
            if (msg.Length > 0) return msg;
            return msg;
        }
        public string Update(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_Grade_Update", new
            {
                GradeId,
                ClassCode,
                StudentCode,
                Score
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }

        public string Delete(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_Grade_Delete", new { StudentCode, ClassCode });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
    }
}
