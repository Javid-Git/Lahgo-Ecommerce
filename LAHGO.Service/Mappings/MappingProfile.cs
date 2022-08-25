using AutoMapper;
using LAHGO.Core.Entities;
using LAHGO.Service.ViewModels.CategoryVMs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryPostVM, Category>()
                .ForMember(des=>des.CreatedAt, src=>src.MapFrom(s=>DateTime.UtcNow.AddHours(4)));
            CreateMap<CategoryGetVM, Category>();
            CreateMap<CategoryListVM, Category>();
        }

    }
}
