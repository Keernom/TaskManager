using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TaskManagerApi.Auth
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        const string KEY = "j4hR6QddW/7a2fK6XZ55bHapSRlrMnklu+nn0m4isJA=";   // ключ для шифрации
        public const int LIFETIME = 10; // время жизни токена - 10 минут
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
