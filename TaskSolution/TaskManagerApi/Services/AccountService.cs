using Entities.Models;
using System.Security.Claims;
using System.Text;
using TaskManagerApi.Data;

namespace TaskManagerApi.Services
{
    public class AccountService
    {
        public readonly AppDbContext _db;

        public AccountService(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        public (string, string) GetUserFromBasicAuth(HttpRequest request)
        {
            string userLogin = "";
            string userPassword = "";
            string authHeader = request.Headers["Authorization"].ToString();

            if (authHeader != null && authHeader.StartsWith("Basic")) 
            {
                string encodedUser = authHeader.Replace("Basic", "");
                var encoding = Encoding.GetEncoding("iso-8859-1");

                string[] user = encoding.GetString(Convert.FromBase64String(encodedUser)).Split(':');
                userLogin = user[0];
                userPassword = user[1];
            }

            return (userLogin, userPassword);
        }
    }
}
