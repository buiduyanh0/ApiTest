using ApiTest2.Models;
using ApiTest2.Services;
using BSS;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Http;

namespace ApiTest2.Controllers
{
    [RoutePrefix("api/classregistration")]
    public class ClassRegistrationController : ApiController
    {
        // GET: ClassRegistration
        #region lấy dữ liệu đăng ký môn học
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
                    string msg = ClassRegistration.GetAllClassRegistration(out List<ClassRegistration> lstregistration);
                    if (msg.Length > 0) return msg.ToMNFResultError("GetAllClassRegistration");

                    return lstregistration.ToResultOk();
                }
                else
                {
                    string msg = "Bạn không có quyền truy cập";

                    return Result.GetResultError(msg);
                }
            }
            else
            {
                string msg = "Bạn cần đăng nhập!";

                return Result.GetResultError(msg);
            }
        }
        #endregion

        #region lấy dữ liệu các sinh viên đã đăng ký lớp học
        //[Authorize]
        [HttpGet]
        [Route("{classcode}")]
        public Result GetListClassStudent(string classcode)
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
                string msg = ClassRegistration.GetAllClassRegistrationByClassCode(classcode, out List<ClassRegistration> lstregistration);
                if (msg.Length > 0) return msg.ToMNFResultError("GetAllClassRegistrationByClassCode");

                return lstregistration.ToResultOk();
            }
            else
            {
                string msg = "Bạn cần đăng nhập!";

                return Result.GetResultError(msg);
            }
        }
        #endregion

        #region lấy dữ liệu các lớp học sinh viên đã đăng ký
        //[Authorize]
        [HttpGet]
        [Route("{studentcode}")]
        public Result GetListClassRegistedByStudent(string studentcode)
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
                string msg = ClassRegistration.GetAllStudentRegistedClass(studentcode, out List<ClassRegistration> lstregistration);
                if (msg.Length > 0) return msg.ToMNFResultError("GetAllClassRegistrationByClassCode");

                return lstregistration.ToResultOk();
            }
            else
            {
                string msg = "Bạn cần đăng nhập!";

                return Result.GetResultError(msg);
            }
        }
        #endregion

        #region lấy dữ liệu lớp học của sinh viên đã đăng ký
        //[Authorize]
        [HttpGet]
        [Route("{classcode}/{studentcode}")]
        public Result GetOneClassRegistration(string classcode, string studentcode)
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
                string msg = ApiTest2.Models.ClassRegistration.GetOneRegistrationByStudentCode(studentcode, classcode, out ClassRegistration classRegistation);
                if (msg.Length > 0) return msg.ToMNFResultError("GetOneRegistrationByStudentCode", new { studentcode, classcode });

                return classRegistation.ToResultOk();
            }
            else
            {
                string msg = "Bạn cần đăng nhập!";

                return Result.GetResultError(msg);
            }
        }
        #endregion

        #region add thông tin đăng ký môn học
        //[Authorize]
        [HttpPost]
        [Route("edit/{id:int}")]
        public Result ClassRegistrationAddorUpdate(int id, ClassRegistrationServices.ClassRegistrationAddorUpdateInfo oClientRequestInfo)
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
                if (!superAdmin && !isTeacher)
                {
                    string msg = ClassRegistrationServices.InsertorUpdateToDB(id, oClientRequestInfo, out ClassRegistration classRegistration);
                    if (msg.Length > 0) return msg.ToMNFResultError("InserorUpdatetToDB", new { oClientRequestInfo });

                    return classRegistration.ToResultOk();
                }
                else
                {
                    string msg = "Chỉ có sinh viên mới được sử dụng chức năng này";

                    return Result.GetResultError(msg);
                }
            }
            else
            {
                string msg = "Bạn cần đăng nhập!";

                return Result.GetResultError(msg);
            }
        }
        #endregion

        //#region update thông tin đơn vị
        //[HttpPost]
        //[Route("edit/{id:int}")]
        //public Result ChucVuUpdate(int id, DonViServices.DonViUpdateInfo oClientRequestInfo)
        //{
        //    string msg = DonViServices.DonViUpdateToDB(id, oClientRequestInfo, out DonVi donvi);
        //    if (msg.Length > 0) return msg.ToMNFResultError("ChucVuUpdateToDB", new { id, oClientRequestInfo });

        //    return donvi.ToResultOk();
        //}
        //#endregion

        #region xóa thông tin đăng ký
        //[Authorize]
        [HttpDelete]
        [Route("delete/{classcode}/{studentcode}")]
        public Result ClassRegistrationtDelete(string classcode, string studentcode)
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
                string msg = ClassRegistration.GetOneRegistrationByStudentCode(classcode, studentcode, out ClassRegistration classRegistration);
                if (msg.Length > 0) msg.ToMNFResultError("GetOneClassRegistrationByCode", new { studentcode, classcode });

                BSS.DBM dbm = new BSS.DBM();
                dbm.BeginTransac();

                msg = ClassRegistrationServices.DoDelete(dbm, classcode, studentcode, classRegistration);
                if (msg.Length > 0) { dbm.RollBackTransac(); return Log.ProcessError(msg).ToResultError(); }

                dbm.CommitTransac();

                return Result.GetResultOk();
            }
            else
            {
                string msg = "Bạn cần đăng nhập!";

                return Result.GetResultError(msg);
            }
        }
        #endregion
    }
}