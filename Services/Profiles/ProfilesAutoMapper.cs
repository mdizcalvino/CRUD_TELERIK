using AutoMapper;
using Kendo.Mvc.UI;
using Modelos.Modelos;
using Services.Dtos;
using System;
using System.Collections.Generic;

using System.Text;

namespace Services.Profiles
{
    public class Source<T>
    {
        public T Value { get; set; }
        public string Fuente { get; set; }
    }

    public class Destination<T>
    {
        public T Value { get; set; }
        public string Destino { get; set; }
    }


    public class ProfilesAutoMapper : Profile
    {

        public ProfilesAutoMapper()
        {
            
            //CreateMap<Customers, CustomersDto>();
            CreateMap(typeof(DataSourceResult), typeof(gridDto<>));
            CreateMap(typeof(genericoOrigen<>), typeof(genericoDestino<>));
            //    CreateMap(Source<>, typeof(Destination<>))
            //        .ForMember(des => des.fuenn;

            //    CreateMap<User, UserViewModel>()
            //.ForMember(dest =>
            //    dest.FName,
            //    opt => opt.MapFrom(src => src.FirstName))
            //.ForMember(dest =>
            //    dest.LName,
            //    opt => opt.MapFrom(src => src.LastName))
            //.ReverseMap();

            //CreateMap<List<Customers>, List<CustomersDto>>();


            //CreateMap<typeof(DataSourceResult), typeof(gridDto<>)>();

            //        .ForMember(dest => dest., opt => opt.MapFrom(src => src.Data))
            //        .ReverseMap();

            // Configurations for other classes in this business 
            // area can be included here as well, like below:

            // CreateMap<Role, RoleDTO>();
            // CreateMap<Permission, PermissionDTO>();
        }


        //Mapper.Initialize(cfg => cfg.CreateMap(typeof(IFoo), typeof(IGenericFoo<>)));


     

    }
}
