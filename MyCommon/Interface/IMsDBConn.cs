using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MyCommon.Interface
{
    public interface IMsDBConn
    {
        IDbConnection Connection { get; }
        IDbTransaction Transcation { get; }

        int Add<T>(T data, List<string> NotMatchList = null);

        Task<int> AddAsync<T>(T data, List<string> NotMatchList = null);
        int Update<T>(string[] setColumns, T setValues, string[] conditionColumns = null, T conditionValues = default(T));

        Task<int> UpdateAsync<T>(string[] setColumns, T setValues, string[] conditionColumns = null, T conditionValues = default(T));
        int Delete<T>(T data);
        Task<int> DeleteAsync<T>(T data);
        IEnumerable<T> Query<T>(string sSqlCmd, IDynamicParameters param = null);
        Task<IEnumerable<T>> QueryAsync<T>(string sSqlCmd, IDynamicParameters param = null);

        Task<T> QueryFirstAsync<T>(string sSqlCmd, IDynamicParameters param = null);

        Task<T> QueryFirstOrDefaultAsync<T>(string sSqlCmd, IDynamicParameters param = null);

        Task<T> QuerySingleAsync<T>(string sSqlCmd, IDynamicParameters param = null);

        Task<T> QuerySingleOrDefaultAsync<T>(string sSqlCmd, IDynamicParameters param = null);

        int Excute(string sql, IDynamicParameters param);
        Task<int> ExecuteAsync(string sql, SqlMapper.IDynamicParameters param);

        T ExecuteScalar<T>(string sSqlCmd, IDynamicParameters param = null);

        Task<T> ExecuteScalarAsync<T>(string sSqlCmd, IDynamicParameters param = null);

        void Commit();
        void Rollback();
        void Dispose();
    }
}
