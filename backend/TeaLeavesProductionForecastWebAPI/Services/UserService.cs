using TeaLeavesProductionForecastWebAPI.Data;
using TeaLeavesProductionForecastWebAPI.DTO;
using TeaLeavesProductionForecastWebAPI.Enums;
using TeaLeavesProductionForecastWebAPI.Helpers;
using TeaLeavesProductionForecastWebAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;


namespace TeaLeavesProductionForecastWebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly IMailService _mailService;

        public UserService(AppDbContext db, IMailService mailService)
        {
            _db = db;
            _mailService = mailService;
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

        public async Task<UserResponseDto?> GetUserByIdAsync(int userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

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

        public async Task<string?> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            bool emailExists = await _db.Users.AnyAsync(u => u.Email == dto.Email && u.Id != id);
            if (emailExists)
                throw new Exception("Email already exists.");

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.Area = dto.Area;
            user.Estate = dto.Estate;
            user.Role = dto.Role;
            user.UpdatedAt = DateTime.Now;

            await _db.SaveChangesAsync();

            return "User updated successfully";
        }


        public async Task<bool> SendResetPasswordEmailAsync(int userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null) return false;

            var otp = new Random().Next(100000, 999999).ToString();

            user.PasswordResetOtp = otp;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(10);
            await _db.SaveChangesAsync();

            try
            {
                string subject = "Reset Password OTP";
                string body = $"Your OTP is: {otp}. It expires in 10 minutes.";

                await _mailService.SendEmailAsync(user.Email, subject, body);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email sending failed: " + ex.Message);
                return false;
            }
        }


        public async Task<bool> VerifyOtpAndResetPasswordAsync(int userId, string otp, string newPassword)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null) return false;

            // Check if OTP matches
            if (user.PasswordResetOtp != otp)
                throw new Exception("Invalid OTP");

            // Check if OTP is expired
            if (user.OtpExpiry == null || user.OtpExpiry < DateTime.UtcNow)
                throw new Exception("OTP has expired");

            // Hash the new password before saving
            user.PasswordHash = PasswordHelper.HashPassword(newPassword);

            // Clear OTP fields
            user.PasswordResetOtp = null;
            user.OtpExpiry = null;
            user.UpdatedAt = DateTime.Now;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SendResetPasswordEmailByEmailAsync(string email)
        {
            // Find user by email
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            // Generate OTP
            var otp = new Random().Next(100000, 999999).ToString();
            user.PasswordResetOtp = otp;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(10);

            await _db.SaveChangesAsync();

            try
            {
                string subject = "Reset Password OTP";
                string body = $"Your OTP is: {otp}. It expires in 10 minutes.";

                await _mailService.SendEmailAsync(user.Email, subject, body);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email sending failed: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> VerifyOtpAndResetPasswordByEmailAsync(string email, string otp, string newPassword)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            if (user.PasswordResetOtp != otp)
                throw new Exception("Invalid OTP");

            if (user.OtpExpiry == null || user.OtpExpiry < DateTime.UtcNow)
                throw new Exception("OTP has expired");

            user.PasswordHash = PasswordHelper.HashPassword(newPassword);

            user.PasswordResetOtp = null;
            user.OtpExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }
    }
}
