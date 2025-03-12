using AutoMapper;

namespace MGisbert.Appointments.Models.Mappings
{
    public class ModelsProfileClass : Profile
    {
        public ModelsProfileClass()
        {
            CreateMap<Data.Entities.User, User>().ReverseMap();
            CreateMap<Data.Entities.Role, Role>().ReverseMap();
            CreateMap<Data.Entities.Appointment, Appointment>().ReverseMap();


            CreateMap<Request.AppointmentRequest, Data.Entities.Appointment>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ReverseMap();
        }
    }
}
