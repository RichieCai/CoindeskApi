using CoindeskApi.Interface.Repository;
using CoindeskApi.Models.MetaData;
using Dapper;
using MyCommon.Interface;

namespace CoindeskApi.Repository
{
    public class CoindeskTWRepositroy : ICoindeskTWRepositroy
    {
        public IMsDBConn _conn;
        public CoindeskTWRepositroy(IMsDBConn conn)
        {
            _conn = conn;
        }


        public async Task<List<CoindeskTw>> GetAll()
        {
            var parameter = new DynamicParameters();

            string sCmd = $@"
                select  * 
                from CoindeskTW
                ";
            var result = await _conn.QueryAsync<CoindeskTw>(sCmd, parameter);
            return result.ToList();
        }
    }
}
