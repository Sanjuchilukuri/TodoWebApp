using ToDo.Service.DTO;
using ToDo.Service.DTO.User;

namespace ToDo.Tests.MockData
{
    public class UserMockData
    {
        public static UserDetails GetValidUser()
        {
            return new UserDetails
            {
                Id = 18,
                Name = "Test",
            };
        }

        public static LoginRegisterModel ValidUserCredentials()
        {
            return new LoginRegisterModel
            {
                UserName = "Test",
                Password = "Test"
            };
        }

        public static LoginRegisterModel InValidUserCredentials()
        {
            return new LoginRegisterModel
            {
                UserName = "?><:}{][",
                Password = "?><:}{]["
            };
        }

        public static LoginRegisterModel NewuserCredentials()
        {
            return new LoginRegisterModel
            {
                UserName = "user",
                Password = "passwd"
            };
        }

        public static LoginRegisterModel ExisteduserCredentials()
        {
            return new LoginRegisterModel
            {
                UserName = "ExistedUser",
                Password = "passwd"
            };
        }

    }
}
