﻿using ApiTest2.Models;
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
    [RoutePrefix("api/grade")]
    public class GradeController : ApiController
    {
        #region lấy dữ liệu điểm theo mã lớp
        //[Authorize]
        [HttpGet]
        [Route("{classcode}")]
        public Result GetListByClassCode(string classcode)
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
                    string msg = Grade.GetAllGradeByClassCode(classcode, out List<Grade> lstgrade);
                    if (msg.Length > 0) return msg.ToMNFResultError("GetAllGradeByStudentCode");

                    return lstgrade.ToResultOk();
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

        #region lấy dữ liệu điểm theo mssv
        //[Authorize]
        [HttpGet]
        [Route("{studentcode}")]
        public Result GetListGradeByStudentCode(string studentcode)
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
                string msg = Grade.GetAllGradeByStudentCode(studentcode, out List<Grade> lstgrade);
                if (msg.Length > 0) return msg.ToMNFResultError("GetAllGradeByStudentCode");

                return lstgrade.ToResultOk();
            }
            else
            {
                string msg = "Bạn cần đăng nhập!";

                return Result.GetResultError(msg);
            }
        }
        #endregion

        #region lấy dữ liệu điểm của môn học theo MSSV
        //[Authorize]
        [HttpGet]
        [Route("{classcode}/{studentcode}")]
        public Result GetOneGradeByStudentCode(string classcode, string studentcode)
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
                string msg = ApiTest2.Models.Grade.GetOneGradeByCode(classcode, studentcode, out Grade grade);
                if (msg.Length > 0) msg.ToMNFResultError("GetOneGradeByCode", new { classcode, studentcode });

                return grade.ToResultOk();
            }
            else
            {
                string msg = "Bạn cần đăng nhập!";

                return Result.GetResultError(msg);
            }
        }
        #endregion

        #region add thông tin điểm mới
        //[Authorize]
        [HttpPost]
        [Route("edit/{id:int}")]
        public Result GradeAddorUpdate(int id, GradeServices.GradeAddorUpdateInfo oClientRequestInfo)
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
                    string msg = GradeServices.InsertorUpdateToDB(id, oClientRequestInfo, out Grade grade);
                    if (msg.Length > 0) return msg.ToMNFResultError("InsertorUpdateToDB", new { oClientRequestInfo });

                    return grade.ToResultOk();
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
        //[Authorize]
        [HttpDelete]
        [Route("delete/{classcode}/{studentcode}")]
        public Result GradeDelete(string studentcode, string classcode)
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
                    string msg = Grade.GetOneGradeByCode(studentcode, classcode, out Grade grade);
                    if (msg.Length > 0) msg.ToMNFResultError("GetOnePhongBanByID", new { studentcode, classcode });

                    BSS.DBM dbm = new BSS.DBM();
                    dbm.BeginTransac();

                    msg = GradeServices.DoDelete(dbm, studentcode, classcode, grade);
                    if (msg.Length > 0) { dbm.RollBackTransac(); return Log.ProcessError(msg).ToResultError(); }

                    dbm.CommitTransac();

                    return Result.GetResultOk();
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
    }
}

