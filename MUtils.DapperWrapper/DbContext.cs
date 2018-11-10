using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace MUtils.DapperWrapper
{
    public abstract class DbContext
    {
        private readonly string _cs;

        protected DbContext(string cs)
        {
            _cs = cs;
        }

        protected IEnumerable<T> Query<T>(string script, object param = null)
        {
            using (var conn = new SqlConnection(_cs))
            {
                return conn.Query<T>(script, param);
            }
        }

        protected IEnumerable<dynamic> Query(string script, object param = null)
        {
            using (var conn = new SqlConnection(_cs))
            {
                return conn.Query(script, param);

            }
        }

        protected IDataReader Reader(string script, object param = null)
        {
            using (var conn = new SqlConnection(_cs))
            {
                var result = conn.ExecuteReader(script, param);
                return result;
            }
        }

        protected (IEnumerable<T1>, IEnumerable<T2>) QueryMultiple<T1, T2>(string script, object param = null)
        {
            using (var conn = new SqlConnection(_cs))
            {
                var result = conn.QueryMultiple(script, param);
                var first = result.Read<T1>();
                var second = result.Read<T2>();
                return (first, second);
            }
        }

        protected (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>) QueryMultiple<T1, T2, T3>(string script,
            object param = null)
        {
            using (var conn = new SqlConnection(_cs))
            {
                var result = conn.QueryMultiple(script, param);
                var first = result.Read<T1>();
                var second = result.Read<T2>();
                var third = result.Read<T3>();
                return (first, second, third);
            }
        }

        protected IEnumerable<Dictionary<string, object>> ReaderDictionary(string script, object param = null)
        {
            using (var conn = new SqlConnection(_cs))
            {
                var result = conn.ExecuteReader(script, param);

                while (result.Read())
                {
                    var cnt = result.FieldCount;
                    var dic = new Dictionary<string, object>();
                    for (int i = 0; i < cnt; i++)
                    {
                        var fieldName = result.GetName(i);
                        var value = result[i];
                        dic.Add(fieldName, value);
                    }
                    yield return dic;
                }
            }
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string script, object param = null)
        {
            using (var conn = new SqlConnection(_cs))
            {
                return await conn.QueryAsync<T>(script, param);
            }
        }

        protected void Execute(string script, object param)
        {
            using (var conn = new SqlConnection(_cs))
            {
                conn.Execute(script, param);
            }
        }

        protected async Task ExecuteAsync(string script, object param)
        {
            using (var conn = new SqlConnection(_cs))
            {
                await conn.ExecuteAsync(script, param);
            }
        }

        protected async Task ExecuteSpAsync(string spName, DynamicParameters param)
        {
            using (var conn = new SqlConnection(_cs))
            {
                param.Add("PO_Error", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("PO_Step", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await conn.ExecuteAsync(spName, param, commandType: CommandType.StoredProcedure);
            }
        }

        protected void ExecuteSp(string spName, DynamicParameters param)
        {
            using (var conn = new SqlConnection(_cs))
            {
                param.Add("PO_Error", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("PO_Step", dbType: DbType.Int32, direction: ParameterDirection.Output);
                conn.Execute(spName, param, commandType: CommandType.StoredProcedure);
            }
        }
        protected int ExecuteSp(string spName, DynamicParameters param,out DynamicParameters outParam)
        {
            using (var conn = new SqlConnection(_cs))
            {
                param.Add("PO_Error", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("PO_Step", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var result = conn.Execute(spName, param, commandType: CommandType.StoredProcedure);
                outParam = param;
                return result;
            }
        }
    }
}
