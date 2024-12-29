using CoindeskApi.Models.MetaData;

namespace CoindeskApiTest.MockData
{
    public class MockData_Coindesk
    {

        public static Coindesk GetAssign(string sCode)
        {
            return GetAll().Where(x => x.Code == sCode.Trim().ToUpper()).SingleOrDefault();
        }
        public static List<Coindesk> GetAll()
        {
            return new List<Coindesk>()
            {
                new Coindesk()
                {
                    Code="USD",
                    CodeName="美元",
                    Description="United States Dollar",
                    RateFloat=Convert.ToDecimal(94961.7975),
                    UpdateTime=DateTime.Now,
                    Symbol="&#36;",
                },
                new Coindesk()
                {
                    Code="GBP",
                    CodeName="英鎊",
                    Description="British Pound Sterling",
                    RateFloat=Convert.ToDecimal(75730.1343),
                    UpdateTime=DateTime.Now,
                    Symbol="&pound;",
                },
            };
        }
        public static Coindesk Add()
        {
            return new Coindesk()
            {
                Code = "EUR",
                CodeName = "歐元",
                Description = "Euro",
                RateFloat = Convert.ToDecimal(91103.5946),
                UpdateTime = DateTime.Now,
                Symbol = "&&euro;"
                };
        }

        public static Coindesk Update()
        {
            return new Coindesk()
            {
                Code = "GBP",
                CodeName = "英鎊123",
                Description = "update",
                RateFloat = Convert.ToDecimal(55555.1343),
                UpdateTime = DateTime.Now,
                Symbol = "&pound;",
            };
        }

    }
}
