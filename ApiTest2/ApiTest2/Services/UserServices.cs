﻿using ApiTest2.Models;
using BSS;
using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace ApiTest2.Services
{
    public class UserServices
    {
        public class UserLoginInfo
        {
            public string UserName { get; set; }
            public string pwd { get; set; }
        }
        public static string CheckLogin(UserLoginInfo oClientRequestInfo, out string token)
        {
            token = "";
            string msg = ApiTest2.Models.User.GetOneUserByUserName(oClientRequestInfo.UserName, out User user);
            if (msg.Length > 0) return msg;

            var passwordHashLogin = GetInputPasswordHash(oClientRequestInfo.pwd, user.PasswordSalt);
            if (!user.PasswordHash.SequenceEqual(passwordHashLogin)) return "Tên đăng nhập hoặc Mật khẩu không đúng!";

            msg = Log.WriteActivityLog("Đăng nhập thành công", user.UserID, "");
            // Generate the JWT token after successful login
            token = GenerateJwtToken(user);

            return "";
        }

        public static string DoDelete(DBM dbm, int id, User user)
        {
            string msg = user.Delete(dbm);
            if (msg.Length > 0) return msg;

            string processContent = "đã xóa người dùng có ID: " + id;

            return Log.WriteHistoryLog(dbm, processContent, user.ObjectGUID, 0, "", 0);
        }

        public class UserAddorUpdateInfo
        {
            public string UserName { get; set; }
            public string pwd { get; set; }
            public string Name { get; set; }
            public string GioiTinh { get; set; }
            public DateTime Birthday { get; set; }
            public string Email { get; set; }
            public string NumberPhone { get; set; }
            public bool IsTeacher { get; set; }
            public bool SuperAdmin { get; set; }
            public int IsActive { get; set; }
        }
        public static string InsertorUpdateToDB(int id, UserAddorUpdateInfo oClientRequestInfo, out User user)
        {
            var pwdSalt = GenerateRandomBytes(16);
            var pwdHash = GetInputPasswordHash(oClientRequestInfo.pwd, pwdSalt);
            user = new User
            {
                UserID = id,
                Name = oClientRequestInfo.Name,
                UserName = oClientRequestInfo.UserName,
                PasswordHash = pwdHash,
                PasswordSalt = pwdSalt,
                GioiTinh = oClientRequestInfo.GioiTinh,
                IsActive = oClientRequestInfo.IsActive,
                IsTeacher = oClientRequestInfo.IsTeacher,
                SuperAdmin = oClientRequestInfo.SuperAdmin,
                ObjectGUID = Guid.NewGuid(),
                Birthday = oClientRequestInfo.Birthday,
                Email = oClientRequestInfo.Email,
                NumberPhone = oClientRequestInfo.NumberPhone
            };

            DBM dbm = new DBM();
            dbm.BeginTransac();

            string msg = user.InsertorUpdate(dbm);
            if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

            dbm.CommitTransac();

            msg = User.GetOneUserByUserName(oClientRequestInfo.UserName, out User user1);
            if (msg.Length > 0) return msg;

            msg = Log.WriteHistoryLog(user.UserID == 0 ? "thêm mới user" : "sửa user", user1.ObjectGUID, 0, "", 0);
            return msg;
        }

        //public class UserUpdateInfo
        //{
        //    public string UserName { get; set; }
        //    public string Name { get; set; }
        //    public string pwd { get; set; }
        //    public string GioiTinh { get; set; }
        //    public int IDPhongban { get; set; }
        //    public int IDChucVu { get; set; }
        //    public int IDDonVi { get; set; }
        //    public DateTime Birthday { get; set; }
        //    public string Email { get; set; }
        //    public string NumberPhone { get; set; }
        //}
        //public static string UserUpdateToDB(int id, UserUpdateInfo oClientRequestInfo, out User user)
        //{
        //    var pwdSalt = GenerateRandomBytes(16);
        //    var pwdHash = GetInputPasswordHash(oClientRequestInfo.pwd, pwdSalt);
        //    user = new User
        //    {
        //        UserID = id,
        //        Name = oClientRequestInfo.Name,
        //        UserName = oClientRequestInfo.UserName,
        //        PasswordHash = pwdHash,
        //        PasswordSalt = pwdSalt,
        //        GioiTinh = oClientRequestInfo.GioiTinh,
        //        IDPhongban = oClientRequestInfo.IDPhongban,
        //        IDChucVu = oClientRequestInfo.IDChucVu,
        //        IDDonVi = oClientRequestInfo.IDDonVi,
        //        Birthday = oClientRequestInfo.Birthday,
        //        Email = oClientRequestInfo.Email,
        //        NumberPhone = oClientRequestInfo.NumberPhone,
        //    };

        //    DBM dbm = new DBM();
        //    dbm.BeginTransac();

        //    string msg = user.Update(dbm);
        //    if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

        //    dbm.CommitTransac();

        //    string processContent = "đã sửa phòng ban có ID: " + id + " nội dung sửa: " + oClientRequestInfo.UserName + oClientRequestInfo.Name + oClientRequestInfo.GioiTinh + oClientRequestInfo.IDPhongban + oClientRequestInfo.IDChucVu + oClientRequestInfo.Birthday + oClientRequestInfo.Email + oClientRequestInfo.NumberPhone;
        //    msg = User.GetOneUserByID(id, out user);
        //    if (msg.Length > 0) return msg;

        //    msg = Log.WriteHistoryLog(dbm, processContent, user.ObjectGUID, 0, "", 0);
        //    return msg;
        //}

        #region for password only
        public static byte[] GetInputPasswordHash(string pwd, byte[] salt)
        {
            int passwordDerivationIteration = 1000;
            int passwordBytesLength = 64;
            var inputPwdBytes = Encoding.UTF8.GetBytes(pwd);
            var inputPwdHasher = new Rfc2898DeriveBytes(inputPwdBytes, salt, passwordDerivationIteration);
            return inputPwdHasher.GetBytes(passwordBytesLength);
        }
        public static byte[] GenerateRandomBytes(int length)
        {
            var result = new byte[length];
            RandomNumberGenerator.Create().GetBytes(result);
            return result;
        }

        #endregion

        #region token generate
        private static string GenerateJwtToken(User user)
        {
            var secretKey = "L40/p+SbAVQGchV1pOY2qxaydhYfB0MNvZlSfwB9Kd2vxbdfHrGuPfEHRkD0CeIy\r\n";
            // Define key and signing credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)); // Replace with a secure key
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Define claims (add more claims if needed)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()), // User ID
                new Claim(ClaimTypes.Name, user.UserName),                    // Username
                new Claim("IsTeacher", user.IsTeacher.ToString()),            // Custom claim: Teacher
                new Claim("SuperAdmin", user.SuperAdmin.ToString())
            };

            var identity = new ClaimsIdentity(claims, "CustomAuth");

            // Set authentication cookie or token
            var principal = new ClaimsPrincipal(identity);
            HttpContext.Current.User = principal;

            // Create the token
            var token = new JwtSecurityToken(
                issuer: "ApiTest2", // Replace with your app's name or URL
                audience: "ApiTest2",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30), // Token expiration
                signingCredentials: credentials
            );

            // Return the serialized token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        public class JwtMiddleware : DelegatingHandler
        {
            private const string SecretKey = "JwtSecretKey"; // Same key used for generating the token

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (!request.Headers.TryGetValues("Authorization", out var authorizationHeaders))
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Missing Authorization Header" };
                }

                var token = authorizationHeaders.FirstOrDefault()?.Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid Token" };
                }

                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Convert.FromBase64String(SecretKey);

                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var claims = jwtToken.Claims;
                    // Add claims to context if necessary
                    ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
                    Thread.CurrentPrincipal = principal;
                }
                catch
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid Token" };
                }

                return await base.SendAsync(request, cancellationToken);
            }
        }
    }
}