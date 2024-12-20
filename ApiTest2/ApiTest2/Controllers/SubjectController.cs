﻿using ApiTest2.Models;
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
    [RoutePrefix("api/supject")]
    public class SubjectController : ApiController
    {
        // GET: Subject
        #region lấy dữ liệu môn học
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
                if (superAdmin)
                {
                    string msg = Subject.GetAllSubject(out List<Subject> lstsubject);
                    if (msg.Length > 0) return msg.ToMNFResultError("GetAllSubject");

                    return lstsubject.ToResultOk();
                }
                else
                {
                    string msg = "Bạn không có quyền truy cập";

                    return Result.GetResultError(msg);
                }
            }
            else
            {
                string msg = "Bạn cần đăng nhập";

                return Result.GetResultError(msg);
            }

        }
        #endregion

        #region lấy dữ liệu học phần theo ID
        //[Authorize]
        [HttpGet]
        [Route("{subjectcode}")]
        public Result GetOneSubject(string subjectcode)
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
                string msg = ApiTest2.Models.Subject.GetOneSubjectByCode(subjectcode, out Subject supject);
                if (msg.Length > 0) return msg.ToMNFResultError("GetOneSubjectByCode", new { subjectcode });

                return supject.ToResultOk();
            }
            else
            {
                string msg = "Bạn cần đăng nhập";

                return Result.GetResultError(msg);
            }
        }
        #endregion

        #region add thông tin môn học mới
        //[Authorize]
        [HttpPost]
        [Route("edit/{id:int}")]
        public Result SupjectAddorUpdate(int id, SubjectServices.SubjectAddorUpdateInfo oClientRequestInfo)
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
                    string msg = SubjectServices.InsertorUpdateToDB(id, oClientRequestInfo, out Subject subject);
                    if (msg.Length > 0) return msg.ToMNFResultError("InserorUpdatetToDB", new { oClientRequestInfo });

                    return subject.ToResultOk();
                }
                else
                {
                    string msg = "Bạn không có quyền truy cập";

                    return Result.GetResultError(msg);
                }
            }
            else
            {
                string msg = "Bạn cần đăng nhập";

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

        #region xóa thông tin môn học
        //[Authorize]
        [HttpDelete]
        [Route("delete/{supjectcode}")]
        public Result SubjectDelete(string subjectcode)
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
                    string msg = Subject.GetOneSubjectByCode(subjectcode, out Subject subject);
                    if (msg.Length > 0) msg.ToMNFResultError("GetOneSubjectByCode", new { subjectcode });

                    BSS.DBM dbm = new BSS.DBM();
                    dbm.BeginTransac();

                    msg = SubjectServices.DoDelete(dbm, subjectcode, subject);
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
                string msg = "Bạn cần đăng nhập";

                return Result.GetResultError(msg);
            }
            #endregion
        }
    }
}