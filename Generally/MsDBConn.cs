using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generally
{
    public class MsDBConn
    {
        private string Connectionstring = "DefaultConnection";
        IDbConnection _conn = null;
        private readonly IConfiguration _config;
        public MsDBConn(IConfiguration config)
        {
            _config = config;
        }
        public async Task<IEnumerable<T>> LoadData<T>(string sSqlCmd)
        {
            IEnumerable<T> tResult = null;
            try
            {
                using (_conn = new SqlConnection(_config.GetConnectionString(Connectionstring)))
                {
                    _conn.Open();
                    tResult = await _conn.QueryAsync<T>(sSqlCmd, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }
            return tResult;
        }

        public async Task<IEnumerable<T>> LoadData<T, U>(string sSqlCmd, U parmeters)
        {
            IEnumerable<T> tResult = null;
            try
            {
                using (_conn = new SqlConnection(_config.GetConnectionString(Connectionstring)))
                {
                    _conn.Open();
                    tResult = await _conn.QueryAsync<T>(sSqlCmd, parmeters, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }
            return tResult;
        }

        public async Task<int> ExecuteData(string sSqlCmd, dynamic parmeters)
        {
            int iResult = 0;
            try
            {
                using (_conn = new SqlConnection(_config.GetConnectionString(Connectionstring)))
                {
                    _conn.Open();
                    // iResult = await conn.ExecuteAsync(sSqlCmd, new { parmeters }, commandType: CommandType.Text);
                    iResult = await _conn.ExecuteAsync(sSqlCmd, (object)parmeters, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }
            return iResult;

        }

        public async Task<int> ExecuteData2<U>(string sSqlCmd, U parmeters, IDbConnection conn)
        {
            int iResult = 0;
            try
            {
                using (_conn = new SqlConnection(_config.GetConnectionString(Connectionstring)))
                {
                    _conn.Open();
                    iResult = await _conn.ExecuteAsync(sSqlCmd, parmeters, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_conn.State == ConnectionState.Open)
                {
                    _conn.Close();
                }
            }
            return iResult;

        }

        public async Task<IEnumerable<T>> LoadDataSP<T, U>(string storeProcedure, U parmeters)
        {
            using (_conn = new SqlConnection(_config.GetConnectionString(Connectionstring)))
            {
                return await _conn.QueryAsync<T>(storeProcedure, parmeters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task ExecuteDataSP<T>(string storeProcedure, T parmeters)
        {
            using (_conn = new SqlConnection(_config.GetConnectionString(Connectionstring)))
            {
                await _conn.ExecuteAsync(storeProcedure, parmeters, commandType: CommandType.StoredProcedure);
            }

        }




    }
}
