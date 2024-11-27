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
using System.Web.Http;

namespace ApiTest2.Controllers
{
    [RoutePrefix("api/phongban")]
    public class PhongBanController : ApiController
    {
        #region lấy dữ liệu phong ban
        [HttpGet]
        [Route("")]
        public Result GetList()
        {
            string msg = Class.GetAllPhongBan(out List<Class> lstphongban);
            if (msg.Length > 0) return msg.ToMNFResultError("GetAllPhongBan");

            return lstphongban.ToResultOk();
        }
        #endregion

        #region lấy dữ liệu phong ban theo ID
        [HttpGet]
        [Route("{id:int}")]
        public Result GetOnePhongBan(int id)
        {
            string msg = ApiTest2.Models.Class.GetOnePhongBanByID(id, out Class phongban);
            if (msg.Length > 0) msg.ToMNFResultError("GetOnePhongBanByID", new { id });

            return phongban.ToResultOk();

        }
        #endregion

        #region add thông tin phong ban mới
        [HttpPost]
        [Route("edit/{id:int}")]
        public Result PhongBanAddorUpdate(int id, PhongBanServices.PhongBanAddorUpdateInfo oClientRequestInfo)
        {
            string msg = PhongBanServices.InsertorUpdateToDB(id, oClientRequestInfo, out Class phongban);
            if (msg.Length > 0) return msg.ToMNFResultError("InsertToDB", new { oClientRequestInfo });

            return phongban.ToResultOk();
        }
        #endregion

        //#region update thông tin phong ban
        //[HttpPost]
        //[Route("edit/{id:int}")]
        //public Result PhongBanUpdate(int id, PhongBanServices.PhongBanUpdateInfo oClientRequestInfo)
        //{
        //    string msg = PhongBanServices.PhongBanUpdateToDB(id, oClientRequestInfo, out PhongBan phongban);
        //    if (msg.Length > 0) return msg.ToMNFResultError("PhongBanUpdateToDB", new { id, oClientRequestInfo });

        //    return phongban.ToResultOk();
        //}
        //#endregion

        #region xóa thông tin phong ban
        [HttpDelete]
        [Route("delete/{id:int}")]
        public Result PhongBanDelete(int id)
        {
            string msg = Class.GetOnePhongBanByID(id, out Class phongban);
            if (msg.Length > 0) msg.ToMNFResultError("GetOnePhongBanByID", new { id });

            BSS.DBM dbm = new BSS.DBM();
            dbm.BeginTransac();

            msg = PhongBanServices.DoDelete(dbm, id, phongban);
            if (msg.Length > 0) { dbm.RollBackTransac(); return Log.ProcessError(msg).ToResultError(); }

            dbm.CommitTransac();

            return Result.GetResultOk();

            #endregion
        }
    }
}

