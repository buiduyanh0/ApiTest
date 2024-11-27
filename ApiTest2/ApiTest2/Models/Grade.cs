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
        public int ClassId { get; set; }
        public int StudentId { get; set; }
        public int Score { get; set; }
        public DateTime UpdateTime { get; set; }
        public Guid ObjectGUID { get; set; }
        public int IsActive { get; set; }

        public static string GetAllGrade(out List<Grade> lstgrade)
        {
            return DBM.GetList("usp_Grade_GetAll", new { }, out lstgrade);
        }
        public string InsertorUpdate(BSS.DBM dbm)
        {
            string msg = "";

            msg = dbm.SetStoreNameAndParams("usp_Grade_InsertorUpdate", new
            {
                GradeId,
                ClassId,
                StudentId,
                Score
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
        public static string GetOneGradeByStudentID(int StudentId, out Grade grade)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM())
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { Document_Id });
            //    msg = dbm.GetOne(out documentCCS);
            //    return msg;
            //}
            msg = DBM.GetOne("usp_Grade_GetOne", new { StudentId }, out grade);
            if (msg.Length > 0) return msg;
            return msg;
        }
        public string Update(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_DonVi_Update", new
            {
                GradeId,
                ClassId,
                StudentId,
                Score
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }

        public string Delete(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_Grade_Delete", new { GradeId });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
    }
}
