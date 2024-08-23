using WebApi.Models;

namespace WebApi.Services
{
    public interface IUserService
    {
        bool IsValidUserInformation(LoginModel model);
        LoginModel GetUserDetails();        
    }
}
