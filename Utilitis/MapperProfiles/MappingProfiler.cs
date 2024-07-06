using AutoMapper;
using Caching_with_Redis.Controllers.Products.Request;
using Caching_with_Redis.Models;

namespace Caching_with_Redis.Utalitis.MapperProfiles
{
    public class MappingProfiler:Profile
    {
        public MappingProfiler()
        {
            CreateMap<Product, ProuductRequest>().ReverseMap();   
        }
    }
}
