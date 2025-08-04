using TeaLeavesProductionForecastWebAPI.Data;
using TeaLeavesProductionForecastWebAPI.DTO;
using TeaLeavesProductionForecastWebAPI.Enums;
using TeaLeavesProductionForecastWebAPI.Helpers;
using TeaLeavesProductionForecastWebAPI.Model;
using Microsoft.EntityFrameworkCore;


namespace TeaLeavesProductionForecastWebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<UserResponseDto> RegisterUserAsync(RegisterUserDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email already exists.");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                PasswordHash = PasswordHelper.HashPassword(dto.Password),
                Area = dto.Area,
                Estate = dto.Estate,
                Role = dto.Role,
               
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Area = user.Area,
                Estate = user.Estate,
                Role = user.Role.ToString()
            };
        }

        public async Task<string?> LoginUserAsync(LoginRequestDto request)
        {
            var user = await _db.Users .FirstOrDefaultAsync(u => EF.Functions.Collate(u.Email, "Latin1_General_CS_AS") == request.Email);


            if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
                return null;

            return TokenHelper.GenerateJwtToken(user.Id.ToString(), user.Email, user.Role.ToString());
        }

    }
}
