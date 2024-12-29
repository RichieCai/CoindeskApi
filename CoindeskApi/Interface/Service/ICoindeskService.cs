using CoindeskApi.Input;
using CoindeskApi.Models.MetaData;
using CoindeskApi.ViewModels;

namespace CoindeskApi.Interface.Service
{
    public interface ICoindeskService
    {
        Task<Coindesk> GetAssign(string code);

        Task<List<Coindesk>> GetAll();
        Task<ResultVM<Coindesk>> CallApi(string sUrl);

        ResultVM<Coindesk> Add(ConindeskInput input);

        ResultVM<Coindesk> Update(ConindeskInput input);

        ResultVM<Coindesk> Delete(string Code);
    }
}
