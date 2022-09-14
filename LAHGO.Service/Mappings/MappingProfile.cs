using AutoMapper;
using LAHGO.Core.Entities;
using LAHGO.Service.ViewModels.CategoryVMs;
using LAHGO.Service.ViewModels.ColorVMs;
using LAHGO.Service.ViewModels.OrderVMs;
using LAHGO.Service.ViewModels.PCSVMs;
using LAHGO.Service.ViewModels.ProductVMs;
using LAHGO.Service.ViewModels.SizeVMs;
using LAHGO.Service.ViewModels.TypeProductVMs;
using LAHGO.Service.ViewModels.TypingVMs;
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
                 .ForMember(des => des.CreatedAt, src => src.MapFrom(s => DateTime.UtcNow.AddHours(4)));
            CreateMap<Color, ColorGetVM>();
            CreateMap<Color, ColorListVM>();

            CreateMap<TypingCreateVM, Typing>()
                 .ForMember(des => des.CreatedAt, src => src.MapFrom(s => DateTime.UtcNow.AddHours(4)));
            CreateMap<Typing, TypingGetVM>();
            CreateMap<Typing, TypingListVM>();

            CreateMap<SizeCreateVM, Size>()
                .ForMember(des => des.CreatedAt, src => src.MapFrom(s => DateTime.UtcNow.AddHours(4))); ;
            CreateMap<Size, SizeGetVM>();
            CreateMap<Size, SizeListVM>();

            CreateMap<TypeProductCreateVM, ProductTyping>()
                .ForMember(des => des.CreatedAt, src => src.MapFrom(s => DateTime.UtcNow.AddHours(4))); ;
            CreateMap<ProductTyping, TypeProductGetVM>();
            CreateMap<ProductTyping, TypeProductListVM>();

            CreateMap<PCSCreateVM, ProductColorSize>();
            CreateMap<ProductColorSize, PCSGetVM>()
                .ForMember(des => des.SizeId, src => src.MapFrom(s => s.SizeId))
                .ForMember(des => des.ColorId, src => src.MapFrom(s => s.ColorId));

            CreateMap<Product, PCSGetVM>()
               .ForMember(des => des.CategoryId, src => src.MapFrom(s => s.CategoryId));

            CreateMap<ProductColorSize, PCSListVM>();

            CreateMap<Product, ProductGetVM>()
                .ForPath(des => des.ProductColorSizes, src => src.MapFrom(s => s.ProductColorSizes))
                .ForMember(des => des.IsBestSeller, src => src.MapFrom(x => x.IsBestSeller))
                .ForMember(des => des.Typings, src => src.MapFrom(x => x.Typings))
                .ForMember(des => des.IsFavorite, src => src.MapFrom(x => x.IsFavorite))
                .ForMember(des => des.IsLinenShop, src => src.MapFrom(x => x.IsLinenShop))
                .ForMember(des => des.IsNewArrival, src => src.MapFrom(x => x.IsNewArrival))
                .ForMember(des => des.IsWashableSilk, src => src.MapFrom(x => x.IsWashableSilk));

            CreateMap<Product, ProductListVM>()
                .ForPath(des => des.ProductColorSizes, src => src.MapFrom(s => s.ProductColorSizes));

            CreateMap<Order, OrderGetVM>();
                

            CreateMap<ProductCreateVM, Product>()
                .ForMember(des => des.Name, src => src.MapFrom(x => x.Name.Trim()))
                .ForMember(des => des.Describtion, src => src.MapFrom(x => x.Describtion.Trim()))
                .ForMember(des => des.Count, src => src.MapFrom(x => x.Count))
                .ForMember(des => des.IsBestSeller, src => src.MapFrom(x => x.IsBestSeller))
                .ForMember(des => des.IsFavorite, src => src.MapFrom(x => x.IsFavorite))
                .ForMember(des => des.IsLinenShop, src => src.MapFrom(x => x.IsLinenShop))
                .ForMember(des => des.IsNewArrival, src => src.MapFrom(x => x.IsNewArrival))
                .ForMember(des => des.IsWashableSilk, src => src.MapFrom(x => x.IsWashableSilk));
        }

    }
}
