using ApiTest2.Models;
using ApiTest2.Services;
using BSS;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace ApiTest2.Controllers
{
    [RoutePrefix("api/class")]
    public class ClassController : ApiController
    {
        #region lấy dữ liệu lớp học
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
                    string msg = Class.GetAllClass(out List<Class> lstclass);
                    if (msg.Length > 0) return msg.ToMNFResultError("GetAllClass");

                    return lstclass.ToResultOk();
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

        #region lấy dữ liệu lớp học theo ID
        [HttpGet]
        [Route("{classcode:length(6)}")]
        public Result GetOneClass(string classcode)
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
                string msg = ApiTest2.Models.Class.GetOneClassByClassCode(classcode, out Class classs);
                if (msg.Length > 0) return msg.ToMNFResultError("GetOneClassByClassCode", new { classcode });

                return classs.ToResultOk();
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
        public Result ClassAddorUpdate(int id, ClassServices.ClassAddorUpdateInfo oClientRequestInfo)
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
                if (superAdmin)
                {
                    string msg = ClassServices.InsertorUpdateToDB(id, oClientRequestInfo, out Class classs);
                    if (msg.Length > 0) return msg.ToMNFResultError("InsertorUpdateToDB", new { oClientRequestInfo });

                    return classs.ToResultOk();
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
        [Route("delete/{classcode:length(6)}")]
        public Result ClassDelete(string classcode)
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
                    string msg = Class.GetOneClassByClassCode(classcode, out Class classs);
                    if (msg.Length > 0) msg.ToMNFResultError("GetOneClassByClassCode", new { classcode });

                    BSS.DBM dbm = new BSS.DBM();
                    dbm.BeginTransac();

                    msg = ClassServices.DoDelete(dbm, classcode, classs);
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