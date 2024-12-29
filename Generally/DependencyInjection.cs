using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generally
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service)
        {
            //service.AddTransient<ITodoListRepository, ToDoListRepository>();
            //service.AddTransient<IUnitOfWork, UnitOfWork>();
            return service;
        }
    }
}
