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
        [HttpGet]
        [Route("")]
        public Result GetList()
        {
            string msg = ApiTest2.Models.User.GetAllUser(out List<User> lstuser);
            if (msg.Length > 0) return msg.ToMNFResultError("GetAllUser");

            return lstuser.ToResultOk();
        }
        #endregion

        #region lấy dữ liệu người dùng theo ID
        [HttpGet]
        [Route("{id:int}")]
        public Result GetOneUser(int id)
        {
            string msg = ApiTest2.Models.User.GetOneUserByID(id, out User user);
            if (msg.Length > 0) return msg.ToMNFResultError("GetOneUserByID", new { id });

            return user.ToResultOk();
        }
        #endregion

        #region add thông tin người dùng mới

        [HttpPost]
        [Route("edit/{id:int}")]
        public Result UserAddorUpdate(int id, UserServices.UserAddorUpdateInfo oClientRequestInfo)
        {
            string msg = UserServices.InsertorUpdateToDB(id, oClientRequestInfo, out User user);
            if (msg.Length > 0) return msg.ToMNFResultError("InsertorUpdateToDB", new { oClientRequestInfo });

            return user.ToResultOk();
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
        [HttpDelete]
        [Route("delete/{id:int}")]
        public Result UserDelete(int id)
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
        #endregion
    }
}
