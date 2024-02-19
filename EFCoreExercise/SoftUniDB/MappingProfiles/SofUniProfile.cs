using AutoMapper;
using SoftUni.DTO;
using SoftUni.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftUni.MappingProfiles
{
    public class SofUniProfile: Profile
    {
        public SofUniProfile()
        {
            CreateMap<Employee, PersonDto>();
        }
    }
}
