using AutoMapper;
using LAHGO.Core.Entities;
using LAHGO.Service.ViewModels.CategoryVMs;
using LAHGO.Service.ViewModels.ColorVMs;
using LAHGO.Service.ViewModels.PCSVMs;
using LAHGO.Service.ViewModels.ProductVMs;
using LAHGO.Service.ViewModels.SizeVMs;
using System;
using System.Collections.Generic;
using System.Linq;
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


            CreateMap<PCSCreateVM, ProductColorSize>();
            CreateMap<ProductColorSize, PCSGetVM>();
            CreateMap<ProductColorSize, PCSListVM>();

            CreateMap<Product, ProductGetVM>()
                .ForPath(des => des.ProductColorSizes, src => src.MapFrom(s => s.ProductColorSizes));

            CreateMap<Product, ProductListVM>()
                .ForPath(des => des.ProductColorSizes, src => src.MapFrom(s => s.ProductColorSizes));

            CreateMap<ProductCreateVM, Product>()
                .ForMember(des => des.Name, src => src.MapFrom(x => x.Name.Trim()))
                .ForMember(des => des.Describtion, src => src.MapFrom(x => x.Describtion.Trim()))
                .ForMember(des => des.Count, src => src.MapFrom(x => x.Count));


        }

    }
}
