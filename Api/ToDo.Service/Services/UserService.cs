using AutoMapper;
using ToDo.Service.DTO;
using ToDo.Service.DTO.User;
using ToDo.Service.Interfaces;
using ToDo.Data.Entities;
using ToDo.Data.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ToDo.Service.Services
{
    /// <inheritdoc cref="IUser"/>
    public class UserService : IUserService
    {
        private IUserRepo _userRepo;
        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor for <see cref="UserServices"/>
        /// </summary>
        /// <param name="userRepo"><see cref="IUserRepo"/></param>
        /// <param name="mapper"><see cref="IMapper"/></param>
        public UserService(IUserRepo userRepo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            return  int.Parse( _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        public async Task<bool> IsUserExisted(string userName)
        {
            return await _userRepo.IsUserExistedAsync(userName);
        }

        public async Task<UserDetails> Login(LoginRegisterModel user)
        {
            return _mapper.Map<UserDetails>(await _userRepo.IsValidUserAsync(_mapper.Map<User>(user)));
        }

        public async Task<bool> Register(LoginRegisterModel newUser)
        {
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            return await _userRepo.CreateUserAsync(_mapper.Map<User>(newUser));
        }
    }
}
