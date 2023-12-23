using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Data.Common;
using System.Data;

namespace BookInventoryAdminProgram.Model
{
    class PasswordFunctions
    {


        /// <summary>
        /// Grabs salt for user by userID (login Username)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private static string ReturnSalt(int userID)
        {
            string PasswordSalt = string.Empty;
            using (SqlConnection connection = new SqlConnection(Helper.ReturnSettings().ConnectionString))
            {
                try
                {
                    PasswordSalt = connection.Query<string>("dbo.spGetSalt @EmployeeID", new { EmployeeID = userID }).ToList()[0];
                }
                catch(ArgumentOutOfRangeException ex) 
                {
                    return "Employee doesnt exist";
                }
            }
            return PasswordSalt;
        }

        /// <summary>
        /// Hashes Password in SHA256
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private static byte[]  HashPasswordSHA256(string password)
        {       
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return data;
            }
        }
        /// <summary>
        /// Grabs hashed password in DB and checks user's hash against server's hash
        /// </summary>
        /// <param name="userID">This is EmployeeID in the DB</param>
        /// <param name="password"></param>
        /// <returns>true = entry granted, otherwise, re-enter password.</returns>
        public bool VerifyPassword(int userID, string password)
        {

            string passwordSalt = ReturnSalt(userID);
            if (userID == 0 || passwordSalt == "Employee doesnt exist")
            {
                return false;
            }

            Byte[] userPasswordHash = HashPasswordSHA256(password + passwordSalt);
            Byte[] serverPasswordHash;
            bool isAdmin;

            using (SqlConnection connection = new SqlConnection(Helper.ReturnSettings().ConnectionString))
            {
                serverPasswordHash = connection.QuerySingle<Byte[]>("dbo.spGetHash @EmployeeID", new { EmployeeID = userID });
                isAdmin = connection.QuerySingle<bool>("dbo.spVerifyAdmin @EmployeeID", new { EmployeeID = userID });
            }


            string userPasswordHashString = BitConverter.ToString(userPasswordHash).Replace("-", "");
            string serverPasswordHashString = BitConverter.ToString(serverPasswordHash).Replace("-", "");
            if (userPasswordHashString == serverPasswordHashString && isAdmin)
                return true;
            else
                return false;
        }
    }
}
