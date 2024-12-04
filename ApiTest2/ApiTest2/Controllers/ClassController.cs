using ApiTest2.Models;
using ApiTest2.Services;
using BSS;
using DocumentFormat.OpenXml.Office2010.Excel;
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
        #region lấy dữ liệu chức vụ
        [Authorize]
        [HttpGet]
        [Route("")]
        public Result GetList()
        {
            string msg = Class.GetAllClass(out List<Class> lstclass);
            if (msg.Length > 0) return msg.ToMNFResultError("GetAllClass");

            return lstclass.ToResultOk();
        }
        #endregion

        #region lấy dữ liệu chức vụ theo ID
        [HttpGet]
        [Route("{id:int}")]
        public Result GetOneClass(string classcode)
        {
            string msg = ApiTest2.Models.Class.GetOneClassByClassCode(classcode, out Class classs);
            if (msg.Length > 0) return msg.ToMNFResultError("GetOneChucVuByID", new { classcode });

            return classs.ToResultOk();

        }
        #endregion

        #region add thông tin chức vụ mới
        [Authorize]
        [HttpPost]
        [Route("edit/{id:int}")]
        public Result ClassAddorUpdate(int id, ClassServices.ClassAddorUpdateInfo oClientRequestInfo)
        {
            var identity = User.Identity as ClaimsIdentity;
            byte isTeacher = Convert.ToByte(identity.FindFirst("IsTeacher")?.Value); // Convert back to byte
            byte superAdmin = Convert.ToByte(identity.FindFirst("SuperAdmin")?.Value); // Convert back to byte

            if (superAdmin == 1 || isTeacher == 1)
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
        [Authorize]
        [HttpDelete]
        [Route("delete/{id:int}")]
        public Result ChucVuDelete(int id)
        {
            var identity = User.Identity as ClaimsIdentity;
            byte isTeacher = Convert.ToByte(identity.FindFirst("IsTeacher")?.Value); // Convert back to byte
            byte superAdmin = Convert.ToByte(identity.FindFirst("SuperAdmin")?.Value); // Convert back to byte

            if (superAdmin == 1 || isTeacher == 1)
            {
                string msg = Class.GetOneClassByID(id, out Class classs);
                if (msg.Length > 0) msg.ToMNFResultError("GetOneChucVuByID", new { id });

                BSS.DBM dbm = new BSS.DBM();
                dbm.BeginTransac();

                msg = ClassServices.DoDelete(dbm, id, classs);
                if (msg.Length > 0) { dbm.RollBackTransac(); return Log.ProcessError(msg).ToResultError(); }

                dbm.CommitTransac();

                return Result.GetResultOk();
            }
            else
            {
                string msg = "Bạn không có quyền truy cập";

                return Result.GetResultError(msg);
            }
            #endregion
        }
    }
}