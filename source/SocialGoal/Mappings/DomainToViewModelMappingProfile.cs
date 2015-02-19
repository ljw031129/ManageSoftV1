using AutoMapper;
using PagedList;
using SocialGoal.Model.Models;
using SocialGoal.Web.Core.AutoMapperConverters;
using SocialGoal.Web.Core.Models;
using SocialGoal.Web.ViewModels;
using SocialGoal.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialGoal.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
           
            Mapper.CreateMap<UserProfile, UserProfileFormModel>();
           
           

            //测试
            Mapper.CreateMap<Equipment, EquipmentViewModel>();
            //分页数据
            Mapper.CreateMap<IPagedList, PagedListData>();           

            Mapper.CreateMap<IPagedList<Equipment>, IPagedList<EquipmentViewModel>>().ConvertUsing<PagedListConverter<Equipment, EquipmentViewModel>>();

            Mapper.CreateMap<TerminalEquipmentCommand, TerminalEquipmentCommandViewModel>();
            //添加映射关系
           

        }
    }
}