using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace OpenAuth.Generator.Common
{
    public class SqlHelper
    {
        public static T FindEntity<T>(string connectionString, string sqlText, params SqlParameter[] Parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                var dynamicParameter = ToDynamicParameter(Parameters);
                return conn.QueryFirstOrDefault<T>(sqlText, dynamicParameter, null, 180, CommandType.Text);
            }
        }

        public static List<T> Query<T>(string connectionString, string sqlText, params SqlParameter[] Parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                var dynamicParameter = ToDynamicParameter(Parameters);
                return conn.Query<T>(sqlText, dynamicParameter, null, false, 180, CommandType.Text).ToList();
            }
        }

        public static List<dynamic> Query(string connectionString, string sqlText, params SqlParameter[] Parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                var dynamicParameter = ToDynamicParameter(Parameters);
                return conn.Query<dynamic>(sqlText, dynamicParameter, null, false, 180, CommandType.Text).ToList();
            }
        }

        public static DynamicParameters ToDynamicParameter(params SqlParameter[] sqlParameters)
        {
            DynamicParameters parameters = new DynamicParameters();
            foreach (SqlParameter param in sqlParameters)
            {
                parameters.Add(param.ParameterName, param.Value);
            }
            return parameters;
        }
    }
}
