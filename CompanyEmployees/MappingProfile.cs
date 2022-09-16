using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace CompanyEmployees
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            //CreateMap<Company, CompanyDto>()
            //    .ForMember(c => c.FullAddress
            //        , opt => opt.MapFrom(x => string.Join(' ', x.Country, x.Address)));

            CreateMap<Company, CompanyDto>()
                .ForMember(c => c.FullAddress,
                    opt => opt.MapFrom(x => string.Join(' ', x.Country, x.Address)));

            CreateMap<Employee, EmployeeDto>();

            CreateMap<CompanyForCreationDto, Company>();

            CreateMap<EmployeeForCreationDto, Employee>();

            CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();

            CreateMap<CompanyForUpdateDto, Company>();
        }
    }
}
