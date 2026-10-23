using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ASPNETWebAppMVCStudentApp.Helpers
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Băm mật khẩu bằng SHA-256 theo đúng cách SQL Server làm với
        /// HASHBYTES('SHA2_256', N'...'): mã hóa UTF-16LE trước khi băm.
        /// </summary>
        public static byte[] Hash(string password)
        {
            using (var sha = SHA256.Create())
            {
                return sha.ComputeHash(Encoding.Unicode.GetBytes(password));
            }
        }

        public static bool Matches(byte[] storedHash, string password)
        {
            return storedHash != null && Hash(password).SequenceEqual(storedHash);
        }
    }
}
