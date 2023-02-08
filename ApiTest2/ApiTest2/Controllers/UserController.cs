using ApiTest2.Models;
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
        #region lấy dữ liệu người dùng
        [HttpGet]
        [Route("")]
        public Result GetAll()
        {
            string msg = "";
            User user = new User();
            using (BSS.DBM dbm = new BSS.DBM())
            {
                User ouser = new User();
                try
                {
                    msg = DBM.ExecStore("usp_User_GetAll");
                    if (msg.Length > 0)
                    {
                        return BSS.Result.GetResultError("Lỗi khi ghi lại nhật ký cấu hình: " + msg);
                    }
                    return msg.ToResultOk();
                }
                catch (Exception ex)
                {
                    msg = "Lỗi ngoại lệ khi lấy thông tin người dùng " + ex.Message;
                    return BSS.Result.GetResultError(msg);
                }
            }
        }
        #endregion

        #region lấy dữ liệu người dùng theo ID
        public class GetUserFromCli
        {
            public int UserID { get; set; }
        }
        [HttpGet]
        [Route("{id:int}")]
        public Result GetOneUser(int id)
        {
            {
                string msg = "";
                User user = new User();
                using (BSS.DBM dbm = new BSS.DBM())
                {
                    try
                    {
                        msg = ApiTest2.Models.User.GetOne(id, out user);
                        if (msg.Length > 0)
                        {
                            return BSS.Result.GetResultError("Lỗi khi ghi lại nhật ký cấu hình: " + msg);
                        }
                        return BSS.Result.GetResultOk(user); ;
                    }
                    catch (Exception ex)
                    {
                        msg = "Lỗi ngoại lệ khi lấy thông tin người dùng " + ex.Message;
                        return BSS.Result.GetResultError(msg);
                    }
                }
            }
        }
        #endregion

        #region add thông tin người dùng mới
        public class UserAddInfo
        {
            public string UserName { get; set; }
            public string Name { get; set; }
            public string Sex { get; set; }
            public int IDPhongban { get; set; }
            public int IDChucVu { get; set; }
            public DateTime Birthday { get; set; }
            public string Email { get; set; }
            public string NumberPhone { get; set; }
        }
        [HttpPost]
        [Route("add")]
        public Result UserAdd(UserAddInfo oClientRequestInfo)
        {
            string msg = "";
            User user = new User();
            using (BSS.DBM dbm = new BSS.DBM())
            {
                try
                {
                    user.UserName = oClientRequestInfo.UserName;
                    user.Name = oClientRequestInfo.Name;
                    user.GioiTinh = oClientRequestInfo.Sex;
                    user.IDPhongban = oClientRequestInfo.IDPhongban;
                    user.IDChucVu = oClientRequestInfo.IDChucVu;
                    user.Birthday = oClientRequestInfo.Birthday;
                    user.Email = oClientRequestInfo.Email;
                    user.NumberPhone = oClientRequestInfo.NumberPhone;

                    if (user.Name == "")
                    {
                        return BSS.Result.GetResultError("Chưa điền tên " + msg);
                    }
                    if (user.IDPhongban == null)
                    {
                        return BSS.Result.GetResultError("Chưa điền chức vụ " + msg);
                    }
                    msg = user.Insert(dbm);
                    if (msg.Length > 0)
                    {
                        return BSS.Result.GetResultError("Lỗi khi ghi lại nhật ký cấu hình: " + msg);
                    }
                    else
                    {
                        return BSS.Result.GetResultOk("đã thành công thêm mới người dùng");
                    }
                }
                catch (Exception ex)
                {
                    msg = "Lỗi ngoại lệ khi sửa mẫu cảnh báo " + ex.Message;
                    return BSS.Result.GetResultError(msg);
                }

            }
        }
        #endregion

        #region update thông tin người dùng
        public class UserUpdateInfo
        {
            public int UserID { get; set; }
            public string UserName { get; set; }
            public string Name { get; set; }
            public string Sex { get; set; }
            public int IDPhongban { get; set; }
            public int IDChucVu { get; set; }
            public DateTime Birthday { get; set; }
            public string Email { get; set; }
            public string NumberPhone { get; set; }
            public Guid objectGUID { get; set; }
        }
        [HttpPost]
        [Route("edit/{id:int}")]
        public Result UserUpdate(int id, UserUpdateInfo oClientRequestInfo)
        {
            string msg = "";
            User user = new User();
            using (BSS.DBM dbm = new BSS.DBM())
            {
                string msgCompare = "";
                User userOld = new User();
                string processContent = "";
                try
                {
                    msgCompare = ApiTest2.Models.User.GetOne(id, out userOld);
                    if (msg.Length > 0)
                    {
                        return BSS.Result.GetResultError("Lỗi khi lấy thông tin người dùng" + msg);
                    }

                    user.UserID = id;
                    user.UserName = oClientRequestInfo.UserName;
                    user.Name = oClientRequestInfo.Name;
                    user.GioiTinh = oClientRequestInfo.Sex;
                    user.IDPhongban = oClientRequestInfo.IDPhongban;
                    user.IDChucVu = oClientRequestInfo.IDChucVu;
                    user.Birthday = oClientRequestInfo.Birthday;
                    user.Email = oClientRequestInfo.Email;
                    user.NumberPhone = oClientRequestInfo.NumberPhone;

                    if (user.Name == "")
                    {
                        return BSS.Result.GetResultError("Chưa điền tên " + msg);
                    }
                    if (user.IDPhongban == null)
                    {
                        return BSS.Result.GetResultError("Chưa điền chức vụ " + msg);
                    }
                    msg = user.Update(dbm);
                    if (msg.Length > 0)
                    {
                        return BSS.Result.GetResultError("Lỗi khi ghi lại nhật ký cấu hình: " + msg);
                    }

                    processContent = BSS.DifferenceComparator.GetDifferences(userOld, user);

                    msg = Log.WriteHistoryLog(dbm, processContent, oClientRequestInfo.objectGUID, 0, "", 0);
                    if (msg.Length > 0)
                    {
                        return BSS.Result.GetResultError(msg);
                    }
                    else
                    {
                        return BSS.Result.GetResultOk("đã sửa thành công thông tin người dùng");
                    }
                }
                catch (Exception ex)
                {
                    msg = "Lỗi ngoại lệ khi sửa thông tin người dùng " + ex.Message;
                    return BSS.Result.GetResultError(msg);
                }

            }
        }
        #endregion

        #region xóa thông tin người dùng
        [HttpDelete]
        [Route("delete/{id:int}")]
        public Result UserDelete(int id)
        {
            string msg = "";
            string processContent = "";
            User user = new User();
            using (BSS.DBM dbm = new BSS.DBM())
            {
                try
                {
                    msg = ApiTest2.Models.User.GetOne(id, out user);
                    if (msg.Length > 0)
                    {
                        return BSS.Result.GetResultError("Lỗi khi lấy thông tin người dùng" + msg);
                    }
                    msg = user.Delete(dbm);
                    if (msg.Length > 0)
                    {
                        return BSS.Result.GetResultError("Lỗi khi ghi lại nhật ký cấu hình: " + msg);
                    }

                    processContent = "đã xóa người dùng";

                    msg = Log.WriteHistoryLog(dbm, processContent, user.ObjectGUID, 0, "", 0);
                    if (msg.Length > 0)
                    {
                        return BSS.Result.GetResultError(msg);
                    }
                    else
                    {
                        return BSS.Result.GetResultOk("đã xóa thành công thông tin người dùng");
                    }
                    //break;
                }
                catch (Exception ex)
                {
                    msg = "Lỗi ngoại lệ khi xóa người dùng " + ex.Message;
                    return BSS.Result.GetResultError(msg);
                }
            }
        }
        #endregion
    }
}
