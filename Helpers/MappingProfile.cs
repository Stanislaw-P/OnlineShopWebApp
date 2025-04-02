﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Onlineshop.Db.Models;
using OnlineShop.Db.Migrations.Identity;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Areas.Admin.Models;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Helpers
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Cart, CartViewModel>().ReverseMap();
			CreateMap<CartItem, CartItemViewModel>().ReverseMap();
			CreateMap<Order, OrderViewModel>().ReverseMap();
			CreateMap<IdentityRole, RoleViewModel>().ReverseMap();

			CreateMap<Product, ProductViewModel>()
				.ForMember(p => p.ImagePaths, opt => opt.MapFrom(p => p.Images.Select(img => img.URL)));
			
			CreateMap<Product, EditProductViewModel>()
				.ForMember(p => p.ImagesPaths, opt => opt.MapFrom(p => p.Images.Select(img => img.URL)));

			CreateMap<AddProductViewModel, Product>()
			.ForMember(dest => dest.Images, opt => opt.MapFrom((addPrView, pr, destMember, context) =>
			{
				// Получаем imagesPaths из контекста
				var imagesPaths = context.Items["ImagesPaths"] as List<string>;
				return imagesPaths?.Select(path => new Image { URL = path }).ToList();
			})).ReverseMap();

			CreateMap<EditProductViewModel, Product>()
				.ForMember(p => p.Images, opt => opt.MapFrom(p => p.ImagesPaths.Select(path => new Image { URL = path }))).ReverseMap();

			CreateMap<UserDeliveryInfoViewModel, UserDeliveryInfo>()
				.ForMember(u => u.UserAccountId, opt => opt.MapFrom((src, dest, destMember, context) =>
				(Guid)context.Items["UserAccountId"])).ReverseMap();

			CreateMap<User, UserViewModel>()
				.ForMember(src => src.Name, opt => opt.MapFrom(dest => dest.UserName))
				.ForMember(src => src.Surname, opt => opt.MapFrom(dest => dest.UserSurname))
				.ForMember(src => src.Phone, opt => opt.MapFrom(dest => dest.PhoneNumber))
				.ReverseMap();
		}
	}
}
