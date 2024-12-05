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
using System.Web.Mvc;

namespace ApiTest2.Controllers
{
    [RoutePrefix("api/classregistration")]
    public class ClassRegistrationController : Controller
    {
        // GET: ClassRegistration
        #region lấy dữ liệu đăng ký môn học
        [Authorize]
        [HttpGet]
        [Route("")]
        public Result GetList()
        {
            var identity = User.Identity as ClaimsIdentity;
            byte isTeacher = Convert.ToByte(identity.FindFirst("IsTeacher")?.Value); // Convert back to byte
            byte superAdmin = Convert.ToByte(identity.FindFirst("SuperAdmin")?.Value); // Convert back to byte

            if (identity != null)
            {
                if (superAdmin == 1 || isTeacher == 1)
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

        #region lấy dữ liệu các sinh viên đã đăng ký môn học
        [Authorize]
        [HttpGet]
        [Route("")]
        public Result GetListClassStudent(string classcode)
        {
            var identity = User.Identity as ClaimsIdentity;
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

        #region lấy dữ liệu môn học của sinh viên
        [Authorize]
        [HttpGet]
        [Route("{studentcode:string}")]
        public Result GetOneClassRegistration(string studentcode, string classcode)
        {
            var identity = User.Identity as ClaimsIdentity;
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
        [Authorize]
        [HttpPost]
        [Route("edit/{id:int}")]
        public Result ClassRegistrationAddorUpdate(int id, ClassRegistrationServices.ClassRegistrationAddorUpdateInfo oClientRequestInfo)
        {
            var identity = User.Identity as ClaimsIdentity;
            byte isTeacher = Convert.ToByte(identity.FindFirst("IsTeacher")?.Value); // Convert back to byte
            byte superAdmin = Convert.ToByte(identity.FindFirst("SuperAdmin")?.Value); // Convert back to byte
            if (identity != null)
            {
                if (superAdmin != 1 && isTeacher != 1)
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
        [Authorize]
        [HttpDelete]
        [Route("delete/{studentcode:string}")]
        public Result ClassRegistrationtDelete(string studentcode, string classcode)
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                string msg = ClassRegistration.GetOneRegistrationByStudentCode(studentcode, classcode, out ClassRegistration classRegistration);
                if (msg.Length > 0) msg.ToMNFResultError("GetOneClassRegistrationByCode", new { studentcode, classcode });

                BSS.DBM dbm = new BSS.DBM();
                dbm.BeginTransac();

                msg = ClassRegistrationServices.DoDelete(dbm, studentcode, classcode, classRegistration);
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