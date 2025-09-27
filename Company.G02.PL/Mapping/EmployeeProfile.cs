﻿using AutoMapper;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;

namespace Company.G02.PL.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDto, Employee>().ReverseMap();
            //CreateMap<Employee, CreateEmployeeDto>();
        }



    }
}
 