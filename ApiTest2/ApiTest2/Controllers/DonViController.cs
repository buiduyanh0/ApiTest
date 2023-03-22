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
    [RoutePrefix("api/donvi")]
    public class DonViController : ApiController
    {
        // GET: DonVi
        #region lấy dữ liệu đơn vị
        [HttpGet]
        [Route("")]
        public Result GetList()
        {
            string msg = DonVi.GetAllDonVi(out List<DonVi> lstdonvi);
            if (msg.Length > 0) return msg.ToMNFResultError("GetAllDonVi");

            return lstdonvi.ToResultOk();
        }
        #endregion

        #region lấy dữ liệu đơn vị theo ID
        [HttpGet]
        [Route("{id:int}")]
        public Result GetOneDonVi(int id)
        {
            string msg = ApiTest2.Models.DonVi.GetOneDonViByID(id, out DonVi donvi);
            if (msg.Length > 0) return msg.ToMNFResultError("GetOneDonViByID", new { id });

            return donvi.ToResultOk();

        }
        #endregion

        #region add thông tin đơn vị mới
        [HttpPost]
        [Route("edit/{id:int}")]
        public Result DonViAddorUpdate(int id, DonViServices.DonViAddorUpdateInfo oClientRequestInfo)
        {
            string msg = DonViServices.InsertorUpdateToDB(id, oClientRequestInfo, out DonVi donvi);
            if (msg.Length > 0) return msg.ToMNFResultError("InserorUpdatetToDB", new { oClientRequestInfo });

            return donvi.ToResultOk();
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

        #region xóa thông tin đơn vị
        [HttpDelete]
        [Route("delete/{id:int}")]
        public Result DonViDelete(int id)
        {
            string msg = DonVi.GetOneDonViByID(id, out DonVi donvi);
            if (msg.Length > 0) msg.ToMNFResultError("GetOneDonViByID", new { id });

            BSS.DBM dbm = new BSS.DBM();
            dbm.BeginTransac();

            msg = DonViServices.DoDelete(dbm, id, donvi);
            if (msg.Length > 0) { dbm.RollBackTransac(); return Log.ProcessError(msg).ToResultError(); }

            dbm.CommitTransac();

            return Result.GetResultOk();

            #endregion
        }
    }
}