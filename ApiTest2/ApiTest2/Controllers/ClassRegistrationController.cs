using ApiTest2.Models;
using ApiTest2.Services;
using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ApiTest2.Controllers
{
    [RoutePrefix("api/classregistration")]
    public class ClassRegistrationController : Controller
    {
        // GET: ClassRegistration
        #region lấy dữ liệu môn học
        [Authorize]
        [HttpGet]
        [Route("")]
        public Result GetList()
        {
            string msg = ClassRegistration.GetAllClassRegistration(out List<ClassRegistration> lstregistration);
            if (msg.Length > 0) return msg.ToMNFResultError("GetAllClassRegistration");

            return lstregistration.ToResultOk();
        }
        #endregion

        #region lấy dữ liệu môn học theo ID
        [Authorize]
        [HttpGet]
        [Route("{studentcode:string}")]
        public Result GetOneSubject(string studentcode, string classcode)
        {
            string msg = ApiTest2.Models.ClassRegistration.GetOneRegistrationByStudentCode(studentcode, classcode, out ClassRegistration classRegistation);
            if (msg.Length > 0) return msg.ToMNFResultError("GetOneRegistrationByStudentCode", new { studentcode, classcode });

            return classRegistation.ToResultOk();
        }
        #endregion

        #region add thông tin môn học mới
        [Authorize]
        [HttpPost]
        [Route("edit/{id:int}")]
        public Result ClassRegistrationAddorUpdate(int id, ClassRegistrationServices.ClassRegistrationAddorUpdateInfo oClientRequestInfo)
        {
            string msg = ClassRegistrationServices.InsertorUpdateToDB(id, oClientRequestInfo, out ClassRegistration classRegistration);
            if (msg.Length > 0) return msg.ToMNFResultError("InserorUpdatetToDB", new { oClientRequestInfo });

            return classRegistration.ToResultOk();
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

        #region xóa thông tin môn học
        [Authorize]
        [HttpDelete]
        [Route("delete/{studentcode:string}")]
        public Result SubjectDelete(string studentcode, string classcode)
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
        #endregion
    }
}