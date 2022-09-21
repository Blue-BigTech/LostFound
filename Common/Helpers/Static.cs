using Common.Abstract.Configurations;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Abstract.Helpers
{
    public static class Static
    {
        public static string LoggedUserId(this Jwt jwt) => jwt.TokenClaim();
        public static string WebRootPath
        {
            get
            {
                return Directory.GetCurrentDirectory() + "\\wwwroot";
            }
        }
        //public static string ImageUrl { get { return "https://localhost:7238\\"; } }
    }
}
