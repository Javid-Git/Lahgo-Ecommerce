using AutoMapper;
using LAHGO.Core.Entities;
using LAHGO.Service.ViewModels.CategoryVMs;
using LAHGO.Service.ViewModels.ColorVMs;
using LAHGO.Service.ViewModels.SizeVMs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryCreateVM, Category>()
                .ForMember(des=>des.CreatedAt, src=>src.MapFrom(s=>DateTime.UtcNow.AddHours(4)));
            CreateMap<Category, CategoryGetVM > ();
            CreateMap<Category, CategoryListVM>();


            CreateMap<ColorCreateVM, Color>()
                 .ForMember(des => des.CreatedAt, src => src.MapFrom(s => DateTime.UtcNow.AddHours(4))); ;
            CreateMap<Color, ColorGetVM>();
            CreateMap<Color, ColorListVM>();

            CreateMap<SizeCreateVM, Size>()
                .ForMember(des => des.CreatedAt, src => src.MapFrom(s => DateTime.UtcNow.AddHours(4))); ;
            CreateMap<Size, SizeGetVM>();
            CreateMap<Size, SizeListVM>();

        }

    }
}
