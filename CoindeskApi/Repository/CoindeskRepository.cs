using CoindeskApi.Interface.Repository;
using CoindeskApi.Models.MetaData;
using Dapper;
using MyCommon.Interface;

namespace CoindeskApi.Repository
{
    public class CoindeskRepository : ICoindeskRepository
    {
        public IMsDBConn _conn;
        public CoindeskRepository(IMsDBConn conn) {
            _conn = conn;
        }

        public async Task<List<Coindesk>> GetAll()
        {
            string sCmd = @$"
            SELECT [Code]
                  ,[CodeName]
                  ,[Description]
                  ,[DescriptionAes]
                  ,[RateFloat]
                  ,[UpdateTime]
                  ,[Symbol]
              FROM [dbo].[Coindesk]
           ";
            var result = await _conn.QueryAsync<Coindesk>(sCmd);
            return result.ToList();
        }

        public async Task<Coindesk> GetAssign(string Code)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Code", Code.ToUpper());

            string sCmd = @$"
            SELECT [Code]
                  ,[CodeName]
                  ,[Description]
                  ,[DescriptionAes]
                  ,[RateFloat]
                  ,[UpdateTime]
                  ,[Symbol]
              FROM [dbo].[Coindesk]
              Where Code=@Code
           ";

            var result = await _conn.QueryAsync<Coindesk>(sCmd, parameter);
            return result.FirstOrDefault();
        }


        public bool Add(Coindesk c)
        {
            List<string> NotMatchList = new List<string>();
            //NotMatchList.Add("UserId");
            int iResult =  _conn.Add(c, NotMatchList);
            return (iResult > 0) ? true : false;
        }


        public bool Update(Coindesk cd)
        {
            string[] setCol = new string[] { "Description","CodeName", "RateFloat", "Symbol" };
            string[] ConditionCol = new string[] {  "Code" };
            int iResult =  _conn.Update<Coindesk>(setCol, cd, ConditionCol, cd);
            return (iResult > 0) ? true : false;
        }


        public bool Delete(string Code="")
        {
            var parameter = new DynamicParameters();
            string sWhere = "";
            if (!string.IsNullOrEmpty(Code))
            {
                parameter.Add("@Code", Code.ToUpper());
                sWhere = " and Code=@Code";
            }
            string sCmd = $@"
                delete from Coindesk
                where 1=1 {sWhere}
                ";

            int iResult =  _conn.Excute(sCmd, parameter);
            return (iResult > 0) ? true : false;
        }

        public async Task<bool> IsCheckHaveData(string Code="")
        {
            var parameter = new DynamicParameters();
            string sWhere = "";
            if (!string.IsNullOrEmpty(Code))
            {
                parameter.Add("@Code", Code.ToUpper());
                sWhere = " and Code=@Code";
            }
            string sCmd = $@"
                select top 1 * 
                from Coindesk
                where 1=1 {sWhere}
                ";
            var result = await _conn.QueryAsync<Coindesk>(sCmd, parameter);
            if (result.FirstOrDefault() != null)
                return true;

            return false;
        }
    }
}
