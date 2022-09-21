using Common.Abstract.Configurations;
using Common.Abstract.Entities;
using Common.Abstract.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NodaTime;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Common.Abstract.Helpers
{
    public static class TokenHelper
    {
        private static HttpContext httpContext => new HttpContextAccessor().HttpContext;
        public static string GetToken(this Jwt options, string userId)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            };
            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(options.Issuer,
              options.Issuer,
              claims, null,
              DateTime.Now.AddMinutes(options.TokenExpiryMinutes),
              signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key)), SecurityAlgorithms.HmacSha256)));
        }
        public static string TokenGenerate(this Jwt jwt, IEnumerable<Claim> claims, bool isRemember)
        {
            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(jwt.Issuer,
              jwt.Issuer,
              claims, null, 
              DateTime.Now.AddMinutes(jwt.TokenExpiryMinutes),
              signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)), SecurityAlgorithms.HmacSha256)));
        }





        public static TokenValidationParameters GetValidationParameters(double expiryMin, string issuer, string key)
        {
            return new TokenValidationParameters()
            {
                // Clock skew compensates for server time drift.
                // We recommend 5 minutes or less:
                ClockSkew = TimeSpan.FromMinutes(expiryMin),
                RequireSignedTokens = true,
                // Ensure the token hasn't expired:
                RequireExpirationTime = true,
                ValidateLifetime = true,
                // Ensure the token audience matches our audience value (default true):
                ValidateAudience = true,
                // Ensure the token was issued by a trusted authorization server (default true):
                ValidateIssuer = true,

                ValidIssuer = issuer,
                ValidAudience = issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };
        }

        public static Response ValidateToken(this string token, Jwt options)
        {
            Response response = new();
            try
            {
                SecurityToken validatedToken;
                IPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(token, GetValidationParameters(options.TokenExpiryMinutes, options.Issuer, options.Key), out validatedToken);
                JwtSecurityToken validToken = (JwtSecurityToken)validatedToken;
                response.Message = Message.UserAuthenticated;
            }
            catch (SecurityTokenValidationException stvex)
            {
                response.StatusCode = (int)HttpStatusCode.Redirect;
                response.Message = $"Token failed validation: {stvex.Message}";
            }
            catch (ArgumentException argex)
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.Message = $"Token was invalid: {argex.Message}";
            }
            return response;
        }


        //private static readonly Random Random = new();

        //public static string ModifyTokenAndReturnSignature(TokenResponse token)
        //{
        //    var tokenParts = token.Token.Split('.');
        //    var signature = tokenParts[2];

        //    var sb = new StringBuilder();
        //    for (int i = 0; i < signature.Length * 2; i++)
        //    {
        //        sb.Append((char)Random.Next('A', 'z'));
        //    }

        //    tokenParts[2] = sb.ToString().AsBase64().Substring(0, signature.Length);
        //    token.Token = string.Join(".", tokenParts);
        //    return signature;
        //}
        public static string TokenClaim(this Jwt options)
        {
            Response response = new Response();
            try
            {
                string authToken = httpContext.Token();
                SecurityToken validatedToken;
                IPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(authToken,
                   GetValidationParameters(options.TokenExpiryMinutes, options.Issuer, options.Key), out validatedToken);
                JwtSecurityToken validToken = (JwtSecurityToken)validatedToken;
                return validToken.Payload.Claims?.FirstOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Sub, StringComparison.OrdinalIgnoreCase))?.Value;

            }
            catch (SecurityTokenValidationException stvex)
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.Message = $"Token failed validation: {stvex.Message}";
            }
            catch (ArgumentException argex)
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.Message = $"Token was invalid: {argex.Message}";
            }
            return "";
        }
        //public class TokenResponse
        //{
        //    public string Token { get; set; }
        //    public Instant Expires { get; set; }
        //}
    }
}
