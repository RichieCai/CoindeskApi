using CoindeskApi.Input;
using CoindeskApi.Models.MetaData;
using CoindeskApi.Response;

namespace CoindeskApi.Interface.Service
{
    public interface ICoindeskService
    {
        Task<Coindesk> GetAssign(string code);

        Task<List<Coindesk>> GetAll();
        Task<ApiResponse<List<Coindesk>>> CallApi(string sUrl);

        ApiResponse<Coindesk> Add(ConindeskInput input);

        ApiResponse<Coindesk> Update(ConindeskInput input);

        ApiResponse<Coindesk> Delete(string Code);
    }
}
