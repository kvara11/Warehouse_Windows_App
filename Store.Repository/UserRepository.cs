using System;
using System.Collections.Generic;
using System.Text;
using Store.Domain;
using System.Data;
using System.Data.SqlClient;

namespace Store.Repository
{
    public class UserRepository : BaseRepository
    {
        public int Login(string username, string password, out string message)
        {
            //var parameter = CustomSqlParameter("@responseID");
            var loggedUserID = new SqlParameter("@LoggedUserID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var responseMessage = new SqlParameter("@ResponseMessage", SqlDbType.NVarChar, size: 250) { Direction = ParameterDirection.Output };
            var result = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };

            _database.ExecuteNonQuery(
                "UserLogin_SP",
                CommandType.StoredProcedure,
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password),
                loggedUserID,
                responseMessage,
                result
            );

            if ((int)result.Value != 0)
            {
                //throw new Exception(responseMessage.Value.ToString());
                message = responseMessage.Value.ToString();
                return -1;
            }

            message = null;
            return (int)loggedUserID.Value;
        }


        public SqlParameter CustomSqlParameter(string responseID)
        {
            var outparameter = new SqlParameter();
            outparameter.ParameterName = responseID;
            outparameter.SqlDbType = SqlDbType.Int;
            outparameter.Direction = ParameterDirection.Output;
            return outparameter;
        }
    }
}
