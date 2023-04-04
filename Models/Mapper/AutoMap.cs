using AutoMapper;

namespace MvcWebApp.Models.Mapper
{
    public class AutoMap : Profile
    {
        public AutoMap() 
        { 
            CreateMap<Users,UserViewModel>();
        }
    }
}
