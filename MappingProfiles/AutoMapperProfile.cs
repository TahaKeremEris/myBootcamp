using AutoMapper;
using BootcampDay1.MappingProfiles;
using BootcampDay1.DTOs;

namespace BootcampDay1.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // TodoItem <-> TodoDto mapping
            CreateMap<TodoItem, TodoItemDto>().ReverseMap();
        }
    }
}
