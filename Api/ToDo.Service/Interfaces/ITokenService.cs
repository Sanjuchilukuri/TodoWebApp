namespace ToDo.Service.Interfaces
{
    public interface ITokenService
    {
        public string GenerateRefreshToken(int userId);

        public string GenerateAccessToken(int userId);

        public bool ValidateRefreshToken(string refreshToken);

        public dynamic GenerateNewTokens(string refreshToken);
    }
}
