using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_2.Middleware
{
    public static class IoC
    {
        public static IServiceCollection AddDependency (this IServiceCollection services)
        {
            //inyectar servicios de repositorio generico
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));          
           

            services.AddAutoMapper(cfg =>
            {
                cfg.ForAllMaps((obj, cnfg) => cnfg.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)));
            }, AppDomain.CurrentDomain.GetAssemblies() );

            return services;

            
        }


    }
}
