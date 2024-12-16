using BSS;
using DocumentFormat.OpenXml.VariantTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ApiTest2.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public string ClassCode { get; set; }
        public string StudentCode { get; set; }
        public DateTime AttentdaceDate { get; set; }
        public byte IsPresent { get; set; }
        public Guid ObjectGuid { get; set; }
        public int IsActive { get; set; }  

        public static string GetAllAttendance(out List<Attendance> lstattendance)
        {
            return DBM.GetList("usp_Attendance_GetAll", new { }, out lstattendance);
        }
        public string InsertorUpdate(BSS.DBM dbm)
        {
            string msg = "";

            msg = dbm.SetStoreNameAndParams("usp_Attendance_InsertorUpdate", new
            {
                AttendanceId,
                ClassCode,
                StudentCode,
                IsPresent,
                ObjectGuid,
                IsActive
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
        public static string GetOneAttendanceByID(int AttendanceId, out Attendance attendance)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM())
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { Document_Id });
            //    msg = dbm.GetOne(out documentCCS);
            //    return msg;
            //}
            msg = DBM.GetOne("usp_Attendance_GetOne", new { AttendanceId }, out attendance);
            if (msg.Length > 0) return msg;
            return msg;
        }
        public static string GetOneAttendanceByObjectGuid(Guid objectGuid, out Attendance attendance)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM())
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { Document_Id });
            //    msg = dbm.GetOne(out documentCCS);
            //    return msg;
            //}
            msg = DBM.GetOne("usp_Attendance_GetOneByObjectGuid", new { objectGuid }, out attendance);
            if (msg.Length > 0) return msg;
            return msg;
        }
        public static string GetAllAttendanceByClassCode(string classcode, out Attendance attendance)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM())
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { Document_Id });
            //    msg = dbm.GetOne(out documentCCS);
            //    return msg;
            //}
            msg = DBM.GetOne("usp_Attendance_GetOneByClassCode", new { classcode }, out attendance);
            if (msg.Length > 0) return msg;
            return msg;
        }
        public static string GetAllAttendanceByStudentCode(string studentcode, string classcode, out Attendance attendance)
        {
            string msg = "";
            //using (BSS.DBM dbm = new DBM())
            //{
            //    dbm.SetStoreNameAndParams("usp_tbl_DocumentCCS_GetOne", new { Document_Id });
            //    msg = dbm.GetOne(out documentCCS);
            //    return msg;
            //}
            msg = DBM.GetOne("usp_Attendance_GetOneByStudentCode", new { studentcode, classcode }, out attendance);
            if (msg.Length > 0) return msg;
            return msg;
        }
        public string Update(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_Attendance_Update", new
            {
                AttendanceId,
                ClassCode,
                StudentCode,
                IsPresent
            });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }

        public string Delete(BSS.DBM dbm)
        {
            string msg = "";
            msg = dbm.SetStoreNameAndParams("usp_Attendance_Delete", new { AttendanceId });
            if (msg.Length > 0) return msg;

            msg = dbm.ExecStore();
            return msg;
        }
    }
}