
namespace Common.Abstract.Configurations
{
    public class Jwt
    {
        public int TokenExpiryMinutes { get; set; }
        public string Key { get; set; }
        public string Issuer { get; set; }
    }    
    public class AuthCredential
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
    public class ModuleAccessLevel
    {
        public string Path { get; set; }
        public string RoleId { get; set; }
    }
}
