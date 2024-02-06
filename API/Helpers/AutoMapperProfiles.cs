using API.Data;
using API.Dtos;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {   
            
            CreateMap<TestEntity, TestEntityDto>();

            CreateMap<Customer, CustomerDto>().ForMember(dest => dest.CustomerProductListDtos, opt => opt.MapFrom(src => src.CustomerProductLists));
            CreateMap<CustomerDto, Customer>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Team, TeamDto>().ForMember(dest => dest.TeamMemberDtos, opt => opt.MapFrom(src => src.TeamMembers));
            CreateMap<TeamDto, Team>();

            CreateMap<TeamMember, TeamMemberDto>();
            CreateMap<TeamMemberDto, TeamMember>();

            CreateMap<TicketType, TicketTypeDto>();
            CreateMap<TicketTypeDto, TicketType>();

            CreateMap<Priority, PriorityDto>();
            CreateMap<PriorityDto, Priority>();

            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();

            CreateMap<CustomerProductList, CustomerProductListDto>();
            CreateMap<CustomerProductListDto, CustomerProductList>();

            CreateMap<Ticket, TicketDto>().ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                                          .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.TeamName))
                                          .ForMember(dest => dest.PriorityName, opt => opt.MapFrom(src => src.Priority.PriorityName))
                                          .ForMember(dest => dest.RecordUserName, opt => opt.MapFrom(src => src.User.NameSurname))
                                          .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.CompanyName))
                                          .ForMember(dest => dest.TicketTypeName, opt => opt.MapFrom(src => src.TicketType.Name));
            CreateMap<TicketDto, Ticket>();

            CreateMap<TicketNode, TicketNodeDto>();
            CreateMap<TicketNodeDto, TicketNode>();

            CreateMap<UploadFile, UploadFileDto>();
        }
    }
}