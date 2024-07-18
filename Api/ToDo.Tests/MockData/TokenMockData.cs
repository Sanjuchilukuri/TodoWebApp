namespace ToDo.Tests.MockData
{
    public class TokenMockData
    {
        public static string validRefreshToken = "valid-token";

        public static string inValidRefreshToken = "invalid-token";

        public static dynamic GetTokens()
        {
            return new
            {
                jwttoken = "jwt-token",
                refreshToken = "refresh-token"
            };
        }
    }
}
