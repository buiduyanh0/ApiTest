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
    [RoutePrefix("api/chucvu")]
    public class ChucVuController : ApiController
    {
        #region lấy dữ liệu chức vụ
        [HttpGet]
        [Route("")]
        public Result GetList()
        {
            string msg = ChucVuModel.GetAllChucVu(out List<ChucVuModel> lstchucvu);
            if (msg.Length > 0) return msg.ToMNFResultError("GetAllChucVu");

            return lstchucvu.ToResultOk();
        }
        #endregion

        #region lấy dữ liệu chức vụ theo ID
        [HttpGet]
        [Route("{id:int}")]
        public Result GetOneChucVu(int id)
        {
            string msg = ApiTest2.Models.ChucVuModel.GetOneChucVuByID(id, out ChucVuModel chucvu);
            if (msg.Length > 0) return msg.ToMNFResultError("GetOneChucVuByID", new { id });

            return chucvu.ToResultOk();

        }
        #endregion

        #region add thông tin chức vụ mới
        [HttpPost]
        [Route("edit/{id:int}")]
        public Result ChucVuAddorUpdate(int id, ChucVuServices.ChucVuAddorUpdateInfo oClientRequestInfo)
        {
            string msg = ChucVuServices.InsertorUpdateToDB(id, oClientRequestInfo, out ChucVuModel chucvu);
            if (msg.Length > 0) return msg.ToMNFResultError("InsertorUpdateToDB", new { oClientRequestInfo });

            return chucvu.ToResultOk();
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
        [HttpDelete]
        [Route("delete/{id:int}")]
        public Result ChucVuDelete(int id)
        {
            string msg = ChucVuModel.GetOneChucVuByID(id, out ChucVuModel chucvu);
            if (msg.Length > 0) msg.ToMNFResultError("GetOneChucVuByID", new { id });

            BSS.DBM dbm = new BSS.DBM();
            dbm.BeginTransac();

            msg = ChucVuServices.DoDelete(dbm, id, chucvu);
            if (msg.Length > 0) { dbm.RollBackTransac(); return Log.ProcessError(msg).ToResultError(); }

            dbm.CommitTransac();

            return Result.GetResultOk();

            #endregion
        }
    }
}