using CoindeskApi.Input;
using CoindeskApi.Models.MetaData;
using Microsoft.AspNetCore.Mvc;

namespace CoindeskApi.Interface.Repository
{
    public interface ICoindeskRepository
    {

        Task<Coindesk> GetAssign(string code);

        Task<List<Coindesk>> GetAll();

        public bool Add(Coindesk c);

        public bool Update(Coindesk cd);

        public bool Delete(string Code = "");

        Task<bool> IsCheckHaveData(string Code = "");
    }
}
