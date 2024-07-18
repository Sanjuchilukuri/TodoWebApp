using AutoMapper;
using ToDo.Service.DTO;
using ToDo.Service.DTO.ToDoItem;
using ToDo.Service.DTO.User;
using ToDo.Data.Entities;

namespace ToDo.Service
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<ToDoItemDetail, Data.Entities.ToDoItem>().ReverseMap();
            CreateMap<Data.Entities.ToDoItem, ToDoItemSummary>();
            CreateMap<DTO.ToDoItem.ToDoItem, Data.Entities.ToDoItem>();
            CreateMap<User, UserDetails>();
            CreateMap<LoginRegisterModel, User>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName));

        }
    }
}
