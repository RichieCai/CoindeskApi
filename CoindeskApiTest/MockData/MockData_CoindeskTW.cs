using CoindeskApi.Models.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoindeskApiTest.MockData
{
    public class MockData_CoindeskTW
    {

        public static List<CoindeskTw> GetAll()
        {
            return new List<CoindeskTw>()
            {
                new CoindeskTw()
                {
                    Code="USD",
                    CodeName="美元",
                },
                new CoindeskTw()
                {
                    Code="GBP",
                    CodeName="英鎊",
                },
            };
        }
    }
}
