using AutoMapper;
using Project_API_SJP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_API_SJP.AutoMapApp
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User -> AuthenticateResponse
            CreateMap<User, AuthenticateResponse>();

            // RegisterRequest -> User
            //CreateMap<RegisterRequest, User>();

            //// UpdateRequest -> User
            //CreateMap<UpdateRequest, User>()
            //    .ForAllMembers(x => x.Condition(
            //        (src, dest, prop) =>
            //        {
            //            // ignore null & empty string properties
            //            if (prop == null) return false;
            //            if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

            //            return true;
            //        }
            //    ));
        }
    }
}
