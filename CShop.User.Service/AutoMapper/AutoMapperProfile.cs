﻿using AutoMapper;
using CShop.User.Database.Model;
using CShop.User.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.User.Service.AutoMapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserDTO, UserModel>().ReverseMap();
            CreateMap<UpdateUserDTO, UserModel>().ReverseMap();
            CreateMap<ShowUserDTO, UserModel>().ReverseMap();
            CreateMap<LoginDTO, UserModel>();
            CreateMap<SignUpDTO, UserModel>();

        }
    }
}
