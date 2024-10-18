using AutoMapper;
using EmployePortal.Models.Entities;

namespace EmployePortal.Mapping
{
    public class EmployeeMapping:Profile
    {
        public EmployeeMapping()
        {
            CreateMap<EmployeeDTO,Employee>();
        }
    }
}
