using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        public readonly IUserRepository _userRepository;
        public AccountService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterUser(RegisterModel model)
        {
            var user = await _userRepository.GetUserByEmail(model.Email);
            if (user != null) { throw new Exception("Duplicated Email, please log in."); }
            var salt = getRandomSalt();
            var hasedPassword = GetHashedPassword(model.Password, salt);
            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Salt = salt,
                DateOfBirth = model.DateOfBirth,
                HashedPassword = hasedPassword
            };
            await _userRepository.Add(newUser);
            return true;
        }

        public async Task<UserInfoModel> ValidateUser(string email, string password)
        {
            // 用户登录，查看email是否已经注册，在数据库中
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)  // 数据库中找不到 email
            {
                throw new Exception("Email does not exist.");
            }

            // 找到 email，开始比较密码是否一致
            var hasedPassword = GetHashedPassword(password, user.Salt);
            if (hasedPassword.Equals(user.HashedPassword))
            {
                // 创建 cookie
                var userInfo = new UserInfoModel
                {
                    Email = user.Email,
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth.GetValueOrDefault()
                };
                return userInfo;
            }
            else
            {
                throw new Exception("Invalid credentials: Email & Pasword don't match.");
            }
        }

        // 生成 salt
        // 官方文档：https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-9.0
        private string getRandomSalt() {
            // Generate a 128-bit salt using a sequence of
            // cryptographically strong random bytes.
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            return Convert.ToBase64String(salt);
        }
        // 用 salt 和密码生成 hashedpassword
        private string GetHashedPassword(string password, string salt) {
            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: Convert.FromBase64String(salt), // string 转换为 字节数组 byte[]
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
