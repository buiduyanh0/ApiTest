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
    [RoutePrefix("api/grade")]
    public class GradeController : ApiController
    {
        #region lấy dữ liệu điểm theo mã lớp
        [HttpGet]
        [Route("{classcode:string}")]
        public Result GetListByClassCode(string classcode)
        {
            string msg = Grade.GetAllGradeByStudentCode(classcode, out List<Grade> lstgrade);
            if (msg.Length > 0) return msg.ToMNFResultError("GetAllGradeByStudentCode");

            return lstgrade.ToResultOk();
        }
        #endregion

        #region lấy dữ liệu điểm theo mssv
        [HttpGet]
        [Route("{studentcode:string}")]
        public Result GetListByStudentCode(string studentcode)
        {
            string msg = Grade.GetAllGradeByStudentCode(studentcode, out List<Grade> lstgrade);
            if (msg.Length > 0) return msg.ToMNFResultError("GetAllGradeByStudentCode");

            return lstgrade.ToResultOk();
        }
        #endregion

        #region lấy dữ liệu điểm theo sinh viên
        [HttpGet]
        [Route("{studentcode:string}")]
        public Result GetOnePhongBan(string classcode, string studentcode)
        {
            string msg = ApiTest2.Models.Grade.GetOneGradeByCode(classcode, studentcode, out Grade grade);
            if (msg.Length > 0) msg.ToMNFResultError("GetOneGradeByCode", new { classcode, studentcode });

            return grade.ToResultOk();

        }
        #endregion

        #region add thông tin điểm mới
        [HttpPost]
        [Route("edit/{studentcode:string}")]
        public Result PhongBanAddorUpdate(int id, GradeServices.GradeAddorUpdateInfo oClientRequestInfo)
        {
            string msg = GradeServices.InsertorUpdateToDB(id, oClientRequestInfo, out Grade grade);
            if (msg.Length > 0) return msg.ToMNFResultError("InsertToDB", new { oClientRequestInfo });

            return grade.ToResultOk();
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

        #region xóa thông tin điểm
        [HttpDelete]
        [Route("delete/{studentcode:string}")]
        public Result GradeDelete(string studentcode, string classcode)
        {
            string msg = Grade.GetOneGradeByCode(studentcode, classcode, out Grade grade);
            if (msg.Length > 0) msg.ToMNFResultError("GetOnePhongBanByID", new { studentcode, classcode });

            BSS.DBM dbm = new BSS.DBM();
            dbm.BeginTransac();

            msg = GradeServices.DoDelete(dbm, studentcode, classcode, grade);
            if (msg.Length > 0) { dbm.RollBackTransac(); return Log.ProcessError(msg).ToResultError(); }

            dbm.CommitTransac();

            return Result.GetResultOk();
        }
        #endregion
    }
}

