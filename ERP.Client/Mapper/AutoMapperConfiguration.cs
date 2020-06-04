using AutoMapper;
using ERP.Client.Model;
using ERP.Contracts.Domain;

namespace ERP.Client.Mapper
{
    public static class AutoMapperConfiguration
    {
        public static MapperConfiguration Config { get; private set; }
        public static IMapper Mapper { get; private set; }

        public static void Configure()
        {
            Config = new MapperConfiguration(cfg => {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<DeviceDTO, DeviceModel>().ReverseMap();
                cfg.CreateMap<EmployeeDTO, EmployeeModel>().ReverseMap();
                cfg.CreateMap<DivisionDTO, DivisionModel>().ReverseMap();
                cfg.CreateMap<PlantOrderDTO, PlantOrderModel>().ReverseMap();
                cfg.CreateMap<ElementDTO, ElementModel>().ReverseMap();
                cfg.CreateMap<MaterialRequirementDTO, MaterialRequirementModel>().ReverseMap();
                cfg.CreateMap<DivisionInfoDTO, DivisionInfoModel>().ReverseMap();
                cfg.CreateMap<ProcessTemplateDTO, ProcessTemplateModel>().ReverseMap();
                cfg.CreateMap<PlantOrderProcessDTO, PlantOrderProcessModel>().ReverseMap();
                cfg.CreateMap<DivisionDTO, DivisionModel>().ReverseMap().ForMember(dest => dest.DivisionType, opt => opt.MapFrom(src => src.DivisionType)).ForMember(dest => dest.ProcessTemplates, opt => opt.MapFrom(src => src.ProcessTemplates));
                cfg.CreateMap<EmployeeModel, EmployeeDTO>().ReverseMap().ForMember(dest => dest.Device, opt => opt.MapFrom(src => src.Device));
                cfg.CreateMap<DeviceModel, DeviceDTO>().ReverseMap().ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee));
                cfg.CreateMap<DeviceModel, DeviceDTO>().ReverseMap().ForMember(dest => dest.Division, opt => opt.MapFrom(src => src.Division));
                cfg.CreateMap<FileEntryInfoModel, FileEntryInfoDTO>().ReverseMap();
                cfg.CreateMap<FileEntryModel, FileEntryDTO>().ReverseMap().ForMember(dest => dest.FileInfo, opt => opt.MapFrom(src => src.FileInfo));
                cfg.CreateMap<FolderModel, FolderDTO>().ReverseMap().ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files)).ForMember(dest => dest.SubFolders, opt => opt.MapFrom(src => src.SubFolders));
                cfg.CreateMap<ElementModel, ElementDTO>().ReverseMap().ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children));
                cfg.CreateMap<PlantOrderPositionDTO, ElementDTO>();
                cfg.CreateMap<ElementFilterModel, ElementFilterDTO>().ReverseMap();
                cfg.CreateMap<ProjectNumberDTO, ProjectNumberModel>().ReverseMap();
            });

            Mapper = Config.CreateMapper();
        }

        public static ProfileDTO Map(ElementModel element)
        {
            return new ProfileDTO()
            {
                ProfileId = element.Id,
                Amount = element.Amount,
                Count = element.Count,
                Description = element.Description,
                Length = element.Length,
                ProfileNumber = element.Position,
                Contraction = element.Contraction,
                Surface = element.Surface,
                PlantOrderId = element.PlantOrderId,
                Filename = element.Filename
            };
        }

        public static ElementModel Map(ProfileDTO profile)
        {
            return new ElementModel()
            {
                Id = profile.ProfileId,
                Amount = profile.Amount,
                Count = profile.Count,
                Description = profile.Description,
                Length = profile.Length,
                Position = profile.ProfileNumber,
                Contraction = profile.Contraction,
                Surface = profile.Surface,
                PlantOrderId = profile.PlantOrderId,
                Filename = profile.Filename
            };
        }

    }
}
