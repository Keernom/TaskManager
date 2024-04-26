using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TaskManagerWPF.Services.Tests
{
    [TestClass()]
    public class UserRequestServiceTests
    {
        [TestMethod()]
        public void GetTokenTest()
        {
            var token = new UserRequestService().GetToken("admin", "qwerty123");
            Console.WriteLine(token.Access_Token);
            Console.WriteLine(token.Username);
            Assert.IsNotNull(token.Access_Token);
        }
    }
}