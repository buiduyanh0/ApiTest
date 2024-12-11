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
using System.Security.Principal;
using System.Web.Http;
using System.Windows.Interop;

namespace ApiTest2.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        #region login
        [HttpGet]
        [Route("login")]
        public Result Login(UserServices.UserLoginInfo oClientRequestInfo)
        {
            string msg = UserServices.CheckLogin(oClientRequestInfo, out string token);
            if (msg.Length > 0) return msg.ToResultError();

            return Result.GetResult(200, token);
        }
        #endregion
        #region lấy dữ liệu người dùng
        //[Authorize]
        [HttpGet]
        [Route("")]
        public Result GetList()
        {
            var identity = User.Identity as ClaimsIdentity;
            string username = identity.FindFirst(ClaimTypes.Name)?.Value;
            byte isTeacher = Convert.ToByte(identity.FindFirst("IsTeacher")?.Value); // Convert back to byte
            byte superAdmin = Convert.ToByte(identity.FindFirst("SuperAdmin")?.Value); // Convert back to byte
            if (identity != null)
            {
                if (superAdmin == 1 || isTeacher == 1)
                {
                    string msg = ApiTest2.Models.User.GetAllUser(out List<User> lstuser);
                    if (msg.Length > 0) return msg.ToMNFResultError("GetAllUser");

                    return lstuser.ToResultOk();
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

        #region lấy dữ liệu người dùng theo ID
        //[Authorize]
        [HttpGet]
        [Route("{username:length(8)}")]
        public Result GetOneUser(string username)
        {
            var identity = User.Identity as ClaimsIdentity;
            username = identity.FindFirst(ClaimTypes.Name)?.Value;
            byte isTeacher = Convert.ToByte(identity.FindFirst("IsTeacher")?.Value); // Convert back to byte
            byte superAdmin = Convert.ToByte(identity.FindFirst("SuperAdmin")?.Value); // Convert back to byte

            if (superAdmin == 1 || isTeacher == 1)
            {
                string msg = ApiTest2.Models.User.GetOneUserByUserName(username, out User user);
                if (msg.Length > 0) return msg.ToMNFResultError("GetOneUserByUserName", new { username });

                return user.ToResultOk();
            }
            else
            {
                string msg = "Bạn không có quyền truy cập";

                return Result.GetResultError(msg);
            }
        }
        #endregion

        #region add thông tin người dùng mới
        //[Authorize]
        [HttpPost]
        [Route("edit/{id:int}")]
        public Result UserAddorUpdate(int id, UserServices.UserAddorUpdateInfo oClientRequestInfo)
        {
            var identity = User.Identity as ClaimsIdentity;
            byte isTeacher = Convert.ToByte(identity.FindFirst("IsTeacher")?.Value); // Convert back to byte
            byte superAdmin = Convert.ToByte(identity.FindFirst("SuperAdmin")?.Value); // Convert back to byte

            if (identity != null)
            {
                if (superAdmin == 1)
                {
                    string msg = UserServices.InsertorUpdateToDB(id, oClientRequestInfo, out User user);
                    if (msg.Length > 0) return msg.ToMNFResultError("InsertorUpdateToDB", new { oClientRequestInfo });

                    return user.ToResultOk();
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

        //#region update thông tin người dùng
        //[HttpPost]
        //[Route("edit/{id:int}")]
        //public Result UserUpdate(int id, UserServices.UserUpdateInfo oClientRequestInfo)
        //{
        //    string msg = UserServices.UserUpdateToDB(id, oClientRequestInfo, out User user);
        //    if (msg.Length > 0) return msg.ToMNFResultError("UserUpdateToDB", new { oClientRequestInfo, id });

        //    return user.ToResultOk();
        //}
        //#endregion

        #region xóa thông tin người dùng
        //[Authorize]
        [HttpDelete]
        [Route("delete/{id:int}")]
        public Result UserDelete(int id)
        {
            var identity = User.Identity as ClaimsIdentity;
            byte isTeacher = Convert.ToByte(identity.FindFirst("IsTeacher")?.Value); // Convert back to byte
            byte superAdmin = Convert.ToByte(identity.FindFirst("SuperAdmin")?.Value); // Convert back to byte

            if (identity != null)
            {
                if (superAdmin == 1)
                {
                    string msg = ApiTest2.Models.User.GetOneUserByID(id, out User user);
                    if (msg.Length > 0) return msg.ToMNFResultError("GetOneUserByID", new { id });

                    BSS.DBM dbm = new BSS.DBM();
                    dbm.BeginTransac();

                    msg = UserServices.DoDelete(dbm, id, user);
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
        }
        #endregion
    }
}
