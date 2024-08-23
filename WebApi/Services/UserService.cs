using WebApi.Models;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        public bool IsValidUserInformation(LoginModel Model)
        {
            if (Model.UserName.Equals("Keyhan") && Model.Password.Equals("123456"))
                return true;
            else
                return false;
        }

        public LoginModel GetUserDetails()
        {
            return new LoginModel { UserName = "Jay", Password = "123456" };
        }
    }
}
