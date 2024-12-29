using CoindeskApi.Input;
using CoindeskApi.Models.MetaData;

namespace CoindeskApi.Interface.Repository
{
    public interface ICoindeskTWRepositroy
    {
        Task<List<CoindeskTw>> GetAll();
        
    }

}