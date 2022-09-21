using Common.Abstract.Configurations;
using Common.Abstract.Helpers;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Principal;
using System.Text;

namespace Repository
{

    public static class Extension
    {
 
        public static bool IsNullOrZero(this int value) => value == 0;
        public static DbCommand LoadStoredProcedure(this Context context, string storedProcName)
        {
            var cmd = context.Database.GetDbConnection().CreateCommand(); cmd.CommandText = storedProcName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }

        public static DbCommand WithSqlParams(this DbCommand cmd, params (string, object)[] nameValues)
        {
            foreach (var pair in nameValues)
            {
                var param = cmd.CreateParameter();
                param.ParameterName = pair.Item1;
                param.Value = pair.Item2 ?? DBNull.Value;
                cmd.Parameters.Add(param);
            }
            return cmd;
        }
        public static async Task<IList<T>> ExecuteStoredProcedureAsync<T>(this DbCommand command)
         where T : class
        {
            using (command)
            {
                if (command.Connection.State == System.Data.ConnectionState.Closed)
                    await command.Connection.OpenAsync();
                try
                {
                    using var reader = command.ExecuteReader();
                    return reader.MapToList<T>();
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        private static IList<T> MapToList<T>(this DbDataReader dr)
        {
            var objList = new List<T>();
            var props = typeof(T).GetRuntimeProperties();
            var colMapping = dr.GetColumnSchema()
                        .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
                        .ToDictionary(key => key.ColumnName.ToLower());
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    T obj = Activator.CreateInstance<T>();
                    foreach (var prop in props)
                    {
                        if (prop.CustomAttributes.Count(x => x.AttributeType.Name == "NotMappedAttribute").IsNullOrZero())
                        {
                            var val = dr.GetValue(colMapping[prop.Name.ToLower()].ColumnOrdinal.Value);
                            prop.SetValue(obj, val == DBNull.Value ? null : val);
                        }
                    }
                    objList.Add(obj);
                }
            }
            return objList;
        }

        public static Response ValidateToken(this string token)
        {
            Response response = new Response();
            try
            {
                if (true)//string.IsNullOrEmpty(await Static.DistributedCache.GetStringAsync(token.DeactivateToken())))
                {
                    SecurityToken validatedToken;
                    IPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(token, GetValidationParameters(), out validatedToken);
                    JwtSecurityToken validToken = (JwtSecurityToken)validatedToken;
                    response.Message = Message.UserAuthenticated;
                }
                else
                {
                    //response.StatusCode = (int)HttpStatusCode.Redirect;
                    response.Message = Message.InvalidRequest;
                }
            }
            catch (SecurityTokenValidationException stvex)
            {
                //response.HttpCode = HttpStatusCode.Redirect;
                response.Message = $"Token failed validation: {stvex.Message}";
            }
            catch (ArgumentException argex)
            {
                //response.HttpCode = HttpStatusCode.Unauthorized;
                response.Message = $"Token was invalid: {argex.Message}";
            }
            return response;
        }

        public static TokenValidationParameters GetValidationParameters()
        {
            Jwt option = new Jwt();
            return new TokenValidationParameters()
            {
                // Clock skew compensates for server time drift.
                // We recommend 5 minutes or less:
                ClockSkew = TimeSpan.FromMinutes(option.TokenExpiryMinutes),
                RequireSignedTokens = true,
                // Ensure the token hasn't expired:
                RequireExpirationTime = true,
                ValidateLifetime = true,
                // Ensure the token audience matches our audience value (default true):
                ValidateAudience = true,
                // Ensure the token was issued by a trusted authorization server (default true):
                ValidateIssuer = true,

                ValidIssuer = option.Issuer,
                ValidAudience = option.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(option.Key))
            };
        }

        public static async Task<string> FileUpload(string id,byte[] img ,string name)
        {
            var stream = new MemoryStream(img);
            IFormFile file = new FormFile(stream, 0, stream.Length, "name", name);
            string path = Path.Combine(Static.WebRootPath, id);
            //if (!Directory.Exists(path))
            //{
                Directory.CreateDirectory(path);
            //}
            string fileName = file.FileName.AppendTimeStamp();
            if (File.Exists(Path.Combine(path, fileName)))
            {
                File.Delete(Path.Combine(path, fileName));
            }
            using (Stream fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            path = Path.Combine(id, fileName);
            return path;
        }

        public static string AppendTimeStamp(this string fileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),
                DateTime.Now.ToString("-yyyyMMddHHmmssfff"),
                Path.GetExtension(fileName)
                );
        }
        //public string GetSignIN(AdminLoginModel model)
        //{
        //    var result = await _signInManager.PasswordSignInAsync(model.UserName, model.UserPassword, false, false);
        //    if (result.Succeeded)
        //    {
        //        var aspNetUser = await _userManagement.FindByName(model.UserName);
        //        if (aspNetUser != null)
        //        {
        //            var userRoles = await _userManagement.DoRoleExists(aspNetUser);

        //            if (await _userManagement.CheckUserPassword(aspNetUser, model.UserPassword))
        //            {
        //                await _userManagement.ResetAccessFailedCount(aspNetUser);
        //            }
        //            else
        //            {
        //                await _userManagement.AccessFailed(aspNetUser);
        //                if (await _userManagement.UserIsLockedOut(aspNetUser))
        //                {
        //                    response.Message = "To many login attempts, user is locked out.";
        //                    return response;
        //                }

        //                response.Message = "Invalid username or password";
        //                return response;
        //            }

        //            response.Token = _jwt.GetToken(aspNetUser.Id);
        //            response.UserId = aspNetUser.Id.Encode();
        //            //response.Data = new
        //            //{
        //            //    Token = _jwt.GetToken(aspNetUser.Id),
        //            //    User = aspNetUser,
        //            //    StatusCode = (int)HttpStatusCode.OK,
        //            //};
        //            //response.Data = JsonConvert.SerializeObject(response.Data);
        //        }
        //        else
        //        {
        //            response.Detail = Message.ErrorMessage;
        //        }
        //    }
        //    else
        //    {
        //        response.StatusCode = (int)HttpStatusCode.NotFound;
        //        response.Detail = "Username or Password is Invalid";
        //    }
        //    return;
        //}



    }
}
