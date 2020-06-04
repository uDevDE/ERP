
using AutoMapper;
using ERP.Contracts.Domain;
using ERP.Server.Entities.Entity;

namespace ERP.Server.Host.Mapper
{
    public static class AutoMapperConfiguration
    {
        public static MapperConfiguration Config { get; private set; } 
        public static IMapper Mapper { get; private set; }

        public static void Configure()
        {
            Config = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<Device, DeviceDTO>().ReverseMap();
                cfg.CreateMap<DivisionInfo, DivisionInfoDTO>().ReverseMap();
                cfg.CreateMap<Division, DivisionDTO>().ReverseMap().ForMember(dest => dest.DivisionType, opt => opt.MapFrom(src => src.DivisionType));
                cfg.CreateMap<EmployeeDTO, Employee>().ReverseMap().ForMember(dest => dest.Device, opt => opt.Ignore());
                cfg.CreateMap<DeviceDTO, Device>().ReverseMap().ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee));
                cfg.CreateMap<DeviceDTO, Device>().ReverseMap().ForMember(dest => dest.Division, opt => opt.MapFrom(src => src.Division));
                cfg.CreateMap<ProfileDTO, Entities.Entity.Profile>().ReverseMap();
                cfg.CreateMap<ElementFilterDTO, ElementFilter>().ReverseMap();
            });

            Mapper = Config.CreateMapper();
        }
    }
}
