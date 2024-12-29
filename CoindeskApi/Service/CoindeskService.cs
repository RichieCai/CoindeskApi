using CoindeskApi.Input;
using CoindeskApi.Interface.Repository;
using CoindeskApi.Interface.Service;
using CoindeskApi.Models.MetaData;
using CoindeskApi.ViewModels;
using MyCommon.Interface;
using System.Text.Json;

namespace CoindeskApi.Service
{
    public class CoindeskService : ICoindeskService
    {
        private readonly ICoindeskRepository _icoindeskRepository;
        private readonly ICoindeskTWRepositroy _icoindeskRepositoryTW;
        private readonly IMsDBConn _msDBConn;
        public CoindeskService(IMsDBConn msDBConn, ICoindeskRepository icoindeskRepository, ICoindeskTWRepositroy icoindeskRepositoryTW)
        {
            _icoindeskRepository = icoindeskRepository;
            _icoindeskRepositoryTW = icoindeskRepositoryTW;
            _msDBConn = msDBConn;
        }


        public async Task<Coindesk> GetAssign(string Code)
        {
            return await _icoindeskRepository.GetAssign(Code);
        }


        public async Task<List<Coindesk>> GetAll()
        {
            return await _icoindeskRepository.GetAll();
        }

        public async Task<ResultVM<Coindesk>> CallApi(string sUrl)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(sUrl);

            var jsonResponse = JsonDocument.Parse(response);
            var bpi = jsonResponse.RootElement.GetProperty("bpi").EnumerateObject();

            var bIsHaveData = _icoindeskRepository.IsCheckHaveData();
            if (bIsHaveData.Result)
                _icoindeskRepository.Delete();


            var Cheinese = await _icoindeskRepositoryTW.GetAll();

            var result = bpi.Select(b => new Coindesk()
            {

                Code = b.Name.ToUpper(),
                CodeName= Cheinese.Where(x=>x.Code== b.Value.GetProperty("code").GetString()).Select(x=>x.CodeName).FirstOrDefault(),
                Description = b.Value.GetProperty("description").GetString(),
                RateFloat = b.Value.GetProperty("rate_float").GetDecimal(),
                Symbol = b.Value.GetProperty("symbol").GetString(),
                UpdateTime = DateTime.Now
            });

            foreach (var coindesk in result)
            {
                var inserted = _icoindeskRepository.Add(coindesk);
                if (!inserted)
                    return new ResultVM<Coindesk>()
                    {
                        Success = true,
                        Message = "新增失敗",
                        Data = result.ToList()
                    };
            }
            _msDBConn.Commit();
            return new ResultVM<Coindesk>()
            {
                Success = true,
                Message = "資料已取得且新增成功",
                Data= result.ToList()
            };
        }


        public ResultVM<Coindesk> Add(ConindeskInput input)
        {
            var bIsHaveData = _icoindeskRepository.IsCheckHaveData(input.code.ToUpper().Trim());
            if (bIsHaveData.Result)
                return new ResultVM<Coindesk>()
                {
                    Success = false,
                    Message = "資料已存在"
                };

            var result = new Coindesk()
            {
                Code = input.code.Trim().ToUpper(),
                CodeName = input.codename?.Trim(),
                RateFloat = input.ratefloat,
                Description = input.description?.Trim(),
                Symbol = input.symbol?.Trim(),
                UpdateTime=DateTime.Now
            };

            var inserted = _icoindeskRepository.Add(result);
            if (!inserted) 
                return new ResultVM<Coindesk>()
            {
                Success = false,
                Message = "新增失敗"
            };

            _msDBConn.Commit();
            return new ResultVM<Coindesk>()
            {
                Success = true,
                Message = "新增成功"
            };
        }

        public ResultVM<Coindesk> Update(ConindeskInput input)
        {
            if (input == null)
            {
                return new ResultVM<Coindesk>()
                {
                    Success = false,
                    Message = "請輸入幣別"
                };
            }
            var bIsHaveData = _icoindeskRepository.IsCheckHaveData(input.code.ToUpper().Trim());
            if (!bIsHaveData.Result)
                return new ResultVM<Coindesk>()
                {
                    Success = false,
                    Message = "資料不存在"
                };

            var result = new Coindesk()
            {
                Code = input.code.Trim().ToUpper(),
                CodeName = input.codename?.Trim(),
                RateFloat = input.ratefloat,
                Description =input.description?.Trim(),
                Symbol = input.symbol?.Trim(),
                UpdateTime = DateTime.Now
            };

            var updateed = _icoindeskRepository.Update(result);
            if (!updateed)
                return new ResultVM<Coindesk>()
                {
                    Success = false,
                    Message = "更新失敗"
                };
            _msDBConn.Commit();
            return new ResultVM<Coindesk>()
            {
                Success = true,
                Message = "更新成功"
            };
        }


        public ResultVM<Coindesk> Delete(string Code)
        {
            var bIsHaveData = _icoindeskRepository.IsCheckHaveData(Code.Trim().ToUpper());
            if (!bIsHaveData.Result)
                return new ResultVM<Coindesk>()
                {
                    Success = false,
                    Message = "資料不存在"
                };

            var result = _icoindeskRepository.Delete(Code.Trim().ToUpper());
            if (!result)
                return new ResultVM<Coindesk>()
                {
                    Success = false,
                    Message = "更新失敗"
                };
            _msDBConn.Commit();
            return new ResultVM<Coindesk>()
            {
                Success = true,
                Message = "更新成功"
            };
        }
    }
}
