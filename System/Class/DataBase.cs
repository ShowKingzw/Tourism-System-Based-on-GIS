using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Class
{
    public class DataBase
    {

        private string connectionString = "Data Source=(local);Initial Catalog=System;Integrated Security=True;";

        public bool PerformLogin(int username, string password)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            // 执行数据库登录操作
            string query = "SELECT COUNT(*) FROM Users WHERE UserID = @Username AND PasswordHash = @PasswordHash";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@PasswordHash", password);

            int count = (int)command.ExecuteScalar();

            // 如果用户名和密码匹配，count 应该为 1
            return count == 1;



        }


        public bool RegisterUser(int userID, string password)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            // 检查用户是否已经存在
            if (UserExists(userID))
            {
                // 可以根据需要处理用户已存在的逻辑
                //throw new InvalidOperationException("用户已存在");
                MessageBox.Show("已存在", "提示");
                return false;
            }

            // 插入新用户
            string query = "INSERT INTO Users (UserID, PasswordHash) VALUES (@UserID, @PasswordHash)";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", userID);
            command.Parameters.AddWithValue("@PasswordHash", password);

            command.ExecuteNonQuery();
            return true;


        }


        private bool UserExists(int userID)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE UserID = @UserID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@UserID", userID);

                int count = (int)command.ExecuteScalar();

                // 如果UserID已存在，count 应该大于 0
                return count > 0;
            }
        }

    }
}
