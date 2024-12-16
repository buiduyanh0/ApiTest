using ApiTest2.Models;
using ApiTest2.Services;
using BSS;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace ApiTest2.Controllers
{
    [RoutePrefix("api/attendance")]
    public class AttendanceController : ApiController
    {
        #region lấy dữ liệu điểm danh
        //[Authorize]
        [HttpGet]
        [Route("")]
        public Result GetList()
        {
            var identity = User.Identity as ClaimsIdentity;
            string isTeacherString = identity.FindFirst("IsTeacher")?.Value;
            bool isTeacher = bool.TryParse(isTeacherString, out bool resultIsTeacher) ? resultIsTeacher : false;

            string superAdminString = identity.FindFirst("SuperAdmin")?.Value;
            bool superAdmin = bool.TryParse(superAdminString, out bool resultSuperAdmin) ? resultSuperAdmin : false;

            string username = identity.FindFirst(ClaimTypes.Name)?.Value;
            string userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (identity != null)
            {
                if (superAdmin || isTeacher)
                {
                    string msg = Attendance.GetAllAttendance(out List<Attendance> lstattendance);
                    if (msg.Length > 0) return msg.ToMNFResultError("GetAllAttendance");

                    return lstattendance.ToResultOk();
                }
                else
                {
                    string msg = "Bạn không có quyền truy cập";

                    return Result.GetResultError(msg);
                }
            } else
            {
                string msg = "Bạn cần đăng nhập";

                return Result.GetResultError(msg);
            }
            
        }
        #endregion

        //#region lấy dữ liệu lớp học
        //[Authorize]
        //[HttpGet]
        //[Route("{studentcode:string}")]
        //public Result GetListByStudentCode(string studentcode)
        //{
        //    string msg = ApiTest2.Models.Class.GetOneClassByStudentCode(studentcode, out Class classs);
        //    if (msg.Length > 0) return msg.ToMNFResultError("GetOneClassByStudentCode", new { studentcode });

        //    return classs.ToResultOk();
        //}
        //#endregion

        #region lấy dữ liệu điểm danh theo lớp học
        [HttpGet]
        [Route("{classcode}")]
        public Result GetAllAttendanceByClassCode(string classcode)
        {
            var identity = User.Identity as ClaimsIdentity;
            string isTeacherString = identity.FindFirst("IsTeacher")?.Value;
            bool isTeacher = bool.TryParse(isTeacherString, out bool resultIsTeacher) ? resultIsTeacher : false;

            string superAdminString = identity.FindFirst("SuperAdmin")?.Value;
            bool superAdmin = bool.TryParse(superAdminString, out bool resultSuperAdmin) ? resultSuperAdmin : false;

            string username = identity.FindFirst(ClaimTypes.Name)?.Value;
            string userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (identity != null)
            {
                if (superAdmin || isTeacher)
                {
                    string msg = ApiTest2.Models.Attendance.GetAllAttendanceByClassCode(classcode, out Attendance attendance);
                    if (msg.Length > 0) return msg.ToMNFResultError("GetAllAttendanceByClassCode", new { classcode });

                    return attendance.ToResultOk();
                }
                else
                {
                    string msg = "Bạn không có quyền truy cập";

                    return Result.GetResultError(msg);
                }
            } else
            {
                string msg = "Bạn cần đăng nhập";

                return Result.GetResultError(msg);
            }
            
        }
        #endregion

        #region lấy dữ liệu điểm danh theo sinh viên
        [HttpGet]
        [Route("{classcode}/{studentcode}")]
        public Result GetAllAttendanceByStudentCode(string studentcode, string classcode)
        {
            var identity = User.Identity as ClaimsIdentity;
            string isTeacherString = identity.FindFirst("IsTeacher")?.Value;
            bool isTeacher = bool.TryParse(isTeacherString, out bool resultIsTeacher) ? resultIsTeacher : false;

            string superAdminString = identity.FindFirst("SuperAdmin")?.Value;
            bool superAdmin = bool.TryParse(superAdminString, out bool resultSuperAdmin) ? resultSuperAdmin : false;

            string username = identity.FindFirst(ClaimTypes.Name)?.Value;
            string userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (identity != null)
            {
                string msg = ApiTest2.Models.Attendance.GetAllAttendanceByStudentCode(studentcode, classcode, out Attendance attendance);
                if (msg.Length > 0) return msg.ToMNFResultError("GetAllAttendanceByStudentCode", new { studentcode });

                return attendance.ToResultOk();
            }
            else
            {
                string msg = "Bạn cần đăng nhập";

                return Result.GetResultError(msg);
            }
        }
        #endregion

        #region add thông tin lớp học mới
        //[Authorize]
        [HttpPost]
        [Route("edit/{id:int}")]
        public Result AttendanceAddorUpdate(int id, AttendanceServices.AttendanceAddorUpdateInfo oClientRequestInfo)
        {
            var identity = User.Identity as ClaimsIdentity;
            string isTeacherString = identity.FindFirst("IsTeacher")?.Value;
            bool isTeacher = bool.TryParse(isTeacherString, out bool resultIsTeacher) ? resultIsTeacher : false;

            string superAdminString = identity.FindFirst("SuperAdmin")?.Value;
            bool superAdmin = bool.TryParse(superAdminString, out bool resultSuperAdmin) ? resultSuperAdmin : false;

            string username = identity.FindFirst(ClaimTypes.Name)?.Value;
            string userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (identity != null)
            {
                if (superAdmin || isTeacher)
                {
                    string msg = AttendanceServices.InsertorUpdateToDB(id, oClientRequestInfo, out Attendance attendance);
                    if (msg.Length > 0) return msg.ToMNFResultError("InsertorUpdateToDB", new { oClientRequestInfo });

                    return attendance.ToResultOk();
                }
                else
                {
                    string msg = "Bạn không có quyền truy cập";

                    return Result.GetResultError(msg);
                }
            } else
            {
                string msg = "Bạn cần đăng nhập";

                return Result.GetResultError(msg);
            }
            
        }
        #endregion

        //#region update thông tin chức vụ
        //[HttpPost]
        //[Route("edit/{id:int}")]
        //public Result ChucVuUpdate(int id, ChucVuServices.ChucVuUpdateInfo oClientRequestInfo)
        //{
        //    string msg = ChucVuServices.ChucVuUpdateToDB(id, oClientRequestInfo, out ChucVuModel chucvu);
        //    if (msg.Length > 0) return msg.ToMNFResultError("ChucVuUpdateToDB", new { id, oClientRequestInfo });

        //    return chucvu.ToResultOk();
        //}
        //#endregion

        #region xóa thông tin chức vụ
        //[Authorize]
        [HttpDelete]
        [Route("delete/{id:int)}")]
        public Result AttendanceDelete(int id)
        {
            var identity = User.Identity as ClaimsIdentity;
            string isTeacherString = identity.FindFirst("IsTeacher")?.Value;
            bool isTeacher = bool.TryParse(isTeacherString, out bool resultIsTeacher) ? resultIsTeacher : false;

            string superAdminString = identity.FindFirst("SuperAdmin")?.Value;
            bool superAdmin = bool.TryParse(superAdminString, out bool resultSuperAdmin) ? resultSuperAdmin : false;

            string username = identity.FindFirst(ClaimTypes.Name)?.Value;
            string userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (identity != null)
            {
                if (superAdmin || isTeacher)
                {
                    string msg = Attendance.GetOneAttendanceByID(id, out Attendance attendance);
                    if (msg.Length > 0) msg.ToMNFResultError("GetOneAttendanceByID", new { attendance });

                    BSS.DBM dbm = new BSS.DBM();
                    dbm.BeginTransac();

                    msg = AttendanceServices.DoDelete(dbm, id, attendance);
                    if (msg.Length > 0) { dbm.RollBackTransac(); return Log.ProcessError(msg).ToResultError(); }

                    dbm.CommitTransac();

                    return Result.GetResultOk();
                }
                else
                {
                    string msg = "Bạn không có quyền truy cập";

                    return Result.GetResultError(msg);
                }
            } else
            {
                string msg = "Bạn cần đăng nhập";

                return Result.GetResultError(msg);
            }
            #endregion
        }
    }
}